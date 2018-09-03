namespace EveAIO.Pocos
{
    using EveAIO;
    using EveAIO.Tasks;
    using EveAIO.Views;
    using OpenQA.Selenium.Chrome;
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Xml.Serialization;

    public class TaskObject : INotifyPropertyChanged
    {
        private string _id;
        private string _parentId;
        private string _watchTaskId;
        private string _name;
        private string _checkoutId;
        private AtcMethodEnum _atcMethod;
        private PaymentEnum _payment;
        private TaskTypeEnum _taskType;
        private PlatformEnum _platform;
        private RetryDelayEnum _retryDelay;
        private SuprimeColorPickEnum _supremeColorPick;
        private SuprimeAutomationEnum _supremeAutomation;
        private SupremeRegionEnum _supremeRegion;
        private string _link;
        private string _variant;
        private BindingList<string> _keywords;
        private BindingList<string> _negativeKeywords;
        private bool _retryOnError;
        private string _size;
        private bool _randomSize;
        private bool _sizePickRandom;
        private string _delay;
        private string _delayFrom;
        private string _delayTo;
        private StateEnum _state;
        private string _proxyListId;
        private bool _localIPCheckout;
        private string _waitingPeriod;
        private string _proxy;
        private string _discountCode;
        private bool _discount;
        private bool _notify;
        private string _color;
        private string _group;
        private string _orderNo;
        private string _pickedSize;
        private string _imgUrl;
        private string _homeUrl;
        private string _paypalLink;
        private int _captchaRequests;
        private bool _priceCheck;
        private int _minimumPrice;
        private int _maximumPrice;
        private int _checkoutDelay;
        private bool _login;
        private string _username;
        private string _password;
        private bool _specificCaptcha;
        private int _2captchaRequests;
        private int _disolveRequests;
        private int _anticaptchaRequests;
        private int _imagetypersRequests;
        private bool _smartCheckout;
        private string _dummyProduct;
        private FootSitesEnum _footsite;
        private bool _colorPickRandom;
        private bool _shopifySmartCheckoutStop;
        private bool _paypalProxyIgnore;
        private bool _isNegativeSizing;
        private string _gmailEmail;
        private string _gmailPassword;
        private DateTime _scheduleStart;
        private bool _scheduleStartStarting;
        private bool _isStartScheduled;
        private DateTime _scheduleStop;
        private bool _scheduleStopStarting;
        private bool _isStopScheduled;
        private string _status;
        private string _shopifyWebsite;
        private bool _deepSearch;
        private short _deepSearchLinks;
        private bool _isShopifyCheckoutLink;
        private string _shopifyCheckoutLink;
        private bool _last25products;
        private int _quantity;
        private bool _shopifyIsOldMode;
        [CompilerGenerated]
        private PropertyChangedEventHandler PropertyChanged;

        public event PropertyChangedEventHandler PropertyChanged
        {
            [CompilerGenerated] add
            {
                PropertyChangedEventHandler handler3;
                PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
                do
                {
                    handler3 = propertyChanged;
                    PropertyChangedEventHandler handler2 = (PropertyChangedEventHandler) Delegate.Combine(handler3, value);
                    propertyChanged = Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this.PropertyChanged, handler2, handler3);
                }
                while (propertyChanged != handler3);
            }
            [CompilerGenerated] remove
            {
                // This item is obfuscated and can not be translated.
                PropertyChangedEventHandler handler2;
                PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            Label_0041:
                switch (((((propertyChanged == handler2) ? -1336655391 : -1383689998) ^ -229263505) % 3))
                {
                    case 0:
                        goto Label_0041;

                    case 1:
                    {
                        handler2 = propertyChanged;
                        PropertyChangedEventHandler handler3 = (PropertyChangedEventHandler) Delegate.Remove(handler2, value);
                        propertyChanged = Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this.PropertyChanged, handler3, handler2);
                        goto Label_0041;
                    }
                }
            }
        }

        public TaskObject()
        {
            Class7.RIuqtBYzWxthF();
            this._keywords = new BindingList<string>();
            this._negativeKeywords = new BindingList<string>();
            this.CaptchaRequests = 1;
            this._keywords.ListChanged += new ListChangedEventHandler(this._keywords_ListChanged);
            this._negativeKeywords.ListChanged += new ListChangedEventHandler(this._negativeKeywords_ListChanged);
            if (!string.IsNullOrEmpty(this.ParentId))
            {
                this.State = StateEnum.multicart;
            }
            if ((this._platform == PlatformEnum.supreme) && (this._taskType == TaskTypeEnum.manualPicker))
            {
                this.State = StateEnum.imagesPicker;
            }
            this.Status = "Stopped";
        }

        private void _keywords_ListChanged(object sender, ListChangedEventArgs e)
        {
            this.OnPropertyChanged("KeywordsDesc");
        }

        private void _negativeKeywords_ListChanged(object sender, ListChangedEventArgs e)
        {
            this.OnPropertyChanged("NegativeKeywordsDesc");
        }

        public void NegativeKeywordsChange()
        {
            this.OnPropertyChanged("KeywordsDesc");
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, e);
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        public void PositiveKeywordsChange()
        {
            this.OnPropertyChanged("NegativeKeywordsDesc");
        }

        public string Id
        {
            get => 
                this._id;
            set
            {
                if (this._id != value)
                {
                    this._id = value;
                    this.OnPropertyChanged("Id");
                }
            }
        }

        public string ParentId
        {
            get => 
                this._parentId;
            set
            {
                if (this._parentId != value)
                {
                    this._parentId = value;
                    if (!string.IsNullOrEmpty(this._parentId))
                    {
                        this.State = StateEnum.multicart;
                    }
                    this.OnPropertyChanged("ParentId");
                }
            }
        }

        public string WatchTaskId
        {
            get => 
                this._watchTaskId;
            set
            {
                if (this._watchTaskId != value)
                {
                    this._watchTaskId = value;
                    this.OnPropertyChanged("WatchTaskId");
                }
            }
        }

        [DisplayName(" "), XmlIgnore]
        public string StatusImg
        {
            get
            {
                string str = "";
                this.OnPropertyChanged("IsScheduled");
                switch (this._state)
                {
                    case StateEnum.stopped:
                        this.Status = States.GetTaskState(States.TaskState.STOPPED);
                        return "../Images/Tasks/stop_16x16.png";

                    case StateEnum.success:
                        this.Status = States.GetTaskState(States.TaskState.CHECKOUT);
                        Global.Logger.Info($"['{this.Name} - {this.Guid}']: Checkout successful!");
                        return "../Images/Tasks/success_16x16.png";

                    case StateEnum.error:
                        this.Status = States.GetTaskState(States.TaskState.ERROR);
                        return "../Images/Tasks/error_16x16.png";

                    case StateEnum.running:
                        return "../Images/Tasks/progress_16x16.png";

                    case StateEnum.waiting:
                        return "../Images/Tasks/wait_16x16.png";

                    case StateEnum.scheduled:
                        this.Status = States.GetTaskState(States.TaskState.SCHEDULED);
                        str = "../Images/Tasks/schedule_16x16.png";
                        this.OnPropertyChanged("IsScheduled");
                        return str;

                    case StateEnum.smartWaiting:
                        return "../Images/Tasks/smart_16x16.png";

                    case StateEnum.multicart:
                        this.Status = States.GetTaskState(States.TaskState.MULTICART);
                        return "../Images/Tasks/cart_16x16.png";

                    case StateEnum.imagesPicker:
                        this.Status = States.GetTaskState(States.TaskState.MANUAL_PICKER);
                        return "../Images/Tasks/manual2_16x16.png";

                    case StateEnum.watching:
                        this.Status = States.GetTaskState(States.TaskState.WATCH_TASK);
                        return "../Images/Tasks/watch_22x22.png";

                    case StateEnum.profileUsed:
                        return "../Images/Tasks/stop_16x16.png";
                }
                return str;
            }
        }

        [XmlIgnore, DisplayName(" ")]
        public string PlatformImg
        {
            get
            {
                string str = "";
                switch (this._platform)
                {
                    case PlatformEnum.shopify:
                        return "SHOPIFY";

                    case PlatformEnum.supreme:
                        return "SUPREME";

                    case PlatformEnum.jimmyjazz:
                        return "../Images/Platforms/jimmyjazz.png";

                    case PlatformEnum.sivasdescalzo:
                        return "SIVAS";

                    case PlatformEnum.privacy:
                    case PlatformEnum.adidas:
                        return str;

                    case PlatformEnum.sneakersnstuff:
                        return "SNEAKERSnSTUFF";

                    case PlatformEnum.footsites:
                        return "FOOTSITES";

                    case PlatformEnum.hanon:
                        return "../Images/Platforms/hanon.png";

                    case PlatformEnum.woodwood:
                        return "WOODWOOD";

                    case PlatformEnum.hibbett:
                        return "HIBBETT";

                    case PlatformEnum.solebox:
                        return "SOLEBOX";

                    case PlatformEnum.nordstrom:
                        return "NORDSTROM";

                    case PlatformEnum.mesh:
                        return "MESH";

                    case PlatformEnum.mrporter:
                        return "MRPORTER";

                    case PlatformEnum.holypopstore:
                        return "HOLYPOPSTORE";

                    case PlatformEnum.backdoor:
                        return "BACK-DOOR";

                    case PlatformEnum.barneys:
                        return "BARNEYS";

                    case PlatformEnum.offwhite:
                        return "OFF-WHITE";

                    case PlatformEnum.titolo:
                        return "TITOLO";

                    case PlatformEnum.funko:
                        return "FUNKO";

                    case PlatformEnum.oneblockdown:
                        return "ONEBLOCKDOWN";

                    case PlatformEnum.boxlunch:
                        return "BOXLUNCH";

                    case PlatformEnum.puma:
                        return "PUMA";

                    case PlatformEnum.converse:
                        return "CONVERSE";

                    case PlatformEnum.footlockerau:
                        return "FOOTLOCKER AU";

                    case PlatformEnum.footlockereu:
                        return "FOOTLOCKER EU";

                    case PlatformEnum.footaction:
                        return "FOOTACTION";

                    case PlatformEnum.footlocker:
                        return "FOOTLOCKER";

                    case PlatformEnum.supremeinstore:
                        return "SUPREME INSTORE SIGNUP";

                    case PlatformEnum.mcm:
                        return "MCM";

                    case PlatformEnum.sevres:
                        return "24 SEVRES";

                    case PlatformEnum.finishline:
                        return "FINISHLINE";

                    case PlatformEnum.hottopic:
                        return "HOTTOPIC";
                }
                return str;
            }
        }

        [XmlIgnore, DisplayName("P")]
        public string PaymentImg
        {
            get
            {
                string str = "";
                PaymentEnum enum2 = this._payment;
                if (enum2 != PaymentEnum.creditcard)
                {
                    if (enum2 == PaymentEnum.paypal)
                    {
                        str = "../Images/Platforms/paypal_16x16.png";
                    }
                    return str;
                }
                return "../Images/Platforms/cc_16x16.png";
            }
        }

        [XmlIgnore, DisplayName("A")]
        public string AtcMethodImg
        {
            get
            {
                switch (this._atcMethod)
                {
                    case AtcMethodEnum.frontend:
                        return "../Images/Platforms/frontend_16x16.png";

                    case AtcMethodEnum.backend:
                        return "../Images/Platforms/backend_16x16.png";
                }
                return "";
            }
        }

        [DisplayName("T")]
        public string TaskTypeImg
        {
            get
            {
                switch (this._taskType)
                {
                    case TaskTypeEnum.directlink:
                        return "../Images/Platforms/direct_link_16x16.png";

                    case TaskTypeEnum.keywords:
                        return "../Images/Platforms/keywords_16x16.png";

                    case TaskTypeEnum.variant:
                        return "../Images/Platforms/variant_16x16.png";
                }
                return "";
            }
        }

        [DisplayName("Task name")]
        public string Name
        {
            get => 
                this._name;
            set
            {
                if (this._name != value)
                {
                    this._name = value;
                    this.OnPropertyChanged("Name");
                }
            }
        }

        [DisplayName("Link / website")]
        public string Link
        {
            get
            {
                if ((this._platform != PlatformEnum.mesh) || (this._atcMethod != AtcMethodEnum.backend))
                {
                    return this._link;
                }
                if (this._link.Contains("/"))
                {
                    return this._link;
                }
                string str = "";
                switch (this.VariousStringData)
                {
                    case "size":
                        str = "Size.co.uk";
                        break;

                    case "hipstore":
                        str = "Thehipstore.co.uk";
                        break;

                    case "jd":
                        str = "Jdsports.co.uk";
                        break;

                    case "footpatrol":
                        str = "Footpatrol.com";
                        break;
                }
                return (str + " / " + this._link);
            }
            set
            {
                if (this._link != value)
                {
                    this._link = value;
                    this.OnPropertyChanged("Link");
                }
            }
        }

        [DisplayName("Keywords +"), XmlIgnore]
        public string KeywordsDesc
        {
            get
            {
                if ((this._keywords == null) || (this._keywords.Count == 0))
                {
                    return "";
                }
                string str = "";
                foreach (string str2 in this._keywords)
                {
                    str = str + str2.Replace("\n", "").Replace("\r", "") + ", ";
                }
                return str.Substring(0, str.Length - 2);
            }
        }

        [Browsable(false)]
        public BindingList<string> Keywords
        {
            get => 
                this._keywords;
            set
            {
                if (this._keywords != value)
                {
                    this._keywords = value;
                    this.OnPropertyChanged("Keywords");
                    this.OnPropertyChanged("KeywordsDesc");
                }
            }
        }

        [DisplayName("Keywords -"), XmlIgnore]
        public string NegativeKeywordsDesc
        {
            get
            {
                if ((this._negativeKeywords == null) || (this._negativeKeywords.Count == 0))
                {
                    return "";
                }
                string str = "";
                foreach (string str2 in this._negativeKeywords)
                {
                    str = str + str2.Replace("\n", "") + ", ";
                }
                return str.Substring(0, str.Length - 2);
            }
        }

        public string Variant
        {
            get => 
                this._variant;
            set
            {
                if (this._variant != value)
                {
                    this._variant = value;
                    this.OnPropertyChanged("Variant");
                }
            }
        }

        [Browsable(false)]
        public BindingList<string> NegativeKeywords
        {
            get => 
                this._negativeKeywords;
            set
            {
                if (this._negativeKeywords != value)
                {
                    this._negativeKeywords = value;
                    this.OnPropertyChanged("NegativeKeywords");
                    this.OnPropertyChanged("NegativeKeywordsDesc");
                }
            }
        }

        [DisplayName("Checkout profile"), XmlIgnore]
        public string CheckoutProfile
        {
            get
            {
                if (Global.SETTINGS.PROFILES.Any<ProfileObject>(x => x.Id == this._checkoutId))
                {
                    return Global.SETTINGS.PROFILES.First<ProfileObject>(x => (x.Id == this._checkoutId)).Name;
                }
                return "";
            }
        }

        [Browsable(false)]
        public string CheckoutId
        {
            get => 
                this._checkoutId;
            set
            {
                if (this._checkoutId != value)
                {
                    this._checkoutId = value;
                    this.OnPropertyChanged("CheckoutId");
                    this.OnPropertyChanged("CheckoutProfile");
                }
            }
        }

        [Browsable(false)]
        public AtcMethodEnum AtcMethod
        {
            get => 
                this._atcMethod;
            set
            {
                if (this._atcMethod != value)
                {
                    this._atcMethod = value;
                    this.OnPropertyChanged("AtcMethod");
                    this.OnPropertyChanged("AtcMethodImg");
                }
            }
        }

        [Browsable(false)]
        public PaymentEnum Payment
        {
            get => 
                this._payment;
            set
            {
                if (this._payment != value)
                {
                    this._payment = value;
                    this.OnPropertyChanged("Payment");
                    this.OnPropertyChanged("PaymentImg");
                }
            }
        }

        [Browsable(false)]
        public TaskTypeEnum TaskType
        {
            get => 
                this._taskType;
            set
            {
                if (this._taskType != value)
                {
                    this._taskType = value;
                    if (this._taskType == TaskTypeEnum.manualPicker)
                    {
                        this.State = StateEnum.imagesPicker;
                    }
                    this.OnPropertyChanged("TaskType");
                    this.OnPropertyChanged("TaskTypeImg");
                }
            }
        }

        [Browsable(false)]
        public RetryDelayEnum RetryDelay
        {
            get => 
                this._retryDelay;
            set
            {
                if (this._retryDelay != value)
                {
                    this._retryDelay = value;
                    this.OnPropertyChanged("RetryDelay");
                }
            }
        }

        [Browsable(false)]
        public SuprimeColorPickEnum SupremeColorPick
        {
            get => 
                this._supremeColorPick;
            set
            {
                if (this._supremeColorPick != value)
                {
                    this._supremeColorPick = value;
                    this.OnPropertyChanged("SupremeColorPick");
                }
            }
        }

        [Browsable(false)]
        public SuprimeAutomationEnum SupremeAutomation
        {
            get => 
                this._supremeAutomation;
            set
            {
                if (this._supremeAutomation != value)
                {
                    this._supremeAutomation = value;
                    this.OnPropertyChanged("SupremeAutomation");
                }
            }
        }

        [Browsable(false)]
        public SupremeRegionEnum SupremeRegion
        {
            get => 
                this._supremeRegion;
            set
            {
                if (this._supremeRegion != value)
                {
                    this._supremeRegion = value;
                    this.OnPropertyChanged("SupremeRegion");
                }
            }
        }

        [Browsable(false)]
        public bool RetryOnError
        {
            get => 
                this._retryOnError;
            set
            {
                if (this._retryOnError != value)
                {
                    this._retryOnError = value;
                    this.OnPropertyChanged("RetryOnError");
                }
            }
        }

        public string Size
        {
            get => 
                this._size;
            set
            {
                if (this._size != value)
                {
                    this._size = value;
                    this.OnPropertyChanged("Size");
                }
            }
        }

        [Browsable(false)]
        public string ProxyListId
        {
            get => 
                this._proxyListId;
            set
            {
                if (this._proxyListId != value)
                {
                    this._proxyListId = value;
                    this.OnPropertyChanged("ProxyListId");
                    this.OnPropertyChanged("Proxylist");
                }
            }
        }

        [XmlIgnore, DisplayName("Proxy list")]
        public string ProxyList
        {
            get
            {
                if (Global.SETTINGS.PROXIES.Any<ProxyListObject>(x => x.Id == this._proxyListId))
                {
                    return Global.SETTINGS.PROXIES.First<ProxyListObject>(x => (x.Id == this._proxyListId)).Name;
                }
                return "";
            }
        }

        [Browsable(false)]
        public bool LocalIPCheckout
        {
            get => 
                this._localIPCheckout;
            set
            {
                if (this._localIPCheckout != value)
                {
                    this._localIPCheckout = value;
                    this.OnPropertyChanged("LocalIPCheckout");
                }
            }
        }

        public bool Notify
        {
            get => 
                this._notify;
            set
            {
                if (this._notify != value)
                {
                    this._notify = value;
                    this.OnPropertyChanged("Notify");
                    this.OnPropertyChanged("NotifyDesc");
                }
            }
        }

        [XmlIgnore]
        public string NotifyDesc
        {
            get
            {
                if (this._notify)
                {
                    return "✔";
                }
                return "✘";
            }
        }

        public bool Discount
        {
            get => 
                this._discount;
            set
            {
                if (this._discount != value)
                {
                    this._discount = value;
                    this.OnPropertyChanged("Discount");
                }
            }
        }

        [XmlIgnore, DisplayName("Picked proxy")]
        public string Proxy
        {
            get => 
                this._proxy;
            set
            {
                if (this._proxy != value)
                {
                    this._proxy = value;
                    this.OnPropertyChanged("Proxy");
                }
            }
        }

        [DisplayName("Task delay"), XmlIgnore]
        public string WaitingPeriod
        {
            get => 
                this._waitingPeriod;
            set
            {
                if (this._waitingPeriod != value)
                {
                    this._waitingPeriod = value;
                    this.OnPropertyChanged("WaitingPeriod");
                }
            }
        }

        [Browsable(false)]
        public bool RandomSize
        {
            get => 
                this._randomSize;
            set
            {
                if (this._randomSize != value)
                {
                    this._randomSize = value;
                    this.OnPropertyChanged("RandomSize");
                }
            }
        }

        [Browsable(false)]
        public bool SizePickRandom
        {
            get => 
                this._sizePickRandom;
            set
            {
                if (this._sizePickRandom != value)
                {
                    this._sizePickRandom = value;
                    this.OnPropertyChanged("SizePickRandom");
                }
            }
        }

        [Browsable(false)]
        public bool ColorPickRandom
        {
            get => 
                this._colorPickRandom;
            set
            {
                if (this._colorPickRandom != value)
                {
                    this._colorPickRandom = value;
                    this.OnPropertyChanged("ColorPickRandom");
                }
            }
        }

        [Browsable(false)]
        public string Delay
        {
            get => 
                this._delay;
            set
            {
                if (this._delay != value)
                {
                    this._delay = value;
                    this.OnPropertyChanged("Delay");
                }
            }
        }

        [Browsable(false)]
        public string DelayFrom
        {
            get => 
                this._delayFrom;
            set
            {
                if (this._delayFrom != value)
                {
                    this._delayFrom = value;
                    this.OnPropertyChanged("DelayFrom");
                }
            }
        }

        [Browsable(false)]
        public string DelayTo
        {
            get => 
                this._delayTo;
            set
            {
                if (this._delayTo != value)
                {
                    this._delayTo = value;
                    this.OnPropertyChanged("DelayTo");
                }
            }
        }

        [Browsable(false), XmlIgnore]
        public StateEnum State
        {
            get => 
                this._state;
            set
            {
                if (this._state != value)
                {
                    this._state = value;
                    this.OnPropertyChanged("State");
                    this.OnPropertyChanged("StatusImg");
                }
            }
        }

        [Browsable(false)]
        public PlatformEnum Platform
        {
            get => 
                this._platform;
            set
            {
                if (this._platform != value)
                {
                    this._platform = value;
                    this.OnPropertyChanged("Platform");
                    this.OnPropertyChanged("PlatformImg");
                }
            }
        }

        [Browsable(false), XmlIgnore]
        public string PlatformDesc
        {
            get
            {
                switch (this._platform)
                {
                    case PlatformEnum.shopify:
                        return "Shopify";

                    case PlatformEnum.supreme:
                        return "Supreme";

                    case PlatformEnum.jimmyjazz:
                        return "JimmyJazz";
                }
                return "Shopify";
            }
        }

        [Browsable(false)]
        public string DiscountCode
        {
            get => 
                this._discountCode;
            set
            {
                if (this._discountCode != value)
                {
                    this._discountCode = value;
                    this.OnPropertyChanged("DiscountCode");
                }
            }
        }

        [Browsable(false), XmlIgnore]
        public string PaypalLink
        {
            get => 
                this._paypalLink;
            set
            {
                if (this._paypalLink != value)
                {
                    this._paypalLink = value;
                    this.OnPropertyChanged("PaypalLink");
                }
            }
        }

        [Browsable(false), XmlIgnore]
        public string OrderNo
        {
            get => 
                this._orderNo;
            set
            {
                if (this._orderNo != value)
                {
                    this._orderNo = value;
                    this.OnPropertyChanged("OrderNo");
                }
            }
        }

        [Browsable(false), XmlIgnore]
        public string PickedSize
        {
            get => 
                this._pickedSize;
            set
            {
                if (this._pickedSize != value)
                {
                    this._pickedSize = value;
                    this.OnPropertyChanged("PickedSize");
                }
            }
        }

        [XmlIgnore, Browsable(false)]
        public string ImgUrl
        {
            get => 
                this._imgUrl;
            set
            {
                if (this._imgUrl != value)
                {
                    this._imgUrl = value;
                    this.OnPropertyChanged("ImgUrl");
                }
            }
        }

        [Browsable(false), XmlIgnore]
        public string HomeUrl
        {
            get => 
                this._homeUrl;
            set
            {
                if (this._homeUrl != value)
                {
                    this._homeUrl = value;
                    this.OnPropertyChanged("HomeUrl");
                }
            }
        }

        [Browsable(false)]
        public int CaptchaRequests
        {
            get => 
                this._captchaRequests;
            set
            {
                if (this._captchaRequests != value)
                {
                    this._captchaRequests = value;
                    this.OnPropertyChanged("CaptchaRequests");
                }
            }
        }

        [Browsable(false)]
        public bool PriceCheck
        {
            get => 
                this._priceCheck;
            set
            {
                if (this._priceCheck != value)
                {
                    this._priceCheck = value;
                    this.OnPropertyChanged("PriceCheck");
                }
            }
        }

        [Browsable(false)]
        public int MinimumPrice
        {
            get => 
                this._minimumPrice;
            set
            {
                if (this._minimumPrice != value)
                {
                    this._minimumPrice = value;
                    this.OnPropertyChanged("MinimumPrice");
                }
            }
        }

        [Browsable(false)]
        public int MaximumPrice
        {
            get => 
                this._maximumPrice;
            set
            {
                if (this._maximumPrice != value)
                {
                    this._maximumPrice = value;
                    this.OnPropertyChanged("MaximumPrice");
                }
            }
        }

        [Browsable(false)]
        public int CheckoutDelay
        {
            get => 
                this._checkoutDelay;
            set
            {
                if (this._checkoutDelay != value)
                {
                    this._checkoutDelay = value;
                    this.OnPropertyChanged("CheckoutDelay");
                }
            }
        }

        [Browsable(false)]
        public DateTime ScheduleStart
        {
            get => 
                this._scheduleStart;
            set
            {
                if (this._scheduleStart != value)
                {
                    this._scheduleStart = value;
                    this.OnPropertyChanged("ScheduleStart");
                    this.OnPropertyChanged("ScheduleDesc");
                }
            }
        }

        [Browsable(false)]
        public bool ScheduleStartStarting
        {
            get => 
                this._scheduleStartStarting;
            set
            {
                if (this._scheduleStartStarting != value)
                {
                    this._scheduleStartStarting = value;
                    this.OnPropertyChanged("ScheduleStartStarting");
                }
            }
        }

        [Browsable(false)]
        public DateTime ScheduleStop
        {
            get => 
                this._scheduleStop;
            set
            {
                if (this._scheduleStop != value)
                {
                    this._scheduleStop = value;
                    this.OnPropertyChanged("ScheduleStop");
                }
            }
        }

        [Browsable(false)]
        public bool ScheduleStopStarting
        {
            get => 
                this._scheduleStopStarting;
            set
            {
                if (this._scheduleStopStarting != value)
                {
                    this._scheduleStopStarting = value;
                    this.OnPropertyChanged("ScheduleStopStarting");
                }
            }
        }

        [Browsable(false)]
        public bool SmartCheckout
        {
            get => 
                this._smartCheckout;
            set
            {
                if (this._smartCheckout != value)
                {
                    this._smartCheckout = value;
                    this.OnPropertyChanged("SmartCheckout");
                }
            }
        }

        [Browsable(false)]
        public string DummyProduct
        {
            get => 
                this._dummyProduct;
            set
            {
                if (this._dummyProduct != value)
                {
                    this._dummyProduct = value;
                    this.OnPropertyChanged("DummyProduct");
                }
            }
        }

        [Browsable(false)]
        public bool Login
        {
            get => 
                this._login;
            set
            {
                if (this._login != value)
                {
                    this._login = value;
                    this.OnPropertyChanged("Login");
                }
            }
        }

        [Browsable(false)]
        public string Username
        {
            get => 
                this._username;
            set
            {
                if (this._username != value)
                {
                    this._username = value;
                    this.OnPropertyChanged("Username");
                }
            }
        }

        [Browsable(false)]
        public string Password
        {
            get => 
                this._password;
            set
            {
                if (this._password != value)
                {
                    this._password = value;
                    this.OnPropertyChanged("Password");
                }
            }
        }

        [Browsable(false)]
        public bool SpecificCaptcha
        {
            get => 
                this._specificCaptcha;
            set
            {
                if (this._specificCaptcha != value)
                {
                    this._specificCaptcha = value;
                    this.OnPropertyChanged("SpecificCaptcha");
                }
            }
        }

        [Browsable(false)]
        public string Color
        {
            get => 
                this._color;
            set
            {
                if (this._color != value)
                {
                    this._color = value;
                    this.OnPropertyChanged("Color");
                }
            }
        }

        [Browsable(false)]
        public string Group
        {
            get => 
                this._group;
            set
            {
                if (this._group != value)
                {
                    this._group = value;
                    this.OnPropertyChanged("Group");
                }
            }
        }

        [Browsable(false)]
        public int TwoCaptchaRequests
        {
            get => 
                this._2captchaRequests;
            set
            {
                if (this._2captchaRequests != value)
                {
                    this._2captchaRequests = value;
                    this.OnPropertyChanged("TwoCaptchaRequests");
                }
            }
        }

        [Browsable(false)]
        public int DisolveRequests
        {
            get => 
                this._disolveRequests;
            set
            {
                if (this._disolveRequests != value)
                {
                    this._disolveRequests = value;
                    this.OnPropertyChanged("DisolveRequests");
                }
            }
        }

        [Browsable(false)]
        public int AnticaptchaRequests
        {
            get => 
                this._anticaptchaRequests;
            set
            {
                if (this._anticaptchaRequests != value)
                {
                    this._anticaptchaRequests = value;
                    this.OnPropertyChanged("AnticaptchaRequests");
                }
            }
        }

        [Browsable(false)]
        public int ImagetypersRequests
        {
            get => 
                this._imagetypersRequests;
            set
            {
                if (this._imagetypersRequests != value)
                {
                    this._imagetypersRequests = value;
                    this.OnPropertyChanged("ImagetypersRequests");
                }
            }
        }

        [Browsable(false), XmlIgnore]
        public ManualResetEvent Mre { get; set; }

        [Browsable(false), XmlIgnore]
        public ManualResetEvent WatchTaskHold { get; set; }

        [Browsable(false), XmlIgnore]
        public bool Starting { get; set; }

        [Browsable(false), XmlIgnore]
        public Thread RunnerThread { get; set; }

        [Browsable(false), XmlIgnore]
        public string SuccessUrl { get; set; }

        [XmlIgnore, Browsable(false)]
        public string SuccessProductName { get; set; }

        [XmlIgnore, Browsable(false)]
        public string Various { get; set; }

        [Browsable(false), XmlIgnore]
        public string SuccessCountry { get; set; }

        [Browsable(false), XmlIgnore]
        public string SuccessSize { get; set; }

        [Browsable(false)]
        public bool IsStartScheduled
        {
            get => 
                this._isStartScheduled;
            set
            {
                if (this._isStartScheduled != value)
                {
                    this._isStartScheduled = value;
                    this.OnPropertyChanged("IsStartScheduled");
                }
            }
        }

        [Browsable(false)]
        public bool IsStopScheduled
        {
            get => 
                this._isStopScheduled;
            set
            {
                if (this._isStopScheduled != value)
                {
                    this._isStopScheduled = value;
                    this.OnPropertyChanged("IsStopScheduled");
                }
            }
        }

        [XmlIgnore, Browsable(false)]
        public string ScheduleDesc =>
            this._scheduleStart.ToString("MM-dd-yyyy HH:mm:ss");

        [Browsable(false), XmlIgnore]
        public bool IsMultiCartParent { get; set; }

        [XmlIgnore, Browsable(false)]
        public ChromeDriver Driver { get; set; }

        [Browsable(false), XmlIgnore]
        public bool ManualSolved { get; set; }

        [XmlIgnore, Browsable(false)]
        public string SessionsId { get; set; }

        [Browsable(false)]
        public string GmailEmail
        {
            get => 
                this._gmailEmail;
            set
            {
                if (this._gmailEmail != value)
                {
                    this._gmailEmail = value;
                    this.OnPropertyChanged("GmailEmail");
                }
            }
        }

        [Browsable(false)]
        public string GmailPassword
        {
            get => 
                this._gmailPassword;
            set
            {
                if (this._gmailPassword != value)
                {
                    this._gmailPassword = value;
                    this.OnPropertyChanged("GmailPassword");
                }
            }
        }

        [Browsable(false)]
        public FootSitesEnum FootSite
        {
            get => 
                this._footsite;
            set
            {
                if (this._footsite != value)
                {
                    this._footsite = value;
                    this.OnPropertyChanged("FootSite");
                }
            }
        }

        [Browsable(false)]
        public string ShopifyWebsite
        {
            get => 
                this._shopifyWebsite;
            set
            {
                if (this._shopifyWebsite != value)
                {
                    this._shopifyWebsite = value;
                    this.OnPropertyChanged("ShopifyWebsite");
                }
            }
        }

        [Browsable(false)]
        public bool DeepSearch
        {
            get => 
                this._deepSearch;
            set
            {
                if (this._deepSearch != value)
                {
                    this._deepSearch = value;
                    this.OnPropertyChanged("DeepSearch");
                }
            }
        }

        [Browsable(false)]
        public short DeepSearchLinks
        {
            get => 
                this._deepSearchLinks;
            set
            {
                if (this._deepSearchLinks != value)
                {
                    this._deepSearchLinks = value;
                    this.OnPropertyChanged("DeepSearchLinks");
                }
            }
        }

        [XmlIgnore]
        public string Status
        {
            get => 
                this._status;
            set
            {
                if (this._status != value)
                {
                    this._status = value;
                    this.OnPropertyChanged("Status");
                }
            }
        }

        public bool IsShopifyCheckoutLink
        {
            get => 
                this._isShopifyCheckoutLink;
            set
            {
                if (this._isShopifyCheckoutLink != value)
                {
                    this._isShopifyCheckoutLink = value;
                    this.OnPropertyChanged("IsShopifyCheckoutLink");
                }
            }
        }

        public string ShopifyCheckoutLink
        {
            get => 
                this._shopifyCheckoutLink;
            set
            {
                if (this._shopifyCheckoutLink != value)
                {
                    this._shopifyCheckoutLink = value;
                    this.OnPropertyChanged("ShopifyCheckoutLink");
                }
            }
        }

        public bool ShopifySmartCheckoutStop
        {
            get => 
                this._shopifySmartCheckoutStop;
            set
            {
                if (this._shopifySmartCheckoutStop != value)
                {
                    this._shopifySmartCheckoutStop = value;
                    this.OnPropertyChanged("ShopifySmartCheckoutStop");
                }
            }
        }

        [Browsable(false)]
        public bool PaypalProxyIgnore
        {
            get => 
                this._paypalProxyIgnore;
            set
            {
                if (this._paypalProxyIgnore != value)
                {
                    this._paypalProxyIgnore = value;
                    this.OnPropertyChanged("PaypalProxyIgnore");
                }
            }
        }

        [Browsable(false)]
        public bool Last25Products
        {
            get => 
                this._last25products;
            set
            {
                if (this._last25products != value)
                {
                    this._last25products = value;
                    this.OnPropertyChanged("Last25Products");
                }
            }
        }

        [Browsable(false)]
        public int Quantity
        {
            get => 
                this._quantity;
            set
            {
                if (this._quantity != value)
                {
                    this._quantity = value;
                    this.OnPropertyChanged("Quantity");
                }
            }
        }

        [Browsable(false)]
        public bool IsNegativeSizing
        {
            get => 
                this._isNegativeSizing;
            set
            {
                if (this._isNegativeSizing != value)
                {
                    this._isNegativeSizing = value;
                    this.OnPropertyChanged("IsNegativeSizing");
                }
            }
        }

        [Browsable(false)]
        public bool ShopifyIsOldMode
        {
            get => 
                this._shopifyIsOldMode;
            set
            {
                if (this._shopifyIsOldMode != value)
                {
                    this._shopifyIsOldMode = value;
                    this.OnPropertyChanged("ShopifyIsOldMode");
                }
            }
        }

        [Browsable(false)]
        public string Guid { get; set; }

        [Browsable(false)]
        public string VariousStringData { get; set; }

        [Browsable(false)]
        public string VariousStringData2 { get; set; }

        [Browsable(false)]
        public string SkuId { get; set; }

        [Browsable(false)]
        public string StyleId { get; set; }

        [Browsable(false)]
        public bool SolveCaptcha { get; set; }

        [Browsable(false), XmlIgnore]
        public bool CaptchaSolverAssigned { get; set; }

        [Browsable(false), XmlIgnore]
        public bool DelayChanged { get; set; }

        [Browsable(false), XmlIgnore]
        public DateTime? PaypalRefresh { get; set; }

        [Browsable(false), XmlIgnore]
        public DateTime? ShopifySmartSchedule { get; set; }

        [Browsable(false), XmlIgnore]
        public CaptchaSolverWindow CaptchaWindow { get; set; }

        public enum AtcMethodEnum
        {
            frontend,
            backend
        }

        public enum FootSitesEnum
        {
            footlocker,
            footlockerCa,
            footaction,
            eastbay,
            champssports
        }

        public enum PaymentEnum
        {
            creditcard,
            paypal,
            bankTransfer
        }

        public enum PlatformEnum
        {
            shopify,
            supreme,
            jimmyjazz,
            sivasdescalzo,
            privacy,
            adidas,
            sneakersnstuff,
            footsites,
            hanon,
            woodwood,
            hibbett,
            solebox,
            nordstrom,
            mesh,
            mrporter,
            holypopstore,
            backdoor,
            barneys,
            offwhite,
            titolo,
            funko,
            oneblockdown,
            boxlunch,
            puma,
            converse,
            footlockerau,
            footlockereu,
            footaction,
            footlocker,
            supremeinstore,
            mcm,
            sevres,
            finishline,
            hottopic
        }

        public enum RetryDelayEnum
        {
            exact,
            random
        }

        public enum StateEnum
        {
            stopped,
            success,
            error,
            running,
            waiting,
            scheduled,
            smartWaiting,
            multicart,
            imagesPicker,
            watching,
            profileUsed
        }

        public enum SupremeRegionEnum
        {
            USA,
            EU,
            JP
        }

        public enum SuprimeAutomationEnum
        {
            browserless,
            browser
        }

        public enum SuprimeColorPickEnum
        {
            exact,
            contains
        }

        public enum TaskTypeEnum
        {
            directlink,
            keywords,
            variant,
            manualPicker
        }
    }
}

