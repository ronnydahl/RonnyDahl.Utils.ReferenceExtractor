using System;
using System.IO;
using System.Text;
using RonnyDahl.Utils.ReferenceExtractor.Helper;

namespace RonnyDahl.Utils.ReferenceExtractor
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();

            var path = args.Length != 0 ? args[0] : string.Empty;

            if (string.IsNullOrWhiteSpace(path))
            {
                Console.WriteLine("Usage: dotnet run [start directory]");

                return;
            }

            var files = Directory.GetFiles(path, "*.csproj", SearchOption.AllDirectories);

            var helper = new ProgramHelper();
            var sb = new StringBuilder();

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

            File.WriteAllText(@"c:\temp\trash\references.csv", sb.ToString());
        }
    }
}
