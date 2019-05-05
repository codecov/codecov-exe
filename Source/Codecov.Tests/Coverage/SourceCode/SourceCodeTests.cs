using System.IO;
using System.Linq;
using Codecov.Services.VersionControlSystems;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Codecov.Tests.Coverage.SourceCode
{
    public class SourceCodeTests
    {
        private static string _systemDrive = Path.GetPathRoot(DriveInfo.GetDrives().First().ToString());

        [Fact]
        public void GetAll_Should_Get_All_The_Source_Code()
        {
            // Given
            var allPaths = new[] {
                Path.Combine(_systemDrive, "Fake", "Class.cs"),
                Path.Combine(_systemDrive, "Fake", "Interface", "IClass.cs"),
                Path.Combine(_systemDrive, "Fake", ".git")
            };
            var versionControlSystem = Substitute.For<IVersionControlSystem>();
            versionControlSystem.SourceCode.Returns(allPaths);
            var sourceCode = new Codecov.Coverage.SourceCode.SourceCode(versionControlSystem);

            // When
            var getAll = sourceCode.GetAll.ToList();

            // Then
            getAll.Should().HaveCount(3);
            getAll.Should().Contain(allPaths);
        }

        [Fact]
        public void GetAllButCodecovIgnored_Should_Get_All_Source_Code_That_Is_Not_Ignored_By_Codecov()
        {
            // Given
            var allPaths = new[] {
                Path.Combine(_systemDrive, "Fake", "Class.cs"),
                Path.Combine(_systemDrive, "Fake", "Interface", "IClass.cs"),
                Path.Combine(_systemDrive, "Fake", ".git")
            };
            var versionControlSystem = Substitute.For<IVersionControlSystem>();
            versionControlSystem.SourceCode.Returns(allPaths);
            var sourceCode = new Codecov.Coverage.SourceCode.SourceCode(versionControlSystem);

            // When
            var getAllButCodecovIgnored = sourceCode.GetAllButCodecovIgnored.ToList();

            // Then
            getAllButCodecovIgnored.Should().HaveCount(2);
            getAllButCodecovIgnored.Should().Contain(allPaths.Where(p => !p.EndsWith(".git")));
        }
    }
}
