using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace Codecov.Utilities
{
    internal class PathFilterBuilder
    {
        private readonly List<IPathFilter> _filters = new List<IPathFilter>();

        public IPathFilter Build()
        {
            return new CompositePathFilter(_filters);
        }

        public PathFilterBuilder FileHasExtension(string extension)
        {
            if (string.IsNullOrEmpty(extension))
            {
                throw new ArgumentException("Extension cannot be null or empty", nameof(extension));
            }

            _filters.Add(new FileExtensionPathFilter(extension));
            return this;
        }

        public PathFilterBuilder PathContains(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("Path cannot be null or empty", nameof(path));
            }

            path = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? path.Replace(@"/", @"\")
                : path.Replace(@"\", "/");

            _filters.Add(new PathFilter(path));
            return this;
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
    }
}
