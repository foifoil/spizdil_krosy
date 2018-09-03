namespace EveAIO.Pocos
{
    using Newtonsoft.Json;
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Xml.Serialization;

    public class ProxyObject : INotifyPropertyChanged, IEditableObject
    {
        private string _id;
        private bool _enabled;
        private string _ip;
        private int _port;
        private string _username;
        private string _password;
        private string _status;
        private StateEnum _state;

        [field: CompilerGenerated]
        public event PropertyChangedEventHandler PropertyChanged;

        public ProxyObject()
        {
            Class7.RIuqtBYzWxthF();
        }

        public void BeginEdit()
        {
        }

        public void CancelEdit()
        {
        }

        public void EndEdit()
        {
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

        [Browsable(false), Newtonsoft.Json.JsonIgnore]
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

        public bool Enabled
        {
            get => 
                this._enabled;
            set
            {
                if (this._enabled != value)
                {
                    this._enabled = value;
                    this.OnPropertyChanged("Enabled");
                }
            }
        }

        [Newtonsoft.Json.JsonIgnore, XmlIgnore, DisplayName(" ")]
        public string StateImg
        {
            get
            {
                switch (this._state)
                {
                    case StateEnum.untested:
                        return "../Images/ListsIcons/unknown.png";

                    case StateEnum.alive:
                        return "../Images/ListsIcons/ok.png";

                    case StateEnum.dead:
                        return "../Images/ListsIcons/dead.png";
                }
                return "";
            }
        }

        [Newtonsoft.Json.JsonIgnore, Browsable(false), XmlIgnore]
        public StateEnum State
        {
            get => 
                this._state;
            set
            {
                if (this._state != value)
                {
                    this._state = value;
                    this.OnPropertyChanged("State");
                    this.OnPropertyChanged("StateImg");
                }
            }
        }

        public string IP
        {
            get => 
                this._ip;
            set
            {
                if (this._ip != value)
                {
                    this._ip = value;
                    this.OnPropertyChanged("IP");
                }
            }
        }

        public int Port
        {
            get => 
                this._port;
            set
            {
                if (this._port != value)
                {
                    this._port = value;
                    this.OnPropertyChanged("Port");
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

        public string Password
        {
            get => 
                this._password;
            set
            {
                if (this._password != value)
                {
                    this._password = value;
                    this.OnPropertyChanged("Password");
                }
            }
        }

        [Newtonsoft.Json.JsonIgnore, XmlIgnore]
        public string Status
        {
            get => 
                this._status;
            set
            {
                if (this._status != value)
                {
                    this._status = value;
                    this.OnPropertyChanged("Status");
                }
            }
        }

        public enum StateEnum
        {
            untested,
            alive,
            dead
        }
    }
}

