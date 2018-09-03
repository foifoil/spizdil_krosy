namespace EveAIO.Pocos
{
    using System;
    using System.ComponentModel;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows;

    internal class SuccessObject : INotifyPropertyChanged
    {
        private string _id;
        private string _taskName;
        private string _taskId;
        private string _link;
        private DateTime? _time;
        private string _productImage;
        private string _size;
        private ActionEnum? _login;
        private ActionEnum? _atc;
        private ActionEnum? _checkout;
        private string _proxy;
        private int _repetitions;
        private string _parent;
        private bool _isMultiCart;
        private bool _isParent;
        private string _productName;
        private string _store;
        private string _price;

        [field: CompilerGenerated]
        public event PropertyChangedEventHandler PropertyChanged;

        public SuccessObject()
        {
            Class7.RIuqtBYzWxthF();
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

        [DisplayName(" ")]
        public string ProductImage
        {
            get => 
                this._productImage;
            set
            {
                if (this._productImage != value)
                {
                    this._productImage = value;
                    this.OnPropertyChanged("ProductImage");
                }
            }
        }

        public string Link
        {
            get => 
                this._link;
            set
            {
                if (this._link != value)
                {
                    this._link = value;
                    this.OnPropertyChanged("Link");
                }
            }
        }

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
                    this.OnPropertyChanged("ProxyDesc");
                }
            }
        }

        public int Repetitions
        {
            get => 
                this._repetitions;
            set
            {
                if (this._repetitions != value)
                {
                    this._repetitions = value;
                    this.OnPropertyChanged("Repetitions");
                    this.OnPropertyChanged("RepetitionsDesc");
                }
            }
        }

        [DisplayName("Task name")]
        public string TaskName
        {
            get => 
                this._taskName;
            set
            {
                if (this._taskName != value)
                {
                    this._taskName = value;
                    this.OnPropertyChanged("TaskName");
                    this.OnPropertyChanged("TaskNameVis");
                }
            }
        }

        [Browsable(false)]
        public ActionEnum? LoginHidden
        {
            get => 
                this._login;
            set
            {
                ActionEnum? nullable = this._login;
                ActionEnum? nullable2 = value;
                if ((nullable.GetValueOrDefault() == nullable2.GetValueOrDefault()) ? (nullable.HasValue != nullable2.HasValue) : true)
                {
                    this._login = value;
                    this.OnPropertyChanged("LoginHidden");
                    this.OnPropertyChanged("Login");
                }
            }
        }

        [Browsable(false)]
        public ActionEnum? AtcHidden
        {
            get => 
                this._atc;
            set
            {
                ActionEnum? nullable = this._atc;
                ActionEnum? nullable2 = value;
                if ((nullable.GetValueOrDefault() == nullable2.GetValueOrDefault()) ? (nullable.HasValue != nullable2.HasValue) : true)
                {
                    this._atc = value;
                    this.OnPropertyChanged("AtcHidden");
                    this.OnPropertyChanged("Atc");
                }
            }
        }

        [Browsable(false)]
        public ActionEnum? CheckoutHidden
        {
            get => 
                this._checkout;
            set
            {
                ActionEnum? nullable = this._checkout;
                ActionEnum? nullable2 = value;
                if ((nullable.GetValueOrDefault() == nullable2.GetValueOrDefault()) ? (nullable.HasValue != nullable2.HasValue) : true)
                {
                    this._checkout = value;
                    this.OnPropertyChanged("CheckoutHidden");
                    this.OnPropertyChanged("Checkout");
                }
            }
        }

        public string Login
        {
            get
            {
                string str = "../Images/ListsIcons/unknown.png";
                if (this._login.HasValue)
                {
                    str = (((ActionEnum) this._login.Value) == ActionEnum.yes) ? "../Images/ListsIcons/ok.png" : "../Images/ListsIcons/dead.png";
                }
                return str;
            }
        }

        public string Atc
        {
            get
            {
                string str = "../Images/ListsIcons/unknown.png";
                if (this._atc.HasValue)
                {
                    str = (((ActionEnum) this._atc.Value) == ActionEnum.yes) ? "../Images/ListsIcons/ok.png" : "../Images/ListsIcons/dead.png";
                }
                return str;
            }
        }

        public string Checkout
        {
            get
            {
                string str = "../Images/ListsIcons/unknown.png";
                if (this._checkout.HasValue)
                {
                    str = (((ActionEnum) this._checkout.Value) == ActionEnum.yes) ? "../Images/ListsIcons/ok.png" : "../Images/ListsIcons/dead.png";
                }
                return str;
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
                    this.OnPropertyChanged("SizeDesc");
                }
            }
        }

        public string ProductName
        {
            get => 
                this._productName;
            set
            {
                if (this._productName != value)
                {
                    this._productName = value;
                    this.OnPropertyChanged("ProductName");
                    this.OnPropertyChanged("ProductNameDesc");
                }
            }
        }

        public string Price
        {
            get => 
                this._price;
            set
            {
                if (this._price != value)
                {
                    this._price = value;
                    this.OnPropertyChanged("Price");
                    this.OnPropertyChanged("PriceDesc");
                }
            }
        }

        [Browsable(false)]
        public DateTime? Time
        {
            get => 
                this._time;
            set
            {
                DateTime? nullable = this._time;
                DateTime? nullable2 = value;
                if ((nullable.HasValue == nullable2.HasValue) ? (nullable.HasValue ? (nullable.GetValueOrDefault() != nullable2.GetValueOrDefault()) : false) : true)
                {
                    this._time = value;
                    this.OnPropertyChanged("Time");
                    this.OnPropertyChanged("TimeDesc");
                }
            }
        }

        [Browsable(false)]
        public string TaskId
        {
            get => 
                this._taskId;
            set
            {
                if (this._taskId != value)
                {
                    this._taskId = value;
                    this.OnPropertyChanged("TaskId");
                }
            }
        }

        public bool IsMultiCart
        {
            get => 
                this._isMultiCart;
            set
            {
                if (this._isMultiCart != value)
                {
                    this._isMultiCart = value;
                    this.OnPropertyChanged("IsMultiCart");
                    this.OnPropertyChanged("MultiInfo");
                }
            }
        }

        public string Parent
        {
            get => 
                this._parent;
            set
            {
                if (this._parent != value)
                {
                    this._parent = value;
                    this.OnPropertyChanged("Parent");
                    this.OnPropertyChanged("MultiInfo");
                }
            }
        }

        public string Store
        {
            get => 
                this._store;
            set
            {
                if (this._store != value)
                {
                    this._store = value;
                    this.OnPropertyChanged("Store");
                    this.OnPropertyChanged("StoreInfo");
                }
            }
        }

        public System.Windows.Visibility Visibility
        {
            get
            {
                if (!this._isMultiCart)
                {
                    return System.Windows.Visibility.Collapsed;
                }
                return System.Windows.Visibility.Visible;
            }
        }

        public string MultiInfo
        {
            get
            {
                if (!this._isMultiCart)
                {
                    return "";
                }
                string str = "Multiple Cart Item - ";
                if (string.IsNullOrEmpty(this._parent))
                {
                    return (str + "Parent");
                }
                return (str + "Child (" + this._parent + ")");
            }
        }

        [DisplayName("Time")]
        public string TimeDesc
        {
            get
            {
                if (this._time.HasValue)
                {
                    return ("When: " + this._time.Value.ToShortDateString() + " " + this._time.Value.ToString("HH:mm:ss"));
                }
                return "***********";
            }
        }

        public string TaskNameVis =>
            ("Task name: " + this._taskName);

        public string PriceDesc =>
            ("Price: " + WebUtility.HtmlDecode(this._price));

        public string SizeDesc =>
            ("Size: " + this._size);

        public string ProductNameDesc =>
            ("Product: " + WebUtility.HtmlDecode(this._productName));

        public string ProxyDesc =>
            ("Proxy: " + this._proxy);

        public string RepetitionsDesc =>
            ("Repetitions: " + this._repetitions);

        public string StoreDesc =>
            ("Store: " + this._store);

        public enum ActionEnum
        {
            yes,
            no
        }
    }
}

