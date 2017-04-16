using System.Collections.Generic;
using Codecov.Factories;

namespace Codecov.Coverage.Tool
{
    internal interface ICoverage : IDetect
    {
        IEnumerable<ReportFile> CoverageReports { get; }
    }
}