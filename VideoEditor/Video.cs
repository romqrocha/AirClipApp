using System.Globalization;
using FFMpegCore;
using FFMpegCore.Exceptions;

namespace VideoEditor;

/// <summary>
/// Represents a video file.
/// </summary>
/// <remarks>
/// Relies on an FFProbe process to gather information about the video file being represented.
/// </remarks>
/// <resources> No external resources were used for this class. </resources>
/// <authors> Rodrigo Rocha, Taeyang Seo </authors>
public class Video
{
    /** General info about this video's file. */
    private FileInfo VideoFile { get; set; }

    /** Absolute path to this video's file. */
    public string AbsPath => VideoFile.FullName.Replace(
        Extension, Extension.ToLower(), true, CultureInfo.CurrentCulture);
    
    /** This video file's extension, including the leading dot. */
    private string Extension => VideoFile.Extension;

    /** This video file's name. */
    public string Name => Path.GetFileNameWithoutExtension(VideoFile.Name);
    
    /** Detailed info about this video's properties (use FFprobe for this) */
    private IMediaAnalysis? MediaInfo { get; set; }

    /** Use FFProbe.Analyse(filePath) for this */
    public int Width { get; }

    /** Use FFProbe.Analyse(filePath) for this */
    public int Height { get; }
    
    /** Video framerate in frames per second. */
    public double Fps { get; }
    
    /** The duration of this video. */
    public TimeSpan Duration { get; }
    
    /// <summary>
    /// Initializes a video from its file path.
    /// </summary>
    /// <param name="inputPath">Path to the video file.</param>
    /// <exception cref="NullReferenceException">If the file is corrupted or not a valid video.</exception>
    public Video(string inputPath)
    {
        try
        {
            VideoFile = new FileInfo(inputPath);
        }
        catch (ArgumentException)
        {
            throw new IOException("Video path is empty.");
        }
        
        if (!VideoFile.Exists)
            throw new FileNotFoundException($"File {VideoFile.FullName} does not exist.");

        if (!Enum.TryParse<IEditor.Extension>(VideoFile.Extension.Trim('.'), true, out _))
            throw new IOException($"Unsupported extension {VideoFile.Extension}");

        try
        {
            MediaInfo = FFProbe.Analyse(AbsPath);
        }
        catch (FFMpegException ex)
        {
            throw new NullReferenceException(ex.Message);
        }
        if (MediaInfo.PrimaryVideoStream is null)
            throw new NullReferenceException($"Primary video stream for {AbsPath} not found.");
        
        VideoStream videoStream = MediaInfo.PrimaryVideoStream;
        Width = videoStream.Width;
        Height = videoStream.Height;
        Fps = videoStream.FrameRate;
        Duration = videoStream.Duration;
    }
    
    /// <returns>
    /// This file's extension as an Extension enum.
    /// </returns>
    public IEditor.Extension ExtensionAsEnum()
    {
        Enum.TryParse(VideoFile.Extension.Trim('.'), true, out IEditor.Extension ext);
        return ext;
    }

    /// <summary>
    /// Mock constructor for unit testing.
    /// </summary>
    /// <remarks>
    /// Don't use this outside of testing purposes.
    /// </remarks>
    // ReSharper disable once UnusedMember.Global
    public Video()
    {
        VideoFile = new FileInfo("C:");
    }
}