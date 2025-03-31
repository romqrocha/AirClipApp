using System.IO;
using AirClipApp.ViewModels;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace AirClipApp.Views;

/// <summary>
/// The page where the user imports the video that they want to edit.
/// </summary>
/// <resources>
/// Followed <see href="https://docs.avaloniaui.net/docs/get-started/test-drive/">
/// Avalonia's Documentation </see> while creating this.
/// </resources>
/// <authors> Rodrigo Rocha, Taeyang Seo </authors>
public partial class ImportPage : UserControl
{
    private ImportPageViewModel ViewModel { get; } 
    
    public ImportPage()
    {
        InitializeComponent();
        ViewModel = new ImportPageViewModel();
        DataContext = ViewModel;
    }

    // Get rid of async keyword?
    public async void OnImport(object? sender, RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel is null)
            return;
        
        string videoPath = await ViewModel.ImportFromFileSystem(topLevel);
        
        // Removing the first 7 characters of the path because they make the path invalid.
        // Must make sure the video path string is not empty before calling Substring, otherwise
        // it will crash the program.
        if (!string.IsNullOrEmpty(videoPath))
        {
            videoPath = videoPath.Substring(7);    
        }
        ImportStatus.Text = $"Importing '{videoPath}' ...";
        
        // TODO: validate video path
        // TODO: initialize video editor
        if (!IsVideoPathValid(videoPath))
        {
            ImportStatus.Text = $"The video file is not valid. Try again.";
            return;
        }
        
        if (Parent is ContentControl parent)
        {
            parent.Content = new EditorPage();
        }
    }

    // Method to validate video path. Might want to move this to ImportPageViewModel
    private bool IsVideoPathValid(string videoPath)
    {
        return File.Exists(videoPath);
    }
}