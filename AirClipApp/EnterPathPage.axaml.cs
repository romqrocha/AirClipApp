using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace AirClipApp;

public partial class EnterPathPage : UserControl
{
    public EnterPathPage()
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