using System;
using System.Drawing;
using FFMpegCore.Enums;

namespace AirClipApp;

/// <summary>
/// Defines all the operations that an editor should perform on a video,
/// given its absolute file path and an output path.
/// </summary>
public interface IEditor
{
    /// <summary>
    /// Creates an image from one of the frames in the input video.
    /// </summary>
    /// <param name="input">Path to the input file.</param>
    /// <param name="output">Path to where the output file will be.</param>
    /// <param name="widthHeight">The width and height of the image.
    /// -1 in width or height tells ffmpeg resize while keeping aspect ratio.</param>
    /// <param name="captureTime">The time of the frame you want to capture</param>
    public void CaptureImage(string input, string output, Size widthHeight, TimeSpan captureTime);
    
    /// <summary>
    /// Merges multiple input videos into an output video, one after the other.
    /// </summary>
    /// <param name="inputs">A string array of input file paths.</param>
    /// <param name="output">Path to where the output file will be.</param>
    public void Join(string[] inputs, string output);
    
    /// <summary>
    /// Trims the input video from startTime to endTime.
    /// </summary>
    /// <param name="input">Path to the input file.</param>
    /// <param name="output">Path to where the output file will be.</param>
    /// <param name="startTime">When the output video will begin.</param>
    /// <param name="endTime">When the output video will end.</param>
    /// <exception cref="ArgumentException">If endTime is before startTime.</exception>
    public void Trim(string input, string output, TimeSpan startTime, TimeSpan endTime);

    /// <summary>
    /// Mutes the input video.
    /// </summary>
    /// <param name="input">Path to the input file.</param>
    /// <param name="output">Path to where the output file will be.</param>
    public void Mute(string input, string output);

    /// <summary>
    /// Converts the input video from one format to another.
    /// </summary>
    /// <param name="input">Path to the input file.</param>
    /// <param name="output">Path to where the output file will be.</param>
    /// <param name="newExtension">The file extension to convert the input video to.</param>
    public void Convert(string input, string output, Extension newExtension);

    public enum Extension
    {
        Mp4, 
        Mov,
        WebM
    }
}