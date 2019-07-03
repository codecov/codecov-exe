using System;
using System.Collections.Generic;
using Codecov.Utilities;

namespace Codecov.Services.ContinuousIntegrationServers
{
    internal class ContinuousIntegrationServer : IContinuousIntegrationServer
    {
        private readonly Lazy<string> _build = new Lazy<string>(() => EnvironmentVariable.GetEnvironmentVariable("CI_BUILD_ID"));
        private readonly Lazy<string> _buildUrl = new Lazy<string>(() => EnvironmentVariable.GetEnvironmentVariable("CI_BUILD_URL"));
        private readonly Lazy<string> _job = new Lazy<string>(() => EnvironmentVariable.GetEnvironmentVariable("CI_JOB_ID"));

        public virtual string Branch => string.Empty;

        public virtual string Build => _build.Value;

        public virtual string BuildUrl => _buildUrl.Value;

        public virtual string Commit => string.Empty;

        public virtual bool Detecter => false;

        public virtual IDictionary<string, string> GetEnvironmentVariables => new Dictionary<string, string>();

        public virtual string Job => _job.Value;

        public virtual string Pr => string.Empty;

        public virtual string Service => string.Empty;

        public virtual string Slug => string.Empty;

        public virtual string Tag => string.Empty;

        protected void AddEnvironmentVariable(string name)
        {
            if (GetEnvironmentVariables.ContainsKey(name))
            {
                return;
            }

            var value = Environment.GetEnvironmentVariable(name);
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            GetEnvironmentVariables[name] = value;
        }
    }
}
