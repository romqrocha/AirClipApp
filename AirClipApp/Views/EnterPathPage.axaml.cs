using AirClipApp.ViewModels;
using Avalonia.Controls;
using Avalonia.Interactivity;
using VideoEditor;
using FFMpegCore;
using System.IO;

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
        if (IsPathValid(Data.InputtedFfmpegPath))
        {
            // Data.FfmpegEditor = FfmpegEditor()
        }
        if (Parent is ContentControl parent)
        {
            parent.Content = new ImportPage();
        }
    }
    
    // Method that checks if the path is valid by 
    public bool IsPathValid(string inputtedFfmpegPath)
    {
        DirectoryInfo ffmpegDirectory = new DirectoryInfo(inputtedFfmpegPath);
        string ffmpegFileName = "ffmpeg";
        string ffprobeFileName = "ffprobe";
        bool ffmpegExists = ffmpegDirectory.GetFiles(ffmpegFileName).Length > 0;
        bool ffprobeExists = ffmpegDirectory.GetFiles(ffprobeFileName).Length > 0;
        if (ffmpegExists && ffprobeExists) return true; 
        return false;
    }
}