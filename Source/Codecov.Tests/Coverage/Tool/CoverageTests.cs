using System;
using System.IO;
using System.Linq;
using Codecov.Coverage.Tool;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Codecov.Tests.Coverage.Tool
{
    public class CoverageTests
    {
        [Theory, InlineData(null), InlineData(""), InlineData("./ coverage.xml")]
        public void CoverageReport_Should_Be_Empty_If_The_File_Does_Not_Exits(string fileData)
        {
            // Given
            var options = Substitute.For<ICoverageOptions>();
            options.Files.Returns(new[] { fileData });
            var coverage = new Codecov.Coverage.Tool.Coverage(options);

            // When
            Action coverageReport = () =>
            {
                var x = coverage.CoverageReports;
            };

            // Then
            coverageReport.Should().Throw<Exception>().WithMessage("No Report detected.");
        }

        [Fact]
        public void Should_Read_Multiple_Coverage_Reports()
        {
            // Given
            var options = Substitute.For<ICoverageOptions>();
            options.Files.Returns(new[] { "./coverageUnit.xml", "./coverageIntegration.xml" });
            File.WriteAllText("./coverageUnit.xml", "Unit Tests.");
            File.WriteAllText("./coverageIntegration.xml", "Integration Tests.");
            var coverage = new Codecov.Coverage.Tool.Coverage(options);

            // When
            var coverageReport = coverage.CoverageReports.ToList();

            // Then
            coverageReport.Count.Should().Be(2);
            coverageReport[0].File.Should().Be("./coverageUnit.xml");
            coverageReport[0].Content.Should().Be("Unit Tests.");
            coverageReport[1].File.Should().Be("./coverageIntegration.xml");
            coverageReport[1].Content.Should().Be("Integration Tests.");

            // Clean Up
            File.Delete("./coverageUnit.xml");
            File.Delete("./coverageIntegration.xml");
        }

        [Fact]
        public void Should_Read_Single_Coverage_Report()
        {
            // Given
            var options = Substitute.For<ICoverageOptions>();
            options.Files.Returns(new[] { "./coverage.xml" });
            File.WriteAllText("./coverage.xml", "Unit Tests.");
            var coverage = new Codecov.Coverage.Tool.Coverage(options);

            // When
            var coverageReport = coverage.CoverageReports.FirstOrDefault();

            // Then
            coverageReport.File.Should().Be("./coverage.xml");
            coverageReport.Content.Should().Be("Unit Tests.");

            // Clean Up
            File.Delete("./coverage.xml");
        }
    }
}
