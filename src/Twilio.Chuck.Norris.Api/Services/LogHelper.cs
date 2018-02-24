using System;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.Extensions.Logging.AzureAppServices;

namespace Twilio.Chuck.Norris.Api.Services
{
    public static class LogHelper
    {

        // useful reference https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging

        //private static readonly Logger _fileLogger;
        private static readonly ILogger _logger;

        static LogHelper()
        {
            //if (_fileLogger == null)
            //    _fileLogger = LogManager.GetCurrentClassLogger();

            var factory = new LoggerFactory();


            var loggingConfiguration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("logging.json", optional: false, reloadOnChange: true)
                .Build();

            factory.AddConsole(loggingConfiguration);

            // refer to https://blogs.msdn.microsoft.com/webdev/2017/04/26/asp-net-core-logging/
            factory.AddAzureWebAppDiagnostics(
                new AzureAppServicesDiagnosticsSettings
                {
                    OutputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss zzz} [{Level}] {RequestId}-{SourceContext}: {Message}{NewLine}{Exception}"
                });

            if (_logger == null)
                _logger = factory.CreateLogger("Twilio.Chuck.Norris.Api");
            


        }


        public static void Info(string message)
        {
            if (IsEnabled(Level.Information))
            {
                Trace.TraceInformation(GetTraceMsg(message));
                //LogToFile(message, Level.Information);
                LogToConsole(message, Level.Information);
            }
        }

        public static void Debug(string message)
        {
            if (IsEnabled(Level.Debug))
            {
                Trace.WriteLine(GetTraceMsg(message), Enum.GetName(typeof(Level), Level.Debug));
                //LogToFile(message, Level.Debug);
                LogToConsole(message, Level.Debug);
            }
        }

        public static void Warn(string message)
        {
            if (IsEnabled(Level.Warning))
            {
                Trace.TraceWarning(GetTraceMsg(message));
                //LogToFile(message, Level.Warning);
                LogToConsole(message, Level.Warning);
            }
        }

        public static void Error(string message, Exception ex = null)
        {
            if (IsEnabled(Level.Error))
            {
                Trace.TraceError(GetTraceMsg(message));
//                LogToFile(message, Level.Error, ex);
                LogToConsole(message, Level.Error, ex);
            }
        }

        public static void Verbose(string message)
        {
            if (IsEnabled(Level.Verbose))
            {
                Trace.WriteLine(GetTraceMsg(message), Enum.GetName(typeof(Level), Level.Verbose));
                //LogToFile(message, Level.Verbose);
                LogToConsole(message, Level.Verbose);
            }
        }


        private static bool IsEnabled(Level logLevel)
        {
            return true; //Get this from logging.json file (enable/disable various logging outputs)
        }



        //private static void LogToFile(string message, Level logLevel, Exception ex = null)
        //{
        //    NLog.LogLevel nlogLevel;
        //    switch (logLevel)
        //    {
        //        case Level.Error:
        //            nlogLevel = NLog.LogLevel.Error;
        //            break;
        //        case Level.Debug:
        //            nlogLevel = NLog.LogLevel.Debug;
        //            break;
        //        case Level.Warning:
        //            nlogLevel = NLog.LogLevel.Warn;
        //            break;
        //        case Level.Information:
        //            nlogLevel = NLog.LogLevel.Info;
        //            break;
        //        case Level.Verbose:
        //            nlogLevel = NLog.LogLevel.Trace;
        //            break;
        //        default:
        //            nlogLevel = NLog.LogLevel.Debug;
        //            break;
        //    }

        //    _fileLogger.Log(nlogLevel, ex, message);
        //}

        private static void LogToConsole(string message, Level logLevel, Exception ex = null)
        {
            switch (logLevel)
            {
                case Level.Error:
                    _logger.LogError(new EventId(1), ex, message);
                    break;
                case Level.Debug:
                    _logger.LogDebug(new EventId(1), ex, message);
                    break;
                case Level.Warning:
                    _logger.LogWarning(new EventId(1), ex, message);
                    break;
                case Level.Information:
                    _logger.LogInformation(new EventId(1), ex, message);
                    break;
                case Level.Verbose:
                    _logger.LogTrace(new EventId(1), ex, message);
                    break;
                default:
                    _logger.LogDebug(new EventId(1), ex, message);
                    break;
            }
        }

        private static string GetTraceMsg(string message)
        {
            return $"{DateTime.Now.ToString("u")} {message}";
        }

        public enum Level
        {
            Verbose,
            Warning,
            Debug,
            Error,
            Information
        }
    }
}

