using System.Management;
using System;
using System.Diagnostics;
using Topshelf;
using Topshelf.Hosts;
using Uchilka.Integration.Abstractions;
using Uchilka.Integration.TelegramBot;

namespace Uchilka.WinService
{
    internal class WinService : ServiceControl, ICommChannelCommandHandler
    {
        private TelegramBot _commChannel;

        public bool IsRunningAsConsole(HostControl control)
        {
            return control is ConsoleRunHost;
        }

        public void HandleCommand(CommChannelCommandType cmd)
        {
            switch (cmd)
            {
                case CommChannelCommandType.Shutdown:

                    break;
                default:
                    break;
            }
        }

        public bool Start(HostControl hostControl)
        {
            StartCommChannel();

            if (IsRunningAsConsole(hostControl))
            {
                var pid = Process.GetCurrentProcess().Id;
                Console.Title = $"PID - {pid} (0x{pid.ToString("X")})";
            }

            return true;
        }

        private void StartCommChannel()
        {
            _commChannel = new TelegramBot(this);

            _commChannel.SendTextMessage("Uchilka Service started successfully");
        }

        public bool Stop(HostControl hostControl)
        {
            //HarpLogger.Log("Agent Windows Service: stopping...");

            //_host.Stop();

            //HarpLogger.Log("Agent Windows Service: stopped");

            return true;
        }

        private void Shutdown()
        {
            ManagementBaseObject mboShutdown = null;
            ManagementClass mcWin32 = new ManagementClass("Win32_OperatingSystem");
            mcWin32.Get();

            // You can't shutdown without security privileges
            mcWin32.Scope.Options.EnablePrivileges = true;
            ManagementBaseObject mboShutdownParams =
                     mcWin32.GetMethodParameters("Win32Shutdown");

            // Flag 1 means we want to shut down the system. Use "2" to reboot.
            mboShutdownParams["Flags"] = "1";
            mboShutdownParams["Reserved"] = "0";
            foreach (ManagementObject manObj in mcWin32.GetInstances())
            {
                mboShutdown = manObj.InvokeMethod("Win32Shutdown",
                                               mboShutdownParams, null);
            }
        }
    }
}
