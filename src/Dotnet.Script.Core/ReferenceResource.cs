using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

namespace Dotnet.Script.Core
{
    public class NugetRef
    {
        public String Name { get; set; }
        public String Version { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            NugetRef other = obj as NugetRef;
            if (other == null) return false;
            return other.Name == this.Name && other.Version == this.Version;
        }
    }

    /// <summary>
    /// 脚本的引用资源
    /// </summary>
    public class ReferenceResource
    {
        public enum RefMode
        {
            None,
            Dll,
            Package,
            Code
        }

        public RefMode Mode { get; private set; } = RefMode.None;
        public String Value { get; private set; } = String.Empty;
        public String Version { get; private set; } = String.Empty;

        private String _content;
        private String _baseDir;
        public ReferenceResource(String content, String baseDir)
        {
            this._baseDir = baseDir;
            _content = content.Trim();
            Build();
        }

        private const String REF = "#r";
        private const String LOAD = "#load";
        private const String NUGET = "nuget:";

        private void Build()
        {
            if (_content.StartsWith(REF))
            {
                String str = RemoveQuotes(_content.Substring(REF.Length).Trim());
                if (str.StartsWith(NUGET))
                {
                    Mode = RefMode.Package;
                    str = str.Substring(NUGET.Length);
                    int idx = str.IndexOf(',');
                    if (idx <= 0)
                    {
                        Value = str;
                    }
                    else
                    {
                        Value = str.Substring(0, idx).Trim();
                        Version = str.Substring(idx + 1).Trim();
                    }
                }
                else
                {
                    Mode = RefMode.Dll;
                    Value = str;
                }
            }
            else if (_content.StartsWith(LOAD))
            {
                Mode = RefMode.Code;
                Value = RemoveQuotes(_content.Substring(LOAD.Length).Trim());
            }

            if(Mode != RefMode.Package && String.IsNullOrEmpty(Value) == false)
            {
                Value = new FileInfo(System.IO.Path.Combine(_baseDir, Value)).FullName;
            }
        }

        private String RemoveQuotes(String text)
        {
            if (String.IsNullOrEmpty(text) || text.Length < 2) return text;
            text = text.Substring(1);
            int idx = text.IndexOf("\"");
            if (idx >= 0) text = text.Substring(0,idx);
            return text;
        }

        public override string ToString()
        {
            return $"{Mode}-{Value}-{Version}";
        }
    }
}
