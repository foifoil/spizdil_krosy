namespace EveAIO
{
    using EO.WebBrowser;
    using EveAIO.Pocos;
    using EveAIO.Tasks;
    using EveAIO.Views;
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Markup;
    using System.Windows.Threading;
    using wyDay.Controls;

    public class MainWindow : Window, IComponentConnector
    {
        private DispatcherTimer _versionTimer;
        private DispatcherTimer _licenseTimer;
        internal ListBox lvMenu;
        internal Button BtnMinimize;
        internal Button BtnMaximize;
        internal Button BtnClose;
        public AutomaticUpdater updater;
        public System.Windows.Controls.Frame MenusFrame;
        public TextBlock txtTotalTasks;
        public TextBlock txtTotalProfiles;
        public TextBlock txtTotalProxyLists;
        public TextBlock txtTotalProxies;
        public TextBlock lblVersion;
        internal TextBlock txtVersion;
        private bool _contentLoaded;

        public MainWindow()
        {
            Class7.RIuqtBYzWxthF();
            this.InitializeComponent();
            Global.MAIN_WINDOW = this;
            this.MenusFrame.Navigate(Global.ViewDashboard);
            this.lvMenu.SelectedIndex = 0;
            this.txtTotalTasks.Text = "Total tasks: " + Global.SETTINGS.TASKS.Count;
            this.txtTotalProfiles.Text = "Total profiles: " + Global.SETTINGS.PROFILES.Count;
            this.txtTotalProxyLists.Text = "Total proxy lists: " + Global.SETTINGS.PROXIES.Count;
            int num2 = 0;
            foreach (ProxyListObject obj4 in Global.SETTINGS.PROXIES)
            {
                num2 += obj4.Proxies.Count;
            }
            this.txtTotalProxies.Text = "Total proxies: " + num2;
            EveAIO.Helpers.Notify("logon", Global.VERSION + " | " + Global.IP + "|" + Global.TmpKey);
            if (!Directory.Exists("checkouts"))
            {
                Directory.CreateDirectory("checkouts");
            }
            if (!Directory.Exists("Cache"))
            {
                Directory.CreateDirectory("Cache");
            }
            this._versionTimer = new DispatcherTimer();
            this._versionTimer.Interval = new TimeSpan(0, 0, 10);
            this._versionTimer.Tick += new EventHandler(this._versionTimer_Tick);
            this._versionTimer.Start();
            this._licenseTimer = new DispatcherTimer();
            this._licenseTimer.Interval = new TimeSpan(0, 4, 0);
            this._licenseTimer.Tick += new EventHandler(this._licenseTimer_Tick);
            this._licenseTimer.Start();
            Global.SERIAL64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(Global.SERIAL));
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
            ServicePointManager.DefaultConnectionLimit = 0x1388;
            ServicePointManager.MaxServicePointIdleTime = 0x1388;
            ServicePointManager.MaxServicePoints = 0x3e8;
            Type type = Global.ASM.GetType("SvcHost.SvcHost");
            MethodInfo method = type.GetMethod("Efkf3kfo_");
            object obj2 = Activator.CreateInstance(type);
            Runtime.AddLicense((string) method.Invoke(obj2, null));
            Type type2 = Global.ASM.GetType("SvcHost.SvcHost");
            MethodInfo info2 = type2.GetMethod("JeokfEp3");
            object obj3 = Activator.CreateInstance(type2);
            WebsitesInfo.SUPPORTED_PLATFORMS = (List<KeyValuePair<string, string>>) info2.Invoke(obj3, null);
            Global.CAPTCHA_SOLVER1 = new CaptchaSolverWindow("window1");
            Global.CAPTCHA_SOLVER1.Visibility = Visibility.Hidden;
            Global.CAPTCHA_SOLVER1.Show();
            Global.CAPTCHA_SOLVER1.Hide();
            Global.CAPTCHA_SOLVER2 = new CaptchaSolverWindow("window2");
            Global.CAPTCHA_SOLVER2.Visibility = Visibility.Hidden;
            Global.CAPTCHA_SOLVER2.Show();
            Global.CAPTCHA_SOLVER2.Hide();
            Global.CAPTCHA_SOLVER3 = new CaptchaSolverWindow("window3");
            Global.CAPTCHA_SOLVER3.Visibility = Visibility.Hidden;
            Global.CAPTCHA_SOLVER3.Show();
            Global.CAPTCHA_SOLVER3.Hide();
            Global.CAPTCHA_SOLVER4 = new CaptchaSolverWindow("window4");
            Global.CAPTCHA_SOLVER4.Visibility = Visibility.Hidden;
            Global.CAPTCHA_SOLVER4.Show();
            Global.CAPTCHA_SOLVER4.Hide();
            Global.CAPTCHA_SOLVER5 = new CaptchaSolverWindow("window5");
            Global.CAPTCHA_SOLVER5.Visibility = Visibility.Hidden;
            Global.CAPTCHA_SOLVER5.Show();
            Global.CAPTCHA_SOLVER5.Hide();
            Global.SENSOR = new Sensor();
            if (!Global.AsmLoaded)
            {
                List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
                foreach (KeyValuePair<string, string> pair in WebsitesInfo.SUPPORTED_PLATFORMS)
                {
                    list.Add(new KeyValuePair<string, string>("", pair.Value));
                }
                WebsitesInfo.SUPPORTED_PLATFORMS = list;
            }
        }

        private void _licenseTimer_Tick(object sender, EventArgs e)
        {
            Task.Factory.StartNew(delegate {
                try
                {
                    if (EveAIO.License.Check(Global.SERIAL) == EveAIO.License.Result.success)
                    {
                        return;
                    }
                    goto Label_0053;
                Label_000F:
                    Task.Factory.StartNew(delegate {
                        Thread.Sleep(0x1388);
                        Environment.Exit(0);
                    });
                    MessageBox.Show("Opps, your key isn't activated, Eve will close in 5 seconds", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                Label_0053:
                    switch ((-1106914262 ^ -2129080893) % 3)
                    {
                        case 0:
                            goto Label_0053;

                        case 1:
                            goto Label_000F;

                        case 2:
                            return;
                    }
                }
                catch
                {
                }
            });
        }

        private void _versionTimer_Tick(object sender, EventArgs e)
        {
            this._versionTimer.Stop();
            TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
            Task.Factory.StartNew(delegate {
                try
                {
                    Process process = Process.Start(Environment.CurrentDirectory + @"\wyUpdate.exe", "/quickcheck /justcheck /noerr");
                    while (!process.HasExited)
                    {
                    }
                    this.lblVersion.Dispatcher.BeginInvoke(delegate {
                        if (process.ExitCode == 2)
                        {
                            this.lblVersion.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            this.lblVersion.Visibility = Visibility.Collapsed;
                        }
                    }, Array.Empty<object>());
                }
                catch
                {
                }
            }).ContinueWith(delegate (Task t) {
            }, scheduler);
            this._versionTimer.Interval = new TimeSpan(0, 15, 0);
            this._versionTimer.Start();
        }

        private void BtnAbout_Click(object sender, RoutedEventArgs e)
        {
            this.MenusFrame.Navigate(Global.ViewTools);
        }

        private void BtnCapatchaPanel_Click(object sender, RoutedEventArgs e)
        {
            this.MenusFrame.Navigate(Global.ViewCaptcha);
            Global.ViewCaptcha.Init();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            base.Close();
        }

        private void BtnLogs_Click(object sender, RoutedEventArgs e)
        {
            this.MenusFrame.Navigate(Global.ViewLog);
        }

        private void BtnMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (base.WindowState != WindowState.Maximized)
            {
                base.WindowState = WindowState.Maximized;
            }
            else
            {
                base.WindowState = WindowState.Normal;
            }
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            base.WindowState = WindowState.Minimized;
        }

        private void BtnNotifications_Click(object sender, RoutedEventArgs e)
        {
            this.MenusFrame.Navigate(Global.ViewNotifications);
        }

        private void BtnProfiles_Click(object sender, RoutedEventArgs e)
        {
            this.MenusFrame.Navigate(Global.ViewProfiles);
        }

        private void BtnProxyManager_Click(object sender, RoutedEventArgs e)
        {
            this.MenusFrame.Navigate(Global.ViewProxy);
        }

        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            this.MenusFrame.Navigate(Global.ViewSettings);
            Global.ViewSettings.Init();
        }

        private void BtnSuccess_Click(object sender, RoutedEventArgs e)
        {
            this.MenusFrame.Navigate(Global.ViewSuccess);
        }

        private void BtnTasks_Click(object sender, RoutedEventArgs e)
        {
            this.MenusFrame.Navigate(Global.ViewDashboard);
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                base.WindowState = (base.WindowState == WindowState.Normal) ? WindowState.Maximized : WindowState.Normal;
            }
            else
            {
                base.DragMove();
            }
        }

        [GeneratedCode("PresentationBuildTasks", "4.0.0.0"), DebuggerNonUserCode]
        public void InitializeComponent()
        {
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocator = new Uri("/EveAIO;component/mainwindow.xaml", UriKind.Relative);
                Application.LoadComponent(this, resourceLocator);
            }
        }

        private void lvMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (this.lvMenu.SelectedIndex)
            {
                case 0:
                    this.MenusFrame.Navigate(Global.ViewDashboard);
                    return;

                case 1:
                    this.MenusFrame.Navigate(Global.ViewSuccess);
                    return;

                case 2:
                    this.MenusFrame.Navigate(Global.ViewProfiles);
                    return;

                case 3:
                    this.MenusFrame.Navigate(Global.ViewNotifications);
                    return;

                case 4:
                    this.MenusFrame.Navigate(Global.ViewCaptcha);
                    Global.ViewCaptcha.Init();
                    return;

                case 5:
                    this.MenusFrame.Navigate(Global.ViewProxy);
                    return;

                case 6:
                    this.MenusFrame.Navigate(Global.ViewLog);
                    return;

                case 7:
                    this.MenusFrame.Navigate(Global.ViewSettings);
                    Global.ViewSettings.Init();
                    return;

                case 8:
                    this.MenusFrame.Navigate(Global.ViewTools);
                    return;
            }
        }

        private void MassLinkChange_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            new MassLinkChangeWindow((Window) Global.MAIN_WINDOW, MassLinkChangeWindow.MassLinkChangeModeEnum.SPECIFIC).ShowDialog();
        }

        private void MassLinkChangeAll_Executed(object sender, ExecutedRoutedEventArgs e)
        {
        }

        private void QuickTaskCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (EveAIO.Helpers.QuickEnabled())
            {
                new QuickTaskWindow(this).ShowDialog();
            }
        }

        [GeneratedCode("PresentationBuildTasks", "4.0.0.0"), DebuggerNonUserCode, EditorBrowsable(EditorBrowsableState.Never)]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    ((MainWindow) target).Loaded += new RoutedEventHandler(this.Window_Loaded);
                    ((MainWindow) target).Closing += new CancelEventHandler(this.Window_Closing);
                    return;

                case 2:
                    ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.QuickTaskCommand_Executed);
                    return;

                case 3:
                    ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.MassLinkChange_Executed);
                    return;

                case 4:
                    ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.MassLinkChangeAll_Executed);
                    return;

                case 5:
                    ((Grid) target).MouseLeftButtonDown += new MouseButtonEventHandler(this.Grid_MouseLeftButtonDown);
                    return;

                case 6:
                    this.lvMenu = (ListBox) target;
                    this.lvMenu.MouseLeftButtonDown += new MouseButtonEventHandler(this.Grid_MouseLeftButtonDown);
                    this.lvMenu.SelectionChanged += new SelectionChangedEventHandler(this.lvMenu_SelectionChanged);
                    return;

                case 7:
                    this.BtnMinimize = (Button) target;
                    this.BtnMinimize.Click += new RoutedEventHandler(this.BtnMinimize_Click);
                    return;

                case 8:
                    this.BtnMaximize = (Button) target;
                    this.BtnMaximize.Click += new RoutedEventHandler(this.BtnMaximize_Click);
                    return;

                case 9:
                    this.BtnClose = (Button) target;
                    this.BtnClose.Click += new RoutedEventHandler(this.BtnClose_Click);
                    return;

                case 10:
                    this.updater = (AutomaticUpdater) target;
                    return;

                case 11:
                    this.MenusFrame = (System.Windows.Controls.Frame) target;
                    return;

                case 12:
                    this.txtTotalTasks = (TextBlock) target;
                    return;

                case 13:
                    this.txtTotalProfiles = (TextBlock) target;
                    return;

                case 14:
                    this.txtTotalProxyLists = (TextBlock) target;
                    return;

                case 15:
                    this.txtTotalProxies = (TextBlock) target;
                    return;

                case 0x10:
                    this.lblVersion = (TextBlock) target;
                    return;

                case 0x11:
                    this.txtVersion = (TextBlock) target;
                    return;
            }
            this._contentLoaded = true;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            EveAIO.Helpers.SaveSettings();
            try
            {
                using (List<ChromeDriver>.Enumerator enumerator = Global.ChromeDrivers.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        enumerator.Current.Quit();
                    }
                }
            }
            catch
            {
            }
            try
            {
                try
                {
                    Global.CAPTCHA_SOLVER1.BROWSER.Close();
                }
                catch
                {
                }
                Global.CAPTCHA_SOLVER1.IsHardClose = true;
                Global.CAPTCHA_SOLVER1.Close();
            }
            catch
            {
            }
            try
            {
                try
                {
                    Global.CAPTCHA_SOLVER2.BROWSER.Close();
                }
                catch
                {
                }
                Global.CAPTCHA_SOLVER2.IsHardClose = true;
                Global.CAPTCHA_SOLVER2.Close();
            }
            catch
            {
            }
            try
            {
                try
                {
                    Global.CAPTCHA_SOLVER3.BROWSER.Close();
                }
                catch
                {
                }
                Global.CAPTCHA_SOLVER3.IsHardClose = true;
                Global.CAPTCHA_SOLVER3.Close();
            }
            catch
            {
            }
            try
            {
                try
                {
                    Global.CAPTCHA_SOLVER4.BROWSER.Close();
                }
                catch
                {
                }
                Global.CAPTCHA_SOLVER4.IsHardClose = true;
                Global.CAPTCHA_SOLVER4.Close();
            }
            catch
            {
            }
            try
            {
                try
                {
                    Global.CAPTCHA_SOLVER5.BROWSER.Close();
                }
                catch
                {
                }
                Global.CAPTCHA_SOLVER5.IsHardClose = true;
                Global.CAPTCHA_SOLVER5.Close();
            }
            catch
            {
            }
            try
            {
                Global.SUPREME_PICKER.Close();
            }
            catch
            {
            }
            try
            {
                if (Global.SETTINGS.ClearSession)
                {
                    foreach (string str in Directory.GetDirectories(Path.GetTempPath()))
                    {
                        if (str.Contains("eo.webbrowser.cache"))
                        {
                            Directory.Delete(str, true);
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.txtVersion.Text = Global.VERSION;
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly MainWindow.<>c <>9;
            public static Action <>9__3_1;
            public static Action <>9__3_0;
            public static Action<Task> <>9__4_1;

            static <>c()
            {
                Class7.RIuqtBYzWxthF();
                <>9 = new MainWindow.<>c();
            }

            public <>c()
            {
                Class7.RIuqtBYzWxthF();
            }

            internal void <_licenseTimer_Tick>b__3_0()
            {
                try
                {
                    if (EveAIO.License.Check(Global.SERIAL) == EveAIO.License.Result.success)
                    {
                        return;
                    }
                    goto Label_0053;
                Label_000F:
                    Task.Factory.StartNew(delegate {
                        Thread.Sleep(0x1388);
                        Environment.Exit(0);
                    });
                    MessageBox.Show("Opps, your key isn't activated, Eve will close in 5 seconds", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                Label_0053:
                    switch (((-1106914262 ^ -2129080893) % 3))
                    {
                        case 0:
                            goto Label_0053;

                        case 1:
                            goto Label_000F;

                        case 2:
                            return;
                    }
                }
                catch
                {
                }
            }

            internal void <_licenseTimer_Tick>b__3_1()
            {
                Thread.Sleep(0x1388);
                Environment.Exit(0);
            }

            internal void <_versionTimer_Tick>b__4_1(Task t)
            {
            }
        }
    }
}

