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
    private readonly IEditor _editor;
    private readonly IGifCreator _gifCreator;
    private readonly ICompressor _compressor;
    
    private readonly DirectoryInfo _outputDirectory;
    private readonly string _outputFileName;

    /** Absolute path to the imported video file. */
    private string InputPath => Footage.AbsPath;

    /** Absolute path to the final output file. */
    private string OutputPath => Path.Join(_outputDirectory.FullName, 
        $"{_outputFileName + EditedKey}{IEditor.ExtToString(OutputExtension)}");
    
    public Video Footage { get; private set; }

    public IEditor.Extension OutputExtension { get; private set; }

    private const string OriginalKey = "og0408";
    private const string EditedKey = "ed0408";
    
    /// <summary>
    /// Standard constructor for VideoEditor.
    /// </summary>
    /// <param name="video">The video to be edited.</param>
    /// <param name="editor">Object implementing IEditor to provide basic video editing
    ///     functionality.</param>
    /// <param name="gifCreator">Object implementing IGifCreator to provide gif creation
    ///     functionality.</param>
    /// <param name="compressor">Object implementing ICompressor to provide video compression
    ///     functionality.</param>
    /// <param name="outputDirectory"></param>
    /// <param name="outputFileName"></param>
    /// <param name="outputExtension"></param>
    public VideoEditor(Video video, IEditor editor, IGifCreator gifCreator, ICompressor compressor,
        DirectoryInfo? outputDirectory, string outputFileName, IEditor.Extension outputExtension)
    {
        Footage = video;

        _editor = editor;
        _gifCreator = gifCreator;
        _compressor = compressor;
        
        _outputDirectory = outputDirectory ?? new DirectoryInfo(video.AbsPath);
        _outputFileName = outputFileName;
        OutputExtension = outputExtension;

        if (outputDirectory is null) 
            return;
        
        var newLocation = OutputPath.Replace(EditedKey, OriginalKey);
        File.Copy(Footage.AbsPath, newLocation, true);
        Footage = new Video(newLocation);
        CurrentVideoPath = newLocation.Substring(newLocation.LastIndexOf('\\') + 1);

        if (!outputDirectory.Exists)
        {
            outputDirectory.Create();
        }

    }
    
    /// <summary>
    /// Hack for getting the latest edit from the VideoPlayer.
    /// </summary>
    public static string? CurrentVideoPath { get; private set; }

    /// <summary>
    /// Move the video from the edited location to the original location.
    /// </summary>
    public void CopyEditedToOriginal()
    {
        var newLocation = OutputPath.Replace(EditedKey, OriginalKey);
        File.Move(OutputPath, newLocation, true);
        Footage = new Video(newLocation);
        CurrentVideoPath = newLocation.Substring(newLocation.LastIndexOf('\\') + 1);
    }

    public void Export(string finalPath)
    {
        var finalVideoLocation = OutputPath.Replace(EditedKey, OriginalKey);
        File.Copy(finalVideoLocation, finalPath, true);
        DeleteTempFiles();
    }

    private void DeleteTempFiles()
    {
        DirectoryInfo? outputDir = new FileInfo(OutputPath).Directory;
        if (outputDir is null)
            return;
        
        foreach (var file in outputDir.GetFiles())
        {
            if (file.Name.Contains(EditedKey) || file.Name.Contains(OriginalKey))
            {
                file.Delete();
            }
        }
    }

    /// <summary>
    /// Captures a full-sized image of the imported video at the specified time.
    /// </summary>
    /// <param name="captureTime">The time of the frame you want to capture.</param>
    public BooleanResponse CaptureFullImage(TimeSpan captureTime)
    {
        if (TimeSpanIsOutOfBounds(captureTime))
            return OutOfBoundsResponse(nameof(captureTime));
        
        _editor.CaptureImage(InputPath, OutputPath, new Size(-1, -1), captureTime);
        
        return BooleanResponse.Successful;
    }

    /// <summary>
    /// Captures and crops an image of the imported video at the specified time.
    /// </summary>
    /// <param name="widthHeight">The width and height of the image.
    /// -1 in width or height will resize the image instead of cropping.</param>
    /// <param name="captureTime">The time of the frame you want to capture.</param>
    public BooleanResponse CaptureCroppedImage(Size widthHeight, TimeSpan captureTime)
    {
        if (TimeSpanIsOutOfBounds(captureTime))
            return OutOfBoundsResponse(nameof(captureTime));
        
        _editor.CaptureImage(InputPath, OutputPath, widthHeight, captureTime);
        
        return BooleanResponse.Successful;
    }

    /// <summary>
    /// Merges the imported video with one or more additional videos. The imported video
    /// will go first.
    /// </summary>
    /// <param name="otherInputs">Absolute paths to the additional videos.</param>
    public BooleanResponse MergeWith(string[] otherInputs)
    {
        // TODO: check if all videos have the same extension
        
        if (otherInputs.Length == 0)
            return new BooleanResponse(false, "Must provide at least one video to merge with.");
        
        string[] allInputs = new string[otherInputs.Length + 1];
        allInputs[0] = InputPath;
        for (int i = 0; i < otherInputs.Length; i++)
        {
            allInputs[i + 1] = otherInputs[i];
        }

        _editor.Join(allInputs, OutputPath);
        
        return BooleanResponse.Successful;
    }

    /// <summary>
    /// Trims the imported video from startTime to endTime.
    /// </summary>
    /// <param name="startTime">When the trimmed video will start.</param>
    /// <param name="endTime">When the trimmed video will the end.</param>
    public BooleanResponse Trim(TimeSpan startTime, TimeSpan endTime)
    {
        if (TimeSpanIsOutOfBounds(startTime))
            return OutOfBoundsResponse(nameof(startTime));
        
        if (TimeSpanIsOutOfBounds(endTime))
            return OutOfBoundsResponse(nameof(endTime));

        if (startTime >= endTime)
            return BadStartTimeResponse();
        
        _editor.Trim(InputPath, OutputPath, startTime, endTime);
        
        return BooleanResponse.Successful;
    }

    /// <summary>
    /// Mute the imported video.
    /// Note: There is no unmute operation yet.
    /// </summary>
    public BooleanResponse Mute()
    {
        _editor.Mute(InputPath, OutputPath);
        
        return BooleanResponse.Successful;
    }

    /// <summary>
    /// Converts the imported video to a new video type.
    /// Only some types are supported.
    /// </summary>
    /// <param name="newExtension">The extension of the output video type.</param>
    public BooleanResponse Convert(IEditor.Extension newExtension)
    {
        string oldExt = new FileInfo(OutputPath).Extension;
        string newExt = IEditor.ExtToString(newExtension);
        string output = OutputPath.Replace(oldExt, newExt);
        _editor.Convert(InputPath, output, newExtension);

        OutputExtension = newExtension;
        
        return BooleanResponse.Successful;
    }

    /// <summary>
    /// Creates a gif from the imported video.
    /// </summary>
    public BooleanResponse ConvertToGif()
    {
        _gifCreator.CaptureGif(InputPath, OutputPath, new Size(-1, -1),
            TimeSpan.Zero, Footage.Duration, TimeSpan.Zero);
        
        return BooleanResponse.Successful;
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
    public BooleanResponse CaptureGif(TimeSpan startTime, TimeSpan endTime, TimeSpan duration, 
        Size? widthHeight = null)
    {
        if (TimeSpanIsOutOfBounds(startTime))
            return OutOfBoundsResponse(nameof(startTime));
        
        if (TimeSpanIsOutOfBounds(endTime))
            return OutOfBoundsResponse(nameof(endTime));
        
        if (TimeSpanIsOutOfBounds(duration))
            return OutOfBoundsResponse(nameof(duration));
        
        if (startTime >= endTime)
            return BadStartTimeResponse();
        
        widthHeight ??= new Size(-1, -1);
        _gifCreator.CaptureGif(InputPath, OutputPath, (Size)widthHeight, startTime, endTime, 
            duration);
        
        return BooleanResponse.Successful;
    }

    /// <summary>
    /// Compresses the input video down to the given size in KB.
    /// </summary>
    /// /// <param name="maxKilobytes">The maximum size of the output video in KB.</param>
    public BooleanResponse CompressDownTo(long maxKilobytes)
    {
        if (maxKilobytes < 1)
            return new BooleanResponse(false, "Cannot compress down to less than 1 KB");
        
        _compressor.Compress(InputPath, OutputPath, maxKilobytes);
        
        return BooleanResponse.Successful;
    }

    /// <summary>
    /// Compresses the input video by a percentage equal to the given compression factor
    /// (1.0 = 100%)
    /// </summary>
    /// <param name="compressionFactor">Smaller compression factors tell FFMpeg to compress
    /// the video further. This value must be greater than 0 and less than 1.</param>
    public BooleanResponse CompressBy(float compressionFactor)
    {
        // 0 < compressionFactor < 1
        compressionFactor = Math.Max(compressionFactor, float.Epsilon);
        compressionFactor = Math.Min(compressionFactor, 1.0f - float.Epsilon);

        _compressor.Compress(InputPath, OutputPath, compressionFactor);
        
        return BooleanResponse.Successful;
    }
    
    /// <param name="span">The time span to test.</param>
    /// <returns>True if the given time span is out of bounds.</returns>
    private bool TimeSpanIsOutOfBounds(TimeSpan span)
    {
        if (span > Footage.Duration)
            return true;
        
        if (span < TimeSpan.Zero)
            return true;

        return false;
    }
    
    /// <param name="paramName">The name of the parameter that was out of bounds.</param>
    /// <returns>A standard response for out of bounds parameters.</returns>
    private static BooleanResponse OutOfBoundsResponse(string paramName)
    {
        var response = new BooleanResponse(false, $"{paramName} is out of bounds)");
        return response;
    }
    
    /// <returns>
    /// A standard response for start times that are after the end time.
    /// </returns>
    private static BooleanResponse BadStartTimeResponse()
    {
        return new BooleanResponse(false, "Start time must be before end time.");
    }

}