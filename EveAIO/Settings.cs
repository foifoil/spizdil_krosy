namespace EveAIO
{
    using EveAIO.Pocos;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;

    public class Settings
    {
        public SynchronizedObservableCollection<TaskObject> TASKS;
        public SynchronizedObservableCollection<ProfileObject> PROFILES;
        public SynchronizedObservableCollection<ProxyListObject> PROXIES;
        public SynchronizedObservableCollection<TwitterObject> TWITTER;
        public SynchronizedObservableCollection<SlackObject> SLACK;
        public SynchronizedObservableCollection<SmsObject> SMS;
        public SynchronizedObservableCollection<DiscordObject> DISCORD;
        public SynchronizedObservableCollection<ProfileGroupObject> PROFILES_GROUPS;
        public QuickTaskObject QUICK_TASK;
        public List<LoginPool> LOGIN_POOL;
        public int ShopifySmartDelay;
        public bool CollapseTasks;
        public bool TwitterOn;
        public bool SlackOn;
        public bool SmsOn;
        public bool DiscordOn;
        public string TwoCaptchaApiKey;
        public bool TwoCaptchaHarvester;
        public string ImageTypersUsername;
        public string ImageTypersPassword;
        public bool ImageTypersHarvester;
        public string AntiCaptchaApiKey;
        public bool AntiCaptchaHarvester;
        public string DisolveIp;
        public string DisolveApiKey;
        public bool DisolveHarvester;
        public bool PowerMode;
        public string SupremeCaptchaKey;
        public string SnsCaptchaKey;
        public string ShopifyCaptchaKey;
        public int HarvesterTokens;
        public int HarvesterSeconds;
        public bool LogCleaner;
        public int LogCleanerMinutes;
        public bool DetailedLog;
        public string PrivacyEmail;
        public string PrivacyPassword;
        public CaptchaServiceType PrivacyCaptchaService;
        public string GmailAddress;
        public string GmailPassword;
        public bool LogAutoScroll;
        public bool ClearSession;
        public bool SolverBeep;
        public bool AtcBeep;
        public bool CheckoutBeep;
        public bool PayPalBeep;
        public string ImagePickerProxyId;
        public bool Solver1Enabled;
        public bool Solver2Enabled;
        public bool Solver3Enabled;
        public bool Solver4Enabled;
        public bool Solver5Enabled;
        public string Solver1ProxyList;
        public string Solver2ProxyList;
        public string Solver3ProxyList;
        public string Solver4ProxyList;
        public string Solver5ProxyList;
        public bool EnvLight;

        public Settings()
        {
            Class7.RIuqtBYzWxthF();
            this.TASKS = new SynchronizedObservableCollection<TaskObject>();
            this.PROFILES = new SynchronizedObservableCollection<ProfileObject>();
            this.PROXIES = new SynchronizedObservableCollection<ProxyListObject>();
            this.TWITTER = new SynchronizedObservableCollection<TwitterObject>();
            this.SLACK = new SynchronizedObservableCollection<SlackObject>();
            this.SMS = new SynchronizedObservableCollection<SmsObject>();
            this.DISCORD = new SynchronizedObservableCollection<DiscordObject>();
            this.PROFILES_GROUPS = new SynchronizedObservableCollection<ProfileGroupObject>();
            this.QUICK_TASK = new QuickTaskObject();
            this.LOGIN_POOL = new List<LoginPool>();
            this.ShopifySmartDelay = 10;
            this.HarvesterTokens = 3;
            this.HarvesterSeconds = 60;
            this.ImagePickerProxyId = "";
            this.EnvLight = true;
            this.TASKS.CollectionChanged += new NotifyCollectionChangedEventHandler(this.TASKS_CollectionChanged);
            this.PROFILES.CollectionChanged += new NotifyCollectionChangedEventHandler(this.PROFILES_CollectionChanged);
            this.PROXIES.CollectionChanged += new NotifyCollectionChangedEventHandler(this.PROXIES_CollectionChanged);
        }

        private void PROFILES_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (Global.SETTINGS != null)
            {
                Global.MAIN_WINDOW.txtTotalProfiles.Text = "Total profiles: " + Global.SETTINGS.PROFILES.Count;
                for (int i = 0; i < Global.SETTINGS.PROFILES.Count; i++)
                {
                    Global.SETTINGS.PROFILES[i].No = i + 1;
                }
            }
        }

        private void PROXIES_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (Global.SETTINGS != null)
            {
                Global.MAIN_WINDOW.txtTotalProxyLists.Text = "Total proxy lists: " + Global.SETTINGS.PROXIES.Count;
            }
        }

        public void Save()
        {
            Global.DOMAIN.SetData("settings", this);
        }

        private void TASKS_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (Global.SETTINGS != null)
            {
                Global.MAIN_WINDOW.txtTotalTasks.Text = "Total tasks: " + Global.SETTINGS.TASKS.Count;
            }
        }
    }
}

