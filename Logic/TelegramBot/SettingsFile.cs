using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uchilka.Logic.TelegramBot
{
    internal class SettingsFile
    {
        public PlayerSettings Player { get; set; }
        public TelegramBotSettings TelegramBot { get; set; }
    }

    public class TelegramBotSettings
    {
        public string Token { get; set; }
        public string[] EnabledUsers { get; set; }
    }

    public class PlayerSettings
    {
        public string Path { get; set; }
        public string Arguments { get; set; }
    }
}
