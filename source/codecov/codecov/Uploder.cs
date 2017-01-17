using System;
using System.Net;
using System.Net.Http;
using codecov.Coverage;
using codecov.Services.Utils;

namespace codecov
{
    public class Uploder
    {
        public Uploder(Options options, IReport report, IService service)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (report == null)
            {
                throw new ArgumentNullException(nameof(report));
            }

            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            Options = options;
            Report = report.Coverage;
            Url = service.Find;
        }

        private Options Options { get; }

        private string Report { get; }

        private IUrl Url { get; }

        public void Upload()
        {
            TryUploadingV4();

            //if (TryUploadingV2())
            //{
            //    return;
            //}

            //Console.WriteLine("==> Could not upload report.");
        }

        private bool TryUploadingV2()
        {
            //var query = Url.Query(Options);
            return false;
        }

        private void TryUploadingV4()
        {
            var query = Url.Query(Options);
            var client = new HttpClient();
            var url = $"https://codecov.io/upload/v4?{query}";

            // Post/Ping
            var clientPost = new WebClient();
            clientPost.Headers.Add("Content-Type: text/plain");
            var response = clientPost.UploadString(url, "POST", string.Empty);

            Console.WriteLine(response);

            // Put/ Upload report.
            url = response.Trim().Split('\n')[1];
            var clientPut = new WebClient();
            clientPut.Headers.Add("Content-Type: text/plain");
            clientPut.Headers.Add("x-amz-acl: public-read");
            clientPut.Headers.Add("x-amz-storage-class: REDUCED_REDUNDANCY");
            response = clientPut.UploadString(url, "PUT", Report);
        }
    }
}