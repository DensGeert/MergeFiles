using Protime.Bespoke.Tools.Helpers;

namespace Protime.Bespoke.Tools.Logging
{
    public static class PtLogFactory
    {
        public static IPtLog Create(string name, string defaultLogFileString, string defaultLogLevel, string logExtraFileString, string logExtraLevel)
        {
            //SET DEFAULT FILE LOGGING
            if (!string.IsNullOrEmpty(defaultLogFileString))
            {
                var defaultLogFileInfo = defaultLogFileString.GetFileInfo();
                if (defaultLogFileInfo?.Directory is null || !defaultLogFileInfo.Directory.Exists)
                    return new PtLog();
            }
            defaultLogLevel = string.IsNullOrEmpty(defaultLogLevel) ? "INFO" : defaultLogLevel;

            //SET EXTRA FILE LOGGING
            logExtraLevel = string.IsNullOrEmpty(logExtraLevel) ? "INFO" : logExtraLevel;
            
            if (string.IsNullOrEmpty(logExtraFileString) || string.IsNullOrEmpty(logExtraLevel))
                return new PtLog(name, defaultLogFileString, defaultLogLevel, null, char.MinValue);

            var logFileInfo = logExtraFileString.GetFileInfo();
            if (logFileInfo?.Directory is null || !logFileInfo.Directory.Exists)
                return new PtLog(name, defaultLogFileString, defaultLogLevel, null, char.MinValue);
            
            return new PtLog(name, defaultLogFileString, defaultLogLevel, null, char.MinValue, logExtraFileString, logExtraLevel);
        }
    }
}