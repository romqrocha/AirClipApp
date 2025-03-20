using FFMpegCore;

namespace VideoEditor;

/// <summary>
/// Represents a video file.
/// </summary>
/// <remarks>
/// Relies on an FFProbe process to gather information about the video file being
/// represented.
/// </remarks>
/// <resources> No external resources were used for this class. </resources>
/// <authors> Rodrigo Rocha, Tae Seo </authors>
public class Video
{
    /** General info about this video's file. */
    private FileInfo VideoFile { get; set; }
    
    /** Absolute path to this video's file. */
    public string Path { get; }
    
    /** This video's file extension. */
    public string Extension { get; }
    
    /** Detailed info about this video's properties. */
    private IMediaAnalysis MediaInfo { get; set; }
    
    /** TODO: Video width in pixels (?). */
    public int Width { get; }
    
    /** TODO: Video height in pixels (?). */
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
        VideoFile = new FileInfo(inputPath);

        Path = VideoFile.FullName;
        Extension = VideoFile.Extension;
            
        MediaInfo = FFProbe.Analyse(Path);
        if (MediaInfo.PrimaryVideoStream is null)
            throw new NullReferenceException($"Primary video stream for {Path} not found.");
        
        VideoStream videoStream = MediaInfo.PrimaryVideoStream;
        Width = videoStream.Width;
        Height = videoStream.Height;
        Fps = videoStream.FrameRate;
        Duration = videoStream.Duration;
    }
    
    
}