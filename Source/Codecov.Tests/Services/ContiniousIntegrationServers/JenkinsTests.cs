using System;
using System.Collections.Generic;
using System.Text;
using Codecov.Services.ContinuousIntegrationServers;
using FluentAssertions;
using Xunit;

namespace Codecov.Tests.Services.ContiniousIntegrationServers
{
    public class JenkinsTests : IDisposable
    {
        [Fact]
        public void Branch_Should_Be_Empty_String_When_Enviornment_Variable_Does_Not_Exist()
        {
            // Given
            Environment.SetEnvironmentVariable("ghprbSourceBranch", null);
            Environment.SetEnvironmentVariable("GIT_BRANCH", null);
            Environment.SetEnvironmentVariable("BRANCH_NAME", null);
            var jenkins = new Jenkins();

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
            Environment.SetEnvironmentVariable(key, value);
            var jenkins = new Jenkins();

            // When
            var branch = jenkins.Branch;

            // Then
            branch.Should().Be(value);
        }

        [Fact]
        public void Build_Should_Be_Empty_String_When_Environment_Variables_Do_Not_Exist()
        {
            // Given
            Environment.SetEnvironmentVariable("BUILD_NUMBER", null);

            var jenkins = new Jenkins();

            // When
            var build = jenkins.Build;

            // Then
            build.Should().BeEmpty();
        }

        [Fact]
        public void Build_Should_Not_Be_Empty_String_When_Environment_Variable_Exists()
        {
            // Given
            Environment.SetEnvironmentVariable("BUILD_NUMBER", "111");

            var jenkins = new Jenkins();

            // When
            var build = jenkins.Build;

            // Then
            build.Should().Be("111");
        }

        [Fact]
        public void BuildUrl_Should_Be_Empty_String_When_Environment_Variables_Do_Not_Exist()
        {
            // Given
            Environment.SetEnvironmentVariable("BUILD_URL", null);

            var jenkins = new Jenkins();

            // When
            var buildUrl = jenkins.BuildUrl;

            // Then
            buildUrl.Should().BeEmpty();
        }

        [Fact]
        public void BuildUrl_Should_Not_Be_Empty_String_When_Environment_Variable_Exist()
        {
            // Given
            Environment.SetEnvironmentVariable("BUILD_URL", "https://jenkins.yourcompany.com/some-job/1");

            var jenkins = new Jenkins();

            // When
            var buildUrl = jenkins.BuildUrl;

            // Then
            buildUrl.Should().Be("https://jenkins.yourcompany.com/some-job/1");
        }

        [Fact]
        public void Commit_Should_Be_Empty_String_When_Environment_Variable_Does_Not_Exist()
        {
            // Given
            Environment.SetEnvironmentVariable("ghprbActualCommit", null);
            Environment.SetEnvironmentVariable("GIT_COMMIT", null);
            var jenkins = new Jenkins();

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
            Environment.SetEnvironmentVariable(key, pullId);
            var jenkins = new Jenkins();

            // When
            var commit = jenkins.Commit;

            // Then
            commit.Should().Be(pullId);
        }

        [Fact]
        public void Detecter_Should_Be_False_When_Jenkins_Environment_Variable_Does_Not_Exist()
        {
            // Given
            Environment.SetEnvironmentVariable("JENKINS_URL", null);
            var jenkins = new Jenkins();

            // When
            var detecter = jenkins.Detecter;

            // Then
            detecter.Should().BeFalse();
        }

        [Fact]
        public void Detecter_Should_Be_True_When_Jenkins_Environment_Variable_Exists()
        {
            // Given
            Environment.SetEnvironmentVariable("JENKINS_URL", "https://jenkins.yourcompany.com");
            var jenkins = new Jenkins();

            // When
            var detecter = jenkins.Detecter;

            // Then
            detecter.Should().BeTrue();
        }

        [Fact]
        public void Pr_Should_Be_Empty_String_When_Environment_Variable_Does_Not_Exist()
        {
            // Given
            Environment.SetEnvironmentVariable("ghprbPullId", null);
            Environment.SetEnvironmentVariable("CHANGE_ID", null);
            var jenkins = new Jenkins();

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
            Environment.SetEnvironmentVariable(key, pullId);
            var jenkins = new Jenkins();

            // When
            var pr = jenkins.Pr;

            // Then
            pr.Should().Be(pullId);
        }

        [Fact]
        public void Service_Name_Should_Be_Set()
        {
            // Given
            var jenkins = new Jenkins();

            // When
            var service = jenkins.Service;

            // Then
            service.Should().Be("jenkins");
        }

        public void Dispose()
        {
            // We will remove all environment variables that could have been set during unit test
            var envVariable = new[]
            {
                "ghprbSourceBranch",
                "GIT_BRANCH",
                "BRANCH_NAME",
                "BUILD_NUMBER",
                "BUILD_URL",
                "ghprbActualCommit",
                "GIT_COMMIT",
                "JENKINS_URL",
                "ghprbPullId",
                "CHANGE_ID",
            };

            foreach (var variable in envVariable)
            {
                Environment.SetEnvironmentVariable(variable, null);
            }
        }
    }
}
