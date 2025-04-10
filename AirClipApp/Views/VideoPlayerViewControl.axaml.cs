using AirClipApp.ViewModels;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using LibVLCSharp.Avalonia.Unofficial;


namespace AirClipApp.Views;

/// <summary>
/// User Control that encapsulates our custom VideoView control.
/// </summary>
/// <resources>
/// Followed the example at <see href="https://github.com/radiolondra/YAMP2">
/// </see> to create this.
/// </resources>
/// <authors> Rodrigo Rocha, Taeyang Seo </authors>
public partial class VideoPlayerViewControl : UserControl
{
    public readonly VideoPlayerViewControlViewModel ViewViewModel = new VideoPlayerViewControlViewModel();
    private static VideoPlayerViewControl? _this;

    //public Panel mpContainer;

    public VideoView? _videoViewer;


    public string? videoUrl { get; set; }
    public string? coverUrl { get; set; }
    public string? videoDuration { get; set; }
    public string? videoTitle { get; set; }
    public int videoWidth { get; set; }
    public int videoHeight { get; set; }

    public string videoAspectRatio { get; set; }
    public VideoPlayerViewControl()
    {
        InitializeComponent();

        _this = this;

        DataContext = ViewViewModel;

        _videoViewer = this.Get<VideoView>("VideoViewer");
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public static VideoPlayerViewControl GetInstance()
    {
        return _this;
    }

    public void SetPlayerHandle()
    {
        
        if (_videoViewer == null) 
            return;
            
        _videoViewer.MediaPlayer = ViewViewModel.MediaPlayer;
        _videoViewer.MediaPlayer.Hwnd = _videoViewer.Handle.Handle;

        string path = VideoEditor.VideoEditor.CurrentVideoPath ?? "";
        ViewViewModel.StartPlay(path == "" ? "" : @"TempVideos\" + path);
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        SetPlayerHandle();
    }
}