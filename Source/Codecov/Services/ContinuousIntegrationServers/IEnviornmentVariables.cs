using System.Collections.Generic;

namespace Codecov.Services.ContinuousIntegrationServers
{
    internal interface IEnviornmentVariables
    {
        IDictionary<string, string> GetEnviornmentVariables { get; }
    }
}
