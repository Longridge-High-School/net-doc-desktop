using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace net_doc_desktop;

/// <summary>
/// Interaction logic for Tray.xaml
/// </summary>
public partial class Tray : Window
{
    public Tray ()
    {
        InitializeComponent ();
    }

    void OpenWindow (object sender, RoutedEventArgs e)
    {
        Globals.mainWindow.Show ();
    }

    void OpenSettings (object sender, RoutedEventArgs e)
    {
        SettingsWindow settingsWindow = new SettingsWindow ();
        settingsWindow.Show ();
    }

    void CloseApp (object sender, RoutedEventArgs e)
    {
        System.Windows.Application.Current.Shutdown ();
    }
}