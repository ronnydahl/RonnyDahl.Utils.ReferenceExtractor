using System.Collections.Generic;

namespace RonnyDahl.Utils.ReferenceExtractor.Model
{
    public class Project
    {
        public string Name { get; set; }

        public string AssemblyName { get; set; }

        public string DefaultNamespace { get; set; }

        public string Framework { get; set; }

        public IEnumerable<Reference> PackageReferences { get; set; }

        public IEnumerable<Reference> References { get; set; }
    }
}