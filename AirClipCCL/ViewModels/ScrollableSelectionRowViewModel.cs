using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using AirClipCCL.Views;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AirClipCCL.ViewModels;

/// <summary>
/// View Model for ScrollableSelectionRow
/// </summary>
/// <authors>Rodrigo Rocha</authors>
[SuppressMessage("ReSharper", "ConvertSwitchStatementToSwitchExpression")]
public partial class ScrollableSelectionRowViewModel : ObservableObject
{
    public event Action<UserControl>? SelectedButtonChanged;
    
    [ObservableProperty] private SelectableButtons _selectedButton = SelectableButtons.Trim;
    partial void OnSelectedButtonChanged(SelectableButtons value)
    {
        UserControl control;
        switch (value)
        {
            case SelectableButtons.Capture:
                control = new CaptureDetails();
                break;
            case SelectableButtons.Merge:
                control = new MergeDetails();
                break;
            case SelectableButtons.Mute:
                control = new MuteDetails();
                break;
            case SelectableButtons.Convert:
                control = new ConvertDetails();
                break;
            case SelectableButtons.Gif:
                control = new GifDetails();
                break;
            case SelectableButtons.Compress:
                control = new CompressDetails();
                break;
            case SelectableButtons.Trim:
                control = new TrimDetails();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(value), value, null);
        }
        SelectedButtonChanged?.Invoke(control);
    }

    [RelayCommand]
    private void SetSelectedButton(string buttonName)
    {
        bool success = Enum.TryParse(buttonName, out SelectableButtons selected);
        Debug.WriteLine($"{buttonName}");
        if (!success)
            throw new InvalidEnumArgumentException($"{buttonName} is not a valid button");
        
        SelectedButton = selected;
    }
}

public enum SelectableButtons
{
    Trim,
    Capture,
    Merge,
    Mute,
    Convert,
    Gif,
    Compress
}