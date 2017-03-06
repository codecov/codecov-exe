using System;
using System.Net;
using System.Text.RegularExpressions;
using codecov.Program;

namespace codecov.Coverage
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
                if (Options.Dump)
                {
                    Log.WriteLine(Url.FullUrl.ToString());
                    Log.WriteLine(report);
                    return;
                }

                Log.Arrow("Uploading Reports.");
                Log.Message($"url: {Url.FullUrl.Scheme}://{Url.FullUrl.Authority}");
                Log.Message($"query: {DisplayUrl}");

                var url = Url.FullUrl.ToString();

                //Post: Ping
                Log.Message("-> Pinging Codecov");
                var post = new WebClient();
                post.Headers.Add("Content-Type: text/plain");
                var response = post.UploadString(url, "POST", string.Empty);

                var resp = response.Split('\n');
                var s3 = new Uri(resp[1]);

                // Put: Upload report.
                url = resp[1];
                var put = new WebClient();
                put.Headers.Add("Content-Type: text/plain");
                put.Headers.Add("x-amz-acl: public-read");
                put.Headers.Add("x-amz-storage-class: REDUCED_REDUNDANCY");
                put.UploadString(url, "PUT", report);

                Log.Message($"-> Uploading to S3 {s3.Scheme }://{s3.Authority}");
                Log.Message($"-> View reports at: {resp[0]}");
            }
            catch
            {
                Log.X("Failed to upload coverage report.");
                throw;
            }
        }
    }
}