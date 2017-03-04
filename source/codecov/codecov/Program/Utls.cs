using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace codecov.Program
{
    public static class Utils
    {
        public static string Version
        {
            get
            {
                var version = Assembly.GetExecutingAssembly().GetName().Version;
                return $"exe-{version.Major}.{version.Minor}.{version.Build}";
            }
        }

        public static string NormalizePath(string path)
        {
            return Path.GetFullPath(new Uri(path).LocalPath).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).ToUpperInvariant();
        }

        public static string RemoveAllWhiteSpaceFromString(string str) => Regex.Replace(str.Trim(), @"\s+", string.Empty);

        public static string RunCmd(string command, string commandArguments)
        {
            try
            {
                var startInfo = new ProcessStartInfo(command, commandArguments)
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true
                };

                using (var process = Process.Start(startInfo))
                {
                    if (process == null)
                    {
                        return string.Empty;
                    }

                    using (var reader = process.StandardOutput)
                    {
                        return reader.ReadToEnd().Trim();
                    }
                }
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}