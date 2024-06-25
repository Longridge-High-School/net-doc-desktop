using System.IO;

using net_doc_desktop;

public static class Globals
{
    public static bool hasCreatedTray = false;
    public static MainWindow mainWindow;
    public static string defaultConfigPath = Environment.GetFolderPath (Environment.SpecialFolder.LocalApplicationData) + "\\net-doc-desktop";
}