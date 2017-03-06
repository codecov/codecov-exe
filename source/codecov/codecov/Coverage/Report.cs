using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using codecov.Program;

namespace codecov.Coverage
{
    public static class Report
    {
        public static string Reporter(IEnumerable<string> files, IEnumerable<string> enviornmentVariables, string sourceCodeFiles)
        {
            Log.Arrow("Reading reports.");
            var names = $"    + {string.Join("\n    + ", files)}";
            Log.WriteLine(names);
            const string network = "<<<<<< network";
            var report = $"{GetEnviornmentVariables(enviornmentVariables)}{GetSourceCodeFiles(sourceCodeFiles)}{network}\n";
            report = files.Aggregate(report, (current, file) => current + GetCoverageReport(file));
            return report.TrimEnd('\n');
        }

        private static string GetCoverageReport(string file)
        {
            if (string.IsNullOrWhiteSpace(file))
            {
                return string.Empty;
            }

            const string eof = "<<<<<< EOF";
            return $"# path={file}\n{File.ReadAllText(file.Trim())}\n{eof}\n";
        }

        private static string GetEnviornmentVariables(IEnumerable<string> enviornmentVariables)
        {
            if (enviornmentVariables == null)
            {
                enviornmentVariables = Enumerable.Empty<string>();
            }

            const string env = "<<<<<< ENV";
            var enviornmentvariableNames = new HashSet<string>();

            foreach (var enviornmentvariableName in enviornmentVariables)
            {
                if (!string.IsNullOrWhiteSpace(enviornmentvariableName))
                {
                    enviornmentvariableNames.Add(enviornmentvariableName.Trim());
                }
            }

            var codeCovEnviornmenVariableNames = Environment.GetEnvironmentVariable("CODECOV_ENV");
            if (!string.IsNullOrWhiteSpace(codeCovEnviornmenVariableNames))
            {
                foreach (var codeCovEnviornmenVariableName in codeCovEnviornmenVariableNames.Split(','))
                {
                    enviornmentvariableNames.Add(codeCovEnviornmenVariableName.Trim());
                }
            }

            var enviornmentVariablesNamesAndValues = (from name in enviornmentvariableNames let value = Environment.GetEnvironmentVariable(name) where !string.IsNullOrWhiteSpace(value) select $"{name.Trim()}={value.Trim()}").ToList();

            if (enviornmentVariablesNamesAndValues.Count < 1)
            {
                return string.Empty;
            }

            Log.Arrow("Appending build variables");
            var validEnviornmentVariableNames = (from name in enviornmentvariableNames let value = Environment.GetEnvironmentVariable(name) where !string.IsNullOrWhiteSpace(value) select name.Trim()).ToList();

            var names = $"    + {string.Join("\n    + ", validEnviornmentVariableNames)}";
            Log.WriteLine(names);

            return $"{string.Join("\n", enviornmentVariablesNamesAndValues)}\n{env}\n";
        }

        private static string GetSourceCodeFiles(string sourceCodeFiles) => !string.IsNullOrWhiteSpace(sourceCodeFiles) ? $"{sourceCodeFiles}\n" : string.Empty;
    }
}