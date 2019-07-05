using System;
using Codecov.Coverage.Report;
using Codecov.Logger;
using Codecov.Url;

namespace Codecov.Upload
{
    internal abstract class Upload : IUpload
    {
        private readonly Lazy<IReport> _report;

        private readonly Lazy<IUrl> _url;

        protected Upload(IUrl url, IReport report)
        {
            _url = new Lazy<IUrl>(() => url);
            _report = new Lazy<IReport>(() => report);
        }

        protected IReport Report => _report.Value;

        protected IUrl Url => _url.Value;

        public string Uploader()
        {
            try
            {
                var response = Post();
                if (string.IsNullOrWhiteSpace(response))
                {
                    Log.Verbose("Failed to ping codecov.");
                    return string.Empty;
                }

                var s3 = GetPutUrlFromPostResponse(response);
                if (!Put(s3))
                {
                    Log.Warning($"Failed to upload the report with {this.GetType().Name}.");
                    return string.Empty;
                }

                return response;
            }
            catch (Exception ex)
            {
                Log.VerboseException(ex);
                return string.Empty;
            }
        }

        protected abstract string Post();

        protected abstract bool Put(Uri url);

        private static Uri GetPutUrlFromPostResponse(string postResponse)
        {
            var splitResponse = postResponse.Split('\n');
            return new Uri(splitResponse[splitResponse.Length > 1 ? 1 : 0]);
        }
    }
}
