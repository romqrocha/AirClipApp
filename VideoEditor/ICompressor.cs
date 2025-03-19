namespace VideoEditor;

/// <summary>
/// Defines methods for classes that want to compress videos.
/// </summary>
public interface ICompressor
{
    /// <summary>
    /// Compresses the input video down to the given size in KB.
    /// </summary>
    /// <param name="input">Path to the input file.</param>
    /// <param name="output">Path to where the output file will be.</param>
    /// <param name="maxKilobytes">The maximum size of the output video in KB.</param>
    public void Compress(string input, string output, long maxKilobytes);

    /// <summary>
    /// Compresses the input video by a percentage equal to the given compression factor
    /// (1.0 = 100%)
    /// </summary>
    /// <param name="input">Path to the input file.</param>
    /// <param name="output">Path to where the output file will be.</param>
    /// <param name="compressionFactor">Smaller compression factors tell FFMpeg to compress the
    /// video further. This value must be greater than 0 and less than 1.</param>
    /// <exception cref="ArgumentException">If compressionFactor &lt;= 0 or &gt;= 1</exception>
    public void Compress(string input, string output, float compressionFactor);
}