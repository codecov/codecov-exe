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

        private IEnumerable<ReportFile> LoadReportFile()
        {
            var report = CoverageOptions.Files.Where(x => !string.IsNullOrWhiteSpace(x) && File.Exists(x)).Select(x => new ReportFile(x, File.ReadAllText(x))).ToArray();
            if (!report.Any())
            {
                throw new Exception("No Report detected.");
            }

            return report;
        }
    }
}