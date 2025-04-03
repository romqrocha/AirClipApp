using System.Windows.Input;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Media;

namespace AirClipCCL.Views;

/// <summary>
/// This is Avalonia UI's more complex version of a custom control (templated control).
/// I followed <see href="https://medium.com/@adamciszewski/avalonia-user-vs-templated-control-code-examples-b05301baf3c0">
/// this tutorial </see> to learn more about it. 
/// </summary>
/// <authors>Rodrigo Rocha</authors>
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
    public static readonly StyledProperty<ICommand> HandleClickProperty =
        AvaloniaProperty.Register<ButtonIcon, ICommand>(nameof(HandleClick));

    public ICommand HandleClick
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