using System;
using System.Collections.Generic;
using Codecov.Services.ContinuousIntegrationServers;
using FluentAssertions;
using Xunit;

namespace Codecov.Tests.Services.ContiniousIntegrationServers
{
    public class AzurePipelinesTests
    {
        public static IEnumerable<object[]> Build_Url_Empty_Data
        {
            get
            {
                var possibleDomains = new[]{ null, string.Empty, "https://dev.azure.com/", "http://localhost:5234/" };
                var possibleDummies = new[] { null, string.Empty, "foo", "bar" };

                foreach (var domain in possibleDomains)
                {
                    foreach (var project in possibleDummies)
                    {
                        foreach (var build in possibleDummies)
                        {
                            if (string.IsNullOrEmpty(domain) || string.IsNullOrEmpty(project) || string.IsNullOrEmpty(build))
                            {
                                yield return new object[] { domain, project, build };
                            }
                        }
                    }
                }
            }
        }

        [Fact]
        public void Branch_Should_Be_Empty_String_When_Enviornment_Variable_Does_Not_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("SYSTEM_PULLREQUEST_TARGETBRANCH", null);
            Environment.SetEnvironmentVariable("BUILD_SOURCEBRANCHNAME", null);
            var pipelines = new AzurePipelines();

            // When
            var branch = pipelines.Branch;

            // Then
            branch.Should().BeEmpty();
        }

        [Fact]
        public void Branch_Should_Be_Set_When_PR_Enviornment_Variable_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("SYSTEM_PULLREQUEST_TARGETBRANCH", "develop");
            Environment.SetEnvironmentVariable("BUILD_SOURCEBRANCHNAME", null);
            var pipelines = new AzurePipelines();

            // When
            var branch = pipelines.Branch;

