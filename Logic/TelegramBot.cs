using System.Net;
using System.Linq;
using System.Net.Http;
using System.Xml;
using System.Xml.Linq;
//using Telegram.Bot.Types;

namespace Uchilka.Logic
{
    internal class TelegramBot
    {
        private XmlNodeList userNodes;

        public TelegramBot()
        {
            var settingsFile = new XmlDocument();
            settingsFile.Load("secrets.xml");

            var token = settingsFile.DocumentElement.ChildNodes[0].ChildNodes[0].InnerText;
            userNodes = settingsFile.DocumentElement.ChildNodes[0].ChildNodes[1].ChildNodes;

            foreach(XmlElement node in users)
            {
                
            }

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
    }
}
