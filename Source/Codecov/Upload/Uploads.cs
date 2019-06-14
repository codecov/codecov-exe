using System;
using System.Collections.Generic;
using Codecov.Coverage.Report;
using Codecov.Logger;
using Codecov.Terminal;
using Codecov.Url;

namespace Codecov.Upload
{
    internal class Uploads : IUpload
    {
        private readonly Lazy<IEnumerable<IUpload>> _uploaders;

        public Uploads(IUrl url, IReport report)
        {
            _uploaders = new Lazy<IEnumerable<IUpload>>(() => SetUploaders(url, report));
        }

        private IEnumerable<IUpload> Uploaders => _uploaders.Value;

        public string Uploader()
        {
            foreach (var upload in Uploaders)
            {
                var response = upload.Uploader();
                if (!string.IsNullOrWhiteSpace(response))
                {
                    return response;
                }

                Log.Verboase("Uploader failed.");
            }

            throw new Exception("Failed to upload the report.");
        }

        private static IEnumerable<IUpload> SetUploaders(IUrl url, IReport report)
        {
            return new List<IUpload> {
                new CodecovUploader(url, report),
                new CodecovFallbackUploader(url, report)
            };
        }
    }
}
