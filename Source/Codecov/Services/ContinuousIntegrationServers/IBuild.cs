namespace Codecov.Services.ContinuousIntegrationServers
{
    internal interface IBuild
    {
        string Build { get; }

        string BuildUrl { get; }

        string Job { get; }

        string Service { get; }
    }
}
