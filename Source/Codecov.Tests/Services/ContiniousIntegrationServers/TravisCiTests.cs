using System;
using Codecov.Services.ContinuousIntegrationServers;
using FluentAssertions;
using Xunit;

namespace Codecov.Tests.Services.ContiniousIntegrationServers
{
    public class TravisCiTests
    {
        [Fact]
        public void Branch_Should_Be_Empty_String_When_Environment_Variable_Does_Not_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("TRAVIS_BRANCH", null);
            var travisCi = new TravisCi();

            // When
            var branch = travisCi.Branch;

            // Then
            branch.Should().BeEmpty();
        }

        [Fact]
        public void Branch_Should_Be_Set_When_Environment_Variable_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("TRAVIS_BRANCH", "develop");
            var travisCi = new TravisCi();

            // When
            var branch = travisCi.Branch;

            // Then
            branch.Should().Be("develop");
        }

        [Fact]
        public void Build_Should_Be_Empty_String_When_Environment_Variable_Does_Not_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("TRAVIS_JOB_NUMBER", null);
            var travisCi = new TravisCi();

            // When
            var build = travisCi.Build;

            // Then
            build.Should().BeEmpty();
        }

        [Fact]
        public void Build_Should_Be_Set_When_Environment_Variable_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("TRAVIS_JOB_NUMBER", "5.2");
            var travisCi = new TravisCi();

            // When
            var build = travisCi.Build;

            // Then
            build.Should().Be("5.2");
        }

        [Fact]
        public void Commit_Should_Be_Empty_String_When_Environment_Variable_Does_Not_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("TRAVIS_COMMIT", null);
            var travisCi = new TravisCi();

            // When
            var commit = travisCi.Commit;

            // Then
            commit.Should().BeEmpty();
        }

        [Fact]
        public void Commit_Should_Be_Set_When_Environment_Variable_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("TRAVIS_COMMIT", "123");
            var travisCi = new TravisCi();

            // When
            var commit = travisCi.Commit;

            // Then
            commit.Should().Be("123");
        }

        [Theory, InlineData(null, null), InlineData("", ""), InlineData("True", null), InlineData("True", ""), InlineData(null, "True"), InlineData("", "True"), InlineData("true", "True"), InlineData("True", "true"), InlineData("False", "True"), InlineData("True", "False"), InlineData("False", "False"), InlineData("foo", "bar")]
        public void Detecter_Should_Be_False_When_TravisCi_Environment_Variable_Or_Ci_Environment_Variable_Does_Not_Exit_And_Both_Are_Not_Equal_To_True(string travisCiData, string ciData)
        {
            // Given
            Environment.SetEnvironmentVariable("TRAVIS", travisCiData);
            Environment.SetEnvironmentVariable("CI", ciData);
            var travisCi = new TravisCi();

            // When
            var detecter = travisCi.Detecter;

            // Then
            detecter.Should().BeFalse();
        }

        [Theory]
        [InlineData("True", "True")]
        [InlineData("true", "true")]
        public void Detecter_Should_Be_True_When_travisCi_Environment_Variable_And_Ci_Environment_Variable_Exist_And_Both_Are_Equal_To_True(string travisCiData, string ciData)
        {
            // Given
            Environment.SetEnvironmentVariable("TRAVIS", travisCiData);
            Environment.SetEnvironmentVariable("CI", ciData);
            var travisCi = new TravisCi();

            // When
            var detecter = travisCi.Detecter;

            // Then
            detecter.Should().BeTrue();
        }

        [Theory, InlineData(null), InlineData("")]
        public void Job_Should_Be_Empty_String_When_Environment_Variables_Do_Not_Exit(string jobNumber)
        {
            // Given
            Environment.SetEnvironmentVariable("TRAVIS_JOB_ID", jobNumber);

            var travisCi = new TravisCi();

            // When
            var job = travisCi.Job;

            // Then
            job.Should().BeEmpty();
        }

        [Fact]
        public void Job_Should_Not_Be_Empty_String_When_Environment_Variables_Exit()
        {
            // Given
            Environment.SetEnvironmentVariable("TRAVIS_JOB_ID", "15657");
            var travisCi = new TravisCi();

            // When
            var job = travisCi.Job;

            // Then
            job.Should().Be("15657");
        }

        [Fact]
        public void Pr_Should_Be_Empty_String_When_Environment_Variable_Does_Not_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("TRAVIS_PULL_REQUEST", null);
            var travisCi = new TravisCi();

            // When
            var pr = travisCi.Pr;

            // Then
            pr.Should().BeEmpty();
        }

        [Fact]
        public void Pr_Should_Be_Set_When_Environment_Variable_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("TRAVIS_PULL_REQUEST", "123");
            var travisCi = new TravisCi();

            // When
            var pr = travisCi.Pr;

            // Then
            pr.Should().Be("123");
        }

        [Fact]
        public void Slug_Should_Be_Empty_String_When_Environment_Variable_Does_Not_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("TRAVIS_REPO_SLUG", null);
            var travisCi = new TravisCi();

            // When
            var slug = travisCi.Slug;

            // Then
            slug.Should().BeEmpty();
        }

        [Fact]
        public void Slug_Should_Be_Set_When_Environment_Variable_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("TRAVIS_REPO_SLUG", "foo/bar");
            var travisCi = new TravisCi();

            // When
            var slug = travisCi.Slug;

            // Then
            slug.Should().Be("foo/bar");
        }
    }
}
