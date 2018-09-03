namespace EveAIO.Pocos
{
    using EveAIO;
    using Newtonsoft.Json;
    using System;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class ProxyListObject : INotifyPropertyChanged
    {
        private string _id;
        private string _name;
        private ObservableCollection<ProxyObject> _proxies;
        private string _proxyTextUrl;
        private int _proxyTimeout;
        private TesterEnum _tester;
        private RotationEnum _rotation;

        [field: CompilerGenerated]
        public event PropertyChangedEventHandler PropertyChanged;

        public ProxyListObject()
        {
            Class7.RIuqtBYzWxthF();
            this.Proxies = new ObservableCollection<ProxyObject>();
            this.Proxies.CollectionChanged += new NotifyCollectionChangedEventHandler(this.Proxies_CollectionChanged);
            this.OnPropertyChanged("ProxiesCount");
            this.ProxyTimeout = 10;
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

        private void Proxies_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnPropertyChanged("Proxies");
            int num = 0;
            if (Global.SETTINGS != null)
            {
                foreach (ProxyListObject obj2 in Global.SETTINGS.PROXIES)
                {
                    num += obj2.Proxies.Count;
                }
                Global.MAIN_WINDOW.txtTotalProxies.Text = "Total proxies: " + num;
            }
        }

        public void UpdateProxies()
        {
            this.OnPropertyChanged("ProxiesCount");
        }

        [Newtonsoft.Json.JsonIgnore]
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

        [DisplayName("Proxy #"), Newtonsoft.Json.JsonIgnore]
        public int ProxiesCount =>
            this._proxies.Count;

        [Browsable(false)]
        public string ProxyTestUrl
        {
            get => 
                this._proxyTextUrl;
            set
            {
                if (this._proxyTextUrl != value)
                {
                    this._proxyTextUrl = value;
                    this.OnPropertyChanged("ProxyTestUrl");
                }
            }
        }

        [Browsable(false)]
        public int ProxyTimeout
        {
            get => 
                this._proxyTimeout;
            set
            {
                if (this._proxyTimeout != value)
                {
                    this._proxyTimeout = value;
                    this.OnPropertyChanged("ProxyTimeout");
                }
            }
        }

        [Browsable(false)]
        public RotationEnum Rotation
        {
            get => 
                this._rotation;
            set
            {
                if (this._rotation != value)
                {
                    this._rotation = value;
                    this.OnPropertyChanged("Rotation");
                }
            }
        }

        [Browsable(false)]
        public TesterEnum Tester
        {
            get => 
                this._tester;
            set
            {
                if (this._tester != value)
                {
                    this._tester = value;
                    this.OnPropertyChanged("Tester");
                }
            }
        }

        [Browsable(false)]
        public ObservableCollection<ProxyObject> Proxies
        {
            get => 
                this._proxies;
            set
            {
                if (this._proxies != value)
                {
                    this._proxies = value;
                    this.OnPropertyChanged("Proxies");
                    this.OnPropertyChanged("ProxiesCount");
                }
            }
        }

        public enum RotationEnum
        {
            random,
            linear
        }

        public enum TesterEnum
        {
            website,
            ping
        }
    }
}

