namespace EveAIO.Pocos
{
    using EveAIO;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Xml.Serialization;

    public class ProfileObject : INotifyPropertyChanged
    {
        private string _id;
        private string _name;
        private string _firstName;
        private string _lastName;
        private string _address1;
        private string _address2;
        private string _city;
        private string _stateId;
        private string _state;
        private string _countryId;
        private string _email;
        private string _zip;
        private string _phone;
        private string _nameOnCard;
        private string _ccNumber;
        private string _expiryMonth;
        private string _expiryYear;
        private string _cvv;
        private string _cardTypeId;
        private bool _onePerWebsite;
        private bool _sameBillingShipping;
        private string _firstNameShipping;
        private string _lastNameShipping;
        private string _address1Shipping;
        private string _address2Shipping;
        private string _cityShipping;
        private string _stateIdShipping;
        private string _stateShipping;
        private string _countryIdShipping;
        private string _emailShipping;
        private string _zipShipping;
        private string _phoneShipping;
        private int _no;
        private bool _privacy;
        private string _privacyCardName;
        private string _idGroup;
        private string _birthdayDay;
        private string _birthdayMonth;
        private string _birthdayYear;
        private string _variousBilling;
        private string _variousShipping;

        [field: CompilerGenerated]
        public event PropertyChangedEventHandler PropertyChanged;

        public ProfileObject()
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

        [DisplayName("Profile name")]
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

        [DisplayName("First name")]
        public string FirstName
        {
            get => 
                this._firstName;
            set
            {
                if (this._firstName != value)
                {
                    this._firstName = value;
                    this.OnPropertyChanged("FirstName");
                    this.OnPropertyChanged("WholeName1");
                }
            }
        }

        [DisplayName("Last name")]
        public string LastName
        {
            get => 
                this._lastName;
            set
            {
                if (this._lastName != value)
                {
                    this._lastName = value;
                    this.OnPropertyChanged("LastName");
                    this.OnPropertyChanged("WholeName1");
                }
            }
        }

        [DisplayName("Address #1")]
        public string Address1
        {
            get => 
                this._address1;
            set
            {
                if (this._address1 != value)
                {
                    this._address1 = value;
                    this.OnPropertyChanged("Address1");
                    this.OnPropertyChanged("WholeAddress1");
                }
            }
        }

        [DisplayName("Address #2")]
        public string Address2
        {
            get => 
                this._address2;
            set
            {
                if (this._address2 != value)
                {
                    this._address2 = value;
                    this.OnPropertyChanged("Address2");
                    this.OnPropertyChanged("WholeAddress1");
                }
            }
        }

        public string City
        {
            get => 
                this._city;
            set
            {
                if (this._city != value)
                {
                    this._city = value;
                    this.OnPropertyChanged("City");
                    this.OnPropertyChanged("WholeCity1");
                }
            }
        }

        [Browsable(false)]
        public string StateId
        {
            get => 
                this._stateId;
            set
            {
                if (this._stateId != value)
                {
                    this._stateId = value;
                    this.OnPropertyChanged("StateId");
                    this.OnPropertyChanged("WholeCity1");
                }
            }
        }

        public string State
        {
            get => 
                this._state;
            set
            {
                if (this._state != value)
                {
                    this._state = value;
                    this.OnPropertyChanged("_state");
                    this.OnPropertyChanged("WholeCity1");
                }
            }
        }

        [Browsable(false)]
        public string CountryId
        {
            get => 
                this._countryId;
            set
            {
                if (this._countryId != value)
                {
                    this._countryId = value;
                    this.OnPropertyChanged("CountryId");
                    this.OnPropertyChanged("Country");
                }
            }
        }

        public string Country
        {
            get
            {
                Type type = Global.ASM.GetType("SvcHost.SvcHost");
                MethodInfo method = type.GetMethod("GetCountries");
                object obj2 = Activator.CreateInstance(type);
                return ((List<KeyValuePair<string, string>>) method.Invoke(obj2, null)).First<KeyValuePair<string, string>>(x => (x.Key == this._countryId)).Value;
            }
        }

