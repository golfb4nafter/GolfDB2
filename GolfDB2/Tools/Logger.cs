using System;
using log4net;
using log4net.Config;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace GolfDB2.Tools
{
    public enum LogLevel
    {
        FATAL = 1,
        ERROR = 2,
        WARN = 3,
        INFO = 4,
        DEBUG = 5
    }

    public class LogLine
    {
        public string Method { get; set; }
        public string Message { get; set; }
        public string FilePath { get; set; }
        public int LineNumber { get; set; }
    }

    public class Logger
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(Logger));

        private static bool bInitDone = false;

        public static ILog Log
        {
            get
            {
                CheckInit();
                return _logger;
            }
        }

        private static void CheckInit()
        {
            if (!bInitDone)
            {
                bInitDone = true;
                XmlConfigurator.Configure();
            }
        }

        public static void SetLogLevel(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.DEBUG:
                    ((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository()).Root.Level = log4net.Core.Level.Debug;
                    break;
                case LogLevel.INFO:
                    ((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository()).Root.Level = log4net.Core.Level.Info;
                    break;
                case LogLevel.WARN:
                    ((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository()).Root.Level = log4net.Core.Level.Warn;
                    break;
                case LogLevel.ERROR:
                    ((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository()).Root.Level = log4net.Core.Level.Error;
                    break;
                case LogLevel.FATAL:
                    ((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository()).Root.Level = log4net.Core.Level.Fatal;
                    break;
            }

            ((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository()).RaiseConfigurationChanged(EventArgs.Empty);
        }

        public static void LogError(string method, string message, [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            CheckInit();
            Log.Error(FormatLine(method, message, filePath, lineNumber));
        }

        public static void LogWarn(string method, string message, [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            CheckInit();
            Log.Warn(FormatLine(method, message, filePath, lineNumber));
        }

        public static void LogInfo(string method, string message, [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            CheckInit();
            Log.Info(FormatLine(method, message, filePath, lineNumber));
        }

        public static void LogDebug(string method, string message, [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            CheckInit();
            Log.Debug(FormatLine(method, message, filePath, lineNumber));
        }

        public static string FormatLine(string method, string message, string filePath, int lineNumber)
        {
            LogLine line = new LogLine() { Method = method, Message = message, FilePath = filePath, LineNumber = lineNumber };
            return JsonConvert.SerializeObject(line);
        }

    }
}