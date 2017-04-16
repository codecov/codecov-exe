using System.Linq;
using Codecov.Services.VersionControlSystems;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Codecov.Tests.Coverage.SourceCode
{
    public class SourceCodeTests
    {
        [Fact]
        public void GetAll_Should_Get_All_The_Source_Code()
        {
            // Given
            var versionControlSystem = Substitute.For<IVersionControlSystem>();
            versionControlSystem.SourceCode.Returns(new[] { @"C:\Fake\Class.cs", @"C:\Fake\Interface\IClass.cs", @"C:\Fake\.git" });
            var sourceCode = new Codecov.Coverage.SourceCode.SourceCode(versionControlSystem);

            // When
            var getAll = sourceCode.GetAll.ToList();

            // Then
            getAll.Count.Should().Be(3);
            getAll[0].Should().Be(@"C:\Fake\Class.cs");
            getAll[1].Should().Be(@"C:\Fake\Interface\IClass.cs");
            getAll[2].Should().Be(@"C:\Fake\.git");
        }

        [Fact]
        public void GetAllButCodecovIgnored_Should_Get_All_Source_Code_That_Is_Not_Ignored_By_Codecov()
        {
            // Given
            var versionControlSystem = Substitute.For<IVersionControlSystem>();
            versionControlSystem.SourceCode.Returns(new[] { @"C:\Fake\Class.cs", @"C:\Fake\Interface\IClass.cs", @"C:\Fake\.git" });
            var sourceCode = new Codecov.Coverage.SourceCode.SourceCode(versionControlSystem);

            // When
            var getAllButCodecovIgnored = sourceCode.GetAllButCodecovIgnored.ToList();

            // Then
            getAllButCodecovIgnored.Count.Should().Be(2);
            getAllButCodecovIgnored[0].Should().Be(@"C:\Fake\Class.cs");
            getAllButCodecovIgnored[1].Should().Be(@"C:\Fake\Interface\IClass.cs");
        }
    }
}