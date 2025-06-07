using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using System;
using System.Configuration;
using TYMCL.Pages;
using TYMCL.UserControls;
using TYMCL.Modules;
using System.IO;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace TYMCL
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Logger.Log.Info("启动器-Window", "初始化窗口-MainWindow");
            InitializeComponent();
            BackgroundImage_Set();

            var homePage = new HomePage();
            var launchbar = new LaunchBarControl();

            // 默认显示主页
            MainContent.Content = homePage;
            LaunchBarContent.Content = launchbar;
        }

        private readonly string AppDir = AppContext.BaseDirectory;

        private void Window_MinimizeButton_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e) // 最小化窗口
        {
            Logger.Log.Info("启动器-Window", "最小化窗口");
            WindowState = WindowState.Minimized;
        }

        private void Window_CloseButton_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e) // 关闭窗口
        {
            Logger.Log.Info("启动器-Window", "关闭窗口");
            Close();
        }

        private void Border_PointerPressed(object sender, PointerPressedEventArgs e) // 窗口拖动
        {
            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            {
                BeginMoveDrag(e);
            }
        }

        private void BackgroundImage_Set() // 设置背景图片
        {
            var backgroundImage = Path.Combine(AppDir, "resources", "background.jpg");

            if (backgroundImage != null && File.Exists(backgroundImage))
            {
                Logger.Log.Info("启动器-Border", "设置背景图片");
                try
                {
                    var bitmap = new Bitmap(backgroundImage);
                    var imageBrush = new ImageBrush
                    {
                        Source = bitmap,
                        Stretch = Stretch.UniformToFill,
                    };
                    ContentBorder.Background = imageBrush;
                }
                catch (Exception ex)
                {
                    Logger.Log.Error("启动器-Border",$"设置背景图片失败 \n错误: {ex.Message}");
                    ContentBorder.Background = new SolidColorBrush(Colors.White);
                }
            } else
            {
                Logger.Log.Warn("启动器-Border", "背景图片不存在-使用默认背景");
                ContentBorder.Background = new SolidColorBrush(Colors.White);
            }
            
        }

        private void NavigateToHome(object sender, Avalonia.Interactivity.RoutedEventArgs e) // 切换到主页
        {
            Logger.Log.Info("Content", "切换到主页");
            MainContent.Content = new HomePage();
        }

        private void NavigateToGameManage(object sender, Avalonia.Interactivity.RoutedEventArgs e) // 切换到游戏管理页
        {   
            var gameManagePage = new GameManagePage();
            Logger.Log.Info("Content", "切换到游戏管理页");
            MainContent.Content = gameManagePage;
        }

        private void NavigateToSettings(object sender, Avalonia.Interactivity.RoutedEventArgs e) // 切换到设置页
        {
            var settingsPage = new SettingsPage();
            Logger.Log.Info("Content", "切换到设置页");
            MainContent.Content = settingsPage;
        }



    }
}