namespace EveAIO.Views
{
    using EveAIO;
    using EveAIO.Pocos;
    using Microsoft.Win32;
    using OpenQA.Selenium.Chrome;
    using System;
    using System.CodeDom.Compiler;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.IO.Compression;
    using System.Runtime.CompilerServices;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Markup;
    using System.Windows.Media;

    public class ProxyListWindow : Window, IComponentConnector
    {
        private Global.FormOperation _operation;
        private ProxyListObject _proxyList;
        private bool _lastCheck;
        private CancellationTokenSource _cancelTokenSource;
        private static List<ProxyTester> _testers;
        private static List<ProxyPinger> _pingers;
        private CancellationToken _cancelToken;
        internal Button BtnClose;
        internal Button BtnImport;
        internal Button BtnPaste;
        internal Button BtnStartTest;
        internal Button BtnStopTest;
        internal Button BtnCheckAll;
        internal Button BtnDelDead;
        internal TextBox txtProxyListName;
        internal RadioButton rRotationRandom;
        internal RadioButton rRotationLinear;
        internal RadioButton rTestWebsite;
        internal RadioButton rTestPing;
        internal ProgressBar progBarTester;
        public DataGrid gvProxies;
        internal TextBox txtUrl;
        internal TextBox txtTimeout;
        internal Button btnChrome;
        internal Button btnSave;
        internal Button btnCancel;
        private bool _contentLoaded;

        static ProxyListWindow()
        {
            Class7.RIuqtBYzWxthF();
        }

        public ProxyListWindow(Window owner)
        {
            Class7.RIuqtBYzWxthF();
            this.InitializeComponent();
            base.Owner = owner;
            this._operation = Global.FormOperation.insert;
            ProxyListObject obj1 = new ProxyListObject {
                Id = Guid.NewGuid().ToString()
            };
            this._proxyList = obj1;
        }

        public ProxyListWindow(Window owner, ProxyListObject proxyList)
        {
            Class7.RIuqtBYzWxthF();
            this.InitializeComponent();
            base.Owner = owner;
            this._operation = Global.FormOperation.update;
            this._proxyList = proxyList;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            base.Close();
        }

