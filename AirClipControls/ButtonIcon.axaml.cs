using Avalonia;
using Avalonia.Controls.Primitives;

namespace AirClipControls;

public class ButtonIcon : TemplatedControl
{
    #region ExpanderName
    public static readonly StyledProperty<string> ExpanderNameProperty =
        AvaloniaProperty.Register<ButtonIcon, string>(nameof(ExpanderName));

    public string ExpanderName
    {
        get => GetValue(ExpanderNameProperty);
        set => SetValue(ExpanderNameProperty, value);
    }
    #endregion
    
    #region Content
    public static readonly StyledProperty<string> ContentProperty =
        AvaloniaProperty.Register<ButtonIcon, string>(nameof(Content));

    public string Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }
    #endregion
    
    #region IsChecked
    public static readonly StyledProperty<bool> IsCheckedProperty =
        AvaloniaProperty.Register<ButtonIcon, bool>(nameof(IsChecked));

    public bool IsChecked
    {
        get => GetValue(IsCheckedProperty);
        set => SetValue(IsCheckedProperty, value);
    }
    #endregion
    
    #region CheckboxVisible
    public static readonly StyledProperty<bool> CheckboxVisibleProperty =
        AvaloniaProperty.Register<ButtonIcon, bool>(nameof(CheckboxVisible));

    public bool CheckboxVisible
    {
        get => GetValue(CheckboxVisibleProperty);
        set => SetValue(CheckboxVisibleProperty, value);
    }
    #endregion
}