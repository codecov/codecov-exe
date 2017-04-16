using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Codecov.Services;
using Codecov.Services.ContinuousIntegrationServers;
using Codecov.Utilities;
using Codecov.Yaml;

namespace Codecov.Url
{
    internal class Query : IQuery
    {
        private readonly Lazy<string> _getQuery;

        public Query(IQueryOptions options, IEnumerable<IRepository> repositories, IBuild build, IYaml yaml)
        {
            Options = options;
            Repositories = repositories;
            Build = build;
            Yaml = yaml;
            SetQueryParameters();
            _getQuery = new Lazy<string>(() => string.Join("&", QueryParameters.Select(x => $"{x.Key}={x.Value ?? string.Empty}")));
        }

        public string GetQuery => _getQuery.Value;

        private IBuild Build { get; }

        private IQueryOptions Options { get; }

        private IDictionary<string, string> QueryParameters { get; set; }

        private IEnumerable<IRepository> Repositories { get; }

        private IYaml Yaml { get; }

        private void OverrideIfNotEmptyOrNull(string key, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                QueryParameters[key] = value.RemoveAllWhiteSpace();
            }
        }

        private void SetBranch()
        {
            QueryParameters["branch"] = string.Empty;

            foreach (var repository in Repositories)
            {
                if (!string.IsNullOrWhiteSpace(repository.Branch))
                {
                    QueryParameters["branch"] = repository.Branch;
                    break;
                }
            }

            OverrideIfNotEmptyOrNull("branch", Options.Branch);
        }

        private void SetBuild()
        {
            QueryParameters["build"] = Build.Build;
            OverrideIfNotEmptyOrNull("build", Options.Build);
        }

        private void SetBuildUrl()
        {
            QueryParameters["build_url"] = Build.BuildUrl;
        }

        private void SetCommit()
        {
            QueryParameters["commit"] = string.Empty;

            foreach (var repository in Repositories)
            {
                if (!string.IsNullOrWhiteSpace(repository.Commit))
                {
                    QueryParameters["commit"] = repository.Commit;
                    break;
                }
            }

            OverrideIfNotEmptyOrNull("commit", Options.Commit);
        }

        private void SetFlags()
        {
            QueryParameters["flags"] = string.Empty;

            var flags = Options.Flags;
            if (string.IsNullOrWhiteSpace(flags))
            {
                return;
            }

            var flagsSeperatedByCommasAndNoExtraWhiteSpace = string.Join(",", flags.Split(',').Select(x => x.Trim()));
            QueryParameters["flags"] = flagsSeperatedByCommasAndNoExtraWhiteSpace;
        }

        private void SetJob()
        {
            QueryParameters["job"] = Uri.EscapeDataString(Build.Job);
        }

        private void SetName()
        {
            QueryParameters["name"] = string.Empty;
            OverrideIfNotEmptyOrNull("name", Options.Name);
        }

        private void SetPackage()
        {
            QueryParameters["package"] = About.Version;
        }

        private void SetPr()
        {
            QueryParameters["pr"] = string.Empty;

            foreach (var repository in Repositories)
            {
                if (!string.IsNullOrWhiteSpace(repository.Pr))
                {
                    QueryParameters["pr"] = repository.Pr;
                    break;
                }
            }

            OverrideIfNotEmptyOrNull("pr", Options.Pr);
        }

        private void SetQueryParameters()
        {
            QueryParameters = new Dictionary<string, string>();
            SetBranch();
            SetCommit();
            SetBuild();
            SetTag();
            SetPr();
            SetName();
            SetFlags();
            SetSlug();
            SetToken();
            SetPackage();
            SetBuildUrl();
            SetYaml();
            SetJob();
            SetService();
        }

        private void SetService()
        {
            QueryParameters["service"] = Build.Service;
        }

        private void SetSlug()
        {
            QueryParameters["slug"] = string.Empty;

            foreach (var repository in Repositories)
            {
                if (!string.IsNullOrWhiteSpace(repository.Slug))
                {
                    QueryParameters["slug"] = WebUtility.UrlEncode(repository.Slug);
                    break;
                }
            }

            var slugEnv = Environment.GetEnvironmentVariable("CODECOV_SLUG");
            if (!string.IsNullOrWhiteSpace(slugEnv))
            {
                QueryParameters["slug"] = WebUtility.UrlEncode(slugEnv.Trim());
            }

            if (!string.IsNullOrWhiteSpace(Options.Slug))
            {
                QueryParameters["slug"] = WebUtility.UrlEncode(Options.Slug.Trim());
            }
        }

        private void SetTag()
        {
            QueryParameters["tag"] = string.Empty;

            foreach (var repository in Repositories)
            {
                if (!string.IsNullOrWhiteSpace(repository.Tag))
                {
                    QueryParameters["tag"] = repository.Tag;
                    break;
                }
            }

            OverrideIfNotEmptyOrNull("tag", Options.Tag);
        }

        private void SetToken()
        {
            QueryParameters["token"] = string.Empty;

            var tokenEnv = Environment.GetEnvironmentVariable("CODECOV_TOKEN");
            if (!string.IsNullOrWhiteSpace(tokenEnv))
            {
                QueryParameters["token"] = tokenEnv.RemoveAllWhiteSpace();
            }

            Guid tokenGuid;
            if (Guid.TryParse(Options.Token, out tokenGuid))
            {
                QueryParameters["token"] = Options.Token.RemoveAllWhiteSpace();
            }
        }

        private void SetYaml()
        {
            QueryParameters["yaml"] = Yaml.FileName;
        }
    }
}