using Codecov.Factories;
using Codecov.Services.ContinuousIntegrationServers;
using FluentAssertions;
using Moq;
using Xunit;

namespace Codecov.Tests.Factories
{
    public class ContinuousIntegrationServerFactoryTests
    {
        #region AppVeyor Detection

        [Fact]
        public void Create_ShouldNotDetectAppVeyorWhenAppveyorIsNull()
        {
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("CI")).Returns("True");

            var ci = ContinuousIntegrationServerFactory.Create(ev.Object);

            ci.Should().NotBeOfType<AppVeyor>().And.BeOfType<ContinuousIntegrationServer>();
        }

        [Fact]
        public void Create_ShouldNotDetectAppVeyorWhenAppveyorIsFalse()
        {
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("APPVEYOR")).Returns("False");
            ev.Setup(s => s.GetEnvironmentVariable("CI")).Returns("True");

            var ci = ContinuousIntegrationServerFactory.Create(ev.Object);

            ci.Should().NotBeOfType<AppVeyor>().And.BeOfType<ContinuousIntegrationServer>();
        }

        [Fact]
        public void Create_ShouldNotDetectAppVeyorWhenCiIsNull()
        {
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("APPVEYOR")).Returns("True");

            var ci = ContinuousIntegrationServerFactory.Create(ev.Object);

            ci.Should().NotBeOfType<AppVeyor>().And.BeOfType<ContinuousIntegrationServer>();
        }

        [Fact]
        public void Create_ShouldNotDetectAppVeyorWhenCiIsFalse()
        {
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("APPVEYOR")).Returns("True");
            ev.Setup(s => s.GetEnvironmentVariable("CI")).Returns("False");

            var ci = ContinuousIntegrationServerFactory.Create(ev.Object);

            ci.Should().NotBeOfType<AppVeyor>().And.BeOfType<ContinuousIntegrationServer>();
        }

        [Fact]
        public void Create_ShouldDetectAppVeyorWhenCiAndAppVeyorIsTrue()
        {
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("APPVEYOR")).Returns("True");
            ev.Setup(s => s.GetEnvironmentVariable("CI")).Returns("True");

            var ci = ContinuousIntegrationServerFactory.Create(ev.Object);

            ci.Should().BeOfType<AppVeyor>();
        }

        #endregion AppVeyor Detection

        #region Azure Pipelines Detection

        [Fact]
        public void Create_ShouldNotDetectAzurePipelinesWhenTfBuildIsNull()
        {
            var ev = new Mock<IEnviornmentVariables>();

            var ci = ContinuousIntegrationServerFactory.Create(ev.Object);

            ci.Should().NotBeOfType<AzurePipelines>().And.BeOfType<ContinuousIntegrationServer>();
        }

        [Fact]
        public void Create_ShouldNotDetectAzurePipelinesWhenTfBuildIsFalse()
        {
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("TF_BUILD")).Returns("False");

            var ci = ContinuousIntegrationServerFactory.Create(ev.Object);

            ci.Should().NotBeOfType<AzurePipelines>().And.BeOfType<ContinuousIntegrationServer>();
        }

        [Fact]
        public void Create_ShouldDetectAzurePipelinesWhenTfBuildIsTrue()
        {
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("TF_BUILD")).Returns("True");

            var ci = ContinuousIntegrationServerFactory.Create(ev.Object);

            ci.Should().BeOfType<AzurePipelines>();
        }

        #endregion Azure Pipelines Detection

        #region GitHub Action Detection

        [Fact]
        public void Create_ShouldNotDetectGitHubActionWhenGitHubActionsAndGitHubActionIsNull()
        {
            var ev = new Mock<IEnviornmentVariables>();

            var ci = ContinuousIntegrationServerFactory.Create(ev.Object);

            ci.Should().NotBeOfType<GitHubAction>().And.BeOfType<ContinuousIntegrationServer>();
        }

        [Fact]
        public void Create_ShouldNotDetectGitHubActionWhenGitHubActionsIsFalse()
        {
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("GITHUB_ACTIONS")).Returns("False");

            var ci = ContinuousIntegrationServerFactory.Create(ev.Object);

            ci.Should().NotBeOfType<GitHubAction>().And.BeOfType<ContinuousIntegrationServer>();
        }

        [Fact]
        public void Create_ShouldDetectGitHubActionWhenGitHubActionsIsTrue()
        {
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("GITHUB_ACTIONS")).Returns("True");

            var ci = ContinuousIntegrationServerFactory.Create(ev.Object);

            ci.Should().BeOfType<GitHubAction>();
        }

        [Fact]
        public void Create_ShouldDetectGitHubActionWhenGitHubActionIsNotNull()
        {
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("GITHUB_ACTION")).Returns("Some-kind-of-value");

            var ci = ContinuousIntegrationServerFactory.Create(ev.Object);

            ci.Should().BeOfType<GitHubAction>();
        }

        #endregion GitHub Action Detection

        #region Jenkins Detection

        [Fact]
        public void Create_ShouldNotDetectJenkinsWhenJenkinsUrlIsNull()
        {
            var ev = new Mock<IEnviornmentVariables>();

            var ci = ContinuousIntegrationServerFactory.Create(ev.Object);

            ci.Should().NotBeOfType<Jenkins>().And.BeOfType<ContinuousIntegrationServer>();
        }

        [Fact]
        public void Create_ShouldDetectJenkinsWhenJenkinsUrlIsNotNull()
        {
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("JENKINS_URL")).Returns("https://example.org");

            var ci = ContinuousIntegrationServerFactory.Create(ev.Object);

            ci.Should().BeOfType<Jenkins>();
        }

        #endregion Jenkins Detection

        #region TeamCity Detection

        [Fact]
        public void Create_ShouldNotDetectTeamcityWhenTeamcityVersionIsNull()
        {
            var ev = new Mock<IEnviornmentVariables>();

            var ci = ContinuousIntegrationServerFactory.Create(ev.Object);

            ci.Should().NotBeOfType<TeamCity>().And.BeOfType<ContinuousIntegrationServer>();
        }

        [Fact]
        public void Create_ShouldDetectTeamcityWhenTeamcityVersionIsNotNull()
        {
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("TEAMCITY_VERSION")).Returns("1.0.0");

            var ci = ContinuousIntegrationServerFactory.Create(ev.Object);

            ci.Should().BeOfType<TeamCity>();
        }

        #endregion TeamCity Detection

        #region Travis Detection

        [Fact]
        public void Create_ShouldNotDetectTravisWhenTravisIsNull()
        {
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("CI")).Returns("True");

            var ci = ContinuousIntegrationServerFactory.Create(ev.Object);

            ci.Should().NotBeOfType<Travis>().And.BeOfType<ContinuousIntegrationServer>();
        }

        [Fact]
        public void Create_ShouldNotDetectTravisWhenTravisIsFalse()
        {
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("TRAVIS")).Returns("False");
            ev.Setup(s => s.GetEnvironmentVariable("CI")).Returns("True");

            var ci = ContinuousIntegrationServerFactory.Create(ev.Object);

            ci.Should().NotBeOfType<Travis>().And.BeOfType<ContinuousIntegrationServer>();
        }

        [Fact]
        public void Create_ShouldNotDetectTravisWhenCiIsNull()
        {
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("TRAVIS")).Returns("True");

            var ci = ContinuousIntegrationServerFactory.Create(ev.Object);

            ci.Should().NotBeOfType<Travis>().And.BeOfType<ContinuousIntegrationServer>();
        }

        [Fact]
        public void Create_ShouldNotDetectTravisWhenCiIsFalse()
        {
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("TRAVIS")).Returns("True");
            ev.Setup(s => s.GetEnvironmentVariable("CI")).Returns("False");

            var ci = ContinuousIntegrationServerFactory.Create(ev.Object);

            ci.Should().NotBeOfType<Travis>().And.BeOfType<ContinuousIntegrationServer>();
        }

        [Fact]
        public void Create_ShouldDetectTravisWhenCiAndTravisIsTrue()
        {
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("TRAVIS")).Returns("True");
            ev.Setup(s => s.GetEnvironmentVariable("CI")).Returns("True");

            var ci = ContinuousIntegrationServerFactory.Create(ev.Object);

            ci.Should().BeOfType<Travis>();
        }

        #endregion Travis Detection
    }
}
