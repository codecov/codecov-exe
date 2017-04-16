using System.Collections.Generic;

namespace Codecov.Coverage.Tool
{
    internal interface ICoverageOptions
    {
        IEnumerable<string> Files { get; }
    }
}
