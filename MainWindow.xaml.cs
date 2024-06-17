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
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow ()
    {
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
}