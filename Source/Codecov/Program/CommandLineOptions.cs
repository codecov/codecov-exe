using System.Collections.Generic;
using Codecov.Coverage.EnviornmentVariables;
using Codecov.Coverage.Report;
using Codecov.Coverage.Tool;
using Codecov.Services.VersionControlSystems;
using Codecov.Url;
using CommandLine;

namespace Codecov.Program
{
    /// <summary>
    /// The command line api. <see href="https://docs.codecov.io/v4.3.6/reference#section-upload-query-as-seen-as-query-below"/>.
    /// </summary>
    public class CommandLineOptions : IVersionControlSystemOptions, ICoverageOptions, IEnviornmentVariablesOptions, IHostOptions, IQueryOptions, IReportOptions
    {
        /// <summary>
        /// Gets or sets a property specifing the branch name.
        /// </summary>
        /// <value>A property specifing the branch name.</value>
        [Option("branch", HelpText = "Specify the branch name.")]
        public string Branch { get; set; }

        /// <summary>
        /// Gets or sets a property specifing the build number.
        /// </summary>
        /// <value>A property specifing the build number.</value>
        [Option('b', "build", HelpText = "Specify the build number.")]
        public string Build { get; set; }

        /// <summary>
        /// Gets or sets a property specifing the commit sha.
        /// </summary>
        /// <value>A property specifing the commit sha.</value>
        [Option('c', "sha", HelpText = "Specify the commit sha.")]
        public string Commit { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to toggle functionalities. (1) --disable-network.
        /// Disable uploading the file network.
        /// </summary>
        /// <value>
        /// A value indicating whether to toggle functionalities. (1) --disable-network. Disable
        /// uploading the file network.
        /// </value>
        [Option("disable-network", HelpText = "Toggle functionalities. (1) --disable-network. Disable uploading the file network.")]
        public bool DisableNetwork { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to don't upload and dump to stdin.
        /// </summary>
        /// <value>A value indicating whether to don't upload and dump to stdin.</value>
        [Option('d', "dump", HelpText = "Don't upload and dump to stdin.")]
        public bool Dump { get; set; }

        /// <summary>
        /// Gets or sets a value specifing the enviornment variables to be included with this build.
        /// (1) CODECOV_ENV=VAR1,VAR2. (2) -e VAR1 VAR2.
        /// </summary>
        /// <value>
        /// A value specifing the enviornment variables to be included with this build. (1)
        /// CODECOV_ENV=VAR1,VAR2. (2) -e VAR1 VAR2.
        /// </value>
        [Option('e', "env", HelpText = "Specify enviornment variables to be included with this build. (1) CODECOV_ENV=VAR1,VAR2. (2) -e VAR1 VAR2.")]
        public IEnumerable<string> Envs { get; set; }

        /// <summary>
        /// Gets or sets a value specifing the target file(s) to upload. (1) -f 'path/to/file'. Only
        /// upload this file. (2) -f 'path/to/file1 path/to/file2'. Only upload these files.
        /// </summary>
        /// <value>
        /// A value specifing the target file(s) to upload. (1) -f 'path/to/file'. Only upload this
        /// file. (2) -f 'path/to/file1 path/to/file2'. Only upload these files.
        /// </value>
        [Option('f', "file", HelpText = "Target file(s) to upload. (1) -f 'path/to/file'. Only upload this file. (2) -f 'path/to/file1 path/to/file2'. Only upload these files.")]
        public IEnumerable<string> Files { get; set; }

        /// <summary>
        /// Gets or sets a value specifing the flag the upload to group coverage metrics. (1) --flag
        /// unittests. This upload is only unittests. (2) --flag integration. This upload is only
        /// integration tests. (3) --flag ut,chrome. This upload is chrome - UI tests.
        /// </summary>
        /// <value>
        /// A value specifing the flag the upload to group coverage metrics. (1) --flag unittests.
        /// This upload is only unittests. (2) --flag integration. This upload is only integration
        /// tests. (3) --flag ut,chrome. This upload is chrome - UI tests.
        /// </value>
        [Option("flag", HelpText = "Flag the upload to group coverage metrics. (1) --flag unittests. This upload is only unittests. (2) --flag integration. This upload is only integration tests. (3) --flag ut,chrome. This upload is chrome - UI tests.")]
        public string Flags { get; set; }

        /// <summary>
        /// Gets or sets a value specifing the custom defined name of the upload. Visible in Codecov UI.
        /// </summary>
        /// <value>
        /// A value specifing the custom defined name of the upload. Visible in Codecov UI.
        /// </value>
        [Option('n', "name", HelpText = "Custom defined name of the upload. Visible in Codecov UI.")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to remove color from the output.
        /// </summary>
        /// <value>A value indicating whether to remove color from the output.</value>
        [Option("no-color", HelpText = "Remove color from the output.")]
        public bool NoColor { get; set; }

        /// <summary>
        /// Gets or sets a value specifing the pull request number.
        /// </summary>
        /// <value>A value specifing the pull request number.</value>
        [Option("pr", HelpText = "Specify the pull request number.")]
        public string Pr { get; set; }

        /// <summary>
        /// Gets or sets a value used when not in git/hg project to identify project root directory.
        /// </summary>
        /// <value>A value used when not in git/hg project to identify project root directory.</value>
        [Option("root", HelpText = "Used when not in git/hg project to identify project root directory.")]
        public string RepoRoot { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to exit with 1 if not successful. Default will
        /// Exit with 0.
        /// </summary>
        /// <value>
        /// A value indicating whether to exit with 1 if not successful. Default will Exit with 0.
        /// </value>
        [Option("required", HelpText = "Exit with 1 if not successful. Default will Exit with 0.")]
        public bool Required { get; set; }

        /// <summary>
        /// Gets or sets a value specifing the owner/repo slug used instead of the private repo token
        /// in Enterprise. (option) Set environment variable CODECOV_SLUG=:owner/:repo.
        /// </summary>
        /// <value>
        /// A value specifing the owner/repo slug used instead of the private repo token in
        /// Enterprise. (option) Set environment variable CODECOV_SLUG=:owner/:repo.
        /// </value>
        [Option('r', "slug", HelpText = "owner/repo slug used instead of the private repo token in Enterprise. (option) Set environment variable CODECOV_SLUG=:owner/:repo.")]
        public string Slug { get; set; }

        /// <summary>
        /// Gets or sets a value specifing the git tag.
        /// </summary>
        /// <value>A value specifing the git tag.</value>
        [Option("tag", HelpText = "Specify the git tag.")]
        public string Tag { get; set; }

        /// <summary>
        /// Gets or sets a value specifing the private repository token. (option) set the enviornment
        /// variable CODECOV_TOKEN-uuid. (1) -t @/path/to/token_file. (2) -t uuid.
        /// </summary>
        /// <value>
        /// A value specifing the private repository token. (option) set the enviornment variable
        /// CODECOV_TOKEN-uuid. (1) -t @/path/to/token_file. (2) -t uuid.
        /// </value>
        [Option('t', "token", HelpText = "Set the private repository token. (option) set the enviornment variable CODECOV_TOKEN=:uuid. (1) -t uuid.")]
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets a value specifing the target url for Enterprise customers. (option) Set
        /// environment variable CODECOV_URL=https://my-hosted-codecov.com.
        /// </summary>
        /// <value>
        /// A value specifing the target url for Enterprise customers. (option) Set environment
        /// variable CODECOV_URL=https://my-hosted-codecov.com.
        /// </value>
        [Option('u', "url", HelpText = "Set the target url for Enterprise customers. (option) Set environment variable CODECOV_URL=https://my-hosted-codecov.com.")]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to run in verbose mode.
        /// </summary>
        /// <value>A value indicating whether to run in verbose mode.</value>
        [Option('v', "verbose", HelpText = "Verbose mode.")]
        public bool Verbose { get; set; }
    }
}
