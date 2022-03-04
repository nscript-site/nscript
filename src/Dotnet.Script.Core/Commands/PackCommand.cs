using System.IO;
using System.Linq;
using Dotnet.Script.DependencyModel.Logging;
using Dotnet.Script.DependencyModel.Runtime;
using Microsoft.CodeAnalysis.Scripting.Hosting;

namespace Dotnet.Script.Core.Commands
{
    public class PackCommand
    {
        private readonly ScriptConsole _scriptConsole;
        private readonly LogFactory _logFactory;

        public PackCommand(ScriptConsole scriptConsole, LogFactory logFactory)
        {
            _scriptConsole = scriptConsole;
            _logFactory = logFactory;
        }

        public void Execute(PackCommandOptions options)
        {
            var absoluteFilePath = options.File.Path;
            var publishDirectory = options.OutputDirectory ??
                (options.PublishType == PublishType.Library ? Path.Combine(Path.GetDirectoryName(absoluteFilePath), "pack") : Path.Combine(Path.GetDirectoryName(absoluteFilePath), "pack"));
            var absolutePublishDirectory = publishDirectory.GetRootedPath();
            var packer = new ScriptPacker(_logFactory);
            packer.EnableAot = options.EnableAot;
            packer.GenerateProject(absoluteFilePath, publishDirectory);
        }
    }
}