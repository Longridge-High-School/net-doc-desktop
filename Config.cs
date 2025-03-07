using System.Xml.Serialization;
using System.IO;
using net_doc_desktop;
using System.CodeDom;
using System.Windows;

public static class Config
{
    public static string url = "https://longridge-high-school.github.io/net-doc/";
    public static string configFilePath = Globals.defaultConfigPath + "\\config.xml";
    public static string css = "https://raw.githubusercontent.com/FabrizioMusacchio/GitHub_Flavor_Markdown_CSS/refs/heads/master/GitHub%20Flavor.css";

    public static void Save ()
    {
        ConfigFile file = new ConfigFile ();

        file.url = Config.url;
        file.configFilePath = Environment.ExpandEnvironmentVariables (Config.configFilePath);
        file.css = Config.css;

        var serialiser = new XmlSerializer (typeof (ConfigFile));

        using (var writer = new StreamWriter (Config.configFilePath))
        {
            serialiser.Serialize (writer, file);
        }
    }

    public static void Load ()
    {
        try
        {
            var serialiser = new XmlSerializer (typeof (ConfigFile));
            ConfigFile file;

            using (var reader = new StreamReader (Config.configFilePath))
            {
                file = (ConfigFile) serialiser.Deserialize (reader);
            }

            Config.url = file.url;
            Config.configFilePath = file.configFilePath;
            Config.css = file.css;
        }
        catch (Exception e)
        {
            Console.WriteLine (e.ToString ());
        }
    }
}

[Serializable]
public class ConfigFile
{
    public string url;
    public string configFilePath;
    public string css;
}