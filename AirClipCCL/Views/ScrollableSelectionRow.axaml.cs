using AirClipCCL.ViewModels;
using Avalonia.Controls;

namespace AirClipCCL.Views;

/// <summary>
/// This is Avalonia UI's version of a User Control.
/// I followed <see href="https://medium.com/@adamciszewski/avalonia-user-vs-templated-control-code-examples-b05301baf3c0">
/// this tutorial </see> to learn more about it. 
/// </summary>
/// <authors>Rodrigo Rocha</authors>
public partial class ScrollableSelectionRow : UserControl
{
    public ScrollableSelectionRow()
    {
        InitializeComponent();
        DataContext = new ScrollableSelectionRowViewModel();
    }
}