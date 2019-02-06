using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using Codecov.Coverage.Report;
using Codecov.Logger;
using Codecov.Url;

namespace Codecov.Upload
{
    internal class WebClient : Upload
    {
        public WebClient(IUrl url, IReport report)
            : base(url, report)
        {
        }

        protected override string Post()
        {
            Log.Verboase("Trying to upload using WebClient.");

            var client = new System.Net.WebClient();
            client.Headers.Add("X-Content-Type", "application/x-gzip");
            client.Headers.Add("X-Reduced-Redundancy", "false");
            return client.UploadString(Url.GetUrl, "POST", string.Empty);
        }

        protected override bool Put(Uri url)
        {
            var tempFilePath = WriteReport2TempFile();

            var client = new ExtendedWebClient();
            client.Headers.Add("Content-Type", "application/x-gzip");
            client.Headers.Add("Content-Encoding", "gzip");
            client.UploadFile(url, "PUT", tempFilePath);

            return true;
        }

        private string WriteReport2TempFile()
        {
            var tempFilePath = Path.GetTempFileName();
            using (var stream = new GZipStream(new FileStream(tempFilePath, FileMode.Create, FileAccess.Write, FileShare.None), CompressionLevel.Optimal))
            {
                var content = Encoding.UTF8.GetBytes(Report.Reporter);
                stream.Write(content, 0, content.Length);
            }

            return tempFilePath;
        }

        private class ExtendedWebClient : System.Net.WebClient
        {
            public ExtendedWebClient()
            {
                this.Timeout = 1000000;
            }

            public int Timeout { get; }

            protected override WebRequest GetWebRequest(Uri address)
            {
                var request = base.GetWebRequest(address);
                if (request != null)
                {
                    request.Timeout = Timeout;
                }

                return request;
            }
        }
    }
}
