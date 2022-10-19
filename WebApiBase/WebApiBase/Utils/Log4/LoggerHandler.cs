using log4net;
using System.Reflection;

namespace WebApiBase.Utils.Log4
{
    public class LoggerHandler : ILoggerHandler
    {
        private static readonly ILog _Logger4net = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);

        public bool IsDebugEnabled => _Logger4net.IsDebugEnabled;

        public bool IsInfoEnabled => _Logger4net.IsInfoEnabled;

        public bool IsWarnEnabled => _Logger4net.IsWarnEnabled;

        public bool IsErrorEnabled => _Logger4net.IsErrorEnabled;

        public bool IsFatalEnabled => _Logger4net.IsFatalEnabled;

        public void Debug(string message)
        {
            if (IsDebugEnabled)
            {
                Log(LogLevel.Debug, message);
            }
        }

        public void Debug(string message, Exception exception)
        {
            if (IsDebugEnabled)
            {
                Log(LogLevel.Debug, message, exception);
            }
        }

        public void Info(string message)
        {
            if (IsInfoEnabled)
            {
                Log(LogLevel.Information, message);
            }
        }

        public void Info(string message, Exception exception)
        {
            if (IsInfoEnabled)
            {
                Log(LogLevel.Information, message, exception);
            }
        }

        public void Warn(string message)
        {
            if (IsWarnEnabled)
            {
                Log(LogLevel.Warning, message);
            }
        }

        public void Warn(string message, Exception exception)
        {
            if (IsWarnEnabled)
            {
                Log(LogLevel.Warning, message, exception);
            }
        }

        public void Error(string message)
        {
            if (IsErrorEnabled)
            {
                Log(LogLevel.Error, message);
            }
        }

        public void Error(string message, Exception exception)
        {
            if (IsErrorEnabled)
            {
                Log(LogLevel.Error, message, exception);
            }
        }


        private void Log(LogLevel level, string format)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    _Logger4net.Debug(format);
                    break;
                case LogLevel.Information:
                    _Logger4net.Info(format);
                    break;
                case LogLevel.Warning:
                    _Logger4net.Warn(format);
                    break;
                case LogLevel.Error:
                    _Logger4net.Error(format);
                    break;
                case LogLevel.Critical:
                    _Logger4net.Fatal(format);
                    break;
            }
        }

        private void Log(LogLevel level, string message, Exception exception)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    _Logger4net.Debug(message, exception);
                    break;
                case LogLevel.Information:
                    _Logger4net.Info(message, exception);
                    break;
                case LogLevel.Warning:
                    _Logger4net.Warn(message, exception);
                    break;
                case LogLevel.Error:
                    _Logger4net.Error(message, exception);
                    break;
                case LogLevel.Critical:
                    _Logger4net.Fatal(message, exception);
                    break;
            }
        }
    }
}
