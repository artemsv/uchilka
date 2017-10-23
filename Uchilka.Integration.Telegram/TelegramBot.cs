using System.Linq;
using System.Net.Http;
using Telegram.Bot.Args;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using System;
using System.IO;
using System.Threading.Tasks;
using Uchilka.Integration.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;

namespace Uchilka.Integration.TelegramBot
{
    public class TelegramBot : ICommChannel
    {
        private readonly ICommChannelCommandHandler _commandHandler;
        private readonly ICommChannelMultimediaHandler _mmHandler;
        private readonly TelegramBotSettings _settings;
        private readonly TelegramBotClient _client;

        public TelegramBot(ICommChannelHandler commHandler, TelegramBotSettings settings)
        {
            _commandHandler = commHandler as ICommChannelCommandHandler;
            _settings = settings;

            _mmHandler = commHandler as ICommChannelMultimediaHandler;

            if (_commandHandler is null && _mmHandler is null) throw new ArgumentException();

            var httpHandler = new HttpClientHandler
            {
                UseProxy = false
            };
            var httpClient = new HttpClient(httpHandler);
            _client = new TelegramBotClient(_settings.Token, httpClient);

            _client.OnMessage += Client_OnMessage;
            _client.OnUpdate += Client_OnUpdate;
            _client.OnReceiveError += Client_OnReceiveError;
            _client.OnReceiveGeneralError += Client_OnReceiveGeneralError;
        }

        #region Public

        public void Start()
        {
            _client.StartReceiving(new[] { UpdateType.All });
        }

        public void SendPhotoMessage(Stream stream, string fileName)
        {
            var bytes = new byte[stream.Length];

            stream.Read(bytes, 0, (int)stream.Length);

            CallForEnabled(async chatId =>
            {
                await _client.SendChatActionAsync(chatId, ChatAction.UploadPhoto);
                Message res = null;
                try
                {
                    res = await _client.SendPhotoAsync(chatId, new FileToSend
                    {
                        Content = new MemoryStream(bytes, 0, bytes.Length),
                        Filename = fileName,
                        //FileId = Guid.NewGuid().ToString("n"),
                    }, fileName);
                }
                catch(Exception ex)
                {
                    res = null;
                }
                await _client.SendChatActionAsync(chatId, ChatAction.Typing);

                return res;
            });
        }

        public void SendTextMessage(string message)
        {
            CallForEnabled(async chatId => await _client.SendTextMessageAsync(chatId, message));
        }

        #endregion

        #region Private

        private void CallForEnabled(Func<long, Task<Message>> action)
        {
            if (_settings.EnabledChatIds != null &&
                _settings.EnabledChatIds.Any())
            {
                _settings.EnabledChatIds.ToList().ForEach(x =>
                {
                    try
                    {
                        action(x);
                    }
                    catch(Exception ex)
                    {

                    }
                });
            }
        }

        private void Client_OnReceiveError(object sender, ReceiveErrorEventArgs e)
        {
        }

        private void Client_OnReceiveGeneralError(object sender, ReceiveGeneralErrorEventArgs e)
        {
        }

        private void Client_OnUpdate(object sender, UpdateEventArgs e)
        {
        }

        private void Client_OnMessage(object sender, MessageEventArgs e)
        {
            var client = sender as TelegramBotClient;

            if (_settings.EnabledUsers.Contains(e.Message.From.Username))
            {
                if (e.Message.Type == MessageType.TextMessage)
                {
                    if (e.Message.Entities.Any())
                    {
                        if (e.Message.Entities[0].Type == MessageEntityType.BotCommand)
                        {
                            var cmdText = e.Message.EntityValues[0].Replace("/", string.Empty);

                            if (Enum.TryParse(cmdText, true, out CommChannelCommandType cmd))
                            {
                                _commandHandler.HandleCommand(cmd, e.Message.Date);
                            }
                        }
                    }else
                    {
                        _commandHandler.HandleTextMessage(e.Message.Text);
                    }
                }
                else if (e.Message.Type == MessageType.PhotoMessage)
                {
                    var path = downloadFile(client, e.Message.Photo.LastOrDefault().FileId, "Picture").GetAwaiter().GetResult();
                    _mmHandler?.HandlePicture(path);
                }
                else if (e.Message.Type == MessageType.VideoMessage)
                {
                    var path = downloadFile(client, e.Message.Video.FileId, "Video").GetAwaiter().GetResult();
                    _mmHandler?.HandleVideo(path);
                }
                else if (e.Message.Type == MessageType.VoiceMessage)
                {
                    var res = downloadFile(client, e.Message.Voice.FileId, "Voice").GetAwaiter().GetResult();
                    _mmHandler?.HandleVoice(res);
                }
                else if (e.Message.Type == MessageType.AudioMessage)
                {
                    var path  = downloadFile(client, e.Message.Audio.FileId, "Audio").GetAwaiter().GetResult();
                    _mmHandler?.HandleAudio(path);
                }
            }
        }

        private static async Task<string> downloadFile(TelegramBotClient bot, string fileId, string type)
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

        #endregion
    }
}
