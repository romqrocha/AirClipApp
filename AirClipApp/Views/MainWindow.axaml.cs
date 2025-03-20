using Avalonia.Controls;
using Avalonia.Interactivity;

namespace AirClipApp.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        PageContent.Content = new EditorPage(); // Show Home page when the app starts
    }

    
}