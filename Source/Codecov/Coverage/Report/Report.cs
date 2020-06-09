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

        public Report(IReportOptions options, IEnviornmentVariables enviornmentVariables, ISourceCode sourceCode, ICoverage coverage)
        {
            _reportOptions = new Lazy<IReportOptions>(() => options);
            EnviornmentVariables = enviornmentVariables;
            SourceCode = sourceCode;
            Coverage = coverage;
            _reporter = new Lazy<string>(() => $"{Env}{Network}{CombinedCoverage}");
        }

        public string Reporter => _reporter.Value;

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

        private string Env => !EnviornmentVariables.UserEnvironmentVariables.Any() ? string.Empty : $"{string.Join("\n", EnviornmentVariables.UserEnvironmentVariables.Select(x => x.Key.Trim() + "=" + x.Value.Trim()).ToArray())}\n<<<<<< ENV\n";

        private IEnviornmentVariables EnviornmentVariables { get; }

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

        private IReportOptions ReportOptions => _reportOptions.Value;

        private ISourceCode SourceCode { get; }

        private string Convert2RelativePath(string absolutePath)
            => absolutePath.Replace(SourceCode.Directory + Path.DirectorySeparatorChar, string.Empty);
    }
}
