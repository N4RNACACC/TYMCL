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
            Logger.Log.Info("��ʼ������-MainWindow");
            InitializeComponent();

            // ��ʼ��ҳ��
            Logger.Log.Info("��ʼ��ҳ��");
            _homePage = new HomePage();
            _settingsPage = new SettingsPage();
            Logger.Log.Info("��ʼ��ҳ�����");

            // Ĭ����ʾ��ҳ
            MainContent.Content = _homePage;
        }

        // ҳ��
        private readonly UserControl _homePage;
        private readonly UserControl _settingsPage;

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

        private void NavigateToHome(object sender, Avalonia.Interactivity.RoutedEventArgs e) // �л�����ҳ
        {
            Logger.Log.Info("�л�����ҳ");
            MainContent.Content = _homePage;
        }

        private void NavigateToSettings(object sender, Avalonia.Interactivity.RoutedEventArgs e) // �л�������ҳ
        {
            Logger.Log.Info("�л�������ҳ");
            MainContent.Content = _settingsPage;
        }

    }
}