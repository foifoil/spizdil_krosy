namespace EveAIO.Tasks
{
    using EveAIO;
    using EveAIO.Notifications;
    using EveAIO.Pocos;
    using EveAIO.Tasks.Dto;
    using EveAIO.Tasks.Platforms;
    using HtmlAgilityPack;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    public class TaskRunner
    {
        private TaskObject _task;
        public Random Rnd;
        public CookieContainer Cookies;
        public WebProxy Proxy;
        public bool IsError;
        public KeyValuePair<string, string>? PickedSize;
        public NordStromProduct PickedNordstromSize;
        public BarneysProduct PickedBarneysSize;
        public PumaProduct PickedPumaSize;
        public EveAIO.Tasks.Product Product;
        public HtmlDocument ProductPageHtml;
        public HtmlDocument CartPageHtml;
        public HtmlDocument AddHtml;
        public HtmlDocument CheckoutHtml;
        public string HomeUrl;
        public string CartResponseUri;
        internal Notificator _notificator;
        internal SuccessObject Success;
        public string CheckoutUrl;
        public ProfileObject Profile;
        public bool ForcedPaypal;
        public bool IsSmartCheckoutReady;
        public int PaymentMethod;
        public Stopwatch _watch;
        public DateTime? _tokenTimestamp;
        public bool _proxyPicked;
        public bool _dontRotateProxy;
        public List<string> _watcherTasks;

        public TaskRunner(TaskObject task)
        {
            Class7.RIuqtBYzWxthF();
            this._watcherTasks = new List<string>();
            this._task = task;
            this.Rnd = new Random(this._task.Guid.GetHashCode() + DateTime.Now.Millisecond);
        }

        private void Checkout()
        {
            if (this._task.Platform == TaskObject.PlatformEnum.supremeinstore)
            {
                States.WriteLogger(this._task, States.LOGGER_STATES.SUCCESSFULLY_REGISTERED, null, "", "");
            }
            else
            {
                States.WriteLogger(this._task, States.LOGGER_STATES.SUCCESSFULY_CHECKED_OUT, null, "", "");
            }
            object successLocker = Global.SuccessLocker;
            lock (successLocker)
            {
                Global.ViewLog.txtLog.Dispatcher.Invoke(delegate {
                    this.Success.CheckoutHidden = 0;
                    int index = Global.SUCCESS.IndexOf(Global.SUCCESS.First<SuccessObject>(x => x.TaskId == this._task.Id));
                    SuccessObject item = Global.SUCCESS.First<SuccessObject>(x => x.TaskId == this._task.Id);
                    Global.SUCCESS.RemoveAt(index);
                    Global.SUCCESS.Insert(0, item);
                });
            }
            Task.Factory.StartNew(delegate {
                try
                {
                    string contents = $"Task: {this._task.Name}
Profile: {this.Profile.Name}
Product: {this.Product.ProductTitle}
Size: {this.PickedSize.Value.Key}
Store: {this._task.HomeUrl}
Datetime: {DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToString("HH:mm:ss:fff")}";
                    System.IO.File.WriteAllText("checkouts/" + Guid.NewGuid().ToString() + ".txt", contents);
                }
                catch (Exception)
                {
                }
            });
            EveAIO.Helpers.Notify("checkout", this.Product.Link + ";" + (((this._task.Payment != TaskObject.PaymentEnum.creditcard) || this.ForcedPaypal) ? "paypal" : "credit card") + ";" + this.Profile.Country);
            if (this._notificator != null)
            {
                this._notificator.Notify(Notificator.NotificationType.Checkout);
            }
            if (this.Profile.OnePerWebsite)
            {
                Global.SUCCESS_PROFILES.Add(new KeyValuePair<string, string>(this._task.CheckoutId, this.HomeUrl));
            }
            this._task.State = TaskObject.StateEnum.success;
            if (Global.SETTINGS.TASKS.Any<TaskObject>(x => (x.Platform == TaskObject.PlatformEnum.supreme) && (x.ParentId == this._task.Id)))
            {
                foreach (TaskObject obj4 in from x in Global.SETTINGS.TASKS
                    where (x.Platform == TaskObject.PlatformEnum.supreme) && (x.ParentId == this._task.Id)
                    select x)
                {
                    if (!string.IsNullOrEmpty(obj4.PickedSize))
                    {
                        obj4.State = TaskObject.StateEnum.success;
                    }
                }
            }
            else if (Global.SETTINGS.TASKS.Any<TaskObject>(x => (x.Platform == TaskObject.PlatformEnum.shopify) && (x.ParentId == this._task.Id)))
            {
                foreach (TaskObject obj3 in from x in Global.SETTINGS.TASKS
                    where (x.Platform == TaskObject.PlatformEnum.shopify) && (x.ParentId == this._task.Id)
                    select x)
                {
                    if (!string.IsNullOrEmpty(obj3.PickedSize))
                    {
                        obj3.State = TaskObject.StateEnum.success;
                    }
                }
            }
            this._task.SuccessUrl = this.Product.Link;
            this._task.SuccessCountry = this.Profile.Country;
            this._task.SuccessProductName = this.Product.ProductTitle;
            this._task.SuccessSize = this.PickedSize.Value.Key;
            this._task.Various = "-";
            this._task.Various = "-";
            if (this._task.Platform == TaskObject.PlatformEnum.supreme)
            {
                this._task.Various = "Region: " + this._task.SupremeRegion;
                this._task.Various = this._task.Various + "; Mode: " + this._task.SupremeAutomation;
                this._task.Various = this._task.Various + "; Captcha: " + (this._task.SolveCaptcha ? "true" : "false");
                this._task.Various = this._task.Various + "; Type: " + ((this._task.TaskType == TaskObject.TaskTypeEnum.keywords) ? "kws" : "direct link");
                this._task.Various = this._task.Various + "; Payment: " + ((this._task.Payment == TaskObject.PaymentEnum.creditcard) ? "credit card" : "cash on delivery");
            }
            else
            {
                string str2;
                if (this._task.Platform == TaskObject.PlatformEnum.offwhite)
                {
                    str2 = "";
                    switch (this._task.Payment)
                    {
                        case TaskObject.PaymentEnum.creditcard:
                            str2 = "credit card";
                            break;

                        case TaskObject.PaymentEnum.paypal:
                            str2 = "PayPal";
                            break;

                        case TaskObject.PaymentEnum.bankTransfer:
                            str2 = "bank transfer";
                            break;
                    }
                }
                else
                {
                    if (this._task.Platform != TaskObject.PlatformEnum.solebox)
                    {
                        if (this._task.Platform == TaskObject.PlatformEnum.mesh)
                        {
                            this._task.Various = "Carting: " + ((this._task.AtcMethod == TaskObject.AtcMethodEnum.frontend) ? "frondend" : "backend");
                        }
                    }
                    else
                    {
                        string str = "";
                        switch (this._task.Payment)
                        {
                            case TaskObject.PaymentEnum.creditcard:
                                str = "credit card";
                                break;

                            case TaskObject.PaymentEnum.paypal:
                                str = "PayPal";
                                break;

                            case TaskObject.PaymentEnum.bankTransfer:
                                str = "bank transfer";
                                break;
                        }
                        this._task.Various = "Payment: " + str;
                    }
                    goto Label_04F2;
                }
                this._task.Various = "Payment: " + str2;
            }
        Label_04F2:
            if (Global.SETTINGS.CheckoutBeep)
            {
                EveAIO.Helpers.PlayBell();
            }
            if (((this._task.State == TaskObject.StateEnum.success) && ((this._task.Payment == TaskObject.PaymentEnum.creditcard) || (this._task.Payment == TaskObject.PaymentEnum.bankTransfer))) && !this.ForcedPaypal)
            {
                string speed = "-";
                try
                {
                    if ((this._watch != null) && !this._watch.IsRunning)
                    {
                        speed = this._watch.Elapsed.TotalSeconds.ToString() + "s";
                    }
                }
                catch
                {
                }
                Notificator.SendSuccess(this._task, speed);
                EveAIO.Helpers.NotifySuccess(this._task.HomeUrl, this.Product.ProductTitle, this._task.SuccessSize);
            }
        }

        private void LocalIpReset()
        {
            if ((this.Proxy != null) && this._task.LocalIPCheckout)
            {
                this.Proxy = null;
                if (!this.IsSmartCheckoutReady)
                {
                    this.Cookies = new CookieContainer();
                }
            }
        }

        internal void SetProxy()
        {
            if (((((this._task.Platform != TaskObject.PlatformEnum.oneblockdown) || (this.Cookies == null)) || (this.Cookies.Count <= 0)) && ((this._task.Platform != TaskObject.PlatformEnum.offwhite) || !this._dontRotateProxy)) && (!string.IsNullOrEmpty(this._task.ProxyListId) && Global.SETTINGS.PROXIES.Any<ProxyListObject>(x => (x.Id == this._task.ProxyListId))))
            {
                ProxyObject proxy = EveAIO.Helpers.GetProxy(Global.SETTINGS.PROXIES.First<ProxyListObject>(x => x.Id == this._task.ProxyListId), this.Rnd);
                if (proxy != null)
                {
                    if (!string.IsNullOrEmpty(proxy.Username))
                    {
                        this.Proxy = new WebProxy(proxy.IP, proxy.Port);
                        ICredentials credentials = new NetworkCredential(proxy.Username, proxy.Password);
                        this.Proxy.Credentials = credentials;
                    }
                    else
                    {
                        this.Proxy = new WebProxy(proxy.IP, proxy.Port);
                    }
                    this._task.Proxy = proxy.IP + ":" + proxy.Port;
                }
            }
        }

        private void SetSuccess()
        {
            SuccessObject obj1 = new SuccessObject {
                Id = Guid.NewGuid().ToString(),
                TaskId = this._task.Id,
                TaskName = this._task.Name,
                Time = new DateTime?(DateTime.Now),
                IsMultiCart = this._task.IsMultiCartParent,
                Store = EveAIO.Helpers.GetStoreUrl(this._task)
            };
            this.Success = obj1;
            if (((this._task.Platform == TaskObject.PlatformEnum.solebox) || (this._task.Platform == TaskObject.PlatformEnum.sivasdescalzo)) || (this._task.Platform == TaskObject.PlatformEnum.holypopstore))
            {
                this.Success.LoginHidden = 0;
            }
            if (((this._task.TaskType != TaskObject.TaskTypeEnum.variant) && (!this._task.Link.ToLowerInvariant().Contains("/cart/") || !this._task.Link.ToLowerInvariant().Contains(":1"))) && (((this._task.Platform != TaskObject.PlatformEnum.mrporter) || ((this._task.Platform == TaskObject.PlatformEnum.mrporter) && (!this._task.Link.ToLowerInvariant().Contains("-") || this._task.Link.ToLowerInvariant().Contains("https")))) && ((this._task.Platform != TaskObject.PlatformEnum.solebox) || (((this._task.Platform == TaskObject.PlatformEnum.solebox) && !this._task.Link.Contains("anid")) && !this._task.Link.Contains("aid")))))
            {
                this.Success.Size = this.PickedSize.Value.Key;
                this.Success.Price = this.Product.Price;
                this.Success.Link = this.Product.Link;
                this.Success.ProductImage = this._task.ImgUrl;
                this.Success.ProductName = this.Product.ProductTitle;
                if (this.Proxy != null)
                {
                    this.Success.Proxy = this._task.Proxy;
                }
                else
                {
                    this.Success.Proxy = "-";
                }
                Global.ViewSuccess.listSuccess.Dispatcher.BeginInvoke(delegate {
                    if (!Global.SUCCESS.Any<SuccessObject>(x => (x.TaskId == this._task.Id)))
                    {
                        this.Success.Repetitions = 1;
                        Global.SUCCESS.Add(this.Success);
                    }
                    else
                    {
                        SuccessObject obj2 = Global.SUCCESS.First<SuccessObject>(x => x.TaskId == this._task.Id);
                        this.Success.Repetitions = obj2.Repetitions + 1;
                        int index = Global.SUCCESS.IndexOf(Global.SUCCESS.First<SuccessObject>(x => x.TaskId == this._task.Id));
                        Global.SUCCESS[index] = this.Success;
                    }
                }, Array.Empty<object>());
            }
        }

        public void Start()
        {
            try
            {
                DateTime? shopifySmartSchedule;
                HtmlNode.ElementsFlags.Remove("form");
                HtmlNode.ElementsFlags.Remove("option");
                Global.ProxyCheck();
                if (!Global.SETTINGS.PROFILES.Any<ProfileObject>(x => (x.Id == this._task.CheckoutId)))
                {
                    goto Label_193C;
                }
                this.Profile = Global.SETTINGS.PROFILES.First<ProfileObject>(x => x.Id == this._task.CheckoutId);
                this._task.Proxy = "";
                this._task.IsStartScheduled = false;
                this._task.Starting = true;
                this._task.IsMultiCartParent = false;
                IPlatform platform = null;
                this.HomeUrl = this._task.Link;
                this._task.HomeUrl = this.HomeUrl;
                if ((this._task.TaskType == TaskObject.TaskTypeEnum.directlink) && (this._task.Platform != TaskObject.PlatformEnum.supremeinstore))
                {
                    if (this._task.IsShopifyCheckoutLink)
                    {
                        this.HomeUrl = this._task.ShopifyCheckoutLink;
                    }
                    int startIndex = this.HomeUrl.IndexOf("//") + 2;
                    int index = this.HomeUrl.Substring(startIndex).IndexOf("/");
                    if (index != -1)
                    {
                        this.HomeUrl = this.HomeUrl.Substring(0, startIndex + index);
                    }
                    this._task.HomeUrl = this.HomeUrl;
                }
                switch (this._task.Platform)
                {
                    case TaskObject.PlatformEnum.shopify:
                        this.SetProxy();
                        this._proxyPicked = true;
                        this.Cookies = new CookieContainer();
                        if ((((!this._task.ShopifyIsOldMode && !this._task.HomeUrl.Contains("store.travisscott.com")) && (!this._task.HomeUrl.Contains("shop-jp.palaceskateboards.com") && !this._task.SmartCheckout)) && (((this._task.Payment != TaskObject.PaymentEnum.paypal) && !this._task.IsShopifyCheckoutLink) && !this._task.Link.Contains("/checkouts/"))) && (!WebsitesInfo.SHOPIFY_WEBS.Any<ShopifyWebsiteInfo>(x => (x.Website == this._task.ShopifyWebsite)) || !WebsitesInfo.SHOPIFY_WEBS.First<ShopifyWebsiteInfo>(x => (x.Website == this._task.ShopifyWebsite)).PaypalOnly))
                        {
                            platform = new Shopify2(this, this._task);
                        }
                        else
                        {
                            platform = new Shopify(this, this._task);
                        }
                        break;

                    case TaskObject.PlatformEnum.sivasdescalzo:
                        this.SetProxy();
                        this._proxyPicked = true;
                        this.Cookies = new CookieContainer();
                        platform = new Sivasdescalzo(this, this._task);
                        break;

                    case TaskObject.PlatformEnum.solebox:
                        this.SetProxy();
                        this._proxyPicked = true;
                        this.Cookies = new CookieContainer();
                        platform = new Solebox(this, this._task);
                        break;

                    case TaskObject.PlatformEnum.mrporter:
                        this.SetProxy();
                        this._proxyPicked = true;
                        this.Cookies = new CookieContainer();
                        platform = new MrPorter(this, this._task);
                        break;

                    case TaskObject.PlatformEnum.holypopstore:
                        this.SetProxy();
                        this._proxyPicked = true;
                        this.Cookies = new CookieContainer();
                        platform = new Holypopstore(this, this._task);
                        break;

                    case TaskObject.PlatformEnum.offwhite:
                        this.SetProxy();
                        this._proxyPicked = true;
                        this.Cookies = new CookieContainer();
                        platform = new OffWhite(this, this._task);
                        break;

                    case TaskObject.PlatformEnum.titolo:
                        this.SetProxy();
                        this._proxyPicked = true;
                        this.Cookies = new CookieContainer();
                        platform = new Titolo(this, this._task);
                        break;

                    case TaskObject.PlatformEnum.oneblockdown:
                        this.SetProxy();
                        this._proxyPicked = true;
                        this.Cookies = new CookieContainer();
                        platform = new Oneblockdown(this, this._task);
                        break;

                    case TaskObject.PlatformEnum.boxlunch:
                        this.SetProxy();
                        this._proxyPicked = true;
                        this.Cookies = new CookieContainer();
                        platform = new Boxlunch(this, this._task);
                        break;

                    case TaskObject.PlatformEnum.sneakersnstuff:
                        this.SetProxy();
                        this._proxyPicked = true;
                        this.Cookies = new CookieContainer();
                        platform = new Sneakernstuff(this, this._task);
                        break;

                    case TaskObject.PlatformEnum.footaction:
                        this.SetProxy();
                        this._proxyPicked = true;
                        this.Cookies = new CookieContainer();
                        platform = new Footaction(this, this._task);
                        break;

                    case TaskObject.PlatformEnum.footlocker:
                        this.SetProxy();
                        this._proxyPicked = true;
                        this.Cookies = new CookieContainer();
                        platform = new Footlocker(this, this._task);
                        break;

                    case TaskObject.PlatformEnum.finishline:
                        this.SetProxy();
                        this._proxyPicked = true;
                        this.Cookies = new CookieContainer();
                        platform = new Finishline(this, this._task);
                        break;

                    case TaskObject.PlatformEnum.hottopic:
                        this.SetProxy();
                        this._proxyPicked = true;
                        this.Cookies = new CookieContainer();
                        platform = new Hottopic(this, this._task);
                        break;
                }
                States.WriteLogger(this._task, States.LOGGER_STATES.TASK_INIT, null, "", "");
                bool flag = true;
                foreach (TaskObject obj2 in from x in Global.SETTINGS.TASKS
                    where (!string.IsNullOrEmpty(x.WatchTaskId) && (x.WatchTaskId == this._task.Id)) && (x.WatchTaskHold == null)
                    select x)
                {
                    this._watcherTasks.Add(obj2.Id);
                }
                if (this._watcherTasks.Count > 0)
                {
                    TaskManager.StartTasks(this._watcherTasks);
                }
                goto Label_1919;
            Label_0578:
                this.Profile = Global.SETTINGS.PROFILES.First<ProfileObject>(x => x.Id == this._task.CheckoutId);
                this.ForcedPaypal = false;
                this._task.Starting = false;
                this._task.PickedSize = "";
                this._task.ManualSolved = false;
                this._watch = new Stopwatch();
                this._watch.Start();
                this.IsSmartCheckoutReady = false;
                this._task.CaptchaSolverAssigned = false;
                try
                {
                    if (this._task.Payment == TaskObject.PaymentEnum.paypal)
                    {
                        shopifySmartSchedule = this._task.PaypalRefresh;
                        if (shopifySmartSchedule.HasValue)
                        {
                            shopifySmartSchedule = this._task.PaypalRefresh;
                            if (shopifySmartSchedule.Value.AddMinutes(5.0) < DateTime.Now)
                            {
                                Task.Factory.StartNew(delegate {
                                    try
                                    {
                                        this._task.PaypalRefresh = new DateTime?(DateTime.Now);
                                        this._task.Driver.Navigate().Refresh();
                                    }
                                    catch (Exception)
                                    {
                                    }
                                });
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(this._task.WatchTaskId))
                    {
                        this._task.State = TaskObject.StateEnum.watching;
                        States.WriteLogger(this._task, States.LOGGER_STATES.WAITING_FOR_WATCHING_TASK, null, "", "");
                        this._task.WatchTaskHold = new ManualResetEvent(false);
                        this._task.WatchTaskHold.WaitOne();
                        this._task.State = TaskObject.StateEnum.running;
                        States.WriteLogger(this._task, States.LOGGER_STATES.WOKEN_UP, null, "", "");
                    }
                    if (!this._proxyPicked)
                    {
                        this.Proxy = null;
                    }
                    this.IsError = false;
                    this.PickedSize = null;
                    this.PickedBarneysSize = null;
                    this.PickedNordstromSize = null;
                    this.PickedPumaSize = null;
                    this.Product = null;
                    this.HomeUrl = this._task.Link;
                    this._task.HomeUrl = this.HomeUrl;
                    this.ProductPageHtml = null;
                    this.CartPageHtml = null;
                    this._notificator = null;
                    this._task.SessionsId = "";
                    if (((this._task.TaskType == TaskObject.TaskTypeEnum.directlink) && (this._task.Platform != TaskObject.PlatformEnum.supremeinstore)) && ((this._task.Platform != TaskObject.PlatformEnum.mrporter) || ((this._task.Platform == TaskObject.PlatformEnum.mrporter) && this._task.Link.ToLowerInvariant().Contains("http"))))
                    {
                        if (this._task.IsShopifyCheckoutLink)
                        {
                            this.HomeUrl = this._task.ShopifyCheckoutLink;
                        }
                        int startIndex = this.HomeUrl.IndexOf("//") + 2;
                        int index = this.HomeUrl.Substring(startIndex).IndexOf("/");
                        if (index != -1)
                        {
                            this.HomeUrl = this.HomeUrl.Substring(0, startIndex + index);
                        }
                        this._task.HomeUrl = this.HomeUrl;
                    }
                    this._task.State = TaskObject.StateEnum.running;
                    if (!this._task.ScheduleStartStarting)
                    {
                        shopifySmartSchedule = null;
                        this._task.ShopifySmartSchedule = shopifySmartSchedule;
                    }
                    this._task.ScheduleStartStarting = false;
                    if (!this._proxyPicked)
                    {
                        this.SetProxy();
                    }
                    if (!flag)
                    {
                        platform.SetClient();
                    }
                    flag = false;
                    this._proxyPicked = false;
                    if (((this._task.SmartCheckout && !this.IsSmartCheckoutReady) || !this._tokenTimestamp.HasValue) && (((((this._task.Platform != TaskObject.PlatformEnum.solebox) && (this._task.Platform != TaskObject.PlatformEnum.offwhite)) && (this._task.Platform != TaskObject.PlatformEnum.titolo)) && ((this._task.Platform != TaskObject.PlatformEnum.holypopstore) && ((((this._task.Platform != TaskObject.PlatformEnum.shopify) && (this._task.Platform != TaskObject.PlatformEnum.mrporter)) && (this._task.Platform != TaskObject.PlatformEnum.footaction)) && ((this._task.Platform != TaskObject.PlatformEnum.boxlunch) || ((this._task.Platform == TaskObject.PlatformEnum.boxlunch) && !this._task.Login))))) && ((this._task.Platform != TaskObject.PlatformEnum.hottopic) || ((this._task.Platform == TaskObject.PlatformEnum.hottopic) && !this._task.Login))))
                    {
                        this.Cookies = new CookieContainer();
                    }
                    bool flag2 = true;
                    if (this.Profile.OnePerWebsite && Global.SUCCESS_PROFILES.Any<KeyValuePair<string, string>>(x => ((x.Key == this._task.CheckoutId) && (x.Value == this.HomeUrl))))
                    {
                        this._task.Status = States.GetTaskState(States.TaskState.PROFILE_USED);
                        States.WriteLogger(this._task, States.LOGGER_STATES.PROFILE_ALREADY_USED, null, "", "");
                        flag2 = false;
                        this._task.State = TaskObject.StateEnum.profileUsed;
                        return;
                    }
                    if ((this._task.Platform != TaskObject.PlatformEnum.funko) || ((this.Profile.CountryId == "US") && (this.Profile.CountryIdShipping == "US")))
                    {
                        if (flag2)
                        {
                            switch (this._task.Platform)
                            {
                                case TaskObject.PlatformEnum.supreme:
                                    platform = new Supreme(this, this._task);
                                    break;

                                case TaskObject.PlatformEnum.jimmyjazz:
                                    platform = new JimmyJazz(this, this._task);
                                    break;

                                case TaskObject.PlatformEnum.footsites:
                                    platform = new Footsites(this, this._task);
                                    break;

                                case TaskObject.PlatformEnum.woodwood:
                                    platform = new Woodwood(this, this._task);
                                    break;

                                case TaskObject.PlatformEnum.hibbett:
                                    platform = new Hibbett(this, this._task);
                                    break;

                                case TaskObject.PlatformEnum.nordstrom:
                                    platform = new Nordstrom(this, this._task);
                                    break;

                                case TaskObject.PlatformEnum.mesh:
                                    platform = new Mesh(this, this._task);
                                    break;

                                case TaskObject.PlatformEnum.backdoor:
                                    platform = new Backdoor(this, this._task);
                                    break;

                                case TaskObject.PlatformEnum.barneys:
                                    platform = new Barneys(this, this._task);
                                    break;

                                case TaskObject.PlatformEnum.funko:
                                    platform = new Funko(this, this._task);
                                    break;

                                case TaskObject.PlatformEnum.puma:
                                    platform = new Puma(this, this._task);
                                    break;

                                case TaskObject.PlatformEnum.converse:
                                    platform = new Converse(this, this._task);
                                    break;

                                case TaskObject.PlatformEnum.footlockerau:
                                    platform = new Footlockerau(this, this._task);
                                    break;

                                case TaskObject.PlatformEnum.footlockereu:
                                    platform = new Footlockereu(this, this._task);
                                    break;

                                case TaskObject.PlatformEnum.supremeinstore:
                                    platform = new SupremeInstore(this, this._task);
                                    break;

                                case TaskObject.PlatformEnum.mcm:
                                    platform = new Mcm(this, this._task);
                                    break;

                                case TaskObject.PlatformEnum.sevres:
                                    platform = new Sevres(this, this._task);
                                    break;
                            }
                            if ((this._task.TaskType != TaskObject.TaskTypeEnum.directlink) && (this._task.TaskType != TaskObject.TaskTypeEnum.variant))
                            {
                                this.Product = null;
                                if (platform.Search())
                                {
                                    foreach (TaskObject local1 in from x in Global.SETTINGS.TASKS
                                        where (!string.IsNullOrEmpty(x.WatchTaskId) && (x.WatchTaskId == this._task.Id)) && (x.WatchTaskHold > null)
                                        select x)
                                    {
                                        local1.WatchTaskHold.Set();
                                        local1.WatchTaskHold = null;
                                    }
                                    this.LocalIpReset();
                                    this.SetSuccess();
                                    if (this._task.Notify)
                                    {
                                        this._notificator = new Notificator(this._task, this.Product.Link, this.Product.ProductTitle);
                                    }
                                    if (this._notificator != null)
                                    {
                                        this._notificator.Notify(Notificator.NotificationType.Restock);
                                    }
                                    bool flag5 = true;
                                    if (this._task.Login)
                                    {
                                        this._task.Status = States.GetTaskState(States.TaskState.LOGGING_IN);
                                        if (flag5 = platform.Login())
                                        {
                                            this.Success.LoginHidden = 0;
                                        }
                                        else
                                        {
                                            this.Success.LoginHidden = 1;
                                        }
                                    }
                                    if (flag5)
                                    {
                                        if (platform.Atc() && (Global.SERIAL.Substring(5) != Global.SERIAL.ToUpperInvariant().Substring(5)))
                                        {
                                            if (Global.SETTINGS.AtcBeep)
                                            {
                                                EveAIO.Helpers.PlayBell();
                                            }
                                            this.Success.AtcHidden = 0;
                                            if (this._notificator != null)
                                            {
                                                this._notificator.Notify(Notificator.NotificationType.Atc);
                                            }
                                            this._task.PickedSize = this.PickedSize.Value.Key;
                                            if (!Global.SETTINGS.TASKS.Any<TaskObject>(x => ((x.Platform == TaskObject.PlatformEnum.supreme) && (x.ParentId == this._task.Id))))
                                            {
                                                if (Global.SETTINGS.TASKS.Any<TaskObject>(x => (x.Platform == TaskObject.PlatformEnum.shopify) && (x.ParentId == this._task.Id)))
                                                {
                                                    this._task.IsMultiCartParent = true;
                                                }
                                            }
                                            else
                                            {
                                                List<TaskObject> childTasks = (from x in Global.SETTINGS.TASKS
                                                    where (x.Platform == TaskObject.PlatformEnum.supreme) && (x.ParentId == this._task.Id)
                                                    select x).ToList<TaskObject>();
                                                this._task.IsMultiCartParent = true;
                                                ((Supreme) platform).MultiCart(childTasks);
                                            }
                                            this._task.Status = States.GetTaskState(States.TaskState.CHECKING_OUT);
                                            if (platform.Checkout())
                                            {
                                                this.Checkout();
                                            }
                                            else
                                            {
                                                this.Success.CheckoutHidden = 1;
                                            }
                                        }
                                        else
                                        {
                                            this.Success.AtcHidden = 1;
                                        }
                                    }
                                }
                                else if ((((this._task.Status != "Login unsuccessful") && (this._task.Status != "Browser missing")) && ((this._task.Status != "Cloudfront error") && (this._task.Status != "Page not found"))) && (((this._task.Status != "US IP needed") && (this._task.Status != "IP banned")) && (this._task.Status != "US shipping needed")))
                                {
                                    this._task.Status = States.GetTaskState(States.TaskState.WAITING_FOR_PRODUCT);
                                    States.WriteLogger(this._task, States.LOGGER_STATES.NO_PRODUCTS_FOUND, null, "", "");
                                }
                            }
                            else
                            {
                                bool flag3 = false;
                                if ((((this._task.TaskType == TaskObject.TaskTypeEnum.directlink) && (!this._task.Link.ToLowerInvariant().Contains("/cart/") || !this._task.Link.ToLowerInvariant().Contains(":1"))) && ((this._task.Platform != TaskObject.PlatformEnum.mrporter) || ((this._task.Platform == TaskObject.PlatformEnum.mrporter) && (!this._task.Link.ToLowerInvariant().Contains("-") || (this._task.Link.ToLowerInvariant().Contains("-") && this._task.Link.ToLowerInvariant().Contains("https")))))) && (((this._task.Platform != TaskObject.PlatformEnum.solebox) || ((this._task.Platform == TaskObject.PlatformEnum.solebox) && !this._task.Link.Contains("aid"))) && (flag3 = platform.DirectLink(this._task.Link))))
                                {
                                    foreach (TaskObject local2 in from x in Global.SETTINGS.TASKS
                                        where (!string.IsNullOrEmpty(x.WatchTaskId) && (x.WatchTaskId == this._task.Id)) && (x.WatchTaskHold > null)
                                        select x)
                                    {
                                        local2.WatchTaskHold.Set();
                                        local2.WatchTaskHold = null;
                                    }
                                }
                                if (this.Product != null)
                                {
                                    if (flag3 && this._task.Notify)
                                    {
                                        this._notificator = new Notificator(this._task, this.Product.Link, this.Product.ProductTitle);
                                    }
                                    if (this._notificator != null)
                                    {
                                        this._notificator.Notify(Notificator.NotificationType.Restock);
                                    }
                                }
                                if (((!flag3 && (this._task.TaskType != TaskObject.TaskTypeEnum.variant)) && ((((this._task.TaskType != TaskObject.TaskTypeEnum.directlink) || !this._task.Link.ToLowerInvariant().Contains("/cart/")) || !this._task.Link.ToLowerInvariant().Contains(":1")) && (((this._task.Platform != TaskObject.PlatformEnum.mrporter) || !this._task.Link.ToLowerInvariant().Contains("-")) || this._task.Link.ToLowerInvariant().Contains("https")))) && ((this._task.Platform != TaskObject.PlatformEnum.solebox) || !this._task.Link.Contains("aid")))
                                {
                                    if (this._task.State == TaskObject.StateEnum.scheduled)
                                    {
                                        return;
                                    }
                                    if ((((this._task.Status != "Login unsuccessful") && (this._task.Status != "Browser missing")) && ((this._task.Status != "Cloudfront error") && (this._task.Status != "Page not found"))) && ((this._task.Status != "US IP needed") && (this._task.Status != "IP banned")))
                                    {
                                        this._task.Status = States.GetTaskState(States.TaskState.WAITING_FOR_RESTOCK);
                                    }
                                }
                                else
                                {
                                    this.LocalIpReset();
                                    if ((this._task.Platform != TaskObject.PlatformEnum.shopify) || (!this._task.Link.ToLowerInvariant().Contains("/checkouts/") && !this._task.IsShopifyCheckoutLink))
                                    {
                                        this.SetSuccess();
                                    }
                                    bool flag4 = true;
                                    EveAIO.Helpers.ProxyCheck();
                                    if (this._task.Login)
                                    {
                                        this._task.Status = States.GetTaskState(States.TaskState.LOGGING_IN);
                                        flag4 = platform.Login();
                                        if ((this._task.Platform != TaskObject.PlatformEnum.shopify) || (!this._task.Link.ToLowerInvariant().Contains("/checkouts/") && !this._task.IsShopifyCheckoutLink))
                                        {
                                            if (!flag4)
                                            {
                                                this.Success.LoginHidden = 1;
                                            }
                                            else
                                            {
                                                this.Success.LoginHidden = 0;
                                            }
                                        }
                                    }
                                    if (flag4)
                                    {
                                        if (platform.Atc() && (Global.SERIAL.Substring(5) != Global.SERIAL.ToUpperInvariant().Substring(5)))
                                        {
                                            if (((this._task.Platform == TaskObject.PlatformEnum.shopify) && this._task.Link.ToLowerInvariant().Contains("/checkouts/")) || this._task.IsShopifyCheckoutLink)
                                            {
                                                this.SetSuccess();
                                                if (flag4)
                                                {
                                                    this.Success.LoginHidden = 0;
                                                }
                                            }
                                            if (Global.SETTINGS.AtcBeep)
                                            {
                                                EveAIO.Helpers.PlayBell();
                                            }
                                            this.Success.AtcHidden = 0;
                                            if (this._notificator != null)
                                            {
                                                this._notificator.Notify(Notificator.NotificationType.Atc);
                                            }
                                            this._task.PickedSize = this.PickedSize.Value.Key;
                                            if (!Global.SETTINGS.TASKS.Any<TaskObject>(x => ((x.Platform == TaskObject.PlatformEnum.supreme) && (x.ParentId == this._task.Id))))
                                            {
                                                if (Global.SETTINGS.TASKS.Any<TaskObject>(x => (x.Platform == TaskObject.PlatformEnum.shopify) && (x.ParentId == this._task.Id)))
                                                {
                                                    this._task.IsMultiCartParent = true;
                                                }
                                            }
                                            else
                                            {
                                                List<TaskObject> childTasks = (from x in Global.SETTINGS.TASKS
                                                    where (x.Platform == TaskObject.PlatformEnum.supreme) && (x.ParentId == this._task.Id)
                                                    select x).ToList<TaskObject>();
                                                this._task.IsMultiCartParent = true;
                                                ((Supreme) platform).MultiCart(childTasks);
                                            }
                                            this._task.Status = States.GetTaskState(States.TaskState.CHECKING_OUT);
                                            if (platform.Checkout())
                                            {
                                                this.Checkout();
                                            }
                                            else
                                            {
                                                this.Success.CheckoutHidden = 1;
                                            }
                                        }
                                        else if (((this._task.Platform != TaskObject.PlatformEnum.shopify) || !this._task.Link.ToLowerInvariant().Contains("/checkouts/")) && !this._task.IsShopifyCheckoutLink)
                                        {
                                            this.Success.AtcHidden = 1;
                                        }
                                    }
                                    else if ((((this._task.Status != "Browser missing") && (this._task.Status != "Cloudfront error")) && ((this._task.Status != "Page not found") && (this._task.Status != "US IP needed"))) && ((this._task.Status != "IP banned") && (this._task.Status != "US shipping needed")))
                                    {
                                        this._task.Status = States.GetTaskState(States.TaskState.LOGIN_UNSUCCESSFUL);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        this._task.Status = States.GetTaskState(States.TaskState.SHIPPING_NOT_AVAILABLE);
                        States.WriteLogger(this._task, States.LOGGER_STATES.SHIPPING_NOT_AVAILABLE, null, "", "");
                        flag2 = false;
                        this._task.State = TaskObject.StateEnum.stopped;
                        return;
                    }
                }
                catch (ThreadAbortException)
                {
                    return;
                }
                catch (Exception exception)
                {
                    this._task.Status = States.GetTaskState(States.TaskState.ERROR);
                    States.WriteLogger(this._task, States.LOGGER_STATES.TASK_PROCESS_ERROR, exception, "", "");
                    if (!this._task.RetryOnError)
                    {
                        this._task.State = TaskObject.StateEnum.error;
                        return;
                    }
                }
                if (this.IsError && !this._task.RetryOnError)
                {
                    goto Label_192E;
                }
                if (this.IsError)
                {
                    this.Cookies = new CookieContainer();
                }
                if (this._task.State == TaskObject.StateEnum.success)
                {
                    goto Label_1919;
                }
                int num7 = 0;
                string str = "";
                if (this._task.RetryDelay != TaskObject.RetryDelayEnum.exact)
                {
                    num7 = this.Rnd.Next(int.Parse(this._task.DelayFrom), int.Parse(this._task.DelayTo));
                    str = " (random)";
                }
                else
                {
                    num7 = int.Parse(this._task.Delay);
                    str = " (fixed)";
                }
                this._task.WaitingPeriod = num7 + "ms" + str;
                this._task.State = TaskObject.StateEnum.waiting;
                if (this._task.Platform != TaskObject.PlatformEnum.shopify)
                {
                    goto Label_18BA;
                }
                shopifySmartSchedule = this._task.ShopifySmartSchedule;
                if (!shopifySmartSchedule.HasValue)
                {
                    goto Label_18BA;
                }
                shopifySmartSchedule = this._task.ShopifySmartSchedule;
                if (shopifySmartSchedule.Value.AddMinutes(5.0).AddSeconds((double) -Global.SETTINGS.ShopifySmartDelay) <= DateTime.Now)
                {
                    goto Label_18BA;
                }
                this._task.Status = States.GetTaskState(States.TaskState.SHOPIFY_SMART_SCHEDULE);
                States.WriteLogger(this._task, States.LOGGER_STATES.SHOPIFY_SMART_SCHEDULE, null, "", "");
                goto Label_18A4;
            Label_1853:
                shopifySmartSchedule = this._task.ShopifySmartSchedule;
                if (shopifySmartSchedule.Value.AddMinutes(5.0).AddSeconds((double) -Global.SETTINGS.ShopifySmartDelay) <= DateTime.Now)
                {
                    goto Label_18BA;
                }
                Thread.Sleep(500);
            Label_18A4:
                shopifySmartSchedule = this._task.ShopifySmartSchedule;
                if (shopifySmartSchedule.HasValue)
                {
                    goto Label_1853;
                }
            Label_18BA:
                shopifySmartSchedule = null;
                this._task.ShopifySmartSchedule = shopifySmartSchedule;
                for (int i = 0; i < (num7 / 100); i++)
                {
                    if (this._task.DelayChanged)
                    {
                        goto Label_18F9;
                    }
                    Thread.Sleep(100);
                }
                goto Label_1919;
            Label_18F9:
                this._task.DelayChanged = false;
                goto Label_1919;
            Label_1907:
                if (this._task.Starting)
                {
                    goto Label_0578;
                }
                return;
            Label_1919:
                if (this._task.State != TaskObject.StateEnum.success)
                {
                    goto Label_0578;
                }
                goto Label_1907;
            Label_192E:
                this._task.State = TaskObject.StateEnum.error;
                return;
            Label_193C:
                this._task.State = TaskObject.StateEnum.error;
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception exception2)
            {
                this._task.Status = States.GetTaskState(States.TaskState.ERROR);
                States.WriteLogger(this._task, States.LOGGER_STATES.TASK_PROCESS_ERROR, exception2, "", "");
            }
        }
    }
}

