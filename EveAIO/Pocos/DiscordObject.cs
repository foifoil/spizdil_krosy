namespace EveAIO.Pocos
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class DiscordObject : INotifyPropertyChanged
    {
        private DiscordMessageEnum _type;
        private string _webhookId;
        private string _webhookToken;
        private string _message;

        [field: CompilerGenerated]
        public event PropertyChangedEventHandler PropertyChanged;

        public DiscordObject()
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
        public DiscordMessageEnum DiscordType
        {
            get => 
                this._type;
            set
            {
                if (this._type != value)
                {
                    this._type = value;
                    this.OnPropertyChanged("DiscordType");
                }
            }
        }

        public string WebhookId
        {
            get => 
                this._webhookId;
            set
            {
                if (this._webhookId != value)
                {
                    this._webhookId = value;
                    this.OnPropertyChanged("WebhookId");
                }
            }
        }

        public string WebhookToken
        {
            get => 
                this._webhookToken;
            set
            {
                if (this._webhookToken != value)
                {
                    this._webhookToken = value;
                    this.OnPropertyChanged("WebhookToken");
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

        public enum DiscordMessageEnum
        {
            Restock,
            Atc,
            Checkout,
            PayPal
        }
    }
}

