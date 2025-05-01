using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using System;
using System.Configuration;
using TYMCL.Pages;
using TYMCL.Modules;

namespace TYMCL
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Logger.Log.Info("初始化窗口-MainWindow");
            InitializeComponent();

            // 初始化页面
            Logger.Log.Info("初始化页面");
            _homePage = new HomePage();
            _settingsPage = new SettingsPage();
            Logger.Log.Info("初始化页面完成");

            // 默认显示主页
            MainContent.Content = _homePage;
        }

        // 页面
        private readonly UserControl _homePage;
        private readonly UserControl _settingsPage;

        private void Window_MinimizeButton_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e) // 最小化窗口
        {
            Logger.Log.Info("最小化窗口");
            WindowState = WindowState.Minimized;
        }

        private void Window_CloseButton_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e) // 关闭窗口
        {
            Logger.Log.Info("关闭窗口");
            Close();
        }

        private void Border_PointerPressed(object sender, PointerPressedEventArgs e) // 窗口拖动
        {
            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            {
                BeginMoveDrag(e);
            }
        }

        private void NavigateToHome(object sender, Avalonia.Interactivity.RoutedEventArgs e) // 切换到主页
        {
            Logger.Log.Info("切换到主页");
            MainContent.Content = _homePage;
        }

        private void NavigateToSettings(object sender, Avalonia.Interactivity.RoutedEventArgs e) // 切换到设置页
        {
            Logger.Log.Info("切换到设置页");
            MainContent.Content = _settingsPage;
        }

    }
}