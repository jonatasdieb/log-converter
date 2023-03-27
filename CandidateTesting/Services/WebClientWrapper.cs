using CandidateTesting.JonatasDiebAraujoLima.Interfaces;
using System.IO;
using System.Net;

namespace CandidateTesting.JonatasDiebAraujoLima.Services
{
    public class WebClientWrapper : IWebClientWrapper
    {
        private readonly WebClient _webClient;

        public WebClientWrapper()
        {
            _webClient = new WebClient();
        }

        public Stream OpenRead(string uri)
        {
            return _webClient.OpenRead(uri);
        }

        public void Dispose()
        {
            _webClient.Dispose();
        }
    }
}
