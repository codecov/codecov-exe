using System;
using System.IO;
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
            var codecovYamlFullPath = SourceCode.GetAll.FirstOrDefault(file =>
            {
                var fileName = Path.GetFileName(file);
                return fileName.Equals(".codecov.yaml", StringComparison.OrdinalIgnoreCase) ||
                       fileName.Equals("codecov.yaml", StringComparison.OrdinalIgnoreCase) ||
                       fileName.Equals(".codecov.yml", StringComparison.OrdinalIgnoreCase) ||
                       fileName.Equals("codecov.yml", StringComparison.OrdinalIgnoreCase);
            });

            return string.IsNullOrWhiteSpace(codecovYamlFullPath)
                ? string.Empty
                : Path.GetFileName(codecovYamlFullPath);
        }
    }
}
