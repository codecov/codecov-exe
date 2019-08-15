using System;
using System.Collections.Generic;
using Codecov.Services.ContinuousIntegrationServers;
using FluentAssertions;
using Xunit;

namespace Codecov.Tests.Services.ContiniousIntegrationServers
{
    public class AppVeyorTests : IDisposable
    {
        public static IEnumerable<object[]> Build_Url_Empty_Data
        {
            get
            {
                var possibleDomains = new[]{ null, string.Empty, "https://ci.appveyor.com", "http://localhost:5234" };
                var possibleDummies = new[] { null, string.Empty, "foo", "bar" };

                foreach (var domain in possibleDomains)
                {
                    foreach (var account in possibleDummies)
                    {
                        foreach (var slug in possibleDummies)
                        {
                            foreach (var job in possibleDummies)
                            {
                                if (string.IsNullOrEmpty(domain) || string.IsNullOrEmpty(account) || string.IsNullOrEmpty(slug) || string.IsNullOrEmpty(job))
                                {
                                    yield return new object[] { domain, account, slug, job };
                                }
                            }
                        }
                    }
                }
            }
        }

        [Fact]
        public void Branch_Should_Be_Empty_String_When_Enviornment_Variable_Does_Not_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("APPVEYOR_REPO_BRANCH", null);
            var appVeyor = new AppVeyor();

            // When
            var branch = appVeyor.Branch;

            // Then
            branch.Should().BeEmpty();
        }

        [Fact]
        public void Branch_Should_Be_Set_When_Enviornment_Variable_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("APPVEYOR_REPO_BRANCH", "develop");
            var appVeyor = new AppVeyor();

            // When
            var branch = appVeyor.Branch;

