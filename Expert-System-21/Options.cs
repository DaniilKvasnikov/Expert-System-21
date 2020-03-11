using CommandLine;

namespace Expert_System_21
{
    public class Options
    {
        [Option('v', "visual", Required = false, HelpText = "Visualisation graph.")]
        public bool Visualisation { get; private set; }

        [Option('d', "debug", Required = false, HelpText = "Debug parser mode.")]
        public bool DebugMode { get; private set; }

        [Option('f', "file_name", Required = false, HelpText = "Input file name.")]
        public string FileName { get; private set; }

        [Option('l', "log", Required = false, HelpText = "Log full info.")]
        public bool FullLog { get; private set; }
    }
}