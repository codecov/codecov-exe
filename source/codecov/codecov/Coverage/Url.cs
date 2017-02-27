using System;

namespace codecov.Coverage
{
    public static class Url
    {
        public static string GetUrl(string baseUrlCommandLine, string baseUrlYml, string query)
        {
            var baseUrl = GetBaseUrl(baseUrlCommandLine, baseUrlYml);
            return $"{baseUrl}/upload/v4?{query}";
        }

        private static string GetBaseUrl(string baseUrlCommandLine, string baseUrlYml)
        {
            // Command line
            if (!string.IsNullOrWhiteSpace(baseUrlCommandLine))
            {
                return baseUrlCommandLine.Trim().TrimEnd('/');
            }

            // Enviornment variable
            var urlEnv = Environment.GetEnvironmentVariable("CODECOV_URL");
            if (!string.IsNullOrWhiteSpace(urlEnv))
            {
                return urlEnv.Trim().TrimEnd('/');
            }

            // yaml file
            if (!string.IsNullOrWhiteSpace(baseUrlYml))
            {
                return baseUrlYml.Trim();
            }

            return "https://codecov.io";
        }
    }
}