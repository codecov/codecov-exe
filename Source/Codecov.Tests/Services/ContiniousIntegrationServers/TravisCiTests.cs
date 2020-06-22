using System;
using Codecov.Services.ContinuousIntegrationServers;
using FluentAssertions;
using Moq;
using Xunit;

namespace Codecov.Tests.Services.ContiniousIntegrationServers
{
    public class TravisTests
    {
        [Fact]
        public void Branch_Should_Be_Empty_String_When_Environment_Variable_Does_Not_Exits()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            var travis = new Travis(ev.Object);

            // When
            var branch = travis.Branch;

            // Then
            branch.Should().BeEmpty();
        }

        [Fact]
        public void Branch_Should_Be_Set_When_Environment_Variable_Exits()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("TRAVIS_BRANCH")).Returns("develop");
            var travis = new Travis(ev.Object);

            // When
            var branch = travis.Branch;

            // Then
            branch.Should().Be("develop");
        }

        [Fact]
        public void Build_Should_Be_Empty_String_When_Environment_Variable_Does_Not_Exits()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            var travis = new Travis(ev.Object);

            // When
            var build = travis.Build;

            // Then
            build.Should().BeEmpty();
        }

        [Fact]
        public void Build_Should_Be_Set_When_Environment_Variable_Exits()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("TRAVIS_JOB_NUMBER")).Returns("5.2");
            var travis = new Travis(ev.Object);

            // When
            var build = travis.Build;

            // Then
            build.Should().Be("5.2");
        }

        [Fact]
        public void Commit_Should_Be_Empty_String_When_Environment_Variable_Does_Not_Exits()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            var travis = new Travis(ev.Object);

            // When
            var commit = travis.Commit;

            // Then
            commit.Should().BeEmpty();
        }

        [Fact]
        public void Commit_Should_Be_Set_When_Environment_Variable_Exits()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("TRAVIS_COMMIT")).Returns("123");
            var travis = new Travis(ev.Object);

            // When
            var commit = travis.Commit;

            // Then
            commit.Should().Be("123");
        }

        [Theory, InlineData(null, null), InlineData("", ""), InlineData("True", null), InlineData("True", ""), InlineData(null, "True"), InlineData("", "True"), InlineData("true", "True"), InlineData("True", "true"), InlineData("False", "True"), InlineData("True", "False"), InlineData("False", "False"), InlineData("foo", "bar")]
        public void Detecter_Should_Be_False_When_Travis_Environment_Variable_Or_Ci_Environment_Variable_Does_Not_Exit_And_Both_Are_Not_Equal_To_True(string travisData, string ciData)
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("TRAVIS")).Returns(travisData);
            ev.Setup(s => s.GetEnvironmentVariable("CI")).Returns(ciData);
            var travis = new Travis(ev.Object);

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
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("TRAVIS")).Returns(travisData);
            ev.Setup(s => s.GetEnvironmentVariable("CI")).Returns(ciData);
            var travis = new Travis(ev.Object);

            // When
            var detecter = travis.Detecter;

            // Then
            detecter.Should().BeTrue();
        }

        [Fact]
        public void BuildUrl_Should_Be_Empty_String_When_Environment_Variables_Do_Not_Exist()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            var travis = new Travis(ev.Object);

            // When
            var buildUrl = travis.BuildUrl;

            // Then
            buildUrl.Should().BeEmpty();
        }

        [Fact]
        public void BuildUrl_Should_Not_Be_Empty_String_When_Environment_Variable_Exist()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("TRAVIS_JOB_WEB_URL")).Returns("https://travis-ci.org/some-job");
            var travis = new Travis(ev.Object);

            // When
            var buildUrl = travis.BuildUrl;

            // Then
            buildUrl.Should().Be("https://travis-ci.org/some-job");
        }

        [Fact]
        public void Job_Should_Be_Empty_String_When_Environment_Variables_Do_Not_Exit()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            var travis = new Travis(ev.Object);

            // When
            var job = travis.Job;

            // Then
            job.Should().BeEmpty();
        }

        [Fact]
        public void Job_Should_Not_Be_Empty_String_When_Environment_Variables_Exit()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("TRAVIS_JOB_ID")).Returns("15657");
            var travis = new Travis(ev.Object);

            // When
            var job = travis.Job;

            // Then
            job.Should().Be("15657");
        }

        [Fact]
        public void Pr_Should_Be_Empty_String_When_Environment_Variable_Does_Not_Exits()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            var travis = new Travis(ev.Object);

            // When
            var pr = travis.Pr;

            // Then
            pr.Should().BeEmpty();
        }

        [Fact]
        public void Pr_Should_Be_Set_When_Environment_Variable_Exits()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("TRAVIS_PULL_REQUEST")).Returns("123");
            var travis = new Travis(ev.Object);

            // When
            var pr = travis.Pr;

            // Then
            pr.Should().Be("123");
        }

        [Fact]
        public void Slug_Should_Be_Empty_String_When_Environment_Variable_Does_Not_Exits()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            var travis = new Travis(ev.Object);

            // When
            var slug = travis.Slug;

            // Then
            slug.Should().BeEmpty();
        }

        [Fact]
        public void Slug_Should_Be_Set_When_Environment_Variable_Exits()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("TRAVIS_REPO_SLUG")).Returns("foo/bar");
            var travis = new Travis(ev.Object);

            // When
            var slug = travis.Slug;

            // Then
            slug.Should().Be("foo/bar");
        }

        [Fact]
        public void Tag_Should_Empty_String_When_Environment_Variable_Does_Not_Exist()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            var travis = new Travis(ev.Object);

            // When
            var tag = travis.Tag;

            // THen
            tag.Should().BeEmpty();
        }

        [Fact]
        public void Tag_Should_Not_Be_Empty_String_When_Environment_Variable_Exist()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("TRAVIS_TAG")).Returns("v1.2.4");
            var travis = new Travis(ev.Object);

            // When
            var tag = travis.Tag;

            // Then
            tag.Should().Be("v1.2.4");
        }
    }
}