        private void BtnCheckAll_Click(object sender, RoutedEventArgs e)
        {
            IEnumerator<ProxyObject> enumerator;
            if (this._lastCheck)
            {
                using (enumerator = this._proxyList.Proxies.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        enumerator.Current.Enabled = false;
                    }
                }
                this._lastCheck = false;
            }
            else
            {
                using (enumerator = this._proxyList.Proxies.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        enumerator.Current.Enabled = true;
                    }
                }
                this._lastCheck = true;
            }
        }

        private void btnChome_Click(object sender, RoutedEventArgs e)
        {
            ProxyObject it;
            string url;
            if ((this.gvProxies.SelectedItems.Count != 0) && !string.IsNullOrEmpty(this.txtUrl.Text))
            {
                it = null;
                try
                {
                    it = (ProxyObject) this.gvProxies.SelectedItems[0];
                }
                catch
                {
                    return;
                }
                url = this.txtUrl.Text;
                Task.Factory.StartNew(delegate {
                    try
                    {
                        ChromeDriverService service = ChromeDriverService.CreateDefaultService();
                        service.HideCommandPromptWindow = true;
                        ChromeOptions options = new ChromeOptions();
                        options.AddArgument("ignore-certificate-errors");
                        string str3 = Guid.NewGuid().ToString();
                        string str2 = Helpers.Decrypt(File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"\ext\manifest.json"));
                        string str = Helpers.Decrypt(File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"\ext\background.js")).Replace("{0}", it.IP).Replace("{1}", it.Port.ToString()).Replace("{2}", it.Username).Replace("{3}", it.Password);
                        Dictionary<string, string> dictionary = new Dictionary<string, string> {
                            { 
                                "manifest.json",
                                str2
                            },
                            { 
                                "background.js",
                                str
                            }
                        };
                        using (MemoryStream stream = new MemoryStream())
                        {
                            string path = AppDomain.CurrentDomain.BaseDirectory + @"\ext\" + str3 + ".zip";
                            using (ZipArchive archive = new ZipArchive(stream, ZipArchiveMode.Create, true))
                            {
                                foreach (KeyValuePair<string, string> pair in dictionary)
                                {
                                    using (Stream stream2 = archive.CreateEntry(pair.Key).Open())
                                    {
                                        using (StreamWriter writer = new StreamWriter(stream2))
                                        {
                                            writer.Write(pair.Value);
                                        }
                                    }
                                }
                            }
                            using (FileStream stream3 = new FileStream(path, FileMode.Create))
                            {
                                stream.Seek(0L, SeekOrigin.Begin);
                                stream.CopyTo(stream3);
                            }
                        }
                        options.AddExtension(AppDomain.CurrentDomain.BaseDirectory + @"\ext\" + str3 + ".zip");
                        ChromeDriver item = new ChromeDriver(service, options);
                        Global.ChromeDrivers.Add(item);
                        item.Navigate().GoToUrl(url);
                    }
                    catch (Exception)
                    {
                    }
                });
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            base.Close();
        }

        private void BtnDelDead_Click(object sender, RoutedEventArgs e)
        {
            List<ProxyObject> list = new List<ProxyObject>();
            foreach (ProxyObject obj2 in from x in this._proxyList.Proxies
                where x.Status.Contains("DEAD")
                select x)
            {
                list.Add(obj2);
            }
            foreach (ProxyObject obj3 in list)
            {
                this._proxyList.Proxies.Remove(obj3);
            }
        }

        private void BtnImport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog {
                    Filter = "*.txt|*.txt"
                };
                bool? nullable = dialog.ShowDialog();
                if (nullable.GetValueOrDefault() ? nullable.HasValue : false)
                {
                    foreach (string str in File.ReadAllLines(dialog.FileName))
                    {
                        if (!string.IsNullOrEmpty(str))
                        {
                            ProxyObject item = Helpers.CheckProxyValidity(str);
                            if (item != null)
                            {
                                item.Enabled = true;
                                this._proxyList.Proxies.Add(item);
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Global.Logger.Error("Error while importing proxies", exception);
                MessageBox.Show("Error occured while importing proxies", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }

        private void BtnPaste_Click(object sender, RoutedEventArgs e)
        {
            string[] strArray = Regex.Split(Clipboard.GetText(), "\r\n|\r|\n");
            for (int i = 0; i < strArray.Length; i++)
            {
                ProxyObject item = Helpers.CheckProxyValidity(strArray[i]);
                if (item != null)
                {
                    item.Enabled = true;
                    this._proxyList.Proxies.Add(item);
                }
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (this.Check())
            {
                this._proxyList.Name = this.txtProxyListName.Text.Trim();
                this._proxyList.ProxyTestUrl = this.txtUrl.Text.Trim();
                this._proxyList.ProxyTimeout = string.IsNullOrEmpty(this.txtTimeout.Text) ? 0 : int.Parse(this.txtTimeout.Text.Trim());
                this._proxyList.Rotation = (!this.rRotationRandom.IsChecked.HasValue || !this.rRotationRandom.IsChecked.Value) ? ProxyListObject.RotationEnum.linear : ProxyListObject.RotationEnum.random;
                this._proxyList.Tester = (!this.rTestWebsite.IsChecked.HasValue || !this.rTestWebsite.IsChecked.Value) ? ProxyListObject.TesterEnum.ping : ProxyListObject.TesterEnum.website;
                this._proxyList.UpdateProxies();
                if (this._operation == Global.FormOperation.insert)
                {
                    Global.SETTINGS.PROXIES.Add(this._proxyList);
                }
                Helpers.SaveSettings();
                base.Close();
            }
        }

        private void BtnStartTest_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtUrl.Text.Trim()) && !string.IsNullOrEmpty(this.txtTimeout.Text.Trim()))
            {
                this.txtUrl.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
                this.txtTimeout.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
                if (!Global.IsProxyTestRunning)
                {
                    List<ProxyObject> proxies = new List<ProxyObject>();
                    foreach (object obj3 in (IEnumerable) this.gvProxies.Items)
                    {
                        if (obj3 is ProxyObject)
                        {
                            ProxyObject item = (ProxyObject) obj3;
                            item.Status = "";
                            item.State = ProxyObject.StateEnum.untested;
                            proxies.Add(item);
                        }
                    }
                    if (proxies.Count >= 1)
                    {
                        this.progBarTester.Visibility = Visibility.Visible;
                        this.progBarTester.IsEnabled = true;
                        Global.IsProxyTestRunning = true;
                        TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
                        this._cancelTokenSource = new CancellationTokenSource();
                        this._cancelToken = this._cancelTokenSource.Token;
                        _testers = new List<ProxyTester>();
                        _pingers = new List<ProxyPinger>();
                        string url = this.txtUrl.Text.Trim();
                        int timeout = int.Parse(this.txtTimeout.Text.Trim());
                        if (this.rTestWebsite.IsChecked.HasValue && this.rTestWebsite.IsChecked.Value)
                        {
                            Task.Factory.StartNew(() => Parallel.ForEach<Tuple<int, int>>(Partitioner.Create(0, proxies.Count, 5), delegate (Tuple<int, int> range, ParallelLoopState state) {
                                if (this._cancelToken.IsCancellationRequested)
                                {
                                    state.Break();
                                }
                                for (int i = range.Item1; i < range.Item2; i++)
                                {
                                    ProxyTester item = new ProxyTester(this, proxies[i], url, timeout);
                                    _testers.Add(item);
                                    item.Start();
                                }
                            }), this._cancelToken).ContinueWith(delegate (Task t) {
                                Global.IsProxyTestRunning = false;
                                this.progBarTester.Visibility = Visibility.Collapsed;
                                this.progBarTester.IsEnabled = false;
                            }, scheduler);
                        }
                        else
                        {
                            Task.Factory.StartNew(() => Parallel.ForEach<Tuple<int, int>>(Partitioner.Create(0, proxies.Count, 5), delegate (Tuple<int, int> range, ParallelLoopState state) {
                                if (this._cancelToken.IsCancellationRequested)
                                {
                                    state.Break();
                                }
                                for (int i = range.Item1; i < range.Item2; i++)
                                {
                                    ProxyPinger item = new ProxyPinger(this, proxies[i], timeout);
                                    _pingers.Add(item);
                                    item.Start();
                                }
                            }), this._cancelToken).ContinueWith(delegate (Task t) {
                                Global.IsProxyTestRunning = false;
                                this.progBarTester.Visibility = Visibility.Collapsed;
                                this.progBarTester.IsEnabled = false;
                            }, scheduler);
                        }
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(this.txtUrl.Text))
                {
                    this.txtUrl.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
                }
                else
                {
                    this.txtUrl.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                }
                if (string.IsNullOrEmpty(this.txtTimeout.Text))
                {
                    this.txtTimeout.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                }
                else
                {
                    this.txtTimeout.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
                }
            }
        }

        private void BtnStopTest_Click(object sender, RoutedEventArgs e)
        {
            if (Global.IsProxyTestRunning)
            {
                goto Label_0034;
            }
        Label_0015:
            switch (((-239844091 ^ -220152645) % 4))
            {
                case 0:
                    goto Label_0015;

                case 2:
                    return;

                case 3:
                    break;

                default:
                {
                    using (List<ProxyTester>.Enumerator enumerator2 = _testers.GetEnumerator())
                    {
                        while (!enumerator2.MoveNext())
                        {
                        Label_0055:
                            if (-15645259 || true)
                            {
                                goto Label_0070;
                            }
                        Label_0058:
                            enumerator2.Current.Stop();
                        Label_0070:
                            switch (((-151330560 ^ -220152645) % 4))
                            {
                                case 0:
                                    goto Label_0070;

                                case 1:
                                {
                                    continue;
                                }
                                case 3:
                                    goto Label_0058;
                            }
                            goto Label_00B7;
                        }
                        goto Label_0055;
                    }
                }
            }
        Label_0034:
            if (_testers != null)
            {
                goto Label_0015;
            }
        Label_00B7:
            if (_pingers != null)
            {
                using (List<ProxyPinger>.Enumerator enumerator = _pingers.GetEnumerator())
                {
                    while (!enumerator.MoveNext())
                    {
                    Label_00D6:
                        if (-305270298 || true)
                        {
                            goto Label_00F1;
                        }
                    Label_00D9:
                        enumerator.Current.Stop();
                    Label_00F1:
                        switch (((-1282054772 ^ -220152645) % 4))
                        {
                            case 0:
                                goto Label_00F1;

                            case 2:
                            {
                                continue;
                            }
                            case 3:
                                goto Label_00D9;
                        }
                        goto Label_0137;
                    }
                    goto Label_00D6;
                }
            }
        Label_0137:
            this._cancelTokenSource.Cancel();
        }

        private bool Check()
        {
            if (string.IsNullOrEmpty(this.txtProxyListName.Text))
            {
                this.txtProxyListName.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                return false;
            }
            this.txtProxyListName.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
            return true;
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.DragMove();
        }

        private void gvProxies_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
        }

        private void gvProxies_AutoGeneratedColumns(object sender, EventArgs e)
        {
            foreach (DataGridColumn column2 in this.gvProxies.Columns)
            {
                if (column2.Header.ToString() != "Enabled")
                {
                    if (column2.Header.ToString() != "StateImg")
                    {
                        if (column2.Header.ToString() == "IP")
                        {
                            DataGridTextColumn column3 = column2 as DataGridTextColumn;
                            if (column3 != null)
                            {
                                column3.Header = "IP";
                                column3.Width = 200.0;
                                column3.MinWidth = 150.0;
                                column3.HeaderStyle = this.gvProxies.FindResource("column0") as Style;
                                column3.ElementStyle = this.gvProxies.FindResource("txtStyle") as Style;
                                column3.EditingElementStyle = Global.SETTINGS.EnvLight ? (this.gvProxies.FindResource("textStyleLight") as Style) : (this.gvProxies.FindResource("textStyleDark") as Style);
                            }
                        }
                        else if (column2.Header.ToString() == "Port")
                        {
                            DataGridTextColumn column = column2 as DataGridTextColumn;
                            if (column != null)
                            {
                                column.Header = "PORT";
                                column.Width = 80.0;
                                column.MinWidth = 80.0;
                                column.HeaderStyle = this.gvProxies.FindResource("column0") as Style;
                                column.ElementStyle = this.gvProxies.FindResource("txtStyle") as Style;
                                column.EditingElementStyle = Global.SETTINGS.EnvLight ? (this.gvProxies.FindResource("textStyleLight") as Style) : (this.gvProxies.FindResource("textStyleDark") as Style);
                            }
                        }
                        else if (column2.Header.ToString() == "Username")
                        {
                            DataGridTextColumn column5 = column2 as DataGridTextColumn;
                            if (column5 != null)
                            {
                                column5.Header = "USERNAME";
                                column5.Width = 90.0;
                                column5.MinWidth = 90.0;
                                column5.HeaderStyle = this.gvProxies.FindResource("column0") as Style;
                                column5.ElementStyle = this.gvProxies.FindResource("txtStyle") as Style;
                                column5.EditingElementStyle = Global.SETTINGS.EnvLight ? (this.gvProxies.FindResource("textStyleLight") as Style) : (this.gvProxies.FindResource("textStyleDark") as Style);
                            }
                        }
                        else if (column2.Header.ToString() == "Password")
                        {
                            DataGridTextColumn column6 = column2 as DataGridTextColumn;
                            if (column6 != null)
                            {
                                column6.Header = "PASSWORD";
                                column6.Width = 120.0;
                                column6.MinWidth = 120.0;
                                column6.HeaderStyle = this.gvProxies.FindResource("column2") as Style;
                                column6.ElementStyle = this.gvProxies.FindResource("txtStyle") as Style;
                                column6.EditingElementStyle = this.gvProxies.FindResource("textStyleDark") as Style;
                            }
                        }
                        else if (column2.Header.ToString() == "Status")
                        {
                            DataGridTextColumn column8 = column2 as DataGridTextColumn;
                            if (column8 != null)
                            {
                                column8.Header = "STATUS";
                                column8.Width = new DataGridLength(1.0, DataGridLengthUnitType.Star);
                                column8.MinWidth = 120.0;
                                column8.FontSize = 14.0;
                                column8.IsReadOnly = true;
                                column8.HeaderStyle = this.gvProxies.FindResource("column2") as Style;
                                column8.ElementStyle = this.gvProxies.FindResource("txtStyle") as Style;
                                column8.EditingElementStyle = Global.SETTINGS.EnvLight ? (this.gvProxies.FindResource("textStyleLight") as Style) : (this.gvProxies.FindResource("textStyleDark") as Style);
                            }
                        }
                    }
                    else
                    {
                        DataGridTemplateColumn column4 = column2 as DataGridTemplateColumn;
                        if (column4 != null)
                        {
                            column4.Header = "S";
                            column4.Width = 20.0;
                            column4.MinWidth = 20.0;
                            column4.MaxWidth = 20.0;
                            column4.CanUserResize = false;
                            column4.IsReadOnly = true;
                            column4.HeaderStyle = this.gvProxies.FindResource("column0") as Style;
                        }
                    }
                }
                else
                {
                    DataGridTemplateColumn column7 = column2 as DataGridTemplateColumn;
                    if (column7 != null)
                    {
                        column7.Header = "E";
                        column7.Width = 20.0;
                        column7.MinWidth = 20.0;
                        column7.CanUserResize = false;
                        column7.HeaderStyle = this.gvProxies.FindResource("column0") as Style;
                    }
                }
            }
        }

        private void gvProxies_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            switch (e.Column.Header.ToString())
            {
                case "Id":
                    e.Column.Visibility = Visibility.Collapsed;
                    return;

                case "State":
                    e.Column.Visibility = Visibility.Collapsed;
                    return;

                case "StateImg":
                {
                    DataGridTemplateColumn column2 = new DataGridTemplateColumn {
                        Header = e.Column.Header,
                        CellTemplate = (DataTemplate) this.gvProxies.Resources["imageCellTemplate"],
                        SortMemberPath = e.Column.SortMemberPath
                    };
                    e.Column = column2;
                    return;
                }
                case "Enabled":
                {
                    DataGridTemplateColumn column = new DataGridTemplateColumn {
                        Header = e.Column.Header,
                        CellTemplate = (DataTemplate) this.gvProxies.Resources["chCellTemplate"],
                        SortMemberPath = e.Column.SortMemberPath
                    };
                    e.Column = column;
                    return;
                }
            }
        }

        private void gvProxies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((this.gvProxies.SelectedItems.Count != 0) && !string.IsNullOrEmpty(this.txtUrl.Text.Trim()))
            {
                this.btnChrome.IsEnabled = true;
                this.btnChrome.Opacity = 1.0;
            }
            else
            {
                this.btnChrome.IsEnabled = false;
                this.btnChrome.Opacity = 0.6;
            }
        }

        [GeneratedCode("PresentationBuildTasks", "4.0.0.0"), DebuggerNonUserCode]
        public void InitializeComponent()
        {
            // This item is obfuscated and can not be translated.
            if (!this._contentLoaded)
            {
                goto Label_002C;
            }
        Label_000D:
            switch (((-500642882 ^ -2117679615) % 4))
            {
                case 0:
                    goto Label_000D;

                case 1:
                    break;

                case 2:
                    return;

                case 3:
                    return;

                default:
                    return;
            }
        Label_002C:
            this._contentLoaded = true;
            Uri resourceLocator = new Uri("/EveAIO;component/views/proxylistwindow.xaml", UriKind.Relative);
            Application.LoadComponent(this, resourceLocator);
            goto Label_000D;
        }

        [GeneratedCode("PresentationBuildTasks", "4.0.0.0"), EditorBrowsable(EditorBrowsableState.Never), DebuggerNonUserCode]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    ((ProxyListWindow) target).Loaded += new RoutedEventHandler(this.Window_Loaded);
                    return;

                case 2:
                    ((Grid) target).MouseLeftButtonDown += new MouseButtonEventHandler(this.Grid_MouseLeftButtonDown);
                    return;

                case 3:
                    this.BtnClose = (Button) target;
                    this.BtnClose.Click += new RoutedEventHandler(this.BtnClose_Click);
                    return;

                case 4:
                    this.BtnImport = (Button) target;
                    this.BtnImport.Click += new RoutedEventHandler(this.BtnImport_Click);
                    return;

                case 5:
                    this.BtnPaste = (Button) target;
                    this.BtnPaste.Click += new RoutedEventHandler(this.BtnPaste_Click);
                    return;

                case 6:
                    this.BtnStartTest = (Button) target;
                    this.BtnStartTest.Click += new RoutedEventHandler(this.BtnStartTest_Click);
                    return;

                case 7:
                    this.BtnStopTest = (Button) target;
                    this.BtnStopTest.Click += new RoutedEventHandler(this.BtnStopTest_Click);
                    return;

                case 8:
                    this.BtnCheckAll = (Button) target;
                    this.BtnCheckAll.Click += new RoutedEventHandler(this.BtnCheckAll_Click);
                    return;

                case 9:
                    this.BtnDelDead = (Button) target;
                    this.BtnDelDead.Click += new RoutedEventHandler(this.BtnDelDead_Click);
                    return;

                case 10:
                    this.txtProxyListName = (TextBox) target;
                    return;

                case 11:
                    this.rRotationRandom = (RadioButton) target;
                    return;

                case 12:
                    this.rRotationLinear = (RadioButton) target;
                    return;

                case 13:
                    this.rTestWebsite = (RadioButton) target;
                    return;

                case 14:
                    this.rTestPing = (RadioButton) target;
                    return;

                case 15:
                    this.progBarTester = (ProgressBar) target;
                    return;

                case 0x10:
                    this.gvProxies = (DataGrid) target;
                    this.gvProxies.AutoGeneratingColumn += new EventHandler<DataGridAutoGeneratingColumnEventArgs>(this.gvProxies_AutoGeneratingColumn);
                    this.gvProxies.AutoGeneratedColumns += new EventHandler(this.gvProxies_AutoGeneratedColumns);
                    this.gvProxies.SelectionChanged += new SelectionChangedEventHandler(this.gvProxies_SelectionChanged);
                    return;

                case 0x11:
                    this.txtUrl = (TextBox) target;
                    this.txtUrl.TextChanged += new TextChangedEventHandler(this.txtUrl_TextChanged);
                    return;

                case 0x12:
                    this.txtTimeout = (TextBox) target;
                    this.txtTimeout.PreviewTextInput += new TextCompositionEventHandler(this.txt_PreviewTextInput);
                    return;

                case 0x13:
                    this.btnChrome = (Button) target;
                    this.btnChrome.Click += new RoutedEventHandler(this.btnChome_Click);
                    return;

                case 20:
                    this.btnSave = (Button) target;
                    this.btnSave.Click += new RoutedEventHandler(this.btnSave_Click);
                    return;

                case 0x15:
                    this.btnCancel = (Button) target;
                    this.btnCancel.Click += new RoutedEventHandler(this.btnCancel_Click);
                    return;
            }
            this._contentLoaded = true;
        }

        private void txt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        private void txtUrl_TextChanged(object sender, TextChangedEventArgs e)
        {
            if ((this.gvProxies.SelectedItems.Count != 0) && !string.IsNullOrEmpty(this.txtUrl.Text.Trim()))
            {
                this.btnChrome.IsEnabled = true;
                this.btnChrome.Opacity = 1.0;
            }
            else
            {
                this.btnChrome.IsEnabled = false;
                this.btnChrome.Opacity = 0.6;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ICollectionView defaultView = CollectionViewSource.GetDefaultView(this._proxyList.Proxies);
            this.gvProxies.ItemsSource = defaultView;
            if (this._operation == Global.FormOperation.update)
            {
                this.txtProxyListName.Text = this._proxyList.Name;
                this.txtUrl.Text = this._proxyList.ProxyTestUrl;
                this.txtTimeout.Text = this._proxyList.ProxyTimeout.ToString();
                if (this._proxyList.Rotation != ProxyListObject.RotationEnum.linear)
                {
                    this.rRotationRandom.IsChecked = true;
                }
                else
                {
                    this.rRotationLinear.IsChecked = true;
                }
                if (this._proxyList.Tester == ProxyListObject.TesterEnum.website)
                {
                    this.rTestWebsite.IsChecked = true;
                }
                else
                {
                    this.rTestPing.IsChecked = true;
                }
            }
            else
            {
                this.rTestWebsite.IsChecked = true;
                this.rRotationRandom.IsChecked = true;
                this.txtTimeout.Text = "10";
            }
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly ProxyListWindow.<>c <>9;
            public static Func<ProxyObject, bool> <>9__27_0;

            static <>c()
            {
                Class7.RIuqtBYzWxthF();
                <>9 = new ProxyListWindow.<>c();
            }

            public <>c()
            {
                Class7.RIuqtBYzWxthF();
            }

            internal bool <BtnDelDead_Click>b__27_0(ProxyObject x) => 
                x.Status.Contains("DEAD");
        }
    }
}

