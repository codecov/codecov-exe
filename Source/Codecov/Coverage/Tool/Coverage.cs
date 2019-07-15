using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GlobExpressions;

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
            HashSet<string> expanded = new HashSet<string> { path };
            expandedPath = expanded;
            if (string.IsNullOrWhiteSpace(path))
            {
                Logger.Log.Warning("Invalid report path.");
                return false;
            }

            if (path.Contains('*')
                || path.Contains('?')
                || path.Contains('!')
                || path.Contains(','))
            {
                // Logger.Log.Information($"Using wildcard path {path}");
                List<string> matches = Glob.Files(Environment.CurrentDirectory, path, GlobOptions.Compiled | GlobOptions.CaseInsensitive)?.ToList();
                if (matches?.Any() != true)
                {
                    // Logger.Log.Warning($"There are no files that match the wildcard {path}.");
                    return false;
                }

                expanded.Clear();
                matches.ForEach(_ =>
                {
                    // Logger.Log.Information($"Adding file {_} that matches wildcard path {path}");
                    expanded.Add(_);
                });

                return true;
            }
            else if (!File.Exists(path))
            {
                Logger.Log.Warning($"The file {path} does not exist.");
                return false;
            }

            return true;
        }

        private IEnumerable<ReportFile> LoadReportFile()
        {
            ReportFile[] report = CoverageOptions.Files?
                .SelectMany(x =>
                {
                    if (VerifyReportFileAndExpandGlobPatterns(x, out IEnumerable<string> expanded))
                    {
                        return expanded;
                    }

                    return Enumerable.Empty<string>();
                })?
                .Select(x => new ReportFile(x, File.ReadAllText(x)))?.ToArray();
            if (report?.Any() != true)
            {
                throw new Exception("No Report detected.");
            }

            return report;
        }
    }
}
