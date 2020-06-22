using System;
using System.Collections.Generic;
using System.Linq;
using Codecov.Coverage.Report;
using Codecov.Exceptions;
using Codecov.Logger;
using Codecov.Url;

namespace Codecov.Upload
{
    internal class Uploads : IUpload
    {
        private readonly Lazy<IEnumerable<IUpload>> _uploaders;

        public Uploads(IUrl url, IReport report, IEnumerable<string> features)
        {
            _uploaders = new Lazy<IEnumerable<IUpload>>(() => SetUploaders(url, report, features));
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

            throw new UploadException("Failed to upload the report.");
        }

        private static IEnumerable<IUpload> SetUploaders(IUrl url, IReport report, IEnumerable<string> features)
        {
            var uploaders = new List<IUpload>();

            if (!features.Any(f => string.Compare(f, "s3", StringComparison.OrdinalIgnoreCase) == 0))
            {
                uploaders.Add(new CodecovUploader(url, report));
            }

            uploaders.Add(new CodecovFallbackUploader(url, report));

            return uploaders;
        }
    }
}
