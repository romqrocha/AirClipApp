using System;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AirClipCCL.ViewModels;

public partial class OperationDetailsViewModel : ObservableObject
{
    private const string ZeroFloat = "0.0";
    private const string DefaultPath = "";
    private const string NotApplicable = "N/A";
    private const string DefaultPercentage = "50.0";
    private const string DefaultExtension = "mov";
    
    [ObservableProperty] private string _startTimeInput = ZeroFloat;
    [ObservableProperty] private string _endTimeInput = NotApplicable;
    
    [ObservableProperty] private string _videoPathInput = DefaultPath;
    
    [ObservableProperty] private string _durationInput = ZeroFloat;
    
    [ObservableProperty] private string _widthInput = NotApplicable;
    [ObservableProperty] private string _heightInput = NotApplicable;

    [ObservableProperty] private string _sizeInMbInput = NotApplicable;
    [ObservableProperty] private string _compressionLevelInput = DefaultPercentage; 
    
    [ObservableProperty] private string _newExtensionInput = DefaultExtension;

    /// <summary>
    /// Resets all input fields to their default values.
    /// </summary>
    public void ClearInput()
    {
        StartTimeInput = ZeroFloat;
        EndTimeInput = NotApplicable;
        
        VideoPathInput = DefaultPath;
        
        DurationInput = ZeroFloat;
        
        WidthInput = NotApplicable;
        HeightInput = NotApplicable;
        
        SizeInMbInput = NotApplicable;
        CompressionLevelInput = DefaultPercentage;
    
        NewExtensionInput = DefaultExtension;
    }
    
    public TimeSpan StartTime { get; private set; }

    /// <summary>
    /// Parses the input string for StartTime into a TimeSpan.
    /// </summary>
    public void ParseStartTime()
    {
        double timeFloat = double.Parse(StartTimeInput);
        TimeSpan startTime = TimeSpan.FromSeconds(timeFloat);
        StartTime = startTime;
    }
    
    public TimeSpan? EndTime { get; private set; }
    
    /// <summary>
    /// Parses the input string for EndTime into a TimeSpan.
    /// </summary>
    public void ParseEndTime()
    {
        if (EndTimeInput == NotApplicable)
        {
            EndTime = null;
        }
        else
        {
            double timeFloat = double.Parse(EndTimeInput);
            TimeSpan endTime = TimeSpan.FromSeconds(timeFloat);
            EndTime = endTime;
        }
    }
    
    public FileInfo? VideoPath { get; private set; }

    /// <summary>
    /// Parses the input string for VideoPath into a FileInfo.
    /// </summary>
    public void ParseVideoPath()
    {
        if (VideoPathInput == DefaultPath)
        {
            VideoPath = null;
        }
        else
        {
            FileInfo videoFileInfo = new FileInfo(VideoPathInput);
            VideoPath = videoFileInfo;
        }
    }
    
    public TimeSpan? Duration { get; private set; }

    /// <summary>
    /// Parses the input string for Duration into a TimeSpan.
    /// </summary>
    public void ParseDuration()
    {
        if (DurationInput == ZeroFloat)
        {
            Duration = null;
        }
        else
        {
            double timeFloat = double.Parse(DurationInput);
            TimeSpan duration = TimeSpan.FromSeconds(timeFloat);
            Duration = duration;
        }
    }
    
    public int? Width { get; private set; }

    /// <summary>
    /// Parses the input string for Width into an int.
    /// </summary>
    public void ParseWidth()
    {
        if (WidthInput == NotApplicable)
        {
            Width = null;
        }
        else
        {
            int width = int.Parse(WidthInput);
            Width = width;
        }
    }
    
    public int? Height { get; private set; }

    /// <summary>
    /// Parses the input string for Height into an int.
    /// </summary>
    public void ParseHeight()
    {
        if (HeightInput == NotApplicable)
        {
            Height = null;
        }
        else
        {
            int height = int.Parse(HeightInput);
            Height = height; 
        }
    }

    public int? SizeInMb { get; private set; }

    /// <summary>
    /// Parses the input string for SizeInMb into an int.
    /// </summary>
    public void ParseSizeInMb()
    {
        if (SizeInMbInput == NotApplicable)
        {
            SizeInMb = null;
        }
        else
        {
            int sizeInMb = int.Parse(SizeInMbInput);
            SizeInMb = sizeInMb;
        }
    }
    
    public float? CompressionLevel { get; private set; }

    /// <summary>
    /// Parses the input string percentage for CompressionLevel into a float (0.0, 1.0)
    /// </summary>
    public void ParseCompressionLevel()
    {
        if (CompressionLevelInput == NotApplicable)
        {
            CompressionLevel = null;
        }
        else
        {
            float percentage = float.Parse(CompressionLevelInput);
            float proportion = percentage / 100f;
            CompressionLevel = proportion;
        }
    }
    
    public string? NewExtension { get; private set; }

    /// <summary>
    /// Parses the input string for NewExtension.
    /// </summary>
    public void ParseNewExtension()
    {
        string newExt = NewExtensionInput;
        NewExtension = newExt;
    }
}