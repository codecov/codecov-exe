using System;
using System.IO;
using System.Linq;
using Codecov.Coverage.SourceCode;
using Codecov.Coverage.Tool;
using Codecov.Services.ContinuousIntegrationServers;

namespace Codecov.Coverage.Report
{
    internal class Report : IReport
    {
        private readonly Lazy<string> _reporter;
        private readonly Lazy<IReportOptions> _reportOptions;

        public Report(IReportOptions options, IEnvironmentVariables environmentVariables, ISourceCode sourceCode, ICoverage coverage)
        {
            _reportOptions = new Lazy<IReportOptions>(() => options);
            EnvironmentVariables = environmentVariables;
            SourceCode = sourceCode;
            Coverage = coverage;
            _reporter = new Lazy<string>(() => $"{Env}{Network}{CombinedCoverage}");
        }

        public string Reporter => _reporter.Value;

        private IReportOptions ReportOptions => _reportOptions.Value;

        private string CombinedCoverage
        {
            get
            {
                var codecovReport = string.Empty;
                var coverages = Coverage.CoverageReports.ToArray();

                if (!coverages.AsEnumerable().Any())
                {
                    throw new Exception("No coverage report discovered.");
                }

                return coverages.Aggregate(codecovReport, (current, coverage) => current + $"# path={coverage.File}\n{coverage.Content}\n<<<<<< EOF\n");
            }
        }

        private ICoverage Coverage { get; }

        private string Env => !EnvironmentVariables.GetEnvironmentVariables.Any() ? string.Empty : $"{string.Join("\n", EnvironmentVariables.GetEnvironmentVariables.Select(x => x.Key.Trim() + "=" + x.Value.Trim()).ToArray())}\n<<<<<< ENV\n";

        private IEnvironmentVariables EnvironmentVariables { get; }

        private ISourceCode SourceCode { get; }

        private string Network
        {
            get
            {
                if (ReportOptions.DisableNetwork)
                {
                    return string.Empty;
                }

                const string network = "<<<<<< network\n";
                if (!SourceCode.GetAllButCodecovIgnored.Any())
                {
                    return network;
                }

                var codecovSourceCode = string.Empty;
                var sourceCode = SourceCode.GetAllButCodecovIgnored;
                return $"{sourceCode.Aggregate(codecovSourceCode, (current, source) => current + $"{Convert2RelativePath(source)}\n")}{network}";
            }
        }

        private string Convert2RelativePath(string absolutePath)
        {
            return absolutePath.Replace(SourceCode.Directory + Path.DirectorySeparatorChar, string.Empty);
        }
    }
}
