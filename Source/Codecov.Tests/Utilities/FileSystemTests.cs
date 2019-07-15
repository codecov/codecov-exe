using System.IO;
using System.Linq;
using Codecov.Utilities;
using FluentAssertions;
using Xunit;

namespace Codecov.Tests.Utilities
{
    public class FileSystemTests
    {
        private static string _systemDrive = Path.GetPathRoot(DriveInfo.GetDrives().First().ToString());

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

        [UnixFact]
        public void NormalizedPath_Should_Change_Backward_Slashes_To_Forward_Slashes_On_Unix()
        {
            // Given
            const string path = @"\home\fake\github";

            // When
            var normalizedPath = FileSystem.NormalizedPath(path);

            // Then
            normalizedPath.Should().Be(@"/home/fake/github");
        }

        [WindowsFact]
        public void NormalizedPath_Should_Change_Forward_Slashes_To_Backward_Slashes_On_Windows()
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
            var actual = Path.Combine(Directory.GetCurrentDirectory(), "fake", "github");
            normalizedPath.Should().Be(actual);
        }

        [Fact]
        public void NormalizedPath_Should_Have_The_Same_Case()
        {
            // Given
            string path = Path.Combine(_systemDrive, "Fake", "GitHub");

            // When
            var normalizedPath = FileSystem.NormalizedPath(path);

            // Then
            normalizedPath.Should().Be(path);
        }

        [Fact]
        public void NormalizedPath_Should_Remove_Ending_Slash()
        {
            // Given
            string path = Path.Combine(_systemDrive, "fake", "github") + Path.DirectorySeparatorChar;

            // When
            var normalizedPath = FileSystem.NormalizedPath(path);

            // Then
            normalizedPath.Should().Be(path.TrimEnd(Path.DirectorySeparatorChar));
        }

        [Fact]
        public void NormalizedPath_Should_Work_If_Path_Has_Spaces_In_It()
        {
            // Given
            string path = Path.Combine(_systemDrive, "fake path", "github");

            // When
            var normalizedPath = FileSystem.NormalizedPath(path);

            // Then
            normalizedPath.Should().Be(path);
        }
    }
}
