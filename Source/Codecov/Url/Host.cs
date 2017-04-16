using System;

namespace Codecov.Url
{
    internal class Host : IHost
    {
        private readonly Lazy<string> _getHost;

        public Host(IHostOptions options)
        {
            Options = options;
            _getHost = new Lazy<string>(LoadHost);
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
            var urlEnv = Environment.GetEnvironmentVariable("CODECOV_URL");
            return !string.IsNullOrWhiteSpace(urlEnv) ? urlEnv.Trim().TrimEnd('/') : "https://codecov.io";
        }
    }
}
