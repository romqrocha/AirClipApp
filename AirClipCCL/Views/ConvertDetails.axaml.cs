using AirClipCCL.ViewModels;
using Avalonia.Controls;

namespace AirClipCCL.Views;

public partial class ConvertDetails : UserControl
{
    /// <summary> View Model for this Control </summary>
    public OperationDetailsViewModel ViewModel { get; } = new OperationDetailsViewModel();
    
    public ConvertDetails()
    {
        InitializeComponent();
        DataContext = ViewModel;
    }
}