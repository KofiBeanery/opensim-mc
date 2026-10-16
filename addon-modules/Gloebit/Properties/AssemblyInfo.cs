using Mono.Addins;
using Mono.Addins.Description;

// Registers the module with Mono.Addins so the region module loader discovers it
// (NGC v1.0.49 used its own plugin registry + addin.xml instead).
[assembly: Addin("Gloebit", OpenSim.VersionInfo.VersionNumber)]
[assembly: AddinDependency("OpenSim.Region.Framework", OpenSim.VersionInfo.VersionNumber)]
