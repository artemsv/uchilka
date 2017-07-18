﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}