using System;
using System.Collections.Generic;
using System.Linq;
using Codecov.Services.ContinuousIntegrationServers;

namespace Codecov.Factories
{
    internal static class ContinuousIntegrationServerFactory
    {
        public static IContinuousIntegrationServer Create(IEnviornmentVariables environmentVariables)
        {
            if (environmentVariables is null)
            {
                throw new ArgumentNullException(nameof(environmentVariables));
            }

            var assembly = typeof(ContinuousIntegrationServerFactory).Assembly;
            var interfaceType = typeof(IContinuousIntegrationServer);

            var continuousServers = assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract && interfaceType.IsAssignableFrom(t) && t != typeof(ContinuousIntegrationServer));
            var buildServer = GetFirstDetectedCiServer(continuousServers, environmentVariables);

            return buildServer;
        }

        private static IContinuousIntegrationServer GetFirstDetectedCiServer(IEnumerable<Type> supportedServers, IEnviornmentVariables environmentVariables)
        {
            foreach (var t in supportedServers)
            {
                var csi = t.GetConstructor(new[] { typeof(IEnviornmentVariables) });

                if (csi == null)
                {
                    continue;
                }

                var buildServer = (IContinuousIntegrationServer)csi.Invoke(new object[] { environmentVariables });
                if (buildServer.Detecter)
                {
                    return buildServer;
                }
            }

            return new ContinuousIntegrationServer(environmentVariables);
        }
    }
}
