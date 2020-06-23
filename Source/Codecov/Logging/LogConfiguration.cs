using System.Diagnostics;
using Codecov.Program;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace Codecov.Logging
{
    internal static class LogConfiguration
    {
        private const string ConsoleFullTemplate = "[{Level:u3}] " + ConsoleInfoTemplate;
        private const string ConsoleInfoTemplate = "{Message:l}{NewLine}{Exception}";
        private static readonly ConsoleTheme _consoleTheme = AnsiConsoleTheme.Code;
        private static readonly ConsoleTheme _noColorTheme = ConsoleTheme.None;

        public static void ConfigureLogging(CommandLineOptions options)
        {
            var config = new LoggerConfiguration()
                .MinimumLevel.Verbose();

            CreateDebugLogger(config);
            CreateConsoleInformationLogger(config, ConsoleInfoTemplate, options);
            CreateConsoleFullLogger(config, ConsoleFullTemplate, options);

            Log.Logger = config.CreateLogger();
        }

        private static void CreateConsoleFullLogger(LoggerConfiguration config, string consoleTemplate, CommandLineOptions options)
        {
            var color = options.NoColor ? _noColorTheme : _consoleTheme;

            config.WriteTo.Logger((config) => config
                .Filter.ByExcluding((logEvent) => !options.Verbose && logEvent.Level == LogEventLevel.Verbose)
                .Filter.ByExcluding((logEvent) => logEvent.Level == LogEventLevel.Information)
                .WriteTo.Console(
                    outputTemplate: consoleTemplate,
                    standardErrorFromLevel: LogEventLevel.Warning,
                    theme: color));
        }

        private static void CreateConsoleInformationLogger(LoggerConfiguration config, string consoleTemplate, CommandLineOptions options)
        {
            var color = options.NoColor ? _noColorTheme : _consoleTheme;

            config.WriteTo.Logger((config) => config
                .Filter.ByIncludingOnly((logEvent) => logEvent.Level == LogEventLevel.Information)
                .WriteTo.Console(
                    outputTemplate: consoleTemplate,
                    theme: color));
        }

        [Conditional("DEBUG")]
        private static void CreateDebugLogger(LoggerConfiguration config)
        {
            config.WriteTo.Debug();
        }
    }
}
