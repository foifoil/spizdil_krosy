namespace EveAIO.Pocos
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class QuickTaskObject : INotifyPropertyChanged
    {
        private string _idProfile;
        private string _idProxyList;
        private TaskObject.RetryDelayEnum _retryDelay;
        private string _delay;
        private string _delayFrom;
        private string _delayTo;
        private bool _autopaste;
        private string _size;
        private bool _randomSize;
        private bool _sizePickRandom;

        [field: CompilerGenerated]
        public event PropertyChangedEventHandler PropertyChanged;

        public QuickTaskObject()
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

        [Browsable(false)]
        public string IdProfile
        {
            get => 
                this._idProfile;
            set
            {
                if (this._idProfile != value)
                {
                    this._idProfile = value;
                    this.OnPropertyChanged("IdProfile");
                }
            }
        }

        public string IdProxyList
        {
            get => 
                this._idProxyList;
            set
            {
                if (this._idProxyList != value)
                {
                    this._idProxyList = value;
                    this.OnPropertyChanged("IdProxyList");
                }
            }
        }

        public TaskObject.RetryDelayEnum RetryDelay
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

        public bool Autopaste
        {
            get => 
                this._autopaste;
            set
            {
                if (this._autopaste != value)
                {
                    this._autopaste = value;
                    this.OnPropertyChanged("Autopaste");
                }
            }
        }

        [Browsable(false)]
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
    }
}

