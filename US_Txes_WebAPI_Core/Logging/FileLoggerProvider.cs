using Microsoft.Extensions.Logging;

namespace US_Txes_WebAPI_Core.Logging
{
    public class FileLoggerProvider : ILoggerProvider
    {
        private readonly string _filePath;
        public FileLoggerProvider(string filePath)
        {
            _filePath = filePath;
        }
        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(_filePath);
        }

        public void Dispose()
        {
            
        }
    }
}
