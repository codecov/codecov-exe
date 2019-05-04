using Codecov.Coverage.SourceCode;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Codecov.Tests.Yaml
{
    public class YamlTests
    {
        [WindowsFact]
        public void Should_Find_Yaml_File_If_Exists_On_Windows()
        {
            // Given
            var sourceCode = Substitute.For<ISourceCode>();
            sourceCode.GetAll.Returns(new[] { @"C:\Fake\Class.cs", @"C:\Fake\Interface\IClass.cs", @"C:\Fake\.git", @"C:\Fake\codecov.yaml" });
            var yaml = new Codecov.Yaml.Yaml(sourceCode);

            // When
            var fileName = yaml.FileName;

            // Then
            fileName.Should().Be("codecov.yaml");
        }

        [UnixFact]
        public void Should_Find_Yaml_File_If_Exists_On_Unix()
        {
            // Given
            var sourceCode = Substitute.For<ISourceCode>();
            sourceCode.GetAll.Returns(new[] { @"/Fake/Class.cs", @"/Fake/Interface/IClass.cs", @"/Fake/.git", @"/Fake/codecov.yaml" });
            var yaml = new Codecov.Yaml.Yaml(sourceCode);

            // When
            var fileName = yaml.FileName;

            // Then
            fileName.Should().Be("codecov.yaml");
        }

        [WindowsFact]
        public void Should_Return_Empty_String_If_Yaml_File_Does_Not_Exit_On_Windows()
        {
            // Given
            var sourceCode = Substitute.For<ISourceCode>();
            sourceCode.GetAll.Returns(new[] { @"C:\Fake\Class.cs", @"C:\Fake\Interface\IClass.cs", @"C:\Fake\.git" });
            var yaml = new Codecov.Yaml.Yaml(sourceCode);

            // When
            var fileName = yaml.FileName;

            // Then
            fileName.Should().BeEmpty();
        }

        [UnixFact]
        public void Should_Return_Empty_String_If_Yaml_File_Does_Not_Exit_On_Unix()
        {
            // Given
            var sourceCode = Substitute.For<ISourceCode>();
            sourceCode.GetAll.Returns(new[] { @"/Fake/Class.cs", @"/Fake/Interface/IClass.cs", @"/Fake/.git" });
            var yaml = new Codecov.Yaml.Yaml(sourceCode);

            // When
            var fileName = yaml.FileName;

            // Then
            fileName.Should().BeEmpty();
        }
    }
}
