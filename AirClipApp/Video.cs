using System;
using System.Drawing;
using System.IO;
using FFMpegCore;
using FFMpegCore.Enums;
using FFMpegCore.Pipes;

namespace AirClipApp;

public class Video
{
    public string InputPath { get; private set; }
    public string OutputPath { get; private set; }

    public int Width { get; }
    public int Height { get; }
    
    public Video(string inputPath, string outputPath)
    {
        InputPath = inputPath;
        OutputPath = outputPath;
        var mediaInfo = FFProbe.Analyse(InputPath);
        Width = mediaInfo.PrimaryVideoStream!.Width;
        Height = mediaInfo.PrimaryVideoStream!.Height;
    }
    

    public IMediaAnalysis GetInfo()
    {
        var mediaInfo = FFProbe.Analyse(InputPath);
        return mediaInfo;
    }

    public void Join(string otherVideoPath)
    {
        FFMpeg.Join(OutputPath, InputPath, otherVideoPath);
    }

    public void Snapshot(int width, int height)
    {
        FFMpeg.Snapshot(InputPath, OutputPath, new Size(width, height), TimeSpan.FromSeconds(1));
    }

    public void Trim(int start, int end)
    {
        FFMpeg.SubVideo(InputPath, OutputPath, TimeSpan.FromSeconds(start), TimeSpan.FromSeconds(end));
    }

    public void Mute()
    {
        FFMpeg.Mute(InputPath, OutputPath);
    }
    
    public void Compress()
    {
        FFMpegArguments
            .FromFileInput(InputPath)
            .OutputToFile(OutputPath, true, options => options
                .WithVideoCodec(VideoCodec.LibX264)
                .WithConstantRateFactor(21)
                .WithAudioCodec(AudioCodec.Aac)
                .WithVariableBitrate(4)
                .WithVideoFilters(filterOptions => filterOptions
                    .Scale(VideoSize.Ld))
                .WithFastStart())
            .ProcessSynchronously();
    }

    public void ExportWeb(Stream source, Stream destination)
    {
        FFMpegArguments
            .FromPipeInput(new StreamPipeSource(source))
            .OutputToPipe(new StreamPipeSink(destination), options => options
                .WithVideoCodec("vp9")
                .ForceFormat("webm"))
            .ProcessSynchronously();
    }
    
    
}