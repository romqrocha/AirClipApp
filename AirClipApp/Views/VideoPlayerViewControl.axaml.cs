using System;
using AirClipApp.ViewModels;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using LibVLCSharp.Avalonia.Unofficial;
using LibVLCSharp.Shared;

namespace AirClipApp.Views
{
    public partial class VideoPlayerViewControl : UserControl
    {
        public readonly VideoPlayerViewControlModel ViewModel = new VideoPlayerViewControlModel();
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

            DataContext = ViewModel;

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
            
            _videoViewer.MediaPlayer = ViewModel.MediaPlayer;
            _videoViewer.MediaPlayer.Hwnd = _videoViewer.Handle.Handle;
            //string mrl = new Uri(@"C:\Users\16046\Videos\Mockumentary\2.MP4").AbsoluteUri;
            ViewModel.StartPlay("2.MP4");

            ViewModel.IsStopped = true;
        }

        private void Button_OnClick(object? sender, RoutedEventArgs e)
        {
            SetPlayerHandle();
        }
    }
}