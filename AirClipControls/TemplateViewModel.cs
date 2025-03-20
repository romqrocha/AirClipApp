using CommunityToolkit.Mvvm.ComponentModel;

namespace AirClipControls;

public partial class TemplateViewModel : ObservableObject
{
    [ObservableProperty] private string _expanderName = string.Empty;
    [ObservableProperty] private string _content = string.Empty;
    [ObservableProperty] private bool _isChecked;
    [ObservableProperty] private bool _checkboxVisible; 
}