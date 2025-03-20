using System.Collections.ObjectModel;

namespace AirClipControls;

public class UserControlViewModel
{
    public ObservableCollection<TemplateViewModel> TemplateViewModels { get; set; } = [];

    public UserControlViewModel()
    {
        TemplateViewModels.Add(new TemplateViewModel
        {
            ExpanderName = "Scene",
            Content = "Some scene settings",
            CheckboxVisible = false,
        });
        TemplateViewModels.Add(new TemplateViewModel
        {
            ExpanderName = "Units",
            Content = "Fancy physical units",
            CheckboxVisible = false,
        });
        TemplateViewModels.Add(new TemplateViewModel
        {
            ExpanderName = "Gravity",
            Content = "Are you falling down?",
            CheckboxVisible = true,
        });
    }
}