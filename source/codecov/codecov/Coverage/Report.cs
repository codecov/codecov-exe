using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace codecov.Coverage
{
    public static class Report
    {
        public static string Reporter(IEnumerable<string> files, IEnumerable<string> enviornmentVariables, string root)
        {
            var report = GetEnviornmentVariables(enviornmentVariables) + GetSourceCodeFiles(root);
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
            return $"{File.ReadAllText(file.Trim())}\n{eof}\n";
        }

        private static string GetEnviornmentVariables(IEnumerable<string> enviornmentVariables)
        {
            if (enviornmentVariables == null)
            {
                return string.Empty;
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

            return enviornmentVariablesNamesAndValues.Count > 0 ? $"{string.Join("\n", enviornmentVariablesNamesAndValues)}\n{env}\n" : string.Empty;
        }

        private static string GetSourceCodeFiles(string root)
        {
            if (string.IsNullOrWhiteSpace(root))
            {
                return string.Empty;
            }

            var gitRoot = root.Trim();
            const string network = "<<<<<< network";
            var files = Directory.GetFiles(gitRoot, "*.*", SearchOption.AllDirectories).Select(f => f.Replace(gitRoot, string.Empty).TrimStart('\\').TrimStart('/')).Where(file => !(file.StartsWith(".git/") || file.StartsWith(".git\\")));
            return $"{string.Join("\n", files)}\n{network}\n";
        }
    }
}