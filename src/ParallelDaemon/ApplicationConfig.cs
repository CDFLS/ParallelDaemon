using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandLine;
using YamlDotNet.Serialization;

namespace ParallelDaemon
{
    public class ApplicationConfig
    {
        [YamlMember(Alias = "message_server")]
        public string MessageServer { get; set; }
        [YamlMember(Alias = "message_port")]
        public int MessagePort { get; set; }

        [YamlMember(Alias = "message_exchange_main")]
        public string MainMessageExchange { get; set; }

        [YamlMember(Alias = "message_exchange_web")]
        public string WebMessageExchange { get; set; }
    }

    public class CommandLineOptions
    {
        [Option('c', Default = "config.yml", HelpText = "配置文件路径", Required = false)]
        public string Config { get; set; }
    }
}
