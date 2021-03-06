﻿using System;
using System.Diagnostics;
using Topshelf;
using Topshelf.Hosts;

namespace Uchilka.WinService
{
    internal class WinService : ServiceControl
    {
        private BotManager _botManager;

        public bool IsRunningAsConsole(HostControl control)
        {
            return control is ConsoleRunHost;
        }

        public bool Start(HostControl hostControl)
        {
            _botManager = new BotManager();
            _botManager.Start();

            if (IsRunningAsConsole(hostControl))
            {
                var pid = Process.GetCurrentProcess().Id;
                Console.Title = $"PID - {pid} (0x{pid.ToString("X")})";
            }

            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            _botManager.Stop();

            //HarpLogger.Log("Agent Windows Service: stopping...");

            //_host.Stop();

            //HarpLogger.Log("Agent Windows Service: stopped");

            return true;
        }
    }
}
