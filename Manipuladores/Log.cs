using Microsoft.Extensions.Logging;


namespace edilex.Manipuladores
{
    public class Log
    {
        public static ILoggerFactory getLoggerFactory()
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                .AddFilter("Microsoft", LogLevel.Warning)
                .AddFilter("System", LogLevel.Warning)
                .AddFilter("NonHostConsoleApp.Program", LogLevel.Debug);
            });

            return loggerFactory;
        }
    }
}