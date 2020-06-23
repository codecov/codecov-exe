using Codecov.Services.ContinuousIntegrationServers;
using FluentAssertions;
using Moq;
using Xunit;

namespace Codecov.Tests.Services.ContiniousIntegrationServers
{
    public class JenkinsTests
    {
        [Fact]
        public void Branch_Should_Be_Empty_String_When_Enviornment_Variable_Does_Not_Exist()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            var jenkins = new Jenkins(ev.Object);

            // When
            var branch = jenkins.Branch;

            // Then
            branch.Should().BeEmpty();
        }

        [Theory]
        [InlineData("ghprbSourceBranch", "123")]
        [InlineData("GIT_BRANCH", "456")]
        [InlineData("BRANCH_NAME", "789")]
        public void Branch_Should_Be_Set_When_Enviornment_Variable_Exist(string key, string value)
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable(key)).Returns(value);
            var jenkins = new Jenkins(ev.Object);

            // When
            var branch = jenkins.Branch;

            // Then
            branch.Should().Be(value);
        }

        [Fact]
        public void Build_Should_Be_Empty_String_When_Environment_Variables_Do_Not_Exist()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            var jenkins = new Jenkins(ev.Object);

            // When
            var build = jenkins.Build;

            // Then
            build.Should().BeEmpty();
        }

        [Fact]
        public void Build_Should_Not_Be_Empty_String_When_Environment_Variable_Exists()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("BUILD_NUMBER")).Returns("111");
            var jenkins = new Jenkins(ev.Object);

            // When
            var build = jenkins.Build;

            // Then
            build.Should().Be("111");
        }

        [Fact]
        public void BuildUrl_Should_Be_Empty_String_When_Environment_Variables_Do_Not_Exist()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            var jenkins = new Jenkins(ev.Object);

            // When
            var buildUrl = jenkins.BuildUrl;

            // Then
            buildUrl.Should().BeEmpty();
        }

        [Fact]
        public void BuildUrl_Should_Not_Be_Empty_String_When_Environment_Variable_Exist()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("BUILD_URL")).Returns("https://jenkins.yourcompany.com/some-job/1");
            var jenkins = new Jenkins(ev.Object);

            // When
            var buildUrl = jenkins.BuildUrl;

            // Then
            buildUrl.Should().Be("https://jenkins.yourcompany.com/some-job/1");
        }

        [Fact]
        public void Commit_Should_Be_Empty_String_When_Environment_Variable_Does_Not_Exist()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            var jenkins = new Jenkins(ev.Object);

            // When
            var commit = jenkins.Commit;

            // Then
            commit.Should().BeEmpty();
        }

        [Theory]
        [InlineData("ghprbActualCommit", "123")]
        [InlineData("GIT_COMMIT", "456")]
        public void Commit_Should_Be_Set_When_Environment_Variable_Exists(string key, string pullId)
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable(key)).Returns(pullId);
            var jenkins = new Jenkins(ev.Object);

            // When
            var commit = jenkins.Commit;

            // Then
            commit.Should().Be(pullId);
        }

        [Fact]
        public void Detecter_Should_Be_False_When_Jenkins_Environment_Variable_Does_Not_Exist()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            var jenkins = new Jenkins(ev.Object);

            // When
            var detecter = jenkins.Detecter;

            // Then
            detecter.Should().BeFalse();
        }

        [Fact]
        public void Detecter_Should_Be_True_When_Jenkins_Environment_Variable_Exists()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("JENKINS_URL")).Returns("https://jenkins.yourcompany.com");
            var jenkins = new Jenkins(ev.Object);

            // When
            var detecter = jenkins.Detecter;

            // Then
            detecter.Should().BeTrue();
        }

        [Fact]
        public void Pr_Should_Be_Empty_String_When_Environment_Variable_Does_Not_Exist()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            var jenkins = new Jenkins(ev.Object);

            // When
            var pr = jenkins.Pr;

            // Then
            pr.Should().BeEmpty();
        }

        [Theory]
        [InlineData("ghprbPullId", "123")]
        [InlineData("CHANGE_ID", "456")]
        public void Pr_Should_Be_Set_When_Environment_Variable_Exists(string key, string pullId)
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable(key)).Returns(pullId);
            var jenkins = new Jenkins(ev.Object);

            // When
            var pr = jenkins.Pr;

            // Then
            pr.Should().Be(pullId);
        }

        [Fact]
        public void Service_Name_Should_Be_Set()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            var jenkins = new Jenkins(ev.Object);

            // When
            var service = jenkins.Service;

            // Then
            service.Should().Be("jenkins");
        }
    }
}
