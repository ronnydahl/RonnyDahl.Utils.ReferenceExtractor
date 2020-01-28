using System;
using System.IO;
using System.Text;
using CommandLine;
using RonnyDahl.Utils.ReferenceExtractor.Helper;
using RonnyDahl.Utils.ReferenceExtractor.Model;

namespace RonnyDahl.Utils.ReferenceExtractor
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();

            var startPath = string.Empty;
            var outputFile = string.Empty;

            Parser.Default.ParseArguments<Options>(args).WithParsed(option =>
            {
                startPath = option.InputFolder;
                outputFile = option.OutputFile;
            });

            if (string.IsNullOrWhiteSpace(startPath))
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(outputFile))
            {
                return;
            }

            var files = Directory.GetFiles(startPath, "*.csproj", SearchOption.AllDirectories);

            var helper = new ProgramHelper();
            var sb = new StringBuilder();

            sb.AppendLine("Project;DefaultNamespace;AssemblyName;Framework;ReferenceName;Version;ReferenceType");

            foreach (var file in files)
            {
                var result = helper.ProcessProject(file);

                foreach (var item in result.PackageReferences)
                {
                    sb.AppendLine($"{result.Name};{result.DefaultNamespace};{result.AssemblyName};{result.Framework};{item.Name};{item.Version};{item.ReferenceType}");
                }

                foreach (var item in result.References)
                {
                    sb.AppendLine($"{result.Name};{result.DefaultNamespace};{result.AssemblyName};{result.Framework};{item.Name};{item.Version};{item.ReferenceType}");
                }
            }

            File.WriteAllText(outputFile, sb.ToString());

            Console.WriteLine($"CSV exported to: {outputFile}");
            Console.WriteLine();
        }
    }
}
