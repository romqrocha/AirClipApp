namespace AirClipApp.Views;

/// <summary>
/// UserControl objects for each page owned by the Main Window.
/// </summary>
/// <authors>Rodrigo Rocha, Taeyang Seo</authors>
public class Pages
{
    public static HomePage HomePage { get; } = new HomePage();
    public static EnterPathPage EnterPathPage { get; } = new EnterPathPage();
    public static ImportPage ImportPage { get; } = new ImportPage();
    public static EditorPage EditorPage { get; } = new EditorPage();
}