using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using RonnyDahl.Utils.ReferenceExtractor.Model;

namespace RonnyDahl.Utils.ReferenceExtractor.Helper
{
    public class ProgramHelper
    {
        public Project ProcessProject(string path)
        {
            var document = XDocument.Load(path);

            var projectName = new FileInfo(path).Name;

            var project = GetProject(document);
            project.Name = projectName;

            project.PackageReferences = GetReferences(document, ReferenceType.PackageReference);
            project.References = GetReferences(document, ReferenceType.AssemblyReference);

            return project;
        }

        private Project GetProject(XDocument document)
        {
            XNamespace msbuild = "http://schemas.microsoft.com/developer/msbuild/2003";

            var references = document
                .Element(msbuild + "Project")
                ?.Elements(msbuild + "PropertyGroup")
                .Select(refElem => refElem)
                .ToList();

            var frameworkVersion = references?.Elements().FirstOrDefault(f => f.Name.LocalName == "TargetFrameworkVersion")?.Value;
            var defaultNamespace = references?.Elements().FirstOrDefault(f => f.Name.LocalName == "RootNamespace")?.Value;
            var assemblyName = references?.Elements().FirstOrDefault(f => f.Name.LocalName == "AssemblyName")?.Value;

            var result = new Project
            {
                AssemblyName = assemblyName, 
                DefaultNamespace = defaultNamespace, 
                Framework = frameworkVersion
            };

            return result;
        }

        private IEnumerable<Reference> GetReferences(XDocument document, ReferenceType referenceType)
        {
            XNamespace msbuild = "http://schemas.microsoft.com/developer/msbuild/2003";

            var result = new List<Reference>();

            var refType = referenceType == ReferenceType.PackageReference ? "PackageReference" : "Reference";

            var references = document
                .Element(msbuild + "Project")
                ?.Elements(msbuild + "ItemGroup")
                .Elements(msbuild + refType)
                .Select(refElem => refElem)
                .ToList();

            if (references == null)
                return result;

            foreach (var reference in references)
            {
                var version = ExtractVersion(reference);
                var name = ExtractName(reference);

                result.Add(new Reference(name, version, referenceType));
            }

            return result;
        }

        private string ExtractName(XElement reference)
        {
            var attributeValue = reference.Attribute("Include")?.Value;

            if (!string.IsNullOrWhiteSpace(attributeValue))
                return attributeValue;

            var elementValue = reference.Elements().FirstOrDefault(f => f.Name.LocalName == "Version")?.Value;

            return !string.IsNullOrWhiteSpace(elementValue) ? elementValue : string.Empty;
        }

        private string ExtractVersion(XElement reference)
        {
            var attributeValue = reference.Attribute("Version")?.Value;

            if (!string.IsNullOrWhiteSpace(attributeValue))
                return attributeValue;

            var elementVersion = reference.Elements().FirstOrDefault(f => f.Name.LocalName == "Version")?.Value;

            return !string.IsNullOrWhiteSpace(elementVersion) ? elementVersion : string.Empty;
        }
    }
}
