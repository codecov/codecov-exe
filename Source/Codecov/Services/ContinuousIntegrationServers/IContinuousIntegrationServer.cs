using Codecov.Factories;

namespace Codecov.Services.ContinuousIntegrationServers
{
    internal interface IContinuousIntegrationServer : IRepository, IBuild, IEnviornmentVariables, IDetect
    {
    }
}
