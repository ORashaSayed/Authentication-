using Serilog;
using Serilog.Events;

namespace JWT.API.Helpers
{
    public static class AzureLogBuilder
    {
        private const string OutputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] ({SourceContext}) {Message}{NewLine}{Exception}";

        public static ILogger CreateLogger(bool logToConsole = true)
        {
            Log.Logger = Configure(logToConsole).CreateLogger();

            Log.Logger.Information("Logs can be found at: {logs}", "Azure Web App Configured Log Path");

            return Log.Logger;
        }

        private static LoggerConfiguration Configure(bool logToConsole)
        {
            var logger = new LoggerConfiguration()
                    // Log Includes: Debug, Information, Warning, Error and Fatal
                    .MinimumLevel.Debug()
                    .Enrich.FromLogContext()
                    .WriteTo.AzureApp(LogEventLevel.Error, outputTemplate: OutputTemplate);

            if (logToConsole)
            {
                logger = logger.WriteTo.Async(config => config.Console(outputTemplate: OutputTemplate));
            }

            return logger;
        }
    }
}
