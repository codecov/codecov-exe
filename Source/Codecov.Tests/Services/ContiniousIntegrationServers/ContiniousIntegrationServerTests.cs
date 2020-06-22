using Codecov.Services.ContinuousIntegrationServers;
using FluentAssertions;
using Moq;
using Xunit;

namespace Codecov.Tests.Services.ContiniousIntegrationServers
{
    public class ContiniousIntegrationServerTests
    {
        [Fact]
        public void Branch_Should_Be_Empty_String()
        {
            // Given
            var continuousIntegrationServer = new ContinuousIntegrationServer(new Mock<IEnviornmentVariables>().Object);

            // When
            var branch = continuousIntegrationServer.Branch;

            // Then
            branch.Should().BeEmpty();
        }

        [Fact]
        public void Build_Should_Be_Empty_String_When_Enviornment_Variable_Does_Not_Exits()
        {
            // Given
            var cis = new ContinuousIntegrationServer(new Mock<IEnviornmentVariables>().Object);

            // When
            var build = cis.Build;

            // Then
            build.Should().BeEmpty();
        }

        [Fact]
        public void Build_Should_Be_Set_When_Enviornment_Variable_Exits()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("CI_BUILD_ID")).Returns("123");
            var cis = new ContinuousIntegrationServer(ev.Object);

            // When
            var build = cis.Build;
            // Then
            build.Should().Be("123");
        }

        [Fact]
        public void BuildUrl_Should_Be_Empty_String_When_Enviornment_Variable_Does_Not_Exits()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            var cis = new ContinuousIntegrationServer(ev.Object);

            // When
            var buildUrl = cis.BuildUrl;

            // Then
            buildUrl.Should().BeEmpty();
        }

        [Fact]
        public void BuildUrl_Should_Be_Set_When_Enviornment_Variable_Exits()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("CI_BUILD_URL")).Returns("www.google.com");
            var cis = new ContinuousIntegrationServer(ev.Object);

            // When
            var buildUrl = cis.BuildUrl;
            // Then
            buildUrl.Should().Be("www.google.com");
        }

        [Fact]
        public void Commit_Should_Empty_String()
        {
            // Given
            var continuousIntegrationServer = new ContinuousIntegrationServer(new Mock<IEnviornmentVariables>().Object);

            // When
            var commit = continuousIntegrationServer.Commit;

            // Then
            commit.Should().BeEmpty();
        }

        [Fact]
        public void Detecter_Should_False()
        {
            // Given
            var continuousIntegrationServer = new ContinuousIntegrationServer(new Mock<IEnviornmentVariables>().Object);

            // when
            var detecter = continuousIntegrationServer.Detecter;

            // Then
            detecter.Should().BeFalse();
        }

        [Fact]
        public void GetEnviornmentVariables_Should_Empty_Dictionary()
        {
            // Given
            var continuousIntegrationServer = new ContinuousIntegrationServer(new Mock<IEnviornmentVariables>().Object);

            // When
            var enviornmentVariables = continuousIntegrationServer.UserEnvironmentVariables;

            // Then
            enviornmentVariables.Should().BeEmpty();
        }

        [Fact]
        public void Job_Should_Be_Empty_String_When_Enviornment_Variable_Does_Not_Exits()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("CI_JOB_ID")).Returns(string.Empty);
            var cis = new ContinuousIntegrationServer(ev.Object);

            // When
            var job = cis.Job;

            // Then
            job.Should().BeEmpty();
        }

        [Fact]
        public void Job_Should_Be_Set_When_Enviornment_Variable_Exits()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("CI_JOB_ID")).Returns("123");
            var cis = new ContinuousIntegrationServer(ev.Object);

            // When
            var job = cis.Job;

            // Then
            job.Should().Be("123");
        }

        [Fact]
        public void Pr_Should_Empty_String()
        {
            // Given
            var continuousIntegrationServer = new ContinuousIntegrationServer(new Mock<IEnviornmentVariables>().Object);

            // when
            var pr = continuousIntegrationServer.Pr;

            // Then
            pr.Should().BeEmpty();
        }

        [Fact]
        public void Service_Should_Empty_String()
        {
            // Given
            var continuousIntegrationServer = new ContinuousIntegrationServer(new Mock<IEnviornmentVariables>().Object);

            // When
            var service = continuousIntegrationServer.Service;

            // Then
            service.Should().BeEmpty();
        }

        [Fact]
        public void Slug_Should_Empty_String()
        {
            // Given
            var continuousIntegrationServer = new ContinuousIntegrationServer(new Mock<IEnviornmentVariables>().Object);

            // When
            var slug = continuousIntegrationServer.Slug;

            // Then
            slug.Should().BeEmpty();
        }

        [Fact]
        public void Tag_Should_Empty_String()
        {
            // Given
            var continuousIntegrationServer = new ContinuousIntegrationServer(new Mock<IEnviornmentVariables>().Object);

            // When
            var slug = continuousIntegrationServer.Tag;

            // Then
            slug.Should().BeEmpty();
        }
    }
}
