using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace HedgePlatform.BLL.Infr
{
    public static class Log
    {
        public static ILoggerFactory LoggerFactory { get; set; }
        public static ILogger CreateLogger<t>() => LoggerFactory.CreateLogger<t>();
        public static ILogger CreateLogger(string categoryName) => LoggerFactory.CreateLogger(categoryName);
    }
}
