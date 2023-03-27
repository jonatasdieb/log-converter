using CandidateTesting.JonatasDiebAraujoLima.Interfaces;
using CandidateTesting.JonatasDiebAraujoLima.StartupConfig;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CandidateTesting.JonatasDiebAraujoLima
{
    public class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = BuildServiceProvider();

            /*
             * Obtém os comandos disponíveis que, no caso, irá retornar apenas ConvertCommandService referente ao 'convert'.
             * Mas já estaria pronto caso o sistema passasse a permitir outros comandos.
             */
            var commands = GetCommands(serviceProvider);

            try
            {   
                ValidateArgs(args, commands);
                var normalizedArgs = NormalizeArgs(args);

                var command = commands.FirstOrDefault(c => c.GetContext() == args[0]);

                command.Execute(args.Skip(1).ToArray());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static IEnumerable<ICommand> GetCommands(ServiceProvider serviceProvider)
        {
            return serviceProvider.GetServices<ICommand>();
        }      

        public static void ValidateArgs(string[] args, IEnumerable<ICommand> commands)
        {
            if (args.Length == 0)
                throw new ArgumentException("Command not found");

            if (commands.FirstOrDefault(c => c.GetContext() == args[0]) == null)
                throw new ArgumentException($"Unknown command: {args[0]}");            
        }

        public static string[] NormalizeArgs(string[] args)
        {
            return args.Select(a => a.ToLower()).ToArray();
        }

        public static ServiceProvider BuildServiceProvider()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.RegisterDependencyResolver();
            return serviceCollection.BuildServiceProvider();
        }
    }
}
