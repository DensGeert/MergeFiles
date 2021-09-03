using System;

namespace Protime.Bespoke.Tools.Logging
{
    public interface IPtLog
    {
        void Fatal(Exception ex);
        void Fatal(string message, Exception ex);
        void Fatal(string message, object[] paramsObjects, Exception ex);
        void Fatal(string message);
        void Error(Exception ex);
        void Error(string message, Exception ex);
        void Error(string message, object[] paramsObjects, Exception ex);
        void Error(string message);
        void Warn(Exception ex);
        void Warn(string message, Exception ex);
        void Warn(string message, object[] paramsObjects, Exception ex);
        void Warn(string message);
        void Info(Exception ex);
        void Info(string message, Exception ex);
        void Info(string message, object[] paramsObjects, Exception ex);
        void Info(string message);
        void Debug(Exception ex);
        void Debug(string message, Exception ex);
        void Debug(string message, object[] paramsObjects, Exception ex);
        void Debug(string message);
        void Trace(Exception ex);
        void Trace(string message, Exception ex);
        void Trace(string message, object[] paramsObjects, Exception ex);
        void Trace(string message);
        void IncreaseIndentMessages(int increaseBy);
        void DecreaseIndentMessages(int decreaseBy);
        void ResetIndentMessages();
    }
}
