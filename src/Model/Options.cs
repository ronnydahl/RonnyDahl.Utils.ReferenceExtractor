using CommandLine;

namespace RonnyDahl.Utils.ReferenceExtractor.Model
{
    public class Options
    {
        [Option('i', "InputFolder", Required = true, HelpText = "The start path to retrieve references")]
        public string InputFolder { get; set; }

        [Option('o', "OutputFile", Required = true, HelpText = "Path to output file")]
        public string OutputFile { get; set; }
    }
}
