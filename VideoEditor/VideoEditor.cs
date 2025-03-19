using System.Drawing;

namespace VideoEditor;

public class VideoEditor
{
    private Video _footage;
    private readonly IEditor _editor;
    private readonly IGifCreator _gifCreator;
    private readonly ICompressor _compressor;
    private DirectoryInfo _outputDirectory;
    private string _outputFileName;
    private string _outputExtension;

    private string InputPath => _footage.Path;
    private string OutputPath => Path.Join(_outputDirectory.FullName, _outputFileName + _outputExtension);

    public VideoEditor(Video video, IEditor editor, IGifCreator gifCreator, ICompressor compressor, 
        DirectoryInfo outputDirectory, string outputFileName, string outputExtension)
    {
        _footage = video;
        
        _editor = editor;
        _gifCreator = gifCreator;
        _compressor = compressor;
        
        _outputDirectory = outputDirectory;
        _outputFileName = outputFileName;
        _outputExtension = outputExtension;
    }

    public void CaptureFullImage(TimeSpan captureTime)
    {
        _editor.CaptureImage(InputPath, OutputPath, new Size(-1, -1), captureTime);
    }

    public void CaptureCroppedImage(Size widthHeight, TimeSpan captureTime)
    {
        _editor.CaptureImage(InputPath, OutputPath, widthHeight, captureTime);
    }

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

    public void Trim(string input, string output, TimeSpan startTime, TimeSpan endTime)
    {
        _editor.Trim(InputPath, OutputPath, startTime, endTime);
    }

    public void Mute()
    {
        _editor.Mute(InputPath, OutputPath);
    }

    public void Convert(IEditor.Extension newExtension)
    {
        // TODO: output path should have new extension instead of old extension
        _editor.Convert(InputPath, OutputPath, newExtension);
    }

    public void ConvertToGif()
    {
        _gifCreator.CaptureGif(InputPath, OutputPath, new Size(-1, -1), 
            TimeSpan.Zero, _footage.Duration, TimeSpan.Zero);
    }

    public void CaptureGif(TimeSpan startTime, TimeSpan endTime, TimeSpan duration, Size? widthHeight = null)
    {
        widthHeight ??= new Size(-1, -1);
        _gifCreator.CaptureGif(InputPath, OutputPath, (Size)widthHeight, startTime, endTime, duration);
    }

    public void CompressDownTo(long maxKilobytes)
    {
        _compressor.Compress(InputPath, OutputPath, maxKilobytes);
    }

    public void CompressBy(float compressionFactor)
    {
        _compressor.Compress(InputPath, OutputPath, compressionFactor);
    }
}