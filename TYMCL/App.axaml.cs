using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using TYMCL.ViewModels;
using TYMCL.Views;
using TYMCL.Services;

namespace TYMCL;

public partial class App : Application
{
    
    public override void Initialize()
    {
        Logger.Log.Info("INIT","================== START ===============");
        Logger.Log.Debug("axamlLoad","加载Xaml");
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        Logger.Log.Debug("INIT","设置起始View");
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainViewModel()
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = new MainViewModel()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
