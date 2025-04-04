using CommunityToolkit.Mvvm.ComponentModel;

namespace AirClipCCL.ViewModels;

public partial class OperationDetailsViewModel : ObservableObject
{
    private const string ZeroFloat = "0.0";
    private const string DefaultPath = "";
    private const string NotApplicable = "N/A";
    private const string DefaultPercentage = "50.0";
    private const string DefaultExtension = "mov";
    
    [ObservableProperty] private string _startTimeInput = ZeroFloat;
    [ObservableProperty] private string _endTimeInput = NotApplicable;
    
    [ObservableProperty] private string _videoPathInput = DefaultPath;
    
    [ObservableProperty] private string _durationInput = ZeroFloat;
    
    [ObservableProperty] private string _widthInput = NotApplicable;
    [ObservableProperty] private string _heightInput = NotApplicable;

    [ObservableProperty] private string _sizeInMbInput = NotApplicable;
    [ObservableProperty] private string _compressionLevelInput = DefaultPercentage; 
    
    [ObservableProperty] private string _newExtensionInput = DefaultExtension;
}