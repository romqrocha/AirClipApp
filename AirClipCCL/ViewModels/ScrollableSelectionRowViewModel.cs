using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AirClipCCL.ViewModels;

public partial class ScrollableSelectionRowViewModel : ObservableObject
{
    [ObservableProperty] private List<ButtonIconViewModel> _editingOperations = [];

    public ScrollableSelectionRowViewModel()
    {
        EditingOperations.Add(new ButtonIconViewModel
        {
            HandleClick = () => {},
            IconLabel = "Cut"
        });
    }
}