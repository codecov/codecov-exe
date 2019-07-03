using System;
using System.IO;
using System.Linq;
using Codecov.Services.VersionControlSystems;
using Codecov.Terminal;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Codecov.Tests.Services.VersionControlSystems
{
    public class VersionControlSystemTests
    {
        private static string _systemDrive = Path.GetPathRoot(DriveInfo.GetDrives().First().ToString());
        private static readonly IVersionControlSystemOptions Options = Substitute.For<IVersionControlSystemOptions>(); // Given

        private static readonly ITerminal Terminal = Substitute.For<ITerminal>(); // Given

        [Fact]
        public void Branch_Should_Be_Empty_String_When_Environment_Variable_Does_Not_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("VCS_BRANCH_NAME", null);
            var versionControlSystem = new VersionControlSystem(Options, Terminal);

            // When
            var branch = versionControlSystem.Branch;

            // Then
            branch.Should().BeEmpty();
        }

        [Fact]
        public void Branch_Should_Be_Set_When_Environment_Variable_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("VCS_BRANCH_NAME", "develop");
            var versionControlSystem = new VersionControlSystem(Options, Terminal);

            // When
            var branch = versionControlSystem.Branch;

            // Then
            branch.Should().Be("develop");
        }

        [Fact]
        public void Detecter_Should_Be_False()
        {
            // Given
            var versionControlSystem = new VersionControlSystem(Options, Terminal);

            // When
            var detecter = versionControlSystem.Detecter;

            // Then
            detecter.Should().BeFalse();
        }

        [Fact]
        public void Pr_Should_Be_Empty_String_When_Environment_Variable_Does_Not_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("VCS_PULL_REQUEST", null);
            var versionControlSystem = new VersionControlSystem(Options, Terminal);

            // When
            var pr = versionControlSystem.Pr;

            // Then
            pr.Should().BeEmpty();
        }

        [Fact]
        public void Pr_Should_Be_Set_When_Environment_Variable_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("VCS_PULL_REQUEST", "123");
            var versionControlSystem = new VersionControlSystem(Options, Terminal);

            // When
            var pr = versionControlSystem.Pr;

            // Then
            pr.Should().Be("123");
        }

        [Fact]
        public void Repo_Root_Should_Be_A_Normalized_Path_When_Options_Are_Set()
        {
            // Given
            var options = Substitute.For<IVersionControlSystemOptions>();
            options.RepoRoot.Returns((_systemDrive + "/fake/github/").Replace('\\', '/').Replace("//", "/"));

            var versionControlSystem = new VersionControlSystem(options, Terminal);

            // When
            var repoRoot = versionControlSystem.RepoRoot;

            // Then
            var expected = Path.Combine(_systemDrive, "fake", "github");
            repoRoot.Should().Be(expected);
        }

        [Theory, InlineData(""), InlineData(null)]
        public void Repo_Root_Should_Default_To_Current_Directory_When_Options_Are_Null_Or_Empty_String(string repoRootData)
        {
            // Given
            var options = Substitute.For<IVersionControlSystemOptions>();
            options.RepoRoot.Returns(repoRootData);

            var versionControlSystem = new VersionControlSystem(options, Terminal);

            // When
            var repoRoot = versionControlSystem.RepoRoot;

            // Then
            repoRoot.Should().Be(Directory.GetCurrentDirectory());
        }

        [Fact]
        public void Slug_Should_Be_Empty_String_When_Environment_Variable_Does_Not_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("VCS_SLUG", null);
            var versionControlSystem = new VersionControlSystem(Options, Terminal);

            // When
            var slug = versionControlSystem.Slug;

            // Then
            slug.Should().BeEmpty();
        }

        [Fact]
        public void Slug_Should_Be_Set_When_Environment_Variable_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("VCS_SLUG", "owner/repo");
            var versionControlSystem = new VersionControlSystem(Options, Terminal);

            // When
            var slug = versionControlSystem.Slug;

            // Then
            slug.Should().Be("owner/repo");
        }

        [Fact]
        public void SourceCode_Should_Return_Empty_Collection_If_No_Files_Exit()
        {
            // Given
            Directory.CreateDirectory("./FakeRepo");
            Directory.CreateDirectory("./FakeRepo/Src");

            var options = Substitute.For<IVersionControlSystemOptions>();
            options.RepoRoot.Returns("./FakeRepo");

            var versionControlSystem = new VersionControlSystem(options, Terminal);

            // When
            var sourceCode = versionControlSystem.SourceCode;

            // Then
            sourceCode.Should().BeEmpty();

            // Clean up
            Directory.Delete("./FakeRepo", true);
        }

        [Fact]
        public void SourceCode_Should_Return_File_Collection_If_Files_Exit()
        {
            // Given
            var baseDirectory = Path.Combine(Directory.GetCurrentDirectory(), "FakeRepo");
            Directory.CreateDirectory("./FakeRepo");
            Directory.CreateDirectory("./FakeRepo/Src");
            using (File.Create("./FakeRepo/.git")) { }
            using (File.Create("./FakeRepo/Src/class1.cs")) { }
            using (File.Create("./FakeRepo/README.md")) { }

            var options = Substitute.For<IVersionControlSystemOptions>();
            options.RepoRoot.Returns("./FakeRepo");

            var versionControlSystem = new VersionControlSystem(options, Terminal);

            // When
            versionControlSystem.SourceCode.ToList().Sort();
            var sortedSourceCode = versionControlSystem.SourceCode.ToList();

            // Then
            sortedSourceCode.Count.Should().Be(3);
            sortedSourceCode.Should().Contain(Path.Combine(baseDirectory, ".git"));
            sortedSourceCode.Should().Contain(Path.Combine(baseDirectory, "README.md"));
            sortedSourceCode.Should().Contain(Path.Combine(baseDirectory, "Src", "class1.cs"));

            // Clean up
            Directory.Delete("./FakeRepo", true);
        }

        [Fact]
        public void Tag_Should_Be_Empty_String_When_Environment_Variable_Does_Not_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("VCS_TAG", null);
            var versionControlSystem = new VersionControlSystem(Options, Terminal);

            // When
            var tag = versionControlSystem.Tag;

            // Then
            tag.Should().BeEmpty();
        }

        [Fact]
        public void Tag_Should_Be_Set_When_Environment_Variable_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("VCS_TAG", "v1.0.0");
            var versionControlSystem = new VersionControlSystem(Options, Terminal);

            // When
            var tag = versionControlSystem.Tag;

            // Then
            tag.Should().Be("v1.0.0");
        }
    }
}
