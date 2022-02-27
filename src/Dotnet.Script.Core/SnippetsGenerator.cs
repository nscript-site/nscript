using Dotnet.Script.Core.Templates;
using Dotnet.Script.DependencyModel.Environment;
using Dotnet.Script.DependencyModel.Logging;
using Dotnet.Script.DependencyModel.Process;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Dotnet.Script.Core
{
    public class SnippetsGenerator
    {
        private readonly ScriptEnvironment _scriptEnvironment;
        private const string DefaultAspnetScriptFileName = "httpserver.csx";
        private const string DefaultEmbeioScriptFileName = "eserver.csx";
        private const string SnippetsDirName = "snippets";
        private readonly ScriptConsole _scriptConsole;
        private readonly CommandRunner _commandRunner;

        public SnippetsGenerator(LogFactory logFactory) : this(logFactory, ScriptConsole.Default, ScriptEnvironment.Default)
        {
        }

        public SnippetsGenerator(LogFactory logFactory, ScriptConsole scriptConsole, ScriptEnvironment scriptEnvironment)
        {
            _commandRunner = new CommandRunner(logFactory);
            _scriptConsole = scriptConsole;
            _scriptEnvironment = scriptEnvironment;
        }

        public void Generate(string currentWorkingDirectory)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Path.Combine(currentWorkingDirectory, SnippetsDirName));
            if (dirInfo.Exists == false) dirInfo.Create();

            CreateNewScriptFileFromTemplate("httpserver.csx", dirInfo.FullName, "aspnet.csx.template");
            CreateNewScriptFileFromTemplate("eserver.csx", dirInfo.FullName, "eserver.csx.template");

            CreateImportScriptFile(dirInfo.FullName, "aspnet.csx", () => CreateImportFile("Microsoft.AspNetCore.App"));
            CreateImportScriptFile(dirInfo.FullName, "winui.csx", () => CreateImportFile("Microsoft.WindowsDesktop.App"));
        }

        public void CreateNewScriptFileFromTemplate(string fileName, string currentDirectory, string templateName)
        {
            _scriptConsole.WriteNormal($"Creating '{fileName}'");
            if (!Path.HasExtension(fileName))
            {
                fileName = Path.ChangeExtension(fileName, ".csx");
            }

            var pathToScriptFile = Path.Combine(currentDirectory, fileName);
            if (!File.Exists(pathToScriptFile))
            {
                var scriptFileTemplate = TemplateLoader.ReadTemplate(templateName);

                if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ||
                    RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    // add a shebang to set dotnet-script as the interpreter for .csx files
                    // and make sure we are using environment newlines, because shebang won't work with windows cr\lf
                    scriptFileTemplate = $"#!/usr/bin/env nscript" + Environment.NewLine + scriptFileTemplate.Replace("\r\n", Environment.NewLine);
                }

                File.WriteAllText(pathToScriptFile, scriptFileTemplate, new UTF8Encoding(false /* Linux shebang can't handle BOM */));

                if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ||
                    RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    // mark .csx file as executable, this activates the shebang to run dotnet-script as interpreter
                    _commandRunner.Execute($"/bin/chmod", $"+x \"{pathToScriptFile}\"");
                }
                _scriptConsole.WriteSuccess($"...'{pathToScriptFile}' [Created]");
            }
            else
            {
                _scriptConsole.WriteHighlighted($"...'{pathToScriptFile}' already exists [Skipping]");
            }
        }

        private void CreateImportScriptFile(string currentWorkingDirectory, string filename, Func<String> generator)
        {
            _scriptConsole.Out.WriteLine($"Creating import script file '{filename}'");
            if (Directory.GetFiles(currentWorkingDirectory, filename).Any())
            {
                _scriptConsole.WriteHighlighted($"...Folder already contains {filename} [Skipping]");
            }
            else
            {
                String pathToScriptFile = Path.Combine(currentWorkingDirectory, filename);
                String content = generator();
                File.WriteAllText(pathToScriptFile, content);
                _scriptConsole.WriteSuccess($"...'{pathToScriptFile}' [Created]");
            }
        }

        private static bool IsAssembly(string file)
        {
            // https://docs.microsoft.com/en-us/dotnet/standard/assembly/identify
            try
            {
                System.Reflection.AssemblyName.GetAssemblyName(file);
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        private String CreateImportFile(String sdkName)
        {
            StringBuilder sb = new StringBuilder();
            var netcoreAppRuntimeAssemblyLocation = Path.GetDirectoryName(typeof(object).Assembly.Location);
            netcoreAppRuntimeAssemblyLocation = netcoreAppRuntimeAssemblyLocation.Replace("Microsoft.NETCore.App", sdkName);
            var netcoreAppRuntimeAssemblies = Directory.GetFiles(netcoreAppRuntimeAssemblyLocation, "*.dll").Where(IsAssembly).ToArray();
            foreach (String item in netcoreAppRuntimeAssemblies)
            {
                String fullPath = item.Replace("\\", "/");
                sb.AppendLine($"#r \"{fullPath}\"");
            }
            return sb.ToString();
        }
    }
}
