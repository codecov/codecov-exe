using System;
using System.Collections.Generic;
using System.Linq;
using Codecov.Services.VersionControlSystems;
using Codecov.Terminal;

namespace Codecov.Factories
{
    internal static class VersionControlSystemFactory
    {
        public static IVersionControlSystem Create(IVersionControlSystemOptions options, ITerminal terminal)
        {
            var assembly = typeof(VersionControlSystemFactory).Assembly;
            var interfaceType = typeof(IVersionControlSystem);

            var versionControlSystems = assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract && interfaceType.IsAssignableFrom(t) && t != typeof(VersionControlSystem));
            var vcs = GetFirstDetectedVcsSystem(versionControlSystems, options, terminal);
            return vcs ?? new VersionControlSystem(options, terminal);
        }

        private static IVersionControlSystem GetFirstDetectedVcsSystem(IEnumerable<Type> supportedVcs, IVersionControlSystemOptions options, ITerminal terminal)
        {
            foreach (var t in supportedVcs)
            {
                var csi = t.GetConstructor(new[] { typeof(IVersionControlSystemOptions), typeof(ITerminal) });

                if (csi is null)
                {
                    continue;
                }

                var buildServer = (IVersionControlSystem)csi.Invoke(new object[] { options, terminal });
                if (buildServer.Detecter)
                {
                    return buildServer;
                }
            }

            return new VersionControlSystem(options, terminal);
        }
    }
}
