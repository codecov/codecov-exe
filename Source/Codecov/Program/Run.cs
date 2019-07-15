using System;
using System.Collections.Generic;
using Codecov.Logger;
using Codecov.Upload;
using Codecov.Utilities;
using CommandLine;

namespace Codecov.Program
{
    internal static class Run
    {
        private static CommandLineOptions _commandLineOptions;
        private static int _kill;

        internal static int Runner(IEnumerable<string> args)
        {
            try
            {
                Init(args);
                Uploader();
                return 0;
            }
            catch (Exception e)
            {
                Log.Fatal($"{e.Message}\n{e.StackTrace}");

                return _kill;
            }
            finally
            {
                // Cleaning up undisposed fields/Properties
                CodecovUploader.Cleanup();
                Log.Cleanup();
            }
        }

        private static void ConfigureHowProgramExitsOnFail()
        {
            _kill = _commandLineOptions.Required ? 1 : 0;
        }

        private static void Init(IEnumerable<string> args)
        {
            ParseAndSetCommandLineArgs(args);
            ConfigureHowProgramExitsOnFail();
            Log.Create(_commandLineOptions.Verbose, _commandLineOptions.NoColor);
            About.DisplayFiglet();
        }

        private static void ParseAndSetCommandLineArgs(IEnumerable<string> args)
        {
            var result = (Parsed<CommandLineOptions>)Parser.Default.ParseArguments<CommandLineOptions>(args)
                .WithNotParsed((errors) =>
                {
                    foreach (var error in errors)
                    {
                        if (error.Tag == ErrorType.UnknownOptionError)
                        {
                            Environment.Exit(0xA0); // Exit immediately with exit code 160 for invalid arguments
                        }
                    }
                });
            _commandLineOptions = result.Value;
        }

        private static void Uploader()
        {
            var upload = new UploadFacade(_commandLineOptions);
            upload.Uploader();
        }
    }
}
