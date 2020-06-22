using Codecov.Services.ContinuousIntegrationServers;
using FluentAssertions;
using Moq;
using Xunit;

namespace Codecov.Tests.Services.ContiniousIntegrationServers
{
    public class TeamCityTests
    {
        [Fact]
        public void Branch_Should_Be_Empty_String_When_Enviornment_Variable_Does_Not_Exits()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            var teamCity = new TeamCity(ev.Object);

            // When
            var branch = teamCity.Branch;

            // Then
            branch.Should().BeEmpty();
        }

        [Fact]
        public void Branch_Should_Be_Set_When_Enviornment_Variable_Exits()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("TEAMCITY_BUILD_BRANCH")).Returns("develop");
            var teamCity = new TeamCity(ev.Object);

            // When
            var branch = teamCity.Branch;

            // Then
            branch.Should().Be("develop");
        }

        [Fact]
        public void Build_Should_Be_Empty_String_When_Enviornment_Variable_Does_Not_Exits()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            var teamCity = new TeamCity(ev.Object);

            // When
            var build = teamCity.Build;

            // Then
            build.Should().BeEmpty();
        }

        [Fact]
        public void Build_Should_Be_Set_When_Enviornment_Variable_Exits()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("TEAMCITY_BUILD_ID")).Returns("123");
            var teamCity = new TeamCity(ev.Object);

            // When
            var build = teamCity.Build;

            // Then
            build.Should().Be("123");
        }

        [Fact]
        public void BuildUrl_Should_Be_Empty_String_When_Enviornment_Variable_Does_Not_Exits()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            var teamCity = new TeamCity(ev.Object);

            // When
            var build = teamCity.BuildUrl;

            // Then
            build.Should().BeEmpty();
        }

        [Fact]
        public void BuildUrl_Should_Be_Set_When_Enviornment_Variable_Exits()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("TEAMCITY_BUILD_URL")).Returns("www.google.com/hello");
            var teamCity = new TeamCity(ev.Object);

            // When
            var build = teamCity.BuildUrl;

            // Then
            build.Should().Be("www.google.com%2Fhello");
        }

        [Fact]
        public void Commit_Should_Be_Empty_String_When_Enviornment_Variable_Does_Not_Exits()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            var teamCity = new TeamCity(ev.Object);

            // When
            var commit = teamCity.Commit;

            // Then
            commit.Should().BeEmpty();
        }

        [Fact]
        public void Commit_Should_Be_Set_Correctly_When_Both_Enviornment_Variable_Exits()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("TEAMCITY_BUILD_COMMIT")).Returns("4491f681062057a9021199f3d19dfa3c3598d43c");
            ev.Setup(s => s.GetEnvironmentVariable("BUILD_VCS_NUMBER")).Returns("5491f681062057a9021199f3d19dfa3c3598d43c");
            var teamCity = new TeamCity(ev.Object);

            // When
            var commit = teamCity.Commit;

            // Then
            commit.Should().Be("4491f681062057a9021199f3d19dfa3c3598d43c");
        }

        [Fact]
        public void Commit_Should_Be_Set_When_Default_Enviornment_Variable_Exits()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("BUILD_VCS_NUMBER")).Returns("5491f681062057a9021199f3d19dfa3c3598d43c");
            var teamCity = new TeamCity(ev.Object);

            // When
            var commit = teamCity.Commit;

            // Then
            commit.Should().Be("5491f681062057a9021199f3d19dfa3c3598d43c");
        }

        [Fact]
        public void Commit_Should_Be_Set_When_Enviornment_Variable_Exits()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("TEAMCITY_BUILD_COMMIT")).Returns("4491f681062057a9021199f3d19dfa3c3598d43c");
            var teamCity = new TeamCity(ev.Object);

            // When
            var commit = teamCity.Commit;

            // Then
            commit.Should().Be("4491f681062057a9021199f3d19dfa3c3598d43c");
        }

        [Theory, InlineData(null), InlineData("")]
        public void Detecter_Should_Be_Empty_String_When_Enviornment_Variable_Does_Not_Exits_Or_Is_Empty(string teamCityData)
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("TEAMCITY_VERSION")).Returns(teamCityData);
            var teamCity = new TeamCity(ev.Object);

            // When
            var detecter = teamCity.Detecter;

            // Then
            detecter.Should().BeFalse();
        }

        [Fact]
        public void Detecter_Should_Be_Set_When_Enviornment_Variable_Exits()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("TEAMCITY_VERSION")).Returns("true");
            var teamCity = new TeamCity(ev.Object);

            // When
            var detecter = teamCity.Detecter;

            // Then
            detecter.Should().BeTrue();
        }

        [Fact]
        public void Http_Slug_Should_Be_Set_When_Enviornment_Variable_Exits()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("TEAMCITY_BUILD_REPOSITORY")).Returns("https://github.com/larzw/codecov-exe.git");
            var teamCity = new TeamCity(ev.Object);

            // When
            var slug = teamCity.Slug;

            // Then
            slug.Should().Be("larzw/codecov-exe");
        }

        [Fact]
        public void Shh_Slug_Should_Be_Set_When_Enviornment_Variable_Exits()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            ev.Setup(s => s.GetEnvironmentVariable("TEAMCITY_BUILD_REPOSITORY")).Returns("git@github.com:larzw/codecov-exe.git");
            var teamCity = new TeamCity(ev.Object);

            // When
            var slug = teamCity.Slug;

            // Then
            slug.Should().Be("larzw/codecov-exe");
        }

        [Fact]
        public void Slug_Should_Be_Empty_String_When_Enviornment_Variable_Does_Not_Exits()
        {
            // Given
            var ev = new Mock<IEnviornmentVariables>();
            var teamCity = new TeamCity(ev.Object);

            // When
            var slug = teamCity.Slug;

            // Then
            slug.Should().BeEmpty();
        }
    }
}
