using CommunityToolkit.Mvvm.ComponentModel;
using VideoEditor;

namespace AirClipApp.ViewModels;

/// <summary>
/// View Model for EnterPathPage
/// </summary>
/// <resources>
/// Followed <see href="https://docs.avaloniaui.net/docs/get-started/test-drive/">
/// Avalonia's Documentation </see> while creating this. For information about FFmpeg, see
/// <see href="https://ffmpeg.org/ffmpeg.html">FFmpeg Documentation</see>.
/// </resources>
/// <authors> Rodrigo Rocha, Tae Seo </authors>
public class EnterPathPageViewModel : ObservableObject
{
    public string FfmpegPathPrompt { get; set; } = 
        "Please enter the absolute path of your installed Ffmpeg binaries:";

    public string TempFilesPathPrompt { get; set; } =
        "Please enter the absolute path of where you would like to store temporary files:";

    public string ChocolateyPathExample =>
        @"C:\ProgramData\chocolatey\lib\ffmpeg\tools\ffmpeg\bin\";

    public string TempFilesPathExample =>
        "/Users/myname/Downloads";

    public string InputtedFfmpegPath { get; set; } = string.Empty;

    public string InputtedTempFilesPath { get; set; } = string.Empty;

}