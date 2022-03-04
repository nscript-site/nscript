﻿using Dotnet.Script.DependencyModel.Environment;
using Dotnet.Script.DependencyModel.Logging;
using Dotnet.Script.DependencyModel.Process;
using Dotnet.Script.DependencyModel.ProjectSystem;
using Microsoft.CodeAnalysis;
using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Dotnet.Script.Core.Templates;

namespace Dotnet.Script.Core
{
    /// <summary>
    /// 脚本打包器。与 publish 不同，pack 指令会通过 ScriptPacker 将脚本转换为正常的 dotnet 程序，通过正常的流程进行打包。
    /// 这里需要注意的是，可以 pack 的脚本有一定的要求，在打包的脚本里 入口的代码需要在 “//@main 注释” 之下
    /// </summary>
    public class ScriptPacker
    {
        private const string ScriptingVersion = "4.0.0";

        private string PacketClassName;

        public bool EnableAot = false;

        public ScriptPacker(ScriptProjectProvider scriptProjectProvider, ScriptConsole scriptConsole)
        {
            PacketClassName = "PacketClass" + DateTime.Now.ToFileTimeUtc().ToString();
        }

        public ScriptPacker(LogFactory logFactory) : this(new ScriptProjectProvider(logFactory), ScriptConsole.Default)
        {
        }

        public String GenerateProjectFile(String assemblyName)
        {
            String proj = null;
            proj = TemplateLoader.ReadTemplate(EnableAot? "pack.aot.csproj.template":"pack.csproj.template");
            proj = proj.Replace("PACKAGE_REFERENCE", GetNugetReference());
            proj = proj.Replace("DLL_REFERENCE", GetDllReference());
            proj = proj.Replace("PATH_BASE_OUTPUT", Path.Combine(Path.GetTempPath(), PacketClassName, "bin"));
            proj = proj.Replace("PATH_OBJ_OUTPUT", Path.Combine(Path.GetTempPath(), PacketClassName, "obj"));
            proj = proj.Replace("ASSEMBLY_NAME", assemblyName);
            return proj;
        }

        public String GenerateNugetConfigFileContent()
        {
            return TemplateLoader.ReadTemplate("aot.nuget.template");
        }

        private String GetNugetReference()
        {
            // 将 nuget 转换成包引用，例如： <PackageReference Include="Newtonsoft.Json" Version="13.0.1" />
            return String.Empty;
        }

        private String GetDllReference()
        {
            // 将 dll 引用转换成下面格式：
            //<Reference Include="Geb.Media.IO">
            //  <HintPath>..\..\..\Projects\21_Private_NScript\app\lib\Geb.Media.IO.dll</HintPath>
            //</Reference>
            return String.Empty;
        }

        protected virtual IEnumerable<string> ImportedNamespaces => new[]
        {
            "System",
            "System.IO",
            "System.Collections.Generic",
            "System.Diagnostics",
            "System.Dynamic",
            "System.Linq",
            "System.Linq.Expressions",
            "System.Text",
            "System.Threading.Tasks"
        };

        public String GenerateShell(String rid)
        {
            if(EnableAot == false)
                return "dotnet publish -c release /p:PublishSingleFile=true /p:PublishTrimmed=true /p:EnableCompressionInSingleFile=true -r @@RID --self-contained -o ./out".Replace("@@RID", rid);
            else
                return "dotnet publish -c release -r @@RID -o ./out".Replace("@@RID", rid);
        }

        public String GenerateCode(String csxFilePath)
        {
            String[] lines = File.ReadAllLines(csxFilePath);
            List<String> sbUsing = new ();
            List<String> sbBody = new ();
            List<String> sbHeader = new ();
            List<String> sbMain = new ();

            bool bodyFlag = false;  // 是否是代码的 body 部分, !,#load,#r,"using"等视为 body 部分
            bool mainFlag = false;  // 是否进入 main 部分

            foreach(String line in lines)
            {
                String txt = line.Trim();

                if(txt.StartsWith("!"))
                {
                    if(bodyFlag == false)
                        continue;
                }

                if (txt.StartsWith("#load") || txt.StartsWith("#r"))
                {
                    if (bodyFlag == false)
                    {
                        sbHeader.Add(line);
                        continue;
                    }
                }

                if(txt.StartsWith("using") && txt.EndsWith(";"))
                {
                    if(bodyFlag == false)
                    {
                        sbUsing.Add(line);
                        continue;
                    }
                }

                if (txt.StartsWith("//@main"))
                {
                    bodyFlag = true;
                    mainFlag = true;
                    sbMain.Add(line);
                    continue;
                }

                if (txt.StartsWith("//") || String.IsNullOrEmpty(txt))
                {
                    if (bodyFlag == false) sbHeader.Add(line);
                    else if (mainFlag == false) sbBody.Add(line);
                    else sbMain.Add(line);
                    continue;
                }

                if (bodyFlag == false) bodyFlag = true;

                if (mainFlag == true)
                    sbMain.Add(line);
                else
                    sbBody.Add(line);
            }

            StringBuilder sbOut = new StringBuilder();
            foreach(var line in sbHeader)
            {
                String txt = line.TrimStart();
                if (txt.StartsWith("#"))
                    sbOut.AppendLine("//" + line);
                else if (txt.StartsWith("!"))
                    sbOut.AppendLine("//" + line);
                else
                    sbOut.AppendLine(line);
            }

            var dftUsing = ImportedNamespaces;
            foreach(var ns in dftUsing)
            {
                sbOut.AppendLine("using " + ns + ";");
            }

            foreach (var line in sbUsing)
            {
                sbOut.AppendLine(line);
            }

            sbOut.AppendLine("public partial class " + PacketClassName + " {");
            foreach (var line in sbBody)
            {
                sbOut.AppendLine(line);
            }

            if(sbMain.Count > 0)
            {
                sbOut.AppendLine("public static int Main(string[] Args)");
                sbOut.AppendLine("{");
                foreach (var line in sbMain)
                {
                    sbOut.AppendLine(line);
                }
                sbOut.AppendLine("return 0;");
                sbOut.AppendLine("}");
            }

            sbOut.AppendLine("}");

            return sbOut.ToString();
        }

