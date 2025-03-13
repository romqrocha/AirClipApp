using System.Diagnostics;
using AirClipApp.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace AirClipApp.Views;

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