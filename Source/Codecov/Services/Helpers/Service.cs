using System;
using System.Collections.Generic;
using System.Linq;
using Codecov.Program;
using System.IO;
using System.Text.RegularExpressions;

namespace Codecov.Services.Helpers
{
    public class Service : IService
    {
        public Service(Options options)
        {
            Options = options;
        }

        public virtual bool Detect => true;

        public virtual void SetQueryParams()
        {
            
        }

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

        public string RepoRoot
        {
            get
            {
                var repoRoot = ".";
                if (!Utils.RemoveAllWhiteSpaceFromString(Options.Root).Equals(string.Empty))
                {
                    repoRoot = Utils.NormalizePath(Options.Root);
                }
                else
                {
                    var git = new Git(Options);
                    if (git.Detect)
                    {
                        repoRoot = Utils.RunCmd("git", "rev-parse --show-toplevel");
                    }
                }

                return repoRoot;
            }
        }

        public string SourceCodeFiles
        {
            get
            {
                var repoRoot = RepoRoot;
                Log.Message($"Project root: {repoRoot}");
                if (Options.Disable.Contains("network", StringComparer.OrdinalIgnoreCase))
                {
                    Log.Verbose("Skipping obtaining a list of source files.");
                    return string.Empty;
                }

                const string network = "<<<<<< network";

                var git = new Git(Options);
                if (git.Detect)
                {
                    Log.Verbose("Obtaining a list of source files using git.");
                    var file = Utils.RunCmd("git", $"-C {repoRoot} ls-tree --full-tree -r HEAD --name-only");
                    return file.Trim() + "\n" + network;
                }
                else
                {
                    Log.Verbose("Obtaining a list of source files manually.");
                    // Read file system.
                    var files = Directory.EnumerateFiles(repoRoot, "*.*", SearchOption.AllDirectories)
                        .Where(file => !file.ToUpper().Contains(@"\.GIT\"))
                        .Where(file => !file.ToUpper().Contains(@"\BIN\DEBUG\"))
                        .Where(file => !file.ToUpper().Contains(@"\BIN\RELEASE\"))
                        .Where(file => !file.ToUpper().Contains(@"\OBJ\DEBUG\"))
                        .Where(file => !file.ToUpper().Contains(@"\OBJ\RELEASE\"))
                        .Where(file => !file.ToUpper().Contains(@"\.VSCODE\"))
                        .Where(file => !file.ToUpper().Contains(@"\.VS\"))
                        .Where(file => !file.ToUpper().Contains(@"\OBJ\PROJECT.ASSETS.JSON"))
                        .Where(file => !file.ToUpper().EndsWith(".CSPROJ.NUGET.G.TARGETS"))
                        .Where(file => !file.ToUpper().EndsWith(".CSPROJ.NUGET.G.PROPS"))
                        .Where(file => !Path.GetExtension(file).Equals(".csproj.nuget.g.props", StringComparison.OrdinalIgnoreCase))
                        .Where(file => !Path.GetExtension(file).Equals(".dll", StringComparison.OrdinalIgnoreCase))
                        .Where(file => !Path.GetExtension(file).Equals(".exe", StringComparison.OrdinalIgnoreCase))
                        .Where(file => !Path.GetExtension(file).Equals(".gif", StringComparison.OrdinalIgnoreCase))
                        .Where(file => !Path.GetExtension(file).Equals(".jpg", StringComparison.OrdinalIgnoreCase))
                        .Where(file => !Path.GetExtension(file).Equals(".jpeg", StringComparison.OrdinalIgnoreCase))
                        .Where(file => !Path.GetExtension(file).Equals(".md", StringComparison.OrdinalIgnoreCase))
                        .Where(file => !file.ToUpper().Contains(@"\VIRTUALENV\"))
                        .Where(file => !file.ToUpper().Contains(@"\.VIRTUALENV\"))
                        .Where(file => !file.ToUpper().Contains(@"\VIRTUALENVS\"))
                        .Where(file => !file.ToUpper().Contains(@"\.VIRTUALENVS\"))
                        .Where(file => !file.ToUpper().Contains(@"\ENV\"))
                        .Where(file => !file.ToUpper().Contains(@"\.ENV\"))
                        .Where(file => !file.ToUpper().Contains(@"\ENVS\"))
                        .Where(file => !file.ToUpper().Contains(@"\.ENVS\"))
                        .Where(file => !file.ToUpper().Contains(@"\VENV\"))
                        .Where(file => !file.ToUpper().Contains(@"\.VENV\"))
                        .Where(file => !file.ToUpper().Contains(@"\VENVS\"))
                        .Where(file => !file.ToUpper().Contains(@"\.VENVS\"))
                        .Where(file => !file.ToUpper().Contains(@"\BUILD\LIB\"))
                        .Where(file => !file.ToUpper().Contains(@"\.egg-info\"))
                        .Where(file => !file.ToUpper().Contains(@"\shunit2-2.1.6\"))
                        .Where(file => !file.ToUpper().Contains(@"\vendor\"))
                        .Where(file => !file.ToUpper().Contains(@"\js\generated\coverage\"))
                        .Where(file => !file.ToUpper().Contains(@"\__pycache__\"))
                        .Where(file => !file.ToUpper().Contains(@"\__pycache__\"))
                        .Where(file => !file.ToUpper().Contains(@"\node_modules\"))
                        .Where(file => !Path.GetExtension(file).Equals(".png", StringComparison.OrdinalIgnoreCase));

                    var removedRelativePath = files.Select(f => f.TrimStart('.').TrimStart('\\').Replace("\\","/"));
                    return string.Join("\n", removedRelativePath).Trim() + "\n" + network;
                }
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