using System;
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
        private readonly DirectoryInfo _testsDirInfo;

        public DataController(string dataPath)
        {
            _dataPath = dataPath;
            _dataDirInfo = new DirectoryInfo(_dataPath);
            _testsDirInfo = new DirectoryInfo(Path.Combine(_dataPath, "Tests"));
        }

        public IEnumerable<string> GetTestCatalog()
        {
            if (!_testsDirInfo.Exists)
            {
                Directory.CreateDirectory(_testsDirInfo.FullName);
            }
            return _testsDirInfo.EnumerateDirectories().Select(x => x.Name);
        }

        public Test GetTest(string name)
        {
            var testDir = new DirectoryInfo(Path.Combine(_testsDirInfo.FullName, name));
            var testFile = new FileInfo(Path.Combine(testDir.FullName, "test.txt"));

            var res = new Test
            {
                Name = name,
                Questions = GetQuestions(testFile)
            };

            return res;
        }

        private IEnumerable<Question> GetQuestions(FileInfo testFile)
        {
            var res = new List<Question>();

            using (var stream = testFile.OpenRead())
            {
                using (var reader = new StreamReader(stream))
                {
                    while(!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                    }
                }
            }

            return res;
        }
    }
}
