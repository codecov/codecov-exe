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

        public bool Detecter => false;

        private ICoverageOptions CoverageOptions { get; }

        private static bool VerifyReportFileAndExpandGlobPatterns(string path, out IEnumerable<string> expandedPath)
        {
            var expanded = new HashSet<string> { path };
            expandedPath = expanded;
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
                var matches = Glob.Files(Environment.CurrentDirectory, path, GlobOptions.Compiled | GlobOptions.CaseInsensitive)?.ToList();
                if (matches?.Any() != true)
                {
                    return false;
                }

                expanded.Clear();
                matches.ForEach(_ => expanded.Add(_));

                return true;
            }
            else if (!File.Exists(path))
            {
                Log.Warning($"The file {path} does not exist.");
                return false;
            }

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
