namespace EveAIO.Views
{
    using EveAIO;
    using EveAIO.Pocos;
    using EveAIO.Tasks;
    using Microsoft.Win32;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.Support.UI;
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Markup;
    using System.Windows.Media;
    using System.Windows.Threading;
    using System.Xml.Serialization;

    public class DashboardView : Page, IComponentConnector, IStyleConnector
    {
        public static bool ExpandTasks;
        private DispatcherTimer _scheduler;
        internal Button BtnNewTask;
        internal Button BtnEditTask;
        internal Button BtnMassEditTask;
        internal Button BtnDeleteTask;
        internal Button BtnDuplicateTasks;
        internal Button BtnImportTask;
        internal Button BtnExportTasks;
        internal Button BtnStartTask;
        internal Button BtnStopTasks;
        internal Button BtnQuickTask;
        internal DataGrid gvTasks;
        internal MenuItem contextBtnStart;
        internal MenuItem contextBtnStop;
        internal MenuItem contextBtnStartGroup;
        internal MenuItem contextBtnStopGroup;
        internal MenuItem contextBtnAdd;
        internal MenuItem contextBtnEdit;
        internal MenuItem contextBtnDelete;
        internal MenuItem contextBtnDuplicate;
        internal MenuItem contextBtnSelectAll;
        internal MenuItem contextBtnMassLinkChange;
        internal MenuItem contextBtnSchedule;
        internal MenuItem contextBtnScheduleStop;
        internal MenuItem contextBtnOpenBrowser;
        private bool _contentLoaded;

        static DashboardView()
        {
            Class7.RIuqtBYzWxthF();
            ExpandTasks = !Global.SETTINGS.CollapseTasks;
        }

        public DashboardView()
        {
            Class7.RIuqtBYzWxthF();
            this._scheduler = new DispatcherTimer();
            this.InitializeComponent();
            ListCollectionView view = new ListCollectionView(Global.SETTINGS.TASKS);
            view.GroupDescriptions.Add(new PropertyGroupDescription("PlatformImg"));
            this.gvTasks.ItemsSource = view;
            this.gvTasks.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
            this._scheduler.Interval = new TimeSpan(0, 0, 1);
            this._scheduler.Tick += new EventHandler(this._scheduler_Tick);
            this._scheduler.Start();
        }

        private void _scheduler_Tick(object sender, EventArgs e)
        {
            this._scheduler.Stop();
            List<string> ids = new List<string>();
            foreach (TaskObject obj2 in from x in Global.SETTINGS.TASKS
                where x.IsStartScheduled
                select x)
            {
                if (obj2.State != TaskObject.StateEnum.scheduled)
                {
                    obj2.State = TaskObject.StateEnum.scheduled;
                }
                if ((obj2.ScheduleStart <= DateTime.Now.ToLocalTime()) && !obj2.ScheduleStartStarting)
                {
                    obj2.ShopifySmartSchedule = new DateTime?(obj2.ScheduleStart);
                    obj2.ScheduleStartStarting = true;
                    obj2.IsStartScheduled = false;
                    ids.Add(obj2.Id);
                }
                else
                {
                    TimeSpan span = obj2.ScheduleStart.Subtract(DateTime.Now.ToLocalTime());
                    string[] textArray1 = new string[] { $"{Math.Floor(span.TotalHours).ToString():00}", " : ", $"{span.Minutes:00}", " : ", $"{span.Seconds:00}" };
                    obj2.Status = string.Concat(textArray1);
                }
            }
            if (ids.Count > 0)
            {
                TaskManager.StartTasks(ids);
            }
            ids = new List<string>();
            foreach (TaskObject obj3 in from x in Global.SETTINGS.TASKS
                where x.IsStopScheduled
                select x)
            {
                if ((obj3.ScheduleStop <= DateTime.Now.ToLocalTime()) && !obj3.ScheduleStopStarting)
                {
                    obj3.ScheduleStopStarting = true;
                    obj3.IsStopScheduled = false;
                    ids.Add(obj3.Id);
                }
            }
            if (ids.Count > 0)
            {
                TaskManager.StopTasks(ids);
            }
            this._scheduler.Start();
        }

        private void BtnDeleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (this.gvTasks.SelectedItems.Count > 0)
            {
                if (MessageBox.Show("Really delete the selected tasks?", "Are you sure?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    List<string> ids = new List<string>();
                    foreach (TaskObject obj3 in this.gvTasks.SelectedItems)
                    {
                        ids.Add(obj3.Id);
                    }
                    TaskManager.StopTasks(ids);
                    foreach (string id in ids)
                    {
                        TaskObject item = Global.SETTINGS.TASKS.First<TaskObject>(x => x.Id == id);
                        Global.SETTINGS.TASKS.Remove(item);
                    }
                }
                Helpers.SaveSettings();
            }
        }

