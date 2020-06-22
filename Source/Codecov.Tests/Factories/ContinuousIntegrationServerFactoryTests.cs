using System;
using Codecov.Factories;
using Codecov.Services.ContinuousIntegrationServers;
using FluentAssertions;
using Xunit;

namespace Codecov.Tests.Factories
{
    public class ContinuousIntegrationServerFactoryTests
    {
        public ContinuousIntegrationServerFactoryTests()
        {
            var environmentVariables = new[] {
                "CI",
                "APPVEYOR",
                "TF_BUILD",
                "GITHUB_ACTIONS",
                "GITHUB_ACTION",
                "JENKINS_URL",
                "TEAMCITY_VERSION",
                "TRAVIS"
            };

            foreach (var environmentVariable in environmentVariables)
            {
                Environment.SetEnvironmentVariable(environmentVariable, null, EnvironmentVariableTarget.Process);
            }
        }

        #region AppVeyor Detection

        [Fact]
        public void Create_ShouldNotDetectAppVeyorWhenAppveyorIsNull()
        {
            Environment.SetEnvironmentVariable("APPVEYOR", "True");

            var ci = ContinuousIntegrationServerFactory.Create();

            ci.Should().NotBeOfType<AppVeyor>().And.BeOfType<ContinuousIntegrationServer>();
        }

        [Fact]
        public void Create_ShouldNotDetectAppVeyorWhenAppveyorIsFalse()
        {
            Environment.SetEnvironmentVariable("APPVEYOR", "False");

            var ci = ContinuousIntegrationServerFactory.Create();

            ci.Should().NotBeOfType<AppVeyor>().And.BeOfType<ContinuousIntegrationServer>();
        }

        [Fact]
        public void Create_ShouldNotDetectAppVeyorWhenCiIsNull()
        {
            Environment.SetEnvironmentVariable("CI", "True");

            var ci = ContinuousIntegrationServerFactory.Create();

            ci.Should().NotBeOfType<AppVeyor>().And.BeOfType<ContinuousIntegrationServer>();
        }

        [Fact]
        public void Create_ShouldNotDetectAppVeyorWhenCiIsFalse()
        {
            Environment.SetEnvironmentVariable("CI", "False");

            var ci = ContinuousIntegrationServerFactory.Create();

            ci.Should().NotBeOfType<AppVeyor>().And.BeOfType<ContinuousIntegrationServer>();
        }

        [Fact]
        public void Create_ShouldDetectAppVeyorWhenCiAndAppVeyorIsTrue()
        {
            Environment.SetEnvironmentVariable("CI", "True");
            Environment.SetEnvironmentVariable("APPVEYOR", "True");

            var ci = ContinuousIntegrationServerFactory.Create();

            ci.Should().BeOfType<AppVeyor>();
        }

        #endregion AppVeyor Detection

        #region Azure Pipelines Detection

        [Fact]
        public void Create_ShouldNotDetectAzurePipelinesWhenTfBuildIsNull()
        {
            var ci = ContinuousIntegrationServerFactory.Create();

            ci.Should().NotBeOfType<AzurePipelines>().And.BeOfType<ContinuousIntegrationServer>();
        }

        [Fact]
        public void Create_ShouldNotDetectAzurePipelinesWhenTfBuildIsFalse()
        {
            Environment.SetEnvironmentVariable("TF_BUILD", "False");

            var ci = ContinuousIntegrationServerFactory.Create();

            ci.Should().NotBeOfType<AzurePipelines>().And.BeOfType<ContinuousIntegrationServer>();
        }

        [Fact]
        public void Create_ShouldDetectAzurePipelinesWhenTfBuildIsTrue()
        {
            Environment.SetEnvironmentVariable("TF_BUILD", "True");

            var ci = ContinuousIntegrationServerFactory.Create();

            ci.Should().BeOfType<AzurePipelines>();
        }

        #endregion Azure Pipelines Detection

        #region GitHub Action Detection

        [Fact]
        public void Create_ShouldNotDetectGitHubActionWhenGitHubActionsAndGitHubActionIsNull()
        {
            var ci = ContinuousIntegrationServerFactory.Create();

            ci.Should().NotBeOfType<GitHubAction>().And.BeOfType<ContinuousIntegrationServer>();
        }

