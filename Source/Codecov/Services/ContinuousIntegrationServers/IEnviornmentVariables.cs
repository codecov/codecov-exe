using System.Collections.Generic;

namespace Codecov.Services.ContinuousIntegrationServers
{
    internal interface IEnviornmentVariables
    {
        IDictionary<string, string> UserEnvironmentVariables { get; }

        string GetEnvironmentVariable(string name);
    }
}
