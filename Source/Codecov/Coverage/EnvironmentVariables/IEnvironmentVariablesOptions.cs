using System.Collections.Generic;

namespace Codecov.Coverage.EnvironmentVariables
{
    internal interface IEnvironmentVariablesOptions
    {
        IEnumerable<string> Envs { get; }
    }
}
