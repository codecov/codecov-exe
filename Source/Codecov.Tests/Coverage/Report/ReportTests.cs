using System.Collections.Generic;
using System.IO;
using System.Linq;
using Codecov.Coverage.Report;
using Codecov.Coverage.SourceCode;
using Codecov.Coverage.Tool;
using Codecov.Services.ContinuousIntegrationServers;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Codecov.Tests.Coverage.Report
{
    public class ReportTests
    {
        private static string _systemDrive = Path.GetPathRoot(DriveInfo.GetDrives().First().ToString());

        [Fact]
        public void If_DisableNetwork_Is_True_The_Source_Code_Should_Be_Empty()
        {
            // Given
            var enviornmentVariables = Substitute.For<IEnviornmentVariables>();
            enviornmentVariables.GetEnviornmentVariables.Returns(new Dictionary<string, string> { { "foo", "bar" }, { "fizz", "bizz" } });
            var options = Substitute.For<IReportOptions>();
            options.DisableNetwork = true;
            var sourceCode = Substitute.For<ISourceCode>();
            sourceCode.GetAllButCodecovIgnored.Returns(new[] {
                 Path.Combine(_systemDrive, "foo", "Class.cs"),
                 Path.Combine(_systemDrive, "foo", "Interface", "IClass.cs")
            });
            sourceCode.Directory.Returns(Path.Combine(_systemDrive, "foo"));
            var coverage = Substitute.For<ICoverage>();
            coverage.CoverageReports.Returns(new[] { new ReportFile("./coverageUnit.xml", "Unit Tests."), new ReportFile("./coverageIntegration.xml", "Integration Tests.") });
            var report = new Codecov.Coverage.Report.Report(options, enviornmentVariables, sourceCode, coverage);

            // When
            var reporter = report.Reporter;

            // Then
            reporter.Should().Be("foo=bar\nfizz=bizz\n<<<<<< ENV\n# path=./coverageUnit.xml\nUnit Tests.\n<<<<<< EOF\n# path=./coverageIntegration.xml\nIntegration Tests.\n<<<<<< EOF\n");
        }

        [Fact]
        public void Should_Generate_A_Report()
        {
            // Given
            var enviornmentVariables = Substitute.For<IEnviornmentVariables>();
            enviornmentVariables.GetEnviornmentVariables.Returns(new Dictionary<string, string> { { "foo", "bar" }, { "fizz", "bizz" } });
            var options = Substitute.For<IReportOptions>();
            options.DisableNetwork = false;
            var sourceCode = Substitute.For<ISourceCode>();
            sourceCode.GetAllButCodecovIgnored.Returns(new[] {
                 Path.Combine(_systemDrive, "foo", "Class.cs"),
                 Path.Combine(_systemDrive, "foo", "Interface", "IClass.cs")
            });
            sourceCode.Directory.Returns(Path.Combine(_systemDrive, "foo"));
            var coverage = Substitute.For<ICoverage>();
            coverage.CoverageReports.Returns(new[] { new ReportFile("./coverageUnit.xml", "Unit Tests."), new ReportFile("./coverageIntegration.xml", "Integration Tests.") });
            var report = new Codecov.Coverage.Report.Report(options, enviornmentVariables, sourceCode, coverage);

            // When
            var reporter = report.Reporter;

            // Then
            var dirSplit = Path.DirectorySeparatorChar;
            reporter.Should().Be(
                $"foo=bar\nfizz=bizz\n<<<<<< ENV\nClass.cs\nInterface{dirSplit}IClass.cs\n<<<<<< network\n# path=./coverageUnit.xml\nUnit Tests.\n<<<<<< EOF\n# path=./coverageIntegration.xml\nIntegration Tests.\n<<<<<< EOF\n");
        }
    }
}
