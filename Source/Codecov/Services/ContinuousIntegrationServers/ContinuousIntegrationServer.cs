using System;
using System.Collections.Generic;
using System.Linq;

namespace Codecov.Services.ContinuousIntegrationServers
{
    internal class ContinuousIntegrationServer : IContinuousIntegrationServer
    {
        private readonly Lazy<string> _build;
        private readonly Lazy<string> _buildUrl;
        private readonly IEnviornmentVariables _environmentVariables;
        private readonly Lazy<string> _job;

        public ContinuousIntegrationServer(IEnviornmentVariables environmentVariables)
        {
            _environmentVariables = environmentVariables;
            _build = new Lazy<string>(() => GetEnvironmentVariable("CI_BUILD_ID"));
            _buildUrl = new Lazy<string>(() => GetEnvironmentVariable("CI_BUILD_URL"));
            _job = new Lazy<string>(() => GetEnvironmentVariable("CI_JOB_ID"));
        }

        public virtual string Branch => string.Empty;

        public virtual string Build => _build.Value;

        public virtual string BuildUrl => _buildUrl.Value;

        public virtual string Commit => string.Empty;

        public virtual bool Detecter => false;

        public virtual string Job => _job.Value;
        public virtual string Pr => string.Empty;
        public virtual string Project => string.Empty;
        public virtual string ServerUri => string.Empty;
        public virtual string Service => string.Empty;
        public virtual string Slug => string.Empty;
        public virtual string Tag => string.Empty;
        public virtual IDictionary<string, string> UserEnvironmentVariables => new Dictionary<string, string>();

        public string GetEnvironmentVariable(string name)
        {
            var env = _environmentVariables.GetEnvironmentVariable(name);
            if (string.IsNullOrEmpty(env))
            {
                return string.Empty;
            }

            return env;
        }

        protected void AddEnviornmentVariable(string name)
        {
            if (UserEnvironmentVariables.ContainsKey(name))
            {
                return;
            }

            var value = GetEnvironmentVariable(name);
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            UserEnvironmentVariables[name] = value;
        }

        protected bool CheckEnvironmentVariables(params string[] environmentVariables)
        {
            var foundVariables = new List<string>();

            foreach (var variable in environmentVariables)
            {
                var ci = GetEnvironmentVariable(variable);

                if (string.IsNullOrWhiteSpace(ci) || !ci.Equals("True", StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }

                foundVariables.Add(ci);
            }

            var firstValue = foundVariables.FirstOrDefault();
            return !string.IsNullOrEmpty(firstValue) && foundVariables.Skip(1).All(v => v.Equals(firstValue, StringComparison.Ordinal));
        }

        protected string GetFirstExistingEnvironmentVariable(params string[] names)
        {
            foreach (var name in names)
            {
                var env = GetEnvironmentVariable(name);

                if (!string.IsNullOrWhiteSpace(env))
                {
                    return env;
                }
            }

            return string.Empty;
        }
    }
}
