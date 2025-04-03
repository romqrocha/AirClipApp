using System;
using AirClipCCL.Models;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AirClipCCL.ViewModels;

public partial class ButtonIconViewModel : ObservableObject
{
    [ObservableProperty] private Action _handleClick = () => {};
    [ObservableProperty] private string _iconLabel = "string.Empty";
    [ObservableProperty] private StreamGeometry _iconGeometry = Icons.MissingImage;
}