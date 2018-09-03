namespace EveAIO.Captcha
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Xml.Serialization;

    internal class CaptchaToken : INotifyPropertyChanged
    {
        private string _id;
        private string _token;
        private CaptchaSolver.CaptchaService _captchaType;
        private DateTime _timestamp;
        private DateTime _created;
        private DateTime _expires;
        private string _platform;
        private string _website;
        private int _solveTime;

        [field: CompilerGenerated]
        public event PropertyChangedEventHandler PropertyChanged;

        public CaptchaToken()
        {
            Class7.RIuqtBYzWxthF();
            this.Created = DateTime.Now;
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

        public string Token
        {
            get => 
                this._token;
            set
            {
                if (this._token != value)
                {
                    this._token = value;
                    this.OnPropertyChanged("Token");
                }
            }
        }

        public string Platform
        {
            get => 
                this._platform;
            set
            {
                if (this._platform != value)
                {
                    this._platform = value;
                    this.OnPropertyChanged("Platform");
                }
            }
        }

        public string Website
        {
            get => 
                this._website;
            set
            {
                if (this._website != value)
                {
                    this._website = value;
                    this.OnPropertyChanged("Website");
                }
            }
        }

        public DateTime Timestamp
        {
            get => 
                this._timestamp;
            set
            {
                if (this._timestamp != value)
                {
                    this._timestamp = value;
                    this.OnPropertyChanged("Timestamp");
                }
            }
        }

        public DateTime Created
        {
            get => 
                this._created;
            set
            {
                if (this._created != value)
                {
                    this._created = value;
                    this.OnPropertyChanged("Created");
                }
            }
        }

        public DateTime Expires
        {
            get => 
                this._expires;
            set
            {
                if (this._expires != value)
                {
                    this._expires = value;
                    this.OnPropertyChanged("Expires");
                }
            }
        }

        public int SolveTime
        {
            get => 
                this._solveTime;
            set
            {
                if (this._solveTime != value)
                {
                    this._solveTime = value;
                    this.OnPropertyChanged("SolveTime");
                }
            }
        }

        [Browsable(false)]
        public CaptchaSolver.CaptchaService CaptchaType
        {
            get => 
                this._captchaType;
            set
            {
                if (this._captchaType != value)
                {
                    this._captchaType = value;
                    this.OnPropertyChanged("CaptchaType");
                }
            }
        }

        [XmlIgnore, DisplayName(" ")]
        public string CaptchaTypeImg
        {
            get
            {
                string str = "";
                switch (this._captchaType)
                {
                    case CaptchaSolver.CaptchaService.TwoCaptcha:
                        return "../Images/Captcha/2captcha_icon.png";

                    case CaptchaSolver.CaptchaService.AntiCaptcha:
                        return "../Images/Captcha/anticaptcha_icon.png";

                    case CaptchaSolver.CaptchaService.ImageTypers:
                        return "../Images/Captcha/imagetypers_icon.png";

                    case CaptchaSolver.CaptchaService.Disolve:
                        return str;

                    case CaptchaSolver.CaptchaService.Manual:
                        return "../Images/Captcha/hand_16x16.png";
                }
                return str;
            }
        }
    }
}

