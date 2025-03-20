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
    
    public static FilePickerFileType VideoSupported { get; } = new("Supported Videos")
    {
        Patterns = ["*.mp4", "*.mov", "*.webm"],
        AppleUniformTypeIdentifiers = ["public.video"], // might be wrong
        MimeTypes = ["video/*"] // might be wrong
    };

    /// <summary>
    /// Import a video file from the local file system.
    /// </summary>
    /// <param name="topLevel">Top level of the current control.
    /// Alternatively, we can switch to a Window reference later.</param>
    public async Task<string> ImportFromFileSystem(TopLevel topLevel)
    {
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