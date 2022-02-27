using Dotnet.Script.DependencyModel.Logging;

namespace Dotnet.Script.Core.Commands
{
    public class SinppetsCommand
    {
        private readonly LogFactory _logFactory;

        public SinppetsCommand(LogFactory logFactory)
        {
            _logFactory = logFactory;
        }

        public void Execute(InitCommandOptions options)
        {
            var g = new SnippetsGenerator(_logFactory);
            g.Generate(options.WorkingDirectory);
        }
    }
}
