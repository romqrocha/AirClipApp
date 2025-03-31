using System.Drawing;

namespace VideoEditor;

/// <summary>
/// Provides full video editing functionality. Aggregated with interfaces rather than FfmpegEditor for clarity
/// and organization.
/// </summary>
/// <resources> No external resources were used for this class. </resources>
/// <authors> Rodrigo Rocha, Tae Seo </authors>
public class VideoEditor
{
    private Video _footage;
    private readonly IEditor _editor;
    private readonly IGifCreator _gifCreator;
    private readonly ICompressor _compressor;
    private DirectoryInfo? _outputDirectory = null;
    private string? _outputFileName = null;
    private string? _outputExtension = null;
    
    /** Absolute path to the imported video file. */
    private string InputPath => _footage.Path;
    /** Absolute path to the final output file. */
    private string OutputPath => Path.Join(_outputDirectory?.FullName, _outputFileName + _outputExtension);

    /// <summary>
    /// Standard constructor for VideoEditor.
    /// </summary>
    /// <param name="video">The video to be edited.</param>
    /// <param name="editor">Object implementing IEditor to provide basic video editing
    /// functionality.</param>
    /// <param name="gifCreator">Object implementing IGifCreator to provide gif creation
    /// functionality.</param>
    /// <param name="compressor">Object implementing ICompressor to provide video compression
    /// functionality.</param>
    /// <param name="outputDirectory">The directory where final output files should be
    /// saved.</param>
    /// <param name="outputFileName">The name of the final output file (no extension)</param>
    /// <param name="outputExtension">The extension of the final output file ('.ext')</param>
    public VideoEditor(Video video, IEditor editor, IGifCreator gifCreator, ICompressor compressor)
    {
        _footage = video;
        
        _editor = editor;
        _gifCreator = gifCreator;
        _compressor = compressor;
        
        // _outputDirectory = outputDirectory;
        // _outputFileName = outputFileName;
        // _outputExtension = outputExtension;
    }

    /// <summary>
    /// Captures a full-sized image of the imported video at the specified time.
    /// </summary>
    /// <param name="captureTime">The time of the frame you want to capture.</param>
    public void CaptureFullImage(TimeSpan captureTime)
    {
        _editor.CaptureImage(InputPath, OutputPath, new Size(-1, -1), captureTime);
    }

    /// <summary>
    /// Captures and crops an image of the imported video at the specified time.
    /// </summary>
    /// <param name="widthHeight">The width and height of the image.
    /// -1 in width or height will resize the image instead of cropping.</param>
    /// <param name="captureTime">The time of the frame you want to capture.</param>
    public void CaptureCroppedImage(Size widthHeight, TimeSpan captureTime)
    {
        _editor.CaptureImage(InputPath, OutputPath, widthHeight, captureTime);
    }

    /// <summary>
    /// Merges the imported video with one or more additional videos. The imported video
    /// will go first.
    /// </summary>
    /// <param name="otherInputs">Absolute paths to the additional videos.</param>
    public void MergeWith(string[] otherInputs)
    {
        string[] allInputs = new string[otherInputs.Length + 1];
        allInputs[0] = InputPath;
        for (int i = 0; i < otherInputs.Length; i++)
        {
            allInputs[i + 1] = otherInputs[i];
        }
        _editor.Join(allInputs, OutputPath);
    }

    /// <summary>
    /// Trims the imported video from startTime to endTime
    /// </summary>
    /// <param name="startTime">When the trimmed video will start.</param>
    /// <param name="endTime">When the trimmed video will the end.</param>
    public void Trim(TimeSpan startTime, TimeSpan endTime)
    {
        _editor.Trim(InputPath, OutputPath, startTime, endTime);
    }

    /// <summary>
    /// Mute the imported video.
    /// Note: There is no unmute operation yet.
    /// </summary>
    public void Mute()
    {
        _editor.Mute(InputPath, OutputPath);
    }

    /// <summary>
    /// Converts the imported video to a new video type.
    /// Only some types are supported.
    /// </summary>
    /// <param name="newExtension">The extension of the output video type.</param>
    public void Convert(IEditor.Extension newExtension)
    {
        // TODO: output path should have new extension instead of old extension
        _editor.Convert(InputPath, OutputPath, newExtension);
    }

    /// <summary>
    /// Creates a gif from the imported video.
    /// </summary>
    public void ConvertToGif()
    {
        _gifCreator.CaptureGif(InputPath, OutputPath, new Size(-1, -1), 
            TimeSpan.Zero, _footage.Duration, TimeSpan.Zero);
    }

    /// <summary>
    /// Creates a gif from the imported video.
    /// </summary>
    /// <param name="widthHeight"> The width and height of the image.
    /// -1 in width or height tells ffmpeg resize while keeping aspect ratio.</param>
    /// <param name="startTime">When in the input video the gif will start.</param>
    /// <param name="endTime">When in the input video the gif will end.</param>
    /// <param name="duration">The duration of the gif (will be sped up or slowed down to fit).
    /// Use TimeSpan.Zero for the default duration (without changes in speed).</param>
    public void CaptureGif(TimeSpan startTime, TimeSpan endTime, TimeSpan duration, Size? widthHeight = null)
    {
        widthHeight ??= new Size(-1, -1);
        _gifCreator.CaptureGif(InputPath, OutputPath, (Size)widthHeight, startTime, endTime, duration);
    }

    /// <summary>
    /// Compresses the input video down to the given size in KB.
    /// </summary>
    /// /// <param name="maxKilobytes">The maximum size of the output video in KB.</param>
    public void CompressDownTo(long maxKilobytes)
    {
        _compressor.Compress(InputPath, OutputPath, maxKilobytes);
    }

    /// <summary>
    /// Compresses the input video by a percentage equal to the given compression factor
    /// (1.0 = 100%)
    /// </summary>
    /// <param name="compressionFactor">Smaller compression factors tell FFMpeg to compress
    /// the video further. This value must be greater than 0 and less than 1.</param>
    public void CompressBy(float compressionFactor)
    {
        // 0 < compressionFactor < 1
        compressionFactor = Math.Max(compressionFactor, float.Epsilon);
        compressionFactor = Math.Min(compressionFactor, 1.0f - float.Epsilon);
        
        _compressor.Compress(InputPath, OutputPath, compressionFactor);
    }
}