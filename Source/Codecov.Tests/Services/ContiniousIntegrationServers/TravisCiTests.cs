using System;
using Codecov.Services.ContinuousIntegrationServers;
using FluentAssertions;
using Xunit;

namespace Codecov.Tests.Services.ContiniousIntegrationServers
{
    public class TravisTests : IDisposable
    {
        [Fact]
        public void Branch_Should_Be_Empty_String_When_Environment_Variable_Does_Not_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("TRAVIS_BRANCH", null);
            var travis = new Travis();

            // When
            var branch = travis.Branch;

            // Then
            branch.Should().BeEmpty();
        }

        [Fact]
        public void Branch_Should_Be_Set_When_Environment_Variable_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("TRAVIS_BRANCH", "develop");
            var travis = new Travis();

            // When
            var branch = travis.Branch;

            // Then
            branch.Should().Be("develop");
        }

        [Fact]
        public void Build_Should_Be_Empty_String_When_Environment_Variable_Does_Not_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("TRAVIS_JOB_NUMBER", null);
            var travis = new Travis();

            // When
            var build = travis.Build;

            // Then
            build.Should().BeEmpty();
        }

        [Fact]
        public void Build_Should_Be_Set_When_Environment_Variable_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("TRAVIS_JOB_NUMBER", "5.2");
            var travis = new Travis();

            // When
            var build = travis.Build;

            // Then
            build.Should().Be("5.2");
        }

        [Fact]
        public void Commit_Should_Be_Empty_String_When_Environment_Variable_Does_Not_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("TRAVIS_COMMIT", null);
            var travis = new Travis();

            // When
            var commit = travis.Commit;

            // Then
            commit.Should().BeEmpty();
        }

        [Fact]
        public void Commit_Should_Be_Set_When_Environment_Variable_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("TRAVIS_COMMIT", "123");
            var travis = new Travis();

            // When
            var commit = travis.Commit;

            // Then
            commit.Should().Be("123");
        }

        [Theory, InlineData(null, null), InlineData("", ""), InlineData("True", null), InlineData("True", ""), InlineData(null, "True"), InlineData("", "True"), InlineData("true", "True"), InlineData("True", "true"), InlineData("False", "True"), InlineData("True", "False"), InlineData("False", "False"), InlineData("foo", "bar")]
        public void Detecter_Should_Be_False_When_Travis_Environment_Variable_Or_Ci_Environment_Variable_Does_Not_Exit_And_Both_Are_Not_Equal_To_True(string travisData, string ciData)
        {
            // Given
            Environment.SetEnvironmentVariable("TRAVIS", travisData);
            Environment.SetEnvironmentVariable("CI", ciData);
            var travis = new Travis();

            // When
            var detecter = travis.Detecter;

            // Then
            detecter.Should().BeFalse();
        }

        [Theory]
        [InlineData("True", "True")]
        [InlineData("true", "true")]
        public void Detecter_Should_Be_True_When_Travis_Environment_Variable_And_Ci_Environment_Variable_Exist_And_Both_Are_Equal_To_True(string travisData, string ciData)
        {
            // Given
            Environment.SetEnvironmentVariable("TRAVIS", travisData);
            Environment.SetEnvironmentVariable("CI", ciData);
            var travis = new Travis();

            // When
            var detecter = travis.Detecter;

            // Then
            detecter.Should().BeTrue();
        }

        [Theory, InlineData(null), InlineData("")]
        public void BuildUrl_Should_Be_Empty_String_When_Environment_Variables_Do_Not_Exist(string jobUrl)
        {
            // Given
            Environment.SetEnvironmentVariable("TRAVIS_JOB_WEB_URL", jobUrl);

            var travis = new Travis();

            // When
            var buildUrl = travis.BuildUrl;

            // Then
            buildUrl.Should().BeEmpty();
        }

        [Fact]
        public void BuildUrl_Should_Not_Be_Empty_String_When_Environment_Variable_Exist()
        {
            // Given
            Environment.SetEnvironmentVariable("TRAVIS_JOB_WEB_URL", "https://travis-ci.org/some-job");

            var travis = new Travis();

            // When
            var buildUrl = travis.BuildUrl;

            // Then
            buildUrl.Should().Be("https://travis-ci.org/some-job");
        }

        [Theory, InlineData(null), InlineData("")]
        public void Job_Should_Be_Empty_String_When_Environment_Variables_Do_Not_Exit(string jobNumber)
        {
            // Given
            Environment.SetEnvironmentVariable("TRAVIS_JOB_ID", jobNumber);

            var travis = new Travis();

            // When
            var job = travis.Job;

            // Then
            job.Should().BeEmpty();
        }

        [Fact]
        public void Job_Should_Not_Be_Empty_String_When_Environment_Variables_Exit()
        {
            // Given
            Environment.SetEnvironmentVariable("TRAVIS_JOB_ID", "15657");
            var travis = new Travis();

            // When
            var job = travis.Job;

            // Then
            job.Should().Be("15657");
        }

        [Fact]
        public void Pr_Should_Be_Empty_String_When_Environment_Variable_Does_Not_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("TRAVIS_PULL_REQUEST", null);
            var travis = new Travis();

            // When
            var pr = travis.Pr;

            // Then
            pr.Should().BeEmpty();
        }

        [Fact]
        public void Pr_Should_Be_Set_When_Environment_Variable_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("TRAVIS_PULL_REQUEST", "123");
            var travis = new Travis();

            // When
            var pr = travis.Pr;

            // Then
            pr.Should().Be("123");
        }

        [Fact]
        public void Slug_Should_Be_Empty_String_When_Environment_Variable_Does_Not_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("TRAVIS_REPO_SLUG", null);
            var travis = new Travis();

            // When
            var slug = travis.Slug;

            // Then
            slug.Should().BeEmpty();
        }

        [Fact]
        public void Slug_Should_Be_Set_When_Environment_Variable_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("TRAVIS_REPO_SLUG", "foo/bar");
            var travis = new Travis();

            // When
            var slug = travis.Slug;

            // Then
            slug.Should().Be("foo/bar");
        }

        public void Dispose()
        {
            // We will remove all environment variables that could have been set during unit test
            var envVariable = new[]
            {
                "CI",
                "CODECOV_SLUG", // We use this travis fork tests
                "TRAVIS",
                "TRAVIS_BRANCH",
                "TRAVIS_JOB_NUMBER",
                "TRAVIS_COMMIT",
                "TRAVIS_JOB_ID",
                "TRAVIS_PULL_REQUEST",
                "TRAVIS_REPO_SLUG",
                "TRAVIS_JOB_WEB_URL",
            };

            foreach (var variable in envVariable)
            {
                Environment.SetEnvironmentVariable(variable, null);
            }
        }
    }
}
