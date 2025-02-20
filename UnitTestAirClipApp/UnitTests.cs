using AirClipApp;
using FFMpegCore;
[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace UnitTestAirClipApp;

[Collection("Important Tests")]
public class UnitTests
{
    private const string AbsFfmpegPath = 
        @"C:\ProgramData\chocolatey\lib\ffmpeg\tools\ffmpeg\bin\";
    private const string AbsTmpFilePath = 
        @"C:\Users\16046\RiderProjects\AirClipApp\UnitTestAirClipApp\Temp";
    private const string AbsInputPath = 
        @"C:\Users\16046\RiderProjects\AirClipApp\UnitTestAirClipApp\#.MP4";
    private const string AbsOutputPath =
        @"C:\Users\16046\RiderProjects\AirClipApp\UnitTestAirClipApp\#_modified.MP4";

    private readonly Video _video;
    private readonly Video _video2;
    private readonly Video _video3;
    private readonly Video _video4;
    
    public UnitTests()
    {
        GlobalFFOptions.Configure(new FFOptions
        {
            BinaryFolder = AbsFfmpegPath,
            TemporaryFilesFolder = AbsTmpFilePath,
        });
        var outPath1 = AbsOutputPath.Replace("#", "1");
        var outPath2 = AbsOutputPath.Replace("#", "2");
        var outPath3 = AbsOutputPath.Replace("#", "3");
        var outPath4 = AbsOutputPath.Replace("#", "4");
        
        _video = new Video(AbsInputPath.Replace("#", "1"), outPath1);
        _video2 = new Video(AbsInputPath.Replace("#", "2"), outPath2);
        _video3 = new Video(AbsInputPath.Replace("#", "3"), outPath3);
        _video4 = new Video(AbsInputPath.Replace("#", "4"), outPath4);
        
    }
    
    [Fact]
    public void TestImportVideo()
    {
        var info = _video.GetInfo();
        Assert.Contains("mp4", info.Format.FormatName);
    }

    [Fact]
    public void TestSnapshot()
    {
        _video.Snapshot(_video.Height, _video.Width);
        Assert.True(File.Exists(_video.OutputPath));
        Assert.True(File.Exists(_video.OutputPath.Replace(".mp4", ".png")));
    }

    [Fact]
    public void TestTrimVideo()
    {
        _video.Trim(0, 1);
        var newVideo = new Video(_video.OutputPath, _video.InputPath);
        Assert.Equal(TimeSpan.FromSeconds(1).Seconds, newVideo.GetInfo().Duration.Seconds);
    }
    
    [Fact]
    public void TestMergeVideos()
    {
        var inputDuration = _video2.GetInfo().Duration;
        _video2.Join(_video2.InputPath);
        var newVideo = new Video(_video2.OutputPath, _video2.InputPath).GetInfo().Duration.Seconds; 
        Assert.Equal(newVideo, 
            inputDuration.Add(inputDuration).Seconds);
    }

    
    [Fact]
    public void TestMuteVideo()
    {
        _video.Mute();
        Assert.True(File.Exists(_video.OutputPath));
    }

    [Fact]
    public void TestExportVideo()
    {
        _video.ExportWeb(new FileStream(_video3.InputPath, FileMode.Open), 
            new FileStream(_video3.OutputPath.Replace(".mp4", ".webm"), FileMode.Create));
        Assert.True(File.Exists(_video3.OutputPath));
    }

    
    [Fact]
    public void TestCompressVideo()
    {
        var oldFileInfo = new FileInfo(_video4.InputPath);
        var newFileInfo = new FileInfo(_video4.OutputPath);
        _video.Compress();
        newFileInfo.Refresh();
        Assert.True(oldFileInfo.OpenRead().Length > newFileInfo.OpenRead().Length);
    }

    [Fact]
    public void TestCropVideo()
    {
        _video.Snapshot(100, 100);
        Assert.True(File.Exists(_video.OutputPath));
    }
}