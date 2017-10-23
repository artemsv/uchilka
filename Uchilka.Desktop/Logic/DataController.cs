using System.Collections.Generic;
using System.IO;
using System.Linq;
using Uchilka.DataModels;

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

        public IEnumerable<string> GetTestCatalog()
        {
            if (!_dataDirInfo.Exists)
            {
                Directory.CreateDirectory(_dataDirInfo.FullName);
            }
            return _dataDirInfo.EnumerateDirectories().Select(x => x.Name);
        }

        public Test GetTest(string name)
        {
            return new Test
            {
                Name = name
            };
        }
    }
}
