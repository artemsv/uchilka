namespace Uchilka.Integration.Abstractions
{
    public class TelegramBotSettings
    {
        public string Token { get; set; }
        public string[] EnabledUsers { get; set; }
        public long[] EnabledChatIds { get; set; }
    }

    public class SettingsFile
    {
        //public PlayerSettings Player { get; set; }
        public TelegramBotSettings TelegramBot { get; set; }
    }

    public class PlayerSettings
    {
        public string Path { get; set; }
        public string Arguments { get; set; }
    }
}
