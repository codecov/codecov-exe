using System;
using System.Net;

namespace codecov.Coverage
{
    public class Upload : IUpload
    {
        public void Uploader(string report, string url)
        {
            //Post: Ping
            var post = new WebClient();
            post.Headers.Add("Content-Type: text/plain");
            var response = post.UploadString(url, "POST", string.Empty);
            Console.WriteLine(response);

            // Put: Upload report.
            url = response.Trim().Split('\n')[1];
            var put = new WebClient();
            put.Headers.Add("Content-Type: text/plain");
            put.Headers.Add("x-amz-acl: public-read");
            put.Headers.Add("x-amz-storage-class: REDUCED_REDUNDANCY");
            put.UploadString(url, "PUT", report);
        }
    }
}