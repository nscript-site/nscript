﻿using System.IO;
using System.Text;
using Dotnet.Script.DependencyModel.Environment;
using Dotnet.Script.DependencyModel.ProjectSystem;
using Dotnet.Script.Shared.Tests;
using Xunit;
using Xunit.Abstractions;

namespace Dotnet.Script.Tests
{
    [Collection("IntegrationTests")]
    public class ScriptProjectProviderTests
    {
        private readonly ScriptEnvironment _scriptEnvironment;

        public ScriptProjectProviderTests(ITestOutputHelper testOutputHelper)
        {
            testOutputHelper.Capture();
            _scriptEnvironment = ScriptEnvironment.Default;
        }

        [Fact]
        public void ShouldLogProjectFileContent()
        {
            StringBuilder log = new StringBuilder();
            var provider = new ScriptProjectProvider(type => ((level, message, exception) => log.AppendLine(message)));

            provider.CreateProject(TestPathUtils.GetPathToTestFixtureFolder("HelloWorld"), _scriptEnvironment.TargetFramework, true);
            var output = log.ToString();

            Assert.Contains("<Project Sdk=\"Microsoft.NET.Sdk\">", output);
        }
    }
}