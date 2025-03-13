namespace AirClipApp.ViewModels;

public class EnterPathPageViewModel : ViewModelBase
{
    public string PathPrompt => 
        "Please enter the absolute path to your installed Ffmpeg binaries:";
    
    public string ChocolateyPathExample =>
        @"C:\ProgramData\chocolatey\lib\ffmpeg\tools\ffmpeg\bin\";

    public string ErrorMsg { get; set; } = string.Empty;
}