using System;
using System.Drawing;

namespace AirClipApp;

/// <summary>
/// Defines a method for editors that want to be able to capture gifs from videos
/// </summary>
public interface IGifCreator
{
    /// <summary>
    /// Creates a gif from the input video.
    /// </summary>
    /// <param name="input">Path to the input file.</param>
    /// <param name="output">Path to where the output file will be.</param>
    /// <param name="widthHeight"> The width and height of the image.
    /// -1 in width or height tells ffmpeg resize while keeping aspect ratio.</param>
    /// <param name="startTime">When in the input video the gif will start.</param>
    /// <param name="endTime">When in the input video the gif will end.</param>
    /// <param name="duration">The duration of the gif (will be sped up or slowed down to fit).
    /// TimeSpan.Zero for the default duration (without changes in speed).</param>
    public void CaptureGif(string input, string output, Size widthHeight, 
        TimeSpan startTime, TimeSpan endTime, TimeSpan duration);
}