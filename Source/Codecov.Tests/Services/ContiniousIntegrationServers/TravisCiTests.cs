using System;
using Codecov.Services.ContinuousIntegrationServers;
using FluentAssertions;
using Xunit;

namespace Codecov.Tests.Services.ContiniousIntegrationServers
{
    public class TravisTests
    {
        [Fact]
        public void Branch_Should_Be_Empty_String_When_Environment_Variable_Does_Not_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("TRAVIS_BRANCH", null);
            var Travis = new Travis();

            // When
            var branch = Travis.Branch;

            // Then
            branch.Should().BeEmpty();
        }

        [Fact]
        public void Branch_Should_Be_Set_When_Environment_Variable_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("TRAVIS_BRANCH", "develop");
            var Travis = new Travis();

            // When
            var branch = Travis.Branch;

            // Then
            branch.Should().Be("develop");
        }

        [Fact]
        public void Build_Should_Be_Empty_String_When_Environment_Variable_Does_Not_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("TRAVIS_JOB_NUMBER", null);
            var Travis = new Travis();

            // When
            var build = Travis.Build;

            // Then
            build.Should().BeEmpty();
        }

        [Fact]
        public void Build_Should_Be_Set_When_Environment_Variable_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("TRAVIS_JOB_NUMBER", "5.2");
            var Travis = new Travis();

            // When
            var build = Travis.Build;

            // Then
            build.Should().Be("5.2");
        }

        [Fact]
        public void Commit_Should_Be_Empty_String_When_Environment_Variable_Does_Not_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("TRAVIS_COMMIT", null);
            var Travis = new Travis();

            // When
            var commit = Travis.Commit;

            // Then
            commit.Should().BeEmpty();
        }

        [Fact]
        public void Commit_Should_Be_Set_When_Environment_Variable_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("TRAVIS_COMMIT", "123");
            var Travis = new Travis();

            // When
            var commit = Travis.Commit;

            // Then
            commit.Should().Be("123");
        }

        [Theory, InlineData(null, null), InlineData("", ""), InlineData("True", null), InlineData("True", ""), InlineData(null, "True"), InlineData("", "True"), InlineData("true", "True"), InlineData("True", "true"), InlineData("False", "True"), InlineData("True", "False"), InlineData("False", "False"), InlineData("foo", "bar")]
        public void Detecter_Should_Be_False_When_Travis_Environment_Variable_Or_Ci_Environment_Variable_Does_Not_Exit_And_Both_Are_Not_Equal_To_True(string TravisData, string ciData)
        {
            // Given
            Environment.SetEnvironmentVariable("TRAVIS", TravisData);
            Environment.SetEnvironmentVariable("CI", ciData);
            var Travis = new Travis();

            // When
            var detecter = Travis.Detecter;

            // Then
            detecter.Should().BeFalse();
        }

        [Theory]
        [InlineData("True", "True")]
        [InlineData("true", "true")]
        public void Detecter_Should_Be_True_When_Travis_Environment_Variable_And_Ci_Environment_Variable_Exist_And_Both_Are_Equal_To_True(string TravisData, string ciData)
        {
            // Given
            Environment.SetEnvironmentVariable("TRAVIS", TravisData);
            Environment.SetEnvironmentVariable("CI", ciData);
            var Travis = new Travis();

            // When
            var detecter = Travis.Detecter;

            // Then
            detecter.Should().BeTrue();
        }

        [Theory, InlineData(null), InlineData("")]
        public void Job_Should_Be_Empty_String_When_Environment_Variables_Do_Not_Exit(string jobNumber)
        {
            // Given
            Environment.SetEnvironmentVariable("TRAVIS_JOB_ID", jobNumber);

            var Travis = new Travis();

            // When
            var job = Travis.Job;

            // Then
            job.Should().BeEmpty();
        }

        [Fact]
        public void Job_Should_Not_Be_Empty_String_When_Environment_Variables_Exit()
        {
            // Given
            Environment.SetEnvironmentVariable("TRAVIS_JOB_ID", "15657");
            var Travis = new Travis();

            // When
            var job = Travis.Job;

            // Then
            job.Should().Be("15657");
        }

        [Fact]
        public void Pr_Should_Be_Empty_String_When_Environment_Variable_Does_Not_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("TRAVIS_PULL_REQUEST", null);
            var Travis = new Travis();

            // When
            var pr = Travis.Pr;

            // Then
            pr.Should().BeEmpty();
        }

        [Fact]
        public void Pr_Should_Be_Set_When_Environment_Variable_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("TRAVIS_PULL_REQUEST", "123");
            var Travis = new Travis();

            // When
            var pr = Travis.Pr;

            // Then
            pr.Should().Be("123");
        }

        [Fact]
        public void Slug_Should_Be_Empty_String_When_Environment_Variable_Does_Not_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("TRAVIS_REPO_SLUG", null);
            var Travis = new Travis();

            // When
            var slug = Travis.Slug;

            // Then
            slug.Should().BeEmpty();
        }

        [Fact]
        public void Slug_Should_Be_Set_When_Environment_Variable_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("TRAVIS_REPO_SLUG", "foo/bar");
            var Travis = new Travis();

            // When
            var slug = Travis.Slug;

            // Then
            slug.Should().Be("foo/bar");
        }
    }
}
