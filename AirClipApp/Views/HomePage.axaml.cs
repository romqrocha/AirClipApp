using AirClipApp.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace AirClipApp.Views;

/// <summary>
/// The page first initialized after the main window starts up.
/// </summary>
/// <resources>
/// Followed <see href="https://docs.avaloniaui.net/docs/get-started/test-drive/">
/// Avalonia's Documentation </see> while creating this.
/// </resources>
/// <authors> Rodrigo Rocha, Tae Seo </authors>
public partial class HomePage : UserControl
{
    public HomePage()
    {
        InitializeComponent();
        DataContext = new HomePageViewModel();
    }
    private void OnGetStartedClick(object? sender, RoutedEventArgs e)
    {
        if (Parent is ContentControl contentControl)
        {
            contentControl.Content = new EnterPathPage();
        }
    }
}