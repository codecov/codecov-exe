using System;
using Codecov.Services.ContinuousIntegrationServers;
using FluentAssertions;
using Xunit;

namespace Codecov.Tests.Services.ContiniousIntegrationServers
{
    public class ContiniousIntegrationServerTests
    {
        [Fact]
        public void Branch_Should_Be_Empty_String()
        {
            // Given
            var continuousIntegrationServer = new ContinuousIntegrationServer();

            // When
            var branch = continuousIntegrationServer.Branch;

            // Then
            branch.Should().BeEmpty();
        }

        [Fact]
        public void Build_Should_Be_Empty_String_When_Enviornment_Variable_Does_Not_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("CI_BUILD_ID", null);
            var continuousIntegrationServer = new ContinuousIntegrationServer();

            // When
            var build = continuousIntegrationServer.Build;

            // Then
            build.Should().BeEmpty();
        }

        [Fact]
        public void Build_Should_Be_Set_When_Enviornment_Variable_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("CI_BUILD_ID", "123");
            var continuousIntegrationServer = new ContinuousIntegrationServer();

            // When
            var build = continuousIntegrationServer.Build;
            // Then
            build.Should().Be("123");
        }

        [Fact]
        public void BuildUrl_Should_Be_Empty_String_When_Enviornment_Variable_Does_Not_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("CI_BUILD_URL", null);
            var continuousIntegrationServer = new ContinuousIntegrationServer();

            // When
            var buildUrl = continuousIntegrationServer.BuildUrl;

            // Then
            buildUrl.Should().BeEmpty();
        }

        [Fact]
        public void BuildUrl_Should_Be_Set_When_Enviornment_Variable_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("CI_BUILD_URL", "www.google.com");
            var continuousIntegrationServer = new ContinuousIntegrationServer();

            // When
            var buildUrl = continuousIntegrationServer.BuildUrl;
            // Then
            buildUrl.Should().Be("www.google.com");
        }

        [Fact]
        public void Commit_Should_Empty_String()
        {
            // Given
            var continuousIntegrationServer = new ContinuousIntegrationServer();

            // When
            var commit = continuousIntegrationServer.Commit;

            // Then
            commit.Should().BeEmpty();
        }

        [Fact]
        public void Detecter_Should_False()
        {
            // Given
            var continuousIntegrationServer = new ContinuousIntegrationServer();

            // when
            var detecter = continuousIntegrationServer.Detecter;

            // Then
            detecter.Should().BeFalse();
        }

        [Fact]
        public void GetEnviornmentVariables_Should_Empty_Dictionary()
        {
            // Given
            var continuousIntegrationServer = new ContinuousIntegrationServer();

            // When
            var enviornmentVariables = continuousIntegrationServer.GetEnviornmentVariables;

            // Then
            enviornmentVariables.Should().BeEmpty();
        }

        [Fact]
        public void Job_Should_Be_Empty_String_When_Enviornment_Variable_Does_Not_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("CI_JOB_ID", null);
            var continuousIntegrationServer = new ContinuousIntegrationServer();

            // When
            var job = continuousIntegrationServer.Job;

            // Then
            job.Should().BeEmpty();
        }

        [Fact]
        public void Job_Should_Be_Set_When_Enviornment_Variable_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("CI_JOB_ID", "123");
            var continuousIntegrationServer = new ContinuousIntegrationServer();

            // When
            var job = continuousIntegrationServer.Job;

            // Then
            job.Should().Be("123");
        }

        [Fact]
        public void Pr_Should_Empty_String()
        {
            // Given
            var continuousIntegrationServer = new ContinuousIntegrationServer();

            // when
            var pr = continuousIntegrationServer.Pr;

            // Then
            pr.Should().BeEmpty();
        }

        [Fact]
        public void Service_Should_Empty_String()
        {
            // Given
            var continuousIntegrationServer = new ContinuousIntegrationServer();

            // When
            var service = continuousIntegrationServer.Service;

            // Then
            service.Should().BeEmpty();
        }

        [Fact]
        public void Slug_Should_Empty_String()
        {
            // Given
            var continuousIntegrationServer = new ContinuousIntegrationServer();

            // When
            var slug = continuousIntegrationServer.Slug;

            // Then
            slug.Should().BeEmpty();
        }

        [Fact]
        public void Tag_Should_Empty_String()
        {
            // Given
            var continuousIntegrationServer = new ContinuousIntegrationServer();

            // When
            var slug = continuousIntegrationServer.Tag;

            // Then
            slug.Should().BeEmpty();
        }
    }
}
