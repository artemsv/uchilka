using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Uchilka.Logic
{
    internal class DataController
    {
        private readonly string _dataPath;
        private readonly DirectoryInfo _dataDirInfo;

        public DataController(string dataPath)
        {
            _dataPath = dataPath;
            _dataDirInfo = new DirectoryInfo(_dataPath);
        }

        public IEnumerable<string> GetCatalog()
        {
            return _dataDirInfo.EnumerateDirectories().Select(x => x.Name);
        }
    }
}
