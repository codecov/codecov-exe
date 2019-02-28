using System.Linq;
using Codecov.Services.ContinuousIntegrationServers;

namespace Codecov.Factories
{
    internal static class ContinuousIntegrationServerFactory
    {
        public static IContinuousIntegrationServer Create()
        {
            var continuousIntegrationServers = new IContinuousIntegrationServer[] { new AppVeyor(), new TeamCity(), new AzurePipelines() };
            var buildServer = continuousIntegrationServers.FirstOrDefault(x => x.Detecter);
            return buildServer ?? new ContinuousIntegrationServer();
        }
    }
}
