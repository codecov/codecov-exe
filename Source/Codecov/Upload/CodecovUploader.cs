using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using Codecov.Coverage.Report;
using Codecov.Logger;
using Codecov.Url;

namespace Codecov.Upload
{
    internal class CodecovUploader : Upload
    {
        private static HttpClient client;

        static CodecovUploader()
        {
            if (client == null)
            {
                client = new HttpClient();
                client.DefaultRequestHeaders.UserAgent.ParseAdd($"codecove-exe/{Assembly.GetExecutingAssembly().GetName().Version}");
            }
        }

        public CodecovUploader(IUrl url, IReport report)
            : base(url, report)
        {
        }

        public static void Cleanup()
        {
            client?.Dispose();
            client = null;
        }

        protected virtual void ConfigureContent(HttpContent content)
        {
            content.Headers.ContentEncoding.Clear();
            content.Headers.ContentEncoding.Add("gzip");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-gzip");
        }

        protected virtual void ConfigureRequest(HttpRequestMessage request)
        {
            request.Headers.TryAddWithoutValidation("x-amz-acl", "public-read");
        }

        protected virtual HttpResponseMessage CreateResponse(HttpRequestMessage request)
        {
            ConfigureRequest(request);

            request.Content = new ByteArrayContent(GetReportBytes());

            ConfigureContent(request.Content);

            var response = client.SendAsync(request).Result;
            return response;
        }

        protected byte[] GetReportBytes()
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var stream = new GZipStream(memoryStream, CompressionLevel.Optimal))
                {
                    var content = Encoding.UTF8.GetBytes(Report.Reporter);
                    stream.Write(content, 0, content.Length);
                }

                return memoryStream.ToArray();
            }
        }

        protected override string Post()
        {
            Log.Verboase("Trying to upload using HttpClient");
            using (var request = new HttpRequestMessage(new HttpMethod("POST"), this.Url.GetUrl))
            {
                request.Headers.TryAddWithoutValidation("X-Reduced-Redundancy", "false");
                request.Headers.TryAddWithoutValidation("X-Content-Type", "application/x-gzip");

                Log.Information("Pinging Codecov");
                var response = client.SendAsync(request).Result;
                if (!response.IsSuccessStatusCode)
                {
                    Log.Warning($"Unable to ping Codecov. Server returned: ({response.StatusCode}) {response.ReasonPhrase}");
                    return string.Empty;
                }

                return response.Content.ReadAsStringAsync().Result;
            }
        }

        protected override bool Put(Uri url)
        {
            using (var request = new HttpRequestMessage(new HttpMethod("PUT"), url))
            {
                Log.Information("Uploading");
                var response = CreateResponse(request);

                return response.IsSuccessStatusCode;
            }
        }
    }
}
