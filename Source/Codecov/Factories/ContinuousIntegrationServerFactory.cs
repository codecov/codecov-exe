using System;
using System.Collections.Generic;
using System.Linq;
using Codecov.Services.ContinuousIntegrationServers;

namespace Codecov.Factories
{
    internal static class ContinuousIntegrationServerFactory
    {
        public static IContinuousIntegrationServer Create()
        {
            var assembly = typeof(ContinuousIntegrationServerFactory).Assembly;
            var interfaceType = typeof(IContinuousIntegrationServer);

            var continuousServers = assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract && interfaceType.IsAssignableFrom(t) && t != typeof(ContinuousIntegrationServer));
            var buildServer = GetFirstDetectedCiServer(continuousServers);

            return buildServer ?? new ContinuousIntegrationServer();
        }

        private static IContinuousIntegrationServer GetFirstDetectedCiServer(IEnumerable<Type> supportedServers)
        {
            foreach (var t in supportedServers)
            {
                var buildServer = (IContinuousIntegrationServer)Activator.CreateInstance(t);
                if (buildServer.Detecter)
                {
                    return buildServer;
                }
            }

            return null;
        }
    }
}
