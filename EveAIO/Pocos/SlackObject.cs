namespace EveAIO.Pocos
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class SlackObject : INotifyPropertyChanged
    {
        private SlackMessageEnum _type;
        private string _hook;
        private string _username;
        private string _channel;
        private string _message;

        [field: CompilerGenerated]
        public event PropertyChangedEventHandler PropertyChanged;

        public SlackObject()
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
        public SlackMessageEnum SlackType
        {
            get => 
                this._type;
            set
            {
                if (this._type != value)
                {
                    this._type = value;
                    this.OnPropertyChanged("SlackType");
                }
            }
        }

        public string Hook
        {
            get => 
                this._hook;
            set
            {
                if (this._hook != value)
                {
                    this._hook = value;
                    this.OnPropertyChanged("Hook");
                }
            }
        }

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

        public string Channel
        {
            get => 
                this._channel;
            set
            {
                if (this._channel != value)
                {
                    this._channel = value;
                    this.OnPropertyChanged("Channel");
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

        public enum SlackMessageEnum
        {
            Restock,
            Atc,
            Checkout,
            PayPal
        }
    }
}

