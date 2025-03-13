using AirClipApp.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace AirClipApp.Views;

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