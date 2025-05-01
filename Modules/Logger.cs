// 程序日志记录器
// 日志将保存在 ｛根目录｝\logs 文件夹下，文件名格式为 log_yyyyMMdd_HHmmss.log
// 使用方式：在任意Class中插入 Logger.Log.｛level｝("｛日志信息｝");
using log4net;
using log4net.Appender;
using log4net.Layout;
using System.IO;
using System;
using log4net.Repository.Hierarchy;
namespace TYMCL.Modules;

public static class Logger
{
    public static ILog Log { get; private set; }

    public static void Initialize()
    {
        // 创建日志目录
        var logDir = Path.Combine(Directory.GetCurrentDirectory(), "logs");
        if (!Directory.Exists(logDir)) Directory.CreateDirectory(logDir);

        // 生成带毫秒的时间戳文件名
        var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        var logPath = Path.Combine(logDir, $"log_{timestamp}.log");

        // 配置文件追加器
        var appender = new FileAppender
        {
            Name = "FileAppender",
            File = logPath,
            Layout = new PatternLayout("%date{[yyyyMMdd HHmmss]} [%thread] [%level] - %message%newline"),
            AppendToFile = true
        };
        appender.ActivateOptions();

        // 应用配置
        var hierarchy = (Hierarchy)LogManager.GetRepository();
        hierarchy.Root.RemoveAllAppenders();
        hierarchy.Root.AddAppender(appender);
        hierarchy.Root.Level = log4net.Core.Level.All;
        hierarchy.Configured = true;

        Log = LogManager.GetLogger(typeof(Logger));
    }
}