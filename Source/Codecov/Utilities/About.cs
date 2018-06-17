using System;
using System.Reflection;

namespace Codecov.Utilities
{
    internal static class About
    {
        internal static string Version
        {
            get
            {
                var assemblyVersion = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
                return $"exe-{assemblyVersion}";
            }
        }

        internal static void DisplayFiglet()
        {
            Console.WriteLine($@"
              _____          _
             / ____|        | |
            | |     ___   __| | ___  ___ _____   __
            | |    / _ \ / _  |/ _ \/ __/ _ \ \ / /
            | |___| (_) | (_| |  __/ (_| (_) \ V /
             \_____\___/ \____|\___|\___\___/ \_/
                                         {Version}
            ");
        }
    }
}
