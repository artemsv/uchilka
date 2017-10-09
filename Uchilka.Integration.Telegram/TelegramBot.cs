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
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Uchilka.Integration.Abstractions;

namespace Uchilka.Integration.TelegramBot
{
    internal class TelegramBot : ICommChannel
    {
        private readonly ICommChannelCommandHandler _commandHandler;
        private readonly SettingsFile _settings;
        private readonly TelegramBotClient _client;

        public TelegramBot(ICommChannelCommandHandler commandHandler)
        {
            _commandHandler = commandHandler;

            var jsonFile = System.IO.File.ReadAllText("secrets.json");
            _settings = JsonConvert.DeserializeObject<SettingsFile>(jsonFile);

            var httpHandler = new HttpClientHandler
            {
                UseProxy = false
            };
            var httpClient = new HttpClient(httpHandler);
            _client = new TelegramBotClient(_settings.TelegramBot.Token, httpClient);

            _client.OnMessage += Client_OnMessage;
            _client.StartReceiving(new[] { UpdateType.All });
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
                            if (Enum.TryParse(e.Message.EntityValues[0], true, out CommChannelCommandType cmd))
                            {
                                _commandHandler.HandleCommand(cmd);
                            }
                        }
                    }
                }else if (e.Message.Type == MessageType.PhotoMessage)
                {
                    var path = downloadFile(client, e.Message.Photo.LastOrDefault().FileId, "Photo").GetAwaiter().GetResult();

                    _commandHandler.HandlePhoto(path);

                }else if (e.Message.Type == MessageType.VideoMessage)
                {
                    downloadFile(client, e.Message.Video.FileId, "Video").GetAwaiter().GetResult();
                }
                else if (e.Message.Type == MessageType.VoiceMessage)
                {
                    var res = downloadFile(client, e.Message.Voice.FileId, "Voice").GetAwaiter().GetResult();

                    _commandHandler.HandleVoice(res);
                }
                else if (e.Message.Type == MessageType.AudioMessage)
                {
                    downloadFile(client, e.Message.Audio.FileId, "Audio").GetAwaiter().GetResult();
                }
            }
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
            catch (Exception)
            {
                throw;
            }
        }

        public void SendTextMessage(string message)
        {
            _settings.TelegramBot.EnabledChatIds.ToList().ForEach(x =>
                _client.SendTextMessageAsync(x, message));
        }
    }
}
