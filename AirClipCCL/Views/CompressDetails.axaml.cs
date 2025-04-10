using AirClipCCL.ViewModels;

namespace AirClipCCL.Views;

public partial class CompressDetails : OperationDetailsControl
{
    /// <inheritdoc/>
    public override EditOperation OperationType => EditOperation.Compress;

    /// <inheritdoc/>
    public override void OnPerformOperation()
    {
        ViewModel.ParseSizeInMb();
        ViewModel.ParseCompressionLevel();
    }

    public CompressDetails()
    {
        InitializeComponent();
        DataContext = ViewModel;
    }
}