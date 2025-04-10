using AirClipCCL.ViewModels;

namespace AirClipCCL.Views;

public partial class ConvertDetails : OperationDetailsControl
{
    /// <inheritdoc/>
    public override EditOperation OperationType => EditOperation.Convert;

    /// <inheritdoc/>
    public override void OnPerformOperation()
    {
        ViewModel.ParseNewExtension();
    }

    public ConvertDetails()
    {
        InitializeComponent();
        DataContext = ViewModel;
    }
}