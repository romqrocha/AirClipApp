using AirClipCCL.ViewModels;

namespace AirClipCCL.Views;

public partial class GifDetails : OperationDetailsControl
{
    /// <inheritdoc/>
    public override EditOperation OperationType => EditOperation.Gif;

    /// <inheritdoc/>
    public override void OnPerformOperation()
    {
        ViewModel.ParseStartTime();
        ViewModel.ParseEndTime();
        ViewModel.ParseDuration();
        ViewModel.ParseWidth();
        ViewModel.ParseHeight();
    }

    public GifDetails()
    {
        InitializeComponent();
        DataContext = ViewModel;
    }
}