using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using codecov.Program;

namespace codecov.Services.Utils
{
    public interface ICodecovYml
    {
        string Slug { get; }

        string Token { get; }

        string Url { get; }
    }

    public class CodecovYml : ICodecovYml
    {
        public CodecovYml(string root)
        {
            var file = FindFile(root);
            if (File.Exists(file))
            {
                Yml = File.ReadAllText(file);
            }
        }

        public string Slug => GetValue(@"codecov_slug");

        public string Token => GetValue(@"codecov_token");

        public string Url => GetValue(@"codecov_url");

        private string Yml { get; }

        private static string FindFile(string root)
        {
            var gitRoot = root.Trim();
            var yml = Directory.GetFiles(gitRoot, "*.*", SearchOption.AllDirectories).FirstOrDefault(f => f.Equals("codecov.yml") || f.Equals(".codecov.yml"));

            if (yml != null)
            {
                return yml;
            }

            Log.Verbose("Yaml not found, that's ok! Learn more at http://docs.codecov.io/docs/codecov-yaml");
            return string.Empty;
        }

        private string GetValue(string key)
        {
            if (Yml == null)
            {
                return string.Empty;
            }

            var regexSameLine = new Regex($@"{key}\s*:\s*.*", RegexOptions.IgnoreCase);
            var matchSameLine = regexSameLine.Match(Yml);
            if (matchSameLine.Success)
            {
                var split = matchSameLine.Value.Split(':');
                if (split.Length == 2)
                {
                    return split[1].Trim();
                }
            }

            var regexNewLine = new Regex($@"{key}\s*:\s*-\s.*", RegexOptions.IgnoreCase);
            var matchNewLine = regexNewLine.Match(Yml);
            if (matchNewLine.Success)
            {
                var split = matchNewLine.Value.Split('-');
                if (split.Length == 2)
                {
                    return split[1].Trim();
                }
            }

            return string.Empty;
        }
    }
}