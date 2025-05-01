using Avalonia.Controls;
using System.IO;
using System.Threading.Tasks;
using System;
using TYMCL.Modules;
using Avalonia.Media.Imaging;
using Avalonia.Media;


namespace TYMCL;

public partial class StartupWindow : Window
{
    private bool _initializationComplete = false;
    private bool _environmentIsSafe = true;
    private bool _userChoseToContinue = false;

    public StartupWindow()
    {
        InitializeComponent();
        ThemeImage_Set();
        Loaded += async (sender, e) =>
        {
            await InitializeAppAsync();
            if (_initializationComplete == true)
            {
                SwitchToMainWindow();
            }
        };
    }

    private async Task InitializeAppAsync()
    {
        Logger.Log.Info("开始初始化流程");

        await Task.Delay(2000);
        ReadyIcon.Text = "\uf00c;";

        // 环境检查
        await Environment_Check();

        if (_environmentIsSafe == false && !_userChoseToContinue)
        {
            Logger.Log.Info("环境检查不通过");
            // 显示错误界面并等待用户选择
            await ShowErrorAndWaitForUserDecision();

            if (!_userChoseToContinue)
            {
                // 用户选择退出
                Environment.Exit(0);
                return;
            }
        }

        // 继续初始化流程
        if (_environmentIsSafe == true || _userChoseToContinue)
        {
            CheckEnvIcon.Text = "\uf00c;";
            Logger.Log.Info("环境检查通过");
            // 更新检查
            await Update_Check();

            _initializationComplete = true;
            
            InitIcon.Text = "\uf00c";

            await Task.Delay(2000);
        }
    }

    private async Task ShowErrorAndWaitForUserDecision() // 显示错误界面并等待用户决定
    {
        var errorControl = new ErrorDisplayUserControl();
        ErrorPageUserControl.Content = errorControl;

        // 创建任务完成源来等待用户决定
        var tcs = new TaskCompletionSource<bool>();

        errorControl.ContinueRequested += () => {
            _userChoseToContinue = true;
            Logger.Log.Info("继续初始化流程");
            tcs.SetResult(true);
        };

        errorControl.ExitRequested += () => {
            Logger.Log.Info("退出App");
            tcs.SetResult(false);
        };

        await tcs.Task;
    }

    private void ThemeImage_Set() // 设置启动主题图
    {
        Logger.Log.Info("设置启动主题图");
        var appDir = AppContext.BaseDirectory;
        var themeImageDir = Path.Combine(appDir, "resources", "Theme.png");

        if (File.Exists(themeImageDir))
        {
            Logger.Log.Info("设置自定义启动主题图");
            var bitmap = new Bitmap(themeImageDir);
            var imageBrush = new ImageBrush
            {
                Source = bitmap,
                Stretch = Stretch.UniformToFill,
            };
            themeimageBorder.Background = imageBrush;
        }
        else
        {
            Logger.Log.Info("未设置自定义启动主题图-使用默认主题图");
        }
    }

    private async Task Update_Check() // 更新检查
    {
        Logger.Log.Info("检查可用更新");
        await Task.Delay(5000); //
        Logger.Log.Info("更新检查-完成");
        CheckUpdateIcon.Text = "\uf00c";
    }

    private async Task Environment_Check() // 环境检查
    {
        Logger.Log.Info("开始环境检查");
        // APP目录
        var appDir = Path.GetFullPath(AppContext.BaseDirectory);
        Logger.Log.Info("APP目录:" + appDir);
        //系统检查
        var osVersion = Environment.OSVersion.VersionString;
        Logger.Log.Info("OS:" + osVersion);

        // 发布类型检查
        Logger.Log.Info("----------发布类型检查--------");
#if DEBUG
        Logger.Log.Info("App发布类型:Debug");
        Logger.Log.Info("App处于开发环境");
#else
        Logger.Log.Info("App发布类型:Release");
#endif
        Logger.Log.Info("----------------------------");

        Logger.Log.Info("----------Java检查----------");
        // Java检查
        await GameTools.JavaWatch();
        Logger.Log.Info("----------------------------");

        Logger.Log.Info("---------App目录检查---------");
        // 临时目录索引
        var tempPaths = new[]
        {
            Path.GetTempPath(),
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            Environment.GetFolderPath(Environment.SpecialFolder.InternetCache),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "AppData", "Local", "Temp"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "AppData", "Local", "7-Zip"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "AppData", "Local", "RAR"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "AppData", "Local", "WinRAR")
        };

        // 检查临时目录
        foreach (var tempPath in tempPaths)
        {
            var fullTempPath = Path.GetFullPath(tempPath);
            if (appDir.StartsWith(fullTempPath, StringComparison.OrdinalIgnoreCase))
            {
                var message = $"应用程序正在临时目录中运行:\n{appDir}\n\n";
                Logger.Log.Error(message);
                _environmentIsSafe = false;
                CheckEnvIcon.Text = "!";
                return;
            }
        }

        // App目录索引
        var appDirs = new[]
        {
            Path.Combine(appDir, "resources"),
            Path.Combine(appDir, "configs"),
            Path.Combine(appDir, ".minecraft")
        };

        // 检查App目录
        foreach (var dir in appDirs)
        {
            if (!Directory.Exists(dir))
            {
                Logger.Log.Warn($"目录不存在: {dir}");
                try
                {
                    Directory.CreateDirectory(dir);
                    Logger.Log.Info($"已创建目录: {dir}");
                }
                catch (Exception ex)
                {
                    var message = $"无法创建必要目录: {dir}\n错误: {ex.Message}";
                    Logger.Log.Error(message);
                    _environmentIsSafe = false;
                    CheckEnvIcon.Text = "!";
                    return;
                }
            }
            else
            {
                Logger.Log.Info($"目录已验证: {dir}");
            }
        }
        Logger.Log.Info("----------------------------");
    }

    private void SwitchToMainWindow() // 切换到主窗口
    {
        var mainWindow = new MainWindow();
        mainWindow.Show();

        Close();
    }
}