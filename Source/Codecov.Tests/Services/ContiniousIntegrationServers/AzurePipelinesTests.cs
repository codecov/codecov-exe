using System.Collections.Generic;
using Codecov.Services.ContinuousIntegrationServers;
using FluentAssertions;
using Moq;
using Xunit;

namespace Codecov.Tests.Services.ContiniousIntegrationServers
{
    public class AzurePipelinesTests
    {
        public static IEnumerable<object[]> Build_Url_Empty_Data
        {
            get
            {
                var possibleDomains = new[] { null, string.Empty, "https://dev.azure.com/", "http://localhost:5234/" };
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
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("SYSTEM_PULLREQUEST_TARGETBRANCH")).Returns(string.Empty);
            ev.Setup(s => s.GetEnvironmentVariable("BUILD_SOURCEBRANCHNAME")).Returns(string.Empty);
            var pipelines = new AzurePipelines(ev.Object);

            // When
            var branch = pipelines.Branch;

            // Then
            branch.Should().BeEmpty();
        }

        [Fact]
        public void Branch_Should_Be_Set_When_PR_Enviornment_Variable_Exits()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("SYSTEM_PULLREQUEST_TARGETBRANCH")).Returns("develop");
            ev.Setup(s => s.GetEnvironmentVariable("BUILD_SOURCEBRANCHNAME")).Returns(string.Empty);
            var pipelines = new AzurePipelines(ev.Object);

            // When
            var branch = pipelines.Branch;

