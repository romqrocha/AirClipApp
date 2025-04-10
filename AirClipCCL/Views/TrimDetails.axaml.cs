using AirClipCCL.ViewModels;

namespace AirClipCCL.Views;

public partial class TrimDetails : OperationDetailsControl
{
    /// <inheritdoc/>
    public override EditOperation OperationType => EditOperation.Trim;

    public override void OnPerformOperation()
    {
        ViewModel.ParseStartTime();
        ViewModel.ParseEndTime();
    }

    public TrimDetails()
    {
        InitializeComponent();
        DataContext = ViewModel;
    }
}