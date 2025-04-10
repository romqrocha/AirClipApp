using AirClipCCL.ViewModels;

namespace AirClipCCL.Views;

public partial class CaptureDetails : OperationDetailsControl
{
    /// <inheritdoc/>
    public override EditOperation OperationType => EditOperation.Capture;

    /// <inheritdoc/>
    public override void OnPerformOperation()
    {
        ViewModel.ParseStartTime();
        ViewModel.ParseWidth();
        ViewModel.ParseHeight();
    }
    
    public CaptureDetails()
    {
        InitializeComponent();
        DataContext = ViewModel;
    }
}