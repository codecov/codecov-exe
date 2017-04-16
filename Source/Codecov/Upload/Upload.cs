using System;
using System.IO;
using System.Net;
using Codecov.Coverage.Report;
using Codecov.Url;

namespace Codecov.Upload
{
    internal class Upload : IUpload
    {
        public Upload(IUrl url, IReport report)
        {
            Url = url;
            Report = report;
        }

        private IReport Report { get; }

        private IUrl Url { get; }

        public string Uploader()
        {
            var url = Url.GetUrl;

            // Post: Ping
            var postRequest = (HttpWebRequest)WebRequest.Create(url);
            postRequest.ContentType = "text/plain";
            postRequest.Method = "POST";
            using (var postStreamWriter = new StreamWriter(postRequest.GetRequestStreamAsync().Result))
            {
                postStreamWriter.Write(string.Empty);
            }

            var postResponse = (HttpWebResponse)postRequest.GetResponseAsync().Result;
            if (postResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Failed to ping Codecov.");
            }

            Uri s3;
            string response;
            using (var postStreamReader = new StreamReader(postResponse.GetResponseStream()))
            {
                response = postStreamReader.ReadToEnd();
                var splitResponse = response.Split('\n');
                s3 = new Uri(splitResponse[1]);
            }

            // Put: Upload report.
            var putRequest = (HttpWebRequest)WebRequest.Create(s3);
            putRequest.ContentType = "text/plain";
            putRequest.Method = "PUT";
            putRequest.Headers["x-amz-acl"] = "public-read";
            putRequest.Headers["x-amz-storage-class"] = "REDUCED_REDUNDANCY";
            using (var putStreamWriter = new StreamWriter(putRequest.GetRequestStreamAsync().Result))
            {
                putStreamWriter.Write(Report.Reporter);
            }

            var putResponse = (HttpWebResponse)putRequest.GetResponseAsync().Result;
            if (putResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Failed to upload the report.");
            }

            return response;
        }
    }
}