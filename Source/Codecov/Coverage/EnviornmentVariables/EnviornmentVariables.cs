using System;
using System.Collections.Generic;
using Codecov.Services.ContinuousIntegrationServers;
using Codecov.Utilities;

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

        public IDictionary<string, string> UserEnvironmentVariables => _getEnviornmentVariables.Value;

        private IContinuousIntegrationServer ContinuousIntegrationServer { get; }

        private IEnviornmentVariablesOptions Options { get; }

        public string GetEnvironmentVariable(string name)
        {
            if (UserEnvironmentVariables.ContainsKey(name))
            {
                return UserEnvironmentVariables[name];
            }

            var value = Environment.GetEnvironmentVariable(name);
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            return value; // We don't set the dictionary since it is used for user customized values
        }

        private IDictionary<string, string> LoadEnviornmentVariables()
        {
            var enviornmentVariables = new Dictionary<string, string>(ContinuousIntegrationServer.UserEnvironmentVariables);

            const string codecovName = "CODECOV_ENV";
            var codecovValue = EnviornmentVariable.GetEnviornmentVariable(codecovName);
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

                var value = EnviornmentVariable.GetEnviornmentVariable(enviornmentVariableName);
                if (string.IsNullOrWhiteSpace(value))
                {
                    continue;
                }

                enviornmentVariables[enviornmentVariableName] = EnviornmentVariable.GetEnviornmentVariable(enviornmentVariableName);
            }

            return enviornmentVariables;
        }
    }
}
