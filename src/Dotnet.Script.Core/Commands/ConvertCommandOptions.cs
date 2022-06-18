using System.IO;
using System.Linq;
using Dotnet.Script.DependencyModel.Logging;
using Dotnet.Script.DependencyModel.Runtime;
using Microsoft.CodeAnalysis.Scripting.Hosting;

namespace Dotnet.Script.Core.Commands
{
    public class ConvertCommandOptions
    {
        public ConvertCommandOptions(ScriptFile file)
        {
            File = file;
        }

        public ScriptFile File { get; }
    }
}
