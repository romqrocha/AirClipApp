using AirClipCCL.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AirClipCCL.Views;

/**
 * This is Avalonia UI's version of a User Control
 * I followed this tutorial to learn more about it:
 * https://medium.com/@adamciszewski/avalonia-user-vs-templated-control-code-examples-b05301baf3c0
 */
public partial class ScrollableSelectionRow : UserControl
{
    public ScrollableSelectionRow()
    {
        InitializeComponent();
        DataContext = new ScrollableSelectionRowViewModel();
    }
}