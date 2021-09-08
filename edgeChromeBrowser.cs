using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;

namespace edgeChromium
{
    public partial class edgeChromeBrowser : Form
    {
        WebView2 _webBrowser;
      
        public edgeChromeBrowser()
        {
            InitializeComponent();
            // this.KeyDown += Form1_KeyDown;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            this.Text = $"Modifier: { e.Modifiers } Key :  { e.KeyCode} ";
        }

        void LoadEdgeChromium()
        {
            _webBrowser = new WebView2
            {
                Visible = true,
                Parent = pnlWebView,
                Dock = DockStyle.Fill,

                Location = new Point(0, 0),
                Size = pnlWebView.Size
            };

            string webview2Runtime = GetWebView2RuntimeVersion();
            if (string.IsNullOrEmpty(webview2Runtime))
            {
                MessageBox.Show("Webview2Runtime not installed");
                DownloadAndInstallWebview2Runtime();
            }
            InitializeAsync();
            _webBrowser.NavigationStarting += _webBrowser_NavigationStarting;
            
        }

        async void InitializeAsync()
        {

            var env = await CoreWebView2Environment.CreateAsync();
            await _webBrowser.EnsureCoreWebView2Async(env);

            _webBrowser.CoreWebView2.Settings.AreBrowserAcceleratorKeysEnabled = false;
            _webBrowser.CoreWebView2.WebMessageReceived += CoreWebView2_WebMessageReceived;                       
            _webBrowser.Source = new Uri("file:///C:/Microsoft-Git/edgeChromium/CheckExternal.html");



            ObjectHelper objectHelper = new ObjectHelper();
            
            _webBrowser.CoreWebView2.AddHostObjectToScript("ChromeBrowser", objectHelper.GetObjectForScripting());

            _webBrowser.CoreWebView2.Settings.IsScriptEnabled = true;
            _webBrowser.CoreWebView2.Settings.AreHostObjectsAllowed = true;      
            _webBrowser.CoreWebView2.OpenDevToolsWindow();

        }

        private void CoreWebView2_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            String uri = e.TryGetWebMessageAsString();
            this.Text = uri;
            _webBrowser.CoreWebView2.PostWebMessageAsString(uri);
        }

        private void edgeBrowser_KeyDown(object sender, KeyEventArgs e)
        {
            this.Text = $"Modifier: { e.Modifiers } Key :  { e.KeyCode} ";

            bool quitKeysPressed = (e.KeyCode == Keys.F10) &&
                    ((e.Modifiers & Keys.Shift) == Keys.Shift) &&
                    ((e.Modifiers & Keys.Alt) == Keys.Alt) &&
                    ((e.Modifiers & Keys.Control) == Keys.Control);

            if (!quitKeysPressed)
            {
                quitKeysPressed = (e.KeyCode == Keys.Q) &&
                    ((e.Modifiers & Keys.Shift) == Keys.Shift) &&
                    ((e.Modifiers & Keys.Control) == Keys.Control);

            }

            if (quitKeysPressed)
            {
                MessageBox.Show($"quitKeysPressed e.Modifiers : { e.Modifiers }");
            }
        }
      
        private void _webBrowser_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            this.Text = $"Modifier: { e.Modifiers } Key :  { e.KeyCode} ";
        }


        private string GetWebView2RuntimeVersion()
        {
            string retValue = string.Empty;
            try
            {
                retValue = CoreWebView2Environment.GetAvailableBrowserVersionString();
            }
            catch (Exception ex)
            {

            }
            return retValue;
        }

        private void DownloadAndInstallWebview2Runtime()
        {
            string webviewRuntimeFilePath = @"C:\edge Chromium\runtime\Webview2Runtime.exe";
            WebClient wc = new WebClient();
            if (!File.Exists(webviewRuntimeFilePath))
            {
                Uri uri = new Uri("https://go.microsoft.com/fwlink/p/?LinkId=2124703");
                wc.DownloadFileTaskAsync(uri, webviewRuntimeFilePath).Wait();
            }
            Process.Start(webviewRuntimeFilePath);
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            if (_webBrowser != null && _webBrowser.CoreWebView2 != null)
            {
                _webBrowser.CoreWebView2.Navigate(txtAddress.Text);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadEdgeChromium();
        }

        private void _webBrowser_NavigationStarting(object sender, CoreWebView2NavigationStartingEventArgs e)
        {
            string domain = GetDomainString(e.Uri);


            //String uri = e.Uri;
            //if (!uri.StartsWith("https://"))
            //{
            //    _webBrowser.CoreWebView2.ExecuteScriptAsync($"alert('{uri} is not safe, try an https link')");
            //    e.Cancel = true;
            //}

            //  MessageBox.Show(domain);
            //if (domain != "www.google.com")
            //{
            //    e.Cancel = true;
            //}
        }

        private string GetDomainString(string url)
        {
            string domainString = string.Empty;

            if (!string.IsNullOrEmpty(url))
            {
                Uri myUri = new Uri(url);
                url = myUri.Host;

                domainString = url;
            }

            return domainString;
        }

        private object GetObjectForScripting()
        {            
            object[] constructorParameters = new object[] { };
            Assembly baAssembly = null;
            baAssembly = Assembly.LoadFile(@"C:\Users\varumugam\source\repos\ReflectionTest\ReflectionTest\bin\Debug\ReflectionTest.dll");
            Type type = baAssembly.GetType("ReflectionTest.ObjectHelper");
            return Activator.CreateInstance(type, null);
        }
    }
}
