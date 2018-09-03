namespace EveAIO.Pocos
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class SmsObject : INotifyPropertyChanged
    {
        private SmsMessageEnum _type;
        private string _accountSid;
        private string _authToken;
        private string _numberFrom;
        private string _numberTo;
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
                switch (((((propertyChanged == handler2) ? -1680018680 : -1775586137) ^ -1662671767) % 3))
                {
                    case 0:
                        goto Label_0041;

                    case 1:
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

        public SmsObject()
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
        public SmsMessageEnum SmsType
        {
            get => 
                this._type;
            set
            {
                if (this._type != value)
                {
                    this._type = value;
                    this.OnPropertyChanged("SmsType");
                }
            }
        }

        [DisplayName("Account SID")]
        public string AccountSid
        {
            get => 
                this._accountSid;
            set
            {
                if (this._accountSid != value)
                {
                    this._accountSid = value;
                    this.OnPropertyChanged("AccountSid");
                }
            }
        }

        [DisplayName("Token")]
        public string AuthToken
        {
            get => 
                this._authToken;
            set
            {
                if (this._authToken != value)
                {
                    this._authToken = value;
                    this.OnPropertyChanged("AuthToken");
                }
            }
        }

        [DisplayName("Number from")]
        public string NumberFrom
        {
            get => 
                this._numberFrom;
            set
            {
                if (this._numberFrom != value)
                {
                    this._numberFrom = value;
                    this.OnPropertyChanged("NumberFrom");
                }
            }
        }

        [DisplayName("Number to")]
        public string NumberTo
        {
            get => 
                this._numberTo;
            set
            {
                if (this._numberTo != value)
                {
                    this._numberTo = value;
                    this.OnPropertyChanged("NumberTo");
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

        public enum SmsMessageEnum
        {
            Restock,
            Atc,
            Checkout,
            PayPal
        }
    }
}

