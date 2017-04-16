using System.Diagnostics;

namespace Codecov.Terminal
{
    internal class Terminal : ITerminal
    {
        public string Run(string command, string commandArguments)
        {
            try
            {
                var startInfo = new ProcessStartInfo(command.Trim(), commandArguments.Trim())
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