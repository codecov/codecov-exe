using System;
using System.Collections.Generic;
using Codecov.Services.ContinuousIntegrationServers;

namespace Codecov.Coverage.EnviornmentVariables
{
    internal class EnviornmentVariables : IEnviornmentVariables
    {
        private readonly Lazy<IDictionary<string, string>> _getEnviornmentVariables;

        public EnviornmentVariables(IEnviornmentVariablesOptions options, IContinuousIntegrationServer continuousIntegrationServer)
        {
            Options = options;
            ContinuousIntegrationServer = continuousIntegrationServer;
            _getEnviornmentVariables = new Lazy<IDictionary<string, string>>(LoadEnviornmentVariables);
        }

        public IDictionary<string, string> GetEnviornmentVariables => _getEnviornmentVariables.Value;

        private IContinuousIntegrationServer ContinuousIntegrationServer { get; }

        private IEnviornmentVariablesOptions Options { get; }

        private IDictionary<string, string> LoadEnviornmentVariables()
        {
            var enviornmentVariables = new Dictionary<string, string>(ContinuousIntegrationServer.GetEnviornmentVariables);

            const string codecovName = "CODECOV_ENV";
            var codecovValue = Environment.GetEnvironmentVariable(codecovName);
            if (!string.IsNullOrWhiteSpace(codecovValue) && !enviornmentVariables.ContainsKey(codecovName))
            {
                enviornmentVariables[codecovName] = codecovValue;
            }

            var enviornmentVariableNames = Options.Envs;
            foreach (var enviornmentVariableName in enviornmentVariableNames)
            {
                if (enviornmentVariables.ContainsKey(enviornmentVariableName))
                {
                    continue;
                }

                var value = Environment.GetEnvironmentVariable(enviornmentVariableName);
                if (string.IsNullOrWhiteSpace(value))
                {
                    continue;
                }

                enviornmentVariables[enviornmentVariableName] = Environment.GetEnvironmentVariable(enviornmentVariableName);
            }

            return enviornmentVariables;
        }
    }
}
