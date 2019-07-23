using System;
using System.Net.Http;
using Codecov.Coverage.Report;
using Codecov.Logger;
using Codecov.Url;

namespace Codecov.Upload
{
    internal sealed class CodecovFallbackUploader : CodecovUploader
    {
        public CodecovFallbackUploader(IUrl url, IReport report)
            : base(url, report)
        {
        }

        protected override void ConfigureContent(HttpContent content)
        {
            content.Headers.ContentEncoding.Clear();
            content.Headers.ContentEncoding.Add("gzip");
        }

        protected override void ConfigureRequest(HttpRequestMessage request)
        {
            request.Headers.Accept.Clear();
            request.Headers.Accept.ParseAdd("text/plain");
            request.Headers.Add("X-Content-Encoding", "gzip");
        }

        protected override string Post()
            => Url.GetFallbackUrl.ToString();

        protected override bool Put(Uri url)
        {
            using (var request = new HttpRequestMessage(new HttpMethod("POST"), url))
            {
                Log.Information("Uploading to Codecov");
                var response = CreateResponse(request);

                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    Log.Information($"View reports at: {GetReportUrl(content)}");
                }
                else
                {
                    Log.Warning($"Unable to upload coverage report to Codecov. Server returned: ({(int)response.StatusCode}) {response.ReasonPhrase}");

                    if (string.Equals(response.Content.Headers.ContentType.MediaType, "text/plain", StringComparison.OrdinalIgnoreCase))
                    {
                        Log.Warning(response.Content.ReadAsStringAsync().Result);
                    }
                    else
                    {
                        Log.Warning("Unknown reason. Possible reason being invalid parameters.");
                    }
                }

                return response.IsSuccessStatusCode;
            }
        }

        private static string GetReportUrl(string content)
        {
            // The report url is expected to be the second line in the content response
            var splitResponse = content.Split('\n');
            return splitResponse[splitResponse.Length > 1 ? 1 : 0];
        }
    }
}
