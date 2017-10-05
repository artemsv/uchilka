using System.Net;
using System.Net.Http;
//using Telegram.Bot.Types;

namespace Uchilka.Logic
{
    internal class TelegramBot
    {
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
    }
}
