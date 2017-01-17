using System;

namespace codecov.Coverage
{
    public class Report : IReport
    {
        public string Coverage { get; }

        public Report(string file)
        {
            Coverage = Read(file);
        }

        private static string Read(string file)
        {
            if (string.IsNullOrWhiteSpace(file))
            {
                throw new ArgumentException(nameof(file));
            }

            return Map(System.IO.File.ReadAllText(file));
        }

        private static string Map(string report)
        {
            return $"<<<<<< network\n{report}\n<<<<<< EOF";
        }
    }
}
