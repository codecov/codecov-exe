using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
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
                using (var process = new Process())
                {
                    process.StartInfo = new ProcessStartInfo(command.Trim(), commandArguments.Trim())
                    {
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    };

                    var output = new StringBuilder();
                    var error = new StringBuilder();

                    using (var outputWaitHandle = new AutoResetEvent(false))
                    using (var errorWaitHandle = new AutoResetEvent(false))
                    {
                        process.OutputDataReceived += (sender, e) =>
                        {
                            if (e.Data == null)
                            {
                                // ReSharper disable AccessToDisposedClosure
                                outputWaitHandle.Set(); // ReSharper restore AccessToDisposedClosure
                            }
                            else
                            {
                                output.AppendLine(e.Data);
                            }
                        };

                        process.ErrorDataReceived += (sender, e) =>
                        {
                            if (e.Data == null)
                            {
                                // ReSharper disable AccessToDisposedClosure
                                errorWaitHandle.Set(); // ReSharper restore AccessToDisposedClosure
                            }
                            else
                            {
                                error.AppendLine(e.Data);
                            }
                        };

                        process.Start();

                        process.BeginOutputReadLine();
                        process.BeginErrorReadLine();

                        const int timeout = 300000; // 5 mins
                        if (!process.WaitForExit(timeout) || !outputWaitHandle.WaitOne(timeout) || !errorWaitHandle.WaitOne(timeout))
                        {
                            throw new Exception("Terminal process timed out.");
                        }

                        var errorAsString = error.ToString().Trim();
                        if (!string.IsNullOrWhiteSpace(errorAsString))
                        {
                            throw new Exception(errorAsString);
                        }

                        return output.ToString().Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.VerboseException(ex);
                return string.Empty;
            }
        }
    }
}
