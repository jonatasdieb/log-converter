using CandidateTesting.JonatasDiebAraujoLima.Adapters;
using CandidateTesting.JonatasDiebAraujoLima.Interfaces;
using CandidateTesting.JonatasDiebAraujoLima.Services;
using CandidateTesting.JonatasDiebAraujoLima.TargetFormats;
using JonatasDiebAraujoLima.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace CandidateTesting.JonatasDiebAraujoLima.StartupConfig
{
    public static class DependencyResolver
    {
        public static void RegisterDependencyResolver(this IServiceCollection services)
        {               
            services.AddSingleton<IWebClientWrapper, WebClientWrapper>();
            services.AddScoped<ICommand, ConvertCommand>();
            services.AddScoped<ILogAdapter, MinhaCdnLogAdapter>();         
            services.AddScoped<ILogsToTargetFormatter, AgoraFormatter>();
            services.AddScoped<ISaveConvertedLogs, SaveConvertedLogs>();
        }
    }
}