        public string Email
        {
            get => 
                this._email;
            set
            {
                if (this._email != value)
                {
                    this._email = value;
                    this.OnPropertyChanged("Email");
                }
            }
        }

        [Browsable(false)]
        public string Zip
        {
            get => 
                this._zip;
            set
            {
                if (this._zip != value)
                {
                    this._zip = value;
                    this.OnPropertyChanged("Zip");
                    this.OnPropertyChanged("WholeCity1");
                }
            }
        }

        [Browsable(false)]
        public string Phone
        {
            get => 
                this._phone;
            set
            {
                if (this._phone != value)
                {
                    this._phone = value;
                    this.OnPropertyChanged("Phone");
                }
            }
        }

        [Browsable(false)]
        public string NameOnCard
        {
            get => 
                this._nameOnCard;
            set
            {
                if (this._nameOnCard != value)
                {
                    this._nameOnCard = value;
                    this.OnPropertyChanged("NameOnCard");
                }
            }
        }

        [DisplayName("Card number")]
        public string CCNumber
        {
            get => 
                this._ccNumber;
            set
            {
                if (this._ccNumber != value)
                {
                    this._ccNumber = value;
                    this.OnPropertyChanged("CCNumber");
                }
            }
        }

        [DisplayName("Card type")]
        public string CardType
        {
            get
            {
                Type type = Global.ASM.GetType("SvcHost.SvcHost");
                MethodInfo method = type.GetMethod("GetCardTypes");
                object obj2 = Activator.CreateInstance(type);
                List<KeyValuePair<string, string>> source = (List<KeyValuePair<string, string>>) method.Invoke(obj2, null);
                if (source.Any<KeyValuePair<string, string>>(x => x.Key == this._cardTypeId))
                {
                    return source.First<KeyValuePair<string, string>>(x => (x.Key == this._cardTypeId)).Value;
                }
                return "";
            }
        }

        [Browsable(false)]
        public string ExpiryMonth
        {
            get => 
                this._expiryMonth;
            set
            {
                if (this._expiryMonth != value)
                {
                    this._expiryMonth = value;
                    this.OnPropertyChanged("ExpiryMonth");
                    this.OnPropertyChanged("CardExp");
                }
            }
        }

        [Browsable(false)]
        public string ExpiryYear
        {
            get => 
                this._expiryYear;
            set
            {
                if (this._expiryYear != value)
                {
                    this._expiryYear = value;
                    this.OnPropertyChanged("ExpiryYear");
                    this.OnPropertyChanged("CardExp");
                }
            }
        }

        public string Expiration =>
            (this._expiryMonth + "/" + this._expiryYear);

        [Browsable(false)]
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

        [Browsable(false)]
        public string CardTypeId
        {
            get => 
                this._cardTypeId;
            set
            {
                if (this._cardTypeId != value)
                {
                    this._cardTypeId = value;
                    this.OnPropertyChanged("CardTypeId");
                    this.OnPropertyChanged("CardType");
                }
            }
        }

        [Browsable(false)]
        public bool OnePerWebsite
        {
            get => 
                this._onePerWebsite;
            set
            {
                if (this._onePerWebsite != value)
                {
                    this._onePerWebsite = value;
                    this.OnPropertyChanged("OnePerWebsite");
                    this.OnPropertyChanged("OnePerWeb");
                }
            }
        }

        [Browsable(false)]
        public bool Privacy
        {
            get => 
                this._privacy;
            set
            {
                if (this._privacy != value)
                {
                    this._privacy = value;
                    this.OnPropertyChanged("Privacy");
                    this.OnPropertyChanged("PrivacyDesc");
                }
            }
        }

        [Browsable(false)]
        public string FirstNameShipping
        {
            get => 
                this._firstNameShipping;
            set
            {
                if (this._firstNameShipping != value)
                {
                    this._firstNameShipping = value;
                    this.OnPropertyChanged("FirstNameShipping");
                    this.OnPropertyChanged("WholeName2");
                }
            }
        }

        [Browsable(false)]
        public string LastNameShipping
        {
            get => 
                this._lastNameShipping;
            set
            {
                if (this._lastNameShipping != value)
                {
                    this._lastNameShipping = value;
                    this.OnPropertyChanged("LastNameShipping");
                    this.OnPropertyChanged("WholeName2");
                }
            }
        }

