using System;
using System.Linq;
using Codecov.Coverage.SourceCode;

namespace Codecov.Yaml
{
    internal class Yaml : IYaml
    {
        private readonly Lazy<string> _fileName;

        public Yaml(ISourceCode sourceCode)
        {
            SourceCode = sourceCode;
            _fileName = new Lazy<string>(LoadFileName);
        }

        public string FileName => _fileName.Value;

        private ISourceCode SourceCode { get; }

        private string LoadFileName()
        {
            var codecovYamlFullPath = SourceCode.GetAll.FirstOrDefault(file => file.ToLower().EndsWith(@"\.codecov.yaml") || file.ToLower().EndsWith(@"\codecov.yaml") || file.ToLower().EndsWith(@"\.codecov.yml") || file.ToLower().EndsWith(@"\codecov.yml"));
            if (string.IsNullOrWhiteSpace(codecovYamlFullPath))
            {
                return string.Empty;
            }

            var codecovYamlFullPathSplit = codecovYamlFullPath.Split('\\');
            return !codecovYamlFullPathSplit.Any() ? string.Empty : codecovYamlFullPathSplit[codecovYamlFullPathSplit.Length - 1];
        }
    }
}
