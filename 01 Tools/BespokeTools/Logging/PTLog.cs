using System;
using System.Linq;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace Protime.Bespoke.Tools.Logging
{
    public class PtLog : IPtLog
    {
        private readonly Logger _logger;

        private readonly string _name;
        private readonly string _logFile;
        private readonly string _level = "TRACE";
        private readonly string _layout = "${date:format=yyyy-MM-dd HH\\:mm\\:ss} | ${level:uppercase=true:padding=5:padCharacter= } | ${message} | ${exception:format=toString,Data}";
        private readonly string _layoutExtra = "${date:format=yyyy-MM-dd HH\\:mm\\:ss} | ${level:uppercase=true:padding=5:padCharacter= } | ${message} | ${exception:format=message}";
        private readonly char _indentLevelChar = '\t';
        private int _indentLevel;

        private readonly string _logFileExtra;
        private readonly string _logLevelExtra;

        public PtLog()
        {
            _logger = LogManager.GetCurrentClassLogger();
        }

        public PtLog(string name, string logFile, string level, string layout, char indentLevelChar)
        {
            _name = name;
            _logFile = logFile;
            if (!string.IsNullOrEmpty(level))
                _level = level;
            if (!string.IsNullOrEmpty(layout))
                _layout = layout;
            if (indentLevelChar != char.MinValue)
                _indentLevelChar = indentLevelChar;
            _indentLevel = 0;

            var configuration = CreateLoggingConfiguration();

            _logger = GetLogger(configuration);
        }

        public PtLog(string name, string logFile, string level, string layout, char indentLevelChar, string logFileExtra, string logLevelExtra)
        {
            _name = name;
            _logFile = logFile;
            if (!string.IsNullOrEmpty(level))
                _level = level;
            if (!string.IsNullOrEmpty(layout))
                _layout = layout;
            if (indentLevelChar != char.MinValue)
                _indentLevelChar = indentLevelChar;
            _indentLevel = 0;

            _logFileExtra = logFileExtra;
            _logLevelExtra = logLevelExtra;

            var configuration = CreateLoggingConfiguration();

            _logger = GetLogger(configuration);
        }

        public void Fatal(Exception ex)
        {
            _logger.Fatal(ex);
        }

        public void Fatal(string message, Exception ex)
        {
            _logger.Fatal(ex, FormatMessage(message));
        }

        public void Fatal(string message, object[] paramsObjects, Exception ex)
        {
            _logger.Fatal(ex, FormatMessage(message), paramsObjects);
        }

        public void Fatal(string message)
        {
            _logger.Fatal(FormatMessage(message));
        }

        public void Error(Exception ex)
        {
            _logger.Error(ex);
        }

        public void Error(string message, Exception ex)
        {
            _logger.Error(ex, FormatMessage(message));
        }

        public void Error(string message, object[] paramsObjects, Exception ex)
        {
            _logger.Error(ex, FormatMessage(message), paramsObjects);
        }

        public void Error(string message)
        {
            _logger.Error(FormatMessage(message));
        }

        public void Warn(Exception ex)
        {
            _logger.Warn(ex);
        }

        public void Warn(string message, Exception ex)
        {
            _logger.Warn(ex, FormatMessage(message));
        }

        public void Warn(string message, object[] paramsObjects, Exception ex)
        {
            _logger.Warn(ex, FormatMessage(message), paramsObjects);
        }

        public void Warn(string message)
        {
            _logger.Warn(FormatMessage(message));
        }

        public void Info(Exception ex)
        {
            _logger.Info(ex);
        }

        public void Info(string message, Exception ex)
        {
            _logger.Info(ex, FormatMessage(message));
        }

        public void Info(string message, object[] paramsObjects, Exception ex)
        {
            _logger.Info(ex, FormatMessage(message), paramsObjects);
        }

        public void Info(string message)
        {
            _logger.Info(FormatMessage(message));
        }

        public void Debug(Exception ex)
        {
            _logger.Debug(ex);
        }

        public void Debug(string message, Exception ex)
        {
            _logger.Debug(ex, FormatMessage(message));
        }

        public void Debug(string message, object[] paramsObjects, Exception ex)
        {
            _logger.Debug(ex, FormatMessage(message), paramsObjects);
        }

        public void Debug(string message)
        {
            _logger.Debug(FormatMessage(message));
        }

        public void Trace(Exception ex)
        {
            _logger.Trace(ex);
        }

        public void Trace(string message, Exception ex)
        {
            _logger.Trace(ex, FormatMessage(message));
        }

        public void Trace(string message, object[] paramsObjects, Exception ex)
        {
            _logger.Trace(ex, FormatMessage(message), paramsObjects);
        }

        public void Trace(string message)
        {
            _logger.Trace(FormatMessage(message));
        }

        public void IncreaseIndentMessages(int increaseBy)
        {
            _indentLevel += increaseBy;
        }

        public void DecreaseIndentMessages(int decreaseBy)
        {
            _indentLevel -= decreaseBy;
        }

        public void ResetIndentMessages()
        {
            _indentLevel = 0;
        }


        #region Private Methods

        private Logger GetLogger(LoggingConfiguration configuration)
        {
            Logger logger;
            if (configuration is null)
                return LogManager.GetCurrentClassLogger();

            var loggingFactory = new LogFactory(configuration);
            logger = loggingFactory.GetLogger(GetLoggingRuleName());
            return logger;
        }

        private LoggingConfiguration CreateLoggingConfiguration()
        {
            var configuration = new LoggingConfiguration();

            //Copy global(*) Logging configuration
            var globalConfig = LogManager.Configuration;
            if (globalConfig != null)
            {
                foreach (var loggingRule in globalConfig.LoggingRules)
                {
                    if (loggingRule.NameMatches("*"))
                    {
                        foreach (var target in loggingRule.Targets)
                        {
                            configuration.AddRule(loggingRule.Levels.Min(), loggingRule.Levels.Max(), target);
                        }
                    }
                }
            }
            
            //Set default File Logging for job
            if (!string.IsNullOrEmpty(_logFile) && !string.IsNullOrEmpty(_level))
                configuration.AddRule(GetLogLevel(_level), LogLevel.Fatal, CreateFileTarget(GetTargetName(), _logFile, _layout), GetLoggingRuleName());

            //Set extra File Logging for job
            if (!string.IsNullOrEmpty(_logFileExtra) && !string.IsNullOrEmpty(_logLevelExtra))
                configuration.AddRule(GetLogLevel(_logLevelExtra), LogLevel.Fatal, CreateFileTarget($"Extra{GetTargetName()}", _logFileExtra, _layoutExtra), GetLoggingRuleName());

            return configuration;
        }

        private static FileTarget CreateFileTarget(string name, string logFile, string layout)
        {
            return new FileTarget { Name = name, FileName = logFile, Layout = layout };
        }

        private string GetLoggingRuleName()
        {
            return $"Job_{_name}";
        }

        private string GetTargetName()
        {
            return $"File_{_name}";
        }

        private string FormatMessage(string message)
        {
            var messageResult = $"{string.Empty.PadLeft(_indentLevel, _indentLevelChar)}{message}";
            return messageResult;
        }

        private static LogLevel GetLogLevel(string logLevel)
        {
            LogLevel jobLogLevel;
            switch (logLevel.ToUpper())
            {
                case "TRACE":
                    jobLogLevel = LogLevel.Trace;
                    break;
                case "DEBUG":
                    jobLogLevel = LogLevel.Debug;
                    break;
                case "INFO":
                    jobLogLevel = LogLevel.Info;
                    break;
                case "WARN":
                    jobLogLevel = LogLevel.Warn;
                    break;
                case "ERROR":
                    jobLogLevel = LogLevel.Error;
                    break;
                case "FATAL":
                    jobLogLevel = LogLevel.Fatal;
                    break;
                default:
                    jobLogLevel = LogLevel.Off;
                    break;
            }

            return jobLogLevel;
        }

        #endregion
    }
}