using System;
using codecov.Services.Utils;

namespace codecov.Services
{
    internal class Hg : Url, IDetect
    {
        public Hg()
        {
            QueryParameters["branch"] = Branch;
            QueryParameters["commit"] = Commit;
        }

        public bool Detect
        {
            get
            {
                var isHg = !string.IsNullOrWhiteSpace(RunHg("version"));
                if (!isHg)
                {
                    return false;
                }

                Console.WriteLine("==> No CI provider detected, using Mercurial.");
                return true;
            }
        }

        private static string Branch
        {
            get
            {
                var branch = RunHg(@"branch");
                if (branch != null && branch.Equals("HEAD"))
                {
                    return string.Empty;
                }

                return !string.IsNullOrWhiteSpace(branch) ? branch : string.Empty;
            }
        }

        private static string Commit
        {
            get
            {
                var commit = RunHg(@"rev-parse HEAD").Replace("+", string.Empty);
                return !string.IsNullOrWhiteSpace(commit) ? commit : string.Empty;
            }
        }

        private static string RunHg(string commandArguments)
        {
            return Cmd.Run("hg", commandArguments);
        }
    }
}