using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Codecov.Program
{
    public static class Utils
    {
        public static string Version
        {
            get
            {
                var assemblyVersion = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion.Split('.');
                var version = $"{assemblyVersion[0]}.{assemblyVersion[1]}.{assemblyVersion[2]}";
                return $"exe-{version}";
            }
        }

        public static string NormalizePath(string path)
        {
            return Path.GetFullPath(new Uri(path).LocalPath).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).ToUpperInvariant();
        }

        public static string RemoveAllWhiteSpaceFromString(string str) => string.IsNullOrWhiteSpace(str) ? string.Empty : Regex.Replace(str.Trim(), @"\s+", string.Empty);

        public static string RunCmd(string command, string commandArguments)
        {
            try
            {
                var startInfo = new ProcessStartInfo(command, commandArguments)
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
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