using System.Collections.Generic;
using Codecov.Factories;

namespace Codecov.Coverage.Tool
{
    internal interface ICoverage
    {
        IEnumerable<ReportFile> CoverageReports { get; }
    }
}
