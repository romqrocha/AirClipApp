using System;
using AirClipCCL.Views;
using Avalonia.Controls;

namespace AirClipApp.Views;

/// <summary>
/// The page where the user can make edits to their video.
/// </summary>
/// <resources>
/// Followed <see href="https://docs.avaloniaui.net/docs/get-started/test-drive/">
/// Avalonia's Documentation </see> while creating this. For information about FFmpeg, see
/// <see href="https://ffmpeg.org/ffmpeg.html">FFmpeg Documentation</see>.
/// </resources>
/// <authors> Rodrigo Rocha, Taeyang Seo </authors>
public partial class EditorPage : UserControl
{
    public EditorPage()
    {
        InitializeComponent();
    }

    public void SubscribeToSelectedButtonChanged(Action<OperationDetailsControl> action)
    {
        SelectionRow.ViewModel.SelectedButtonChanged += action;
    }
}