        [Fact]
        public void Create_ShouldNotDetectGitHubActionWhenGitHubActionsIsFalse()
        {
            Environment.SetEnvironmentVariable("GITHUB_ACTIONS", "False");

            var ci = ContinuousIntegrationServerFactory.Create();

            ci.Should().NotBeOfType<GitHubAction>().And.BeOfType<ContinuousIntegrationServer>();
        }

        [Fact]
        public void Create_ShouldDetectGitHubActionWhenGitHubActionsIsTrue()
        {
            Environment.SetEnvironmentVariable("GITHUB_ACTIONS", "True");

            var ci = ContinuousIntegrationServerFactory.Create();

            ci.Should().BeOfType<GitHubAction>();
        }

        [Fact]
        public void Create_ShouldDetectGitHubActionWhenGitHubActionIsNotNull()
        {
            Environment.SetEnvironmentVariable("GITHUB_ACTION", "Some-Kind-Of-Value");

            var ci = ContinuousIntegrationServerFactory.Create();

            ci.Should().BeOfType<GitHubAction>();
        }

        #endregion GitHub Action Detection

        #region Jenkins Detection

        [Fact]
        public void Create_ShouldNotDetectJenkinsWhenJenkinsUrlIsNull()
        {
            var ci = ContinuousIntegrationServerFactory.Create();

            ci.Should().NotBeOfType<Jenkins>().And.BeOfType<ContinuousIntegrationServer>();
        }

        [Fact]
        public void Create_ShouldDetectJenkinsWhenJenkinsUrlIsNotNull()
        {
            Environment.SetEnvironmentVariable("JENKINS_URL", "https://example.org");

            var ci = ContinuousIntegrationServerFactory.Create();

            ci.Should().BeOfType<Jenkins>();
        }

        #endregion Jenkins Detection

        #region TeamCity Detection

        [Fact]
        public void Create_ShouldNotDetectTeamcityWhenTeamcityVersionIsNull()
        {
            var ci = ContinuousIntegrationServerFactory.Create();

            ci.Should().NotBeOfType<TeamCity>().And.BeOfType<ContinuousIntegrationServer>();
        }

        [Fact]
        public void Create_ShouldDetectTeamcityWhenTeamcityVersionIsNotNull()
        {
            Environment.SetEnvironmentVariable("TEAMCITY_VERSION", "1.0.0");

            var ci = ContinuousIntegrationServerFactory.Create();

            ci.Should().BeOfType<TeamCity>();
        }

        #endregion TeamCity Detection

        #region Travis Detection

        [Fact]
        public void Create_ShouldNotDetectTravisWhenTravisIsNull()
        {
            Environment.SetEnvironmentVariable("TRAVIS", "True");

            var ci = ContinuousIntegrationServerFactory.Create();

            ci.Should().NotBeOfType<Travis>().And.BeOfType<ContinuousIntegrationServer>();
        }

        [Fact]
        public void Create_ShouldNotDetectTravisWhenTravisIsFalse()
        {
            Environment.SetEnvironmentVariable("TRAVIS", "False");

            var ci = ContinuousIntegrationServerFactory.Create();

            ci.Should().NotBeOfType<Travis>().And.BeOfType<ContinuousIntegrationServer>();
        }

        [Fact]
        public void Create_ShouldNotDetectTravisWhenCiIsNull()
        {
            Environment.SetEnvironmentVariable("CI", "True");

            var ci = ContinuousIntegrationServerFactory.Create();

            ci.Should().NotBeOfType<Travis>().And.BeOfType<ContinuousIntegrationServer>();
        }

        [Fact]
        public void Create_ShouldNotDetectTravisWhenCiIsFalse()
        {
            Environment.SetEnvironmentVariable("CI", "False");

            var ci = ContinuousIntegrationServerFactory.Create();

            ci.Should().NotBeOfType<Travis>().And.BeOfType<ContinuousIntegrationServer>();
        }

        [Fact]
        public void Create_ShouldDetectTravisWhenCiAndTravisIsTrue()
        {
            Environment.SetEnvironmentVariable("CI", "True");
            Environment.SetEnvironmentVariable("TRAVIS", "True");

            var ci = ContinuousIntegrationServerFactory.Create();

            ci.Should().BeOfType<Travis>();
        }

        #endregion Travis Detection
    }
}
