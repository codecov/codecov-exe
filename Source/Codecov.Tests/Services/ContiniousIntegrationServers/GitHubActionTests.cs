using System;
using Codecov.Services.ContinuousIntegrationServers;
using FluentAssertions;
using Xunit;

namespace Codecov.Tests.Services.ContiniousIntegrationServers
{
    public class GitHubActionTests : IDisposable
    {
        [Fact]
        public void Branch_Should_Be_Empty_When_Environment_Variable_Does_Not_Exist()
        {
            // Given
            Environment.SetEnvironmentVariable("GITHUB_REF", null);
            var githubAction = new GitHubAction();

            // When
            var branch = githubAction.Branch;

            // Then
            branch.Should().BeEmpty();
        }

        [Fact]
        public void Branch_Should_Be_Set_When_Enviornment_Variable_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("GITHUB_REF", "ref/heads/develop");
            var githubAction = new GitHubAction();

            // When
            var branch = githubAction.Branch;

            // Then
            branch.Should().Be("develop");
        }

        [Fact]
        public void Commit_Should_Be_Empty_String_When_Enviornment_Variable_Does_Not_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("GITHUB_SHA", null);
            var githubAction = new GitHubAction();

            // When
            var commit = githubAction.Commit;

            // Then
            commit.Should().BeEmpty();
        }

        [Fact]
        public void Commit_Should_Be_Set_When_Enviornment_Variable_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("GITHUB_SHA", "123");
            var githubAction = new GitHubAction();

            // When
            var commit = githubAction.Commit;

            // Then
            commit.Should().Be("123");
        }

        [Theory, InlineData(null), InlineData(""), InlineData("False"), InlineData("false"), InlineData("foo")]
        public void Detecter_Should_Be_False_When_Actions_Environment_Variable_Does_Not_Exist_Or_Is_Not_True(string environmentData)
        {
            // Given
            Environment.SetEnvironmentVariable("GITHUB_ACTIONS", environmentData);
            var githubActions = new GitHubAction();

            // When
            var detecter = githubActions.Detecter;

            // Then
            detecter.Should().BeFalse();
        }

        [Theory, InlineData("True"), InlineData("true")]
        public void Detecter_Should_Be_True_When_Actions_Environment_Variable_Exist_And_Is_True(string environmentData)
        {
            // Given
            Environment.SetEnvironmentVariable("GITHUB_ACTIONS", environmentData);
            var githubActions = new GitHubAction();

            // When
            var detecter = githubActions.Detecter;

            // Then
            detecter.Should().BeTrue();
        }

        [Fact]
        public void Service_Should_Be_Set_To_GitHubActions()
        {
            // Given
            var githubAction = new GitHubAction();

            // When
            var service = githubAction.Service;

            // Then
            service.Should().Be("github-actions");
        }

        [Fact]
        public void Slug_Should_Be_Empty_String_When_Environment_Variable_Does_Not_Exist()
        {
            // Given
            Environment.SetEnvironmentVariable("GITHUB_REPOSITORY", null);
            var githubAction = new GitHubAction();

            // When
            var slug = githubAction.Slug;

            // Then
            slug.Should().BeEmpty();
        }

        [Fact]
        public void Slug_Should_Be_Set_When_Environment_Variable_Exist()
        {
            // Given
            Environment.SetEnvironmentVariable("GITHUB_REPOSITORY", "foo/bar");
            var githubAction = new GitHubAction();

            // When
            var slug = githubAction.Slug;

            // Then
            slug.Should().Be("foo/bar");
        }

        public void Dispose()
        {
            // We will remove all environment variables that could have been set during unit test
            var envVariable = new[]
            {
                "GITHUB_REF",
                "GITHUB_SHA",
                "GITHUB_ACTIONS",
                "GITHUB_REPOSITORY"
            };

            foreach (var variable in envVariable)
            {
                Environment.SetEnvironmentVariable(variable, null);
            }
        }
    }
}
