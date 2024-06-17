using System.Xml.Serialization;
using System.IO;
using net_doc_desktop;
using System.CodeDom;

public static class Config
{
    public static string url = "https://longridge-high-school.github.io/net-doc/";
    public static string configFilePath = ".\\config.xml";

    public static void Save ()
    {
        ConfigFile file = new ConfigFile ();

        file.url = Config.url;
        file.configFilePath = Config.configFilePath;

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
        }
        catch
        {
            Console.WriteLine ("Could not find config file.");
        }
    }
}

[Serializable]
public class ConfigFile
{
    public string url;
    public string configFilePath;
}