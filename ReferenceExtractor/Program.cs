using System;
using System.IO;
using ReferenceExtractor.Helper;

namespace ReferenceExtractor
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();

            var path = args.Length != 0 ? args[0] : string.Empty;
            //path = @"C:\git\HafslundStrom\HafslundData.Authorize\src\HafslundData.Authorize.csproj";

            string[] filenames = Directory.GetFiles(path, "*.csproj", SearchOption.AllDirectories);

            //foreach (var item in filenames)
            //{
            //    Console.WriteLine(item);

            //}

            if (string.IsNullOrWhiteSpace(path))
            {
                Console.WriteLine("It would really help if you provide a .csproj path");

                return;
            }

            var helper = new PackageHelper();

            foreach (var file in filenames)
            {
                var result = helper.ProcessProject(file);

                foreach (var item in result.PackageReferences)
                {
                    Console.WriteLine($"{result.Name};{result.DefaultNamespace};{result.AssemblyName};{result.Framework};{item.Name};{item.Version};{item.ReferenceType}");
                }

                foreach (var item in result.References)
                {
                    Console.WriteLine($"{result.Name};{result.DefaultNamespace};{result.AssemblyName};{result.Framework};{item.Name};{item.Version};{item.ReferenceType}");
                }
            }
        }
    }
}