using CommunityToolkit.Mvvm.ComponentModel;
using VideoEditor;

namespace AirClipApp.ViewModels;


/// <summary>
/// View Model for the app's main window. I think that this is the default view model for
/// any user control that's a child of the main window.
/// </summary>
/// <authors> Rodrigo Rocha, Taeyang Seo </authors>
public class MainWindowViewModel : ObservableObject
{
    public static FfmpegEditor? FfmpegEditor { get; set; }

    public static Video? Video { get; set; }

    public static VideoEditor.VideoEditor? VideoEditor { get; set; }
}