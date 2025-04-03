using CommunityToolkit.Mvvm.ComponentModel;
using Avalonia.Media.Imaging;
using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AirClipApp.Views;

namespace AirClipApp.ViewModels;

/// <summary>
/// View Model for VideoPlayerViewControl
/// </summary>
/// <resources>
/// Followed the example at <see href="https://github.com/radiolondra/YAMP2">
/// </see> to create this.
/// </resources>
/// <authors> Rodrigo Rocha, Taeyang Seo </authors>
public partial class VideoPlayerViewControlViewModel : ObservableObject
{
    private static HttpClient httpClient = new();
    private readonly LibVLC _libVLC;
    public MediaPlayer MediaPlayer { get; }
    
    public VideoPlayerViewControlViewModel()
    {
        const bool isDebugging = false;

        if (Avalonia.Controls.Design.IsDesignMode) 
            return;
        
        var libVlcDirectoryPath = Path.Combine(Environment.CurrentDirectory, 
            "libvlc", "win-x64");

        Core.Initialize(libVlcDirectoryPath);
        _libVLC = new LibVLC(enableDebugLogs: isDebugging, "--avcodec-hw=any");

        if (isDebugging)
            _libVLC.Log += VlcLogger_Event;

        //MediaPlayer = new MediaPlayer(_libVLC) { EnableHardwareDecoding = true };
        MediaPlayer = new MediaPlayer(_libVLC)
        {
            Fullscreen = true,
            //EnableMouseInput = false,
            //Scale = 1
        };

        MediaPlayer.TimeChanged += MediaPlayer_TimeChanged;
        MediaPlayer.Playing += MediaPlayer_Playing;
        MediaPlayer.EndReached += MediaPlayer_EndReached;
        MediaPlayer.Vout += MediaPlayer_VideoOut;

        IsStopped = false;
    }

    /// <summary>
    /// VLClib logger event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="l"></param>
    private void VlcLogger_Event(object? sender, LogEventArgs l)
    {
        Debug.WriteLine(l.Message);
    }

    /// <summary>
    /// Event fired when media changes and when play starts after stop
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MediaPlayer_VideoOut(object? sender, MediaPlayerVoutEventArgs e)
    {
        IsStopped = false; // Needed to show player over still image

        MediaPlayer.Volume = 50;
        //Debug.WriteLine($"*** VOUT *** => XVolume={VideoPlayerView.ControlsView.viewModel.XVolume} MPvolume={MediaPlayer.Volume}");
    }

    /// <summary>
    /// Event fired when MediaPlayer reaches the end of media
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MediaPlayer_EndReached(object? sender, EventArgs e)
    {
        MediaPlayer.Stop();
    }

    /// <summary>
    /// Event fired once when MediaPlayer starts playing media
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MediaPlayer_Playing(object? sender, EventArgs e)
    {
        try
        {
            MediaPlayer.Scale = 0;

            //VideoDuration = MediaPlayer.Length / 1000;
        }
        catch { }
    }

    /// <summary>
    /// Event fired at every timechange while playing
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MediaPlayer_TimeChanged(object? sender, MediaPlayerTimeChangedEventArgs e)
    {
        try
        {
            MediaPlayer.Volume = 50;
            //VideoPlayerViewControl.ControlsView.viewModel.XTime = MediaPlayer.Time / 1000.0;
        }
        catch
        {
            //VideoPlayerViewControl.ControlsView.viewModel.XTime = 0.0;
        }
    }

    #region PROPERTIES
    
    [ObservableProperty] private Bitmap? _cover;

    [ObservableProperty] private bool _isStopped;

    #endregion



    public void Dispose()
    {
        try
        {
            MediaPlayer.Dispose();
            _libVLC.Dispose();
        }
        catch { }
    }

    public void StartPlay(string path)
    {
        //Thread thread = new Thread(() => LoadCover(coverUrl));
        //thread.Start();

        try
        {
            //var currentDirectory = Settings.ApplicationFolder();
            //var destination = Path.Combine(currentDirectory, "record.ts");
            /*
            using var media = new Media(_libVLC, new Uri(ephemeralUrl)
                //,
                //":sout=#file{dst=" + destination + "}",
                //":sout-keep"                    
                , "--avcodec-hw=any"
                );
            */
            using var media = new Media(_libVLC, path);

            // VideoPlayerViewControl.ControlsView.viewModel.IsPlaying = true;

            MediaPlayer.Volume = 50;

            Equalizer eq = new Equalizer();
            Debug.WriteLine($"EQUALIZER BANDS:{eq.BandCount}");

            MediaPlayer.SetEqualizer(eq);
            eq.Dispose();

            MediaPlayer.Play(media);
            media.Dispose();


        }
        catch { }


    }        

    private async void LoadCover(string coverUrl)
    {
        if (!String.IsNullOrWhiteSpace(coverUrl))
        {
            using (var imageStream = await LoadCoverBitmapAsync(coverUrl))
            {
                Cover = Bitmap.DecodeToWidth(imageStream, 400);
            }
        }
    }

    
    private async Task<Stream> LoadCoverBitmapAsync(string coverUrl)
    {
        byte[] data;
        try
        {
            data = await httpClient.GetByteArrayAsync(coverUrl);
        }
        catch (Exception ex)
        {
           // string assemblyPath = Utilities.ApplicationFolder();
           //  data = ImageToByteArray(Path.Combine(assemblyPath, Settings.WeTubeImageNotAvailable));
           data = await httpClient.GetByteArrayAsync(coverUrl);
        }

        return new MemoryStream(data);
    }
    
    private static byte[] ImageToByteArray(string imageName)
    {
        //Initialize a file stream to read the image file
        FileStream fs = new FileStream(imageName, FileMode.Open, FileAccess.Read);

        //Initialize a byte array with size of stream
        byte[] imgByteArr = new byte[fs.Length];

        //Read data from the file stream and put into the byte array
        fs.Read(imgByteArr, 0, Convert.ToInt32(fs.Length));

        //Close a file stream
        fs.Close();

        return imgByteArr;
    }

    public void PlayerIsExiting()
    {
        //VideoPlayerViewControl.ControlsView.viewModel.Stop();
        //Dispose();
    }
}