using AirClipCCL.ViewModels;
using Avalonia.Controls;

namespace AirClipCCL.Views;

public abstract partial class OperationDetailsControl : UserControl
{
    /// <summary> View Model for this Control </summary>
    public OperationDetailsViewModel ViewModel { get; } = new OperationDetailsViewModel();
    
    /// <summary> The type of operation that this control is for. </summary>
    public abstract EditOperation OperationType { get; }
    
    /// <summary>
    /// Parses the appropriate input controls and calls the
    /// correct method to perform the operation.
    /// </summary>
    public abstract void OnPerformOperation();

    protected OperationDetailsControl()
    {
        InitializeComponent();
    }
}