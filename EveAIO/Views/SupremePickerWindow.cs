namespace EveAIO.Views
{
    using EveAIO;
    using EveAIO.Pocos;
    using EveAIO.Tasks;
    using HtmlAgilityPack;
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Markup;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    public class SupremePickerWindow : Window, IComponentConnector
    {
        internal Button BtnClose;
        internal Button btnReloadProducts;
        internal ListBox prodList;
        internal Button btnReloadTasks;
        internal ListBox tasksList;
        internal Button btnAssign;
        private bool _contentLoaded;

        public SupremePickerWindow()
        {
            Class7.RIuqtBYzWxthF();
            this.InitializeComponent();
        }

        private void btnAssign_Click(object sender, RoutedEventArgs e)
        {
            if ((this.tasksList.SelectedItems.Count > 0) && (this.prodList.SelectedItems.Count > 0))
            {
                string str = ((Image) this.prodList.SelectedItems[0]).Tag.ToString();
                List<string> ids = new List<string>();
                foreach (object obj2 in this.tasksList.SelectedItems)
                {
                    string id = ((TextBlock) obj2).Tag.ToString();
                    TaskObject local1 = Global.SETTINGS.TASKS.First<TaskObject>(x => x.Id == id);
                    local1.TaskType = TaskObject.TaskTypeEnum.directlink;
                    local1.Link = str;
                    local1.State = TaskObject.StateEnum.stopped;
                    ids.Add(id);
                }
                this.btnReloadTasks_Click(null, null);
                TaskManager.StartTasks(ids);
            }
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

        private void btnReloadProducts_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.prodList.Items.Clear();
                Task.Factory.StartNew(() => Global.SUPREME_PICKER.Dispatcher.BeginInvoke(delegate {
                    try
                    {
                        this.btnReloadProducts.IsEnabled = false;
                        this.btnReloadProducts.Opacity = 0.6;
                        this.btnReloadProducts.Content = "Loading ...";
                        HttpWebRequest request = (HttpWebRequest) WebRequest.Create("http://www.supremenewyork.com/shop/new");
                        request.KeepAlive = false;
                        request.Accept = "*/*";
                        if (!string.IsNullOrEmpty(Global.SETTINGS.ImagePickerProxyId) && Global.SETTINGS.PROXIES.Any<ProxyListObject>(x => (x.Id == Global.SETTINGS.ImagePickerProxyId)))
                        {
                            ProxyObject obj2 = EveAIO.Helpers.GetProxy(Global.SETTINGS.PROXIES.First<ProxyListObject>(x => x.Id == Global.SETTINGS.ImagePickerProxyId));
                            if (obj2 != null)
                            {
                                WebProxy proxy = null;
                                if (!string.IsNullOrEmpty(obj2.Username))
                                {
                                    proxy = new WebProxy(obj2.IP, obj2.Port);
                                    ICredentials credentials = new NetworkCredential(obj2.Username, obj2.Password);
                                    proxy.Credentials = credentials;
                                }
                                else
                                {
                                    proxy = new WebProxy(obj2.IP, obj2.Port);
                                }
                                request.Proxy = proxy;
                            }
                        }
                        string html = "";
                        using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
                        {
                            html = new StreamReader(response.GetResponseStream()).ReadToEnd();
                        }
                        HtmlDocument document1 = new HtmlDocument();
                        document1.LoadHtml(html);
                        foreach (HtmlNode node in from x in document1.DocumentNode.Descendants("div")
                            where (x.Attributes["class"] != null) && (x.Attributes["class"].Value == "inner-article")
                            select x)
                        {
                            string str2 = "http://www.supremenewyork.com" + node.Descendants("a").First<HtmlNode>().Attributes["href"].Value;
                            BitmapImage image2 = new BitmapImage(new Uri("https:" + node.Descendants("img").First<HtmlNode>().Attributes["src"].Value, UriKind.Absolute));
                            Image newItem = new Image {
                                Margin = new Thickness(2.0),
                                Height = 60.0,
                                Width = 60.0,
                                Source = image2,
                                Tag = str2,
                                VerticalAlignment = VerticalAlignment.Center,
                                HorizontalAlignment = HorizontalAlignment.Center
                            };
                            newItem.MouseDown += new MouseButtonEventHandler(this.Img_MouseDown);
                            this.prodList.Items.Add(newItem);
                        }
                    }
                    catch (Exception)
                    {
                    }
                    finally
                    {
                        this.btnReloadProducts.IsEnabled = true;
                        this.btnReloadProducts.Opacity = 1.0;
                        this.btnReloadProducts.Content = "Reload";
                    }
                }, Array.Empty<object>()));
            }
            catch (Exception)
            {
            }
        }

        private void btnReloadTasks_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.tasksList.Items.Clear();
                Task.Factory.StartNew(() => Global.SUPREME_PICKER.Dispatcher.BeginInvoke(delegate {
                    foreach (TaskObject obj2 in from x in Global.SETTINGS.TASKS
                        where (x.Platform == TaskObject.PlatformEnum.supreme) && (x.TaskType == TaskObject.TaskTypeEnum.manualPicker)
                        select x)
                    {
                        TextBlock newItem = new TextBlock {
                            HorizontalAlignment = HorizontalAlignment.Stretch,
                            FontSize = 14.0,
                            Text = obj2.Name,
                            Tag = obj2.Id,
                            Padding = new Thickness(2.0),
                            Background = new SolidColorBrush(Colors.Transparent)
                        };
                        this.tasksList.Items.Add(newItem);
                    }
                }, Array.Empty<object>()));
            }
            catch (Exception)
            {
            }
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.DragMove();
        }

        private void Img_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if ((e.ChangedButton == MouseButton.Left) && (e.ClickCount == 2))
            {
                this.btnAssign_Click(null, null);
            }
        }

        [GeneratedCode("PresentationBuildTasks", "4.0.0.0"), DebuggerNonUserCode]
        public void InitializeComponent()
        {
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocator = new Uri("/EveAIO;component/views/supremepickerwindow.xaml", UriKind.Relative);
                Application.LoadComponent(this, resourceLocator);
            }
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
                    this.BtnClose = (Button) target;
                    this.BtnClose.Click += new RoutedEventHandler(this.BtnClose_Click);
                    return;

                case 3:
                    this.btnReloadProducts = (Button) target;
                    this.btnReloadProducts.Click += new RoutedEventHandler(this.btnReloadProducts_Click);
                    return;

                case 4:
                    this.prodList = (ListBox) target;
                    return;

                case 5:
                    this.btnReloadTasks = (Button) target;
                    this.btnReloadTasks.Click += new RoutedEventHandler(this.btnReloadTasks_Click);
                    return;

                case 6:
                    this.tasksList = (ListBox) target;
                    return;

                case 7:
                    this.btnAssign = (Button) target;
                    this.btnAssign.Click += new RoutedEventHandler(this.btnAssign_Click);
                    return;
            }
            this._contentLoaded = true;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Global.SUPREME_PICKER = null;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.btnReloadProducts_Click(null, null);
            this.btnReloadTasks_Click(null, null);
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly SupremePickerWindow.<>c <>9;
            public static Func<ProxyListObject, bool> <>9__6_2;
            public static Func<ProxyListObject, bool> <>9__6_3;
            public static Func<HtmlNode, bool> <>9__6_4;
            public static Func<TaskObject, bool> <>9__8_2;

            static <>c()
            {
                Class7.RIuqtBYzWxthF();
                <>9 = new SupremePickerWindow.<>c();
            }

            public <>c()
            {
                Class7.RIuqtBYzWxthF();
            }

            internal bool <btnReloadProducts_Click>b__6_2(ProxyListObject x) => 
                (x.Id == Global.SETTINGS.ImagePickerProxyId);

            internal bool <btnReloadProducts_Click>b__6_3(ProxyListObject x) => 
                (x.Id == Global.SETTINGS.ImagePickerProxyId);

            internal bool <btnReloadProducts_Click>b__6_4(HtmlNode x) => 
                ((x.Attributes["class"] != null) && (x.Attributes["class"].Value == "inner-article"));

            internal bool <btnReloadTasks_Click>b__8_2(TaskObject x) => 
                ((x.Platform == TaskObject.PlatformEnum.supreme) && (x.TaskType == TaskObject.TaskTypeEnum.manualPicker));
        }
    }
}

