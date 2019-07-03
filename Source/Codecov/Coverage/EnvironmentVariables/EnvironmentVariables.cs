using System;
using System.Collections.Generic;
using Codecov.Services.ContinuousIntegrationServers;

namespace Codecov.Coverage.EnvironmentVariables
{
    internal class EnvironmentVariables : IEnvironmentVariables
    {
        private readonly Lazy<IDictionary<string, string>> _getEnvironmentVariables;

        public EnvironmentVariables(IEnvironmentVariablesOptions options, IContinuousIntegrationServer continuousIntegrationServer)
        {
            Options = options;
            ContinuousIntegrationServer = continuousIntegrationServer;
            _getEnvironmentVariables = new Lazy<IDictionary<string, string>>(LoadEnvironmentVariables);
        }

        public IDictionary<string, string> GetEnvironmentVariables => _getEnvironmentVariables.Value;

        private IContinuousIntegrationServer ContinuousIntegrationServer { get; }

        private IEnvironmentVariablesOptions Options { get; }

        private IDictionary<string, string> LoadEnvironmentVariables()
        {
            var environmentVariables = new Dictionary<string, string>(ContinuousIntegrationServer.GetEnvironmentVariables);

            const string codecovName = "CODECOV_ENV";
            var codecovValue = Environment.GetEnvironmentVariable(codecovName);
            if (!string.IsNullOrWhiteSpace(codecovValue) && !environmentVariables.ContainsKey(codecovName))
            {
                environmentVariables[codecovName] = codecovValue;
            }

            var environmentVariableNames = Options.Envs;
            foreach (var environmentVariableName in environmentVariableNames)
            {
                if (environmentVariables.ContainsKey(environmentVariableName))
                {
                    continue;
                }

                var value = Environment.GetEnvironmentVariable(environmentVariableName);
                if (string.IsNullOrWhiteSpace(value))
                {
                    continue;
                }

                environmentVariables[environmentVariableName] = Environment.GetEnvironmentVariable(environmentVariableName);
            }

            return environmentVariables;
        }
    }
}
