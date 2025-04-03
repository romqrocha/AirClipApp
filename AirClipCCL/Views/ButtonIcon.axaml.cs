using System;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Media;

namespace AirClipCCL.Views;

/**
 * This is Avalonia UI's more complex version of a Custom Control
 * I followed this tutorial to learn more about it:
 * https://medium.com/@adamciszewski/avalonia-user-vs-templated-control-code-examples-b05301baf3c0
 */
public class ButtonIcon : TemplatedControl
{
    #region IconLabel
    public static readonly StyledProperty<string> IconLabelProperty =
        AvaloniaProperty.Register<ButtonIcon, string>(nameof(IconLabel));

    public string IconLabel
    {
        get => GetValue(IconLabelProperty);
        set => SetValue(IconLabelProperty, value);
    }
    #endregion
    
    #region HandleClick
    public static readonly StyledProperty<Action> HandleClickProperty =
        AvaloniaProperty.Register<ButtonIcon, Action>(nameof(HandleClick));

    public Action HandleClick
    {
        get => GetValue(HandleClickProperty);
        set => SetValue(HandleClickProperty, value);
    }
    #endregion
    
    #region IconGeometry
    public static readonly StyledProperty<StreamGeometry> IconGeometryProperty =
        AvaloniaProperty.Register<ButtonIcon, StreamGeometry>(nameof(IconGeometry));

    public StreamGeometry IconGeometry
    {
        get => GetValue(IconGeometryProperty);
        set => SetValue(IconGeometryProperty, value);
    }
    #endregion
}