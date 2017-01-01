using System;
using System.IO;
using YamlDotNet.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandLine;
using RabbitMQ.Client;

namespace ParallelDaemon
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Console.WriteLine("CWOJ new daemon started.");
            var options = CommandLine.Parser.Default.ParseArguments<CommandLineOptions>(args).MapResult(
                p => new { Success = true, Result = p }, p => new { Success = false, Result = new CommandLineOptions() });

            if (!options.Success)
            {
                Console.WriteLine("命令语法不正确。");
                return 1;
            }
            using (var configReader = File.OpenText(options.Result.Config))
            {
                var deserializer = new Deserializer();
                GlobalConfig.Config = deserializer.Deserialize<ApplicationConfig>(configReader);
            }

            var factory = new ConnectionFactory() { HostName = GlobalConfig.Config.MessageServer, Port = GlobalConfig.Config.MessagePort, UserName = "asdf", Password = "asdf" };
            var connection = factory.CreateConnection())

            RequestDispatcher.Start(connection);
            Console.ReadLine();
            return 0;
        }
    }
}
