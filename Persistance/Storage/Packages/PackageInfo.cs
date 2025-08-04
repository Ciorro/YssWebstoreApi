namespace YssWebstoreApi.Persistance.Storage.Packages
{
    public class PackageInfo
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public Entities.OperatingSystem TargetOs { get; set; }

        public PackageInfo(string name, string version, Entities.OperatingSystem targetOs)
        {
            Name = name;
            Version = version;
            TargetOs = targetOs;
        }
    }
}
