using CandidateTesting.JonatasDiebAraujoLima.Interfaces;
using CandidateTesting.JonatasDiebAraujoLima.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CandidateTesting.JonatasDiebAraujoLima.TargetFormats
{
    public class AgoraFormatter : ILogsToTargetFormatter
    {
        public string GetFormat() => "agora";

        public string Format(List<Log> logs)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("# Version: 1.0");
            builder.AppendLine($"# Date: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
            builder.AppendLine("# Fields: provider http-method status-code uri-path time-taken response-size cache-status");

            foreach (var log in logs)
            {
                builder.AppendLine($"\"{log.Provider}\" {log.Method} {log.StatusCode} {log.UriPath} {log.TimeTaken} {log.ResponseSize} {log.CacheStatus}");
            }

            return builder.ToString();
        }
    }
}
