using System.Diagnostics;

namespace codecov.Services.Utils
{
    internal static class Cmd
    {
        internal static string Run(string command, string commandArguments)
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