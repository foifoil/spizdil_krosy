namespace EveAIO.Pocos
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class TwitterObject : INotifyPropertyChanged
    {
        private TwitterMessageEnum _type;
        private string _consumerKey;
        private string _consumerKeySecret;
        private string _accessToken;
        private string _accessTokenSecret;
        private string _message;
        [CompilerGenerated]
        private PropertyChangedEventHandler PropertyChanged;

        public event PropertyChangedEventHandler PropertyChanged
        {
            [CompilerGenerated] add
            {
                // This item is obfuscated and can not be translated.
                PropertyChangedEventHandler handler2;
                PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            Label_0041:
                switch (((((propertyChanged != handler2) ? -1740964141 : -1077676658) ^ -2025366480) % 3))
                {
                    case 0:
                        goto Label_0041;

                    case 2:
                    {
                        handler2 = propertyChanged;
                        PropertyChangedEventHandler handler3 = (PropertyChangedEventHandler) Delegate.Combine(handler2, value);
                        propertyChanged = Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this.PropertyChanged, handler3, handler2);
                        goto Label_0041;
                    }
                }
            }
            [CompilerGenerated] remove
            {
                PropertyChangedEventHandler handler2;
                PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
                do
                {
                    handler2 = propertyChanged;
                    PropertyChangedEventHandler handler3 = (PropertyChangedEventHandler) Delegate.Remove(handler2, value);
                    propertyChanged = Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this.PropertyChanged, handler3, handler2);
                }
                while (propertyChanged != handler2);
            }
        }

        public TwitterObject()
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

        [DisplayName("Type")]
        public TwitterMessageEnum TwitterType
        {
            get => 
                this._type;
            set
            {
                if (this._type != value)
                {
                    this._type = value;
                    this.OnPropertyChanged("TwitterType");
                }
            }
        }

        public string ConsumerKey
        {
            get => 
                this._consumerKey;
            set
            {
                if (this._consumerKey != value)
                {
                    this._consumerKey = value;
                    this.OnPropertyChanged("ConsumerKey");
                }
            }
        }

        public string ConsumerKeySecret
        {
            get => 
                this._consumerKeySecret;
            set
            {
                if (this._consumerKeySecret != value)
                {
                    this._consumerKeySecret = value;
                    this.OnPropertyChanged("ConsumerKeySecret");
                }
            }
        }

        public string AccessToken
        {
            get => 
                this._accessToken;
            set
            {
                if (this._accessToken != value)
                {
                    this._accessToken = value;
                    this.OnPropertyChanged("AccessToken");
                }
            }
        }

        public string AccessTokenSecret
        {
            get => 
                this._accessTokenSecret;
            set
            {
                if (this._accessTokenSecret != value)
                {
                    this._accessTokenSecret = value;
                    this.OnPropertyChanged("AccessTokenSecret");
                }
            }
        }

        public string Message
        {
            get => 
                this._message;
            set
            {
                if (this._message != value)
                {
                    this._message = value;
                    this.OnPropertyChanged("Message");
                }
            }
        }

        public enum TwitterMessageEnum
        {
            Restock,
            Atc,
            Checkout,
            PayPal
        }
    }
}

