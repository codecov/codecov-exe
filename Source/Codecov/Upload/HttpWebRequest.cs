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
    internal class HttpWebRequest : Upload
    {
        public HttpWebRequest(IUrl url, IReport report)
            : base(url, report)
        {
        }

        protected override string Post()
        {
            Log.Verboase("Trying to upload using HttpWebRequest.");

            var postRequest = (System.Net.HttpWebRequest)WebRequest.Create(Url.GetUrl);
            postRequest.Method = "POST";
            postRequest.Headers["X-Reduced-Redundancy"] = "false";
            postRequest.Headers["X-Content-Type"] = "application/x-gzip";
            using (var postStreamWriter = new StreamWriter(postRequest.GetRequestStreamAsync().Result))
            {
                postStreamWriter.Write(string.Empty);
            }

            var postResponse = (HttpWebResponse)postRequest.GetResponseAsync().Result;
            if (postResponse.StatusCode != HttpStatusCode.OK)
            {
                return string.Empty;
            }

            using (var postStreamReader = new StreamReader(postResponse.GetResponseStream()))
            {
                return postStreamReader.ReadToEnd();
            }
        }

        protected override bool Put(Uri url)
        {
            var putRequest = (System.Net.HttpWebRequest)WebRequest.Create(url);
            putRequest.ContentType = "application/x-gzip";
            putRequest.Method = "PUT";
            putRequest.Headers["Content-Encoding"] = "gzip";
            putRequest.Headers["x-amz-acl"] = "public-read";
            using (var putStreamWriter = new GZipStream(putRequest.GetRequestStreamAsync().Result, CompressionLevel.Optimal))
            {
                var content = Encoding.UTF8.GetBytes(Report.Reporter);
                putStreamWriter.Write(content, 0, content.Length);
            }

            var putResponse = (HttpWebResponse)putRequest.GetResponseAsync().Result;

            if (putResponse.StatusCode != HttpStatusCode.OK)
            {
                Log.Verboase($"Cannot send to {url}. Received {putResponse.StatusCode}");
                return false;
            }

            return true;
        }
    }
}
