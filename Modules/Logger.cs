// 程序日志记录器
// 日志将保存在 ｛根目录｝\logs 文件夹下，文件名格式为 log_yyyyMMddHHmmss.log
// 使用方式：在任意Class中插入 Logger.Log.｛level｝("操作类型","｛日志信息｝");
using System;
using log4net;
using log4net.Config;
using System.IO;
using log4net.Appender;
using log4net.Layout;
using log4net.Core;

namespace TYMCL.Modules
{
    public static class Logger
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Logger));
        private const string LogsDirectory = "logs";

        static Logger()
        {
            Configure();
        }

        private static void Configure()
        {
            Directory.CreateDirectory(LogsDirectory);
            var logRepository = LogManager.GetRepository();

            // 构建日志格式
            var patternLayout = new PatternLayout
            {
                ConversionPattern = "[%date{yyyy-MM-dd HH:mm:ss}][%property{operationType}][%level] - %message%newline"
            };
            patternLayout.ActivateOptions();

            var fileAppender = new RollingFileAppender
            {
                Name = "FileAppender",
                File = Path.Combine(LogsDirectory, $"TYMCL-log-{DateTime.Now:yyyyMMddHHmmss}.log"),
                AppendToFile = true,
                Layout = patternLayout,
                RollingStyle = RollingFileAppender.RollingMode.Once,
                MaxSizeRollBackups = 1,
                StaticLogFileName = false
            };
            fileAppender.ActivateOptions();

            if (IsDebugBuild())
            {
                var debugAppender = new DebugAppender()
                {
                    Name = "DebugAppender",
                    Layout = patternLayout
                };
                debugAppender.ActivateOptions();
                BasicConfigurator.Configure(logRepository, fileAppender, debugAppender);
            }
            else
            {
                BasicConfigurator.Configure(logRepository, fileAppender);
            }
        }

        private static bool IsDebugBuild() // Debug构建检查
        {
#if DEBUG
            return true;
#else
            return false;
#endif
        }

        private class DebugAppender : AppenderSkeleton
        {
            protected override void Append(LoggingEvent loggingEvent)
            {
                System.Diagnostics.Debug.WriteLine(RenderLoggingEvent(loggingEvent));
            }
        }

        public static class Log
        {
            public static void Debug(string operationType, string message) => LogMessage(operationType, message, log.Debug);
            public static void Info(string operationType, string message) => LogMessage(operationType, message, log.Info);
            public static void Warn(string operationType, string message) => LogMessage(operationType, message, log.Warn);
            public static void Error(string operationType, string message) => LogMessage(operationType, message, log.Error);
            public static void Fatal(string operationType, string message) => LogMessage(operationType, message, log.Fatal);

            // 异常方法添加 operationType
            public static void Error(string operationType, string message, Exception ex) => LogMessage(operationType, $"{message} - {ex}", log.Error);

            // 统一处理上下文属性
            private static void LogMessage(string operationType, string message, Action<object> logAction)
            {
                // 使用 LogicalThreadContext 确保异步安全
                LogicalThreadContext.Properties["operationType"] = operationType;
                logAction(message);
                LogicalThreadContext.Properties.Remove("operationType");
            }
        }
    }
}
