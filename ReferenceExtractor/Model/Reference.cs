namespace ReferenceExtractor.Model
{
    public class Reference
    {
        public Reference(string name, string version, ReferenceType referenceType)
        {
            Name = name;
            Version = version;
            ReferenceType = referenceType;
        }

        public ReferenceType ReferenceType { get; set; }

        public string Name { get; set; }

        public string Version { get; set; }
    }
}
