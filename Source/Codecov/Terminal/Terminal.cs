using System;
using System.Diagnostics;
using Codecov.Logger;

namespace Codecov.Terminal
{
    internal class Terminal : ITerminal
    {
        public virtual bool Exits => true;

        public string RunScript(string script)
        {
            return Run(script, string.Empty);
        }

        public virtual string Run(string command, string commandArguments)
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

                    using (var reader = process.StandardError)
                    {
                        var error = reader.ReadToEnd().Trim();
                        if (!string.IsNullOrWhiteSpace(error))
                        {
                            throw new Exception(error);
                        }
                    }

                    using (var reader = process.StandardOutput)
                    {
                        return reader.ReadToEnd().Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.VerboaseException(ex);
                return string.Empty;
            }
        }
    }
}
