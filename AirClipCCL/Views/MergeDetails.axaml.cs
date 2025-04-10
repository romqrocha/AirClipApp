using AirClipCCL.ViewModels;

namespace AirClipCCL.Views;

public partial class MergeDetails : OperationDetailsControl
{
    /// <inheritdoc/>
    public override EditOperation OperationType => EditOperation.Merge;

    /// <inheritdoc/>
    public override void OnPerformOperation()
    {
        ViewModel.ParseVideoPath();
        
    }

    public MergeDetails()
    {
        InitializeComponent();
        DataContext = ViewModel;
    }
}