using System.Collections.Generic;
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
        [Fact]
        public void Should_Generate_A_Report()
        {
            // Given
            var enviornmentVariables = Substitute.For<IEnviornmentVariables>();
            enviornmentVariables.GetEnviornmentVariables.Returns(new Dictionary<string, string> { { "foo", "bar" }, { "fizz", "bizz" } });
            var options = Substitute.For<IReportOptions>();
            options.DisableNetwork = false;
            var sourceCode = Substitute.For<ISourceCode>();
            sourceCode.GetAllButCodecovIgnored.Returns(new[] { @"C:\foo\Class.cs", @"C:\foo\Interface\IClass.cs" });
            sourceCode.Directory.Returns(@"C:\foo");
            var coverage = Substitute.For<ICoverage>();
            coverage.CoverageReports.Returns(new[] { new ReportFile("./coverageUnit.xml", "Unit Tests."), new ReportFile("./coverageIntegration.xml", "Integration Tests.") });
            var report = new Codecov.Coverage.Report.Report(options, enviornmentVariables, sourceCode, coverage);

            // When
            var reporter = report.Reporter;

            // Then
            reporter.Should().Be("foo=bar\nfizz=bizz\n<<<<<< ENV\nClass.cs\nInterface\\IClass.cs\n<<<<<< network\n# path=./coverageUnit.xml\nUnit Tests.\n<<<<<< EOF\n# path=./coverageIntegration.xml\nIntegration Tests.\n<<<<<< EOF\n");
        }

        [Fact]
        public void If_DisableNetwork_Is_True_The_Source_Code_Should_Be_Empty()
        {
            // Given
            var enviornmentVariables = Substitute.For<IEnviornmentVariables>();
            enviornmentVariables.GetEnviornmentVariables.Returns(new Dictionary<string, string> { { "foo", "bar" }, { "fizz", "bizz" } });
            var options = Substitute.For<IReportOptions>();
            options.DisableNetwork = true;
            var sourceCode = Substitute.For<ISourceCode>();
            sourceCode.GetAllButCodecovIgnored.Returns(new[] { @"C:\foo\Class.cs", @"C:\foo\Interface\IClass.cs" });
            sourceCode.Directory.Returns(@"C:\foo");
            var coverage = Substitute.For<ICoverage>();
            coverage.CoverageReports.Returns(new[] { new ReportFile("./coverageUnit.xml", "Unit Tests."), new ReportFile("./coverageIntegration.xml", "Integration Tests.") });
            var report = new Codecov.Coverage.Report.Report(options, enviornmentVariables, sourceCode, coverage);

            // When
            var reporter = report.Reporter;

            // Then
            reporter.Should().Be("foo=bar\nfizz=bizz\n<<<<<< ENV\n# path=./coverageUnit.xml\nUnit Tests.\n<<<<<< EOF\n# path=./coverageIntegration.xml\nIntegration Tests.\n<<<<<< EOF\n");
        }
    }
}