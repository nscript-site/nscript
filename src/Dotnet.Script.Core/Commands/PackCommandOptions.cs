using Dotnet.Script.DependencyModel.Environment;
using Microsoft.CodeAnalysis;
#if NETCOREAPP
using System.Runtime.Loader;
#endif

namespace Dotnet.Script.Core.Commands
{
    public class PackCommandOptions
    {
        public PackCommandOptions(ScriptFile file, string outputDirectory, string libraryName, PublishType publishType, OptimizationLevel optimizationLevel, string[] packageSources, string runtimeIdentifier, bool noCache, bool enableAot)
        {
            File = file;
            OutputDirectory = outputDirectory;
            LibraryName = libraryName;
            PublishType = publishType;
            OptimizationLevel = optimizationLevel;
            PackageSources = packageSources;
            RuntimeIdentifier = runtimeIdentifier ?? ScriptEnvironment.Default.RuntimeIdentifier;
            NoCache = noCache;
            EnableAot = enableAot;
        }

        public bool EnableAot { get; }
        public ScriptFile File { get; }
        public string OutputDirectory { get; }
        public string LibraryName { get; }
        public PublishType PublishType { get; }
        public OptimizationLevel OptimizationLevel { get; }
        public string[] PackageSources { get; }
        public string RuntimeIdentifier { get; }
        public bool NoCache { get; }

#if NETCOREAPP
#nullable enable
        /// <summary>
        /// Gets or sets a custom assembly load context to use for script isolation.
        /// </summary>
        public AssemblyLoadContext? AssemblyLoadContext { get; init; }
#nullable restore
#endif
    }
}