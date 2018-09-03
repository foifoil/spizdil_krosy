namespace EveAIO.Views
{
    using EO.Base;
    using EO.WebBrowser;
    using EO.WebEngine;
    using EO.Wpf;
    using EveAIO;
    using EveAIO.Captcha;
    using EveAIO.Pocos;
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Markup;

    public class CaptchaSolverWindow : Window, IComponentConnector
    {
        public BrowserWindow BROWSER;
        private EO.WebBrowser.WebView _browser;
        private WebControl _webControl;
        private Engine _engine;
        private string m_siteKey;
        private string m_sitePath;
        private readonly object m_lock;
        private readonly object assign_lock;
        private SolverSite _solverSite;
        private TaskObject _task;
        private static bool _loadCaptcha;
        private string _name;
        public bool IsFree;
        public bool IsUserEnabled;
        public bool IsHardClose;
        internal TextBlock txtTitle;
        internal Button BtnMinimize;
        internal Button BtnClose;
        internal Button btnBrowser;
        internal TextBlock lblCount;
        internal Border border;
        private bool _contentLoaded;

        static CaptchaSolverWindow()
        {
            Class7.RIuqtBYzWxthF();
        }

        public CaptchaSolverWindow()
        {
            Class7.RIuqtBYzWxthF();
            this.m_lock = new object();
            this.assign_lock = new object();
            this.IsFree = true;
            this.IsUserEnabled = true;
            this.InitializeComponent();
        }

        public CaptchaSolverWindow(string name)
        {
            Class7.RIuqtBYzWxthF();
            this.m_lock = new object();
            this.assign_lock = new object();
            this.IsFree = true;
            this.IsUserEnabled = true;
            this.InitializeComponent();
            this._name = name;
            switch (this._name)
            {
                case "window1":
                    this.IsUserEnabled = Global.SETTINGS.Solver1Enabled;
                    break;

                case "window2":
                    this.IsUserEnabled = Global.SETTINGS.Solver2Enabled;
                    break;

                case "window3":
                    this.IsUserEnabled = Global.SETTINGS.Solver3Enabled;
                    break;

                case "window4":
                    this.IsUserEnabled = Global.SETTINGS.Solver4Enabled;
                    break;

                case "window5":
                    this.IsUserEnabled = Global.SETTINGS.Solver5Enabled;
                    break;
            }
            WebControl control1 = new WebControl {
                Width = 410.0,
                Height = 540.0
            };
            this._webControl = control1;
            this.border.Width = 410.0;
            this.border.Height = 540.0;
            this._engine = Engine.Create(this._name);
            this._engine.Options.CachePath = AppDomain.CurrentDomain.BaseDirectory + @"\Cache\" + this._name;
            string str = this._name;
            switch (str)
            {
                case "window2":
                {
                    if (string.IsNullOrEmpty(Global.SETTINGS.Solver2ProxyList) || !Global.SETTINGS.PROXIES.Any<ProxyListObject>(x => (x.Id == Global.SETTINGS.Solver2ProxyList)))
                    {
                        this.txtTitle.Text = this.txtTitle.Text + " - local IP";
                        break;
                    }
                    ProxyObject proxy = Helpers.GetProxy(Global.SETTINGS.PROXIES.First<ProxyListObject>(x => x.Id == Global.SETTINGS.Solver2ProxyList));
                    if (proxy != null)
                    {
                        if (string.IsNullOrEmpty(proxy.Username))
                        {
                            ProxyInfo info2 = new ProxyInfo(ProxyType.HTTP, proxy.IP, proxy.Port);
                            this._engine.Options.Proxy = info2;
                        }
                        else
                        {
                            ProxyInfo info9 = new ProxyInfo(ProxyType.HTTP, proxy.IP, proxy.Port, proxy.Username, proxy.Password);
                            this._engine.Options.Proxy = info9;
                        }
                        this.txtTitle.Text = this.txtTitle.Text + " - " + proxy.IP;
                    }
                    break;
                }
                case "window3":
                {
                    if (string.IsNullOrEmpty(Global.SETTINGS.Solver3ProxyList) || !Global.SETTINGS.PROXIES.Any<ProxyListObject>(x => (x.Id == Global.SETTINGS.Solver3ProxyList)))
                    {
                        this.txtTitle.Text = this.txtTitle.Text + " - local IP";
                        break;
                    }
                    ProxyObject proxy = Helpers.GetProxy(Global.SETTINGS.PROXIES.First<ProxyListObject>(x => x.Id == Global.SETTINGS.Solver3ProxyList));
                    if (proxy != null)
                    {
                        if (string.IsNullOrEmpty(proxy.Username))
                        {
                            ProxyInfo info3 = new ProxyInfo(ProxyType.HTTP, proxy.IP, proxy.Port);
                            this._engine.Options.Proxy = info3;
                        }
                        else
                        {
                            ProxyInfo info = new ProxyInfo(ProxyType.HTTP, proxy.IP, proxy.Port, proxy.Username, proxy.Password);
                            this._engine.Options.Proxy = info;
                        }
                        this.txtTitle.Text = this.txtTitle.Text + " - " + proxy.IP;
                    }
                    break;
                }
                case "window4":
                {
                    if (string.IsNullOrEmpty(Global.SETTINGS.Solver4ProxyList) || !Global.SETTINGS.PROXIES.Any<ProxyListObject>(x => (x.Id == Global.SETTINGS.Solver4ProxyList)))
                    {
                        this.txtTitle.Text = this.txtTitle.Text + " - local IP";
                        break;
                    }
                    ProxyObject proxy = Helpers.GetProxy(Global.SETTINGS.PROXIES.First<ProxyListObject>(x => x.Id == Global.SETTINGS.Solver4ProxyList));
                    if (proxy != null)
                    {
                        if (string.IsNullOrEmpty(proxy.Username))
                        {
                            ProxyInfo info4 = new ProxyInfo(ProxyType.HTTP, proxy.IP, proxy.Port);
                            this._engine.Options.Proxy = info4;
                        }
                        else
                        {
                            ProxyInfo info8 = new ProxyInfo(ProxyType.HTTP, proxy.IP, proxy.Port, proxy.Username, proxy.Password);
                            this._engine.Options.Proxy = info8;
                        }
                        this.txtTitle.Text = this.txtTitle.Text + " - " + proxy.IP;
                    }
                    break;
                }
                case "window1":
                    if (!string.IsNullOrEmpty(Global.SETTINGS.Solver1ProxyList) && Global.SETTINGS.PROXIES.Any<ProxyListObject>(x => (x.Id == Global.SETTINGS.Solver1ProxyList)))
                    {
                        ProxyObject proxy = Helpers.GetProxy(Global.SETTINGS.PROXIES.First<ProxyListObject>(x => x.Id == Global.SETTINGS.Solver1ProxyList));
                        if (proxy != null)
                        {
                            if (!string.IsNullOrEmpty(proxy.Username))
                            {
                                ProxyInfo info5 = new ProxyInfo(ProxyType.HTTP, proxy.IP, proxy.Port, proxy.Username, proxy.Password);
                                this._engine.Options.Proxy = info5;
                            }
                            else
                            {
                                ProxyInfo info10 = new ProxyInfo(ProxyType.HTTP, proxy.IP, proxy.Port);
                                this._engine.Options.Proxy = info10;
                            }
                            this.txtTitle.Text = this.txtTitle.Text + " - " + proxy.IP;
                        }
                    }
                    else
                    {
                        this.txtTitle.Text = this.txtTitle.Text + " - local IP";
                    }
                    break;

                default:
                    if (str == "window5")
                    {
                        if (!string.IsNullOrEmpty(Global.SETTINGS.Solver5ProxyList) && Global.SETTINGS.PROXIES.Any<ProxyListObject>(x => (x.Id == Global.SETTINGS.Solver5ProxyList)))
                        {
                            ProxyObject proxy = Helpers.GetProxy(Global.SETTINGS.PROXIES.First<ProxyListObject>(x => x.Id == Global.SETTINGS.Solver5ProxyList));
                            if (proxy != null)
                            {
                                if (!string.IsNullOrEmpty(proxy.Username))
                                {
                                    ProxyInfo info7 = new ProxyInfo(ProxyType.HTTP, proxy.IP, proxy.Port, proxy.Username, proxy.Password);
                                    this._engine.Options.Proxy = info7;
                                }
                                else
                                {
                                    ProxyInfo info6 = new ProxyInfo(ProxyType.HTTP, proxy.IP, proxy.Port);
                                    this._engine.Options.Proxy = info6;
                                }
                                this.txtTitle.Text = this.txtTitle.Text + " - " + proxy.IP;
                            }
                        }
                        else
                        {
                            this.txtTitle.Text = this.txtTitle.Text + " - local IP";
                        }
                    }
                    break;
            }
            this._browser = new EO.WebBrowser.WebView();
            this._browser.Engine = this._engine;
            BrowserOptions options = new BrowserOptions {
                AllowJavaScript = true,
                AllowJavaScriptAccessClipboard = true,
                AllowJavaScriptDOMPaste = true,
                EnableXSSAuditor = false,
                EnableWebSecurity = false
            };
            this._browser.SetOptions(options);
            this._webControl.WebView = this._browser;
            this._browser.CertificateError += new CertificateErrorHandler(this._browser_CertificateError);
            this._browser.RegisterJSExtensionFunction("submit", new JSExtInvokeHandler(this.Solved));
            this.border.Child = this._webControl;
            this._browser.LoadCompleted += new LoadCompletedEventHandler(this._browser_LoadCompleted);
            this.Change();
        }

        private void _browser_CertificateError(object sender, CertificateErrorEventArgs e)
        {
            e.Continue();
        }

        private void _browser_LoadCompleted(object sender, LoadCompletedEventArgs e)
        {
        }

        private void btnBrowser_Click(object sender, RoutedEventArgs e)
        {
            Mouse.GetPosition(Application.Current.MainWindow);
            if (this.BROWSER == null)
            {
                this.BROWSER = new BrowserWindow(this, this._engine);
                this.BROWSER.Show();
            }
            else
            {
                this.BROWSER.Activate();
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            switch (this._name)
            {
                case "window1":
                    Global.CaptchaSolver1Opened = false;
                    break;

                case "window2":
                    Global.CaptchaSolver2Opened = false;
                    break;

                case "window3":
                    Global.CaptchaSolver3Opened = false;
                    break;

                case "window4":
                    Global.CaptchaSolver4Opened = false;
                    break;

                case "window5":
                    Global.CaptchaSolver5Opened = false;
                    break;
            }
            base.Hide();
        }

        private void BtnMaximize_Click(object sender, RoutedEventArgs e)
        {
            base.WindowState = WindowState.Maximized;
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            base.WindowState = WindowState.Minimized;
        }

        public void Change()
        {
            this.lblCount.Dispatcher.BeginInvoke(() => this.lblCount.Text = Global.CAPTCHA_QUEUE.Count<TaskObject>(x => !x.ManualSolved).ToString(), Array.Empty<object>());
            object obj2 = Global.CAPTCHA_ASSIGN_LOCK;
            lock (obj2)
            {
                if ((this._task == null) && Global.CAPTCHA_QUEUE.Any<TaskObject>(x => (!x.ManualSolved && !x.CaptchaSolverAssigned)))
                {
                    this.IsFree = false;
                    this._task = Global.CAPTCHA_QUEUE.First<TaskObject>(x => !x.ManualSolved);
                    this._task.CaptchaSolverAssigned = true;
                    this._task.CaptchaWindow = this;
                }
                else
                {
                    if (Global.CAPTCHA_QUEUE.Any<TaskObject>(x => !x.ManualSolved && !x.CaptchaSolverAssigned))
                    {
                        return;
                    }
                    this._task = null;
                }
                if (this._task != null)
                {
                    this.IsFree = false;
                }
            }
            if (this._task != null)
            {
                this.Reload();
            }
            else
            {
                this.IsFree = true;
                this._webControl.Dispatcher.BeginInvoke(() => this._browser.LoadHtml($"<!DOCTYPE html><html><head><style>{LoaderHtmlElement.Style.Replace("$body$", Global.SETTINGS.EnvLight ? "#F4F5F8" : "#1E1E1E").Replace("$h2$", Global.SETTINGS.EnvLight ? "#111111" : "#EEEEEE").Replace("$h3$", Global.SETTINGS.EnvLight ? "#111111" : "#EEEEEE")}</style><center><h2>WAITING FOR CAPTCHA REQUESTS</h2></center></body></html>", "http://eveaio.com"), Array.Empty<object>());
            }
        }

        private void Grid_MouseLeftButtonDown()
        {
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.DragMove();
        }

        [GeneratedCode("PresentationBuildTasks", "4.0.0.0"), DebuggerNonUserCode]
        public void InitializeComponent()
        {
            // This item is obfuscated and can not be translated.
            Uri uri;
            if (!this._contentLoaded)
            {
                goto Label_002C;
            }
        Label_000D:
            switch (((-820314253 ^ -583829314) % 4))
            {
                case 0:
                    goto Label_000D;

                case 1:
                    return;

                case 3:
                    break;

                default:
                    Application.LoadComponent(this, uri);
                    return;
            }
        Label_002C:
            this._contentLoaded = true;
            uri = new Uri("/EveAIO;component/views/captchasolverwindow.xaml", UriKind.Relative);
            goto Label_000D;
        }

        [DllImport("user32.dll", CallingConvention=CallingConvention.StdCall, CharSet=CharSet.Auto)]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
        public void Reload()
        {
            try
            {
                _loadCaptcha = true;
                object @lock = this.m_lock;
                lock (@lock)
                {
                    if (base.WindowState == WindowState.Minimized)
                    {
                        base.WindowState = WindowState.Normal;
                    }
                    base.Activate();
                    if (Global.SETTINGS.SolverBeep)
                    {
                        Helpers.PlayBell();
                    }
                    if (this._task.Platform != TaskObject.PlatformEnum.sneakersnstuff)
                    {
                        if ((this._task.Platform != TaskObject.PlatformEnum.supreme) && (this._task.Platform != TaskObject.PlatformEnum.supremeinstore))
                        {
                            if (this._task.Platform != TaskObject.PlatformEnum.mrporter)
                            {
                                if (this._task.Platform == TaskObject.PlatformEnum.sivasdescalzo)
                                {
                                    this._solverSite = SolverSite.SIVAS;
                                }
                                else if (this._task.Platform != TaskObject.PlatformEnum.hibbett)
                                {
                                    if (this._task.Platform == TaskObject.PlatformEnum.offwhite)
                                    {
                                        this._solverSite = SolverSite.OW;
                                    }
                                    else if (this._task.Platform != TaskObject.PlatformEnum.footaction)
                                    {
                                        if (this._task.Platform == TaskObject.PlatformEnum.privacy)
                                        {
                                            this._solverSite = SolverSite.PRIVACY;
                                        }
                                        else if (this._task.Platform != TaskObject.PlatformEnum.holypopstore)
                                        {
                                            this._solverSite = SolverSite.SHOPIFY;
                                        }
                                        else
                                        {
                                            this._solverSite = SolverSite.HOLYPOP;
                                        }
                                    }
                                    else
                                    {
                                        this._solverSite = SolverSite.FOOTACTION;
                                    }
                                }
                                else
                                {
                                    this._solverSite = SolverSite.HIBBETT;
                                }
                            }
                            else
                            {
                                this._solverSite = SolverSite.MRPORTER;
                            }
                        }
                        else
                        {
                            this._solverSite = SolverSite.SUPREME;
                        }
                    }
                    else
                    {
                        this._solverSite = SolverSite.SNS;
                    }
                    switch (this._solverSite)
                    {
                        case SolverSite.SHOPIFY:
                            this.m_sitePath = this._task.HomeUrl;
                            this.m_siteKey = Global.SETTINGS.ShopifyCaptchaKey;
                            break;

                        case SolverSite.SNS:
                            this.m_sitePath = "https://www.sneakersnstuff.com/";
                            this.m_siteKey = Global.SETTINGS.SnsCaptchaKey;
                            break;

                        case SolverSite.SUPREME:
                            this.m_sitePath = "http://www.supremenewyork.com";
                            this.m_siteKey = Global.SETTINGS.SupremeCaptchaKey;
                            break;

                        case SolverSite.MRPORTER:
                            this.m_sitePath = "https://www.mrporter.com";
                            this.m_siteKey = WebsitesInfo.MR_PORTER_CAPTCHA_KEY;
                            break;

                        case SolverSite.SIVAS:
                            this.m_sitePath = "https://www.sivasdescalzo.com";
                            this.m_siteKey = WebsitesInfo.SIVAS_CAPTCHA_KEY;
                            break;

                        case SolverSite.HIBBETT:
                            this.m_sitePath = "https://www.hibbett.com";
                            this.m_siteKey = WebsitesInfo.HIBBET_CAPTCHA_KEY;
                            break;

                        case SolverSite.OW:
                            this.m_sitePath = "https://www.off---white.com";
                            this.m_siteKey = WebsitesInfo.OW_CAPTCHA_KEY;
                            break;

                        case SolverSite.FOOTACTION:
                            this.m_sitePath = "https://www.footaction.com";
                            this.m_siteKey = WebsitesInfo.FOOTACTION_CAPTCHA_KEY;
                            break;

                        case SolverSite.PRIVACY:
                            this.m_sitePath = "https://privacy.com";
                            this.m_siteKey = WebsitesInfo.PRIVACY_CAPTCHA_KEY;
                            break;

                        case SolverSite.HOLYPOP:
                            this.m_sitePath = "https://www.holypopstore.com";
                            this.m_siteKey = WebsitesInfo.HOLYPOP_CAPTCHA_KEY;
                            break;
                    }
                    if (((Global.ASM == null) || string.IsNullOrEmpty(Global.SERIAL)) || Global.SERIAL.Contains("11111"))
                    {
                        this.m_siteKey = "fjeifjei!!F3";
                    }
                    this._browser.LoadHtml($"<!DOCTYPE html><html><head><style>{LoaderHtmlElement.Style.Replace("$body$", Global.SETTINGS.EnvLight ? "#F4F5F8" : "#1E1E1E").Replace("$h2$", Global.SETTINGS.EnvLight ? "#111111" : "#EEEEEE").Replace("$h3$", Global.SETTINGS.EnvLight ? "#111111" : "#EEEEEE")}</style></head><body onload='click()'>" + $"<center><h2>Solving captchas for:</h2><h3>{this.m_sitePath}</h3><div class="g-recaptcha" data-sitekey="{this.m_siteKey}"  data-callback="submit"" + "</div></center><script type='text/javascript' src='https://www.google.com/recaptcha/api.js'></script><script>function click(){window.frames[0].document.getElementsByClassName('recaptcha-checkbox-checkmark')[0].click();}</script></body></html>", this.m_sitePath);
                }
            }
            catch (Exception exception)
            {
                Global.Logger.Error("Error in manual solver accured", exception);
                MessageBox.Show("Error in manual solver accured", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                base.Close();
            }
        }

        [DllImport("user32")]
        public static extern int SetCursorPos(int x, int y);
        public void Solved(object sender, JSExtInvokeArgs e)
        {
            string str = e.Arguments[0].ToString();
            CaptchaToken item = new CaptchaToken {
                Id = Guid.NewGuid().ToString(),
                Created = DateTime.Now,
                CaptchaType = CaptchaSolver.CaptchaService.Manual,
                Website = this.m_sitePath
            };
            item.Token = str;
            item.Timestamp = DateTime.Now;
            item.Expires = item.Timestamp.AddMinutes(2.0);
            TimeSpan span = (TimeSpan) (item.Timestamp - item.Created);
            item.SolveTime = (span.Minutes * 60) + span.Seconds;
            this._task.ManualSolved = true;
            this._task.CaptchaWindow = null;
            this._task = null;
            switch (this._solverSite)
            {
                case SolverSite.SHOPIFY:
                    item.Platform = "Shopify";
                    Global.ShopifyTokens.Add(item);
                    break;

                case SolverSite.SNS:
                    item.Platform = "Sneakernstuff";
                    Global.SnsTokens.Add(item);
                    break;

                case SolverSite.SUPREME:
                    item.Platform = "Supreme";
                    Global.SupremeTokens.Add(item);
                    break;

                case SolverSite.MRPORTER:
                    item.Platform = "MrPorter";
                    Global.MrPorterTokens.Add(item);
                    break;

                case SolverSite.SIVAS:
                    item.Platform = "Sivas";
                    Global.SivasTokens.Add(item);
                    break;

                case SolverSite.HIBBETT:
                    item.Platform = "Hibbett";
                    Global.HibbettTokens.Add(item);
                    break;

                case SolverSite.OW:
                    item.Platform = "OW";
                    Global.OWTokens.Add(item);
                    break;

                case SolverSite.FOOTACTION:
                    item.Platform = "Footaction";
                    Global.FootactionTokens.Add(item);
                    break;

                case SolverSite.PRIVACY:
                    item.Platform = "Privacy";
                    Global.PrivacyTokens.Add(item);
                    break;

                case SolverSite.HOLYPOP:
                    item.Platform = "Holypopstore";
                    Global.HolypopTokens.Add(item);
                    break;
            }
            this.IsFree = true;
            this.Change();
        }

        public void Stop(bool clearCache)
        {
            this._engine.Stop(clearCache);
        }

        [DebuggerNonUserCode, EditorBrowsable(EditorBrowsableState.Never), GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    ((CaptchaSolverWindow) target).Closing += new CancelEventHandler(this.Window_Closing);
                    return;

                case 2:
                    ((Grid) target).MouseLeftButtonDown += new MouseButtonEventHandler(this.Grid_MouseLeftButtonDown);
                    return;

                case 3:
                    this.txtTitle = (TextBlock) target;
                    return;

                case 4:
                    this.BtnMinimize = (Button) target;
                    this.BtnMinimize.Click += new RoutedEventHandler(this.BtnMinimize_Click);
                    return;

                case 5:
                    this.BtnClose = (Button) target;
                    this.BtnClose.Click += new RoutedEventHandler(this.BtnClose_Click);
                    return;

                case 6:
                    this.btnBrowser = (Button) target;
                    this.btnBrowser.Click += new RoutedEventHandler(this.btnBrowser_Click);
                    return;

                case 7:
                    this.lblCount = (TextBlock) target;
                    return;

                case 8:
                    this.border = (Border) target;
                    return;
            }
            this._contentLoaded = true;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            switch (this._name)
            {
                case "window1":
                    Global.CaptchaSolver1Opened = false;
                    break;

                case "window2":
                    Global.CaptchaSolver2Opened = false;
                    break;

                case "window3":
                    Global.CaptchaSolver3Opened = false;
                    break;

                case "window4":
                    Global.CaptchaSolver4Opened = false;
                    break;

                case "window5":
                    Global.CaptchaSolver5Opened = false;
                    break;
            }
            base.Hide();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (!this.IsHardClose)
            {
                e.Cancel = true;
                switch (this._name)
                {
                    case "window1":
                        Global.CaptchaSolver1Opened = false;
                        break;

                    case "window2":
                        Global.CaptchaSolver2Opened = false;
                        break;

                    case "window3":
                        Global.CaptchaSolver3Opened = false;
                        break;

                    case "window4":
                        Global.CaptchaSolver4Opened = false;
                        break;

                    case "window5":
                        Global.CaptchaSolver5Opened = false;
                        break;
                }
                base.Hide();
            }
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly CaptchaSolverWindow.<>c <>9;
            public static Func<ProxyListObject, bool> <>9__19_0;
            public static Func<ProxyListObject, bool> <>9__19_5;
            public static Func<ProxyListObject, bool> <>9__19_1;
            public static Func<ProxyListObject, bool> <>9__19_6;
            public static Func<ProxyListObject, bool> <>9__19_2;
            public static Func<ProxyListObject, bool> <>9__19_7;
            public static Func<ProxyListObject, bool> <>9__19_3;
            public static Func<ProxyListObject, bool> <>9__19_8;
            public static Func<ProxyListObject, bool> <>9__19_4;
            public static Func<ProxyListObject, bool> <>9__19_9;
            public static Func<TaskObject, bool> <>9__28_2;
            public static Func<TaskObject, bool> <>9__28_3;
            public static Func<TaskObject, bool> <>9__28_4;
            public static Func<TaskObject, bool> <>9__28_5;

            static <>c()
            {
                Class7.RIuqtBYzWxthF();
                <>9 = new CaptchaSolverWindow.<>c();
            }

            public <>c()
            {
                Class7.RIuqtBYzWxthF();
            }

            internal bool <.ctor>b__19_0(ProxyListObject x) => 
                (x.Id == Global.SETTINGS.Solver1ProxyList);

            internal bool <.ctor>b__19_1(ProxyListObject x) => 
                (x.Id == Global.SETTINGS.Solver2ProxyList);

            internal bool <.ctor>b__19_2(ProxyListObject x) => 
                (x.Id == Global.SETTINGS.Solver3ProxyList);

            internal bool <.ctor>b__19_3(ProxyListObject x) => 
                (x.Id == Global.SETTINGS.Solver4ProxyList);

            internal bool <.ctor>b__19_4(ProxyListObject x) => 
                (x.Id == Global.SETTINGS.Solver5ProxyList);

            internal bool <.ctor>b__19_5(ProxyListObject x) => 
                (x.Id == Global.SETTINGS.Solver1ProxyList);

            internal bool <.ctor>b__19_6(ProxyListObject x) => 
                (x.Id == Global.SETTINGS.Solver2ProxyList);

            internal bool <.ctor>b__19_7(ProxyListObject x) => 
                (x.Id == Global.SETTINGS.Solver3ProxyList);

            internal bool <.ctor>b__19_8(ProxyListObject x) => 
                (x.Id == Global.SETTINGS.Solver4ProxyList);

            internal bool <.ctor>b__19_9(ProxyListObject x) => 
                (x.Id == Global.SETTINGS.Solver5ProxyList);

            internal bool <Change>b__28_2(TaskObject x) => 
                !x.ManualSolved;

            internal bool <Change>b__28_3(TaskObject x) => 
                (!x.ManualSolved && !x.CaptchaSolverAssigned);

            internal bool <Change>b__28_4(TaskObject x) => 
                !x.ManualSolved;

            internal bool <Change>b__28_5(TaskObject x) => 
                (!x.ManualSolved && !x.CaptchaSolverAssigned);
        }

        private enum SolverSite
        {
            SHOPIFY,
            SNS,
            SUPREME,
            MRPORTER,
            SIVAS,
            HIBBETT,
            OW,
            FOOTACTION,
            PRIVACY,
            HOLYPOP
        }
    }
}

