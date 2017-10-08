using System.Net;
using System.Linq;
using System.Net.Http;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using Telegram.Bot.Args;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using System;
using Telegram.Bot.Types;
using System.IO;
using System.Threading.Tasks;
using System.Media;
using System.Windows.Media;
using Newtonsoft.Json;
using System.Diagnostics;
//using Telegram.Bot.Types;

namespace Uchilka.Logic.TelegramBot
{
    internal class TelegramBot
    {
        private XmlNodeList userNodes;
        private readonly List<string> _enabledUsers;
        private readonly IBotCommandHandler _commandHandler;
        private readonly SettingsFile _settings;

        public TelegramBot(IBotCommandHandler commandHandler)
        {
            _commandHandler = commandHandler;

            var jsonFile = System.IO.File.ReadAllText("secrets.json");
            _settings = JsonConvert.DeserializeObject<SettingsFile>(jsonFile);

            var httpHandler = new HttpClientHandler
            {
                UseProxy = false
            };
            var httpClient = new HttpClient(httpHandler);
            var client = new TelegramBotClient(_settings.TelegramBot.Token, httpClient);

            client.OnMessage += Client_OnMessage;
            client.StartReceiving(new[] { UpdateType.All });
        }

        private void Client_OnMessage(object sender, MessageEventArgs e)
        {
            var client = sender as TelegramBotClient;

            if (_settings.TelegramBot.EnabledUsers.Contains(e.Message.From.Username))
            {
                if (e.Message.Type == MessageType.TextMessage)
                {
                    if (e.Message.Entities.Any())
                    {
                        if (e.Message.Entities[0].Type == MessageEntityType.BotCommand)
                        {
                            if (Enum.TryParse(e.Message.EntityValues[0], true, out BotCommandType cmd))
                            {

                            }
                        }
                    }
                }else if (e.Message.Type == MessageType.PhotoMessage)
                {
                    downloadFile(client, e.Message.Photo.LastOrDefault().FileId, "Photo").GetAwaiter().GetResult();
                }else if (e.Message.Type == MessageType.VideoMessage)
                {
                    downloadFile(client, e.Message.Video.FileId, "Video").GetAwaiter().GetResult();
                }
                else if (e.Message.Type == MessageType.VoiceMessage)
                {
                    var res = downloadFile(client, e.Message.Voice.FileId, "Voice").GetAwaiter().GetResult();

                    PlayVoice(res);
                }
                else if (e.Message.Type == MessageType.AudioMessage)
                {
                    downloadFile(client, e.Message.Audio.FileId, "Audio").GetAwaiter().GetResult();
                }
            }
        }

        private void PlayVoice(string res)
        {
            Process.Start(_settings.Player.Path, string.Format(_settings.Player.Arguments, res));
        }

        public bool TestApiAsync()
        {
            var handler = new HttpClientHandler
            {
                UseProxy = false
            };

            var h = new HttpClient(handler);
            var authenticationToken = "";
            var url = $"https://api.telegram.org/bot{authenticationToken}";
            var res = h.GetAsync(url + "/getMe").Result;

            //var botClient = new Telegram.Bot.TelegramBotClient("", h);

            return true;
            //return botClient.GetMeAsync().Result;
        }

        public static async Task<string> downloadFile(TelegramBotClient bot, string fileId, string type)
        {
            try
            {
                var file = await bot.GetFileAsync(fileId);
                var filename = file.FileId + "." + file.FilePath.Split('.').Last();

                var di = new DirectoryInfo($"./Downloads/{type}");

                if (!di.Exists)
                {
                    Directory.CreateDirectory(di.FullName);
                }

                var fullPath = Path.Combine(di.FullName, filename);

                using (var saveImageStream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.FileStream.CopyToAsync(saveImageStream);
                }

                return fullPath;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
