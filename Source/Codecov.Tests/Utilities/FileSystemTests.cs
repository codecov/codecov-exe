using System.IO;
using Codecov.Utilities;
using FluentAssertions;
using Xunit;

namespace Codecov.Tests.Utilities
{
    public class FileSystemTests
    {
        [Fact]
        public void NormalizedPath_Should_Be_Empty_If_Path_Is_Empty()
        {
            // Given
            var path = string.Empty;

            // When
            var normalizedPath = FileSystem.NormalizedPath(path);

            // Then
            normalizedPath.Should().BeEmpty();
        }

        [Fact]
        public void NormalizedPath_Should_Be_Empty_If_Path_Is_Null()
        {
            // Given
            string path = null;

            // When
            var normalizedPath = FileSystem.NormalizedPath(path);

            // Then
            normalizedPath.Should().BeEmpty();
        }

        [Fact]
        public void NormalizedPath_Should_Change_Forward_Slashes_To_Backward_Slashes()
        {
            // Given
            const string path = @"c:/fake/github";

            // When
            var normalizedPath = FileSystem.NormalizedPath(path);

            // Then
            normalizedPath.Should().Be(@"c:\fake\github");
        }

        [Fact]
        public void NormalizedPath_Should_Create_Aboslute_Path_From_Relative_Path()
        {
            // Given
            const string path = @".\fake\github";

            // When
            var normalizedPath = FileSystem.NormalizedPath(path);

            // Then
            var actual = $@"{Directory.GetCurrentDirectory()}\fake\github";
            normalizedPath.Should().Be(actual);
        }

        [Fact]
        public void NormalizedPath_Should_Have_The_Same_Case()
        {
            // Given
            const string path = @"C:\Fake\GitHub";

            // When
            var normalizedPath = FileSystem.NormalizedPath(path);

            // Then
            normalizedPath.Should().Be(@"C:\Fake\GitHub");
        }

        [Fact]
        public void NormalizedPath_Should_Remove_Ending_Slash()
        {
            // Given
            const string path = @"c:\fake\github\";

            // When
            var normalizedPath = FileSystem.NormalizedPath(path);

            // Then
            normalizedPath.Should().Be(@"c:\fake\github");
        }

        [Fact]
        public void NormalizedPath_Should_Work_If_Path_Has_Spaces_In_It()
        {
            // Given
            const string path = @"C:\fake path\github";

            // When
            var normalizedPath = FileSystem.NormalizedPath(path);

            // Then
            normalizedPath.Should().Be(@"C:\fake path\github");
        }
    }
}
