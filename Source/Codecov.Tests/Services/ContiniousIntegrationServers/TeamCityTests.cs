using System;
using Codecov.Services.ContinuousIntegrationServers;
using FluentAssertions;
using Xunit;

namespace Codecov.Tests.Services.ContiniousIntegrationServers
{
    public class TeamCityTests
    {
        [Fact]
        public void Branch_Should_Be_Empty_String_When_Enviornment_Variable_Does_Not_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("TEAMCITY_BUILD_BRANCH", null);
            var teamCity = new TeamCity();

            // When
            var branch = teamCity.Branch;

            // Then
            branch.Should().BeEmpty();
        }

        [Fact]
        public void Branch_Should_Be_Set_When_Enviornment_Variable_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("TEAMCITY_BUILD_BRANCH", "develop");
            var teamCity = new TeamCity();

            // When
            var branch = teamCity.Branch;

            // Then
            branch.Should().Be("develop");

            // Clean up
            Environment.SetEnvironmentVariable("TEAMCITY_BUILD_BRANCH", null);
        }

        [Fact]
        public void BuildUrl_Should_Be_Empty_String_When_Enviornment_Variable_Does_Not_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("TEAMCITY_BUILD_URL", null);
            var teamCity = new TeamCity();

            // When
            var build = teamCity.BuildUrl;

            // Then
            build.Should().BeEmpty();
        }

        [Fact]
        public void BuildUrl_Should_Be_Set_When_Enviornment_Variable_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("TEAMCITY_BUILD_URL", "www.google.com/hello");
            var teamCity = new TeamCity();

            // When
            var build = teamCity.BuildUrl;

            // Then
            build.Should().Be("www.google.com%2Fhello");

            // Clean up
            Environment.SetEnvironmentVariable("TEAMCITY_BUILD_URL", null);
        }

        [Fact]
        public void Build_Should_Be_Empty_String_When_Enviornment_Variable_Does_Not_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("TEAMCITY_BUILD_ID", null);
            var teamCity = new TeamCity();

            // When
            var build = teamCity.Build;

            // Then
            build.Should().BeEmpty();
        }

        [Fact]
        public void Build_Should_Be_Set_When_Enviornment_Variable_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("TEAMCITY_BUILD_ID", "123");
            var teamCity = new TeamCity();

            // When
            var build = teamCity.Build;

            // Then
            build.Should().Be("123");

            // Clean up
            Environment.SetEnvironmentVariable("TEAMCITY_BUILD_ID", null);
        }

        [Fact]
        public void Commit_Should_Be_Empty_String_When_Enviornment_Variable_Does_Not_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("TEAMCITY_BUILD_COMMIT", null);
            Environment.SetEnvironmentVariable("BUILD_VCS_NUMBER", null);
            var teamCity = new TeamCity();

            // When
            var commit = teamCity.Commit;

            // Then
            commit.Should().BeEmpty();
        }

        [Fact]
        public void Commit_Should_Be_Set_When_Enviornment_Variable_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("TEAMCITY_BUILD_COMMIT", "4491f681062057a9021199f3d19dfa3c3598d43c");
            Environment.SetEnvironmentVariable("BUILD_VCS_NUMBER", null);
            var teamCity = new TeamCity();

            // When
            var commit = teamCity.Commit;

            // Then
            commit.Should().Be("4491f681062057a9021199f3d19dfa3c3598d43c");

            // Clean up
            Environment.SetEnvironmentVariable("TEAMCITY_BUILD_COMMIT", null);
        }

        [Fact]
        public void Commit_Should_Be_Set_When_Default_Enviornment_Variable_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("TEAMCITY_BUILD_COMMIT", null);
            Environment.SetEnvironmentVariable("BUILD_VCS_NUMBER", "5491f681062057a9021199f3d19dfa3c3598d43c");
            var teamCity = new TeamCity();

            // When
            var commit = teamCity.Commit;

            // Then
            commit.Should().Be("5491f681062057a9021199f3d19dfa3c3598d43c");

            // Clean up
            Environment.SetEnvironmentVariable("BUILD_VCS_NUMBER", null);
        }

        [Fact]
        public void Commit_Should_Be_Set_Correctly_When_Both_Enviornment_Variable_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("TEAMCITY_BUILD_COMMIT", "4491f681062057a9021199f3d19dfa3c3598d43c");
            Environment.SetEnvironmentVariable("BUILD_VCS_NUMBER", "5491f681062057a9021199f3d19dfa3c3598d43c");
            var teamCity = new TeamCity();

            // When
            var commit = teamCity.Commit;

            // Then
            commit.Should().Be("4491f681062057a9021199f3d19dfa3c3598d43c");

            // Clean up
            Environment.SetEnvironmentVariable("TEAMCITY_BUILD_COMMIT", null);
            Environment.SetEnvironmentVariable("BUILD_VCS_NUMBER", null);
        }

        [Fact]
        public void Slug_Should_Be_Empty_String_When_Enviornment_Variable_Does_Not_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("TEAMCITY_BUILD_REPOSITORY", null);
            var teamCity = new TeamCity();

            // When
            var slug = teamCity.Slug;

            // Then
            slug.Should().BeEmpty();
        }

        [Fact]
        public void Http_Slug_Should_Be_Set_When_Enviornment_Variable_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("TEAMCITY_BUILD_REPOSITORY", "https://github.com/larzw/codecov-exe.git");
            var teamCity = new TeamCity();

            // When
            var slug = teamCity.Slug;

            // Then
            slug.Should().Be("larzw/codecov-exe");

            // Clean up
            Environment.SetEnvironmentVariable("TEAMCITY_BUILD_REPOSITORY", null);
        }

        [Fact]
        public void Shh_Slug_Should_Be_Set_When_Enviornment_Variable_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("TEAMCITY_BUILD_REPOSITORY", "git@github.com:larzw/codecov-exe.git");
            var teamCity = new TeamCity();

            // When
            var slug = teamCity.Slug;

            // Then
            slug.Should().Be("larzw/codecov-exe");

            // Clean up
            Environment.SetEnvironmentVariable("TEAMCITY_BUILD_REPOSITORY", null);
        }

        [Theory, InlineData(null), InlineData("")]
        public void Detecter_Should_Be_Empty_String_When_Enviornment_Variable_Does_Not_Exits_Or_Is_Empty(string teamCityData)
        {
            // Given
            Environment.SetEnvironmentVariable("TEAMCITY_VERSION", teamCityData);
            var teamCity = new TeamCity();

            // When
            var detecter = teamCity.Detecter;

            // Then
            detecter.Should().BeFalse();

            // Clean up
            Environment.SetEnvironmentVariable("TEAMCITY_VERSION", null);
        }

        [Fact]
        public void Detecter_Should_Be_Set_When_Enviornment_Variable_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("TEAMCITY_VERSION", "true");
            var teamCity = new TeamCity();

            // When
            var detecter = teamCity.Detecter;

            // Then
            detecter.Should().BeTrue();

            // Clean up
            Environment.SetEnvironmentVariable("TEAMCITY_VERSION", null);
        }
    }
}