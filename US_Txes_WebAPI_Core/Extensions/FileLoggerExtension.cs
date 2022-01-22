using Microsoft.Extensions.Logging;
using US_Txes_WebAPI_Core.Logging;

namespace US_Txes_WebAPI_Core.Extensions
{
    public static class FileLoggerExtension
    {
        public static ILoggerFactory AddFile(this ILoggerFactory loggerFactory, string filePath)
        {
            loggerFactory.AddProvider(new FileLoggerProvider(filePath));

            return loggerFactory;
        }
    }
}