        [Browsable(false)]
        public string Address1Shipping
        {
            get => 
                this._address1Shipping;
            set
            {
                if (this._address1Shipping != value)
                {
                    this._address1Shipping = value;
                    this.OnPropertyChanged("Address1Shipping");
                    this.OnPropertyChanged("WholeAddress2");
                }
            }
        }

        [Browsable(false)]
        public string Address2Shipping
        {
            get => 
                this._address2Shipping;
            set
            {
                if (this._address2Shipping != value)
                {
                    this._address2Shipping = value;
                    this.OnPropertyChanged("Address2Shipping");
                    this.OnPropertyChanged("WholeAddress2");
                }
            }
        }

        [Browsable(false)]
        public string CityShipping
        {
            get => 
                this._cityShipping;
            set
            {
                if (this._cityShipping != value)
                {
                    this._cityShipping = value;
                    this.OnPropertyChanged("CityShipping");
                    this.OnPropertyChanged("WholeCity2");
                }
            }
        }

        [Browsable(false)]
        public string StateIdShipping
        {
            get => 
                this._stateIdShipping;
            set
            {
                if (this._stateIdShipping != value)
                {
                    this._stateIdShipping = value;
                    this.OnPropertyChanged("StateIdShipping");
                    this.OnPropertyChanged("WholeCity2");
                }
            }
        }

        [Browsable(false)]
        public string StateShipping
        {
            get => 
                this._stateShipping;
            set
            {
                if (this._stateShipping != value)
                {
                    this._stateShipping = value;
                    this.OnPropertyChanged("StateShipping");
                    this.OnPropertyChanged("WholeCity2");
                }
            }
        }

        [Browsable(false)]
        public string CountryIdShipping
        {
            get => 
                this._countryIdShipping;
            set
            {
                if (this._countryIdShipping != value)
                {
                    this._countryIdShipping = value;
                    this.OnPropertyChanged("CountryIdShipping");
                    this.OnPropertyChanged("CountryShipping");
                }
            }
        }

        [Browsable(false)]
        public string CountryShipping
        {
            get
            {
                Type type = Global.ASM.GetType("SvcHost.SvcHost");
                MethodInfo method = type.GetMethod("GetCountries");
                object obj2 = Activator.CreateInstance(type);
                return ((List<KeyValuePair<string, string>>) method.Invoke(obj2, null)).First<KeyValuePair<string, string>>(x => (x.Key == this._countryIdShipping)).Value;
            }
        }

        [Browsable(false)]
        public string EmailShipping
        {
            get => 
                this._emailShipping;
            set
            {
                if (this._emailShipping != value)
                {
                    this._emailShipping = value;
                    this.OnPropertyChanged("EmailShipping");
                }
            }
        }

        [Browsable(false)]
        public string ZipShipping
        {
            get => 
                this._zipShipping;
            set
            {
                if (this._zipShipping != value)
                {
                    this._zipShipping = value;
                    this.OnPropertyChanged("ZipShipping");
                }
            }
        }

        [Browsable(false)]
        public string PhoneShipping
        {
            get => 
                this._phoneShipping;
            set
            {
                if (this._phoneShipping != value)
                {
                    this._phoneShipping = value;
                    this.OnPropertyChanged("PhoneShipping");
                }
            }
        }

        [Browsable(false)]
        public bool SameBillingShipping
        {
            get => 
                this._sameBillingShipping;
            set
            {
                if (this._sameBillingShipping != value)
                {
                    this._sameBillingShipping = value;
                    this.OnPropertyChanged("SameBillingShipping");
                    this.OnPropertyChanged("SameInfo");
                }
            }
        }

        [Browsable(false)]
        public string IdGroup
        {
            get => 
                this._idGroup;
            set
            {
                if (this._idGroup != value)
                {
                    this._idGroup = value;
                    this.OnPropertyChanged("IdGroup");
                    this.OnPropertyChanged("GroupDesc");
                }
            }
        }

        [XmlIgnore, Browsable(false)]
        public int No
        {
            get => 
                this._no;
            set
            {
                if (this._no != value)
                {
                    this._no = value;
                    this.OnPropertyChanged("No");
                }
            }
        }

