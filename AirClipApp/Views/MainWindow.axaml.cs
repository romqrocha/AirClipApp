using Avalonia.Controls;
using Avalonia.Interactivity;

namespace AirClipApp.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        PageContent.Content = new HomePage(); // Show Home page when the app starts
    }

    
}