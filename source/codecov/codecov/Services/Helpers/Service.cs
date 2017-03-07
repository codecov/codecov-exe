using System;
using System.Collections.Generic;
using System.Linq;
using codecov.Program;

namespace codecov.Services.Helpers
{
    public abstract class Service : IService
    {
        protected Service(Options options)
        {
            Options = options;
        }

        public abstract bool Detect { get; }

        public string Query
        {
            get
            {
                OverrideIfNotEmptyOrNull("branch", Options.Branch);
                OverrideIfNotEmptyOrNull("commit", Options.Sha);
                OverrideIfNotEmptyOrNull("build", Options.Build);
                OverrideIfNotEmptyOrNull("tag", Options.Tag);
                OverrideIfNotEmptyOrNull("pr", Options.Pr);
                OverrideIfNotEmptyOrNull("name", Options.Name);
                SetFlags();
                SetSlug();
                SetToken();
                SetPackage();

                return string.Join("&", QueryParameters.Select(x => $"{x.Key}={x.Value ?? string.Empty}"));
            }
        }

        public string RepoRoot => !Utils.RemoveAllWhiteSpaceFromString(Options.Root).Equals(".") ? Utils.NormalizePath(Options.Root) : Utils.RunCmd("git", "rev-parse --show-toplevel");

        public string SourceCodeFiles
        {
            get
            {
                Log.Message($"Project root: {RepoRoot}");
                return Utils.RunCmd("git", $"-C {RepoRoot} ls-tree --full-tree -r HEAD --name-only");
            }
        }

        protected Options Options { get; }

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

        private void OverrideIfNotEmptyOrNull(string key, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                QueryParameters[key] = Utils.RemoveAllWhiteSpaceFromString(value);
            }
        }

        private void SetFlags()
        {
            if (Options.Flag == null)
            {
                return;
            }

            var flags = string.Join(",", Options.Flag.Select(x => x.Trim()));
            if (!string.IsNullOrWhiteSpace(flags))
            {
                QueryParameters["flags"] = Utils.RemoveAllWhiteSpaceFromString(flags);
            }
        }

        private void SetPackage()
        {
            QueryParameters["package"] = Utils.Version;
        }

        private void SetSlug()
        {
            if (!string.IsNullOrWhiteSpace(Options.Slug))
            {
                QueryParameters["slug"] = System.Net.WebUtility.UrlEncode(Options.Slug.Trim());
            }

            var slugEnv = Environment.GetEnvironmentVariable("CODECOV_SLUG");
            if (!string.IsNullOrWhiteSpace(slugEnv))
            {
                Log.Message("-> Slug set from env.");
                QueryParameters["slug"] = System.Net.WebUtility.UrlEncode(slugEnv.Trim());
            }
        }

        private void SetToken()
        {
            Guid tokenGuid;
            if (Guid.TryParse(Options.Token, out tokenGuid))
            {
                QueryParameters["token"] = Utils.RemoveAllWhiteSpaceFromString(Options.Token);
            }

            var tokenEnv = Environment.GetEnvironmentVariable("CODECOV_TOKEN");
            if (!string.IsNullOrWhiteSpace(tokenEnv))
            {
                QueryParameters["token"] = Utils.RemoveAllWhiteSpaceFromString(tokenEnv);
            }
        }
    }
}