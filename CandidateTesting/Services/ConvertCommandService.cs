using CandidateTesting.JonatasDiebAraujoLima.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace CandidateTesting.JonatasDiebAraujoLima.Services
{
    public class ConvertCommandService : ICommand
    {
        private readonly IEnumerable<ILogAdapter> _logAdapters;
        private readonly ISaveConvertedLogs _saveConvertedLogs;

        public ConvertCommandService(IEnumerable<ILogAdapter> logAdapters, ISaveConvertedLogs saveConvertedLogs)
        {
            _logAdapters = logAdapters;
            _saveConvertedLogs = saveConvertedLogs; 
        }

        public string GetContext() => "convert";

        public void Execute(string[] args)
        {
            /*
             * Futuramente poderia incluir os argumentos 'provider' e 'targetFormat' (args[2] e args[2]) e dessa forma o padrão Adapter 
             * entraria em cena pra fazer a conversão de acordo com a necessidade de cada Provider para o TargetFormat escolhido, deixando
             * o código já aberto para receber novos Providers e TargetFormats.
             */
            string provider = "minha_cdn";
            string targetFormat = "agora";

            var adapter = _logAdapters.FirstOrDefault(a => a.GetProvider() == provider);

            Validate(args, adapter);

            string sourceUrl = args[0];
            string targetPath = args[1];            

            var convertedLogs = adapter.Adapt(sourceUrl, targetFormat);

            _saveConvertedLogs.Save(convertedLogs, targetPath);
        }


        private void Validate(string[] args, ILogAdapter adapter)
        {
            if (args.Length != 2)
                throw new ArgumentException($"Correct format: {GetContext()} <sourceUrl> <targetPath>");

            if (adapter is null)
                throw new ArgumentException($"Unknown Provider");

            string sourceUrl = args[0];
            string targetPath = args[1];

            if(!sourceUrl.EndsWith(".txt") || !targetPath.EndsWith(".txt"))
                throw new ArgumentException("sourceUrl and targetPath should be a .txt file");
        }

    }
}
