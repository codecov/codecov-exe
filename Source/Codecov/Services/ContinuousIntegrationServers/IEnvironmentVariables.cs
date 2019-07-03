using System.Collections.Generic;

namespace Codecov.Services.ContinuousIntegrationServers
{
    internal interface IEnvironmentVariables
    {
        IDictionary<string, string> GetEnvironmentVariables { get; }
    }
}
