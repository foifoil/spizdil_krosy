namespace EveAIO.Views
{
    using EveAIO;
    using EveAIO.Pocos;
    using EveAIO.Tasks;
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Markup;

    public class QuickTaskWindow : Window, IComponentConnector
    {
        private TaskObject.PlatformEnum _platform;
        internal Button BtnClose;
        public TextBox txtLink;
        internal TextBlock lblLinkType;
        public TextBox txtQuantity;
        internal GroupBox gbSizing;
        internal CheckBox chInherit;
        internal TextBlock lblSize;
        internal TextBox txtSize;
        internal CheckBox chSizeRandom;
        internal CheckBox chPickRandomNotAvailable;
        internal Button btnStart;
        private bool _contentLoaded;

        public QuickTaskWindow(Window owner)
        {
            Class7.RIuqtBYzWxthF();
            base.Owner = owner;
            this.InitializeComponent();
            this.chInherit.IsChecked = true;
            try
            {
                if (Global.SETTINGS.QUICK_TASK.Autopaste)
                {
                    this.txtLink.Text = Clipboard.GetText();
                }
            }
            catch
            {
            }
            if (Global.SETTINGS.PROFILES_GROUPS.Any<ProfileGroupObject>(x => x.Id == Global.SETTINGS.QUICK_TASK.IdProfile))
            {
                this.txtQuantity.Text = Global.SETTINGS.PROFILES_GROUPS.First<ProfileGroupObject>(x => (x.Id == Global.SETTINGS.QUICK_TASK.IdProfile)).Profiles;
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            base.Close();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            int num = 1;
            int num2 = 0;
            int num3 = 0;
            List<string> list = new List<string>();
            try
            {
                num = int.Parse(this.txtQuantity.Text.Trim());
            }
            catch
            {
            }
            List<string> ids = new List<string>();
            for (int i = 0; i < num; i++)
            {
                TaskObject obj3 = new TaskObject {
                    Id = Guid.NewGuid().ToString(),
                    Guid = Guid.NewGuid().ToString()
                };
                obj3.Name = "Quick task " + ((i + 1)).ToString();
                obj3.RandomSize = true;
                obj3.ProxyListId = Global.SETTINGS.QUICK_TASK.IdProxyList;
                obj3.RetryOnError = true;
                obj3.Notify = true;
                obj3.RetryDelay = Global.SETTINGS.QUICK_TASK.RetryDelay;
                obj3.Delay = Global.SETTINGS.QUICK_TASK.Delay;
                obj3.DelayFrom = Global.SETTINGS.QUICK_TASK.DelayFrom;
                obj3.DelayTo = Global.SETTINGS.QUICK_TASK.DelayTo;
                obj3.Payment = TaskObject.PaymentEnum.creditcard;
                obj3.TaskType = TaskObject.TaskTypeEnum.directlink;
                obj3.Platform = this._platform;
                obj3.AtcMethod = TaskObject.AtcMethodEnum.frontend;
                obj3.Link = this.txtLink.Text.Trim();
                obj3.Quantity = 1;
                TaskObject item = obj3;
                if (this.chInherit.IsChecked.HasValue && this.chInherit.IsChecked.Value)
                {
                    if (!string.IsNullOrEmpty(Global.SETTINGS.QUICK_TASK.Size) && !Global.SETTINGS.QUICK_TASK.RandomSize)
                    {
                        item.RandomSize = false;
                        item.Size = Global.SETTINGS.QUICK_TASK.Size;
                        item.SizePickRandom = Global.SETTINGS.QUICK_TASK.SizePickRandom;
                    }
                }
                else if ((this.chSizeRandom.IsChecked.HasValue && this.chSizeRandom.IsChecked.Value) || !string.IsNullOrEmpty(this.txtSize.Text.Trim()))
                {
                    item.RandomSize = !this.chSizeRandom.IsChecked.HasValue ? false : this.chSizeRandom.IsChecked.Value;
                    item.Size = this.txtSize.Text.ToString();
                    item.SizePickRandom = !this.chPickRandomNotAvailable.IsChecked.HasValue ? false : this.chPickRandomNotAvailable.IsChecked.Value;
                }
                if (this._platform != TaskObject.PlatformEnum.footsites)
                {
                    if (this._platform == TaskObject.PlatformEnum.mesh)
                    {
                        if (item.Link.ToLowerInvariant().Contains("size.co"))
                        {
                            item.VariousStringData = "size";
                        }
                        else if (!item.Link.ToLowerInvariant().Contains("hipstore"))
                        {
                            if (!item.Link.ToLowerInvariant().Contains("jdspo"))
                            {
                                item.VariousStringData = "footpatrol";
                            }
                            else
                            {
                                item.VariousStringData = "jd";
                            }
                        }
                        else
                        {
                            item.VariousStringData = "hipstore";
                        }
                    }
                }
                else if (!item.Link.ToLowerInvariant().Contains("footlocker.com"))
                {
                    if (!item.Link.ToLowerInvariant().Contains("footlocker.ca"))
                    {
                        if (item.Link.ToLowerInvariant().Contains("eastbay"))
                        {
                            item.FootSite = TaskObject.FootSitesEnum.eastbay;
                        }
                        else if (!item.Link.ToLowerInvariant().Contains("footaction"))
                        {
                            item.FootSite = TaskObject.FootSitesEnum.champssports;
                        }
                        else
                        {
                            item.FootSite = TaskObject.FootSitesEnum.footaction;
                        }
                    }
                    else
                    {
                        item.FootSite = TaskObject.FootSitesEnum.footlockerCa;
                    }
                }
                else
                {
                    item.FootSite = TaskObject.FootSitesEnum.footlocker;
                }
                if (item.Link.ToLowerInvariant().Contains("solebox"))
                {
                    if (!Global.SETTINGS.LOGIN_POOL.Any<LoginPool>(x => (x.Website == LoginPoolEnum.Solebox)))
                    {
                        MessageBox.Show("Solebox login pool empty", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                        return;
                    }
                    if (list.Count == 0)
                    {
                        foreach (LoginPool pool2 in from x in Global.SETTINGS.LOGIN_POOL
                            where x.Website == LoginPoolEnum.Solebox
                            select x)
                        {
                            list.Add(pool2.Link);
                        }
                    }
                    item.Login = true;
                    char[] separator = new char[] { ':' };
                    string[] strArray = list[num3].Split(separator);
                    item.Username = strArray[0].Trim();
                    item.Password = strArray[1].Trim();
                    num3++;
                    if (num3 == list.Count)
                    {
                        num3 = 0;
                    }
                }
                else if (item.Link.ToLowerInvariant().Contains("undefeated"))
                {
                    if (!Global.SETTINGS.LOGIN_POOL.Any<LoginPool>(x => (x.Website == LoginPoolEnum.Undefeated)))
                    {
                        MessageBox.Show("Undefeated login pool empty", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                        return;
                    }
                    if (list.Count == 0)
                    {
                        foreach (LoginPool pool in from x in Global.SETTINGS.LOGIN_POOL
                            where x.Website == LoginPoolEnum.Undefeated
                            select x)
                        {
                            list.Add(pool.Link);
                        }
                    }
                    item.Login = true;
                    char[] separator = new char[] { ':' };
                    string[] strArray3 = list[num3].Split(separator);
                    item.Username = strArray3[0].Trim();
                    item.Password = strArray3[1].Trim();
                    num3++;
                    if (num3 == list.Count)
                    {
                        num3 = 0;
                    }
                }
                else if (item.Link.ToLowerInvariant().Contains("titolo"))
                {
                    if (!Global.SETTINGS.LOGIN_POOL.Any<LoginPool>(x => (x.Website == LoginPoolEnum.Titolo)))
                    {
                        MessageBox.Show("Titolo login pool empty", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                        return;
                    }
                    if (list.Count == 0)
                    {
                        foreach (LoginPool pool3 in from x in Global.SETTINGS.LOGIN_POOL
                            where x.Website == LoginPoolEnum.Titolo
                            select x)
                        {
                            list.Add(pool3.Link);
                        }
                    }
                    item.Login = true;
                    char[] separator = new char[] { ':' };
                    string[] strArray2 = list[num3].Split(separator);
                    item.Username = strArray2[0].Trim();
                    item.Password = strArray2[1].Trim();
                    num3++;
                    if (num3 == list.Count)
                    {
                        num3 = 0;
                    }
                }
                else if (item.Link.ToLowerInvariant().Contains("holypopstore"))
                {
                    if (!Global.SETTINGS.LOGIN_POOL.Any<LoginPool>(x => (x.Website == LoginPoolEnum.Holypopstore)))
                    {
                        MessageBox.Show("Holypopstore login pool empty", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                        return;
                    }
                    if (list.Count == 0)
                    {
                        foreach (LoginPool pool4 in from x in Global.SETTINGS.LOGIN_POOL
                            where x.Website == LoginPoolEnum.Holypopstore
                            select x)
                        {
                            list.Add(pool4.Link);
                        }
                    }
                    item.Login = true;
                    char[] separator = new char[] { ':' };
                    string[] strArray4 = list[num3].Split(separator);
                    item.Username = strArray4[0].Trim();
                    item.Password = strArray4[1].Trim();
                    num3++;
                    if (num3 == list.Count)
                    {
                        num3 = 0;
                    }
                }
                if (Global.SETTINGS.PROFILES.Any<ProfileObject>(x => x.Id == Global.SETTINGS.QUICK_TASK.IdProfile))
                {
                    item.CheckoutId = Global.SETTINGS.QUICK_TASK.IdProfile;
                }
                else
                {
                    List<ProfileObject> list3 = (from x in Global.SETTINGS.PROFILES
                        where x.IdGroup == Global.SETTINGS.QUICK_TASK.IdProfile
                        select x).ToList<ProfileObject>();
                    item.CheckoutId = list3[num2].Id;
                    num2++;
                    if (num2 == list3.Count)
                    {
                        num2 = 0;
                    }
                }
                Global.SETTINGS.TASKS.Add(item);
                ids.Add(item.Id);
            }
            Helpers.SaveSettings();
            TaskManager.StartTasks(ids);
            base.Close();
        }

        private void chInherit_Checked(object sender, RoutedEventArgs e)
        {
            if (this.chInherit.IsChecked.HasValue && this.chInherit.IsChecked.Value)
            {
                this.txtSize.IsEnabled = false;
                this.txtSize.Opacity = 0.6;
                this.lblSize.IsEnabled = false;
                this.lblSize.Opacity = 0.6;
                this.chPickRandomNotAvailable.IsEnabled = false;
                this.chPickRandomNotAvailable.Opacity = 0.6;
                this.chSizeRandom.IsEnabled = false;
                this.chSizeRandom.Opacity = 0.6;
            }
            else
            {
                this.txtSize.IsEnabled = true;
                this.txtSize.Opacity = 1.0;
                this.lblSize.IsEnabled = true;
                this.lblSize.Opacity = 1.0;
                this.chPickRandomNotAvailable.IsEnabled = true;
                this.chPickRandomNotAvailable.Opacity = 1.0;
                this.chSizeRandom.IsEnabled = true;
                this.chSizeRandom.Opacity = 1.0;
            }
        }

        private void chSizeRandom_Checked(object sender, RoutedEventArgs e)
        {
            if (this.chSizeRandom != null)
            {
                if (this.chSizeRandom.IsChecked.HasValue && this.chSizeRandom.IsChecked.Value)
                {
                    this.txtSize.Text = "Random";
                    this.txtSize.IsEnabled = false;
                    this.txtSize.Opacity = 0.6;
                    this.chPickRandomNotAvailable.IsEnabled = false;
                    this.chPickRandomNotAvailable.Opacity = 0.6;
                }
                else
                {
                    this.txtSize.Text = "";
                    this.txtSize.IsEnabled = true;
                    this.txtSize.Opacity = 1.0;
                    this.chPickRandomNotAvailable.IsEnabled = true;
                    this.chPickRandomNotAvailable.Opacity = 1.0;
                }
            }
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.DragMove();
        }

        [GeneratedCode("PresentationBuildTasks", "4.0.0.0"), DebuggerNonUserCode]
        public void InitializeComponent()
        {
            // This item is obfuscated and can not be translated.
            if (!this._contentLoaded)
            {
                goto Label_002C;
            }
        Label_000D:
            switch (((-2071535609 ^ -1286555206) % 4))
            {
                case 0:
                    break;

                case 1:
                    return;

                case 2:
                    goto Label_000D;

                case 3:
                    return;

                default:
                    return;
            }
        Label_002C:
            this._contentLoaded = true;
            Uri resourceLocator = new Uri("/EveAIO;component/views/quicktaskwindow.xaml", UriKind.Relative);
            Application.LoadComponent(this, resourceLocator);
            goto Label_000D;
        }

        private void NumberCheck(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        private void QuickTaskCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (this.btnStart.IsEnabled)
            {
                this.btnStart_Click(null, null);
            }
        }

        [GeneratedCode("PresentationBuildTasks", "4.0.0.0"), DebuggerNonUserCode, EditorBrowsable(EditorBrowsableState.Never)]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.QuickTaskCommand_Executed);
                    return;

                case 2:
                    ((Grid) target).MouseLeftButtonDown += new MouseButtonEventHandler(this.Grid_MouseLeftButtonDown);
                    return;

                case 3:
                    this.BtnClose = (Button) target;
                    this.BtnClose.Click += new RoutedEventHandler(this.BtnClose_Click);
                    return;

                case 4:
                    this.txtLink = (TextBox) target;
                    this.txtLink.TextChanged += new TextChangedEventHandler(this.txtLink_TextChanged);
                    return;

                case 5:
                    this.lblLinkType = (TextBlock) target;
                    return;

                case 6:
                    this.txtQuantity = (TextBox) target;
                    this.txtQuantity.PreviewTextInput += new TextCompositionEventHandler(this.NumberCheck);
                    return;

                case 7:
                    this.gbSizing = (GroupBox) target;
                    return;

                case 8:
                    this.chInherit = (CheckBox) target;
                    this.chInherit.Checked += new RoutedEventHandler(this.chInherit_Checked);
                    this.chInherit.Unchecked += new RoutedEventHandler(this.chInherit_Checked);
                    return;

                case 9:
                    this.lblSize = (TextBlock) target;
                    return;

                case 10:
                    this.txtSize = (TextBox) target;
                    return;

                case 11:
                    this.chSizeRandom = (CheckBox) target;
                    this.chSizeRandom.Checked += new RoutedEventHandler(this.chSizeRandom_Checked);
                    this.chSizeRandom.Unchecked += new RoutedEventHandler(this.chSizeRandom_Checked);
                    return;

                case 12:
                    this.chPickRandomNotAvailable = (CheckBox) target;
                    return;

                case 13:
                    this.btnStart = (Button) target;
                    this.btnStart.Click += new RoutedEventHandler(this.btnStart_Click);
                    return;
            }
            this._contentLoaded = true;
        }

        private void txtLink_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.txtLink.Text.Contains("supreme"))
            {
                this.lblLinkType.Text = "Supreme (Quick task unsupported)";
                this.btnStart.IsEnabled = false;
                this.btnStart.Opacity = 0.6;
            }
            else if ((!string.IsNullOrEmpty(this.txtLink.Text.Trim()) && this.txtLink.Text.ToLowerInvariant().Contains("http")) && (this.txtLink.Text.Length > 8))
            {
                string str = this.txtLink.Text.ToLowerInvariant().Trim();
                if (!str.Contains("hibbett"))
                {
                    if (str.Contains("boxlunch"))
                    {
                        this._platform = TaskObject.PlatformEnum.boxlunch;
                        this.lblLinkType.Text = "boxlunch";
                    }
                    else if (!str.Contains("hottopic"))
                    {
                        if (str.Contains("woodwood"))
                        {
                            this._platform = TaskObject.PlatformEnum.woodwood;
                            this.lblLinkType.Text = "woodwood";
                        }
                        else if (!str.Contains("mcmwor"))
                        {
                            if (!str.Contains("solebox"))
                            {
                                if (str.Contains("nordstrom"))
                                {
                                    this._platform = TaskObject.PlatformEnum.nordstrom;
                                    this.lblLinkType.Text = "nordstrom";
                                }
                                else if (!str.Contains("mrporter"))
                                {
                                    if (str.Contains("back-door"))
                                    {
                                        this._platform = TaskObject.PlatformEnum.backdoor;
                                        this.lblLinkType.Text = "backdoor";
                                    }
                                    else if (str.Contains("titolo"))
                                    {
                                        this._platform = TaskObject.PlatformEnum.titolo;
                                        this.lblLinkType.Text = "titolo";
                                    }
                                    else if (!str.Contains("sneakersnstuff"))
                                    {
                                        if (!str.Contains("off---white"))
                                        {
                                            if (str.Contains("shop.funko"))
                                            {
                                                this._platform = TaskObject.PlatformEnum.funko;
                                                this.lblLinkType.Text = "funko";
                                            }
                                            else if (!str.Contains("puma."))
                                            {
                                                if (str.Contains("converse."))
                                                {
                                                    this._platform = TaskObject.PlatformEnum.converse;
                                                    this.lblLinkType.Text = "converse";
                                                }
                                                else if (!str.Contains("footlocker.com.au"))
                                                {
                                                    if (!str.Contains("footlocker.eu") && !str.Contains("footlocker.co.uk"))
                                                    {
                                                        if (str.Contains("finishline.com"))
                                                        {
                                                            this._platform = TaskObject.PlatformEnum.finishline;
                                                            this.lblLinkType.Text = "finishline";
                                                        }
                                                        else if ((!str.Contains("footlocker.ca") && !str.Contains("champsspo")) && !str.Contains("eastbay"))
                                                        {
                                                            if ((!str.Contains("size.c") && !str.Contains("hipstore")) && (!str.Contains("jdspo") && !str.Contains("footpatrol")))
                                                            {
                                                                if (str.Contains("footaction"))
                                                                {
                                                                    this._platform = TaskObject.PlatformEnum.footaction;
                                                                    this.lblLinkType.Text = "footaction";
                                                                }
                                                                else if (str.Contains("footlocker.com/"))
                                                                {
                                                                    this._platform = TaskObject.PlatformEnum.footaction;
                                                                    this.lblLinkType.Text = "footlocker";
                                                                }
                                                                else if (!str.Contains("holypop"))
                                                                {
                                                                    this._platform = TaskObject.PlatformEnum.shopify;
                                                                    this.lblLinkType.Text = "shopify";
                                                                }
                                                                else
                                                                {
                                                                    this._platform = TaskObject.PlatformEnum.holypopstore;
                                                                    this.lblLinkType.Text = "holypopstore";
                                                                }
                                                            }
                                                            else
                                                            {
                                                                this._platform = TaskObject.PlatformEnum.mesh;
                                                                this.lblLinkType.Text = "mesh";
                                                            }
                                                        }
                                                        else
                                                        {
                                                            this._platform = TaskObject.PlatformEnum.footsites;
                                                            this.lblLinkType.Text = "footsites";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        this._platform = TaskObject.PlatformEnum.footlockereu;
                                                        this.lblLinkType.Text = "fleu";
                                                    }
                                                }
                                                else
                                                {
                                                    this._platform = TaskObject.PlatformEnum.footlockerau;
                                                    this.lblLinkType.Text = "flau";
                                                }
                                            }
                                            else
                                            {
                                                this._platform = TaskObject.PlatformEnum.puma;
                                                this.lblLinkType.Text = "puma";
                                            }
                                        }
                                        else
                                        {
                                            this._platform = TaskObject.PlatformEnum.offwhite;
                                            this.lblLinkType.Text = "offwhite";
                                        }
                                    }
                                    else
                                    {
                                        this._platform = TaskObject.PlatformEnum.sneakersnstuff;
                                        this.lblLinkType.Text = "sneakersnstuff";
                                    }
                                }
                                else
                                {
                                    this._platform = TaskObject.PlatformEnum.mrporter;
                                    this.lblLinkType.Text = "mrporter";
                                }
                            }
                            else
                            {
                                this._platform = TaskObject.PlatformEnum.solebox;
                                this.lblLinkType.Text = "solebox";
                            }
                        }
                        else
                        {
                            this._platform = TaskObject.PlatformEnum.mcm;
                            this.lblLinkType.Text = "mcm";
                        }
                    }
                    else
                    {
                        this._platform = TaskObject.PlatformEnum.hottopic;
                        this.lblLinkType.Text = "hottopic";
                    }
                }
                else
                {
                    this._platform = TaskObject.PlatformEnum.hibbett;
                    this.lblLinkType.Text = "hibbet";
                }
                this.btnStart.IsEnabled = true;
                this.btnStart.Opacity = 1.0;
            }
            else
            {
                this.lblLinkType.Text = "unknown link";
                this.btnStart.IsEnabled = false;
                this.btnStart.Opacity = 0.6;
            }
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly QuickTaskWindow.<>c <>9;
            public static Func<ProfileGroupObject, bool> <>9__1_0;
            public static Func<ProfileGroupObject, bool> <>9__1_1;
            public static Func<LoginPool, bool> <>9__3_1;
            public static Func<LoginPool, bool> <>9__3_2;
            public static Func<LoginPool, bool> <>9__3_3;
            public static Func<LoginPool, bool> <>9__3_4;
            public static Func<LoginPool, bool> <>9__3_5;
            public static Func<LoginPool, bool> <>9__3_6;
            public static Func<LoginPool, bool> <>9__3_7;
            public static Func<LoginPool, bool> <>9__3_8;
            public static Func<ProfileObject, bool> <>9__3_0;
            public static Func<ProfileObject, bool> <>9__3_9;

            static <>c()
            {
                Class7.RIuqtBYzWxthF();
                <>9 = new QuickTaskWindow.<>c();
            }

            public <>c()
            {
                Class7.RIuqtBYzWxthF();
            }

            internal bool <.ctor>b__1_0(ProfileGroupObject x) => 
                (x.Id == Global.SETTINGS.QUICK_TASK.IdProfile);

            internal bool <.ctor>b__1_1(ProfileGroupObject x) => 
                (x.Id == Global.SETTINGS.QUICK_TASK.IdProfile);

            internal bool <btnStart_Click>b__3_0(ProfileObject x) => 
                (x.Id == Global.SETTINGS.QUICK_TASK.IdProfile);

            internal bool <btnStart_Click>b__3_1(LoginPool x) => 
                (x.Website == LoginPoolEnum.Solebox);

            internal bool <btnStart_Click>b__3_2(LoginPool x) => 
                (x.Website == LoginPoolEnum.Solebox);

            internal bool <btnStart_Click>b__3_3(LoginPool x) => 
                (x.Website == LoginPoolEnum.Undefeated);

            internal bool <btnStart_Click>b__3_4(LoginPool x) => 
                (x.Website == LoginPoolEnum.Undefeated);

            internal bool <btnStart_Click>b__3_5(LoginPool x) => 
                (x.Website == LoginPoolEnum.Titolo);

            internal bool <btnStart_Click>b__3_6(LoginPool x) => 
                (x.Website == LoginPoolEnum.Titolo);

            internal bool <btnStart_Click>b__3_7(LoginPool x) => 
                (x.Website == LoginPoolEnum.Holypopstore);

            internal bool <btnStart_Click>b__3_8(LoginPool x) => 
                (x.Website == LoginPoolEnum.Holypopstore);

            internal bool <btnStart_Click>b__3_9(ProfileObject x) => 
                (x.IdGroup == Global.SETTINGS.QUICK_TASK.IdProfile);
        }
    }
}

