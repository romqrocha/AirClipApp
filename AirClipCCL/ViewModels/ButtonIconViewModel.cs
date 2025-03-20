using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AirClipCCL.ViewModels;

public partial class ButtonIconViewModel : ObservableObject
{
    [ObservableProperty] private Action _handleClick = () => {};
    [ObservableProperty] private string _iconLabel = string.Empty;
}