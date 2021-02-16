using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Codecov.Exceptions;
using GlobExpressions;
using Serilog;

namespace Codecov.Coverage.Tool
{
    internal class Coverage : ICoverage
    {
        private readonly Lazy<IEnumerable<ReportFile>> _coverageReports;

        public Coverage(ICoverageOptions coverageOptions)
        {
            CoverageOptions = coverageOptions;
            _coverageReports = new Lazy<IEnumerable<ReportFile>>(LoadReportFile);
        }

        public IEnumerable<ReportFile> CoverageReports => _coverageReports.Value;

        private ICoverageOptions CoverageOptions { get; }

        private static bool VerifyReportFileAndExpandGlobPatterns(string path, out IEnumerable<string> expanded)
        {
            expanded = null;

            if (string.IsNullOrWhiteSpace(path))
            {
                Log.Warning("Invalid report path.");
                return false;
            }

            if (path.Contains('*')
                || path.Contains('?')
                || path.Contains('!')
                || path.Contains(','))
            {
                string directory;
                string pattern;
                if (Path.IsPathRooted(path))
                {
                    directory = Path.GetPathRoot(path);
                    pattern = path.Substring(directory.Length);
                }
                else
                {
                    directory = Environment.CurrentDirectory;
                    pattern = path;
                }

                expanded = Glob.Files(directory, pattern, GlobOptions.Compiled | GlobOptions.CaseInsensitive)
                    .Select(x => Path.Combine(directory, x))
                    .Distinct();

                return true;
            }
            else if (!File.Exists(path))
            {
                Log.Warning("The file {path} does not exist.", path);
                return false;
            }

            expanded = new[] { path };
            return true;
        }

        private IEnumerable<ReportFile> LoadReportFile()
        {
            var report = CoverageOptions.Files?
                .SelectMany(x =>
                    VerifyReportFileAndExpandGlobPatterns(x, out var expanded)
                        ? expanded
                        : Enumerable.Empty<string>())
                .Select(x => new ReportFile(x, File.ReadAllText(x))).ToArray();
            if (report?.Any() != true)
            {
                throw new CoverageException("No Report detected.");
            }

            return report;
        }
    }
}
