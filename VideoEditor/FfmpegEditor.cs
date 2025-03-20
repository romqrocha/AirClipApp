using System.Drawing;
using FFMpegCore;
using FFMpegCore.Enums;

namespace VideoEditor;

/// <summary>
/// Implements editor functionality for this program by calling an FFmpeg process.
/// </summary>
/// <remarks>
/// If you are looking for method documentation, it's all inherited from the
/// interfaces they implement.
/// </remarks>
/// <resources>
/// The <see href="https://github.com/rosenbjerg/FFMpegCore?tab=readme-ov-file#ffmpegcore">
/// FFMpegCore documentation </see> was heavily consulted to write this implementation.
/// </resources>
/// <authors> Rodrigo Rocha, Tae Seo </authors>
public class FfmpegEditor : IEditor, IGifCreator, ICompressor
{
    /// <summary>
    /// Initializes a new FfmpegEditor.
    /// </summary>
    /// <param name="ffmpegBinFolder">The /bin folder in your FFMpeg installation.</param>
    /// <param name="tempFilesWorkingFolder">A folder where files can be stored temporarily.</param>
    public FfmpegEditor(DirectoryInfo ffmpegBinFolder, DirectoryInfo tempFilesWorkingFolder)
    {
        GlobalFFOptions.Configure(new FFOptions
        {
            BinaryFolder = ffmpegBinFolder.FullName, 
            TemporaryFilesFolder = tempFilesWorkingFolder.FullName,
        });
    }
    
    public void CaptureImage(string input, string output, Size widthHeight, TimeSpan captureTime)
    {
        FFMpeg.Snapshot(input, output, widthHeight, captureTime);
    }

    public void Join(string[] inputs, string output)
    {
        FFMpeg.Join(output, inputs);
    }

    public void Trim(string input, string output, TimeSpan startTime, TimeSpan endTime)
    {
        FFMpeg.SubVideo(input, output, startTime, endTime);
    }
    
    public void Mute(string input, string output)
    {
        FFMpeg.Mute(input, output);
    }

    public void Convert(string input, string output, IEditor.Extension newExtension)
    {
        ContainerFormat newFormat = ExtToType(newExtension);
        FFMpeg.Convert(input, output, newFormat);
        return;

        // TODO: find a better place for this function (assuming it will be used elsewhere)
        ContainerFormat ExtToType(IEditor.Extension extension)
        {
            return extension switch
            {
                IEditor.Extension.Mp4 => VideoType.Mp4,
                IEditor.Extension.Mov => VideoType.Mov,
                IEditor.Extension.WebM => VideoType.WebM,
                _ => throw new ArgumentOutOfRangeException(nameof(extension), extension, 
                    "Invalid extension.")
            };
        }
}

    public void CaptureGif(string input, string output, Size widthHeight, 
        TimeSpan startTime, TimeSpan endTime, TimeSpan duration)
    {
        TimeSpan startToEnd = endTime - startTime;
        FFMpeg.GifSnapshot(input, output, widthHeight, startTime, startToEnd);

        if (duration.Equals(TimeSpan.Zero))
            return;
        
        // TODO: speed up or slow down gif according to given duration
    }

    public void Compress(string input, string output, long maxKilobytes)
    {
        throw new NotImplementedException();
        // TODO: compress video until video size is less than maxKilobytes
    }

    public void Compress(string input, string output, float compressionFactor)
    {
        throw new NotImplementedException();
        // maxSize = originalSize * compressionFactor
        // TODO: compress video until video size is less than maxSize
    }
}