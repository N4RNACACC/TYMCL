using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using System.Threading.Tasks;
using TYMCL.Modules;

namespace TYMCL.UserControls;

public partial class LaunchBarControl : UserControl
{
    public LaunchBarControl()
    {
        InitializeComponent();
    }

    private async void GameLaunchButton_Click(object sender, RoutedEventArgs e)
    {
        await GameTools.GameLaunchActive();
    }
}