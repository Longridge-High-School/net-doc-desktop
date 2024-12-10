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
using System.IO;

namespace net_doc_desktop;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow ()
    {
        if (IsProcessRunningSameSession ())
        {
            MessageBox.Show ("Only one Net Doc Desktop instance can run at a time.");
            System.Windows.Application.Current.Shutdown ();
        }
        
        Directory.CreateDirectory (Globals.defaultConfigPath); // Default config directory should always exist.
        Config.Load ();
        
        if (!Globals.hasCreatedTray)
        {
            Tray tray = new Tray ();
            tray.Show ();
        }
        
        Globals.mainWindow = this;

        InitializeComponent ();

        webView.Source = new Uri (Config.url);
    }

    protected override void OnClosing (CancelEventArgs e)
    {
        e.Cancel = true;
        
        this.Hide ();
    }

    public bool IsProcessRunningSameSession ()
    {
        var currentSessionID = System.Diagnostics.Process.GetCurrentProcess ().SessionId;
        return System.Diagnostics.Process.GetProcessesByName (System.IO.Path.GetFileNameWithoutExtension (System.Reflection.Assembly.GetEntryAssembly ().Location)).Where (p => p.SessionId == currentSessionID).Count () > 1;
    }
}