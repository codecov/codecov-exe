using System.Collections.Generic;

namespace Codecov.Coverage.EnviornmentVariables
{
    internal interface IEnviornmentVariablesOptions
    {
        IEnumerable<string> Envs { get; }
    }
}
