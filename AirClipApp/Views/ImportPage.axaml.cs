using AirClipApp.ViewModels;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace AirClipApp.Views;

/// <summary>
/// The page where we the user imports the video that they want to edit.
/// </summary>
/// <resources>
/// Followed <see href="https://docs.avaloniaui.net/docs/get-started/test-drive/">
/// Avalonia's Documentation </see> while creating this.
/// </resources>
/// <authors> Rodrigo Rocha, Tae Seo </authors>
public partial class ImportPage : UserControl
{
    private ImportPageViewModel ViewModel { get; } 
    
    public ImportPage()
    {
        InitializeComponent();
        ViewModel = new ImportPageViewModel();
        DataContext = ViewModel;
    }

    public async void OnImport(object? sender, RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel is null)
            return;
        
        string videoPath = await ViewModel.ImportFromFileSystem(topLevel);
        ImportStatus.Text = $"Importing '{videoPath}' ...";
        
        // TODO: validate video path
        // TODO: initialize video editor
        
        if (Parent is ContentControl parent)
        {
            parent.Content = new EditorPage();
        }
    }
}