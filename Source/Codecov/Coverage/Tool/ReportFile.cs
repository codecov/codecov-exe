namespace Codecov.Coverage.Tool
{
    internal struct ReportFile
    {
        internal ReportFile(string file, string content)
        {
            File = file;
            Content = content;
        }

        internal string Content { get; }

        internal string File { get; }
    }
}
