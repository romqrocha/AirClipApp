using AirClipApp.ViewModels;
using Avalonia.Controls;
using Avalonia.Interactivity;
using VideoEditor;
using FFMpegCore;


namespace AirClipApp.Views;

/// <summary>
/// The page where the user should enter the path to their FFmpeg installation.
/// </summary>
/// <resources>
/// Followed <see href="https://docs.avaloniaui.net/docs/get-started/test-drive/">
/// Avalonia's Documentation </see> while creating this. For information about FFmpeg, see
/// <see href="https://ffmpeg.org/ffmpeg.html">FFmpeg Documentation</see>.
/// </resources>
/// <authors> Rodrigo Rocha, Taeyang Seo </authors>
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
        
        // Instantiate an FfmpegEditor
        //Data.FfmpegEditor = FfmpegEditor()
        if (Parent is ContentControl parent)
        {
            parent.Content = new ImportPage();
        }
    }
    
    // Create method that checks if the path is valid
}