using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using TYMCL.Services;

namespace TYMCL.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void WindowMove(object sender, PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);
            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            {
                // 开始拖动
                Logger.Log.Info("Window","拖动窗口");
                BeginMoveDrag(e);
            }
        }

        private void MinimizeWindow(object sender, RoutedEventArgs e)
        {
            Logger.Log.Info("Window","最小化窗口");
            WindowState = WindowState.Minimized; // 最小化
        }
        
        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Logger.Log.Info("EXIT","=============== EXIT ================");
            Close(); // 退出
        }
    }
}
