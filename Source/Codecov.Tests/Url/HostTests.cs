using Codecov.Services.ContinuousIntegrationServers;
using Codecov.Url;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Codecov.Tests.Url
{
    public class HostTests
    {
        [Fact]
        public void Should_Set_Default_Host()
        {
            // Given
            var hostOptions = Substitute.For<IHostOptions>();
            var environmentVariables = Substitute.For<IEnviornmentVariables>();
            var host = new Host(hostOptions, environmentVariables);

            // When
            var getHost = host.GetHost;

            // Then
            getHost.Should().Be("https://codecov.io");
        }

        [Fact]
        public void Should_Set_From_Commandline()
        {
            // Given
            var hostOptions = Substitute.For<IHostOptions>();
            hostOptions.Url.Returns("www.google.com/");
            var environmentVariables = Substitute.For<IEnviornmentVariables>();
            var host = new Host(hostOptions, environmentVariables);

            // When
            var getHost = host.GetHost;

            // Then
            getHost.Should().Be("www.google.com");
        }

        [Fact]
        public void Should_Set_From_EnvironmentVariable()
        {
            // Given
            var hostOptions = Substitute.For<IHostOptions>();
            var environmentVariables = Substitute.For<IEnviornmentVariables>();
            environmentVariables.GetEnvironmentVariable("CODECOV_URL").Returns("www.google.com/");
            var host = new Host(hostOptions, environmentVariables);

            // When
            var getHost = host.GetHost;

            // Then
            getHost.Should().Be("www.google.com");
        }
    }
}
