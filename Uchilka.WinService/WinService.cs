using System.Management;
using System;
using System.Diagnostics;
using Uchilka.Integration.Abstractions;
using Uchilka.Integration.TelegramBot;
using Newtonsoft.Json;
using Topshelf;
using Topshelf.Hosts;
using Uchilka.WinService.Win32;
using System.IO;

namespace Uchilka.WinService
{
    internal class WinService : ServiceControl, ICommChannelCommandHandler
    {
        private TelegramBot _commChannel;
        private DateTime _startTime;

        public bool IsRunningAsConsole(HostControl control)
        {
            return control is ConsoleRunHost;
        }

        public void HandleCommand(CommChannelCommandType cmd, DateTime time)
        {
            if (time > _startTime)
            {
                switch (cmd)
                {
                    case CommChannelCommandType.Shutdown:
                        Shutdown();
                        break;
                    case CommChannelCommandType.GetScreen:
                        SendScreenShot();
                        break;
                    default:
                        break;
                }
            }else
            {
                Debug.WriteLine($"Command {cmd} is out of date {time}");
            }
        }

        private void SendScreenShot()
        {
            var targetDir = Path.Combine(Directory.GetCurrentDirectory(), "ScreenShots");
            var fileName = $"{DateTime.Now.ToString("yyyy-MM-dd_hh_mm_ss")}.png";

            if (TakeScreenShot(targetDir, fileName) == 0)
            {
                using (var stream = new FileStream(Path.Combine(targetDir, fileName), FileMode.Open))
                {
                    _commChannel.SendTextMessage($"Sending photo: {fileName}");
                    _commChannel.SendPhotoMessage(stream, fileName);
                }
            }
            else
            {
                _commChannel.SendTextMessage("ERROR: take screen capture failed");
            }
        }

        private uint TakeScreenShot(string targetDir, string fileName)
        {
            var commandLine = $"Uchilka.ScreenCapturer.exe \"{targetDir}\" {fileName}";

            var res = ProcessAsCurrentUser.CreateProcessAsCurrentUser(null, commandLine);

            Debug.WriteLine($"Screen Capturer launch result: {res}");

            return res;
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
            var jsonFile = System.IO.File.ReadAllText("secrets.json");
            var settings = JsonConvert.DeserializeObject<SettingsFile>(jsonFile);

            _commChannel = new TelegramBot(this, settings.TelegramBot);
            _startTime = DateTime.Now;
            _commChannel.Start();

            _commChannel.SendTextMessage($"Uchilka Service started ({Environment.MachineName})");
        }

        public bool Stop(HostControl hostControl)
        {
            _commChannel.SendTextMessage("Uchilka Service stopped");
            //HarpLogger.Log("Agent Windows Service: stopping...");

            //_host.Stop();

            //HarpLogger.Log("Agent Windows Service: stopped");

            return true;
        }

        private void Shutdown()
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "Shutdown",
                Arguments = "-s -t 10",
                CreateNoWindow = true
            });
        }

        private void ShutdownWMI()
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
