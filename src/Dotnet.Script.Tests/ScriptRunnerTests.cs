﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Dotnet.Script.Core;
using Dotnet.Script.DependencyModel.Runtime;
using Dotnet.Script.Shared.Tests;
using Gapotchenko.FX.Reflection;
using Moq;
using Xunit;

namespace Dotnet.Script.Tests
{
    public class ScriptRunnerTests
    {
        [Fact]
        public void ResolveAssembly_ReturnsNull_WhenRuntimeDepsMapDoesNotContainAssembly()
        {
            var scriptRunner = CreateScriptRunner();

            var result = scriptRunner.ResolveAssembly(AssemblyLoadPal.ForCurrentAppDomain, new AssemblyName("AnyAssemblyName"), new Dictionary<string, RuntimeAssembly>());

            Assert.Null(result);
        }

        private ScriptRunner CreateScriptRunner()
        {
            var logFactory = TestOutputHelper.CreateTestLogFactory();
            var scriptCompiler = new ScriptCompiler(logFactory, false);

            return new ScriptRunner(scriptCompiler, logFactory, ScriptConsole.Default);
        }
    }
}