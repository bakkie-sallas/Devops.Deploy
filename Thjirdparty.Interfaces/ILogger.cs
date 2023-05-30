namespace Thirdparty.Interfaces
{
    public interface ILogger
    {
        public void Error(string message, object obj = null);
        public void Info(string message, object obj = null);
    }
}