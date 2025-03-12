using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace AirClipApp;

public partial class HomePage : UserControl
{
    public HomePage()
    {
        InitializeComponent();
    }
    private void OnGetStartedClick(object? sender, RoutedEventArgs e)
    {
        if (this.Parent is ContentControl contentControl)
        {
            contentControl.Content = new EnterPathPage();
        }
    }
}