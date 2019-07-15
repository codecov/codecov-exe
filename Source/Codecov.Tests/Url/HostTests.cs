using System;
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
            var host = new Host(hostOptions);

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
            var host = new Host(hostOptions);

            // When
            var getHost = host.GetHost;

            // Then
            getHost.Should().Be("www.google.com");
        }

        [Fact]
        public void Should_Set_From_EnvironmentVariable()
        {
            // Given
            Environment.SetEnvironmentVariable("CODECOV_URL", "www.google.com/");
            var hostOptions = Substitute.For<IHostOptions>();
            var host = new Host(hostOptions);

            // When
            var getHost = host.GetHost;

            // Then
            getHost.Should().Be("www.google.com");

            // Cleanup
            Environment.SetEnvironmentVariable("CODECOV_URL", null);
        }
    }
}
