using System.IO;
using System.Linq;
using Codecov.Coverage.SourceCode;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Codecov.Tests.Yaml
{
    public class YamlTests
    {
        private static readonly string _systemDrive = Path.GetPathRoot(DriveInfo.GetDrives().First().ToString());

        [Theory]
        [InlineData("codecov.yaml")]
        [InlineData(".codecov.yaml")]
        [InlineData("codecov.yml")]
        [InlineData(".codecov.yml")]
        public void Should_Find_Yaml_File_If_Exists(string configFileName)
        {
            // Given
            var sourceCode = Substitute.For<ISourceCode>();
            sourceCode.GetAll.Returns(new[] {
                Path.Combine(_systemDrive, "Fake", "Class.cs"),
                Path.Combine(_systemDrive, "Fake", "Interface", "IClass.cs"),
                Path.Combine(_systemDrive, "Fake", ".git"),
                Path.Combine(_systemDrive, "Fake", configFileName)
            });
            var yaml = new Codecov.Yaml.Yaml(sourceCode);

            // When
            var fileName = yaml.FileName;

            // Then
            fileName.Should().Be(configFileName);
        }

        [Fact]
        public void Should_Return_Empty_String_If_Yaml_File_Does_Not_Exit()
        {
            // Given
            var sourceCode = Substitute.For<ISourceCode>();
            sourceCode.GetAll.Returns(new[] {
                Path.Combine(_systemDrive, "Fake", "Class.cs"),
                Path.Combine(_systemDrive, "Fake", "Interface", "IClass.cs"),
                Path.Combine(_systemDrive, "Fake", ".git")
            });
            var yaml = new Codecov.Yaml.Yaml(sourceCode);

            // When
            var fileName = yaml.FileName;

            // Then
            fileName.Should().BeEmpty();
        }
    }
}
