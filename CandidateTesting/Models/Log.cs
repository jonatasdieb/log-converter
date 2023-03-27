namespace CandidateTesting.JonatasDiebAraujoLima.Models
{
    public class Log
    {
        public string Provider { get; private set; }
        public string Method { get; private set; }
        public string StatusCode { get; private set; }
        public string UriPath { get; private set; }
        public string TimeTaken{ get; private set; }
        public string ResponseSize { get; private set; }
        public string CacheStatus { get; private set; }

        public Log(string provider, string method, string statusCode, string uriPath, string timeTaken, string responseSize, string cacheStatus)
        {
            Provider = provider;
            Method = method;
            StatusCode = statusCode;
            UriPath = uriPath;
            TimeTaken = timeTaken;
            ResponseSize = responseSize;
            CacheStatus = cacheStatus;  
        }
    }
}