            // Then
            branch.Should().Be("develop");
        }

        [Fact]
        public void Branch_Should_Be_Set_When_Branch_Enviornment_Variable_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("SYSTEM_PULLREQUEST_TARGETBRANCH", null);
            Environment.SetEnvironmentVariable("BUILD_SOURCEBRANCHNAME", "develop");
            var pipelines = new AzurePipelines();

            // When
            var branch = pipelines.Branch;

            // Then
            branch.Should().Be("develop");
        }

        [Fact]
        public void Branch_Should_Prefer_Pull_Request()
        {
            // Given
            Environment.SetEnvironmentVariable("SYSTEM_PULLREQUEST_TARGETBRANCH", "pr");
            Environment.SetEnvironmentVariable("BUILD_SOURCEBRANCHNAME", "master");
            var pipelines = new AzurePipelines();

            // When
            var branch = pipelines.Branch;

            // Then
            branch.Should().Be("pr");
        }

        [Fact]
        public void Build_Should_Be_Empty_String_When_Enviornment_Variable_Does_Not_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("BUILD_BUILDNUMBER", null);
            var pipelines = new AzurePipelines();

            // When
            var build = pipelines.Build;

            // Then
            build.Should().BeEmpty();
        }

        [Fact]
        public void Build_Should_Be_Set_When_Enviornment_Variable_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("BUILD_BUILDNUMBER", "123");
            var pipelines = new AzurePipelines();

            // When
            var build = pipelines.Build;

            // Then
            build.Should().Be("123");
        }

        [Fact]
        public void Commit_Should_Be_Empty_String_When_Enviornment_Variable_Does_Not_Exits()
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
        public void Commit_Should_Be_Set_When_Enviornment_Variable_Exits()
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
        public void Detecter_Should_Be_False_When_TfBuild_Enviornment_Variable_Does_Not_Exit(string pipelinesData)
        {
            // Given
            Environment.SetEnvironmentVariable("TF_BUILD", pipelinesData);
            var pipelines = new AzurePipelines();

            // When
            var detecter = pipelines.Detecter;

            // Then
            detecter.Should().BeFalse();
        }

        [Theory, MemberData(nameof(Build_Url_Empty_Data))]
        public void BuildUrl_Should_Be_Empty_String_When_Environment_Variables_Do_Not_Exist(string serverUrl, string project, string build)
        {
            // Given
            Environment.SetEnvironmentVariable("SYSTEM_TEAMFOUNDATIONSERVERURI", serverUrl);
            Environment.SetEnvironmentVariable("SYSTEM_TEAMPROJECT", project);
            Environment.SetEnvironmentVariable("BUILD_BUILDID", build);

            var pipelines = new AzurePipelines();

            // When
            var buildUrl = pipelines.BuildUrl;

            // Then
            buildUrl.Should().BeEmpty();
        }

        [Fact]
        public void BuildUrl_Should_Not_Empty_String_When_Environment_Variable_Exists()
        {
            // Given
            Environment.SetEnvironmentVariable("SYSTEM_TEAMFOUNDATIONSERVERURI", "https://dev.azure.com/");
            Environment.SetEnvironmentVariable("SYSTEM_TEAMPROJECT", "project");
            Environment.SetEnvironmentVariable("BUILD_BUILDID", "build");

            var pipelines = new AzurePipelines();

            // When
            var buildUrl = pipelines.BuildUrl;

            // Then
            buildUrl.Should().Be("https://dev.azure.com/project/_build/results?buildId=build");
        }

        [Theory, InlineData("http://"), InlineData("http://."), InlineData("http://.."), InlineData("http://../"), InlineData("http://?"), InlineData("http://??"), InlineData("http://#"), InlineData("http://##"), InlineData("//"), InlineData("//a"), InlineData("///a"), InlineData("///"), InlineData("foo.com"), InlineData("rdar://1234"), InlineData("h://test"), InlineData("http:// shouldfail.com"), InlineData(":// should fail"), InlineData("ftps://foo.bar/"), InlineData("http://.www.foo.bar/")]
        public void BuildUrl_Should_Be_Empty_When_Appveyor_Url_Is_Invalid_Domain(string urlData)
        {
            // Given
            Environment.SetEnvironmentVariable("SYSTEM_TEAMFOUNDATIONSERVERURI", urlData);
            Environment.SetEnvironmentVariable("SYSTEM_TEAMPROJECT", "project");
            Environment.SetEnvironmentVariable("BUILD_BUILDID", "build");

            var pipelines = new AzurePipelines();

            // When
            var buildUrl = pipelines.BuildUrl;

            // Then
            buildUrl.Should().BeEmpty();
        }

        [Theory, InlineData(null, null), InlineData("", ""), InlineData("foo", "")]
        public void Job_Should_Be_Empty_String_When_Enviornment_Variables_Do_Not_Exit(string slugData, string versionData)
        {
            // Given
            Environment.SetEnvironmentVariable("BUILD_REPOSITORY_NAME", slugData);
            Environment.SetEnvironmentVariable("BUILD_BUILDID", versionData);

            var pipelines = new AzurePipelines();

            // When
            var job = pipelines.Job;

            // Then
            job.Should().BeEmpty();
        }

        [Fact]
        public void Job_Should_Not_Be_Empty_String_When_Enviornment_Variables_Exit()
        {
            // Given
            Environment.SetEnvironmentVariable("BUILD_REPOSITORY_NAME", "foo/bar");
            Environment.SetEnvironmentVariable("BUILD_BUILDID", "bang");
            var pipelines = new AzurePipelines();

            // When
            var job = pipelines.Job;

            // Then
            job.Should().Be("bang");
        }

        [Fact]
        public void Pr_Should_Be_Empty_String_When_Enviornment_Variable_Does_Not_Exits()
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
        public void Pr_Should_Be_Set_When_Enviornment_Variable_Exits()
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
        public void Project_Should_Be_Empty_String_When_Enviornment_Variable_Does_Not_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("SYSTEM_TEAMPROJECT", null);
            var pipelines = new AzurePipelines();

            // When
            var project = pipelines.Project;

            // Then
            project.Should().BeEmpty();
        }

        [Fact]
        public void Project_Should_Be_Set_When_Enviornment_Variable_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("SYSTEM_TEAMPROJECT", "123");
            var pipelines = new AzurePipelines();

            // When
            var project = pipelines.Project;

            // Then
            project.Should().Be("123");
        }

        [Fact]
        public void ServerUri_Should_Be_Empty_String_When_Enviornment_Variable_Does_Not_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("SYSTEM_TEAMFOUNDATIONSERVERURI", null);
            var pipelines = new AzurePipelines();

            // When
            var serverUri = pipelines.ServerUri;

            // Then
            serverUri.Should().BeEmpty();
        }

        [Fact]
        public void ServerUri_Should_Be_Set_When_Enviornment_Variable_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("SYSTEM_TEAMFOUNDATIONSERVERURI", "123");
            var pipelines = new AzurePipelines();

            // When
            var serverUri = pipelines.ServerUri;

            // Then
            serverUri.Should().Be("123");
        }

        [Fact]
        public void Service_Should_Be_Set_To_AzurePipelines()
        {
            // Given
            var pipelines = new AzurePipelines();

            // When
            var service = pipelines.Service;

            // Then
            service.Should().Be("azure_pipelines");
        }

        [Fact]
        public void Slug_Should_Be_Empty_String_When_Enviornment_Variable_Does_Not_Exits()
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
        public void Slug_Should_Be_Set_When_Enviornment_Variable_Exits()
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
