using CandidateTesting.JonatasDiebAraujoLima.Interfaces;
using CandidateTesting.JonatasDiebAraujoLima.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace CandidateTesting.JonatasDiebAraujoLima.Adapters
{
    public class MinhaCdnLogAdapter : ILogAdapter
    {
        private readonly string ProviderTextLog = "MINHA CDN";
        private readonly IWebClientWrapper _webClientWrapper;
        private readonly IEnumerable<ILogsToTargetFormatter> _logsToTargetFormatters;

        public MinhaCdnLogAdapter(IWebClientWrapper webClientWrapper, IEnumerable<ILogsToTargetFormatter> logsToTargetFormatters)
        {
            _webClientWrapper = webClientWrapper;
            _logsToTargetFormatters = logsToTargetFormatters;
        }

        public string GetProvider() => "minha_cdn";

        public string Adapt(string sourceUrl, string targetFormat)
        {
            var logs = MapTextToLogs(sourceUrl, ProviderTextLog);

            var formatter = _logsToTargetFormatters.FirstOrDefault(t => t.GetFormat() == targetFormat);
            var txtLogs = formatter.Format(logs);

            return txtLogs;
        }

        private List<Log> MapTextToLogs(string sourceUrl, string provider)
        {
            var logs = new List<Log>();

            using (var stream = _webClientWrapper.OpenRead(sourceUrl))
            using (var reader = new StreamReader(stream))

                while (!reader.EndOfStream)
                {
                    var logLine = reader.ReadLine();

                    var fields = logLine.Split('|');

                    var responseSize = fields[0];
                    var statusCode = fields[1];
                    var cacheStatus = fields[2];
                    var timeTaken = Math.Round(double.Parse(fields[4], CultureInfo.InvariantCulture), 0);

                    var methodAndUri = fields[3].Split(' ');
                    var method = methodAndUri[0].Replace("\"", "");
                    var uriPath = methodAndUri[1].Split('?')[0];

                    var newLog = new Log(provider, method, statusCode, uriPath, timeTaken.ToString(), responseSize, cacheStatus);
                    logs.Add(newLog);
                }

            return logs;
        }
    }
}
