﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging
{
    public class FileLoggerProvider : ILoggerProvider
    {
        private readonly Func<string, LogLevel, bool> _filter;
        private string _fileName;

        public FileLoggerProvider(Func<string, LogLevel, bool> filter, string fileName)
        {
            _filter = filter;
            _fileName = fileName;
        }
        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(categoryName, _filter, _fileName);
        }
        public void Dispose() { }
    }
}