        private void BtnDuplicateTasks_Click(object sender, RoutedEventArgs e)
        {
            if (this.gvTasks.SelectedItems.Count > 0)
            {
                foreach (TaskObject obj2 in this.gvTasks.SelectedItems)
                {
                    TaskObject item = new TaskObject {
                        Id = Guid.NewGuid().ToString(),
                        Platform = obj2.Platform,
                        Name = obj2.Name,
                        AtcMethod = obj2.AtcMethod,
                        Payment = obj2.Payment,
                        Notify = obj2.Notify,
                        ProxyListId = obj2.ProxyListId,
                        CheckoutDelay = obj2.CheckoutDelay,
                        CheckoutId = obj2.CheckoutId,
                        AnticaptchaRequests = obj2.AnticaptchaRequests,
                        CaptchaRequests = obj2.CaptchaRequests,
                        Delay = obj2.Delay,
                        DelayFrom = obj2.DelayFrom,
                        DelayTo = obj2.DelayTo,
                        Discount = obj2.Discount,
                        DiscountCode = obj2.DiscountCode,
                        DummyProduct = obj2.DummyProduct,
                        ImagetypersRequests = obj2.ImagetypersRequests,
                        Link = obj2.Link,
                        LocalIPCheckout = obj2.LocalIPCheckout,
                        Login = obj2.Login,
                        Username = obj2.Username,
                        Password = obj2.Password,
                        PriceCheck = obj2.PriceCheck,
                        MinimumPrice = obj2.MinimumPrice,
                        MaximumPrice = obj2.MaximumPrice,
                        RandomSize = obj2.RandomSize,
                        RetryDelay = obj2.RetryDelay,
                        RetryOnError = obj2.RetryOnError,
                        Size = obj2.Size,
                        SizePickRandom = obj2.SizePickRandom,
                        SmartCheckout = obj2.SmartCheckout,
                        TaskType = obj2.TaskType,
                        TwoCaptchaRequests = obj2.TwoCaptchaRequests,
                        Variant = obj2.Variant,
                        Color = obj2.Color,
                        Group = obj2.Group,
                        SpecificCaptcha = obj2.SpecificCaptcha,
                        SupremeColorPick = obj2.SupremeColorPick,
                        SupremeAutomation = obj2.SupremeAutomation,
                        GmailEmail = obj2.GmailEmail,
                        GmailPassword = obj2.GmailPassword,
                        SupremeRegion = obj2.SupremeRegion,
                        ColorPickRandom = obj2.ColorPickRandom,
                        DeepSearch = obj2.DeepSearch,
                        DeepSearchLinks = obj2.DeepSearchLinks,
                        FootSite = obj2.FootSite,
                        ShopifyWebsite = obj2.ShopifyWebsite,
                        ShopifySmartCheckoutStop = obj2.ShopifySmartCheckoutStop,
                        IsShopifyCheckoutLink = obj2.IsShopifyCheckoutLink,
                        ShopifyCheckoutLink = obj2.ShopifyCheckoutLink,
                        Guid = Guid.NewGuid().ToString(),
                        SkuId = obj2.SkuId,
                        StyleId = obj2.StyleId,
                        VariousStringData = obj2.VariousStringData,
                        VariousStringData2 = obj2.VariousStringData2,
                        SolveCaptcha = obj2.SolveCaptcha,
                        Last25Products = obj2.Last25Products,
                        DisolveRequests = obj2.DisolveRequests,
                        WatchTaskId = obj2.WatchTaskId,
                        ShopifyIsOldMode = obj2.ShopifyIsOldMode,
                        IsNegativeSizing = obj2.IsNegativeSizing
                    };
                    item.Keywords = new BindingList<string>();
                    foreach (string str in obj2.Keywords)
                    {
                        item.Keywords.Add(str);
                    }
                    item.NegativeKeywords = new BindingList<string>();
                    foreach (string str2 in obj2.NegativeKeywords)
                    {
                        item.NegativeKeywords.Add(str2);
                    }
                    Global.SETTINGS.TASKS.Add(item);
                }
                Helpers.SaveSettings();
            }
        }

