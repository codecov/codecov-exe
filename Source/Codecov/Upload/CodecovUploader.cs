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
        private static HttpClient _client;

        static CodecovUploader()
        {
            if (_client == null)
            {
                _client = new HttpClient();
                _client.DefaultRequestHeaders.UserAgent.ParseAdd($"codecove-exe/{Assembly.GetExecutingAssembly().GetName().Version}");
            }
        }

        public CodecovUploader(IUrl url, IReport report)
            : base(url, report)
        {
        }

        public static void Cleanup()
        {
            _client?.Dispose();
            _client = null;
        }

        protected virtual void ConfigureContent(HttpContent content)
        {
            content.Headers.ContentEncoding.Clear();
            content.Headers.ContentEncoding.Add("gzip");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-gzip");
        }

        protected virtual void ConfigureRequest(HttpRequestMessage request) { }

        protected virtual HttpResponseMessage CreateResponse(HttpRequestMessage request)
        {
            request.Content = new ByteArrayContent(GetReportBytes());

            ConfigureContent(request.Content);

            var response = _client.SendAsync(request).Result;
            return response;
        }

        protected override string Post()
        {
            Log.Verboase("Trying to upload using HttpClient");
            using (var request = new HttpRequestMessage(new HttpMethod("POST"), Url.GetUrl))
            {
                request.Headers.TryAddWithoutValidation("X-Reduced-Redundancy", "false");
                request.Headers.TryAddWithoutValidation("X-Content-Type", "application/x-gzip");

                Log.Information("Pinging Codecov");
                var response = _client.SendAsync(request).Result;
                if (!response.IsSuccessStatusCode)
                {
                    Log.Warning($"Unable to ping Codecov. Server returned: ({(int)response.StatusCode}) {response.ReasonPhrase}");
                    if (string.Equals(response.Content.Headers.ContentType.MediaType, "text/plain", StringComparison.OrdinalIgnoreCase))
                    {
                        Log.Warning(response.Content.ReadAsStringAsync().Result);
                    }
                    else
                    {
                        Log.Warning("Unknown reason. Possible reason being invalid parameters.");
                    }

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
                using (var response = CreateResponse(request))
                {
                    var success = response.IsSuccessStatusCode;

                    if (!success)
                    {
                        ReportFailure(response);
                    }

                    return success;
                }
            }
        }

        protected void ReportFailure(HttpResponseMessage message)
        {
            Log.Warning($"Unable to upload coverage report to Codecov. Server returned: ({(int)message.StatusCode}) {message.ReasonPhrase}");

            if (string.Equals(message.Content.Headers.ContentType.MediaType, "text/plain", StringComparison.OrdinalIgnoreCase))
            {
                Log.Warning(message.Content.ReadAsStringAsync().Result);
            }
            else
            {
                Log.Warning("Unknown reason. Possible reason being invalid parameters.");
            }
        }

        private byte[] GetReportBytes()
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
    }
}
