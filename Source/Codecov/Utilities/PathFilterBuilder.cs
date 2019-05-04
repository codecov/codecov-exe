using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Codecov.Utilities
{
    internal class PathFilterBuilder
    {
        private readonly List<IPathFilter> _filters = new List<IPathFilter>();

        public PathFilterBuilder HasExtension(string extension)
        {
            _filters.Add(new FileExtensionPathFilter(extension));
            return this;
        }

        public PathFilterBuilder FileNameMatches(string pattern)
        {
            _filters.Add(new FileNameFilter(pattern));
            return this;
        }

        public PathFilterBuilder PathContains(string path)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                path = path.Replace(@"\", "/");
            }

            _filters.Add(new PathFilter(path));
            return this;
        }

        public IPathFilter Build()
        {
            return new CompositePathFilter(_filters);
        }

        private class PathFilter : IPathFilter
        {
            private readonly string _subPath;

            public PathFilter(string subPath)
            {
                _subPath = subPath;
            }

            public bool Matches(string path)
            {
                return path.Contains(_subPath, StringComparison.OrdinalIgnoreCase);
            }
        }

        private class FileExtensionPathFilter : IPathFilter
        {
            private readonly string _extension;

            public FileExtensionPathFilter(string extension)
            {
                _extension = extension;
            }

            public bool Matches(string path)
            {
                var fileExtension = Path.GetExtension(path);

                return _extension.Equals(fileExtension, StringComparison.OrdinalIgnoreCase);
            }
        }

        private class FileNameFilter : IPathFilter
        {
            private readonly Regex _regex;

            public FileNameFilter(string regex)
            {
                _regex = new Regex(regex, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            }

            public bool Matches(string path)
            {
                var filename = Path.GetFileName(path);
                return _regex.IsMatch(filename);
            }
        }
        
        private class CompositePathFilter : IPathFilter
        {
            private readonly List<IPathFilter> _filters;

            public CompositePathFilter(List<IPathFilter> filters)
            {
                _filters = filters;
            }


            public bool Matches(string path)
            {
                return _filters.Any(f => f.Matches(path));
            }
        }
    }
}
