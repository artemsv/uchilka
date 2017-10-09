using System.Collections.Generic;
using System.IO;

namespace Uchilka.Logic
{
    internal class ConfigController
    {
        private readonly string _configPath;
        private readonly DirectoryInfo _configDirInfo;

        public ConfigController(string configPath)
        {
            _configPath = configPath;
            _configDirInfo = new DirectoryInfo(_configPath);
        }

        public IEnumerable<string> GetMainMenu()
        {
            return new List<string> { "geyrn 1", "werwerwer" };
        }
    }
}
