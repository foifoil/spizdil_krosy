namespace EveAIO.Pocos
{
    using EveAIO;
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Xml.Serialization;

    public class ProfileGroupObject : INotifyPropertyChanged
    {
        private string _id;
        private string _groupName;
        [XmlIgnore, Browsable(false)]
        public object AssignLock;

        [field: CompilerGenerated]
        public event PropertyChangedEventHandler PropertyChanged;

        public ProfileGroupObject()
        {
            Class7.RIuqtBYzWxthF();
            this.AssignLock = new object();
            this.Id = Guid.NewGuid().ToString();
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
                this._groupName;
            set
            {
                if (this._groupName != value)
                {
                    this._groupName = value;
                    this.OnPropertyChanged("Name");
                }
            }
        }

        [XmlIgnore]
        public string Profiles =>
            Global.SETTINGS.PROFILES.Count<ProfileObject>(x => (x.IdGroup == this._id)).ToString();
    }
}

