using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using System.IO;
using Microsoft.Web.WebView2.WinForms;
using System.Text.RegularExpressions;
using System.Speech.Synthesis;
using System.Reflection.Metadata;
using static System.Net.WebClient;
using SelectPdf;

namespace net_doc_desktop;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private SpeechSynthesizer voice;

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

        voice = new SpeechSynthesizer ();
        
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

    private void Window_KeyDown (object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (e.Key == Key.F1)
        {
            HotKey_About ();
        }
        else if (e.Key == Key.F2)
        {
            HotKey_OpenInBrowser ();
        }
        else if (e.Key == Key.F4)
        {
            HotKey_ReadAloud ();
        }
        else if (e.Key == Key.F5)
        {
            HotKey_Refresh ();
        }
        else if (e.Key == Key.F6)
        {
            HotKey_ExportDocument ();
        }
    }

    private void HotKey_About ()
    {
        MessageBox.Show ("*********************\n* Net Doc Desktop *\n*********************\n\nv2.0\n©️ Copyright 2024 Longridge High School");
    }

    private async void HotKey_OpenInBrowser ()
    {      
        string highlighted = await webView.ExecuteScriptAsync ("try {window.getSelection ().toString ();} catch (error) {alert (error.message);}");

        if (highlighted.Length > 4)
        {
            highlighted = highlighted.Replace ("\\n", " ");
            highlighted = highlighted.Replace ("\"", "");

            if (highlighted.Substring (0, 4).ToLower () == "http") // HTTP or HTTPS URLs
            {
                try
                {
                    System.Diagnostics.Process.Start (new System.Diagnostics.ProcessStartInfo () { FileName = highlighted, UseShellExecute = true });
                    return;
                }
                catch
                {
                    return;
                }
            }

            if (Regex.Matches (highlighted, "^[0-9]+.[0-9]+.[0-9]+.[0-9]+").Count > 0) // IPv4 Addresses
            {
                try
                {
                    System.Diagnostics.Process.Start (new System.Diagnostics.ProcessStartInfo () { FileName = "http://" + highlighted, UseShellExecute = true });
                    return;
                }
                catch
                {
                    return;
                }
            }
        }
    }

    private async void HotKey_ReadAloud ()
    {
        string highlighted = await webView.ExecuteScriptAsync ("window.getSelection ().toString ()");
        highlighted = highlighted.Replace ("\\n", " ");
        
        Task.Factory.StartNew (() => 
        {
            voice.Speak (highlighted);
        });
    }

    private async void HotKey_Refresh ()
    {
        await webView.ExecuteScriptAsync ("location.reload ()");
    }

    private async void HotKey_ExportDocument ()
    {
        string body = await webView.ExecuteScriptAsync ("document.getElementsByClassName ('col-span-3') [0].innerHTML");
        string htmlDocument;

        body = body.Replace ("\\u003C", "<");
        body = body.Replace ("\\n", "");
        body = body.Remove (0, 1);
        body = body.Remove (body.Length - 1, 1);
        body = body.Replace ("\\\"", "\"");

        System.Net.WebClient httpClient = new System.Net.WebClient ();
        string css = httpClient.DownloadString (Config.css);

        htmlDocument = @"
        <!DOCTYPE html>
        <html>
            <head>
                <style>" + css + @"</style>
            </head>
            <body>" + body + @"</body>
        </html>";

        HtmlToPdf converter = new HtmlToPdf ();
        PdfDocument pdf = converter.ConvertHtmlString (htmlDocument);

        System.Windows.Forms.SaveFileDialog saveFileDialogue;
        saveFileDialogue = new System.Windows.Forms.SaveFileDialog ();
        saveFileDialogue.Filter = "PDF Files (*.pdf) | *.pdf";
        System.Windows.Forms.DialogResult result =  saveFileDialogue.ShowDialog ();


        if (result == System.Windows.Forms.DialogResult.OK)
        {
            pdf.Save (saveFileDialogue.FileName);
        }

        pdf.Close ();
    }
}