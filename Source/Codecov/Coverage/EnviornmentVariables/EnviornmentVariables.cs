using System;
using System.Collections.Generic;
using Codecov.Services.ContinuousIntegrationServers;
using Codecov.Utilities;

namespace Codecov.Coverage.EnviornmentVariables
{
    internal class EnviornmentVariables : IEnviornmentVariables
    {
        public EnviornmentVariables(IEnviornmentVariablesOptions options)
        {
            Options = options;
        }

        public IDictionary<string, string> UserEnvironmentVariables { get; private set; } = new Dictionary<string, string>();

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

        internal void LoadEnviornmentVariables(IContinuousIntegrationServer continuousIntegrationServer)
        {
            var enviornmentVariables = new Dictionary<string, string>(continuousIntegrationServer.UserEnvironmentVariables);

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

            UserEnvironmentVariables = enviornmentVariables;
        }
    }
}
