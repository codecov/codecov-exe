using System.Collections.Generic;
using Codecov.Coverage.Tool;

namespace Codecov.MSBuild
{
    internal class Coverage : ICoverage
    {
        public Coverage(IEnumerable<ReportFile> coverageReports)
        {
            CoverageReports = coverageReports;
        }

        public IEnumerable<ReportFile> CoverageReports { get; }
    }
}
