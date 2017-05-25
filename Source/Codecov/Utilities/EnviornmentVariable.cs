using System;

namespace Codecov.Utilities
{
    internal static class EnviornmentVariable
    {
        internal static string GetEnviornmentVariable(string name)
        {
            var env = Environment.GetEnvironmentVariable(name);
            return string.IsNullOrWhiteSpace(env) ? string.Empty : env;
        }
    }
}
