using System;
using Serilog;

namespace Codecov.Logger
{
    internal static class Log
    {
        private static LoggerConfiguration _loggerConfig;

        private static ILogger _logger;

        public static void Error(string message) => _logger.Error(message);

        public static void Information(string message) => _logger.Information(message);

        public static void Verbose(string message) => _logger.Verbose(message);

        public static void VerboseException(Exception ex) => Verbose($"{ex.Message}\n{ex.StackTrace}");

        public static void Warning(string message) => _logger.Warning(message);

        public static void Fatal(string message) => _logger.Fatal(message);

        public static void Create(bool isVerbose, bool noColor)
        {
            _loggerConfig = new LoggerConfiguration();
            SetLogLevelIsVerbose(isVerbose);
            SetLogOutputHasColor(noColor);
            _logger = _loggerConfig.CreateLogger();
        }

        private static void SetLogLevelIsVerbose(bool isVerbose)
        {
            if (isVerbose)
            {
                _loggerConfig.MinimumLevel.Verbose();
                return;
            }

            _loggerConfig.MinimumLevel.Information();
        }

        private static void SetLogOutputHasColor(bool noColor)
        {
            if (noColor)
            {
                _loggerConfig.WriteTo.Console();
                return;
            }

            _loggerConfig.WriteTo.ColoredConsole();
        }
    }
}