            // Then
            branch.Should().Be("develop");
        }

        [Fact]
        public void Branch_Should_Be_Set_When_Branch_Enviornment_Variable_Exits()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("SYSTEM_PULLREQUEST_TARGETBRANCH")).Returns(string.Empty);
            ev.Setup(s => s.GetEnvironmentVariable("BUILD_SOURCEBRANCHNAME")).Returns("develop");
            var pipelines = new AzurePipelines(ev.Object);

            // When
            var branch = pipelines.Branch;

            // Then
            branch.Should().Be("develop");
        }

        [Fact]
        public void Branch_Should_Prefer_Pull_Request()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("SYSTEM_PULLREQUEST_TARGETBRANCH")).Returns("pr");
            ev.Setup(s => s.GetEnvironmentVariable("BUILD_SOURCEBRANCHNAME")).Returns("master");
            var pipelines = new AzurePipelines(ev.Object);

            // When
            var branch = pipelines.Branch;

            // Then
            branch.Should().Be("pr");
        }

        [Fact]
        public void Build_Should_Be_Empty_String_When_Enviornment_Variable_Does_Not_Exits()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("BUILD_BUILDNUMBER")).Returns(string.Empty);
            var pipelines = new AzurePipelines(ev.Object);

            // When
            var build = pipelines.Build;

            // Then
            build.Should().BeEmpty();
        }

        [Fact]
        public void Build_Should_Be_Set_When_Enviornment_Variable_Exits()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("BUILD_BUILDNUMBER")).Returns("123");
            var pipelines = new AzurePipelines(ev.Object);

            // When
            var build = pipelines.Build;

            // Then
            build.Should().Be("123");
        }

        [Fact]
        public void Commit_Should_Be_Empty_String_When_Enviornment_Variable_Does_Not_Exits()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("BUILD_SOURCEVERSION")).Returns(string.Empty);
            var pipelines = new AzurePipelines(ev.Object);

            // When
            var commit = pipelines.Commit;

            // Then
            commit.Should().BeEmpty();
        }

        [Fact]
        public void Commit_Should_Be_Set_When_Enviornment_Variable_Exits()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("BUILD_SOURCEVERSION")).Returns("123");
            var pipelines = new AzurePipelines(ev.Object);

            // When
            var commit = pipelines.Commit;

            // Then
            commit.Should().Be("123");
        }

        [Theory, InlineData(null), InlineData(""), InlineData("False"), InlineData("foo")]
        public void Detecter_Should_Be_False_When_TfBuild_Enviornment_Variable_Does_Not_Exit(string pipelinesData)
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("TF_BUILD")).Returns(pipelinesData);
            var pipelines = new AzurePipelines(ev.Object);

            // When
            var detecter = pipelines.Detecter;

            // Then
            detecter.Should().BeFalse();
        }

        [Theory, MemberData(nameof(Build_Url_Empty_Data))]
        public void BuildUrl_Should_Be_Empty_String_When_Environment_Variables_Do_Not_Exist(string serverUrl, string project, string build)
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("SYSTEM_TEAMFOUNDATIONSERVERURI")).Returns(serverUrl);
            ev.Setup(s => s.GetEnvironmentVariable("SYSTEM_TEAMPROJECT")).Returns(project);
            ev.Setup(s => s.GetEnvironmentVariable("BUILD_BUILDID")).Returns(build);
            var pipelines = new AzurePipelines(ev.Object);

            // When
            var buildUrl = pipelines.BuildUrl;

            // Then
            buildUrl.Should().BeEmpty();
        }

        [Fact]
        public void BuildUrl_Should_Not_Empty_String_When_Environment_Variable_Exists()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("SYSTEM_TEAMFOUNDATIONSERVERURI")).Returns("https://dev.azure.com/");
            ev.Setup(s => s.GetEnvironmentVariable("SYSTEM_TEAMPROJECT")).Returns("project");
            ev.Setup(s => s.GetEnvironmentVariable("BUILD_BUILDID")).Returns("build");
            var pipelines = new AzurePipelines(ev.Object);

            // When
            var buildUrl = pipelines.BuildUrl;

            // Then
            buildUrl.Should().Be("https://dev.azure.com/project/_build/results?buildId=build");
        }

        [Theory, InlineData("http://"), InlineData("http://."), InlineData("http://.."), InlineData("http://../"), InlineData("http://?"), InlineData("http://??"), InlineData("http://#"), InlineData("http://##"), InlineData("//"), InlineData("//a"), InlineData("///a"), InlineData("///"), InlineData("foo.com"), InlineData("rdar://1234"), InlineData("h://test"), InlineData("http:// shouldfail.com"), InlineData(":// should fail"), InlineData("ftps://foo.bar/"), InlineData("http://.www.foo.bar/")]
        public void BuildUrl_Should_Be_Empty_When_Appveyor_Url_Is_Invalid_Domain(string urlData)
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("SYSTEM_TEAMFOUNDATIONSERVERURI")).Returns(urlData);
            ev.Setup(s => s.GetEnvironmentVariable("SYSTEM_TEAMPROJECT")).Returns("project");
            ev.Setup(s => s.GetEnvironmentVariable("BUILD_BUILDID")).Returns("build");
            var pipelines = new AzurePipelines(ev.Object);

            // When
            var buildUrl = pipelines.BuildUrl;

            // Then
            buildUrl.Should().BeEmpty();
        }

        [Theory, InlineData("", ""), InlineData("foo", "")]
        public void Job_Should_Be_Empty_String_When_Enviornment_Variables_Do_Not_Exit(string slugData, string versionData)
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("BUILD_REPOSITORY_NAME")).Returns(slugData);
            ev.Setup(s => s.GetEnvironmentVariable("BUILD_BUILDID")).Returns(versionData);
            var pipelines = new AzurePipelines(ev.Object);

            // When
            var job = pipelines.Job;

            // Then
            job.Should().BeEmpty();
        }

        [Fact]
        public void Job_Should_Not_Be_Empty_String_When_Enviornment_Variables_Exit()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("BUILD_REPOSITORY_NAME")).Returns("foo/bar");
            ev.Setup(s => s.GetEnvironmentVariable("BUILD_BUILDID")).Returns("bang");
            var pipelines = new AzurePipelines(ev.Object);

            // When
            var job = pipelines.Job;

            // Then
            job.Should().Be("bang");
        }

        [Fact]
        public void Pr_Should_Be_Empty_String_When_Enviornment_Variable_Does_Not_Exits()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("SYSTEM_PULLREQUEST_PULLREQUESTNUMBER")).Returns(string.Empty);
            var pipelines = new AzurePipelines(ev.Object);

            // When
            var pr = pipelines.Pr;

            // Then
            pr.Should().BeEmpty();
        }

        [Fact]
        public void Pr_Should_Be_Set_When_Enviornment_Variable_Exits()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("SYSTEM_PULLREQUEST_PULLREQUESTNUMBER")).Returns("123");
            var pipelines = new AzurePipelines(ev.Object);

            // When
            var pr = pipelines.Pr;

            // Then
            pr.Should().Be("123");
        }

        [Fact]
        public void Project_Should_Be_Empty_String_When_Enviornment_Variable_Does_Not_Exits()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("SYSTEM_TEAMPROJECT")).Returns(string.Empty);
            var pipelines = new AzurePipelines(ev.Object);

            // When
            var project = pipelines.Project;

            // Then
            project.Should().BeEmpty();
        }

        [Fact]
        public void Project_Should_Be_Set_When_Enviornment_Variable_Exits()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("SYSTEM_TEAMPROJECT")).Returns("123");
            var pipelines = new AzurePipelines(ev.Object);

            // When
            var project = pipelines.Project;

            // Then
            project.Should().Be("123");
        }

        [Fact]
        public void ServerUri_Should_Be_Empty_String_When_Enviornment_Variable_Does_Not_Exits()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("SYSTEM_TEAMFOUNDATIONSERVERURI")).Returns(string.Empty);
            var pipelines = new AzurePipelines(ev.Object);

            // When
            var serverUri = pipelines.ServerUri;

            // Then
            serverUri.Should().BeEmpty();
        }

        [Fact]
        public void ServerUri_Should_Be_Set_When_Enviornment_Variable_Exits()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("SYSTEM_TEAMFOUNDATIONSERVERURI")).Returns("123");
            var pipelines = new AzurePipelines(ev.Object);

            // When
            var serverUri = pipelines.ServerUri;

            // Then
            serverUri.Should().Be("123");
        }

        [Fact]
        public void Service_Should_Be_Set_To_AzurePipelines()
        {
            // Given
            var pipelines = new AzurePipelines(new Mock<IEnviornmentVariables>().Object);

            // When
            var service = pipelines.Service;

            // Then
            service.Should().Be("azure_pipelines");
        }

        [Fact]
        public void Slug_Should_Be_Empty_String_When_Enviornment_Variable_Does_Not_Exits()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("BUILD_REPOSITORY_NAME")).Returns(string.Empty);
            var pipelines = new AzurePipelines(ev.Object);

            // When
            var slug = pipelines.Slug;

            // Then
            slug.Should().BeEmpty();
        }

        [Fact]
        public void Slug_Should_Be_Set_When_Enviornment_Variable_Exits()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("BUILD_REPOSITORY_NAME")).Returns("foo/bar");
            var pipelines = new AzurePipelines(ev.Object);

            // When
            var slug = pipelines.Slug;

            // Then
            slug.Should().Be("foo/bar");
        }
    }
}