        [XmlIgnore, Browsable(false)]
        public string WholeName1 =>
            (this._firstName + " " + this._lastName);

        [XmlIgnore, Browsable(false)]
        public string WholeAddress1 =>
            (this._address1 + " " + this._address2);

        [XmlIgnore, Browsable(false)]
        public string WholeCity1
        {
            get
            {
                string[] textArray1 = new string[] { this._city, ", ", this._state, " ", this._zip };
                return string.Concat(textArray1);
            }
        }

        [XmlIgnore, Browsable(false)]
        public string WholeName2 =>
            (this._firstNameShipping + " " + this._lastNameShipping);

        [XmlIgnore, Browsable(false)]
        public string WholeAddress2 =>
            (this._address1Shipping + " " + this._address2Shipping);

        [XmlIgnore, Browsable(false)]
        public string WholeCity2
        {
            get
            {
                string[] textArray1 = new string[] { this._cityShipping, ", ", this._stateShipping, " ", this._zipShipping };
                return string.Concat(textArray1);
            }
        }

        [XmlIgnore, Browsable(false)]
        public string CardExp =>
            (this._expiryMonth + "/" + this._expiryYear);

        [Browsable(false), XmlIgnore]
        public string OnePerWeb
        {
            get
            {
                if (this._onePerWebsite)
                {
                    return "One per website: ✔";
                }
                return "One per website: ✘";
            }
        }

        [XmlIgnore, Browsable(false)]
        public string SameInfo
        {
            get
            {
                if (this._sameBillingShipping)
                {
                    return "Same bill/ship info: ✔";
                }
                return "Same bill/ship info: ✘";
            }
        }

        [Browsable(false)]
        public string PrivacyCardName
        {
            get => 
                this._privacyCardName;
            set
            {
                if (this._privacyCardName != value)
                {
                    this._privacyCardName = value;
                    this.OnPropertyChanged("PrivacyCardName");
                }
            }
        }

        [XmlIgnore, Browsable(false)]
        public string PrivacyDesc
        {
            get
            {
                if (this._privacy)
                {
                    return "Privacy: ✔";
                }
                return "Privacy: ✘";
            }
        }

        [Browsable(false), XmlIgnore]
        public string GroupDesc
        {
            get
            {
                if (!string.IsNullOrEmpty(this._idGroup) && Global.SETTINGS.PROFILES_GROUPS.Any<ProfileGroupObject>(x => (x.Id == this._idGroup)))
                {
                    return ("Group: " + Global.SETTINGS.PROFILES_GROUPS.First<ProfileGroupObject>(x => (x.Id == this._idGroup)).Name);
                }
                return "Group: ✘";
            }
        }

        [Browsable(false)]
        public string BirthdayDay
        {
            get => 
                this._birthdayDay;
            set
            {
                if (this._birthdayDay != value)
                {
                    this._birthdayDay = value;
                    this.OnPropertyChanged("BirthdayDay");
                }
            }
        }

        [Browsable(false)]
        public string BirthdayMonth
        {
            get => 
                this._birthdayMonth;
            set
            {
                if (this._birthdayMonth != value)
                {
                    this._birthdayMonth = value;
                    this.OnPropertyChanged("BirthdayMonth");
                }
            }
        }

        [Browsable(false)]
        public string BirthdayYear
        {
            get => 
                this._birthdayYear;
            set
            {
                if (this._birthdayYear != value)
                {
                    this._birthdayYear = value;
                    this.OnPropertyChanged("BirthdayYear");
                }
            }
        }

        [Browsable(false)]
        public string VariousBilling
        {
            get => 
                this._variousBilling;
            set
            {
                if (this._variousBilling != value)
                {
                    this._variousBilling = value;
                    this.OnPropertyChanged("VariousBilling");
                }
            }
        }

        [Browsable(false)]
        public string VariousShipping
        {
            get => 
                this._variousShipping;
            set
            {
                if (this._variousShipping != value)
                {
                    this._variousShipping = value;
                    this.OnPropertyChanged("VariousShipping");
                }
            }
        }
    }
}

