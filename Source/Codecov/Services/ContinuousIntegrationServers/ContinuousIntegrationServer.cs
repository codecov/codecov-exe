using System;
using System.Collections.Generic;
using System.Linq;
using Codecov.Utilities;

namespace Codecov.Services.ContinuousIntegrationServers
{
    internal class ContinuousIntegrationServer : IContinuousIntegrationServer
    {
        private readonly Lazy<string> _build = new Lazy<string>(() => EnviornmentVariable.GetEnviornmentVariable("CI_BUILD_ID"));
        private readonly Lazy<string> _buildUrl = new Lazy<string>(() => EnviornmentVariable.GetEnviornmentVariable("CI_BUILD_URL"));
        private readonly Lazy<string> _job = new Lazy<string>(() => EnviornmentVariable.GetEnviornmentVariable("CI_JOB_ID"));

        public virtual string Branch => string.Empty;

        public virtual string Build => _build.Value;

        public virtual string BuildUrl => _buildUrl.Value;

        public virtual string Commit => string.Empty;

        public virtual bool Detecter => false;

        public virtual IDictionary<string, string> GetEnviornmentVariables => new Dictionary<string, string>();

        public virtual string Job => _job.Value;

        public virtual string Pr => string.Empty;

        public virtual string Project => string.Empty;

        public virtual string ServerUri => string.Empty;

        public virtual string Service => string.Empty;

        public virtual string Slug => string.Empty;

        public virtual string Tag => string.Empty;

        protected static bool CheckEnvironmentVariables(params string[] environmentVariables)
        {
            var foundVariables = new List<string>();

            foreach (var variable in environmentVariables)
            {
                var ci = EnviornmentVariable.GetEnviornmentVariable(variable);

                if (string.IsNullOrWhiteSpace(ci) || !ci.Equals("True", StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }

                foundVariables.Add(ci);
            }

            var firstValue = foundVariables.FirstOrDefault();
            return !string.IsNullOrEmpty(firstValue) && foundVariables.Skip(1).All(v => v.Equals(firstValue, StringComparison.Ordinal));
        }

        public string GetEnvironmentVariable(string name)
        {
            if (GetEnviornmentVariables.ContainsKey(name))
            {
                return GetEnviornmentVariables[name];
            }

            var value = Environment.GetEnvironmentVariable(name);
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            return value;
        }

        protected void AddEnviornmentVariable(string name)
        {
            if (GetEnviornmentVariables.ContainsKey(name))
            {
                return;
            }

            var value = Environment.GetEnvironmentVariable(name);
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            GetEnviornmentVariables[name] = value;
        }
    }
}