        private void ClearGenerateFiles(DirectoryInfo dirOut)
        {
            List<FileInfo> list = new List<FileInfo>();
            list.AddRange(dirOut.GetFiles("*_nsg.csproj"));
            list.AddRange(dirOut.GetFiles("*_nsg.cs"));
            list.AddRange(dirOut.GetFiles("*_nsg.bat"));
            list.AddRange(dirOut.GetFiles("nuget.config"));
            foreach (var item in list)
                item.Delete();
        }

        public void GenerateProject(String csxFilePath, String dirOut)
        {
            DirectoryInfo dirOutInfo = new DirectoryInfo(dirOut);
            if (dirOutInfo.Exists == false) dirOutInfo.Create();

            ClearGenerateFiles(dirOutInfo);

            FileInfo file = new FileInfo(csxFilePath);
            String outFileName = GetSourceFileName(file.FullName);
            String content = GenerateCode(csxFilePath);
            String outFilePath = Path.Combine(dirOutInfo.FullName, outFileName);
            File.WriteAllText(outFilePath, content);
            Console.WriteLine($"Generate code of {csxFilePath} to file: {outFileName} ");

            if (EnableAot == true)
            {
                String nugetConfig = GenerateNugetConfigFileContent();
                String nugetConfigFilePath = Path.Combine(dirOutInfo.FullName, "nuget.config");
                File.WriteAllText(nugetConfigFilePath, nugetConfig) ;
                Console.WriteLine($"Generate file: {nugetConfigFilePath} ");
            }

            var proj = GenerateProjectFile(file.Name.Substring(0,file.Name.Length - file.Extension.Length));
            String outProjFilePath = Path.Combine(dirOutInfo.FullName, "project_nsg.csproj");
            File.WriteAllText(outProjFilePath, proj);
            Console.WriteLine($"Generate project file: {outProjFilePath}");

            GenerateShells(dirOutInfo);
        }

        private void GenerateShells(DirectoryInfo dirOutInfo)
        {
            String[] rids = new string[] { "win-x64","linux-x64" };
            foreach(var rid in rids)
            {
                var str = GenerateShell(rid);
                var path = Path.Combine(dirOutInfo.FullName, "publish_" + rid.Replace("-","_").ToString()  + "_nsg.bat");
                File.WriteAllText(path, str);
                Console.WriteLine($"Generate file: {path}");
            }
        }

        List<Tuple<int, String>> SourceFileNames = new List<Tuple<int, string>>();

        private bool ContainsSourceFileName(String val)
        {
            foreach(var item in SourceFileNames)
            {
                if (item.Item2 == val) return true;
            }
            return false;
        }

        private String GetSourceFileName(String fileFullName)
        {
            int val = fileFullName.GetHashCode();
            foreach(var item in SourceFileNames)
            {
                if (item.Item1 == val)
                    return item.Item2;
            }

            FileInfo fileInfo = new FileInfo(fileFullName);
            String fileName = fileInfo.Name;
            String prefix = fileName.Substring(0, fileName.Length - 4);
            String newFileName = null;

            int num = 0;
            while(newFileName == null)
            {
                String nextFileName = prefix + ((num == 0)? "":num.ToString()) + "_nsg.cs";
                if (ContainsSourceFileName(nextFileName)) num++;
                else
                    newFileName = nextFileName;
            }

            SourceFileNames.Add(new Tuple<int,String>(val, newFileName));
            return newFileName;
        }
    }
}