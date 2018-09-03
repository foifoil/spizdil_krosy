namespace EveAIO.Views
{
    using EO.WebBrowser;
    using EO.WebEngine;
    using EO.Wpf;
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Markup;

    public class BrowserWindow : Window, IComponentConnector
    {
        internal EO.WebBrowser.WebView _browser1;
        private WebControl _webControl1;
        internal EO.WebBrowser.WebView _browser2;
        private WebControl _webControl2;
        internal EO.WebBrowser.WebView _browser3;
        private WebControl _webControl3;
        internal EO.WebBrowser.WebView _browser4;
        private WebControl _webControl4;
        private CaptchaSolverWindow _solver;
        private Browser1View _browser1View;
        private Browser2View _browser2View;
        private Browser3View _browser3View;
        private Browser4View _browser4View;
        internal ListBox lvMenu;
        internal Button BtnMinimize;
        internal Button BtnClose;
        public System.Windows.Controls.Frame MenusFrame;
        private bool _contentLoaded;

        public BrowserWindow(CaptchaSolverWindow solver, Engine engine)
        {
            Class7.RIuqtBYzWxthF();
            this.InitializeComponent();
            this._browser1View = new Browser1View();
            this._browser2View = new Browser2View();
            this._browser3View = new Browser3View(this);
            this._browser4View = new Browser4View();
            this._solver = solver;
            this._webControl1 = new WebControl();
            this._browser1 = new EO.Wpf.WebView();
            this._browser1.CertificateError += new CertificateErrorHandler(this._browser1_CertificateError);
            this._browser1.Engine = engine;
            this._webControl1.WebView = this._browser1;
            this._browser1View.border1.Child = this._webControl1;
            this._browser1.LoadUrl("https://www.gmail.com");
            this._webControl2 = new WebControl();
            this._browser2 = new EO.Wpf.WebView();
            this._browser2.CertificateError += new CertificateErrorHandler(this._browser2_CertificateError);
            this._browser2.Engine = engine;
            this._webControl2.WebView = this._browser2;
            this._browser2View.border2.Child = this._webControl2;
            this._browser2.LoadUrl("https://www.youtube.com");
            this._webControl3 = new WebControl();
            this._browser3 = new EO.Wpf.WebView();
            this._browser3.CertificateError += new CertificateErrorHandler(this._browser3_CertificateError);
            this._browser3.Engine = engine;
            this._webControl3.WebView = this._browser3;
            this._browser3View.border3.Child = this._webControl3;
            this._webControl4 = new WebControl();
            this._browser4 = new EO.Wpf.WebView();
            this._browser4.CertificateError += new CertificateErrorHandler(this._browser4_CertificateError);
            this._browser4.Engine = engine;
            this._webControl4.WebView = this._browser4;
            this._browser4View.border4.Child = this._webControl4;
            this._browser4.LoadUrl("https://www.whatismyip.com");
            this.MenusFrame.Navigate(this._browser1View);
        }

        private void _browser1_CertificateError(object sender, CertificateErrorEventArgs e)
        {
            e.Continue();
        }

        private void _browser2_CertificateError(object sender, CertificateErrorEventArgs e)
        {
            e.Continue();
        }

        private void _browser3_CertificateError(object sender, CertificateErrorEventArgs e)
        {
            e.Continue();
        }

        private void _browser4_CertificateError(object sender, CertificateErrorEventArgs e)
        {
            e.Continue();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            base.Close();
        }

        private void BtnMaximize_Click(object sender, RoutedEventArgs e)
        {
            base.WindowState = WindowState.Maximized;
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            base.WindowState = WindowState.Minimized;
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // This item is obfuscated and can not be translated.
            if (e.ClickCount != 2)
            {
                goto Label_002D;
            }
        Label_000E:
            switch (((-1318711395 ^ -1135223349) % 4))
            {
                case 0:
                    return;

                case 1:
                    break;

                case 2:
                    base.WindowState = (base.WindowState == WindowState.Normal) ? WindowState.Maximized : WindowState.Normal;
                    return;

                case 3:
                    goto Label_000E;

                default:
                    return;
            }
        Label_002D:
            base.DragMove();
            goto Label_000E;
        }

        [DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            // This item is obfuscated and can not be translated.
            if (!this._contentLoaded)
            {
                goto Label_002C;
            }
        Label_000D:
            switch (((-419191216 ^ -744367763) % 4))
            {
                case 0:
                    break;

                case 1:
                    return;

                case 3:
                    goto Label_000D;

                default:
                {
                    Uri resourceLocator = new Uri("/EveAIO;component/views/browserwindow.xaml", UriKind.Relative);
                    Application.LoadComponent(this, resourceLocator);
                    return;
                }
            }
        Label_002C:
            this._contentLoaded = true;
            goto Label_000D;
        }

        private void lvMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // This item is obfuscated and can not be translated.
            switch (this.lvMenu.SelectedIndex)
            {
                case 0:
                    break;

                case 1:
                    goto Label_006F;

                case 2:
                    goto Label_008B;

                case 3:
                    goto Label_009E;

                default:
                    goto Label_0040;
            }
        Label_0029:
            this.MenusFrame.Navigate(this._browser1View);
        Label_0040:
            switch (((-1096657653 ^ -410040977) % 8))
            {
                case 0:
                    goto Label_0029;

                case 1:
                    return;

                case 2:
                    break;

                case 3:
                    return;

                case 4:
                    return;

                case 5:
                    goto Label_0040;

                case 7:
                    goto Label_008B;

                default:
                    goto Label_009E;
            }
        Label_006F:
            this.MenusFrame.Navigate(this._browser2View);
            goto Label_0040;
        Label_008B:
            this.MenusFrame.Navigate(this._browser3View);
            return;
        Label_009E:
            this.MenusFrame.Navigate(this._browser4View);
        }

        [GeneratedCode("PresentationBuildTasks", "4.0.0.0"), EditorBrowsable(EditorBrowsableState.Never), DebuggerNonUserCode]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    ((Grid) target).MouseLeftButtonDown += new MouseButtonEventHandler(this.Grid_MouseLeftButtonDown);
                    return;

                case 2:
                    this.lvMenu = (ListBox) target;
                    this.lvMenu.SelectionChanged += new SelectionChangedEventHandler(this.lvMenu_SelectionChanged);
                    return;

                case 3:
                    this.BtnMinimize = (Button) target;
                    this.BtnMinimize.Click += new RoutedEventHandler(this.BtnMinimize_Click);
                    return;

                case 4:
                    this.BtnClose = (Button) target;
                    this.BtnClose.Click += new RoutedEventHandler(this.BtnClose_Click);
                    return;

                case 5:
                    this.MenusFrame = (System.Windows.Controls.Frame) target;
                    return;
            }
            this._contentLoaded = true;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this._solver.BROWSER = null;
            this._browser1.Dispose();
            this._browser2.Dispose();
            this._browser3.Dispose();
            this._browser4.Dispose();
        }
    }
}

