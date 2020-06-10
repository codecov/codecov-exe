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

        internal static string GetFirstExistingEnvironmentVariable(params string[] names)
        {
            foreach (var name in names)
            {
                var env = GetEnviornmentVariable(name);

                if (!string.IsNullOrWhiteSpace(env))
                {
                    return env;
                }
            }

            return string.Empty;
        }
    }
}
