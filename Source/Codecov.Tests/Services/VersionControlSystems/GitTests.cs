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
    public class GitTests
    {
        private static string _systemDrive = Path.GetPathRoot(DriveInfo.GetDrives().First().ToString());

        [Fact]
        public void Branch_Should_Return_Correct_Branch_If_Exits()
        {
            // Given
            var terminal = Substitute.For<ITerminal>();
            terminal.Run("git", $@"-C ""{Directory.GetCurrentDirectory()}"" rev-parse --abbrev-ref HEAD").Returns("develop");
            var options = Substitute.For<IVersionControlSystemOptions>();
            var git = new Git(options, terminal);

            // When
            var branch = git.Branch;

            // Then
            branch.Should().Be("develop");
        }

        [Theory, InlineData(null), InlineData(""), InlineData("HEAD")]
        public void Branch_Should_Return_Empty_String(string branchData)
        {
            // Given
            var terminal = Substitute.For<ITerminal>();
            terminal.Run("git", $@"-C ""{Directory.GetCurrentDirectory()}"" rev-parse --abbrev-ref HEAD").Returns(branchData);
            var options = Substitute.For<IVersionControlSystemOptions>();
            var git = new Git(options, terminal);

            // When
            var branch = git.Branch;

            // Then
            branch.Should().BeEmpty();
        }

        [Fact]
        public void Commit_Should_Return_Correct_Commit_If_Exits()
        {
            // Given
            var terminal = Substitute.For<ITerminal>();
            terminal.Run("git", $@"-C ""{Directory.GetCurrentDirectory()}"" rev-parse HEAD").Returns("11");
            var options = Substitute.For<IVersionControlSystemOptions>();
            var git = new Git(options, terminal);

            // When
            var commt = git.Commit;

            // Then
            commt.Should().Be("11");
        }

        [Theory, InlineData(null), InlineData("")]
        public void Commit_Should_Return_Empty_String(string commitData)
        {
            // Given
            var terminal = Substitute.For<ITerminal>();
            terminal.Run("git", $@"-C ""{Directory.GetCurrentDirectory()}"" rev-parse HEAD").Returns(commitData);
            var options = Substitute.For<IVersionControlSystemOptions>();
            var git = new Git(options, terminal);

            // When
            var commit = git.Commit;

            // Then
            commit.Should().BeEmpty();
        }

        [Theory, InlineData(null), InlineData("")]
        public void Detecter_Should_Be_False_If_Returns_Null_Or_Empty_String(string terminalData)
        {
            // Given
            var terminal = Substitute.For<ITerminal>();
            terminal.Run("git", "--version").Returns(terminalData);
            Directory.CreateDirectory(".git");
            var options = Substitute.For<IVersionControlSystemOptions>();
            var git = new Git(options, terminal);

            // When
            var detecter = git.Detecter;

            // Then
            detecter.Should().BeFalse();

            // Clean up
            Directory.Delete(".git");
        }

        [Fact]
        public void Detecter_Should_Be_False_If_Dot_Git_Directory_Does_Not_Exit()
        {
            // Given
            var terminal = Substitute.For<ITerminal>();
            terminal.Run("git", "--version").Returns("foo");
            var options = Substitute.For<IVersionControlSystemOptions>();
            var git = new Git(options, terminal);

            // When
            var detecter = git.Detecter;

            // Then
            detecter.Should().BeFalse();
        }

        [Fact]
        public void Detecter_Should_Be_True_If_Does_Not_Returns_Null_Or_Empty_String()
        {
            // Given
            var terminal = Substitute.For<ITerminal>();
            terminal.Run("git", "--version").Returns("foo");
            Directory.CreateDirectory(".git");
            var options = Substitute.For<IVersionControlSystemOptions>();
            var git = new Git(options, terminal);

            // When
            var detecter = git.Detecter;

            // Then
            detecter.Should().BeTrue();

            // Clean up
            Directory.Delete(".git");
        }

        [Theory, InlineData(null), InlineData("")]
        public void RepoRoot_Should_Default_To_Current_Directory_If_Get_Fails(string terminalData)
        {
            // Given
            var terminal = Substitute.For<ITerminal>();
            terminal.Run("git", "rev-parse --show-toplevel").Returns(terminalData);
            var options = Substitute.For<IVersionControlSystemOptions>();
            options.RepoRoot.Returns(string.Empty);
            var git = new Git(options, terminal);

            // When
            var repoRoot = git.RepoRoot;

            // Then
            repoRoot.Should().Be(Directory.GetCurrentDirectory());
        }

        [Theory, InlineData(null), InlineData("")]
        public void RepoRoot_Should_Get_From_Git_If_Options_Are_Not_Set(string optionsData)
        {
            // Given
            var rootDir = Path.Combine(_systemDrive, "Fake");
            var terminal = Substitute.For<ITerminal>();
            terminal.Run("git", "rev-parse --show-toplevel").Returns(rootDir);
            var options = Substitute.For<IVersionControlSystemOptions>();
            options.RepoRoot.Returns(optionsData);
            var git = new Git(options, terminal);

            // When
            var repoRoot = git.RepoRoot;

            // Then
            repoRoot.Should().Be(rootDir);
        }

        [WindowsTheory("Not yet implemented in unix environment"), InlineData(@".\Fake"), InlineData(@"./Fake")]
        public void RepoRoot_Should_Return_Absolute_Path_When_Given_Relative_Path(string repoRootData)
        {
            // Given
            var terminal = Substitute.For<ITerminal>();
            var options = Substitute.For<IVersionControlSystemOptions>();
            options.RepoRoot.Returns(repoRootData);
            var git = new Git(options, terminal);

            // When
            var repoRoot = git.RepoRoot;

            // Then
            var expected = Path.Combine(Directory.GetCurrentDirectory(), "Fake");
            repoRoot.Should().Be(expected);
        }

        [Fact]
        public void Should_Get_SourceCode()
        {
            // Given
            var terminal = Substitute.For<ITerminal>();
            terminal.Run("git", $@"-C ""{Directory.GetCurrentDirectory()}"" ls-tree --full-tree -r HEAD --name-only").Returns("Class.cs");
            var options = Substitute.For<IVersionControlSystemOptions>();
            var git = new Git(options, terminal);

            // When
            var sourceCode = git.SourceCode.ToList();

            // Then
            sourceCode.Count.Should().Be(1);
            var expected = Path.Combine(Directory.GetCurrentDirectory(), "Class.cs");
            sourceCode[0].Should().Be(expected);
        }

        [Theory, InlineData("Class.cs\nIClass.cs"), InlineData("\nClass.cs\nIClass.cs\n\n")]
        public void Should_Get_SourceCode_Seperated_By_NewLines(string terminalData)
        {
            // Given
            var terminal = Substitute.For<ITerminal>();
            terminal.Run("git", $@"-C ""{Directory.GetCurrentDirectory()}"" ls-tree --full-tree -r HEAD --name-only").Returns(terminalData);
            var options = Substitute.For<IVersionControlSystemOptions>();
            var git = new Git(options, terminal);

            // When
            var sourceCode = git.SourceCode.ToList();

            // Then
            sourceCode.Count.Should().Be(2);
            var expected1 = Path.Combine(Directory.GetCurrentDirectory(), "Class.cs");
            var expected2 = Path.Combine(Directory.GetCurrentDirectory(), "IClass.cs");
            sourceCode[0].Should().Be(expected1);
            sourceCode[1].Should().Be(expected2);
        }

        [Theory, InlineData(null), InlineData("")]
        public void Should_Return_Empty_Collection_If_SourceCode_Does_Not_Exit(string terminalData)
        {
            // Given
            var terminal = Substitute.For<ITerminal>();
            terminal.Run("git", $@"-C ""{Directory.GetCurrentDirectory()}"" ls-tree --full-tree -r HEAD --name-only").Returns(terminalData);
            var options = Substitute.For<IVersionControlSystemOptions>();
            var git = new Git(options, terminal);

            // When
            var source = git.SourceCode;

            // Then
            source.Should().Equal(Enumerable.Empty<string>());
        }

        [Theory, InlineData("https://github.com/larzw/codecov-exe.git"), InlineData("git@github.com:larzw/codecov-exe.git")]
        public void Slug_Should_Return_Correct_Result(string slugData)
        {
            // Given
            var terminal = Substitute.For<ITerminal>();
            terminal.Run("git", $@"-C ""{Directory.GetCurrentDirectory()}"" config --get remote.origin.url").Returns(slugData);
            var options = Substitute.For<IVersionControlSystemOptions>();
            var git = new Git(options, terminal);

            // When
            var slug = git.Slug;

            // Then
            slug.Should().Be("larzw/codecov-exe");
        }

        [Theory, InlineData(null), InlineData(""), InlineData("NotASlug")]
        public void Slug_Should_Return_Empty_String(string slugData)
        {
            // Given
            var terminal = Substitute.For<ITerminal>();
            terminal.Run("git", $@"-C ""{Directory.GetCurrentDirectory()}"" config --get remote.origin.url").Returns(slugData);
            var options = Substitute.For<IVersionControlSystemOptions>();
            var git = new Git(options, terminal);

            // When
            var slug = git.Slug;

            // Then
            slug.Should().BeEmpty();
        }

        [Theory, InlineData(null), InlineData("")]
        public void SourceCode_Should_Be_Empty_Collection_If_Git_Returns_Null_Or_Empty_String(string terminalData)
        {
            // Given
            var terminal = Substitute.For<ITerminal>();
            terminal.Run("git", $@"-C ""{Directory.GetCurrentDirectory()}"" ls-tree --full-tree -r HEAD --name-only").Returns(terminalData);
            var options = Substitute.For<IVersionControlSystemOptions>();
            var git = new Git(options, terminal);

            // When
            var sourceCode = git.SourceCode;

            // Then
            sourceCode.Should().BeEmpty();
        }
    }
}
