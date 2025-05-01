using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using MinecraftLaunch;
using MinecraftLaunch.Utilities;
using TYMCL.Modules;

namespace TYMCL
{
    public class App : Application
    {
        public override void Initialize()
        {
            Logger.Initialize(); // 初始化日志系统
            Logger.Log.Info("======== START ========");

            AvaloniaXamlLoader.Load(this);

            DownloadMirrorManager.MaxThread = 256;
            DownloadMirrorManager.IsEnableMirror = true;
            HttpUtil.Initialize();
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new StartupWindow();

                desktop.Exit += (sender, e) => { Logger.Log.Info("======== EXIT ========"); };
            }

            base.OnFrameworkInitializationCompleted();
        }

    }
}