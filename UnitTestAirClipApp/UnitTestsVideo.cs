using System.Diagnostics.CodeAnalysis;
using Moq;
using VideoEditor;

namespace UnitTestAirClipApp;

[Collection("Important Tests")]
[SuppressMessage("ReSharper", "ConvertToLocalFunction")]
public class UnitTestsVideo
{
    private const string TempFilePath = "temp";
    
    /// <summary>
    /// Tries to initialize a new video with invalid paths.
    /// </summary>
    /// <param name="path">Invalid paths.</param>
    [Theory]
    [InlineData("")]
    [InlineData("blah")]
    [InlineData("blah.Mp4")]
    public void NewVideo_InvalidPath(string path)
    {
        Action act = () => _ = new Video(path);
        Assert.ThrowsAny<IOException>(act);
    }

    /// <summary>
    /// Tries to initialize a new video with an invalid extension.
    /// </summary>
    [Fact]
    public void NewVideo_InvalidExtension()
    {
        File.WriteAllBytes(TempFilePath, Array.Empty<byte>());
        
        Action act = () => _ = new Video(TempFilePath);
        Assert.ThrowsAny<IOException>(act);
        
        File.Delete(TempFilePath);
    }
    
    /// <summary>
    /// Tries to initialize a new video with a corrupted or no video stream.
    /// </summary>
    [Fact]
    public void NewVideo_NullVideoStream()
    {
        const string mp4Path = $"{TempFilePath}.Mp4";
        File.WriteAllBytes(mp4Path, Array.Empty<byte>());
        
        Action act = () => _ = new Video(mp4Path);
        Assert.Throws<NullReferenceException>(act);
        
        File.Delete(mp4Path);
    }
}