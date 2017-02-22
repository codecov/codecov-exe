using System;
using System.IO;
using System.Linq;
using codecov.Program;

namespace codecov.Coverage
{
    public class Report
    {
        private const string Footer = "<<<<<< EOF";
        private const string Header = "<<<<<< network";

        public Report(Options options)
        {
            Options = options;
        }

        public string Reporter
        {
            get
            {
                var report = $"{GetFiles()}\n{Header}\n";
                foreach (var file in Options.File)
                {
                    report += $"{Read(file)}\n{Footer}\n";
                }

                return report.TrimEnd('\n');
            }
        }

        private Options Options { get; }

        private static string Read(string file)
        {
            if (string.IsNullOrWhiteSpace(file))
            {
                throw new ArgumentException(nameof(file));
            }

            return File.ReadAllText(file);
        }

        private string GetFiles()
        {
            var files = Directory.GetFiles(Options.Root, "*.*", SearchOption.AllDirectories).Select(f => f.Replace(Options.Root, string.Empty).TrimStart('\\').TrimStart('/')).Where(file => !(file.StartsWith(".git/") || file.StartsWith(".git\\")));
            return string.Join("\n", files);
        }
    }
}