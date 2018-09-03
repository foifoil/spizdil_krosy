namespace EveAIO
{
    using EveAIO.Privacy;
    using EveAIO.Tasks;
    using EveAIO.Views;
    using log4net;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Net;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Windows;

    internal static class Global
    {
        public static AppDomain DOMAIN;
        public static Client CLIENT;
        public static Random RANDOM;
        public static Dictionary<string, string> DI_DATA;
        public static ILog Logger;
        public static DashboardView ViewDashboard;
        public static ProfilesView ViewProfiles;
        public static LogView ViewLog;
        public static CaptchaView ViewCaptcha;
        public static SuccessView ViewSuccess;
        public static ProxyView ViewProxy;
        public static NotificationsView ViewNotifications;
        public static SettingsView ViewSettings;
        public static ToolsView ViewTools;
        public static PrivacyManager PRIVACY_MANAGER;
        public static object MAIN_WINDOW;
        public static Settings SETTINGS;
        public static bool AsmLoaded;
        public static string HddSerial;
        public static string SERIAL;
        public static string SERIAL64;
        public static string Machine_name;
        public static string IP;
        public static bool TestingOn;
        public static string AES_KEY;
        public static string AES_IV;
        public static Assembly ASM;
        internal static Sensor SENSOR;
        public static ObservableCollection<TaskObject> CAPTCHA_QUEUE;
        public static ObservableCollection<SuccessObject> SUCCESS;
        public static bool LOG_PAUSED;
        public static List<KeyValuePair<string, string>> SUCCESS_PROFILES;
        public static List<ChromeDriver> ChromeDrivers;
        public static ObservableCollection<CaptchaToken> SupremeTokens;
        public static ObservableCollection<CaptchaToken> ShopifyTokens;
        public static ObservableCollection<CaptchaToken> SnsTokens;
        public static ObservableCollection<CaptchaToken> MrPorterTokens;
        public static ObservableCollection<CaptchaToken> SivasTokens;
        public static ObservableCollection<CaptchaToken> HibbettTokens;
        public static ObservableCollection<CaptchaToken> OWTokens;
        public static ObservableCollection<CaptchaToken> FootactionTokens;
        public static ObservableCollection<CaptchaToken> PrivacyTokens;
        public static ObservableCollection<CaptchaToken> HolypopTokens;
        public static object CaptchaLocker;
        public static object SolverLocker;
        public static object SuccessLocker;
        public static bool IsProxyTestRunning;
        public static List<KeyVal<string, int>> PROXY_USAGE;
        public static object ProxyLocker;
        public static List<string> RUNNING_SERVERS;
        public static object TmpKey;
        public static CaptchaSolverWindow CAPTCHA_SOLVER1;
        public static CaptchaSolverWindow CAPTCHA_SOLVER2;
        public static CaptchaSolverWindow CAPTCHA_SOLVER3;
        public static CaptchaSolverWindow CAPTCHA_SOLVER4;
        public static CaptchaSolverWindow CAPTCHA_SOLVER5;
        public static SupremePickerWindow SUPREME_PICKER;
        public static object CAPTCHA_ASSIGN_LOCK;
        public static List<ShopifySession> SHOPIFY_SESSIONS;
        public static object SessionLocker;
        public static List<KeyValuePair<string, string>> SHOPIFY_API_TOKENS;
        public static List<string> ADDED_VALS;
        public static bool CaptchaSolver1Opened;
        public static bool CaptchaSolver2Opened;
        public static bool CaptchaSolver3Opened;
        public static bool CaptchaSolver4Opened;
        public static bool CaptchaSolver5Opened;
        private static object L;

        static Global()
        {
            Class7.RIuqtBYzWxthF();
            DOMAIN = AppDomain.CurrentDomain;
            RANDOM = new Random(DateTime.Now.Millisecond);
            DI_DATA = new Dictionary<string, string>();
            Logger = LogManager.GetLogger("logger");
            PRIVACY_MANAGER = null;
            HddSerial = "";
            SERIAL = "";
            SERIAL64 = "";
            TestingOn = false;
            AES_KEY = "B84EFA83B3236@%=";
            AES_IV = "0664117802981456";
            SENSOR = null;
            CAPTCHA_QUEUE = new ObservableCollection<TaskObject>();
            SUCCESS = new ObservableCollection<SuccessObject>();
            SUCCESS_PROFILES = new List<KeyValuePair<string, string>>();
            ChromeDrivers = new List<ChromeDriver>();
            SupremeTokens = new ObservableCollection<CaptchaToken>();
            ShopifyTokens = new ObservableCollection<CaptchaToken>();
            SnsTokens = new ObservableCollection<CaptchaToken>();
            MrPorterTokens = new ObservableCollection<CaptchaToken>();
            SivasTokens = new ObservableCollection<CaptchaToken>();
            HibbettTokens = new ObservableCollection<CaptchaToken>();
            OWTokens = new ObservableCollection<CaptchaToken>();
            FootactionTokens = new ObservableCollection<CaptchaToken>();
            PrivacyTokens = new ObservableCollection<CaptchaToken>();
            HolypopTokens = new ObservableCollection<CaptchaToken>();
            CaptchaLocker = new object();
            SolverLocker = new object();
            SuccessLocker = new object();
            IsProxyTestRunning = false;
            PROXY_USAGE = new List<KeyVal<string, int>>();
            ProxyLocker = new object();
            RUNNING_SERVERS = new List<string>();
            CAPTCHA_ASSIGN_LOCK = new object();
            SHOPIFY_SESSIONS = new List<ShopifySession>();
            SessionLocker = new object();
            SHOPIFY_API_TOKENS = new List<KeyValuePair<string, string>>();
            ADDED_VALS = new List<string>();
            CaptchaSolver1Opened = false;
            CaptchaSolver2Opened = false;
            CaptchaSolver3Opened = false;
            CaptchaSolver4Opened = false;
            CaptchaSolver5Opened = false;
            L = new object();
        }

        private static void CAPTCHA_QUEUE_CollectionChanged(object sender, object e)
        {
            object l = L;
            lock (l)
            {
                if (((CAPTCHA_SOLVER1 != null) && CAPTCHA_SOLVER1.IsFree) && CAPTCHA_SOLVER1.IsUserEnabled)
                {
                    CAPTCHA_SOLVER1.Dispatcher.BeginInvoke(delegate {
                        if (!CaptchaSolver1Opened)
                        {
                            CAPTCHA_SOLVER1.Show();
                            CAPTCHA_SOLVER1.Visibility = Visibility.Visible;
                            CaptchaSolver1Opened = true;
                            CAPTCHA_SOLVER1.Activate();
                        }
                        CAPTCHA_SOLVER1.IsFree = false;
                        CAPTCHA_SOLVER1.Change();
                    }, Array.Empty<object>());
                }
                else if (((CAPTCHA_SOLVER2 != null) && CAPTCHA_SOLVER2.IsFree) && CAPTCHA_SOLVER2.IsUserEnabled)
                {
                    CAPTCHA_SOLVER2.Dispatcher.BeginInvoke(delegate {
                        if (!CaptchaSolver2Opened)
                        {
                            CAPTCHA_SOLVER2.Show();
                            CAPTCHA_SOLVER2.Visibility = Visibility.Visible;
                            CaptchaSolver2Opened = true;
                            CAPTCHA_SOLVER2.Activate();
                        }
                        CAPTCHA_SOLVER2.IsFree = false;
                        CAPTCHA_SOLVER2.Change();
                    }, Array.Empty<object>());
                }
                else if (((CAPTCHA_SOLVER3 != null) && CAPTCHA_SOLVER3.IsFree) && CAPTCHA_SOLVER3.IsUserEnabled)
                {
                    CAPTCHA_SOLVER3.Dispatcher.BeginInvoke(delegate {
                        if (!CaptchaSolver1Opened)
                        {
                            CAPTCHA_SOLVER3.Show();
                            CAPTCHA_SOLVER3.Visibility = Visibility.Visible;
                            CaptchaSolver3Opened = true;
                            CAPTCHA_SOLVER3.Activate();
                        }
                        CAPTCHA_SOLVER3.IsFree = false;
                        CAPTCHA_SOLVER3.Change();
                    }, Array.Empty<object>());
                }
                else if (((CAPTCHA_SOLVER4 != null) && CAPTCHA_SOLVER4.IsFree) && CAPTCHA_SOLVER4.IsUserEnabled)
                {
                    CAPTCHA_SOLVER4.Dispatcher.BeginInvoke(delegate {
                        if (!CaptchaSolver4Opened)
                        {
                            CAPTCHA_SOLVER4.Show();
                            CAPTCHA_SOLVER4.Visibility = Visibility.Visible;
                            CaptchaSolver4Opened = true;
                            CAPTCHA_SOLVER4.Activate();
                        }
                        CAPTCHA_SOLVER4.IsFree = false;
                        CAPTCHA_SOLVER4.Change();
                    }, Array.Empty<object>());
                }
                else if (((CAPTCHA_SOLVER5 != null) && CAPTCHA_SOLVER5.IsFree) && CAPTCHA_SOLVER5.IsUserEnabled)
                {
                    CAPTCHA_SOLVER5.Dispatcher.BeginInvoke(delegate {
                        if (!CaptchaSolver5Opened)
                        {
                            CAPTCHA_SOLVER5.Show();
                            CAPTCHA_SOLVER5.Visibility = Visibility.Visible;
                            CaptchaSolver5Opened = true;
                            CAPTCHA_SOLVER5.Activate();
                        }
                        CAPTCHA_SOLVER5.IsFree = false;
                        CAPTCHA_SOLVER5.Change();
                    }, Array.Empty<object>());
                }
            }
        }

        internal static string CreateRequest(string typ)
        {
            string str = (("{\"call_id\":\"" + EveAIO.Helpers.Md5(DateTime.UtcNow.ToString("yyyy-MM-dd")) + "\", ") + "\"action\":\"" + typ + "\", ") + "\"key\":\"{0}\", ";
            string[] textArray1 = new string[] { str, "\"product_copy\":\"eveaio&", EveAIO.Helpers.GetHardDiskSerialNo(), "&", Environment.UserName.Trim(), "\"}" };
            return string.Concat(textArray1);
        }

        public static void Init()
        {
            Version version = Assembly.GetEntryAssembly().GetName().Version;
            object[] objArray1 = new object[] { "v", version.Major, ".", version.Revision };
            VERSION = string.Concat(objArray1);
            HddSerial = EveAIO.Helpers.GetHardDiskSerialNo();
            if (HddSerial.Length > 0x10)
            {
                HddSerial = HddSerial.Substring(0, 0x10);
            }
            while (HddSerial.Length < 0x10)
            {
                HddSerial = HddSerial + "A";
            }
            EncryptorAes.Key = HddSerial;
            Machine_name = Environment.UserName;
            EveAIO.Helpers.LoadSettings();
            ViewDashboard = new DashboardView();
            ViewProfiles = new ProfilesView();
            ViewLog = new LogView();
            ViewCaptcha = new CaptchaView();
            ViewSuccess = new SuccessView();
            ViewProxy = new ProxyView();
            ViewNotifications = new NotificationsView();
            ViewSettings = new SettingsView();
            ViewTools = new ToolsView();
            CAPTCHA_QUEUE.CollectionChanged += new NotifyCollectionChangedEventHandler(Global.CAPTCHA_QUEUE_CollectionChanged);
            CLIENT = new Client(null, null, false);
            CLIENT.SetDesktopAgent();
            CLIENT.Session.DefaultRequestHeaders.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");
            CLIENT.Session.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "en-GB, en-US; q=0.9, en; q=0.8");
            CLIENT.Session.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
        }

        internal static void ProxyCheck()
        {
            if (WebRequest.GetSystemWebProxy().GetProxy(new Uri("http://www.google.de")).Host.Equals("127.0.0.1"))
            {
                Environment.Exit(0);
            }
        }

        public static string VERSION
        {
            [CompilerGenerated]
            get => 
                <VERSION>k__BackingField;
            [CompilerGenerated]
            set => 
                (<VERSION>k__BackingField = value);
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly Global.<>c <>9;
            public static Action <>9__74_0;
            public static Action <>9__74_1;
            public static Action <>9__74_2;
            public static Action <>9__74_3;
            public static Action <>9__74_4;

            static <>c()
            {
                Class7.RIuqtBYzWxthF();
                <>9 = new Global.<>c();
            }

            public <>c()
            {
                Class7.RIuqtBYzWxthF();
            }

            internal void <CAPTCHA_QUEUE_CollectionChanged>b__74_0()
            {
                if (!Global.CaptchaSolver1Opened)
                {
                    Global.CAPTCHA_SOLVER1.Show();
                    Global.CAPTCHA_SOLVER1.Visibility = Visibility.Visible;
                    Global.CaptchaSolver1Opened = true;
                    Global.CAPTCHA_SOLVER1.Activate();
                }
                Global.CAPTCHA_SOLVER1.IsFree = false;
                Global.CAPTCHA_SOLVER1.Change();
            }

            internal void <CAPTCHA_QUEUE_CollectionChanged>b__74_1()
            {
                if (!Global.CaptchaSolver2Opened)
                {
                    Global.CAPTCHA_SOLVER2.Show();
                    Global.CAPTCHA_SOLVER2.Visibility = Visibility.Visible;
                    Global.CaptchaSolver2Opened = true;
                    Global.CAPTCHA_SOLVER2.Activate();
                }
                Global.CAPTCHA_SOLVER2.IsFree = false;
                Global.CAPTCHA_SOLVER2.Change();
            }

            internal void <CAPTCHA_QUEUE_CollectionChanged>b__74_2()
            {
                if (!Global.CaptchaSolver1Opened)
                {
                    Global.CAPTCHA_SOLVER3.Show();
                    Global.CAPTCHA_SOLVER3.Visibility = Visibility.Visible;
                    Global.CaptchaSolver3Opened = true;
                    Global.CAPTCHA_SOLVER3.Activate();
                }
                Global.CAPTCHA_SOLVER3.IsFree = false;
                Global.CAPTCHA_SOLVER3.Change();
            }

            internal void <CAPTCHA_QUEUE_CollectionChanged>b__74_3()
            {
                if (!Global.CaptchaSolver4Opened)
                {
                    Global.CAPTCHA_SOLVER4.Show();
                    Global.CAPTCHA_SOLVER4.Visibility = Visibility.Visible;
                    Global.CaptchaSolver4Opened = true;
                    Global.CAPTCHA_SOLVER4.Activate();
                }
                Global.CAPTCHA_SOLVER4.IsFree = false;
                Global.CAPTCHA_SOLVER4.Change();
            }

            internal void <CAPTCHA_QUEUE_CollectionChanged>b__74_4()
            {
                if (!Global.CaptchaSolver5Opened)
                {
                    Global.CAPTCHA_SOLVER5.Show();
                    Global.CAPTCHA_SOLVER5.Visibility = Visibility.Visible;
                    Global.CaptchaSolver5Opened = true;
                    Global.CAPTCHA_SOLVER5.Activate();
                }
                Global.CAPTCHA_SOLVER5.IsFree = false;
                Global.CAPTCHA_SOLVER5.Change();
            }
        }

        public enum FormOperation
        {
            insert,
            update
        }
    }
}

