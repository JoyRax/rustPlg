using System;
using System.Collections;
using System.Collections.Generic;
using Oxide.Core.Plugins;
using Oxide.Core.Libraries.Covalence;

namespace Oxide.Plugins
{
    [Info("Epic Stuff", "Unknown Author", "0.1.0")]
    [Description("Makes epic stuff happen")]
    class EpicStuff : CovalencePlugin
    {
        private void Init()
        {
            Puts("A baby plugin is born!");
        }

        [Command("test")]
        private void TestCommand(IPlayer player, string command, string[] args)
        {
            player.Reply("Test successful!");
        }
    }
}