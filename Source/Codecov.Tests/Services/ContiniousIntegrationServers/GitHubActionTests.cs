using System;
using Codecov.Services.ContinuousIntegrationServers;
using FluentAssertions;
using Moq;
using Xunit;

namespace Codecov.Tests.Services.ContiniousIntegrationServers
{
    public class GitHubActionTests
    {
        [Fact]
        public void Branch_Should_Be_Empty_When_Environment_Variable_Does_Not_Exist()
        {
            // Given
            var ga = new Mock<GitHubAction>() { CallBase = true };
            ga.Setup(s => s.GetEnvironmentVariable("GITHUB_REF")).Returns(string.Empty);
            var githubAction = ga.Object;

            // When
            var branch = githubAction.Branch;

            // Then
            branch.Should().BeEmpty();
        }

        [Fact]
        public void Branch_Should_Be_Set_When_Enviornment_Variable_Exits()
        {
            // Given
            var ga = new Mock<GitHubAction>() { CallBase = true };
            ga.Setup(s => s.GetEnvironmentVariable("GITHUB_REF")).Returns("ref/heads/develop");
            var githubAction = ga.Object;

            // When
            var branch = githubAction.Branch;

            // Then
            branch.Should().Be("develop");
        }

        [Fact]
        public void Commit_Should_Be_Empty_String_When_Enviornment_Variable_Does_Not_Exits()
        {
            // Given
            var ga = new Mock<GitHubAction>() { CallBase = true };
            ga.Setup(s => s.GetEnvironmentVariable("GITHUB_SHA")).Returns(string.Empty);
            var githubAction = ga.Object;

            // When
            var commit = githubAction.Commit;

            // Then
            commit.Should().BeEmpty();
        }

        [Fact]
        public void Commit_Should_Be_Set_When_Enviornment_Variable_Exits()
        {
            // Given
            var ga = new Mock<GitHubAction>() { CallBase = true };
            ga.Setup(s => s.GetEnvironmentVariable("GITHUB_SHA")).Returns("123");
            var githubAction = ga.Object;

            // When
            var commit = githubAction.Commit;

            // Then
            commit.Should().Be("123");
        }

        [Theory, InlineData(null), InlineData(""), InlineData("False"), InlineData("false"), InlineData("foo")]
        public void Detecter_Should_Be_False_When_Actions_Environment_Variable_Does_Not_Exist_Or_Is_Not_True(string environmentData)
        {
            // Given
            var ga = new Mock<GitHubAction>() { CallBase = true };
            ga.Setup(s => s.GetEnvironmentVariable("GITHUB_ACTIONS")).Returns(environmentData);
            var githubAction = ga.Object;

            // When
            var detecter = githubAction.Detecter;

            // Then
            detecter.Should().BeFalse();
        }

        [Theory, InlineData("True"), InlineData("true")]
        public void Detecter_Should_Be_True_When_Actions_Environment_Variable_Exist_And_Is_True(string environmentData)
        {
            // Given
            var ga = new Mock<GitHubAction>() { CallBase = true };
            ga.Setup(s => s.GetEnvironmentVariable("GITHUB_ACTIONS")).Returns(environmentData);
            var githubAction = ga.Object;

            // When
            var detecter = githubAction.Detecter;

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
            var ga = new Mock<GitHubAction>() { CallBase = true };
            ga.Setup(s => s.GetEnvironmentVariable("GITHUB_REPOSITORY")).Returns(string.Empty);
            var githubAction = ga.Object;

            // When
            var slug = githubAction.Slug;

            // Then
            slug.Should().BeEmpty();
        }

        [Fact]
        public void Slug_Should_Be_Set_When_Environment_Variable_Exist()
        {
            // Given
            var ga = new Mock<GitHubAction>() { CallBase = true };
            ga.Setup(s => s.GetEnvironmentVariable("GITHUB_REPOSITORY")).Returns("foo/bar");
            var githubAction = ga.Object;

            // When
            var slug = githubAction.Slug;

            // Then
            slug.Should().Be("foo/bar");
        }
    }
}
