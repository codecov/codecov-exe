using System;
using System.IO;
using System.Net;
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
            postRequest.ContentType = "text/plain";
            postRequest.Method = "POST";
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
            putRequest.ContentType = "text/plain";
            putRequest.Method = "PUT";
            putRequest.Headers["x-amz-acl"] = "public-read";
            putRequest.Headers["x-amz-storage-class"] = "REDUCED_REDUNDANCY";
            using (var putStreamWriter = new StreamWriter(putRequest.GetRequestStreamAsync().Result))
            {
                putStreamWriter.Write(Report.Reporter);
            }

            var putResponse = (HttpWebResponse)putRequest.GetResponseAsync().Result;
            return putResponse.StatusCode == HttpStatusCode.OK;
        }
    }
}
