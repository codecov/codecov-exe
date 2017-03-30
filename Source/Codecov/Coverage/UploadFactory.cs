using Codecov.Program;

namespace Codecov.Coverage
{
    public class UploadFactory
    {
        public UploadFactory(Options options, string query)
        {
            Options = options;
            Query = query;
        }

        public IUpload CreateUpload => new Upload(Options, new Url(Options, Query));

        private Options Options { get; }

        private string Query { get; }
    }
}