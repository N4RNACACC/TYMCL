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
            Logger.Log.Info("��ʼ������-MainWindow");
            InitializeComponent();
            BackgroundImage_Set();

            var homePage = new HomePage();
            var launchbar = new LaunchBarControl();

            // Ĭ����ʾ��ҳ
            MainContent.Content = homePage;
            LaunchBarContent.Content = launchbar;
        }

        private readonly string AppDir = AppContext.BaseDirectory;

        private void Window_MinimizeButton_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e) // ��С������
        {
            Logger.Log.Info("��С������");
            WindowState = WindowState.Minimized;
        }

        private void Window_CloseButton_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e) // �رմ���
        {
            Logger.Log.Info("�رմ���");
            Close();
        }

        private void Border_PointerPressed(object sender, PointerPressedEventArgs e) // �����϶�
        {
            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            {
                BeginMoveDrag(e);
            }
        }

        private void BackgroundImage_Set() // ���ñ���ͼƬ
        {
            var backgroundImage = Path.Combine(AppDir, "resources", "background.jpg");

            if (backgroundImage != null && File.Exists(backgroundImage))
            {
                Logger.Log.Info("���ñ���ͼƬ");
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
                    Logger.Log.Error("���ñ���ͼƬʧ��", ex);
                    ContentBorder.Background = new SolidColorBrush(Colors.White);
                }
            } else
            {
                Logger.Log.Error("����ͼƬ������-ʹ��Ĭ�ϱ���");
                ContentBorder.Background = new SolidColorBrush(Colors.White);
            }
            
        }

        private void NavigateToHome(object sender, Avalonia.Interactivity.RoutedEventArgs e) // �л�����ҳ
        {
            Logger.Log.Info("�л�����ҳ");
            MainContent.Content = new HomePage();
        }

        private void NavigateToGameManage(object sender, Avalonia.Interactivity.RoutedEventArgs e) // �л�����Ϸ����ҳ
        {   
            var gameManagePage = new GameManagePage();
            Logger.Log.Info("�л�����Ϸ����ҳ");
            MainContent.Content = gameManagePage;
        }

        private void NavigateToSettings(object sender, Avalonia.Interactivity.RoutedEventArgs e) // �л�������ҳ
        {
            var settingsPage = new SettingsPage();
            Logger.Log.Info("�л�������ҳ");
            MainContent.Content = settingsPage;
        }



    }
}