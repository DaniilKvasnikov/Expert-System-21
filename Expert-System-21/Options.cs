using CommandLine;

namespace Expert_System_21
{
    public class Options
    {
        [Option('v', "visual", Required = false, HelpText = "Set output to verbose messages.")]
        public bool Visualisation { get; private set; }

        [Option('d', "debug", Required = false, HelpText = "Set output to verbose messages.")]
        public bool DebugMode { get; private set; }

        [Option('f', "file_name", Required = false, HelpText = "Set output to verbose messages.")]
        public string FileName { get; private set; }
    }
}