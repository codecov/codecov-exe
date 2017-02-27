using CommandLine;

namespace codecov.Program
{
    public class Options
    {
        [Option('B', "branch", HelpText = "Specify the branch name.")]
        public string Branch { get; set; }

        [Option('b', "build", HelpText = "Specify the build number.")]
        public string Build { get; set; }

        [Option('d', "dump", DefaultValue = false, HelpText = "Don't upload and dump to stdin.")]
        public bool Dump { get; set; }

        [OptionArray('e', "env", HelpText = "Specify enviornment variables to be included with this build. (1) CODECOV_ENV=VAR1,VAR2. (2) -e 'VAR1 VAR2'")]
        public string[] Env { get; set; }

        [OptionArray('f', "file", HelpText = "Target file(s) to upload. (1) -f 'path/to/file'. Only upload this file. (2) -f 'path/to/file1 path/to/file2'. Only upload these files.")]
        public string[] File { get; set; }

        [OptionArray('F', "flag", HelpText = "Flag the upload to group coverage metrics. (1) -F unittests. This upload is only unittests. (2) -F integration. This upload is only integration tests. (3) -F ut,chrome. This upload is chrome - UI tests.")]
        public string[] Flag { get; set; }

        [Option('p', "pr", HelpText = "Specify the pull request number.")]
        public string Pr { get; set; }

        [Option('R', "root", DefaultValue = ".", HelpText = "Used when not in git/hg project to identify project root directory.")]
        public string Root { get; set; }

        [Option('c', "sha", HelpText = "Specify the commit sha.")]
        public string Sha { get; set; }

        [Option('r', "slug", HelpText = "owner/repo slug used instead of the private repo token in Enterprise. (option) Set environment variable CODECOV_SLUG=:owner/:repo.")]
        public string Slug { get; set; }

        [Option('t', "tag", HelpText = "Specify the git tag.")]
        public string Tag { get; set; }

        [Option('t', "token", HelpText = "Set the private repository token. (option) set the enviornment variable CODECOV_TOKEN-uuid. (1) -t @/path/to/token_file. (2) -t uuid.")]
        public string Token { get; set; }

        [Option('u', "url", HelpText = "Set the target url fro Enterprise customers. (option) Set environment variable CODECOV_URL=https://my-hosted-codecov.com.")]
        public string Url { get; set; }

        [Option('v', "verbose", DefaultValue = false, HelpText = "Verbose mode.")]
        public bool Verbose { get; set; }
    }
}