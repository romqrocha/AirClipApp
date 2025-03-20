using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AirClipControls;

public partial class ScrollableRow : UserControl
{
    public ScrollableRow()
    {
        DataContext = new UserControlViewModel();
        InitializeComponent();
    }
}