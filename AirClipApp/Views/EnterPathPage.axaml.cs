using AirClipApp.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace AirClipApp.Views;

public partial class EnterPathPage : UserControl
{
    private EnterPathPageViewModel Data { get; }

    public EnterPathPage()
    {
        InitializeComponent();
        Data = new EnterPathPageViewModel();
        DataContext = Data;

    }
    public void OnSubmit(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(PathTextBox.Text))
        {
            ErrorText.Text = "Please enter a path before submitting.";
            return;
        }
        
        ErrorText.Text = "";
        Data.InputtedFfmpegPath = PathTextBox.Text;

        if (Parent is ContentControl parent)
        {
            parent.Content = new ImportPage();
        }
    }
}