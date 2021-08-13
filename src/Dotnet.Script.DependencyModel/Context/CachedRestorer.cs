using System.IO;
using Dotnet.Script.DependencyModel.Environment;
using Dotnet.Script.DependencyModel.Logging;
using Dotnet.Script.DependencyModel.ProjectSystem;

namespace Dotnet.Script.DependencyModel.Context
{
    /// <summary>
    /// An <see cref="IRestorer"/> decorator that ensures that we only
    /// call out to "dotnet restore" if the underlying project file has changed.
    /// </summary>
    public class CachedRestorer : IRestorer
    {
        private readonly IRestorer _restorer;
        private readonly Logger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CachedRestorer"/> class.
        /// </summary>
        /// <param name="restorer">The <see cref="IRestorer"/> to be used when we miss the cache.</param>
        /// <param name="logFactory">The <see cref="LogFactory"/> to be used for logging.</param>
        public CachedRestorer(IRestorer restorer, LogFactory logFactory)
        {
            _restorer = restorer;
            _logger = logFactory.CreateLogger<CachedRestorer>();
        }

        /// <inheritdoc/>
        public bool CanRestore => _restorer.CanRestore;

        /// <inheritdoc/>
        public void Restore(ProjectFileInfo projectFileInfo, string[] packageSources)
        {
            var projectFile = new ProjectFile(File.ReadAllText(projectFileInfo.Path));

            var pathToCachedProjectFile = $"{projectFileInfo.Path}.cache";
            if (File.Exists(pathToCachedProjectFile))
            {

                var cachedProjectFile = new ProjectFile(File.ReadAllText(pathToCachedProjectFile));
                if (projectFile.Equals(cachedProjectFile))
                {
                    _logger.Debug($"Skipping restore. {projectFileInfo.Path} and {pathToCachedProjectFile} are identical.");
                    return;
                }
                else
                {
                    _logger.Debug($"Cache miss. Deleting stale cache file {pathToCachedProjectFile}");
                    File.Delete(pathToCachedProjectFile);
                    RestoreAndCacheProjectFile();
                }
            }
            else
            {
                RestoreAndCacheProjectFile();
            }

            void RestoreAndCacheProjectFile()
            {
                _restorer.Restore(projectFileInfo, packageSources);
                if (projectFile.IsCacheable)
                {
                    _logger.Debug($"Caching project file : {pathToCachedProjectFile}");
                    projectFile.Save(pathToCachedProjectFile);
                }
                else
                {
                    _logger.Warning($"Unable to cache {projectFileInfo.Path}. For caching and optimal performance, ensure that the script(s) references Nuget packages with a pinned version.");
                }
            }
        }
    }
}