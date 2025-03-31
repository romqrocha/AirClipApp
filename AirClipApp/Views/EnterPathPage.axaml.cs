using AirClipApp.ViewModels;
using Avalonia.Controls;
using Avalonia.Interactivity;
using System.IO;
using System.Diagnostics;
using VideoEditor;

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
    // FFmpeg path: /opt/homebrew/Cellar/ffmpeg/7.1.1_1/bin/
    // Temp files path: /Users/danielseo/Downloads/temp_files
    private EnterPathPageViewModel Data { get; }

    public EnterPathPage()
    {
        InitializeComponent();
        Data = new EnterPathPageViewModel();
        DataContext = Data;
        
    }
    public void OnSubmit(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(FfmpegPathTextBox.Text) || string.IsNullOrWhiteSpace(TempFilesPathTextBox.Text))
        {
            ErrorText.Text = "Please enter a path before submitting.";
            return;
        }
        
        ErrorText.Text = "";
        Data.InputtedFfmpegPath = FfmpegPathTextBox.Text;
        Data.InputtedTempFilesPath = TempFilesPathTextBox.Text;

        // Instantiate an FfmpegEditor
        if (IsPathValid(Data.InputtedFfmpegPath))
        {
            MainWindowViewModel.FfmpegEditor = new FfmpegEditor(new DirectoryInfo(Data.InputtedFfmpegPath),
                new DirectoryInfo(Data.InputtedTempFilesPath));
            
        }
        else
        {
            ErrorText.Text = "The path you have entered is not valid. Try again.";
            return;
        }
        
        if (Parent is ContentControl contentControl)
        {
            contentControl.Content = new ImportPage();
        }

    }
    
    // Method that checks if the path is valid (might want to move this to EnterPathPageViewModel)
    private bool IsPathValid(string inputtedFfmpegPath)
    {
        if (!Directory.Exists(inputtedFfmpegPath)) return false;
        
        DirectoryInfo ffmpegDirectory = new DirectoryInfo(inputtedFfmpegPath);
        bool ffmpegExists = File.Exists(Path.Combine(ffmpegDirectory.FullName, "ffmpeg.exe")) ||
                            File.Exists(Path.Combine(ffmpegDirectory.FullName, "ffmpeg")); 

        bool ffprobeExists = File.Exists(Path.Combine(ffmpegDirectory.FullName, "ffprobe.exe")) ||
                             File.Exists(Path.Combine(ffmpegDirectory.FullName, "ffprobe"));

        return ffmpegExists && ffprobeExists;
    }

}