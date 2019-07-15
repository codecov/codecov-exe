using System.Collections.Generic;

namespace Codecov.Coverage.SourceCode
{
    internal interface ISourceCode
    {
        string Directory { get; }

        IEnumerable<string> GetAll { get; }

        IEnumerable<string> GetAllButCodecovIgnored { get; }
    }
}
