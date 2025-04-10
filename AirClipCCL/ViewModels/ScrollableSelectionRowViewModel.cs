using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using AirClipCCL.Views;
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
    private Dictionary<EditOperation, OperationDetailsControl> OperationsMap { get; } = new()
    {
        [EditOperation.Capture] = new CaptureDetails(),
        [EditOperation.Merge] = new MergeDetails(),
        [EditOperation.Mute] = new MuteDetails(),
        [EditOperation.Convert] = new ConvertDetails(),
        [EditOperation.Gif] = new GifDetails(),
        [EditOperation.Compress] = new CompressDetails(),
        [EditOperation.Trim] = new TrimDetails()
    };
    
    [ObservableProperty] private EditOperation _selectedButton = EditOperation.Trim;
    
    [RelayCommand]
    private void SetSelectedButton(string buttonName)
    {
        bool success = Enum.TryParse(buttonName, true, out EditOperation selected);
        Debug.WriteLine($"{buttonName}");
        if (!success)
            throw new InvalidEnumArgumentException($"{buttonName} is not a valid button");
        
        SelectedButton = selected;
    }
    partial void OnSelectedButtonChanged(EditOperation value)
    {
        OperationDetailsControl control = OperationsMap[value];
        SelectedButtonChanged?.Invoke(control);
    }
    public event Action<OperationDetailsControl>? SelectedButtonChanged;
    
    
}

public enum EditOperation
{
    Trim,
    Capture,
    Merge,
    Mute,
    Convert,
    Gif,
    Compress
}