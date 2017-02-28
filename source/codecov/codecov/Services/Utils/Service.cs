using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using codecov.Program;

namespace codecov.Services.Utils
{
    public abstract class Service : IService
    {
        public abstract bool Detect { get; }

        protected IDictionary<string, string> QueryParameters { get; } =
            new Dictionary<string, string>
            {
                {"branch", string.Empty},
                {"commit", string.Empty},
                {"build", string.Empty},
                {"build_url", string.Empty},
                {"tag", string.Empty},
                {"slug", string.Empty},
                {"yaml", string.Empty},
                {"service", string.Empty},
                {"flags", string.Empty},
                {"pr", string.Empty},
                {"job", string.Empty},
                {"token", string.Empty},
                {"package", string.Empty},
                {"name", string.Empty}
            };

        public string CreateQuery(Options options, ICodecovYml codeCovYml)
        {
            OverrideIfNotEmptyOrNull("branch", options.Branch);
            OverrideIfNotEmptyOrNull("commit", options.Sha);
            OverrideIfNotEmptyOrNull("build", options.Build);
            OverrideIfNotEmptyOrNull("tag", options.Tag);
            OverrideIfNotEmptyOrNull("pr", options.Pr);
            OverrideIfNotEmptyOrNull("name", options.Name);
            SetFlags(options.Flag);
            SetSlug(options.Slug, codeCovYml.Slug);
            SetToken(options.Token, codeCovYml.Token);
            SetPackage();

            return string.Join("&", QueryParameters.Select(x => $"{x.Key}={x.Value ?? string.Empty}"));
        }

        private void OverrideIfNotEmptyOrNull(string key, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                QueryParameters[key] = value.Trim();
            }
        }

        private void SetFlags(string[] flag)
        {
            if (flag == null)
            {
                return;
            }

            var flags = string.Join(",", flag.Select(x => x.Trim()));
            if (!string.IsNullOrWhiteSpace(flags))
            {
                QueryParameters["flags"] = flags;
            }
        }

        private void SetPackage()
        {
            QueryParameters["package"] = $"exe-{Assembly.GetExecutingAssembly().GetName().Version}";
        }

        private void SetSlug(string slugCommandLine, string slugYaml)
        {
            if (!string.IsNullOrWhiteSpace(slugCommandLine))
            {
                QueryParameters["slug"] = System.Net.WebUtility.UrlEncode(slugCommandLine.Trim());
            }

            var slugEnv = Environment.GetEnvironmentVariable("CODECOV_SLUG");
            if (!string.IsNullOrWhiteSpace(slugEnv))
            {
                QueryParameters["slug"] = slugEnv.Trim();
            }

            if (!string.IsNullOrWhiteSpace(slugYaml))
            {
                QueryParameters["slug"] = slugYaml.Trim();
            }
        }

        private void SetToken(string tokenCommandLine, string tokenYaml)
        {
            Guid tokenGuid;
            if (Guid.TryParse(tokenCommandLine, out tokenGuid))
            {
                QueryParameters["token"] = tokenCommandLine.Trim();
            }

            var tokenEnv = Environment.GetEnvironmentVariable("CODECOV_TOKEN");
            if (!string.IsNullOrWhiteSpace(tokenEnv))
            {
                QueryParameters["token"] = tokenEnv.Trim();
            }

            if (!string.IsNullOrWhiteSpace(tokenYaml))
            {
                QueryParameters["token"] = tokenYaml.Trim();
            }
        }
    }
}