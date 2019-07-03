using System;
using System.Collections.Generic;
using Codecov.Coverage.EnvironmentVariables;
using Codecov.Services.ContinuousIntegrationServers;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Codecov.Tests.Coverage.EnvironmentVariables
{
    public class EnvironmentVariablesTests
    {
        [Fact]
        public void GetEnvironmentVariables_Should_Never_Be_Null()
        {
            // Given
            var options = Substitute.For<IEnvironmentVariablesOptions>();
            var continuousIntegrationServer = Substitute.For<IContinuousIntegrationServer>();
            var EnvironmentVariables = new Codecov.Coverage.EnvironmentVariables.EnvironmentVariables(options, continuousIntegrationServer);

            // When
            var getEnvironmentVariables = EnvironmentVariables.GetEnvironmentVariables;

            // Then
            getEnvironmentVariables.Should().NotBeNull();
        }

        [Fact]
        public void Should_Include_CODECOV_ENV()
        {
            // Given
            var options = Substitute.For<IEnvironmentVariablesOptions>();
            var continuousIntegrationServer = Substitute.For<IContinuousIntegrationServer>();
            var EnvironmentVariables = new Codecov.Coverage.EnvironmentVariables.EnvironmentVariables(options, continuousIntegrationServer);
            Environment.SetEnvironmentVariable("CODECOV_ENV", "foo");

            // When
            var getEnvironmentVariables = EnvironmentVariables.GetEnvironmentVariables;

            // Then
            getEnvironmentVariables["CODECOV_ENV"].Should().Be("foo");
        }

        [Fact]
        public void Should_Include_EnvironmentVariables_From_ContiniousIntegrationServer()
        {
            // Given
            var options = Substitute.For<IEnvironmentVariablesOptions>();
            var continuousIntegrationServer = Substitute.For<IContinuousIntegrationServer>();
            continuousIntegrationServer.GetEnvironmentVariables.Returns(new Dictionary<string, string> { { "foo", "bar" }, { "fizz", "bizz" } });
            var EnvironmentVariables = new Codecov.Coverage.EnvironmentVariables.EnvironmentVariables(options, continuousIntegrationServer);
            Environment.SetEnvironmentVariable("foo", null);
            Environment.SetEnvironmentVariable("fizz", null);

            // When
            var getEnvironmentVariables = EnvironmentVariables.GetEnvironmentVariables;

            // Then
            getEnvironmentVariables["foo"].Should().Be("bar");
            getEnvironmentVariables["fizz"].Should().Be("bizz");
        }

        [Fact]
        public void Should_Include_EnvironmentVariables_From_Options()
        {
            // Given
            var options = Substitute.For<IEnvironmentVariablesOptions>();
            options.Envs.Returns(new[] { "foo", "fizz" });
            var continuousIntegrationServer = Substitute.For<IContinuousIntegrationServer>();
            var EnvironmentVariables = new Codecov.Coverage.EnvironmentVariables.EnvironmentVariables(options, continuousIntegrationServer);
            Environment.SetEnvironmentVariable("foo", "bar");
            Environment.SetEnvironmentVariable("fizz", "bizz");

            // When
            var getEnvironmentVariables = EnvironmentVariables.GetEnvironmentVariables;

            // Then
            getEnvironmentVariables["foo"].Should().Be("bar");
            getEnvironmentVariables["fizz"].Should().Be("bizz");
        }
    }
}
