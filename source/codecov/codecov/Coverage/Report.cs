using System;
using System.Collections.Generic;

namespace codecov.Coverage
{
    public static class Report
    {
        private const string Footer = "<<<<<< EOF";

        private const string Header = "<<<<<< network";

        public static string CreateReport(IEnumerable<string> files)
        {
            var report = $"{Header}\n";
            foreach (var file in files)
            {
                report += $"{Read(file)}\n{Footer}\n";
            }

            return report;
        }

        private static string Read(string file)
        {
            if (string.IsNullOrWhiteSpace(file))
            {
                throw new ArgumentException(nameof(file));
            }

            return System.IO.File.ReadAllText(file);
        }
    }
}