        private void BtnEditTask_Click(object sender, RoutedEventArgs e)
        {
            if (this.gvTasks.SelectedItems.Count == 1)
            {
                try
                {
                    TaskObject task = (TaskObject) this.gvTasks.SelectedItems[0];
                    TaskWindow2 window = new TaskWindow2((Window) Global.MAIN_WINDOW, task);
                    window.ShowDialog();
                    if (window.RunAfterSave)
                    {
                        List<string> ids = new List<string>();
                        foreach (string id in window.RunIds)
                        {
                            TaskObject obj3 = Global.SETTINGS.TASKS.First<TaskObject>(x => x.Id == id);
                            if ((obj3.State != TaskObject.StateEnum.running) && (obj3.State != TaskObject.StateEnum.waiting))
                            {
                                ids.Add(obj3.Id);
                            }
                        }
                        TaskManager.StartTasks(ids);
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        private void BtnExportTasks_Click(object sender, RoutedEventArgs e)
        {
            if (Global.SETTINGS.PROFILES.Count > 0)
            {
                try
                {
                    SaveFileDialog dialog = new SaveFileDialog {
                        Filter = "*.eveIO|*.eveIO"
                    };
                    bool? nullable = dialog.ShowDialog();
                    if (nullable.GetValueOrDefault() ? nullable.HasValue : false)
                    {
                        List<TaskObject> o = new List<TaskObject>();
                        foreach (TaskObject obj2 in Global.SETTINGS.TASKS)
                        {
                            o.Add(obj2);
                        }
                        string text = "";
                        XmlSerializer serializer = new XmlSerializer(typeof(List<TaskObject>));
                        using (MemoryStream stream = new MemoryStream())
                        {
                            serializer.Serialize((Stream) stream, o);
                            text = Encoding.ASCII.GetString(stream.ToArray());
                        }
                        string contents = Helpers.Encrypt(text);
                        File.WriteAllText(dialog.FileName, contents);
                        MessageBox.Show("Tasks exported ...", "Information", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    }
                }
                catch (Exception exception)
                {
                    Global.Logger.Error("Error while exporting tasks", exception);
                    MessageBox.Show("Error occured while exporting tasks", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                }
            }
        }

        private void BtnImportTask_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog {
                    Filter = "*.eveIO|*.eveIO"
                };
                bool? nullable = dialog.ShowDialog();
                if (nullable.GetValueOrDefault() ? nullable.HasValue : false)
                {
                    List<TaskObject> list = null;
                    XmlSerializer serializer = new XmlSerializer(typeof(List<TaskObject>));
                    using (StringReader reader = new StringReader(Helpers.Decrypt(File.ReadAllText(dialog.FileName))))
                    {
                        list = (List<TaskObject>) serializer.Deserialize(reader);
                    }
                    foreach (TaskObject task in list)
                    {
                        if (Global.SETTINGS.TASKS.Any<TaskObject>(x => x.Id == task.Id))
                        {
                            task.Id = Guid.NewGuid().ToString();
                        }
                        Global.SETTINGS.TASKS.Add(task);
                    }
                    MessageBox.Show("Imported ...", "Information", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
            }
            catch (Exception exception)
            {
                Global.Logger.Error("Error while importing tasks", exception);
                MessageBox.Show("Error occured while importing tasks", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }

        private void BtnMassEditTask_Click(object sender, RoutedEventArgs e)
        {
            if (this.gvTasks.SelectedItems.Count > 0)
            {
                List<string> ids = new List<string>();
                List<TaskObject.PlatformEnum> source = new List<TaskObject.PlatformEnum>();
                foreach (TaskObject task in this.gvTasks.SelectedItems)
                {
                    ids.Add(task.Id);
                    if (!source.Any<TaskObject.PlatformEnum>(x => (x == task.Platform)))
                    {
                        source.Add(task.Platform);
                    }
                }
                if (source.Count > 1)
                {
                    MessageBox.Show("You can only massedit tasks of the same platform", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                }
                else if (((TaskObject.PlatformEnum) source[0]) == TaskObject.PlatformEnum.woodwood)
                {
                    MessageBox.Show("Mass edit feature for woodwood coming soon", "Info", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
                else if (((TaskObject.PlatformEnum) source[0]) == TaskObject.PlatformEnum.footsites)
                {
                    MessageBox.Show("Mass edit feature for footsites coming soon", "Info", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
                else if (((TaskObject.PlatformEnum) source[0]) == TaskObject.PlatformEnum.nordstrom)
                {
                    MessageBox.Show("Mass edit feature for nordstrom coming soon", "Info", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
                else if (((TaskObject.PlatformEnum) source[0]) == TaskObject.PlatformEnum.mesh)
                {
                    MessageBox.Show("Mass edit feature for mesh coming soon", "Info", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
                else if (((TaskObject.PlatformEnum) source[0]) == TaskObject.PlatformEnum.barneys)
                {
                    MessageBox.Show("Mass edit feature for barneys coming soon", "Info", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
                else if (((TaskObject.PlatformEnum) source[0]) == TaskObject.PlatformEnum.titolo)
                {
                    MessageBox.Show("Mass edit feature for titolo coming soon", "Info", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
                else if (((TaskObject.PlatformEnum) source[0]) == TaskObject.PlatformEnum.funko)
                {
                    MessageBox.Show("Mass edit feature for funko coming soon", "Info", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
                else if (((TaskObject.PlatformEnum) source[0]) == TaskObject.PlatformEnum.puma)
                {
                    MessageBox.Show("Mass edit feature for puma coming soon", "Info", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
                else if (((TaskObject.PlatformEnum) source[0]) == TaskObject.PlatformEnum.converse)
                {
                    MessageBox.Show("Mass edit feature for converse coming soon", "Info", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
                else if (((TaskObject.PlatformEnum) source[0]) == TaskObject.PlatformEnum.holypopstore)
                {
                    MessageBox.Show("Mass edit feature for holypopstore coming soon", "Info", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
                else if (((TaskObject.PlatformEnum) source[0]) == TaskObject.PlatformEnum.footlockerau)
                {
                    MessageBox.Show("Mass edit feature for footlocker au coming soon", "Info", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
                else if (((TaskObject.PlatformEnum) source[0]) == TaskObject.PlatformEnum.footlockereu)
                {
                    MessageBox.Show("Mass edit feature for footlocker eu coming soon", "Info", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
                else if (((TaskObject.PlatformEnum) source[0]) == TaskObject.PlatformEnum.supremeinstore)
                {
                    MessageBox.Show("Mass edit feature for supreme instore signup coming soon", "Info", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
                else if (((TaskObject.PlatformEnum) source[0]) == TaskObject.PlatformEnum.footaction)
                {
                    MessageBox.Show("Mass edit feature for footaction coming soon", "Info", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
                else if (((TaskObject.PlatformEnum) source[0]) == TaskObject.PlatformEnum.footlocker)
                {
                    MessageBox.Show("Mass edit feature for footlocker coming soon", "Info", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
                else if (((TaskObject.PlatformEnum) source[0]) == TaskObject.PlatformEnum.mcm)
                {
                    MessageBox.Show("Mass edit feature for mcm coming soon", "Info", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
                else if (((TaskObject.PlatformEnum) source[0]) == TaskObject.PlatformEnum.solebox)
                {
                    MessageBox.Show("Mass edit feature for solebox coming soon", "Info", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
                else
                {
                    new MassEditWindow((Window) Global.MAIN_WINDOW, ids, source[0]).ShowDialog();
                }
            }
        }

        private void BtnNewTask_Click(object sender, RoutedEventArgs e)
        {
            TaskWindow2 window = new TaskWindow2((Window) Global.MAIN_WINDOW);
            window.ShowDialog();
            if (window.RunAfterSave)
            {
                List<string> ids = new List<string>();
                foreach (string id in window.RunIds)
                {
                    TaskObject obj2 = Global.SETTINGS.TASKS.First<TaskObject>(x => x.Id == id);
                    if ((obj2.State != TaskObject.StateEnum.running) && (obj2.State != TaskObject.StateEnum.waiting))
                    {
                        ids.Add(obj2.Id);
                    }
                }
                TaskManager.StartTasks(ids);
            }
        }

        private void BtnQuickTask_Click(object sender, RoutedEventArgs e)
        {
            if (Helpers.QuickEnabled())
            {
                new QuickTaskWindow((Window) Global.MAIN_WINDOW).ShowDialog();
            }
        }

        private void BtnStartTask_Click(object sender, RoutedEventArgs e)
        {
            if (this.gvTasks.SelectedItems.Count > 0)
            {
                List<string> ids = new List<string>();
                foreach (TaskObject obj2 in this.gvTasks.SelectedItems)
                {
                    if ((obj2.State != TaskObject.StateEnum.running) && (obj2.State != TaskObject.StateEnum.waiting))
                    {
                        ids.Add(obj2.Id);
                    }
                }
                TaskManager.StartTasks(ids);
            }
        }

        private void BtnStopTasks_Click(object sender, RoutedEventArgs e)
        {
            if (this.gvTasks.SelectedItems.Count > 0)
            {
                List<string> ids = new List<string>();
                foreach (TaskObject obj2 in this.gvTasks.SelectedItems)
                {
                    if ((obj2.State != TaskObject.StateEnum.stopped) && (obj2.State != TaskObject.StateEnum.error))
                    {
                        ids.Add(obj2.Id);
                        States.WriteLogger(obj2, States.LOGGER_STATES.TASK_FINISHED, null, "", "");
                    }
                }
                TaskManager.StopTasks(ids);
            }
        }

        private void contextBtnAdd_Click(object sender, RoutedEventArgs e)
        {
            this.BtnNewTask_Click(null, null);
        }

        private void contextBtnDelete_Click(object sender, RoutedEventArgs e)
        {
            this.BtnDeleteTask_Click(null, null);
        }

        private void contextBtnDuplicate_Click(object sender, RoutedEventArgs e)
        {
            this.BtnDuplicateTasks_Click(null, null);
        }

        private void contextBtnEdit_Click(object sender, RoutedEventArgs e)
        {
            this.BtnEditTask_Click(null, null);
        }

        private void contextBtnMassLinkChange_Click(object sender, RoutedEventArgs e)
        {
            new MassLinkChangeWindow((Window) Global.MAIN_WINDOW, MassLinkChangeWindow.MassLinkChangeModeEnum.SPECIFIC).ShowDialog();
        }

        private void contextBtnOpenBrowser_Click(object sender, RoutedEventArgs e)
        {
            if (this.gvTasks.SelectedItems.Count == 1)
            {
                TaskObject task = (TaskObject) this.gvTasks.SelectedItems[0];
                Task.Factory.StartNew(delegate {
                    try
                    {
                        ChromeDriverService service = ChromeDriverService.CreateDefaultService();
                        service.HideCommandPromptWindow = true;
                        ChromeOptions options = new ChromeOptions();
                        if ((!task.PaypalProxyIgnore && !string.IsNullOrEmpty(task.ProxyListId)) && Global.SETTINGS.PROXIES.Any<ProxyListObject>(x => (x.Id == task.ProxyListId)))
                        {
                            ProxyObject proxy = Helpers.GetProxy(Global.SETTINGS.PROXIES.First<ProxyListObject>(x => x.Id == task.ProxyListId));
                            if (proxy != null)
                            {
                                options.AddArgument("ignore-certificate-errors");
                                string str3 = Guid.NewGuid().ToString();
                                string str2 = Helpers.Decrypt(File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"\ext\manifest.json"));
                                string str = Helpers.Decrypt(File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"\ext\background.js")).Replace("{0}", proxy.IP).Replace("{1}", proxy.Port.ToString()).Replace("{2}", proxy.Username).Replace("{3}", proxy.Password);
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
                            }
                        }
                        options.AddArgument("--user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.87 Safari/537.36");
                        task.Driver = new ChromeDriver(service, options);
                        Global.ChromeDrivers.Add(task.Driver);
                        if (task.Platform != TaskObject.PlatformEnum.supreme)
                        {
                            if (task.Platform == TaskObject.PlatformEnum.footlockerau)
                            {
                                task.Driver.Navigate().GoToUrl("https://www.footlocker.com.au");
                            }
                            else if (task.Platform == TaskObject.PlatformEnum.footlockereu)
                            {
                                task.Driver.Navigate().GoToUrl("https://www.footlocker.eu");
                            }
                            else
                            {
                                task.PaypalRefresh = new DateTime?(DateTime.Now);
                                task.Driver.Navigate().GoToUrl("https://www.paypal.com");
                            }
                        }
                        else
                        {
                            task.Driver.Navigate().GoToUrl("https://www.gmail.com");
                        }
                        if (!string.IsNullOrEmpty(task.GmailEmail) && !string.IsNullOrEmpty(task.GmailPassword))
                        {
                            IWebElement element1 = task.Driver.FindElement(By.Id("identifierId"));
                            element1.Clear();
                            element1.SendKeys(task.GmailEmail);
                            task.Driver.FindElement(By.XPath("//span[.='Next']")).Click();
                            new WebDriverWait(task.Driver, TimeSpan.FromSeconds(5.0)).Until<IWebElement>(ExpectedConditions.ElementExists(By.Name("password")));
                            IWebElement element2 = task.Driver.FindElement(By.Name("password"));
                            element2.Clear();
                            element2.SendKeys(task.GmailPassword);
                            Thread.Sleep(500);
                            task.Driver.FindElement(By.XPath("//span[.='Next']")).Click();
                        }
                    }
                    catch (Exception)
                    {
                    }
                });
            }
        }

        private void contextBtnSchedule_Click(object sender, RoutedEventArgs e)
        {
            ScheduleWindow window = new ScheduleWindow((Window) Global.MAIN_WINDOW);
            bool? nullable = window.ShowDialog();
            if ((nullable.GetValueOrDefault() ? nullable.HasValue : false) && (this.gvTasks.SelectedItems.Count > 0))
            {
                List<string> list = new List<string>();
                foreach (object obj3 in this.gvTasks.SelectedItems)
                {
                    if (obj3 is TaskObject)
                    {
                        list.Add(((TaskObject) obj3).Id);
                    }
                }
                foreach (string id in list)
                {
                    TaskObject obj2 = Global.SETTINGS.TASKS.First<TaskObject>(x => x.Id == id);
                    if (((obj2.State == TaskObject.StateEnum.error) || (obj2.State == TaskObject.StateEnum.stopped)) || ((obj2.State == TaskObject.StateEnum.scheduled) || (obj2.State == TaskObject.StateEnum.success)))
                    {
                        obj2.State = TaskObject.StateEnum.scheduled;
                        obj2.IsStartScheduled = true;
                        if (obj2.Platform != TaskObject.PlatformEnum.shopify)
                        {
                            obj2.ScheduleStart = window.PickedTime;
                        }
                        else
                        {
                            obj2.ScheduleStart = window.PickedTime.AddMinutes(-5.0);
                        }
                    }
                }
                Helpers.SaveSettings();
            }
        }

        private void contextBtnScheduleStop_Click(object sender, RoutedEventArgs e)
        {
            ScheduleWindow window = new ScheduleWindow((Window) Global.MAIN_WINDOW);
            bool? nullable = window.ShowDialog();
            if ((nullable.GetValueOrDefault() ? nullable.HasValue : false) && (this.gvTasks.SelectedItems.Count > 0))
            {
                List<string> list = new List<string>();
                foreach (object obj2 in this.gvTasks.SelectedItems)
                {
                    if (obj2 is TaskObject)
                    {
                        list.Add(((TaskObject) obj2).Id);
                    }
                }
                foreach (string id in list)
                {
                    TaskObject local1 = Global.SETTINGS.TASKS.First<TaskObject>(x => x.Id == id);
                    local1.IsStopScheduled = true;
                    local1.ScheduleStop = window.PickedTime;
                }
                Helpers.SaveSettings();
            }
        }

        private void contextBtnSelectAll_Click(object sender, RoutedEventArgs e)
        {
            this.gvTasks.SelectAll();
        }

        private void contextBtnStart_Click(object sender, RoutedEventArgs e)
        {
            this.BtnStartTask_Click(null, null);
        }

        private void contextBtnStartGroup_Click(object sender, RoutedEventArgs e)
        {
            if (this.gvTasks.SelectedItems.Count > 0)
            {
                TaskObject one = (TaskObject) this.gvTasks.SelectedItems[0];
                List<string> ids = new List<string>();
                foreach (TaskObject obj2 in from x in Global.SETTINGS.TASKS
                    where x.Platform == one.Platform
                    select x)
                {
                    if ((obj2.State != TaskObject.StateEnum.running) && (obj2.State != TaskObject.StateEnum.waiting))
                    {
                        ids.Add(obj2.Id);
                    }
                }
                TaskManager.StartTasks(ids);
            }
        }

        private void contextBtnStop_Click(object sender, RoutedEventArgs e)
        {
            this.BtnStopTasks_Click(null, null);
        }

        private void contextBtnStopGroup_Click(object sender, RoutedEventArgs e)
        {
            if (this.gvTasks.SelectedItems.Count > 0)
            {
                TaskObject one = (TaskObject) this.gvTasks.SelectedItems[0];
                List<string> ids = new List<string>();
                foreach (TaskObject obj2 in from x in Global.SETTINGS.TASKS
                    where x.Platform == one.Platform
                    select x)
                {
                    if ((obj2.State != TaskObject.StateEnum.stopped) && (obj2.State != TaskObject.StateEnum.error))
                    {
                        ids.Add(obj2.Id);
                        States.WriteLogger(obj2, States.LOGGER_STATES.TASK_FINISHED, null, "", "");
                    }
                }
                TaskManager.StopTasks(ids);
            }
        }

        public static Visual GetDescendantByType(Visual element, Type type)
        {
            if (element == null)
            {
                return null;
            }
            if (element.GetType() == type)
            {
                return element;
            }
            Visual descendantByType = null;
            if (element is FrameworkElement)
            {
                (element as FrameworkElement).ApplyTemplate();
            }
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
            {
                descendantByType = GetDescendantByType(VisualTreeHelper.GetChild(element, i) as Visual, type);
                if (descendantByType != null)
                {
                    return descendantByType;
                }
            }
            return descendantByType;
        }

        private void gvTasks_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            this.contextBtnEdit.IsEnabled = this.gvTasks.SelectedItems.Count == 1;
            if (this.gvTasks.SelectedItems.Count != 1)
            {
                this.contextBtnOpenBrowser.Visibility = Visibility.Collapsed;
            }
            else
            {
                TaskObject obj2 = (TaskObject) this.gvTasks.SelectedItems[0];
                if (((((obj2.Platform != TaskObject.PlatformEnum.supreme) || (obj2.SupremeAutomation != TaskObject.SuprimeAutomationEnum.browser)) && ((obj2.Platform != TaskObject.PlatformEnum.shopify) || (obj2.Payment != TaskObject.PaymentEnum.paypal))) && (((obj2.Platform != TaskObject.PlatformEnum.footlockerau) || (obj2.Payment != TaskObject.PaymentEnum.paypal)) && ((obj2.Platform != TaskObject.PlatformEnum.footlockereu) || (obj2.Payment != TaskObject.PaymentEnum.paypal)))) && ((obj2.Platform != TaskObject.PlatformEnum.solebox) || (obj2.Payment != TaskObject.PaymentEnum.paypal)))
                {
                    this.contextBtnOpenBrowser.Visibility = Visibility.Collapsed;
                }
                else
                {
                    this.contextBtnOpenBrowser.Visibility = Visibility.Visible;
                }
            }
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
            switch (((0x419f15a0 ^ 0x7c99b785) % 4))
            {
                case 0:
                    goto Label_000D;

                case 1:
                    return;

                case 2:
                    return;

                case 3:
                    break;

                default:
                    return;
            }
        Label_002C:
            this._contentLoaded = true;
            Uri resourceLocator = new Uri("/EveAIO;component/views/dashboardview.xaml", UriKind.Relative);
            Application.LoadComponent(this, resourceLocator);
            goto Label_000D;
        }

        private void MassLinkChange_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.contextBtnMassLinkChange_Click(null, null);
        }

        private void Row_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((sender != null) && (this.gvTasks.SelectedItems.Count == 1))
            {
                TaskObject obj2 = (TaskObject) this.gvTasks.SelectedItems[0];
                if (((obj2.State == TaskObject.StateEnum.stopped) || (obj2.State == TaskObject.StateEnum.error)) || ((obj2.State == TaskObject.StateEnum.success) || (obj2.State == TaskObject.StateEnum.smartWaiting)))
                {
                    List<string> ids = new List<string> {
                        obj2.Id
                    };
                    TaskManager.StartTasks(ids);
                }
                else
                {
                    List<string> ids = new List<string> {
                        obj2.Id
                    };
                    TaskManager.StopTasks(ids);
                }
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    this.BtnNewTask = (Button) target;
                    this.BtnNewTask.Click += new RoutedEventHandler(this.BtnNewTask_Click);
                    return;

                case 2:
                    this.BtnEditTask = (Button) target;
                    this.BtnEditTask.Click += new RoutedEventHandler(this.BtnEditTask_Click);
                    return;

                case 3:
                    this.BtnMassEditTask = (Button) target;
                    this.BtnMassEditTask.Click += new RoutedEventHandler(this.BtnMassEditTask_Click);
                    return;

                case 4:
                    this.BtnDeleteTask = (Button) target;
                    this.BtnDeleteTask.Click += new RoutedEventHandler(this.BtnDeleteTask_Click);
                    return;

                case 5:
                    this.BtnDuplicateTasks = (Button) target;
                    this.BtnDuplicateTasks.Click += new RoutedEventHandler(this.BtnDuplicateTasks_Click);
                    return;

                case 6:
                    this.BtnImportTask = (Button) target;
                    this.BtnImportTask.Click += new RoutedEventHandler(this.BtnImportTask_Click);
                    return;

                case 7:
                    this.BtnExportTasks = (Button) target;
                    this.BtnExportTasks.Click += new RoutedEventHandler(this.BtnExportTasks_Click);
                    return;

                case 8:
                    this.BtnStartTask = (Button) target;
                    this.BtnStartTask.Click += new RoutedEventHandler(this.BtnStartTask_Click);
                    return;

                case 9:
                    this.BtnStopTasks = (Button) target;
                    this.BtnStopTasks.Click += new RoutedEventHandler(this.BtnStopTasks_Click);
                    return;

                case 10:
                    this.BtnQuickTask = (Button) target;
                    this.BtnQuickTask.Click += new RoutedEventHandler(this.BtnQuickTask_Click);
                    return;

                case 11:
                    this.gvTasks = (DataGrid) target;
                    this.gvTasks.ContextMenuOpening += new ContextMenuEventHandler(this.gvTasks_ContextMenuOpening);
                    return;

                case 12:
                    this.contextBtnStart = (MenuItem) target;
                    this.contextBtnStart.Click += new RoutedEventHandler(this.contextBtnStart_Click);
                    return;

                case 13:
                    this.contextBtnStop = (MenuItem) target;
                    this.contextBtnStop.Click += new RoutedEventHandler(this.contextBtnStop_Click);
                    return;

                case 14:
                    this.contextBtnStartGroup = (MenuItem) target;
                    this.contextBtnStartGroup.Click += new RoutedEventHandler(this.contextBtnStartGroup_Click);
                    return;

                case 15:
                    this.contextBtnStopGroup = (MenuItem) target;
                    this.contextBtnStopGroup.Click += new RoutedEventHandler(this.contextBtnStopGroup_Click);
                    return;

                case 0x10:
                    this.contextBtnAdd = (MenuItem) target;
                    this.contextBtnAdd.Click += new RoutedEventHandler(this.contextBtnAdd_Click);
                    return;

                case 0x11:
                    this.contextBtnEdit = (MenuItem) target;
                    this.contextBtnEdit.Click += new RoutedEventHandler(this.contextBtnEdit_Click);
                    return;

                case 0x12:
                    this.contextBtnDelete = (MenuItem) target;
                    this.contextBtnDelete.Click += new RoutedEventHandler(this.contextBtnDelete_Click);
                    return;

                case 0x13:
                    this.contextBtnDuplicate = (MenuItem) target;
                    this.contextBtnDuplicate.Click += new RoutedEventHandler(this.contextBtnDuplicate_Click);
                    return;

                case 20:
                    this.contextBtnSelectAll = (MenuItem) target;
                    this.contextBtnSelectAll.Click += new RoutedEventHandler(this.contextBtnSelectAll_Click);
                    return;

                case 0x15:
                    this.contextBtnMassLinkChange = (MenuItem) target;
                    this.contextBtnMassLinkChange.Click += new RoutedEventHandler(this.contextBtnMassLinkChange_Click);
                    return;

                case 0x16:
                    this.contextBtnSchedule = (MenuItem) target;
                    this.contextBtnSchedule.Click += new RoutedEventHandler(this.contextBtnSchedule_Click);
                    return;

                case 0x17:
                    this.contextBtnScheduleStop = (MenuItem) target;
                    this.contextBtnScheduleStop.Click += new RoutedEventHandler(this.contextBtnScheduleStop_Click);
                    return;

                case 0x18:
                    this.contextBtnOpenBrowser = (MenuItem) target;
                    this.contextBtnOpenBrowser.Click += new RoutedEventHandler(this.contextBtnOpenBrowser_Click);
                    return;
            }
            this._contentLoaded = true;
        }

        [DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0"), EditorBrowsable(EditorBrowsableState.Never)]
        void IStyleConnector.Connect(int connectionId, object target)
        {
            if (connectionId == 0x19)
            {
                EventSetter item = new EventSetter {
                    Event = Control.MouseDoubleClickEvent,
                    Handler = new MouseButtonEventHandler(this.Row_MouseDoubleClick)
                };
                ((Style) target).Setters.Add(item);
            }
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly DashboardView.<>c <>9;
            public static Func<TaskObject, bool> <>9__4_0;
            public static Func<TaskObject, bool> <>9__4_1;

            static <>c()
            {
                Class7.RIuqtBYzWxthF();
                <>9 = new DashboardView.<>c();
            }

            public <>c()
            {
                Class7.RIuqtBYzWxthF();
            }

            internal bool <_scheduler_Tick>b__4_0(TaskObject x) => 
                x.IsStartScheduled;

            internal bool <_scheduler_Tick>b__4_1(TaskObject x) => 
                x.IsStopScheduled;
        }
    }
}

