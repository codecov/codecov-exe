using System.Text.RegularExpressions;
using Codecov.Program;
using System;
using System.IO;
using System.Net;

namespace Codecov.Coverage
{
    public class Upload : IUpload
    {
        public Upload(Options options, IUrl url)
        {
            Options = options;
            Url = url;
        }

        private string DisplayUrl
        {
            get
            {
                var url = Url.FullUrl.ToString();
                Log.Verbose(url);

                var regex = new Regex(@"token=\w{8}-\w{4}-\w{4}-\w{4}-\w{12}&");
                return regex.Replace(url, "token=&");
            }
        }

        private Options Options { get; }

        private IUrl Url { get; }

        public void Uploader(string report)
        {
            try
            {
                var url = Url.FullUrl;
                if (Options.Dump)
                {
                    Log.WriteLine(Url.FullUrl.ToString());
                    Log.WriteLine(report);
                    return;
                }

                Log.Arrow("Uploading Reports.");
                Log.Message($"url: {Url.FullUrl.Scheme}://{Url.FullUrl.Authority}");
                Log.Message($"query: {DisplayUrl}");          

                //Post: Ping
                Log.Message("-> Pinging Codecov");
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
                string reportUrl;
                using (var postStreamReader = new StreamReader(postResponse.GetResponseStream()))
                {
                    var response = postStreamReader.ReadToEnd();
                    Log.Verbose(response);
                    var splitResponse = response.Split('\n');
                    s3 = new Uri(splitResponse[1]);
                    reportUrl = splitResponse[0];
                }

                //Put: Upload report.
                var putRequest = (HttpWebRequest)WebRequest.Create(s3);
                putRequest.ContentType = "text/plain";
                putRequest.Method = "PUT";
                putRequest.Headers["x-amz-acl"] = "public-read";
                putRequest.Headers["x-amz-storage-class"] = "REDUCED_REDUNDANCY";
                using (var putStreamWriter = new StreamWriter(putRequest.GetRequestStreamAsync().Result))
                {
                    putStreamWriter.Write(report);
                }
                var putResponse = (HttpWebResponse)putRequest.GetResponseAsync().Result;
                if (putResponse.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception("Failed to upload the report.");
                }
                Log.Message($"-> Uploading to S3 {s3.Scheme }://{s3.Authority}");
                Log.Message($"-> View reports at: {reportUrl}");
            }
            catch
            {
                Log.X("Failed to upload coverage report.");
                throw;
            }
        }
    }
}