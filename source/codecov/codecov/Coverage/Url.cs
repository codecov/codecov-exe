using System;
using codecov.Program;

namespace codecov.Coverage
{
    public class Url : IUrl
    {
        public Url(Options options, string query)
        {
            BaseUrl = CreateBaseUrl(options.Url);
            Query = query;
        }

        public Uri FullUrl => new Uri($"{BaseUrl}/upload/v4?{Query}");

        private string BaseUrl { get; }

        private string Query { get; }

        private static string CreateBaseUrl(string baseUrl)
        {
            // Command line
            if (!string.IsNullOrWhiteSpace(baseUrl))
            {
                return baseUrl.Trim().TrimEnd('/');
            }

            // Enviornment variable
            var urlEnv = Environment.GetEnvironmentVariable("CODECOV_URL");
            if (!string.IsNullOrWhiteSpace(urlEnv))
            {
                return urlEnv.Trim().TrimEnd('/');
            }

            return "https://codecov.io";
        }
    }
}