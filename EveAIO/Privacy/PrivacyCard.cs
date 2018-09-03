namespace EveAIO.Privacy
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Threading;

    internal class PrivacyCard : INotifyPropertyChanged
    {
        private string _cardId;
        private string _cvv;
        private string _expMonth;
        private string _expYear;
        private string _name;
        private string _cardNumber;

        [field: CompilerGenerated]
        public event PropertyChangedEventHandler PropertyChanged;

        public PrivacyCard()
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

        public string CardId
        {
            get => 
                this._cardId;
            set
            {
                if (this._cardId != value)
                {
                    this._cardId = value;
                    this.OnPropertyChanged("CardId");
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

        public string CardNumber
        {
            get => 
                this._cardNumber;
            set
            {
                if (this._cardNumber != value)
                {
                    this._cardNumber = value;
                    this.OnPropertyChanged("CardNumber");
                }
            }
        }

        public string ExpMonth
        {
            get => 
                this._expMonth;
            set
            {
                if (this._expMonth != value)
                {
                    this._expMonth = value;
                    this.OnPropertyChanged("ExpMonth");
                }
            }
        }

        public string ExpYear
        {
            get => 
                this._expYear;
            set
            {
                if (this._expYear != value)
                {
                    this._expYear = value;
                    this.OnPropertyChanged("ExpYear");
                }
            }
        }

        public string Exp =>
            (this._expMonth + "/" + this._expYear);

        public string Cvv
        {
            get => 
                this._cvv;
            set
            {
                if (this._cvv != value)
                {
                    this._cvv = value;
                    this.OnPropertyChanged("Cvv");
                }
            }
        }
    }
}

