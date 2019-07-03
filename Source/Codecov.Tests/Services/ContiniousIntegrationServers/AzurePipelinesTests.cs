using System;
using Codecov.Services.ContinuousIntegrationServers;
using FluentAssertions;
using Xunit;

namespace Codecov.Tests.Services.ContiniousIntegrationServers
{
    public class AzurePipelinesTests
    {
        [Fact]
        public void Branch_Should_Be_Empty_String_When_Environment_Variable_Does_Not_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("BUILD_SOURCEBRANCHNAME", null);
            var pipelines = new AzurePipelines();

            // When
            var branch = pipelines.Branch;

            // Then
            branch.Should().BeEmpty();
        }

        [Fact]
        public void Branch_Should_Be_Set_When_Environment_Variable_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("BUILD_SOURCEBRANCHNAME", "develop");
            var pipelines = new AzurePipelines();

            // When
            var branch = pipelines.Branch;

            // Then
            branch.Should().Be("develop");
        }

        [Fact]
        public void Build_Should_Be_Empty_String_When_Environment_Variable_Does_Not_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("BUILD_BUILDID", null);
            var pipelines = new AzurePipelines();

            // When
            var build = pipelines.Build;

            // Then
            build.Should().BeEmpty();
        }

        [Fact]
        public void Build_Should_Be_Set_When_Environment_Variable_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("BUILD_BUILDID", "123");
            var pipelines = new AzurePipelines();

            // When
            var build = pipelines.Build;

            // Then
            build.Should().Be("123");
        }

        [Fact]
        public void Commit_Should_Be_Empty_String_When_Environment_Variable_Does_Not_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("BUILD_SOURCEVERSION", null);
            var pipelines = new AzurePipelines();

            // When
            var commit = pipelines.Commit;

            // Then
            commit.Should().BeEmpty();
        }

        [Fact]
        public void Commit_Should_Be_Set_When_Environment_Variable_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("BUILD_SOURCEVERSION", "123");
            var pipelines = new AzurePipelines();

            // When
            var commit = pipelines.Commit;

            // Then
            commit.Should().Be("123");
        }

        [Theory, InlineData(null), InlineData(""), InlineData("False"), InlineData("foo")]
        public void Detecter_Should_Be_False_When_TfBuild_Environment_Variable_Does_Not_Exit(string pipelinesData)
        {
            // Given
            Environment.SetEnvironmentVariable("TF_BUILD", pipelinesData);
            var pipelines = new AzurePipelines();

            // When
            var detecter = pipelines.Detecter;

            // Then
            detecter.Should().BeFalse();
        }

        [Theory, InlineData(null, null), InlineData("", ""), InlineData("foo", ""), InlineData("", "foo")]
        public void Job_Should_Be_Empty_String_When_Environment_Variables_Do_Not_Exit(string slugData, string versionData)
        {
            // Given
            Environment.SetEnvironmentVariable("BUILD_REPOSITORY_NAME", slugData);
            Environment.SetEnvironmentVariable("BUILD_BUILDNUMBER", versionData);

            var pipelines = new AzurePipelines();

            // When
            var job = pipelines.Job;

            // Then
            job.Should().BeEmpty();
        }

        [Fact]
        public void Job_Should_Not_Be_Empty_String_When_Environment_Variables_Exit()
        {
            // Given
            Environment.SetEnvironmentVariable("BUILD_REPOSITORY_NAME", "foo/bar");
            Environment.SetEnvironmentVariable("BUILD_BUILDNUMBER", "bang");
            var pipelines = new AzurePipelines();

            // When
            var job = pipelines.Job;

            // Then
            job.Should().Be("foo/bar/bang");
        }

        [Fact]
        public void Pr_Should_Be_Empty_String_When_Environment_Variable_Does_Not_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("SYSTEM_PULLREQUEST_PULLREQUESTNUMBER", null);
            var pipelines = new AzurePipelines();

            // When
            var pr = pipelines.Pr;

            // Then
            pr.Should().BeEmpty();
        }

        [Fact]
        public void Pr_Should_Be_Set_When_Environment_Variable_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("SYSTEM_PULLREQUEST_PULLREQUESTNUMBER", "123");
            var pipelines = new AzurePipelines();

            // When
            var pr = pipelines.Pr;

            // Then
            pr.Should().Be("123");
        }

        [Fact]
        public void Slug_Should_Be_Empty_String_When_Environment_Variable_Does_Not_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("BUILD_REPOSITORY_NAME", null);
            var pipelines = new AzurePipelines();

            // When
            var slug = pipelines.Slug;

            // Then
            slug.Should().BeEmpty();
        }

        [Fact]
        public void Slug_Should_Be_Set_When_Environment_Variable_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("BUILD_REPOSITORY_NAME", "foo/bar");
            var pipelines = new AzurePipelines();

            // When
            var slug = pipelines.Slug;

            // Then
            slug.Should().Be("foo/bar");
        }
    }
}
