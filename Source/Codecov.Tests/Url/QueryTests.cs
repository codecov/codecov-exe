using System;
using System.Collections.Generic;
using System.Linq;
using Codecov.Services;
using Codecov.Services.ContinuousIntegrationServers;
using Codecov.Url;
using Codecov.Utilities;
using Codecov.Yaml;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Codecov.Tests.Url
{
    public class QueryTests
    {
        [Fact]
        public void Multiple_Repositories_Should_Used_ContiniousIntegrationServer_First()
        {
            // Given
            var queryOptions = Substitute.For<IQueryOptions>();
            var continiousIntegrationServer = Substitute.For<IRepository>();
            continiousIntegrationServer.Branch.Returns("develop");
            var versionControlSystem = Substitute.For<IRepository>();
            versionControlSystem.Branch.Returns("dev");
            var build = Substitute.For<IBuild>();
            var yaml = Substitute.For<IYaml>();
            var query = new Query(queryOptions, new[] { continiousIntegrationServer, versionControlSystem }, build, yaml);

            // When
            var getQuery = query.GetQuery.Split('&');

            // Then
            getQuery.FirstOrDefault(x => x.StartsWith("branch=")).Should().Be("branch=develop");
        }

        [Fact]
        public void Should_Encode_Slashes_In_BranchName()
        {
            // Given
            var queryOptions = Substitute.For<IQueryOptions>();
            var continiousIntegrationServer = Substitute.For<IRepository>();
            continiousIntegrationServer.Branch.Returns("release/6.0.0");
            var versionControlSystem = Substitute.For<IRepository>();
            versionControlSystem.Branch.Returns("dev");
            var build = Substitute.For<IBuild>();
            var yaml = Substitute.For<IYaml>();
            var query = new Query(queryOptions, new[] { continiousIntegrationServer, versionControlSystem }, build, yaml);

            // When
            var getQuery = query.GetQuery.Split('&');

            // Then
            getQuery.Should().Contain("branch=release%2F6.0.0");
        }

        [Fact]
        public void Should_Double_Encode_Pluss_Signs()
        {
            // Given
            var queryOptions = Substitute.For<IQueryOptions>();
            var repository = Substitute.For<IEnumerable<IRepository>>();
            var build = Substitute.For<IBuild>();
            build.Build.Returns("123");
            build.BuildUrl.Returns("www.google.com");
            build.Job.Returns("codecov/codecov-exe/1.0.4-beta.1+5.build.63");
            var yaml = Substitute.For<IYaml>();
            var query = new Query(queryOptions, repository, build, yaml);

            // When
            var getQuery = query.GetQuery.Split('&');

            // Then
            getQuery.FirstOrDefault(x => x.StartsWith("job=")).Should().Be("job=codecov%2Fcodecov-exe%2F1.0.4-beta.1%252B5.build.63");
        }

        [Fact]
        public void Should_Escape_Hash_Token_In_Branch()
        {
            // Given
            var queryOptions = Substitute.For<IQueryOptions>();
            var repository = Substitute.For<IRepository>();
            var repositoryRange = new[] { repository };
            var build = Substitute.For<IBuild>();
            build.Build.Returns("123");
            build.BuildUrl.Returns("www.google.com");
            build.Job.Returns("0");
            repository.Branch.Returns("#8-move-docs");
            var yaml = Substitute.For<IYaml>();
            var query = new Query(queryOptions, repositoryRange, build, yaml);

            // When
            var getQuery = query.GetQuery.Split('&');

            // Then
            getQuery.FirstOrDefault(x => x.StartsWith("branch=")).Should().Be("branch=%238-move-docs");
        }

        [Fact]
        public void Should_Return_Defaults()
        {
            // Given
            var queryOptions = Substitute.For<IQueryOptions>();
            var repository = Substitute.For<IEnumerable<IRepository>>();
            var build = Substitute.For<IBuild>();
            var yaml = Substitute.For<IYaml>();
            var query = new Query(queryOptions, repository, build, yaml);

            // When
            var getQuery = query.GetQuery.Split('&');

            // Then
            getQuery.FirstOrDefault(x => x.StartsWith("branch=")).Should().Be("branch=");
            getQuery.FirstOrDefault(x => x.StartsWith("commit=")).Should().Be("commit=");
            getQuery.FirstOrDefault(x => x.StartsWith("token=")).Should().Be("token=");
            getQuery.FirstOrDefault(x => x.StartsWith("build=")).Should().Be("build=");
            getQuery.FirstOrDefault(x => x.StartsWith("tag=")).Should().Be("tag=");
            getQuery.FirstOrDefault(x => x.StartsWith("pr=")).Should().Be("pr=");
            getQuery.FirstOrDefault(x => x.StartsWith("name=")).Should().Be("name=");
            getQuery.FirstOrDefault(x => x.StartsWith("flags=")).Should().Be("flags=");
            getQuery.FirstOrDefault(x => x.StartsWith("slug=")).Should().Be("slug=");
            getQuery.FirstOrDefault(x => x.StartsWith("build_url=")).Should().Be("build_url=");
            getQuery.FirstOrDefault(x => x.StartsWith("yaml=")).Should().Be("yaml=");
            getQuery.FirstOrDefault(x => x.StartsWith("job=")).Should().Be("job=");
            getQuery.FirstOrDefault(x => x.StartsWith("service=")).Should().Be("service=");
            getQuery.FirstOrDefault(x => x.StartsWith("package=")).Should().Be($"package={About.Version}");
        }

        [Fact]
        public void Should_Set_From_Build()
        {
            // Given
            var queryOptions = Substitute.For<IQueryOptions>();
            var repository = Substitute.For<IEnumerable<IRepository>>();
            var build = Substitute.For<IBuild>();
            build.Build.Returns("123");
            build.BuildUrl.Returns("www.google.com");
            build.Job.Returns("0");
            build.Service.Returns("appveyor");
            var yaml = Substitute.For<IYaml>();
            var query = new Query(queryOptions, repository, build, yaml);

            // When
            var getQuery = query.GetQuery.Split('&');

            // Then
            getQuery.FirstOrDefault(x => x.StartsWith("build=")).Should().Be("build=123");
            getQuery.FirstOrDefault(x => x.StartsWith("build_url=")).Should().Be("build_url=www.google.com");
            getQuery.FirstOrDefault(x => x.StartsWith("job=")).Should().Be("job=0");
            getQuery.FirstOrDefault(x => x.StartsWith("service=")).Should().Be("service=appveyor");
        }

        [Fact]
        public void Should_Set_From_Commandline()
        {
            // Given
            var queryOptions = Substitute.For<IQueryOptions>();
            queryOptions.Branch.Returns("develop");
            queryOptions.Build.Returns("123");
            queryOptions.Commit.Returns("3c075f8d15aea3b3b40cea0bcf441a30bfd5c5d2");
            queryOptions.Flags.Returns("ui, chrome");
            queryOptions.Name.Returns("foo");
            queryOptions.Pr.Returns("bar");
            queryOptions.Slug.Returns("foo/bar");
            queryOptions.Tag.Returns("v1.0");
            queryOptions.Token.Returns("00000000-0000-0000-0000-000000000000");
            var repository = Substitute.For<IEnumerable<IRepository>>();
            var build = Substitute.For<IBuild>();
            var yaml = Substitute.For<IYaml>();
            var query = new Query(queryOptions, repository, build, yaml);

            // When
            var getQuery = query.GetQuery.Split('&');

            // Then
            getQuery.FirstOrDefault(x => x.StartsWith("branch=")).Should().Be("branch=develop");
            getQuery.FirstOrDefault(x => x.StartsWith("build=")).Should().Be("build=123");
            getQuery.FirstOrDefault(x => x.StartsWith("commit=")).Should().Be("commit=3c075f8d15aea3b3b40cea0bcf441a30bfd5c5d2");
            getQuery.FirstOrDefault(x => x.StartsWith("flags=")).Should().Be("flags=ui,chrome");
            getQuery.FirstOrDefault(x => x.StartsWith("name=")).Should().Be("name=foo");
            getQuery.FirstOrDefault(x => x.StartsWith("pr=")).Should().Be("pr=bar");
            getQuery.FirstOrDefault(x => x.StartsWith("slug=")).Should().Be("slug=foo%2Fbar");
            getQuery.FirstOrDefault(x => x.StartsWith("tag=")).Should().Be("tag=v1.0");
            getQuery.FirstOrDefault(x => x.StartsWith("token=")).Should().Be("token=00000000-0000-0000-0000-000000000000");
        }

        [Fact]
        public void Should_Set_From_Enviornmentvariable()
        {
            // Given
            Environment.SetEnvironmentVariable("CODECOV_SLUG", "fizz/bang");
            Environment.SetEnvironmentVariable("CODECOV_TOKEN", "10000000-0000-0000-0000-000000000000");
            var queryOptions = Substitute.For<IQueryOptions>();
            var repository = Substitute.For<IEnumerable<IRepository>>();
            var build = Substitute.For<IBuild>();
            var yaml = Substitute.For<IYaml>();
            var query = new Query(queryOptions, repository, build, yaml);

            // When
            var getQuery = query.GetQuery.Split('&');

            // Then
            getQuery.FirstOrDefault(x => x.StartsWith("slug=")).Should().Be("slug=fizz%2Fbang");
            getQuery.FirstOrDefault(x => x.StartsWith("token=")).Should().Be("token=10000000-0000-0000-0000-000000000000");

            // Cleanup
            Environment.SetEnvironmentVariable("CODECOV_SLUG", null);
            Environment.SetEnvironmentVariable("CODECOV_TOKEN", null);
        }

        [Fact]
        public void Should_Set_From_Repository()
        {
            // Given
            var queryOptions = Substitute.For<IQueryOptions>();
            var repository = Substitute.For<IRepository>();
            repository.Branch.Returns("develop");
            repository.Commit.Returns("3c075f8d15aea3b3b40cea0bcf441a30bfd5c5d2");
            repository.Pr.Returns("123");
            repository.Project.Returns(string.Empty);
            repository.ServerUri.Returns(string.Empty);
            repository.Tag.Returns("v1.0");
            repository.Slug.Returns("larz/codecov-exe");
            var build = Substitute.For<IBuild>();
            var yaml = Substitute.For<IYaml>();
            var query = new Query(queryOptions, new[] { repository }, build, yaml);

            // When
            var getQuery = query.GetQuery.Split('&');

            // Then
            getQuery.FirstOrDefault(x => x.StartsWith("branch=")).Should().Be("branch=develop");
            getQuery.FirstOrDefault(x => x.StartsWith("commit=")).Should().Be("commit=3c075f8d15aea3b3b40cea0bcf441a30bfd5c5d2");
            getQuery.FirstOrDefault(x => x.StartsWith("pr=")).Should().Be("pr=123");
            getQuery.FirstOrDefault(x => x.StartsWith("project=")).Should().Be(null);
            getQuery.FirstOrDefault(x => x.StartsWith("server_uri=")).Should().Be(null);
            getQuery.FirstOrDefault(x => x.StartsWith("tag=")).Should().Be("tag=v1.0");
            getQuery.FirstOrDefault(x => x.StartsWith("slug=")).Should().Be("slug=larz%2Fcodecov-exe");
        }

        [Fact]
        public void Should_Set_From_Repository_WithProject()
        {
            // Given
            var queryOptions = Substitute.For<IQueryOptions>();
            var repository = Substitute.For<IRepository>();
            repository.Branch.Returns("develop");
            repository.Commit.Returns("3c075f8d15aea3b3b40cea0bcf441a30bfd5c5d2");
            repository.Pr.Returns("123");
            repository.Project.Returns("projectA");
            repository.ServerUri.Returns("https://dev.azure.com/");
            repository.Tag.Returns("v1.0");
            repository.Slug.Returns("larz/codecov-exe");
            var build = Substitute.For<IBuild>();
            var yaml = Substitute.For<IYaml>();
            var query = new Query(queryOptions, new[] { repository }, build, yaml);

            // When
            var getQuery = query.GetQuery.Split('&');

            // Then
            getQuery.FirstOrDefault(x => x.StartsWith("branch=")).Should().Be("branch=develop");
            getQuery.FirstOrDefault(x => x.StartsWith("commit=")).Should().Be("commit=3c075f8d15aea3b3b40cea0bcf441a30bfd5c5d2");
            getQuery.FirstOrDefault(x => x.StartsWith("pr=")).Should().Be("pr=123");
            getQuery.FirstOrDefault(x => x.StartsWith("project=")).Should().Be("project=projectA");
            getQuery.FirstOrDefault(x => x.StartsWith("server_uri=")).Should().Be("server_uri=https://dev.azure.com/");
            getQuery.FirstOrDefault(x => x.StartsWith("tag=")).Should().Be("tag=v1.0");
            getQuery.FirstOrDefault(x => x.StartsWith("slug=")).Should().Be("slug=larz%2Fcodecov-exe");
        }

        [Fact]
        public void Should_Set_From_Yaml()
        {
            // Given
            var queryOptions = Substitute.For<IQueryOptions>();
            var repository = Substitute.For<IEnumerable<IRepository>>();
            var build = Substitute.For<IBuild>();
            var yaml = Substitute.For<IYaml>();
            yaml.FileName.Returns("codecov.yaml");
            var query = new Query(queryOptions, repository, build, yaml);

            // When
            var getQuery = query.GetQuery.Split('&');

            // Then
            getQuery.FirstOrDefault(x => x.StartsWith("yaml=")).Should().Be("yaml=codecov.yaml");
        }

        [Theory, InlineData(null), InlineData("")]
        public void Should_Used_VersionControlSystem_If_ContiniousIntegrationServer_Retuns_Null_Or_Empty_String(string branchData)
        {
            // Given
            var queryOptions = Substitute.For<IQueryOptions>();
            var continiousIntegrationServer = Substitute.For<IRepository>();
            continiousIntegrationServer.Branch.Returns(branchData);
            var versionControlSystem = Substitute.For<IRepository>();
            versionControlSystem.Branch.Returns("dev");
            var build = Substitute.For<IBuild>();
            var yaml = Substitute.For<IYaml>();
            var query = new Query(queryOptions, new[] { continiousIntegrationServer, versionControlSystem }, build, yaml);

            // When
            var getQuery = query.GetQuery.Split('&');

            // Then
            getQuery.FirstOrDefault(x => x.StartsWith("branch=")).Should().Be("branch=dev");
        }

        [Fact]
        public void Slug_Should_Override_Enviornment_Variable_And_Set_From_CommandLine()
        {
            // Given
            Environment.SetEnvironmentVariable("CODECOV_SLUG", "fizz/bang");
            var queryOptions = Substitute.For<IQueryOptions>();
            queryOptions.Slug.Returns("foo/bar");
            var repository = Substitute.For<IEnumerable<IRepository>>();
            var build = Substitute.For<IBuild>();
            var yaml = Substitute.For<IYaml>();
            var query = new Query(queryOptions, repository, build, yaml);

            // When
            var getQuery = query.GetQuery.Split('&');

            // Then
            getQuery.FirstOrDefault(x => x.StartsWith("slug=")).Should().Be("slug=foo%2Fbar");

            // Clean up
            Environment.SetEnvironmentVariable("CODECOV_SLUG", null);
        }

        [Fact]
        public void Slug_Should_Override_Repository_And_Set_From_CommandLine()
        {
            // Given
            var queryOptions = Substitute.For<IQueryOptions>();
            queryOptions.Slug.Returns("foo/bar");
            var repository = Substitute.For<IRepository>();
            repository.Slug.Returns("fizz%2Fbang");
            var build = Substitute.For<IBuild>();
            var yaml = Substitute.For<IYaml>();
            var query = new Query(queryOptions, new[] { repository }, build, yaml);

            // When
            var getQuery = query.GetQuery.Split('&');

            // Then
            getQuery.FirstOrDefault(x => x.StartsWith("slug=")).Should().Be("slug=foo%2Fbar");
        }

        [Fact]
        public void Slug_Should_Override_Repository_And_Set_From_Enviornment_Variable()
        {
            // Given
            Environment.SetEnvironmentVariable("CODECOV_SLUG", "foo/bar");
            var queryOptions = Substitute.For<IQueryOptions>();
            var repository = Substitute.For<IRepository>();
            repository.Slug.Returns("fizz%2Fbang");
            var build = Substitute.For<IBuild>();
            var yaml = Substitute.For<IYaml>();
            var query = new Query(queryOptions, new[] { repository }, build, yaml);

            // When
            var getQuery = query.GetQuery.Split('&');

            // Then
            getQuery.FirstOrDefault(x => x.StartsWith("slug=")).Should().Be("slug=foo%2Fbar");

            // Clean Up
            Environment.SetEnvironmentVariable("CODECOV_SLUG", null);
        }

        [Fact]
        public void Token_Should_Override_Enviornment_Variable_And_Set_From_CommandLine()
        {
            // Given
            Environment.SetEnvironmentVariable("CODECOV_TOKEN", "00000000-0000-0000-0000-000000000000");
            var queryOptions = Substitute.For<IQueryOptions>();
            queryOptions.Token.Returns("10000000-0000-0000-0000-000000000000");
            var repository = Substitute.For<IEnumerable<IRepository>>();
            var build = Substitute.For<IBuild>();
            var yaml = Substitute.For<IYaml>();
            var query = new Query(queryOptions, repository, build, yaml);

            // When
            var getQuery = query.GetQuery.Split('&');

            // Then
            getQuery.FirstOrDefault(x => x.StartsWith("token=")).Should().Be("token=10000000-0000-0000-0000-000000000000");

            // Clean up
            Environment.SetEnvironmentVariable("CODECOV_TOKEN", null);
        }
    }
}
