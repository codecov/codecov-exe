using System.Collections.Generic;

namespace Codecov.Coverage.SourceCode
{
    internal interface ISourceCode
    {
        IEnumerable<string> GetAll { get; }

        IEnumerable<string> GetAllButCodecovIgnored { get; }

        string Directory { get; }
    }
}