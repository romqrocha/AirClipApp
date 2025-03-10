using Avalonia.Controls;
using Avalonia.Interactivity;

namespace AirClipApp.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    public void OnSubmit(object? sender, RoutedEventArgs e)
    {
        string ffmpegPath;
        if (string.IsNullOrWhiteSpace(PathTextBox.Text))
        {
            ErrorText.Text = "Please enter a path before submitting.";
        }
        else
        {
            ErrorText.Text = "";
        }
    }
}