using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AirClipApp.Views;
using AirClipCCL.Views;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VideoEditor;

namespace AirClipApp.ViewModels;

/// <summary>
/// View Model for the app's main window.
/// </summary>
/// <authors> Rodrigo Rocha, Taeyang Seo </authors>
public partial class MainWindowViewModel : ObservableObject
{
    #region Pages
    
    /// <summary>
    /// Object containing every page belonging to the main window.
    /// </summary>
    public static Pages Pages { get; } = new Pages();
    
    [ObservableProperty] private UserControl _activePage = Pages.HomePage;
    [RelayCommand] public void SetActivePage(UserControl page) 
        => ActivePage = page;
    
    #endregion

    #region EnterPathPage

    [ObservableProperty] private string _errorText =
        "";

    [ObservableProperty] private string _ffmpegPathPrompt = 
        "Please enter the absolute path of your installed Ffmpeg binaries:";

    [ObservableProperty] private string _tempFilesPathPrompt = 
        "Please enter the absolute path of where you would like to store " +
        "temporary files:";

    private const string ChocolateyPathExample =
        @"C:\ProgramData\chocolatey\lib\ffmpeg\tools\ffmpeg\bin\";
    
    [ObservableProperty] private string _inputtedFfmpegPath = 
        ChocolateyPathExample;

    [ObservableProperty] private string _inputtedTempPath = 
        GetAssemblyPath();
    
    /// <returns>
    /// The absolute path to the current assembly.
    /// </returns>
    private static string GetAssemblyPath()
    {
        return Path.GetDirectoryName(System.Reflection.Assembly.
            GetExecutingAssembly().Location) ?? "null";
    }

    /// <summary>
    /// Verifies that the given paths are valid. If they are, changes the page.
    /// </summary>
    [RelayCommand]
    private void VerifyPaths()
    {
        if (!IsFfmpegPathValid(InputtedFfmpegPath))
        {
            ErrorText = "The ffmpeg path you have entered is not valid. " +
                        "Try again.";
            return;
        }
        if (!IsDirectoryValid(InputtedTempPath))
        {
            ErrorText = "The temporary directory you have entered is not valid.";
            return;
        }
        
        ErrorText = "";
        FfmpegEditor = new FfmpegEditor(new DirectoryInfo(InputtedFfmpegPath),
            new DirectoryInfo(InputtedTempPath));
        
        SetActivePage(Pages.ImportPage);
    }
    
    /// <summary>
    /// Checks if the ffmpeg directory path is valid. The path should lead to
    /// the ffmpeg binaries folder.
    /// </summary>
    /// <param name="ffmpegPath">The path to check.</param>
    /// <returns>True if the path is valid.</returns>
    private static bool IsFfmpegPathValid(string ffmpegPath)
    {
        if (!Directory.Exists(ffmpegPath)) return false;
        
        DirectoryInfo ffmpegDirectory = new DirectoryInfo(ffmpegPath);
        
        if (!IsDirectoryValid(ffmpegDirectory.FullName)) 
            return false;
        
        bool ffmpegExists = File.Exists(Path.Combine(
                                ffmpegDirectory.FullName, "ffmpeg.exe")) ||
                            File.Exists(Path.Combine(
                                ffmpegDirectory.FullName, "ffmpeg")); 

        bool ffprobeExists = File.Exists(Path.Combine(
                                 ffmpegDirectory.FullName, "ffprobe.exe")) ||
                             File.Exists(Path.Combine(
                                 ffmpegDirectory.FullName, "ffprobe"));

        return ffmpegExists && ffprobeExists;
    }

    /// <summary>
    /// Checks if the given path is a valid directory.
    /// </summary>
    /// <param name="path">The path to check.</param>
    /// <returns>True if the path is a valid directory.</returns>
    private static bool IsDirectoryValid(string path)
    {
        var dirInfo = new DirectoryInfo(path);
        
        if (!dirInfo.Exists) 
            return false;

        return true;
    }
    
    #endregion
    
    #region ImportPage
    
    [ObservableProperty] private string _importPrompt = 
        "Import a video to get started!";

    [ObservableProperty] private string _importStatus = 
        "";
    
    /// <summary>
    /// A filter that specifies the video formats that are allowed when
    /// selecting the video file.
    /// </summary>
    private static FilePickerFileType VideoSupported { get; } = 
        new FilePickerFileType("Supported Videos") // Custom file type filter
    {
        // Limits file selection to MP4, MOV, and WebM files.
        Patterns = ["*.mp4", "*.mov", "*.webm"],
        // Limits file selection to MP4, MOV, and WebM files on macOS.
        AppleUniformTypeIdentifiers = ["public.mpeg-4", "com.apple.quicktime-movie", "org.webmproject.webm"], 
        // Limits file selection to MP4, MOV, and WebM files on Linux.
        MimeTypes = ["video/mp4", "video/quicktime", "video/webm"]
    };

    /// <summary>
    /// Import a video file from the local file system. 'async' keyword allows the method
    /// to run asynchronously, meaning it doesn't block the main thread while performing
    /// the file picker operation.
    /// </summary>
    /// <param name="topLevel"> Represents a top-level window or application window.
    /// Alternatively, we can switch to a Window reference later.</param>
    private static async Task<string> ImportFromFileSystem(TopLevel topLevel)
    {
        // Read-only list of IStorageFile objects
        // IStorageFile is an interface that represents a file in the storage system
        IReadOnlyList<IStorageFile> files = 
            await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Open Text File",
                
                // TODO: fix 'ImageAll'
                FileTypeFilter = [VideoSupported, FilePickerFileTypes.ImageAll],
                
                AllowMultiple = false
            });

        if (files.Count < 1) 
            return string.Empty;
        
        string videoPath = files[0].Path.ToString();
        return videoPath;
    }
    
    [RelayCommand]
    private async Task ValidateImport()
    {
        var topLevel = TopLevel.GetTopLevel(Pages.ImportPage);
        if (topLevel is null)
            return;

        ImportStatus = "Importing...";
        string videoPath = await ImportFromFileSystem(topLevel);
        
        // Removes the first 7 characters of the path because they make the path invalid.
        // Must make sure the video path string is not empty before calling Substring, otherwise
        // it will crash the program.
        if (!string.IsNullOrEmpty(videoPath))
        {
            videoPath = videoPath.Substring(8);    
        }
        ImportStatus = $"Importing '{videoPath}' ...";

        if (!IsVideoPathValid(videoPath))
        {
            ImportStatus = "The video file is not valid. Try again.";
            return;
        }
        
        Video = new Video(videoPath);
        SetActivePage(Pages.EditorPage);
    }

    /// <summary>
    /// Checks if the given path exists.
    /// </summary>
    /// <param name="videoPath">The path to check.</param>
    /// <returns>True if a file exists in that path.</returns>
    private static bool IsVideoPathValid(string videoPath)
    {
        return File.Exists(videoPath);
    }
    
    #endregion
    
    #region EditorPage
    
    [ObservableProperty] private Control _activeEditingDetailsControl = new Panel();
    [RelayCommand] public void SetDetailsControl() 
        => ActiveEditingDetailsControl = new TrimDetails();
    
    public static FfmpegEditor? FfmpegEditor { get; set; }
    public static Video? Video { get; set; }
    public static VideoEditor.VideoEditor? VideoEditor { get; set; }
    
    #endregion
    
    
}