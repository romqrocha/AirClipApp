using AirClipCCL.ViewModels;

namespace AirClipCCL.Views;

public partial class MuteDetails : OperationDetailsControl
{
    /// <inheritdoc/>
    public override EditOperation OperationType => EditOperation.Mute;
    
    public MuteDetails()
    {
        InitializeComponent();
        DataContext = ViewModel;
    }

    public override void OnPerformOperation()
    {
        
    }
    
    
}