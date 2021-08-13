﻿using System;

namespace Dotnet.Script.Core
{
    public class ExitCommand : IInteractiveCommand
    {
        public string Name => "exit";

        public void Execute(CommandContext commandContext)
        {
            commandContext.Runner.Exit();
        }
    }
}
