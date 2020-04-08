using System;
using Codecov.Services.ContinuousIntegrationServers;

namespace Codecov.Url
{
    internal class Host : IHost
    {
        private readonly Lazy<string> _getHost;
        private readonly IEnviornmentVariables _environmentVariables;

        public Host(IHostOptions options, IEnviornmentVariables environmentVariables)
        {
            Options = options;
            _getHost = new Lazy<string>(LoadHost);
            _environmentVariables = environmentVariables;
        }

        public string GetHost => _getHost.Value;

        private IHostOptions Options { get; }

        private string LoadHost()
        {
            // Try to get from commandline
            if (!string.IsNullOrWhiteSpace(Options.Url))
            {
                return Options.Url.Trim().TrimEnd('/');
            }

            // Try to get it from enviornment variable else just use default url.
            var urlEnv = _environmentVariables.GetEnvironmentVariable("CODECOV_URL");
            return !string.IsNullOrWhiteSpace(urlEnv) ? urlEnv.Trim().TrimEnd('/') : "https://codecov.io";
        }
    }
}
