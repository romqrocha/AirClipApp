using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AirClipApp.ViewModels;

/// <summary>
/// View Model for ImportPage
/// </summary>
/// <resources>
/// Followed <see href="https://docs.avaloniaui.net/docs/get-started/test-drive/">
/// Avalonia's Documentation </see> while creating this.
/// </resources>
/// <authors> Rodrigo Rocha, Tae Seo </authors>
public class ImportPageViewModel : ObservableObject
{
    public string ImportPrompt => "Import a video to get started!";
    
    public string ImportStatus { get; set; } = string.Empty;
    
    // A filter that specifies the video formats that are allowed when selecting the video file.
    private static FilePickerFileType VideoSupported { get; } = new("Supported Videos") // Custom file type filter
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
    public async Task<string> ImportFromFileSystem(TopLevel topLevel)
    {
        // Read-only list of IStorageFile objects
        // IStorageFile is an interface that represents a file in the storage system
        IReadOnlyList<IStorageFile> files = 
            await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open Text File",
            FileTypeFilter = [VideoSupported, FilePickerFileTypes.ImageAll],
            AllowMultiple = false
        });

        if (files.Count < 1) 
            return string.Empty;
        
        string videoPath = files[0].Path.ToString();
        return videoPath;
    }
}