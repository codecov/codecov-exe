using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

        private static bool VerifyReportFile(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                Logger.Log.Warning("Invalid report path.");
                return false;
            }

            if (!File.Exists(path))
            {
                Logger.Log.Warning($"The file {path} does not exist.");
                return false;
            }

            return true;
        }

        private IEnumerable<ReportFile> LoadReportFile()
        {
            var report = CoverageOptions.Files.Where(x => VerifyReportFile(x)).Select(x => new ReportFile(x, File.ReadAllText(x))).ToArray();
            if (!report.Any())
            {
                throw new Exception("No Report detected.");
            }

            return report;
        }
    }
}
