using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Codecov.Coverage.EnviornmentVariables;
using Codecov.Coverage.Report;
using Codecov.Coverage.SourceCode;
using Codecov.Factories;
using Codecov.Terminal;
using Codecov.Upload;
using Codecov.Url;
using Codecov.Utilities;
using CommandLine;
using Serilog;

namespace Codecov.Program
{
    [ExcludeFromCodeCoverage]
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
                Log.Fatal(e, e.Message);

                return _kill;
            }
            finally
            {
                // Cleaning up undisposed fields/Properties
                CodecovUploader.Cleanup();
                Log.CloseAndFlush();
            }
        }

        private static void ConfigureHowProgramExitsOnFail()
            => _kill = _commandLineOptions.Required ? 1 : 0;

        private static void Init(IEnumerable<string> args)
        {
            ParseAndSetCommandLineArgs(args);
            ConfigureHowProgramExitsOnFail();
            Logging.LogConfiguration.ConfigureLogging(_commandLineOptions);
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
            var coverage = new Coverage.Tool.Coverage(_commandLineOptions);
            var envVars = new EnviornmentVariables(_commandLineOptions);
            var continuousIntegrationServer = ContinuousIntegrationServerFactory.Create(envVars);
            envVars.LoadEnviornmentVariables(continuousIntegrationServer);
            var terminals = TerminalFactory.Create();
            var versionControlSystem = VersionControlSystemFactory.Create(_commandLineOptions, terminals[TerminalName.Generic]);
            var sourceCode = new SourceCode(versionControlSystem);
            var yaml = new Yaml.Yaml(sourceCode);
            var repositories = RepositoryFactory.Create(versionControlSystem, continuousIntegrationServer);
            var url = new Url.Url(new Host(_commandLineOptions, envVars), new Route(), new Query(_commandLineOptions, repositories, continuousIntegrationServer, yaml, envVars));
            var report = new Report(_commandLineOptions, envVars, sourceCode, coverage);
            var upload = new Uploads(url, report, _commandLineOptions.Features);
            var uploadFacade = new UploadFacade(continuousIntegrationServer, versionControlSystem, yaml, coverage, envVars, url, upload);

            uploadFacade.LogContinuousIntegrationAndVersionControlSystem();
            if (_commandLineOptions.Dump)
            {
                Log.Warning("Skipping upload and dumping contents.");
                Log.Information("url: {GetUrl}", url.GetUrl);
                Log.Information(report.Reporter);
                return;
            }

            uploadFacade.UploadReports();
        }
    }
}
