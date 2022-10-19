namespace WebApiBase.Utils.Log4
{
    public interface ILoggerHandler
    {
        public void Debug(string message);

        public void Debug(string message, Exception exception);

        public void Info(string message);

        public void Info(string message, Exception exception);

        public void Warn(string message);

        public void Warn(string message, Exception exception);

        public void Error(string message);

        public void Error(string message, Exception exception);
    }
}
