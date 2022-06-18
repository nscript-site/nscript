using System;
using System.IO;
using System.Linq;
using Dotnet.Script.DependencyModel.Logging;
using Dotnet.Script.DependencyModel.Runtime;
using Microsoft.CodeAnalysis.Scripting.Hosting;
using System.Xml.XPath;

namespace Dotnet.Script.Core.Commands
{
    /// <summary>
    /// 转换命令。将 csproj 文件里的引用消息转换成 nscript 文件里的引用信息
    /// </summary>
    public class ConvertCommand
    {
        private readonly ScriptConsole _scriptConsole;
        private readonly LogFactory _logFactory;

        public ConvertCommand(ScriptConsole scriptConsole, LogFactory logFactory)
        {
            _scriptConsole = scriptConsole;
            _logFactory = logFactory;
        }

        public void Execute(ConvertCommandOptions options)
        {
            if (File.Exists(options.File.Path) == false)
            {
                throw new Exception($"Couldn't find file '{options.File.Path}'");
            }

            try
            {
                XPathDocument doc = new XPathDocument(options.File.Path);
                var nav = doc.CreateNavigator();
                var it = nav.Select("//PackageReference");
                while (it.MoveNext())
                {
                    var cur = it.Current;
                    String refName = cur.GetAttribute("Include", "");
                    String refVersion = cur.GetAttribute("Version", "");
                    if (String.IsNullOrEmpty(refVersion) == false) refVersion = ", " + refVersion;
                    Console.WriteLine($"#r \"nuget: {refName}{refVersion}\"");
                }
                Console.WriteLine();
            }
            catch(Exception ex)
            {
                throw new Exception($"File is not a valid project file - '{options.File.Path}'");
            }
        }
    }
}
