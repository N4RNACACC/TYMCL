using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;

namespace TYMCL;

public partial class ErrorDisplayUserControl : UserControl
{
    public event Action ContinueRequested;
    public event Action ExitRequested;

    public ErrorDisplayUserControl()
    {
        InitializeComponent();

        // 找到按钮并添加事件处理
        var continueButton = this.FindControl<Button>("ContinueButton");
        var exitButton = this.FindControl<Button>("ExitButton");

        continueButton.Click += (s, e) => ContinueRequested?.Invoke();
        exitButton.Click += (s, e) => ExitRequested?.Invoke();
    }
}