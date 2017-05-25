using System;
using System.Collections.Generic;
using Codecov.Coverage.EnviornmentVariables;
using Codecov.Services.ContinuousIntegrationServers;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Codecov.Tests.Coverage.EnviornmentVariables
{
    public class EnviornmentVariablesTests
    {
        [Fact]
        public void GetEnviornmentVariables_Should_Never_Be_Null()
        {
            // Given
            var options = Substitute.For<IEnviornmentVariablesOptions>();
            var continuousIntegrationServer = Substitute.For<IContinuousIntegrationServer>();
            var enviornmentVariables = new Codecov.Coverage.EnviornmentVariables.EnviornmentVariables(options, continuousIntegrationServer);

            // When
            var getEnviornmentVariables = enviornmentVariables.GetEnviornmentVariables;

            // Then
            getEnviornmentVariables.Should().NotBeNull();
        }

        [Fact]
        public void Should_Include_CODECOV_ENV()
        {
            // Given
            var options = Substitute.For<IEnviornmentVariablesOptions>();
            var continuousIntegrationServer = Substitute.For<IContinuousIntegrationServer>();
            var enviornmentVariables = new Codecov.Coverage.EnviornmentVariables.EnviornmentVariables(options, continuousIntegrationServer);
            Environment.SetEnvironmentVariable("CODECOV_ENV", "foo");

            // When
            var getEnviornmentVariables = enviornmentVariables.GetEnviornmentVariables;

            // Then
            getEnviornmentVariables["CODECOV_ENV"].Should().Be("foo");
        }

        [Fact]
        public void Should_Include_EnviornmentVariables_From_ContiniousIntegrationServer()
        {
            // Given
            var options = Substitute.For<IEnviornmentVariablesOptions>();
            var continuousIntegrationServer = Substitute.For<IContinuousIntegrationServer>();
            continuousIntegrationServer.GetEnviornmentVariables.Returns(new Dictionary<string, string> { { "foo", "bar" }, { "fizz", "bizz" } });
            var enviornmentVariables = new Codecov.Coverage.EnviornmentVariables.EnviornmentVariables(options, continuousIntegrationServer);
            Environment.SetEnvironmentVariable("foo", null);
            Environment.SetEnvironmentVariable("fizz", null);

            // When
            var getEnviornmentVariables = enviornmentVariables.GetEnviornmentVariables;

            // Then
            getEnviornmentVariables["foo"].Should().Be("bar");
            getEnviornmentVariables["fizz"].Should().Be("bizz");
        }

        [Fact]
        public void Should_Include_EnviornmentVariables_From_Options()
        {
            // Given
            var options = Substitute.For<IEnviornmentVariablesOptions>();
            options.Envs.Returns(new[] { "foo", "fizz" });
            var continuousIntegrationServer = Substitute.For<IContinuousIntegrationServer>();
            var enviornmentVariables = new Codecov.Coverage.EnviornmentVariables.EnviornmentVariables(options, continuousIntegrationServer);
            Environment.SetEnvironmentVariable("foo", "bar");
            Environment.SetEnvironmentVariable("fizz", "bizz");

            // When
            var getEnviornmentVariables = enviornmentVariables.GetEnviornmentVariables;

            // Then
            getEnviornmentVariables["foo"].Should().Be("bar");
            getEnviornmentVariables["fizz"].Should().Be("bizz");
        }
    }
}
