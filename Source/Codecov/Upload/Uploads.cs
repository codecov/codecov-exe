using System;
using System.Collections.Generic;
using Codecov.Coverage.Report;
using Codecov.Logger;
using Codecov.Terminal;
using Codecov.Url;

namespace Codecov.Upload
{
    internal class Uploads : IUpload
    {
        private readonly Lazy<IEnumerable<IUpload>> _uploaders;

        public Uploads(IUrl url, IReport report, IDictionary<TerminalName, ITerminal> terminals)
        {
            _uploaders = new Lazy<IEnumerable<IUpload>>(() => SetUploaders(url, report, terminals));
        }

        private IEnumerable<IUpload> Uploaders => _uploaders.Value;

        public string Uploader()
        {
            foreach (var upload in Uploaders)
            {
                var response = upload.Uploader();
                if (!string.IsNullOrWhiteSpace(response))
                {
                    return response;
                }

                Log.Verboase("Uploader failed.");
            }

            throw new Exception("Failed to upload the report.");
        }

        private static IEnumerable<IUpload> SetUploaders(IUrl url, IReport report, IDictionary<TerminalName, ITerminal> terminals)
        {
            var uploaders = new List<IUpload> { new HttpWebRequest(url, report) };

            if (terminals[TerminalName.Powershell].Exits)
            {
                uploaders.Add(new WebClient(url, report, terminals[TerminalName.Powershell]));
            }

            return uploaders;
        }
    }
}
