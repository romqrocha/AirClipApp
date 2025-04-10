using System.Drawing;
using Moq;
using VideoEditor;

namespace UnitTestAirClipApp;

/// <summary>
/// Tests the VideoEditor class.
/// </summary>
/// <resources>
/// The <see href="https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices">
/// Microsoft .NET guidelines </see> were consulted in writing these unit tests.
/// Also see <see href="https://www.codemag.com/Article/2305041/Using-Moq-A-Simple-Guide-to-Mocking-for-.NET">
/// this article</see> for the guide I used to get started with Moq. 
/// </resources>
/// <authors> Rodrigo Rocha, Taeyang Seo </authors>
[Collection("Important Tests")]
public class UnitTestsVideoEditor
{
    /* FOR REFERENCE:
    The following path is the default path for a ffmpeg installation from chocolatey:
        @"C:\ProgramData\chocolatey\lib\ffmpeg\tools\ffmpeg\bin\";
    */
    
    /// <returns>
    /// A mock video editor object.
    /// </returns>
    private static VideoEditor.VideoEditor GetMockEditor()
    {
        var mockVideo = new Mock<Video>();
        var mockEditor = new Mock<IEditor>();
        var mockGifCreator = new Mock<IGifCreator>();
        var mockCompressor = new Mock<ICompressor>();
        var mockOutputDirectory = new Mock<DirectoryInfo>();
        
        VideoEditor.VideoEditor editor = new(mockVideo.Object, mockEditor.Object, 
            mockGifCreator.Object, mockCompressor.Object, mockOutputDirectory.Object, 
            "", IEditor.Extension.Mp4);
        
        return editor;
    }

    /// <summary>
    /// Tries to capture an image with a time span that is out of bounds.
    /// </summary>
    /// <param name="time">An out of bounds time span.</param>
    [Theory]
    [InlineData(-1)]
    public void CaptureFullImage_TimeOutOfBounds_Fails(int time)
    {
        var span = new TimeSpan(time);
        var editor = GetMockEditor();
        
        var result = editor.CaptureFullImage(span);
        
        Assert.False(result.Success);
    }
    
    /// <summary>
    /// Tries to capture an image with a valid time span.
    /// </summary>
    /// <param name="time">A valid time span.</param>
    [Theory]
    [InlineData(0)]
    public void CaptureFullImage_TimeInBounds_Succeeds(int time)
    {
        var span = new TimeSpan(time);
        var editor = GetMockEditor();
        
        var result = editor.CaptureFullImage(span);
        
        Assert.True(result.Success);
    }
    
    /// <summary>
    /// Tries to capture a cropped image with a valid time span and size.
    /// </summary>
    /// <param name="time">A valid time span.</param>
    [Theory]
    [InlineData(0)]
    public void CaptureCroppedImage_InBounds_Succeeds(int time)
    {
        var span = new TimeSpan(time);
        var editor = GetMockEditor();
        
        var result = editor.CaptureCroppedImage(new Size(1, 1), span);
        
        Assert.True(result.Success);
    }

    /// <summary>
    /// Tries to merge with this video but does not provide other paths.
    /// </summary>
    [Fact]
    public void MergeWith_MissingVideos()
    {
        var editor = GetMockEditor();
        
        var result = editor.MergeWith([]);
        
        Assert.False(result.Success);
    }

    /// <summary>
    /// Tries to trim a video but the end time is before the start time.
    /// </summary>
    [Fact]
    public void Trim_EndBeforeStart()
    {
        var editor = GetMockEditor();

        var result = editor.Trim(new TimeSpan(1), new TimeSpan(0));
        
        Assert.False(result.Success);
    }

    /// <summary>
    /// Ensures that the simple mute method is working properly.
    /// (I don't even know how this can break)
    /// </summary>
    [Fact]
    public void Mute_Succeeds()
    {
        var editor = GetMockEditor();

        var result = editor.Mute();
        
        Assert.True(result.Success);
    }

    /// <summary>
    /// Tries to convert the video to a different format.
    /// </summary>
    [Fact]
    public void Convert_Succeeds()
    {
        var editor = GetMockEditor();

        var result = editor.Convert(IEditor.Extension.Mov);
        
        Assert.True(result.Success);
    }

    /// <summary>
    /// Tries to convert the video to a gif.
    /// </summary>
    [Fact]
    public void ConvertToGif_Succeeds()
    {
        var editor = GetMockEditor();

        var result = editor.ConvertToGif();
        
        Assert.True(result.Success);
    }

    /// <summary>
    /// Tries to capture a gif with an invalid start time.
    /// </summary>
    [Fact]
    public void CaptureGif_StartEqualsEnd_Fails()
    {
        var editor = GetMockEditor();
        var time = new TimeSpan(1);
        
        var result = editor.CaptureGif(time, time, new TimeSpan(0));
        
        Assert.False(result.Success);
    }

    /// <summary>
    /// Tries to compress a video but the size is too small.
    /// </summary>
    [Fact]
    public void CompressDownTo_TooSmall_Fails()
    {
        var editor = GetMockEditor();

        var result = editor.CompressDownTo(0);
        
        Assert.False(result.Success);
    }

    /// <summary>
    /// Tries to compress a video to a size too small but still succeeds.
    /// </summary>
    [Fact]
    public void CompressBy_TooSmall_Succeeds()
    {
        var editor = GetMockEditor();

        var result = editor.CompressBy(0);
        
        Assert.True(result.Success);
    }
    
    /// <summary>
    /// Tries to compress a video to a size too big but still succeeds.
    /// </summary>
    [Fact]
    public void CompressBy_TooBig_Succeeds()
    {
        var editor = GetMockEditor();

        var result = editor.CompressBy(1);
        
        Assert.True(result.Success);
    }
}

