namespace AirClipApp.ViewModels;

public class EnterPathPageViewModel : ViewModelBase
{
    public string PathPrompt => 
        "Please enter the absolute path of your installed Ffmpeg binaries:";
    
    public string ChocolateyPathExample =>
        @"C:\ProgramData\chocolatey\lib\ffmpeg\tools\ffmpeg\bin\";

    public string InputtedFfmpegPath { get; set; } = string.Empty;
}