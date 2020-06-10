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
            ga.Setup(s => s.GetEnvironmentVariable("GITHUB_HEAD_REF")).Returns(string.Empty);
            var githubAction = ga.Object;

            // When
            var branch = githubAction.Branch;

            // Then
            branch.Should().BeEmpty();
        }

        [Fact]
        public void Branch_Should_Be_Set_From_Head_Ref_When_Environment_Variable_Exist()
        {
            // Given
            var ga = new Mock<GitHubAction>() { CallBase = true };
            ga.Setup(s => s.GetEnvironmentVariable("GITHUB_REF")).Returns("refs/pull/234/merge");
            ga.Setup(s => s.GetEnvironmentVariable("GITHUB_HEAD_REF")).Returns("develop");
            var githubAction = ga.Object;

            // When
            var branch = githubAction.Branch;

            // Then
            branch.Should().Be("develop");
        }

        [Fact]
        public void Branch_Should_Be_Set_When_Enviornment_Variable_Exits()
        {
            // Given
            var ga = new Mock<GitHubAction>() { CallBase = true };
            ga.Setup(s => s.GetEnvironmentVariable("GITHUB_REF")).Returns("refs/heads/develop");
            ga.Setup(s => s.GetEnvironmentVariable("GITHUB_HEAD_REF")).Returns(string.Empty);
            var githubAction = ga.Object;

            // When
            var branch = githubAction.Branch;

            // Then
            branch.Should().Be("develop");
        }

        [Fact]
        public void Build_Should_Be_Empty_When_Environment_Variable_Does_Not_Exist()
        {
            // Given
            var ga = new Mock<GitHubAction>() { CallBase = true };
            ga.Setup(s => s.GetEnvironmentVariable("GITHUB_RUN_ID")).Returns(string.Empty);
            var githubAction = ga.Object;

            // When
            var build = githubAction.Build;

            // Then
            build.Should().BeEmpty();
        }

        [Fact]
        public void Build_Should_Be_Set_When_Enviornment_Variable_Exits()
        {
            // Given
            var ga = new Mock<GitHubAction>() { CallBase = true };
            ga.Setup(s => s.GetEnvironmentVariable("GITHUB_RUN_ID")).Returns("32402849");
            var githubAction = ga.Object;

            // When
            var build = githubAction.Build;

            // Then
            build.Should().Be("32402849");
        }

        [Fact]
        public void BuildUrl_Should_Be_Empty_When_Build_Is_Empty()
        {
            // Given
            var ga = new Mock<GitHubAction>() { CallBase = true };
            ga.Setup(s => s.GetEnvironmentVariable("GITHUB_REPOSITORY")).Returns("codecov/codecov-exe");
            ga.Setup(s => s.GetEnvironmentVariable("GITHUB_RUN_ID")).Returns(string.Empty);
            var githubAction = ga.Object;

            // When
            var buildUrl = githubAction.BuildUrl;

            // Then
            buildUrl.Should().BeEmpty();
        }

        [Fact]
        public void BuildUrl_Should_Be_Empty_When_Slug_Is_Empty()
        {
            // Given
            var ga = new Mock<GitHubAction>() { CallBase = true };
            ga.Setup(s => s.GetEnvironmentVariable("GITHUB_REPOSITORY")).Returns(string.Empty);
            ga.Setup(s => s.GetEnvironmentVariable("GITHUB_RUN_ID")).Returns("some-id");
            var githubAction = ga.Object;

            // When
            var buildUrl = githubAction.BuildUrl;

            // Then
            buildUrl.Should().BeEmpty();
        }

        [Fact]
        public void BuildUrl_Should_Not_Be_Empty_When_Environment_Variables_Exist()
        {
            // Given
            var ga = new Mock<GitHubAction>() { CallBase = true };
            ga.Setup(s => s.GetEnvironmentVariable("GITHUB_REPOSITORY")).Returns("codecov/codecov-exe");
            ga.Setup(s => s.GetEnvironmentVariable("GITHUB_RUN_ID")).Returns("23432");
            var githubAction = ga.Object;

            // When
            var buildUrl = githubAction.BuildUrl;

            // Then
            buildUrl.Should().Be("https://github.com/codecov/codecov-exe/actions/runs/23432");
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

        [Fact]
        public void Detecter_Should_Be_False_When_Action_Environment_Variable_Is_Null_Or_Empty()
        {
            // Given
            var ga = new Mock<GitHubAction>() { CallBase = true };
            ga.Setup(s => s.GetEnvironmentVariable("GITHUB_ACTION")).Returns(string.Empty);
            ga.Setup(s => s.GetEnvironmentVariable("GITHUB_ACTIONS")).Returns(string.Empty);
            var githubAction = ga.Object;

            // When
            var detecter = githubAction.Detecter;

            // Then
            detecter.Should().BeFalse();
        }

        [Theory, InlineData(null), InlineData(""), InlineData("False"), InlineData("false"), InlineData("foo")]
        public void Detecter_Should_Be_False_When_Actions_And_Action_Environment_Variable_Does_Not_Exist_Or_Is_Not_True(string environmentData)
        {
            // Given
            var ga = new Mock<GitHubAction>() { CallBase = true };
            ga.Setup(s => s.GetEnvironmentVariable("GITHUB_ACTIONS")).Returns(environmentData);
            ga.Setup(s => s.GetEnvironmentVariable("GITHUB_ACTION")).Returns(string.Empty);
            var githubAction = ga.Object;

            // When
            var detecter = githubAction.Detecter;

            // Then
            detecter.Should().BeFalse();
        }

        [Fact]
        public void Detecter_Should_Be_True_When_Action_Environment_Variable_Exist_And_Is_Not_Empty()
        {
            // Given
            var ga = new Mock<GitHubAction>() { CallBase = true };
            ga.Setup(s => s.GetEnvironmentVariable("GITHUB_ACTION")).Returns("my-awesome-github-action");
            ga.Setup(s => s.GetEnvironmentVariable("GITHUB_ACTIONS")).Returns(string.Empty);
            var githubActions = ga.Object;

            // When
            var detecter = githubActions.Detecter;

            // Then
            detecter.Should().BeTrue();
        }

        [Theory, InlineData("True"), InlineData("true")]
        public void Detecter_Should_Be_True_When_Actions_Environment_Variable_Exist_And_Is_True(string environmentData)
        {
            // Given
            var ga = new Mock<GitHubAction>() { CallBase = true };
            ga.Setup(s => s.GetEnvironmentVariable("GITHUB_ACTIONS")).Returns(environmentData);
            ga.Setup(s => s.GetEnvironmentVariable("GITHUB_ACTION")).Returns(string.Empty);
            var githubAction = ga.Object;

            // When
            var detecter = githubAction.Detecter;

            // Then
            detecter.Should().BeTrue();
        }

        [Fact]
        public void PR_Should_Not_Be_Empty_When_Environment_Variables_Exist()
        {
            // Given
            var ga = new Mock<GitHubAction>() { CallBase = true };
            ga.Setup(s => s.GetEnvironmentVariable("GITHUB_HEAD_REF")).Returns("patch-2");
            ga.Setup(s => s.GetEnvironmentVariable("GITHUB_REF")).Returns("refs/pull/7/merge");
            var githubAction = ga.Object;

            // When
            var pr = githubAction.Pr;
            var branch = githubAction.Branch;

            // Then
            pr.Should().Be("7");
            branch.Should().Be("patch-2");
        }

        [Fact]
        public void PR_Should_Not_be_Set_If_Head_Ref_Is_Empyt()
        {
            // Given
            var ga = new Mock<GitHubAction>() { CallBase = true };
            ga.Setup(s => s.GetEnvironmentVariable("GITHUB_HEAD_REF")).Returns(string.Empty);
            var githubAction = ga.Object;

            // When
            var pr = githubAction.Pr;

            // THen
            pr.Should().BeEmpty();
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