            // Then
            branch.Should().Be("develop");
        }

        [Fact]
        public void Build_Should_Be_Empty_String_When_Enviornment_Variable_Does_Not_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("APPVEYOR_JOB_ID", null);
            var appVeyor = new AppVeyor();

            // When
            var build = appVeyor.Build;

            // Then
            build.Should().BeEmpty();
        }

        [Fact]
        public void Build_Should_Be_Set_When_Enviornment_Variable_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("APPVEYOR_JOB_ID", "Job 123");
            var appVeyor = new AppVeyor();

            // When
            var build = appVeyor.Build;

            // Then
            build.Should().Be("Job%20123");
        }

        [Fact]
        public void Commit_Should_Be_Empty_String_When_Enviornment_Variable_Does_Not_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("APPVEYOR_REPO_COMMIT", null);
            var appVeyor = new AppVeyor();

            // When
            var commit = appVeyor.Commit;

            // Then
            commit.Should().BeEmpty();
        }

        [Fact]
        public void Commit_Should_Be_Set_When_Enviornment_Variable_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("APPVEYOR_REPO_COMMIT", "123");
            var appVeyor = new AppVeyor();

            // When
            var commit = appVeyor.Commit;

            // Then
            commit.Should().Be("123");
        }

        [Theory, InlineData(null, null), InlineData("", ""), InlineData("True", null), InlineData("True", ""), InlineData(null, "True"), InlineData("", "True"), InlineData("true", "True"), InlineData("True", "true"), InlineData("False", "True"), InlineData("True", "False"), InlineData("False", "False"), InlineData("foo", "bar")]
        public void Detecter_Should_Be_False_When_AppVeyor_Enviornment_Variable_Or_Ci_Enviornment_Variable_Does_Not_Exit_And_Both_Are_Not_Equal_To_True(string appveyorData, string ciData)
        {
            // Given
            Environment.SetEnvironmentVariable("APPVEYOR", appveyorData);
            Environment.SetEnvironmentVariable("CI", ciData);
            var appVeyor = new AppVeyor();

            // When
            var detecter = appVeyor.Detecter;

            // Then
            detecter.Should().BeFalse();
        }

        [Theory]
        [InlineData("True", "True")]
        [InlineData("true", "true")]
        public void Detecter_Should_Be_True_When_AppVeyor_Enviornment_Variable_And_Ci_Enviornment_Variable_Exist_And_Both_Are_Equal_To_True(string appveyorData, string ciData)
        {
            // Given
            Environment.SetEnvironmentVariable("APPVEYOR", appveyorData);
            Environment.SetEnvironmentVariable("CI", ciData);
            var appVeyor = new AppVeyor();

            // When
            var detecter = appVeyor.Detecter;

            // Then
            detecter.Should().BeTrue();
        }

        [Theory, MemberData(nameof(Build_Url_Empty_Data))]
        public void BuildUrl_Should_Be_Empty_String_When_Environment_Variables_Do_Not_Exist(string appveyorUrl, string accountData, string slugData, string jobId)
        {
            // Given
            Environment.SetEnvironmentVariable("APPVEYOR_URL", appveyorUrl);
            Environment.SetEnvironmentVariable("APPVEYOR_ACCOUNT_NAME", accountData);
            Environment.SetEnvironmentVariable("APPVEYOR_PROJECT_SLUG", slugData);
            Environment.SetEnvironmentVariable("APPVEYOR_JOB_ID", jobId);

            var appveyor = new AppVeyor();

            // When
            var buildUrl = appveyor.BuildUrl;

            // Then
            buildUrl.Should().BeEmpty();
        }

        [Fact]
        public void BuildUrl_Should_Not_Empty_String_When_Environment_Variable_Exists()
        {
            // Given
            Environment.SetEnvironmentVariable("APPVEYOR_URL", "https://ci.appveyor.com");
            Environment.SetEnvironmentVariable("APPVEYOR_ACCOUNT_NAME", "foo");
            Environment.SetEnvironmentVariable("APPVEYOR_PROJECT_SLUG", "bar");
            Environment.SetEnvironmentVariable("APPVEYOR_JOB_ID", "xyz");

            var appveyor = new AppVeyor();

            // When
            var buildUrl = appveyor.BuildUrl;

            // Then
            buildUrl.Should().Be("https://ci.appveyor.com/project/foo/bar/build/job/xyz");
        }

        [Theory, InlineData("http://"), InlineData("http://."), InlineData("http://.."), InlineData("http://../"), InlineData("http://?"), InlineData("http://??"), InlineData("http://#"), InlineData("http://##"), InlineData("//"), InlineData("//a"), InlineData("///a"), InlineData("///"), InlineData("foo.com"), InlineData("rdar://1234"), InlineData("h://test"), InlineData("http:// shouldfail.com"), InlineData(":// should fail"), InlineData("ftps://foo.bar/"), InlineData("http://.www.foo.bar/")]
        public void BuildUrl_Should_Be_Empty_When_Appveyor_Url_Is_Invalid_Domain(string urlData)
        {
            // Given
            Environment.SetEnvironmentVariable("APPVEYOR_URL", urlData);
            Environment.SetEnvironmentVariable("APPVEYOR_ACCOUNT_NAME", "foo");
            Environment.SetEnvironmentVariable("APPVEYOR_PROJECT_SLUG", "bar");
            Environment.SetEnvironmentVariable("APPVEYOR_JOB_ID", "xyz");

            var appveyor = new AppVeyor();

            // When
            var buildUrl = appveyor.BuildUrl;

            // Then
            buildUrl.Should().BeEmpty();
        }

        [Theory, InlineData(null, null, null), InlineData("", "", ""), InlineData("foo", "bar", ""), InlineData("", "foo", "bar"), InlineData("foo", "", "bar"), InlineData("", "", "foo"), InlineData("foo", "", ""), InlineData("", "foo", "")]
        public void Job_Should_Be_Empty_String_When_Enviornment_Variables_Do_Not_Exit(string accountData, string slugData, string versionData)
        {
            // Given
            Environment.SetEnvironmentVariable("APPVEYOR_ACCOUNT_NAME", accountData);
            Environment.SetEnvironmentVariable("APPVEYOR_PROJECT_SLUG", slugData);
            Environment.SetEnvironmentVariable("APPVEYOR_BUILD_VERSION", versionData);

            var appVeyor = new AppVeyor();

            // When
            var job = appVeyor.Job;

            // Then
            job.Should().BeEmpty();
        }

        [Fact]
        public void Job_Should_Not_Be_Empty_String_When_Enviornment_Variables_Exit()
        {
            // Given
            Environment.SetEnvironmentVariable("APPVEYOR_ACCOUNT_NAME", "foo");
            Environment.SetEnvironmentVariable("APPVEYOR_PROJECT_SLUG", "bar");
            Environment.SetEnvironmentVariable("APPVEYOR_BUILD_VERSION", "bang");
            var appVeyor = new AppVeyor();

            // When
            var job = appVeyor.Job;

            // Then
            job.Should().Be("foo/bar/bang");
        }

        [Fact]
        public void Pr_Should_Be_Empty_String_When_Enviornment_Variable_Does_Not_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("APPVEYOR_PULL_REQUEST_NUMBER", null);
            var appVeyor = new AppVeyor();

            // When
            var pr = appVeyor.Pr;

            // Then
            pr.Should().BeEmpty();
        }

        [Fact]
        public void Pr_Should_Be_Set_When_Enviornment_Variable_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("APPVEYOR_PULL_REQUEST_NUMBER", "123");
            var appVeyor = new AppVeyor();

            // When
            var pr = appVeyor.Pr;

            // Then
            pr.Should().Be("123");
        }

        [Fact]
        public void Slug_Should_Be_Empty_String_When_Enviornment_Variable_Does_Not_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("APPVEYOR_REPO_NAME", null);
            var appVeyor = new AppVeyor();

            // When
            var slug = appVeyor.Slug;

            // Then
            slug.Should().BeEmpty();
        }

        [Fact]
        public void Slug_Should_Be_Set_When_Enviornment_Variable_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("APPVEYOR_REPO_NAME", "foo/bar");
            var appVeyor = new AppVeyor();

            // When
            var slug = appVeyor.Slug;

            // Then
            slug.Should().Be("foo/bar");
        }

        public void Dispose()
        {
            // We will remove all environment variables that could have been set during unit test
            var envVariable = new[]
            {
                "CI",
                "APPVEYOR_REPO_BRANCH",
                "APPVEYOR_JOB_ID",
                "APPVEYOR_REPO_COMMIT",
                "APPVEYOR",
                "APPVEYOR_ACCOUNT_NAME",
                "APPVEYOR_PROJECT_SLUG",
                "APPVEYOR_BUILD_VERSION",
                "APPVEYOR_PULL_REQUEST_NUMBER",
                "APPVEYOR_REPO_NAME",
                "APPVEYOR_URL",
            };

            foreach (var variable in envVariable)
            {
                Environment.SetEnvironmentVariable(variable, null);
            }
        }
    }
}
