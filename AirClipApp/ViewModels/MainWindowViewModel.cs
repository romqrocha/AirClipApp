using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using AirClipApp.Views;
using AirClipCCL.ViewModels;
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
    [RelayCommand]
    private void SetActivePage(UserControl page) 
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
    
    private static readonly string _DefaultTempDirectory = GetAssemblyPath() + @"\Temp";
    
    [ObservableProperty] private string _inputtedFfmpegPath = 
        ChocolateyPathExample;

    [ObservableProperty] private string _inputtedTempPath =
        _DefaultTempDirectory;
    
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
        var tempDirInfo = new DirectoryInfo(_DefaultTempDirectory);
        if (!tempDirInfo.Exists)
        {
            tempDirInfo.Create();
        }
        
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
        
        _video = new Video(videoPath);
        SetActivePage(Pages.EditorPage);
        Pages.EditorPage.SubscribeToSelectedButtonChanged(SetDetailsControl);
    }
    
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
        AppleUniformTypeIdentifiers = ["public.mpeg-4", "com.apple.quicktime-movie", 
            "org.webmproject.webm"],
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
    
    [ObservableProperty] private OperationDetailsControl _activeEditingDetailsControl 
        = new TrimDetails();
    
    private Video? _video;

    private readonly string _outPath = GetAssemblyPath() + @"\TempVideos";
    
    public VideoEditor.VideoEditor VideoEditor
    {
        get
        {
            if (_videoEditor is not null)
                return _videoEditor;

            if (_video is null)
                throw new NullReferenceException("_video is null");

            var ffmpegBinFolder = new DirectoryInfo(InputtedFfmpegPath);
            var ffmpegTempFolder = new DirectoryInfo(InputtedTempPath);
            var ffmpeg = new FfmpegEditor(ffmpegBinFolder, ffmpegTempFolder);
            
            var outDirectory = new DirectoryInfo(_outPath);
            
            _videoEditor = new VideoEditor.VideoEditor(_video, ffmpeg, ffmpeg, ffmpeg, 
                outDirectory, _video.Name, _video.ExtensionAsEnum());
            return _videoEditor;
        }
    }
    private VideoEditor.VideoEditor? _videoEditor;
    
    private void SetDetailsControl(OperationDetailsControl detailsControl)
    {
        ActiveEditingDetailsControl = detailsControl;
    }

    [RelayCommand]
    private void PerformOperation()
    {
        ActiveEditingDetailsControl.OnPerformOperation();
        
        OperationDetailsViewModel userInput = ActiveEditingDetailsControl.ViewModel;
        BooleanResponse response = DelegateOperation(VideoEditor, userInput);
        if (response.Success)
        {
            VideoEditor.CopyEditedToOriginal();
        }

        Debug.WriteLine($"{(response.Success ? "Success" : "Fail")} - {response.ResponseMsg}");
    }

    [SuppressMessage("ReSharper", "ConvertTypeCheckPatternToNullCheck")]
    private BooleanResponse DelegateOperation(VideoEditor.VideoEditor editor, 
        OperationDetailsViewModel userInput)
    {
        BooleanResponse res;
        EditOperation operationType = ActiveEditingDetailsControl.OperationType;
        switch (operationType)
        {
            case EditOperation.Trim:
                res = editor.Trim(userInput.StartTime, userInput.EndTime ?? editor.Footage.Duration);
                break;
            case EditOperation.Capture:
                if (userInput.Width is null && userInput.Height is null)
                {
                    res = editor.CaptureFullImage(userInput.StartTime);
                }
                else
                {
                    Size widthHeight = new Size(userInput.Width ?? -1, userInput.Height ?? -1); 
                    res = editor.CaptureCroppedImage(widthHeight, userInput.StartTime);
                }
                break;
            case EditOperation.Merge:
                if (userInput.VideoPath is null)
                {
                    res = new BooleanResponse(false, "VideoPath is null");
                }
                else
                {
                    res = editor.MergeWith([userInput.VideoPath.FullName]);
                }
                break;
            case EditOperation.Mute:
                res = editor.Mute(); 
                break;
            case EditOperation.Convert:
                bool parsed = Enum.TryParse(userInput.NewExtension, true, out IEditor.Extension newExt);
                if (!parsed)
                {
                    res = new BooleanResponse(false, $"Invalid extension {userInput.NewExtension}");
                }
                else
                {
                    res = editor.Convert(newExt);
                }
                break;
            case EditOperation.Gif:
                TimeSpan endTime = userInput.EndTime ?? editor.Footage.Duration;
                TimeSpan duration = userInput.Duration ?? TimeSpan.Zero;
                Size size = new Size(userInput.Width ?? -1, userInput.Height ?? -1);
                res = editor.CaptureGif(userInput.StartTime, endTime, duration, size);
                break;
            case EditOperation.Compress:
                if (userInput.CompressionLevel is null && userInput.SizeInMb is null)
                {
                    const string msg = "Either compression level or desired size should be defined.";
                    res = new BooleanResponse(false, msg);
                }
                else if (userInput.CompressionLevel is float compLevel)
                {
                    res = editor.CompressBy(compLevel);
                } 
                else if (userInput.SizeInMb is int sizeInMb)
                {
                    res = editor.CompressDownTo(sizeInMb * 1024);
                }
                else
                {
                    res = new BooleanResponse(false, "Unexpected error.");
                }
                break;
            default:
                res = new BooleanResponse(false, $"Unsupported Operation {operationType}!");
                break;
        }
        return res;
    }
    
    #endregion
    
}