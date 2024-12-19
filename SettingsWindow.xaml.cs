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
using System.Diagnostics;

namespace net_doc_desktop;

/// <summary>
/// Interaction logic for SettingsWindow.xaml
/// </summary>
public partial class SettingsWindow : Window
{
    public SettingsWindow ()
    {
        InitializeComponent ();

        UrlBox.Text = Config.url;
        FilePath.Text = Config.configFilePath;
        CSSBox.Text = Config.css;
    }

    protected override void OnClosing (CancelEventArgs e)
    {
        e.Cancel = true;
        
        this.Hide ();
    }

    private void Apply (object sender, RoutedEventArgs e)
    {
        Config.url = UrlBox.Text;
        Config.configFilePath = FilePath.Text;
        Config.css = CSSBox.Text;
        Config.Save ();

        Process.Start (System.IO.Path.GetDirectoryName (System.Reflection.Assembly.GetEntryAssembly ().Location) + "\\net-doc-desktop.exe");
        System.Windows.Application.Current.Shutdown ();
    }

    private void Cancel (object sender, RoutedEventArgs e)
    {
        this.Hide ();
    }
}