using System;
using Serilog;

namespace Codecov.Logger
{
    internal static class Log
    {
        private static ILogger _logger;
        private static LoggerConfiguration _loggerConfig;

        public static void Create(bool isVerbose, bool noColor)
        {
            _loggerConfig = new LoggerConfiguration();
            SetLogLevelIsVerboase(isVerbose);
            SetLogOutputHasColor(noColor);
#pragma warning disable DF0025 // Marks undisposed objects assinged to a field, originated from method invocation.
            _logger = _loggerConfig.CreateLogger();
#pragma warning restore DF0025 // Marks undisposed objects assinged to a field, originated from method invocation.
        }

        public static void Error(string message) => _logger.Error(message);

        public static void Fatal(string message) => _logger.Fatal(message);

        public static void Information(string message) => _logger.Information(message);

        public static void Verboase(string message) => _logger.Verbose(message);

        public static void VerboaseException(Exception ex) => Verboase($"{ex.Message}\n{ex.StackTrace}");

        public static void Warning(string message) => _logger.Warning(message);

        internal static void Cleanup()
        {
            if (_logger is IDisposable disposable)
            {
                disposable.Dispose();
            }

            _logger = null;
        }

        private static void SetLogLevelIsVerboase(bool isVerbose)
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
