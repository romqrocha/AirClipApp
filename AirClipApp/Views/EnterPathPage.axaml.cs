using Avalonia.Controls;

namespace AirClipApp.Views;

/// <summary>
/// The page where the user should enter the path to their FFmpeg installation.
/// </summary>
/// <resources>
/// Followed <see href="https://docs.avaloniaui.net/docs/get-started/test-drive/">
/// Avalonia's Documentation </see> while creating this. For information about FFmpeg, see
/// <see href="https://ffmpeg.org/ffmpeg.html">FFmpeg Documentation</see>.
/// </resources>
/// <authors> Rodrigo Rocha, Taeyang Seo </authors>
public partial class EnterPathPage : UserControl
{
    // FFmpeg path: /opt/homebrew/Cellar/ffmpeg/7.1.1_1/bin/
    // Temp files path: /Users/danielseo/Downloads/temp_files

    public EnterPathPage()
    {
        InitializeComponent();
    }
}