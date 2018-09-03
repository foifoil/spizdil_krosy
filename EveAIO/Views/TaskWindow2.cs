namespace EveAIO.Views
{
    using EveAIO;
    using EveAIO.Pocos;
    using EveAIO.Views.Platforms;
    using System;
    using System.CodeDom.Compiler;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Markup;
    using System.Windows.Media;

    public class TaskWindow2 : Window, IComponentConnector
    {
        private Global.FormOperation _operation;
        internal TaskObject _task;
        private static Random _rnd;
        public bool RunAfterSave;
        public List<string> RunIds;
        internal PlatformView _platformView;
        internal GeneralTaskSettingsView _generalView;
        internal AdvancedTaskSettingsView _advancedView;
        private bool _platformsInit;
        internal ListBox lvMenu;
        internal Button BtnInfo;
        internal Button BtnClose;
        internal TextBox txtTaskName;
        internal ComboBox cmbPlatform;
        internal ComboBox cmbCheckoutProfile;
        internal ComboBox cmbProxyList;
        public System.Windows.Controls.Frame MenusFrame;
        internal Button btnSaveAndRun;
        internal Button btnSave;
        internal Button btnCancel;
        private bool _contentLoaded;

        static TaskWindow2()
        {
            Class7.RIuqtBYzWxthF();
            _rnd = new Random(DateTime.Now.Millisecond);
        }

        public TaskWindow2(Window owner)
        {
            Class7.RIuqtBYzWxthF();
            this.RunIds = new List<string>();
            this.InitializeComponent();
            base.Owner = owner;
            this._operation = Global.FormOperation.insert;
            TaskObject obj1 = new TaskObject {
                Id = Guid.NewGuid().ToString()
            };
            this._task = obj1;
        }

        public TaskWindow2(Window owner, TaskObject task)
        {
            Class7.RIuqtBYzWxthF();
            this.RunIds = new List<string>();
            this.InitializeComponent();
            base.Owner = owner;
            this._operation = Global.FormOperation.update;
            this._task = task;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            base.Close();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            base.Close();
        }

        private void BtnInfo_Click(object sender, RoutedEventArgs e)
        {
            switch (this._platformView.content.Content)
            {
                case (BoxlunchControl2 _):
                    Process.Start("https://eve-robotics.com/knowledge-base/boxlunch/");
                    break;

                case (ConverseControl2 _):
                    Process.Start("https://eve-robotics.com/knowledge-base/converse/");
                    break;

                case (FootlockerauControl2 _):
                    Process.Start("https://eve-robotics.com/knowledge-base/footlockerau/");
                    break;

                case (FunkoControl2 _):
                    Process.Start("https://eve-robotics.com/knowledge-base/funko/");
                    break;

                case (McmControl2 _):
                    Process.Start("https://eve-robotics.com/knowledge-base/mcm/");
                    break;

                case (MrPorterControl2 _):
                    Process.Start("https://eve-robotics.com/knowledge-base/mrporter/");
                    break;

                case (PumaControl2 _):
                    Process.Start("https://eve-robotics.com/knowledge-base/puma/");
                    break;

                case (SoleboxControl2 _):
                    Process.Start("https://eve-robotics.com/knowledge-base/solebox/");
                    break;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!this.Check())
            {
                return;
            }
            int num19 = 1;
            if ((this._operation == Global.FormOperation.insert) && !string.IsNullOrEmpty(this._advancedView.txtMassCreate.Text.Trim()))
            {
                num19 = int.Parse(this._advancedView.txtMassCreate.Text.Trim());
            }
            int num23 = 0;
            int num8 = 0;
            List<string> list = new List<string>();
            bool flag = !Global.SETTINGS.PROFILES.Any<ProfileObject>(x => (x.Id == ((ComboBoxItem) this.cmbCheckoutProfile.SelectedItem).Tag.ToString()));
            int num21 = 1;
        Label_007E:
            if (num21 > num19)
            {
                Helpers.SaveSettings();
                base.Close();
                return;
            }
            if (this._operation == Global.FormOperation.insert)
            {
                this._task = new TaskObject();
                this._task.Id = Guid.NewGuid().ToString();
                this._task.Guid = Guid.NewGuid().ToString();
            }
            this._task.Name = this.txtTaskName.Text.Trim();
            if (num19 > 1)
            {
                this._task.Name = this._task.Name + " " + num21.ToString();
            }
            this._task.Size = this._generalView.txtSize.Text.Trim();
            this._task.RandomSize = !this._generalView.chSizeRandom.IsChecked.HasValue ? false : this._generalView.chSizeRandom.IsChecked.Value;
            this._task.SizePickRandom = !this._generalView.chPickRandomNotAvailable.IsChecked.HasValue ? false : this._generalView.chPickRandomNotAvailable.IsChecked.Value;
            if ((this.cmbCheckoutProfile.SelectedItem != null) && (((ComboBoxItem) this.cmbCheckoutProfile.SelectedItem).Tag.ToString() != "-1"))
            {
                if (!Global.SETTINGS.PROFILES.Any<ProfileObject>(x => (x.Id == ((ComboBoxItem) this.cmbCheckoutProfile.SelectedItem).Tag.ToString())))
                {
                    List<ProfileObject> list3 = (from x in Global.SETTINGS.PROFILES
                        where x.IdGroup == ((ComboBoxItem) this.cmbCheckoutProfile.SelectedItem).Tag.ToString()
                        select x).ToList<ProfileObject>();
                    this._task.CheckoutId = list3[num23].Id;
                    num23++;
                    if (num23 == list3.Count)
                    {
                        num23 = 0;
                    }
                }
                else
                {
                    this._task.CheckoutId = ((ComboBoxItem) this.cmbCheckoutProfile.SelectedItem).Tag.ToString();
                }
            }
            this._task.IsNegativeSizing = !this._generalView.chSizeNegative.IsChecked.HasValue ? false : this._generalView.chSizeNegative.IsChecked.Value;
            this._task.ProxyListId = ((this.cmbProxyList.SelectedItem == null) || (((ComboBoxItem) this.cmbProxyList.SelectedItem).Tag.ToString() == "-1")) ? "" : ((ComboBoxItem) this.cmbProxyList.SelectedItem).Tag.ToString();
            this._task.RetryOnError = !this._generalView.chRetryOnError.IsChecked.HasValue ? false : this._generalView.chRetryOnError.IsChecked.Value;
            this._task.Notify = !this._generalView.chNotify.IsChecked.HasValue ? false : this._generalView.chNotify.IsChecked.Value;
            this._task.PaypalProxyIgnore = !this._advancedView.chPaypalProxyIgnore.IsChecked.HasValue ? false : this._advancedView.chPaypalProxyIgnore.IsChecked.Value;
            this._task.PriceCheck = !this._generalView.switchPriceCheck.IsChecked.HasValue ? false : this._generalView.switchPriceCheck.IsChecked.Value;
            this._task.MinimumPrice = string.IsNullOrEmpty(this._generalView.txtPriceCheckMin.Text.Trim()) ? 0 : int.Parse(this._generalView.txtPriceCheckMin.Text.Trim());
            this._task.MaximumPrice = string.IsNullOrEmpty(this._generalView.txtPriceCheckMax.Text.Trim()) ? 0 : int.Parse(this._generalView.txtPriceCheckMax.Text.Trim());
            this._task.CaptchaRequests = string.IsNullOrEmpty(this._generalView.txtCaptchaRequests.Text.Trim()) ? 0 : int.Parse(this._generalView.txtCaptchaRequests.Text.Trim());
            this._task.CheckoutDelay = string.IsNullOrEmpty(this._generalView.txtCheckoutDelay.Text.Trim()) ? 0 : int.Parse(this._generalView.txtCheckoutDelay.Text.Trim());
            if ((((this._task.Delay != this._generalView.txtDelay.Text.Trim()) || (this._task.DelayFrom != this._generalView.txtDelayFrom.Text.Trim())) || (this._task.DelayTo != this._generalView.txtDelayTo.Text.Trim())) || (this._task.RetryDelay != ((!this._generalView.chDelayExact.IsChecked.HasValue || !this._generalView.chDelayExact.IsChecked.Value) ? TaskObject.RetryDelayEnum.random : TaskObject.RetryDelayEnum.exact)))
            {
                this._task.DelayChanged = true;
            }
            this._task.RetryDelay = (!this._generalView.chDelayExact.IsChecked.HasValue || !this._generalView.chDelayExact.IsChecked.Value) ? TaskObject.RetryDelayEnum.random : TaskObject.RetryDelayEnum.exact;
            this._task.Delay = this._generalView.txtDelay.Text.Trim();
            this._task.DelayFrom = this._generalView.txtDelayFrom.Text.Trim();
            this._task.DelayTo = this._generalView.txtDelayTo.Text.Trim();
            try
            {
                if (int.Parse(this._task.DelayTo) < int.Parse(this._task.DelayFrom))
                {
                    this._task.DelayTo = this._task.DelayFrom;
                }
            }
            catch
            {
            }
            this._task.SpecificCaptcha = !this._advancedView.switchAdvancedCaptcha.IsChecked.HasValue ? false : this._advancedView.switchAdvancedCaptcha.IsChecked.Value;
            this._task.TwoCaptchaRequests = string.IsNullOrEmpty(this._advancedView.textBox_0.Text.Trim()) ? 0 : int.Parse(this._advancedView.textBox_0.Text.Trim());
            this._task.AnticaptchaRequests = string.IsNullOrEmpty(this._advancedView.txtAntiCaptcha.Text.Trim()) ? 0 : int.Parse(this._advancedView.txtAntiCaptcha.Text.Trim());
            this._task.ImagetypersRequests = string.IsNullOrEmpty(this._advancedView.txtImageTypers.Text.Trim()) ? 0 : int.Parse(this._advancedView.txtImageTypers.Text.Trim());
            this._task.DisolveRequests = string.IsNullOrEmpty(this._advancedView.txtDisolve.Text.Trim()) ? 0 : int.Parse(this._advancedView.txtDisolve.Text.Trim());
            this._task.LocalIPCheckout = !this._advancedView.chLocalIPCheckout.IsChecked.HasValue ? false : this._advancedView.chLocalIPCheckout.IsChecked.Value;
            this._task.IsShopifyCheckoutLink = false;
            this._task.ShopifyCheckoutLink = "";
            this._task.WatchTaskId = ((this._advancedView.cmbWatchTask.SelectedItem == null) || (((ComboBoxItem) this._advancedView.cmbWatchTask.SelectedItem).Tag.ToString() == "-1")) ? "" : ((ComboBoxItem) this._advancedView.cmbWatchTask.SelectedItem).Tag.ToString();
            string s = ((ComboBoxItem) this.cmbPlatform.SelectedItem).Tag.ToString();
            uint num3 = <PrivateImplementationDetails>.ComputeStringHash(s);
            if (num3 > 0x9b115325)
            {
                if (num3 > 0xd25e7a39)
                {
                    if (num3 > 0xe6761580)
                    {
                        if (num3 > 0xf5825582)
                        {
                            if (num3 == 0xf8746c31)
                            {
                                if (s == "shopify")
                                {
                                    ShopifyControl2 content = (ShopifyControl2) this._platformView.content.Content;
                                    this._task.Platform = TaskObject.PlatformEnum.shopify;
                                    this._task.ParentId = ((content.cmbParentTask.SelectedItem == null) || (((ComboBoxItem) content.cmbParentTask.SelectedItem).Tag.ToString() == "-1")) ? "" : ((ComboBoxItem) content.cmbParentTask.SelectedItem).Tag.ToString();
                                    this._task.Login = !content.switchLogin.IsChecked.HasValue ? false : content.switchLogin.IsChecked.Value;
                                    this._task.Username = content.txtUsername.Text.Trim();
                                    this._task.Password = content.txtPassword.Text.Trim();
                                    this._task.Last25Products = !content.switchLast25.IsChecked.HasValue ? false : content.switchLast25.IsChecked.Value;
                                    this._task.AtcMethod = TaskObject.AtcMethodEnum.frontend;
                                    this._task.ShopifyIsOldMode = !content.switchOldMode.IsChecked.HasValue ? false : content.switchOldMode.IsChecked.Value;
                                    try
                                    {
                                        this._task.Quantity = int.Parse(content.txtQuantity.Text.Trim());
                                    }
                                    catch
                                    {
                                        this._task.Quantity = 1;
                                    }
                                    if (this._task.Quantity < 1)
                                    {
                                        this._task.Quantity = 1;
                                    }
                                    if (string.IsNullOrEmpty(this._task.ParentId))
                                    {
                                        if (this._task.State == TaskObject.StateEnum.multicart)
                                        {
                                            this._task.State = TaskObject.StateEnum.stopped;
                                        }
                                    }
                                    else
                                    {
                                        this._task.State = TaskObject.StateEnum.multicart;
                                    }
                                    if (content.radioButton_0.IsChecked.HasValue && content.radioButton_0.IsChecked.Value)
                                    {
                                        this._task.Payment = TaskObject.PaymentEnum.creditcard;
                                    }
                                    else
                                    {
                                        this._task.Payment = TaskObject.PaymentEnum.paypal;
                                    }
                                    this._task.Link = content.txtLink.Text.Trim();
                                    if (this._task.Link[this._task.Link.Length - 1] == '/')
                                    {
                                        this._task.Link = this._task.Link.Substring(0, this._task.Link.LastIndexOf("/"));
                                    }
                                    this._task.Variant = content.txtVariant.Text.Trim();
                                    this._task.ShopifyWebsite = ((ComboBoxItem) content.cmbWebsite.SelectedItem).Tag.ToString();
                                    this._task.DeepSearch = !content.switchDeepSearch.IsChecked.HasValue ? false : content.switchDeepSearch.IsChecked.Value;
                                    try
                                    {
                                        this._task.DeepSearchLinks = short.Parse(content.txtDeepSearchLinks.Text.Trim());
                                    }
                                    catch
                                    {
                                        this._task.DeepSearchLinks = 5;
                                    }
                                    this._task.Keywords.Clear();
                                    this._task.PositiveKeywordsChange();
                                    for (int i = 0; i < content.txtPositiveKws.LineCount; i++)
                                    {
                                        string lineText = content.txtPositiveKws.GetLineText(i);
                                        if (!string.IsNullOrEmpty(lineText) && (lineText != "\n"))
                                        {
                                            this._task.Keywords.Add(lineText);
                                        }
                                    }
                                    this._task.NegativeKeywords.Clear();
                                    this._task.NegativeKeywordsChange();
                                    for (int j = 0; j < content.txtNegativeKws.LineCount; j++)
                                    {
                                        string lineText = content.txtNegativeKws.GetLineText(j);
                                        if (!string.IsNullOrEmpty(lineText) && (lineText != "\n"))
                                        {
                                            this._task.NegativeKeywords.Add(lineText);
                                        }
                                    }
                                    if (content.rTypeDirect.IsChecked.HasValue && content.rTypeDirect.IsChecked.Value)
                                    {
                                        this._task.TaskType = TaskObject.TaskTypeEnum.directlink;
                                    }
                                    else if (content.rTypeKeywords.IsChecked.HasValue && content.rTypeKeywords.IsChecked.Value)
                                    {
                                        this._task.TaskType = TaskObject.TaskTypeEnum.keywords;
                                    }
                                    else
                                    {
                                        this._task.TaskType = TaskObject.TaskTypeEnum.variant;
                                    }
                                    if ((content.cmbCheckoutlink.SelectedItem != null) && (((ComboBoxItem) content.cmbCheckoutlink.SelectedItem).Tag.ToString() != "-"))
                                    {
                                        List<string> tag = (List<string>) ((ComboBoxItem) content.cmbCheckoutlink.SelectedItem).Tag;
                                        this._task.IsShopifyCheckoutLink = true;
                                        this._task.ShopifyCheckoutLink = tag[_rnd.Next(0, tag.Count)];
                                    }
                                    else
                                    {
                                        this._task.IsShopifyCheckoutLink = false;
                                        this._task.ShopifyCheckoutLink = "";
                                    }
                                    this._task.Color = content.txtColor.Text.Trim();
                                    this._task.ColorPickRandom = !content.chPickRandomColorNotAvailable.IsChecked.HasValue ? false : content.chPickRandomColorNotAvailable.IsChecked.Value;
                                    if (content.rColorExact.IsChecked.HasValue && content.rColorExact.IsChecked.Value)
                                    {
                                        this._task.SupremeColorPick = TaskObject.SuprimeColorPickEnum.exact;
                                    }
                                    else
                                    {
                                        this._task.SupremeColorPick = TaskObject.SuprimeColorPickEnum.contains;
                                    }
                                }
                            }
                            else if ((num3 == 0xf9f4459c) && (s == "supremeinstore"))
                            {
                                SupremeInstoreControl2 content = (SupremeInstoreControl2) this._platformView.content.Content;
                                this._task.Platform = TaskObject.PlatformEnum.supremeinstore;
                                this._task.Login = false;
                                this._task.TaskType = TaskObject.TaskTypeEnum.directlink;
                                this._task.Payment = TaskObject.PaymentEnum.creditcard;
                                this._task.SupremeRegion = (!content.rRegionEu.IsChecked.HasValue || !content.rRegionEu.IsChecked.Value) ? TaskObject.SupremeRegionEnum.USA : TaskObject.SupremeRegionEnum.EU;
                                this._task.Link = (this._task.SupremeRegion == TaskObject.SupremeRegionEnum.EU) ? "https://london.supremenewyork.com" : "https://register.supremenewyork.com";
                                if (content.rLocBrooklyn.IsChecked.HasValue && content.rLocBrooklyn.IsChecked.Value)
                                {
                                    this._task.VariousStringData = "brooklyn";
                                }
                                else if (content.rLocManhattan.IsChecked.HasValue && content.rLocManhattan.IsChecked.Value)
                                {
                                    this._task.VariousStringData = "manhattan";
                                }
                                else
                                {
                                    this._task.VariousStringData = "la";
                                }
                            }
                        }
                        else if (num3 != 0xf2b3ba92)
                        {
                            if ((num3 == 0xf5825582) && (s == "boxlunch"))
                            {
                                BoxlunchControl2 content = (BoxlunchControl2) this._platformView.content.Content;
                                this._task.Platform = TaskObject.PlatformEnum.boxlunch;
                                this._task.Login = false;
                                this._task.Link = content.txtLink.Text.Trim();
                                this._task.Keywords.Clear();
                                this._task.PositiveKeywordsChange();
                                for (int i = 0; i < content.txtPositiveKws.LineCount; i++)
                                {
                                    string lineText = content.txtPositiveKws.GetLineText(i);
                                    if (!string.IsNullOrEmpty(lineText) && (lineText != "\n"))
                                    {
                                        this._task.Keywords.Add(lineText);
                                    }
                                }
                                this._task.NegativeKeywords.Clear();
                                this._task.NegativeKeywordsChange();
                                if (!string.IsNullOrEmpty(content.txtNegativeKeywords.Text.Trim()))
                                {
                                    this._task.NegativeKeywords.Add(content.txtNegativeKeywords.Text.Trim());
                                }
                                if (content.rTypeDirect.IsChecked.HasValue && content.rTypeDirect.IsChecked.Value)
                                {
                                    this._task.TaskType = TaskObject.TaskTypeEnum.directlink;
                                }
                                else
                                {
                                    this._task.TaskType = TaskObject.TaskTypeEnum.keywords;
                                }
                                this._task.VariousStringData = content.txtCouponCode.Text.Trim();
                                try
                                {
                                    int num20 = int.Parse(content.txtQuantity.Text);
                                    if ((num20 > 0) && (num20 <= 10))
                                    {
                                        this._task.VariousStringData2 = num20.ToString();
                                    }
                                    else
                                    {
                                        this._task.VariousStringData2 = "1";
                                    }
                                }
                                catch
                                {
                                    this._task.VariousStringData2 = "1";
                                }
                                this._task.Login = !content.switchLogin.IsChecked.HasValue ? false : content.switchLogin.IsChecked.Value;
                                this._task.Username = content.txtUsername.Text.Trim();
                                this._task.Password = content.txtPassword.Text.Trim();
                            }
                        }
                        else if (s == "mrporter")
                        {
                            MrPorterControl2 content = (MrPorterControl2) this._platformView.content.Content;
                            this._task.Platform = TaskObject.PlatformEnum.mrporter;
                            this._task.Link = content.txtLink.Text.Trim();
                            this._task.Login = !content.switchLogin.IsChecked.HasValue ? false : content.switchLogin.IsChecked.Value;
                            this._task.Username = content.txtUsername.Text.Trim();
                            this._task.Password = content.txtPassword.Text.Trim();
                            this._task.TaskType = TaskObject.TaskTypeEnum.directlink;
                            this._task.Payment = TaskObject.PaymentEnum.creditcard;
                            if (content.rRegionUsa.IsChecked.HasValue && content.rRegionUsa.IsChecked.Value)
                            {
                                this._task.SupremeRegion = TaskObject.SupremeRegionEnum.USA;
                            }
                            else
                            {
                                this._task.SupremeRegion = TaskObject.SupremeRegionEnum.EU;
                            }
                        }
                    }
                    else
                    {
                        switch (num3)
                        {
                            case 0xdffd2e3b:
                                if (s == "hottopic")
                                {
                                    HottopicControl content = (HottopicControl) this._platformView.content.Content;
                                    this._task.Platform = TaskObject.PlatformEnum.hottopic;
                                    this._task.Login = false;
                                    this._task.Link = content.txtLink.Text.Trim();
                                    this._task.Keywords.Clear();
                                    this._task.PositiveKeywordsChange();
                                    for (int i = 0; i < content.txtPositiveKws.LineCount; i++)
                                    {
                                        string lineText = content.txtPositiveKws.GetLineText(i);
                                        if (!string.IsNullOrEmpty(lineText) && (lineText != "\n"))
                                        {
                                            this._task.Keywords.Add(lineText);
                                        }
                                    }
                                    this._task.NegativeKeywords.Clear();
                                    this._task.NegativeKeywordsChange();
                                    if (!string.IsNullOrEmpty(content.txtNegativeKeywords.Text.Trim()))
                                    {
                                        this._task.NegativeKeywords.Add(content.txtNegativeKeywords.Text.Trim());
                                    }
                                    if (content.rTypeDirect.IsChecked.HasValue && content.rTypeDirect.IsChecked.Value)
                                    {
                                        this._task.TaskType = TaskObject.TaskTypeEnum.directlink;
                                    }
                                    else
                                    {
                                        this._task.TaskType = TaskObject.TaskTypeEnum.keywords;
                                    }
                                    this._task.VariousStringData = content.txtCouponCode.Text.Trim();
                                    try
                                    {
                                        int num13 = int.Parse(content.txtQuantity.Text);
                                        if ((num13 > 0) && (num13 <= 10))
                                        {
                                            this._task.VariousStringData2 = num13.ToString();
                                        }
                                        else
                                        {
                                            this._task.VariousStringData2 = "1";
                                        }
                                    }
                                    catch
                                    {
                                        this._task.VariousStringData2 = "1";
                                    }
                                    this._task.Login = !content.switchLogin.IsChecked.HasValue ? false : content.switchLogin.IsChecked.Value;
                                    this._task.Username = content.txtUsername.Text.Trim();
                                    this._task.Password = content.txtPassword.Text.Trim();
                                }
                                goto Label_3AE9;

                            case 0xe12329de:
                                if (s == "backdoor")
                                {
                                    BackdoorControl2 content = (BackdoorControl2) this._platformView.content.Content;
                                    this._task.Platform = TaskObject.PlatformEnum.backdoor;
                                    this._task.Link = content.txtLink.Text.Trim();
                                    this._task.Login = false;
                                    this._task.Keywords.Clear();
                                    this._task.PositiveKeywordsChange();
                                    for (int i = 0; i < content.txtPositiveKws.LineCount; i++)
                                    {
                                        string lineText = content.txtPositiveKws.GetLineText(i);
                                        if (!string.IsNullOrEmpty(lineText) && (lineText != "\n"))
                                        {
                                            this._task.Keywords.Add(lineText);
                                        }
                                    }
                                    this._task.NegativeKeywords.Clear();
                                    this._task.NegativeKeywordsChange();
                                    if (content.rTypeDirect.IsChecked.HasValue && content.rTypeDirect.IsChecked.Value)
                                    {
                                        this._task.TaskType = TaskObject.TaskTypeEnum.directlink;
                                    }
                                    else
                                    {
                                        this._task.TaskType = TaskObject.TaskTypeEnum.keywords;
                                    }
                                    this._task.Payment = TaskObject.PaymentEnum.creditcard;
                                }
                                goto Label_3AE9;
                        }
                        if ((num3 == 0xe6761580) && (s == "finishline"))
                        {
                            FinishlineControl content = (FinishlineControl) this._platformView.content.Content;
                            this._task.Platform = TaskObject.PlatformEnum.finishline;
                            this._task.Link = content.txtLink.Text.Trim();
                            this._task.Keywords.Clear();
                            this._task.PositiveKeywordsChange();
                            for (int i = 0; i < content.txtPositiveKws.LineCount; i++)
                            {
                                string lineText = content.txtPositiveKws.GetLineText(i);
                                if (!string.IsNullOrEmpty(lineText) && (lineText != "\n"))
                                {
                                    this._task.Keywords.Add(lineText);
                                }
                            }
                            this._task.NegativeKeywords.Clear();
                            this._task.NegativeKeywordsChange();
                            if (content.rTypeDirect.IsChecked.HasValue && content.rTypeDirect.IsChecked.Value)
                            {
                                this._task.TaskType = TaskObject.TaskTypeEnum.directlink;
                            }
                            else
                            {
                                this._task.TaskType = TaskObject.TaskTypeEnum.keywords;
                            }
                            this._task.Color = content.txtColor.Text.Trim();
                            this._task.ColorPickRandom = !content.chPickRandomColorNotAvailable.IsChecked.HasValue ? false : content.chPickRandomColorNotAvailable.IsChecked.Value;
                            if (content.rColorExact.IsChecked.HasValue && content.rColorExact.IsChecked.Value)
                            {
                                this._task.SupremeColorPick = TaskObject.SuprimeColorPickEnum.exact;
                            }
                            else
                            {
                                this._task.SupremeColorPick = TaskObject.SuprimeColorPickEnum.contains;
                            }
                        }
                    }
                }
                else if (num3 > 0xb98c68cf)
                {
                    if (num3 > 0xc6a8d884)
                    {
                        if (num3 == 0xc9957c5b)
                        {
                            if (s == "footlockereu")
                            {
                                FootlockereuControl content = (FootlockereuControl) this._platformView.content.Content;
                                this._task.Platform = TaskObject.PlatformEnum.footlockereu;
                                this._task.Link = content.txtLink.Text.Trim();
                                this._task.Keywords.Clear();
                                this._task.PositiveKeywordsChange();
                                for (int i = 0; i < content.txtPositiveKws.LineCount; i++)
                                {
                                    string lineText = content.txtPositiveKws.GetLineText(i);
                                    if (!string.IsNullOrEmpty(lineText) && (lineText != "\n"))
                                    {
                                        this._task.Keywords.Add(lineText);
                                    }
                                }
                                this._task.NegativeKeywords.Clear();
                                this._task.NegativeKeywordsChange();
                                if (content.rTypeDirect.IsChecked.HasValue && content.rTypeDirect.IsChecked.Value)
                                {
                                    this._task.TaskType = TaskObject.TaskTypeEnum.directlink;
                                }
                                else
                                {
                                    this._task.TaskType = TaskObject.TaskTypeEnum.keywords;
                                }
                                this._task.Login = false;
                                if (content.radioButton_0.IsChecked.HasValue && content.radioButton_0.IsChecked.Value)
                                {
                                    this._task.Payment = TaskObject.PaymentEnum.creditcard;
                                }
                                else
                                {
                                    this._task.Payment = TaskObject.PaymentEnum.paypal;
                                }
                                this._task.Various = ((ComboBoxItem) content.cmbRegion.SelectedItem).Tag.ToString();
                            }
                        }
                        else if ((num3 == 0xd25e7a39) && (s == "sneakersnstuff"))
                        {
                            SneakernstuffControl2 content = (SneakernstuffControl2) this._platformView.content.Content;
                            this._task.Platform = TaskObject.PlatformEnum.sneakersnstuff;
                            this._task.Login = !content.switchLogin.IsChecked.HasValue ? false : content.switchLogin.IsChecked.Value;
                            this._task.Username = content.txtUsername.Text.Trim();
                            this._task.Password = content.txtPassword.Text.Trim();
                            this._task.Link = content.txtLink.Text.Trim();
                            this._task.Keywords.Clear();
                            this._task.PositiveKeywordsChange();
                            for (int i = 0; i < content.txtPositiveKws.LineCount; i++)
                            {
                                string lineText = content.txtPositiveKws.GetLineText(i);
                                if (!string.IsNullOrEmpty(lineText) && (lineText != "\n"))
                                {
                                    this._task.Keywords.Add(lineText);
                                }
                            }
                            this._task.NegativeKeywords.Clear();
                            this._task.NegativeKeywordsChange();
                            if (content.rTypeDirect.IsChecked.HasValue && content.rTypeDirect.IsChecked.Value)
                            {
                                this._task.TaskType = TaskObject.TaskTypeEnum.directlink;
                            }
                            else
                            {
                                this._task.TaskType = TaskObject.TaskTypeEnum.keywords;
                            }
                            if (content.rColorExact.IsChecked.HasValue && content.rColorExact.IsChecked.Value)
                            {
                                this._task.SupremeColorPick = TaskObject.SuprimeColorPickEnum.exact;
                            }
                            else
                            {
                                this._task.SupremeColorPick = TaskObject.SuprimeColorPickEnum.contains;
                            }
                            this._task.Color = content.txtColor.Text.Trim();
                        }
                    }
                    else if (num3 != 0xbdeaf8c8)
                    {
                        if ((num3 == 0xc6a8d884) && (s == "mcm"))
                        {
                            McmControl2 content = (McmControl2) this._platformView.content.Content;
                            this._task.Platform = TaskObject.PlatformEnum.mcm;
                            this._task.Link = content.txtLink.Text.Trim();
                            this._task.Login = false;
                            this._task.TaskType = TaskObject.TaskTypeEnum.directlink;
                            this._task.Payment = TaskObject.PaymentEnum.creditcard;
                            this._task.Color = content.txtColor.Text.Trim();
                            this._task.ColorPickRandom = !content.chPickRandomColorNotAvailable.IsChecked.HasValue ? false : content.chPickRandomColorNotAvailable.IsChecked.Value;
                            if (content.rColorContains.IsChecked.HasValue && content.rColorContains.IsChecked.Value)
                            {
                                this._task.SupremeColorPick = TaskObject.SuprimeColorPickEnum.contains;
                            }
                            else
                            {
                                this._task.SupremeColorPick = TaskObject.SuprimeColorPickEnum.exact;
                            }
                            this._task.Keywords.Clear();
                            this._task.PositiveKeywordsChange();
                            for (int i = 0; i < content.txtPositiveKws.LineCount; i++)
                            {
                                string lineText = content.txtPositiveKws.GetLineText(i);
                                if (!string.IsNullOrEmpty(lineText) && (lineText != "\n"))
                                {
                                    this._task.Keywords.Add(lineText);
                                }
                            }
                            this._task.NegativeKeywords.Clear();
                            if (content.rTypeDirect.IsChecked.HasValue && content.rTypeDirect.IsChecked.Value)
                            {
                                this._task.TaskType = TaskObject.TaskTypeEnum.directlink;
                            }
                            else
                            {
                                this._task.TaskType = TaskObject.TaskTypeEnum.keywords;
                            }
                        }
                    }
                    else if (s == "converse")
                    {
                        ConverseControl2 content = (ConverseControl2) this._platformView.content.Content;
                        this._task.Platform = TaskObject.PlatformEnum.converse;
                        this._task.Login = false;
                        this._task.Payment = TaskObject.PaymentEnum.creditcard;
                        this._task.Color = content.txtColor.Text.Trim();
                        this._task.ColorPickRandom = !content.chPickRandomColorNotAvailable.IsChecked.HasValue ? false : content.chPickRandomColorNotAvailable.IsChecked.Value;
                        if (content.rColorContains.IsChecked.HasValue && content.rColorContains.IsChecked.Value)
                        {
                            this._task.SupremeColorPick = TaskObject.SuprimeColorPickEnum.contains;
                        }
                        else
                        {
                            this._task.SupremeColorPick = TaskObject.SuprimeColorPickEnum.exact;
                        }
                        this._task.Link = content.txtLink.Text.Trim();
                        this._task.Keywords.Clear();
                        this._task.PositiveKeywordsChange();
                        for (int i = 0; i < content.txtPositiveKws.LineCount; i++)
                        {
                            string lineText = content.txtPositiveKws.GetLineText(i);
                            if (!string.IsNullOrEmpty(lineText) && (lineText != "\n"))
                            {
                                this._task.Keywords.Add(lineText);
                            }
                        }
                        this._task.NegativeKeywords.Clear();
                        this._task.NegativeKeywordsChange();
                        if (content.rTypeDirect.IsChecked.HasValue && content.rTypeDirect.IsChecked.Value)
                        {
                            this._task.TaskType = TaskObject.TaskTypeEnum.directlink;
                        }
                        else
                        {
                            this._task.TaskType = TaskObject.TaskTypeEnum.keywords;
                        }
                    }
                }
                else if (num3 == 0xa100bebc)
                {
                    if (s == "mesh")
                    {
                        MeshControl2 content = (MeshControl2) this._platformView.content.Content;
                        this._task.Platform = TaskObject.PlatformEnum.mesh;
                        this._task.Login = false;
                        this._task.TaskType = TaskObject.TaskTypeEnum.directlink;
                        this._task.Payment = TaskObject.PaymentEnum.creditcard;
                        this._task.Link = content.txtLink.Text.Trim();
                        if (content.rAtcBackend.IsChecked.HasValue && content.rAtcBackend.IsChecked.Value)
                        {
                            this._task.AtcMethod = TaskObject.AtcMethodEnum.backend;
                        }
                        else
                        {
                            this._task.AtcMethod = TaskObject.AtcMethodEnum.frontend;
                        }
                        if (content.rMeshSize.IsChecked.HasValue && content.rMeshSize.IsChecked.Value)
                        {
                            this._task.VariousStringData = "size";
                        }
                        else if (content.rMeshHipStore.IsChecked.HasValue && content.rMeshHipStore.IsChecked.Value)
                        {
                            this._task.VariousStringData = "hipstore";
                        }
                        else if (content.rMeshJd.IsChecked.HasValue && content.rMeshJd.IsChecked.Value)
                        {
                            this._task.VariousStringData = "jd";
                        }
                        else
                        {
                            this._task.VariousStringData = "footpatrol";
                        }
                    }
                }
                else if (num3 != 0xa22f8308)
                {
                    if ((num3 == 0xb98c68cf) && (s == "footlockerau"))
                    {
                        FootlockerauControl2 content = (FootlockerauControl2) this._platformView.content.Content;
                        this._task.Platform = TaskObject.PlatformEnum.footlockerau;
                        this._task.Link = content.txtLink.Text.Trim();
                        this._task.Login = false;
                        this._task.TaskType = TaskObject.TaskTypeEnum.directlink;
                        this._task.Payment = TaskObject.PaymentEnum.creditcard;
                        if (content.radioButton_0.IsChecked.HasValue && content.radioButton_0.IsChecked.Value)
                        {
                            this._task.Payment = TaskObject.PaymentEnum.creditcard;
                        }
                        else
                        {
                            this._task.Payment = TaskObject.PaymentEnum.paypal;
                        }
                    }
                }
                else if (s == "puma")
                {
                    PumaControl2 content = (PumaControl2) this._platformView.content.Content;
                    this._task.Platform = TaskObject.PlatformEnum.puma;
                    this._task.Link = content.txtLink.Text.Trim();
                    this._task.Login = false;
                    this._task.TaskType = TaskObject.TaskTypeEnum.directlink;
                    this._task.Payment = TaskObject.PaymentEnum.creditcard;
                    this._task.Color = content.txtColor.Text.Trim();
                    this._task.ColorPickRandom = !content.chPickRandomColorNotAvailable.IsChecked.HasValue ? false : content.chPickRandomColorNotAvailable.IsChecked.Value;
                    if (content.rColorContains.IsChecked.HasValue && content.rColorContains.IsChecked.Value)
                    {
                        this._task.SupremeColorPick = TaskObject.SuprimeColorPickEnum.contains;
                    }
                    else
                    {
                        this._task.SupremeColorPick = TaskObject.SuprimeColorPickEnum.exact;
                    }
                }
            }
            else if (num3 > 0x2ebde123)
            {
                if (num3 <= 0x599f894b)
                {
                    switch (num3)
                    {
                        case 0x38b6e100:
                            if (s == "funko")
                            {
                                FunkoControl2 content = (FunkoControl2) this._platformView.content.Content;
                                this._task.Platform = TaskObject.PlatformEnum.funko;
                                this._task.Login = false;
                                this._task.Link = content.txtLink.Text.Trim();
                                this._task.Keywords.Clear();
                                this._task.PositiveKeywordsChange();
                                for (int i = 0; i < content.txtPositiveKws.LineCount; i++)
                                {
                                    string lineText = content.txtPositiveKws.GetLineText(i);
                                    if (!string.IsNullOrEmpty(lineText) && (lineText != "\n"))
                                    {
                                        this._task.Keywords.Add(lineText);
                                    }
                                }
                                this._task.NegativeKeywords.Clear();
                                this._task.NegativeKeywordsChange();
                                if (content.rTypeDirect.IsChecked.HasValue && content.rTypeDirect.IsChecked.Value)
                                {
                                    this._task.TaskType = TaskObject.TaskTypeEnum.directlink;
                                }
                                else
                                {
                                    this._task.TaskType = TaskObject.TaskTypeEnum.keywords;
                                }
                            }
                            goto Label_3AE9;

                        case 0x3ef5cd01:
                            if (s == "hibbett")
                            {
                                HibbettControl2 content = (HibbettControl2) this._platformView.content.Content;
                                this._task.Platform = TaskObject.PlatformEnum.hibbett;
                                this._task.Link = content.txtLink.Text.Trim();
                                this._task.Login = false;
                                this._task.TaskType = TaskObject.TaskTypeEnum.directlink;
                                this._task.Payment = TaskObject.PaymentEnum.creditcard;
                            }
                            goto Label_3AE9;
                    }
                    if ((num3 == 0x599f894b) && (s == "nordstrom"))
                    {
                        NordstromControl2 content = (NordstromControl2) this._platformView.content.Content;
                        this._task.Platform = TaskObject.PlatformEnum.nordstrom;
                        this._task.Login = false;
                        this._task.Link = content.txtLink.Text.Trim();
                        if (content.rTypeDirect.IsChecked.HasValue && content.rTypeDirect.IsChecked.Value)
                        {
                            this._task.TaskType = TaskObject.TaskTypeEnum.directlink;
                        }
                        else
                        {
                            this._task.TaskType = TaskObject.TaskTypeEnum.variant;
                            this._task.Link = "https://shop.nordstrom.com";
                        }
                        this._task.SkuId = content.txtSkuId.Text.Trim();
                        this._task.StyleId = content.txtStyleId.Text.Trim();
                        if (content.rColorContains.IsChecked.HasValue && content.rColorContains.IsChecked.Value)
                        {
                            this._task.SupremeColorPick = TaskObject.SuprimeColorPickEnum.contains;
                        }
                        else
                        {
                            this._task.SupremeColorPick = TaskObject.SuprimeColorPickEnum.exact;
                        }
                        this._task.Color = content.txtColor.Text.Trim();
                        if (content.chPickRandomColorNotAvailable.IsChecked.HasValue && content.chPickRandomColorNotAvailable.IsChecked.Value)
                        {
                            this._task.ColorPickRandom = true;
                        }
                        else
                        {
                            this._task.ColorPickRandom = false;
                        }
                    }
                }
                else if (num3 <= 0x6fd1c80c)
                {
                    if (num3 == 0x6f2801c3)
                    {
                        if (s == "footsites")
                        {
                            FootSitesControl2 content = (FootSitesControl2) this._platformView.content.Content;
                            this._task.Platform = TaskObject.PlatformEnum.footsites;
                            this._task.Link = content.txtLink.Text.Trim();
                            this._task.VariousStringData = content.txtCartSecurityCheck.Text.Trim();
                            this._task.Keywords.Clear();
                            this._task.PositiveKeywordsChange();
                            this._task.TaskType = TaskObject.TaskTypeEnum.directlink;
                            if (this._task.Link.ToLowerInvariant().Contains("footlocker.com"))
                            {
                                this._task.FootSite = TaskObject.FootSitesEnum.footlocker;
                            }
                            else if (!this._task.Link.ToLowerInvariant().Contains("footlocker.ca"))
                            {
                                if (!this._task.Link.ToLowerInvariant().Contains("eastbay"))
                                {
                                    if (this._task.Link.ToLowerInvariant().Contains("footaction"))
                                    {
                                        this._task.FootSite = TaskObject.FootSitesEnum.footaction;
                                    }
                                    else
                                    {
                                        this._task.FootSite = TaskObject.FootSitesEnum.champssports;
                                    }
                                }
                                else
                                {
                                    this._task.FootSite = TaskObject.FootSitesEnum.eastbay;
                                }
                            }
                            else
                            {
                                this._task.FootSite = TaskObject.FootSitesEnum.footlockerCa;
                            }
                        }
                    }
                    else if ((num3 == 0x6fd1c80c) && (s == "supreme"))
                    {
                        SupremeControl2 content = (SupremeControl2) this._platformView.content.Content;
                        this._task.Platform = TaskObject.PlatformEnum.supreme;
                        this._task.ParentId = ((content.cmbParentTask.SelectedItem == null) || (((ComboBoxItem) content.cmbParentTask.SelectedItem).Tag.ToString() == "-1")) ? "" : ((ComboBoxItem) content.cmbParentTask.SelectedItem).Tag.ToString();
                        this._task.Link = content.txtLink.Text.Trim();
                        this._task.Color = content.txtColor.Text.Trim();
                        this._task.Group = ((ComboBoxItem) content.cmbGroup.SelectedItem).Tag.ToString();
                        this._task.ColorPickRandom = !content.chPickRandomColorNotAvailable.IsChecked.HasValue ? false : content.chPickRandomColorNotAvailable.IsChecked.Value;
                        this._task.SolveCaptcha = !content.chSolveCaptcha.IsChecked.HasValue ? false : content.chSolveCaptcha.IsChecked.Value;
                        if (content.rSizeExact.IsChecked.HasValue && content.rSizeExact.IsChecked.Value)
                        {
                            this._task.SupremeColorPick = TaskObject.SuprimeColorPickEnum.exact;
                        }
                        else
                        {
                            this._task.SupremeColorPick = TaskObject.SuprimeColorPickEnum.contains;
                        }
                        if (content.rAutomationBrowserless.IsChecked.HasValue && content.rAutomationBrowserless.IsChecked.Value)
                        {
                            this._task.SupremeAutomation = TaskObject.SuprimeAutomationEnum.browserless;
                        }
                        else
                        {
                            this._task.SupremeAutomation = TaskObject.SuprimeAutomationEnum.browser;
                        }
                        if (string.IsNullOrEmpty(this._task.ParentId))
                        {
                            if (this._task.State == TaskObject.StateEnum.multicart)
                            {
                                this._task.State = TaskObject.StateEnum.stopped;
                            }
                        }
                        else
                        {
                            this._task.State = TaskObject.StateEnum.multicart;
                        }
                        this._task.Keywords.Clear();
                        this._task.PositiveKeywordsChange();
                        for (int i = 0; i < content.txtPositiveKws.LineCount; i++)
                        {
                            string lineText = content.txtPositiveKws.GetLineText(i);
                            if (!string.IsNullOrEmpty(lineText) && (lineText != "\n"))
                            {
                                this._task.Keywords.Add(lineText);
                            }
                        }
                        this._task.NegativeKeywords.Clear();
                        this._task.NegativeKeywordsChange();
                        for (int j = 0; j < content.txtNegativeKws.LineCount; j++)
                        {
                            string lineText = content.txtNegativeKws.GetLineText(j);
                            if (!string.IsNullOrEmpty(lineText) && (lineText != "\n"))
                            {
                                this._task.NegativeKeywords.Add(lineText);
                            }
                        }
                        if (content.rTypeDirect.IsChecked.HasValue && content.rTypeDirect.IsChecked.Value)
                        {
                            this._task.TaskType = TaskObject.TaskTypeEnum.directlink;
                        }
                        else if (content.rTypeKeywords.IsChecked.HasValue && content.rTypeKeywords.IsChecked.Value)
                        {
                            this._task.TaskType = TaskObject.TaskTypeEnum.keywords;
                        }
                        else
                        {
                            this._task.TaskType = TaskObject.TaskTypeEnum.manualPicker;
                        }
                        if (content.rRegionUsa.IsChecked.HasValue && content.rRegionUsa.IsChecked.Value)
                        {
                            this._task.SupremeRegion = TaskObject.SupremeRegionEnum.USA;
                        }
                        else if (content.rRegionEu.IsChecked.HasValue && content.rRegionEu.IsChecked.Value)
                        {
                            this._task.SupremeRegion = TaskObject.SupremeRegionEnum.EU;
                        }
                        else
                        {
                            this._task.SupremeRegion = TaskObject.SupremeRegionEnum.JP;
                        }
                        this._task.GmailEmail = content.txtGmailUsername.Text;
                        this._task.GmailPassword = content.txtGmailPassword.Text;
                        if (content.radioButton_0.IsChecked.HasValue && content.radioButton_0.IsChecked.Value)
                        {
                            this._task.Payment = TaskObject.PaymentEnum.creditcard;
                        }
                        else
                        {
                            this._task.Payment = TaskObject.PaymentEnum.bankTransfer;
                        }
                    }
                }
                else if (num3 != 0x739afe01)
                {
                    if ((num3 == 0x9b115325) && (s == "solebox"))
                    {
                        SoleboxInfo info1 = new SoleboxInfo(this);
                        info1.ShowDialog();
                        if (!info1.Understood)
                        {
                            return;
                        }
                        SoleboxControl2 content = (SoleboxControl2) this._platformView.content.Content;
                        this._task.Platform = TaskObject.PlatformEnum.solebox;
                        this._task.Login = true;
                        this._task.Username = content.txtUsername.Text.Trim();
                        this._task.Password = content.txtPassword.Text.Trim();
                        this._task.Link = content.txtLink.Text.Trim();
                        if (content.radioButton_1.IsChecked.HasValue && content.radioButton_1.IsChecked.Value)
                        {
                            this._task.Payment = TaskObject.PaymentEnum.creditcard;
                        }
                        else if (content.radioButton_0.IsChecked.HasValue && content.radioButton_0.IsChecked.Value)
                        {
                            this._task.Payment = TaskObject.PaymentEnum.paypal;
                        }
                        else
                        {
                            this._task.Payment = TaskObject.PaymentEnum.bankTransfer;
                        }
                        this._task.TaskType = TaskObject.TaskTypeEnum.directlink;
                        if (flag)
                        {
                            if (!Global.SETTINGS.LOGIN_POOL.Any<LoginPool>(x => (x.Website == LoginPoolEnum.Solebox)))
                            {
                                MessageBox.Show("Solebox login pool empty", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                                return;
                            }
                            if (list.Count == 0)
                            {
                                foreach (LoginPool pool3 in from x in Global.SETTINGS.LOGIN_POOL
                                    where x.Website == LoginPoolEnum.Solebox
                                    select x)
                                {
                                    list.Add(pool3.Link);
                                }
                            }
                            this._task.Login = true;
                            char[] separator = new char[] { ':' };
                            string[] strArray2 = list[num8].Split(separator);
                            this._task.Username = strArray2[0].Trim();
                            this._task.Password = strArray2[1].Trim();
                            num8++;
                            if (num8 == list.Count)
                            {
                                num8 = 0;
                            }
                        }
                    }
                }
                else if (s == "footlocker")
                {
                    FootlockerControl content = (FootlockerControl) this._platformView.content.Content;
                    this._task.Platform = TaskObject.PlatformEnum.footlocker;
                    this._task.Link = content.txtLink.Text.Trim();
                    this._task.Login = false;
                    this._task.TaskType = TaskObject.TaskTypeEnum.directlink;
                    this._task.Payment = TaskObject.PaymentEnum.creditcard;
                    this._task.SolveCaptcha = !content.chBypassCaptcha.IsChecked.HasValue ? true : !content.chBypassCaptcha.IsChecked.Value;
                }
            }
            else if (num3 <= 0x1ceb28fd)
            {
                switch (num3)
                {
                    case 0x15a9716:
                        if (s == "titolo")
                        {
                            TitoloControl2 content = (TitoloControl2) this._platformView.content.Content;
                            this._task.Platform = TaskObject.PlatformEnum.titolo;
                            this._task.Link = content.txtLink.Text.Trim();
                            this._task.Login = true;
                            this._task.TaskType = TaskObject.TaskTypeEnum.directlink;
                            this._task.Payment = TaskObject.PaymentEnum.creditcard;
                            this._task.Username = content.txtUsername.Text.Trim();
                            this._task.Password = content.txtPassword.Text.Trim();
                            if (flag)
                            {
                                if (!Global.SETTINGS.LOGIN_POOL.Any<LoginPool>(x => (x.Website == LoginPoolEnum.Titolo)))
                                {
                                    MessageBox.Show("Solebox login pool empty", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                                    return;
                                }
                                if (list.Count == 0)
                                {
                                    foreach (LoginPool pool2 in from x in Global.SETTINGS.LOGIN_POOL
                                        where x.Website == LoginPoolEnum.Titolo
                                        select x)
                                    {
                                        list.Add(pool2.Link);
                                    }
                                }
                                this._task.Login = true;
                                char[] separator = new char[] { ':' };
                                string[] strArray3 = list[num8].Split(separator);
                                this._task.Username = strArray3[0].Trim();
                                this._task.Password = strArray3[1].Trim();
                                num8++;
                                if (num8 == list.Count)
                                {
                                    num8 = 0;
                                }
                            }
                        }
                        goto Label_3AE9;

                    case 0xaa6dc47:
                        if (s == "offwhite")
                        {
                            OffWhiteControl2 content = (OffWhiteControl2) this._platformView.content.Content;
                            this._task.Platform = TaskObject.PlatformEnum.offwhite;
                            this._task.Link = content.txtLink.Text.Trim();
                            this._task.Login = false;
                            this._task.Keywords.Clear();
                            this._task.PositiveKeywordsChange();
                            for (int i = 0; i < content.txtPositiveKws.LineCount; i++)
                            {
                                string lineText = content.txtPositiveKws.GetLineText(i);
                                if (!string.IsNullOrEmpty(lineText) && (lineText != "\n"))
                                {
                                    this._task.Keywords.Add(lineText);
                                }
                            }
                            this._task.NegativeKeywords.Clear();
                            this._task.NegativeKeywordsChange();
                            if (content.rTypeDirect.IsChecked.HasValue && content.rTypeDirect.IsChecked.Value)
                            {
                                this._task.TaskType = TaskObject.TaskTypeEnum.directlink;
                            }
                            else
                            {
                                this._task.TaskType = TaskObject.TaskTypeEnum.keywords;
                            }
                            if (content.radioButton_0.IsChecked.HasValue && content.radioButton_0.IsChecked.Value)
                            {
                                this._task.Payment = TaskObject.PaymentEnum.creditcard;
                            }
                            else
                            {
                                this._task.Payment = TaskObject.PaymentEnum.bankTransfer;
                            }
                        }
                        goto Label_3AE9;
                }
                if ((num3 == 0x1ceb28fd) && (s == "woodwood"))
                {
                    WoodwoodControl2 content = (WoodwoodControl2) this._platformView.content.Content;
                    this._task.Platform = TaskObject.PlatformEnum.woodwood;
                    this._task.Link = content.txtLink.Text.Trim();
                    this._task.Keywords.Clear();
                    this._task.PositiveKeywordsChange();
                    for (int i = 0; i < content.txtPositiveKws.LineCount; i++)
                    {
                        string lineText = content.txtPositiveKws.GetLineText(i);
                        if (!string.IsNullOrEmpty(lineText) && (lineText != "\n"))
                        {
                            this._task.Keywords.Add(lineText);
                        }
                    }
                    this._task.NegativeKeywords.Clear();
                    this._task.NegativeKeywordsChange();
                    if (content.rTypeDirect.IsChecked.HasValue && content.rTypeDirect.IsChecked.Value)
                    {
                        this._task.TaskType = TaskObject.TaskTypeEnum.directlink;
                    }
                    else
                    {
                        this._task.TaskType = TaskObject.TaskTypeEnum.keywords;
                    }
                    this._task.Color = content.txtColor.Text.Trim();
                    this._task.ColorPickRandom = !content.chPickRandomColorNotAvailable.IsChecked.HasValue ? false : content.chPickRandomColorNotAvailable.IsChecked.Value;
                    if (content.rColorExact.IsChecked.HasValue && content.rColorExact.IsChecked.Value)
                    {
                        this._task.SupremeColorPick = TaskObject.SuprimeColorPickEnum.exact;
                    }
                    else
                    {
                        this._task.SupremeColorPick = TaskObject.SuprimeColorPickEnum.contains;
                    }
                }
            }
            else
            {
                switch (num3)
                {
                    case 0x26ccbd5b:
                        if (s == "footaction")
                        {
                            FootactionControl content = (FootactionControl) this._platformView.content.Content;
                            this._task.Platform = TaskObject.PlatformEnum.footaction;
                            this._task.Link = content.txtLink.Text.Trim();
                            this._task.Login = false;
                            this._task.TaskType = TaskObject.TaskTypeEnum.directlink;
                            this._task.Payment = TaskObject.PaymentEnum.creditcard;
                            this._task.SolveCaptcha = !content.chBypassCaptcha.IsChecked.HasValue ? true : !content.chBypassCaptcha.IsChecked.Value;
                        }
                        goto Label_3AE9;

                    case 0x2c281c0d:
                        if (s == "sevres")
                        {
                            SevresControl content = (SevresControl) this._platformView.content.Content;
                            this._task.Platform = TaskObject.PlatformEnum.sevres;
                            this._task.Login = false;
                            this._task.Link = content.txtLink.Text.Trim();
                            this._task.Keywords.Clear();
                            this._task.PositiveKeywordsChange();
                            for (int i = 0; i < content.txtPositiveKws.LineCount; i++)
                            {
                                string lineText = content.txtPositiveKws.GetLineText(i);
                                if (!string.IsNullOrEmpty(lineText) && (lineText != "\n"))
                                {
                                    this._task.Keywords.Add(lineText);
                                }
                            }
                            this._task.NegativeKeywords.Clear();
                            this._task.NegativeKeywordsChange();
                            if (!string.IsNullOrEmpty(content.txtNegativeKeywords.Text.Trim()))
                            {
                                this._task.NegativeKeywords.Add(content.txtNegativeKeywords.Text.Trim());
                            }
                            if (content.rTypeDirect.IsChecked.HasValue && content.rTypeDirect.IsChecked.Value)
                            {
                                this._task.TaskType = TaskObject.TaskTypeEnum.directlink;
                            }
                            else
                            {
                                this._task.TaskType = TaskObject.TaskTypeEnum.keywords;
                            }
                        }
                        goto Label_3AE9;
                }
                if ((num3 == 0x2ebde123) && (s == "holypopstore"))
                {
                    HolypopstoreControl content = (HolypopstoreControl) this._platformView.content.Content;
                    this._task.Platform = TaskObject.PlatformEnum.holypopstore;
                    this._task.TaskType = TaskObject.TaskTypeEnum.directlink;
                    this._task.Login = true;
                    this._task.Username = content.txtUsername.Text.Trim();
                    this._task.Password = content.txtPassword.Text.Trim();
                    this._task.Link = content.txtLink.Text.Trim();
                    if (flag)
                    {
                        if (!Global.SETTINGS.LOGIN_POOL.Any<LoginPool>(x => (x.Website == LoginPoolEnum.Holypopstore)))
                        {
                            MessageBox.Show("Solebox login pool empty", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                            return;
                        }
                        if (list.Count == 0)
                        {
                            foreach (LoginPool pool in from x in Global.SETTINGS.LOGIN_POOL
                                where x.Website == LoginPoolEnum.Holypopstore
                                select x)
                            {
                                list.Add(pool.Link);
                            }
                        }
                        this._task.Login = true;
                        char[] separator = new char[] { ':' };
                        string[] strArray = list[num8].Split(separator);
                        this._task.Username = strArray[0].Trim();
                        this._task.Password = strArray[1].Trim();
                        num8++;
                        if (num8 == list.Count)
                        {
                            num8 = 0;
                        }
                    }
                }
            }
        Label_3AE9:
            if ((this._task.Keywords != null) && (this._task.Keywords.Count > 0))
            {
                for (int i = 0; i < this._task.Keywords.Count; i++)
                {
                    this._task.Keywords[i] = this._task.Keywords[i].TrimStart(Array.Empty<char>());
                }
            }
            if ((this._task.NegativeKeywords != null) && (this._task.NegativeKeywords.Count > 0))
            {
                for (int i = 0; i < this._task.NegativeKeywords.Count; i++)
                {
                    this._task.NegativeKeywords[i] = this._task.NegativeKeywords[i].TrimStart(Array.Empty<char>());
                }
            }
            if (this._operation == Global.FormOperation.insert)
            {
                Global.SETTINGS.TASKS.Add(this._task);
            }
            this.RunIds.Add(this._task.Id);
            num21++;
            goto Label_007E;
        }

        private void btnSaveAndRun_Click(object sender, RoutedEventArgs e)
        {
            this.RunAfterSave = true;
            this.btnSave_Click(sender, e);
        }

        private bool Check()
        {
            bool flag = true;
            if (string.IsNullOrEmpty(this.txtTaskName.Text))
            {
                this.txtTaskName.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                flag = false;
            }
            else
            {
                this.txtTaskName.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
            }
            if (((((ComboBoxItem) this.cmbPlatform.SelectedItem).Tag.ToString() != "supreme") || (((SupremeControl2) this._platformView.content.Content).cmbParentTask.SelectedItem == null)) || (((ComboBoxItem) ((SupremeControl2) this._platformView.content.Content).cmbParentTask.SelectedItem).Tag.ToString() == "-1"))
            {
                if ((this.cmbCheckoutProfile.SelectedItem != null) && (((ComboBoxItem) this.cmbCheckoutProfile.SelectedItem).Tag.ToString() != "-1"))
                {
                    ToggleButton templatedParent = this.cmbCheckoutProfile.Template.FindName("abc", this.cmbCheckoutProfile) as ToggleButton;
                    (templatedParent.Template.FindName("InnerBorder", templatedParent) as Border).Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                }
                else
                {
                    ToggleButton templatedParent = this.cmbCheckoutProfile.Template.FindName("abc", this.cmbCheckoutProfile) as ToggleButton;
                    (templatedParent.Template.FindName("InnerBorder", templatedParent) as Border).Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                    flag = false;
                }
            }
            if ((this._generalView.chSizeRandom.IsChecked.HasValue && this._generalView.chSizeRandom.IsChecked.Value) && (this._generalView.chSizePositive.IsChecked.HasValue && this._generalView.chSizePositive.IsChecked.Value))
            {
                this._generalView.txtSize.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
            }
            else if (!string.IsNullOrEmpty(this._generalView.txtSize.Text))
            {
                this._generalView.txtSize.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
            }
            else
            {
                this._generalView.txtSize.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                flag = false;
            }
            if (this._generalView.chDelayExact.IsChecked.HasValue && this._generalView.chDelayExact.IsChecked.Value)
            {
                if (!string.IsNullOrEmpty(this._generalView.txtDelay.Text))
                {
                    this._generalView.txtDelay.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                }
                else
                {
                    this._generalView.txtDelay.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                    flag = false;
                }
                this._generalView.txtDelayFrom.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                this._generalView.txtDelayTo.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
            }
            if (this._generalView.chDelayRandom.IsChecked.HasValue && this._generalView.chDelayRandom.IsChecked.Value)
            {
                if (!string.IsNullOrEmpty(this._generalView.txtDelayFrom.Text))
                {
                    this._generalView.txtDelayFrom.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                }
                else
                {
                    this._generalView.txtDelayFrom.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                    flag = false;
                }
                if (string.IsNullOrEmpty(this._generalView.txtDelayTo.Text))
                {
                    this._generalView.txtDelayTo.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                    flag = false;
                }
                else
                {
                    this._generalView.txtDelayTo.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                }
                this._generalView.txtDelay.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
            }
            bool flag2 = false;
            string s = ((ComboBoxItem) this.cmbPlatform.SelectedItem).Tag.ToString();
            uint num2 = <PrivateImplementationDetails>.ComputeStringHash(s);
            if (num2 > 0x9b115325)
            {
                if (num2 > 0xd25e7a39)
                {
                    if (num2 <= 0xe6761580)
                    {
                        switch (num2)
                        {
                            case 0xdffd2e3b:
                                if (s == "hottopic")
                                {
                                    flag2 = ((HottopicControl) this._platformView.content.Content).Check();
                                }
                                goto Label_0C1B;

                            case 0xe12329de:
                                if (s == "backdoor")
                                {
                                    flag2 = ((BackdoorControl2) this._platformView.content.Content).Check();
                                }
                                goto Label_0C1B;
                        }
                        if ((num2 == 0xe6761580) && (s == "finishline"))
                        {
                            flag2 = ((FinishlineControl) this._platformView.content.Content).Check();
                        }
                    }
                    else if (num2 > 0xf5825582)
                    {
                        if (num2 == 0xf8746c31)
                        {
                            if (s == "shopify")
                            {
                                flag2 = ((ShopifyControl2) this._platformView.content.Content).Check();
                            }
                        }
                        else if ((num2 == 0xf9f4459c) && (s == "supremeinstore"))
                        {
                            flag2 = ((SupremeInstoreControl2) this._platformView.content.Content).Check();
                        }
                    }
                    else if (num2 != 0xf2b3ba92)
                    {
                        if ((num2 == 0xf5825582) && (s == "boxlunch"))
                        {
                            flag2 = ((BoxlunchControl2) this._platformView.content.Content).Check();
                        }
                    }
                    else if (s == "mrporter")
                    {
                        flag2 = ((MrPorterControl2) this._platformView.content.Content).Check();
                    }
                }
                else if (num2 <= 0xb98c68cf)
                {
                    switch (num2)
                    {
                        case 0xa100bebc:
                            if (s == "mesh")
                            {
                                flag2 = ((MeshControl2) this._platformView.content.Content).Check();
                            }
                            goto Label_0C1B;

                        case 0xa22f8308:
                            if (s == "puma")
                            {
                                flag2 = ((PumaControl2) this._platformView.content.Content).Check();
                            }
                            goto Label_0C1B;
                    }
                    if ((num2 == 0xb98c68cf) && (s == "footlockerau"))
                    {
                        flag2 = ((FootlockerauControl2) this._platformView.content.Content).Check();
                    }
                }
                else if (num2 <= 0xc6a8d884)
                {
                    if (num2 == 0xbdeaf8c8)
                    {
                        if (s == "converse")
                        {
                            flag2 = ((ConverseControl2) this._platformView.content.Content).Check();
                        }
                    }
                    else if ((num2 == 0xc6a8d884) && (s == "mcm"))
                    {
                        flag2 = ((McmControl2) this._platformView.content.Content).Check();
                    }
                }
                else if (num2 == 0xc9957c5b)
                {
                    if (s == "footlockereu")
                    {
                        flag2 = ((FootlockereuControl) this._platformView.content.Content).Check();
                    }
                }
                else if ((num2 == 0xd25e7a39) && (s == "sneakersnstuff"))
                {
                    flag2 = ((SneakernstuffControl2) this._platformView.content.Content).Check();
                }
            }
            else if (num2 <= 0x2ebde123)
            {
                if (num2 > 0x1ceb28fd)
                {
                    switch (num2)
                    {
                        case 0x26ccbd5b:
                            if (s == "footaction")
                            {
                                flag2 = ((FootactionControl) this._platformView.content.Content).Check();
                            }
                            goto Label_0C1B;

                        case 0x2c281c0d:
                            if (s == "sevres")
                            {
                                flag2 = ((SevresControl) this._platformView.content.Content).Check();
                            }
                            goto Label_0C1B;
                    }
                    if ((num2 == 0x2ebde123) && (s == "holypopstore"))
                    {
                        flag2 = ((HolypopstoreControl) this._platformView.content.Content).Check();
                    }
                }
                else
                {
                    switch (num2)
                    {
                        case 0x15a9716:
                            if (s == "titolo")
                            {
                                flag2 = ((TitoloControl2) this._platformView.content.Content).Check();
                            }
                            goto Label_0C1B;

                        case 0xaa6dc47:
                            if (s == "offwhite")
                            {
                                flag2 = ((OffWhiteControl2) this._platformView.content.Content).Check();
                            }
                            goto Label_0C1B;
                    }
                    if ((num2 == 0x1ceb28fd) && (s == "woodwood"))
                    {
                        flag2 = ((WoodwoodControl2) this._platformView.content.Content).Check();
                    }
                }
            }
            else if (num2 <= 0x599f894b)
            {
                switch (num2)
                {
                    case 0x38b6e100:
                        if (s == "funko")
                        {
                            flag2 = ((FunkoControl2) this._platformView.content.Content).Check();
                        }
                        goto Label_0C1B;

                    case 0x3ef5cd01:
                        if (s == "hibbett")
                        {
                            flag2 = ((HibbettControl2) this._platformView.content.Content).Check();
                        }
                        goto Label_0C1B;
                }
                if ((num2 == 0x599f894b) && (s == "nordstrom"))
                {
                    flag2 = ((NordstromControl2) this._platformView.content.Content).Check();
                }
            }
            else if (num2 > 0x6fd1c80c)
            {
                if (num2 == 0x739afe01)
                {
                    if (s == "footlocker")
                    {
                        flag2 = ((FootlockerControl) this._platformView.content.Content).Check();
                    }
                }
                else if ((num2 == 0x9b115325) && (s == "solebox"))
                {
                    flag2 = ((SoleboxControl2) this._platformView.content.Content).Check();
                }
            }
            else if (num2 != 0x6f2801c3)
            {
                if ((num2 == 0x6fd1c80c) && (s == "supreme"))
                {
                    flag2 = ((SupremeControl2) this._platformView.content.Content).Check();
                }
            }
            else if (s == "footsites")
            {
                flag2 = ((FootSitesControl2) this._platformView.content.Content).Check();
            }
        Label_0C1B:
            return (flag & flag2);
        }

        private void cmbCheckoutProfile_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.cmbCheckoutProfile.SelectedItem != null)
            {
                string id = ((ComboBoxItem) this.cmbCheckoutProfile.SelectedItem).Tag.ToString();
                if (Global.SETTINGS.PROFILES_GROUPS.Any<ProfileGroupObject>(x => x.Id == id))
                {
                    ProfileGroupObject obj2 = Global.SETTINGS.PROFILES_GROUPS.First<ProfileGroupObject>(x => x.Id == id);
                    this._advancedView.txtMassCreate.Text = obj2.Profiles;
                }
            }
        }

        private void cmbPlatform_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.lvMenu.SelectedIndex != 0)
            {
                this.lvMenu.SelectedIndex = 0;
            }
            this._platformView.content.Content = null;
            this._generalView.gbPriceCheck.IsEnabled = true;
            this._generalView.gbPriceCheck.Opacity = 1.0;
            this._generalView.gbSizing.IsEnabled = true;
            this._generalView.gbSizing.Opacity = 1.0;
            string s = ((ComboBoxItem) this.cmbPlatform.SelectedItem).Tag.ToString();
            uint num2 = <PrivateImplementationDetails>.ComputeStringHash(s);
            if (num2 <= 0x9b115325)
            {
                if (num2 > 0x2ebde123)
                {
                    if (num2 <= 0x599f894b)
                    {
                        switch (num2)
                        {
                            case 0x38b6e100:
                                if (s == "funko")
                                {
                                    this._platformView.content.Content = new FunkoControl2(this);
                                    base.Height = 500.0;
                                    this.BtnInfo.Visibility = Visibility.Visible;
                                    if (!this._platformsInit)
                                    {
                                        this._task.Platform = TaskObject.PlatformEnum.funko;
                                    }
                                }
                                goto Label_0C0C;

                            case 0x3ef5cd01:
                                if (s == "hibbett")
                                {
                                    this._platformView.content.Content = new HibbettControl2(this);
                                    base.Height = 500.0;
                                    this.BtnInfo.Visibility = Visibility.Hidden;
                                    if (!this._platformsInit)
                                    {
                                        this._task.Platform = TaskObject.PlatformEnum.hibbett;
                                    }
                                }
                                goto Label_0C0C;
                        }
                        if ((num2 == 0x599f894b) && (s == "nordstrom"))
                        {
                            this._platformView.content.Content = new NordstromControl2(this);
                            base.Height = 500.0;
                            this.BtnInfo.Visibility = Visibility.Hidden;
                            if (!this._platformsInit)
                            {
                                this._task.Platform = TaskObject.PlatformEnum.nordstrom;
                            }
                        }
                    }
                    else if (num2 > 0x6fd1c80c)
                    {
                        if (num2 == 0x739afe01)
                        {
                            if (s == "footlocker")
                            {
                                this._platformView.content.Content = new FootlockerControl(this);
                                base.Height = 500.0;
                                this.BtnInfo.Visibility = Visibility.Hidden;
                                if (!this._platformsInit)
                                {
                                    this._task.Platform = TaskObject.PlatformEnum.footlocker;
                                }
                            }
                        }
                        else if ((num2 == 0x9b115325) && (s == "solebox"))
                        {
                            this._platformView.content.Content = new SoleboxControl2(this);
                            base.Height = 500.0;
                            this.BtnInfo.Visibility = Visibility.Visible;
                            if (!this._platformsInit)
                            {
                                this._task.Platform = TaskObject.PlatformEnum.solebox;
                            }
                        }
                    }
                    else if (num2 == 0x6f2801c3)
                    {
                        if (s == "footsites")
                        {
                            this._platformView.content.Content = new FootSitesControl2(this);
                            base.Height = 500.0;
                            this.BtnInfo.Visibility = Visibility.Visible;
                            if (!this._platformsInit)
                            {
                                this._task.Platform = TaskObject.PlatformEnum.footsites;
                            }
                        }
                    }
                    else if ((num2 == 0x6fd1c80c) && (s == "supreme"))
                    {
                        this._platformView.content.Content = new SupremeControl2(this);
                        base.Height = 600.0;
                        this.BtnInfo.Visibility = Visibility.Hidden;
                        if (!this._platformsInit)
                        {
                            this._task.Platform = TaskObject.PlatformEnum.supreme;
                        }
                    }
                }
                else if (num2 <= 0x1ceb28fd)
                {
                    switch (num2)
                    {
                        case 0x15a9716:
                            if (s == "titolo")
                            {
                                this._platformView.content.Content = new TitoloControl2(this);
                                base.Height = 500.0;
                                this.BtnInfo.Visibility = Visibility.Hidden;
                                if (!this._platformsInit)
                                {
                                    this._task.Platform = TaskObject.PlatformEnum.titolo;
                                }
                            }
                            goto Label_0C0C;

                        case 0xaa6dc47:
                            if (s == "offwhite")
                            {
                                this._platformView.content.Content = new OffWhiteControl2(this);
                                base.Height = 500.0;
                                this.BtnInfo.Visibility = Visibility.Hidden;
                                if (!this._platformsInit)
                                {
                                    this._task.Platform = TaskObject.PlatformEnum.offwhite;
                                }
                            }
                            goto Label_0C0C;
                    }
                    if ((num2 == 0x1ceb28fd) && (s == "woodwood"))
                    {
                        this._platformView.content.Content = new WoodwoodControl2(this);
                        base.Height = 500.0;
                        this.BtnInfo.Visibility = Visibility.Hidden;
                        if (!this._platformsInit)
                        {
                            this._task.Platform = TaskObject.PlatformEnum.woodwood;
                        }
                    }
                }
                else
                {
                    switch (num2)
                    {
                        case 0x26ccbd5b:
                            if (s == "footaction")
                            {
                                this._platformView.content.Content = new FootactionControl(this);
                                base.Height = 500.0;
                                this.BtnInfo.Visibility = Visibility.Hidden;
                                if (!this._platformsInit)
                                {
                                    this._task.Platform = TaskObject.PlatformEnum.footaction;
                                }
                            }
                            goto Label_0C0C;

                        case 0x2c281c0d:
                            if (s == "sevres")
                            {
                                this._platformView.content.Content = new SevresControl(this);
                                base.Height = 505.0;
                                this.BtnInfo.Visibility = Visibility.Hidden;
                                if (!this._platformsInit)
                                {
                                    this._task.Platform = TaskObject.PlatformEnum.sevres;
                                }
                            }
                            goto Label_0C0C;
                    }
                    if ((num2 == 0x2ebde123) && (s == "holypopstore"))
                    {
                        this._platformView.content.Content = new HolypopstoreControl(this);
                        base.Height = 500.0;
                        this.BtnInfo.Visibility = Visibility.Hidden;
                        if (!this._platformsInit)
                        {
                            this._task.Platform = TaskObject.PlatformEnum.holypopstore;
                        }
                    }
                }
            }
            else if (num2 <= 0xd25e7a39)
            {
                if (num2 <= 0xb98c68cf)
                {
                    switch (num2)
                    {
                        case 0xa100bebc:
                            if (s == "mesh")
                            {
                                this._platformView.content.Content = new MeshControl2(this);
                                base.Height = 500.0;
                                this.BtnInfo.Visibility = Visibility.Hidden;
                                if (!this._platformsInit)
                                {
                                    this._task.Platform = TaskObject.PlatformEnum.mesh;
                                }
                            }
                            goto Label_0C0C;

                        case 0xa22f8308:
                            if (s == "puma")
                            {
                                this._platformView.content.Content = new PumaControl2(this);
                                base.Height = 500.0;
                                this.BtnInfo.Visibility = Visibility.Visible;
                                if (!this._platformsInit)
                                {
                                    this._task.Platform = TaskObject.PlatformEnum.puma;
                                }
                            }
                            goto Label_0C0C;
                    }
                    if ((num2 == 0xb98c68cf) && (s == "footlockerau"))
                    {
                        this._platformView.content.Content = new FootlockerauControl2(this);
                        base.Height = 500.0;
                        this.BtnInfo.Visibility = Visibility.Visible;
                        if (!this._platformsInit)
                        {
                            this._task.Platform = TaskObject.PlatformEnum.footlockerau;
                        }
                    }
                }
                else if (num2 <= 0xc6a8d884)
                {
                    if (num2 == 0xbdeaf8c8)
                    {
                        if (s == "converse")
                        {
                            this._platformView.content.Content = new ConverseControl2(this);
                            base.Height = 500.0;
                            this.BtnInfo.Visibility = Visibility.Visible;
                            if (!this._platformsInit)
                            {
                                this._task.Platform = TaskObject.PlatformEnum.converse;
                            }
                        }
                    }
                    else if ((num2 == 0xc6a8d884) && (s == "mcm"))
                    {
                        this._platformView.content.Content = new McmControl2(this);
                        base.Height = 500.0;
                        this.BtnInfo.Visibility = Visibility.Visible;
                        if (!this._platformsInit)
                        {
                            this._task.Platform = TaskObject.PlatformEnum.mcm;
                        }
                    }
                }
                else if (num2 == 0xc9957c5b)
                {
                    if (s == "footlockereu")
                    {
                        this._platformView.content.Content = new FootlockereuControl(this);
                        base.Height = 500.0;
                        this.BtnInfo.Visibility = Visibility.Hidden;
                        if (!this._platformsInit)
                        {
                            this._task.Platform = TaskObject.PlatformEnum.footlockereu;
                        }
                    }
                }
                else if ((num2 == 0xd25e7a39) && (s == "sneakersnstuff"))
                {
                    this._platformView.content.Content = new SneakernstuffControl2(this);
                    base.Height = 510.0;
                    this.BtnInfo.Visibility = Visibility.Hidden;
                    if (!this._platformsInit)
                    {
                        this._task.Platform = TaskObject.PlatformEnum.sneakersnstuff;
                    }
                }
            }
            else if (num2 > 0xe6761580)
            {
                if (num2 <= 0xf5825582)
                {
                    if (num2 == 0xf2b3ba92)
                    {
                        if (s == "mrporter")
                        {
                            this._platformView.content.Content = new MrPorterControl2(this);
                            base.Height = 500.0;
                            this.BtnInfo.Visibility = Visibility.Visible;
                            if (!this._platformsInit)
                            {
                                this._task.Platform = TaskObject.PlatformEnum.mrporter;
                            }
                        }
                    }
                    else if ((num2 == 0xf5825582) && (s == "boxlunch"))
                    {
                        this._platformView.content.Content = new BoxlunchControl2(this);
                        base.Height = 505.0;
                        this.BtnInfo.Visibility = Visibility.Visible;
                        if (!this._platformsInit)
                        {
                            this._task.Platform = TaskObject.PlatformEnum.boxlunch;
                        }
                    }
                }
                else if (num2 == 0xf8746c31)
                {
                    if (s == "shopify")
                    {
                        this._platformView.content.Content = new ShopifyControl2(this);
                        base.Height = 600.0;
                        this.BtnInfo.Visibility = Visibility.Hidden;
                        if (!this._platformsInit)
                        {
                            this._task.Platform = TaskObject.PlatformEnum.shopify;
                        }
                    }
                }
                else if ((num2 == 0xf9f4459c) && (s == "supremeinstore"))
                {
                    this._platformView.content.Content = new SupremeInstoreControl2(this);
                    base.Height = 500.0;
                    this.BtnInfo.Visibility = Visibility.Hidden;
                    if (!this._platformsInit)
                    {
                        this._task.Platform = TaskObject.PlatformEnum.supremeinstore;
                    }
                }
            }
            else if (num2 == 0xdffd2e3b)
            {
                if (s == "hottopic")
                {
                    this._platformView.content.Content = new HottopicControl(this);
                    base.Height = 505.0;
                    this.BtnInfo.Visibility = Visibility.Hidden;
                    if (!this._platformsInit)
                    {
                        this._task.Platform = TaskObject.PlatformEnum.hottopic;
                    }
                }
            }
            else if (num2 != 0xe12329de)
            {
                if ((num2 == 0xe6761580) && (s == "finishline"))
                {
                    this._platformView.content.Content = new FinishlineControl(this);
                    base.Height = 500.0;
                    this.BtnInfo.Visibility = Visibility.Hidden;
                    if (!this._platformsInit)
                    {
                        this._task.Platform = TaskObject.PlatformEnum.finishline;
                    }
                }
            }
            else if (s == "backdoor")
            {
                this._platformView.content.Content = new BackdoorControl2(this);
                base.Height = 500.0;
                this.BtnInfo.Visibility = Visibility.Hidden;
                if (!this._platformsInit)
                {
                    this._task.Platform = TaskObject.PlatformEnum.backdoor;
                }
            }
        Label_0C0C:
            this._advancedView.cmbWatchTask.Items.Clear();
            ComboBoxItem newItem = new ComboBoxItem {
                Content = "",
                Tag = "-1"
            };
            this._advancedView.cmbWatchTask.Items.Add(newItem);
            foreach (TaskObject obj2 in from x in Global.SETTINGS.TASKS
                where (x.Platform == this._task.Platform) && string.IsNullOrEmpty(x.WatchTaskId)
                select x)
            {
                if (obj2.Id != this._task.Id)
                {
                    ComboBoxItem item2 = new ComboBoxItem {
                        Content = obj2.Name,
                        Tag = obj2.Id
                    };
                    this._advancedView.cmbWatchTask.Items.Add(item2);
                }
            }
            this._advancedView.cmbWatchTask.SelectedIndex = 0;
        }

        private void cmbProxyList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((this.cmbProxyList.SelectedItem != null) && (((ComboBoxItem) this.cmbProxyList.SelectedItem).Tag.ToString() != "-1"))
            {
                this._advancedView.chLocalIPCheckout.IsEnabled = true;
                this._advancedView.chLocalIPCheckout.Opacity = 1.0;
            }
            else
            {
                this._advancedView.chLocalIPCheckout.IsEnabled = false;
                this._advancedView.chLocalIPCheckout.IsChecked = false;
                this._advancedView.chLocalIPCheckout.Opacity = 0.6;
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
            switch (((-450018825 ^ -740423296) % 4))
            {
                case 0:
                    break;

                case 2:
                    goto Label_000D;

                case 3:
                    return;

                default:
                {
                    Uri resourceLocator = new Uri("/EveAIO;component/views/taskwindow2.xaml", UriKind.Relative);
                    Application.LoadComponent(this, resourceLocator);
                    return;
                }
            }
        Label_002C:
            this._contentLoaded = true;
            goto Label_000D;
        }

        private void LoadPlatforms()
        {
            this._platformsInit = true;
            foreach (KeyValuePair<string, string> pair in WebsitesInfo.SUPPORTED_PLATFORMS)
            {
                ComboBoxItem newItem = new ComboBoxItem {
                    Content = pair.Value,
                    Tag = pair.Key
                };
                this.cmbPlatform.Items.Add(newItem);
            }
            this.cmbPlatform.SelectedIndex = 0;
            this._platformsInit = false;
        }

        private void LoadProfiles()
        {
            ComboBoxItem newItem = new ComboBoxItem {
                Content = "",
                Tag = "-1"
            };
            this.cmbCheckoutProfile.Items.Add(newItem);
            if (this._operation == Global.FormOperation.insert)
            {
                foreach (ProfileGroupObject obj2 in Global.SETTINGS.PROFILES_GROUPS)
                {
                    ComboBoxItem item = new ComboBoxItem();
                    string[] textArray1 = new string[] { "*", obj2.Name, " (", obj2.Profiles, ")" };
                    item.Content = string.Concat(textArray1);
                    item.Tag = obj2.Id;
                    this.cmbCheckoutProfile.Items.Add(item);
                }
                ComboBoxItem item2 = new ComboBoxItem {
                    Content = "---------",
                    Tag = "-1"
                };
                this.cmbCheckoutProfile.Items.Add(item2);
            }
            foreach (ProfileObject obj3 in Global.SETTINGS.PROFILES)
            {
                ComboBoxItem item3 = new ComboBoxItem {
                    Content = obj3.Name,
                    Tag = obj3.Id
                };
                this.cmbCheckoutProfile.Items.Add(item3);
            }
        }

        private void LoadProxies()
        {
            ComboBoxItem newItem = new ComboBoxItem {
                Content = "",
                Tag = "-1"
            };
            this.cmbProxyList.Items.Add(newItem);
            foreach (ProxyListObject obj2 in Global.SETTINGS.PROXIES)
            {
                ComboBoxItem item = new ComboBoxItem();
                object[] objArray1 = new object[] { obj2.Name, " (", obj2.ProxiesCount, ")" };
                item.Content = string.Concat(objArray1);
                item.Tag = obj2.Id;
                this.cmbProxyList.Items.Add(item);
            }
        }

        private void lvMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // This item is obfuscated and can not be translated.
            switch (this.lvMenu.SelectedIndex)
            {
                case 0:
                    break;

                case 1:
                    goto Label_003E;

                case 2:
                    goto Label_0084;

                default:
                    goto Label_0055;
            }
        Label_0025:
            this.MenusFrame.Navigate(this._platformView);
            goto Label_0055;
        Label_003E:
            this.MenusFrame.Navigate(this._generalView);
        Label_0055:
            switch (((-1568710447 ^ -1478066033) % 8))
            {
                case 0:
                    goto Label_003E;

                case 1:
                    return;

                case 2:
                    return;

                case 3:
                    return;

                case 4:
                    goto Label_0025;

                case 5:
                    goto Label_0055;

                case 6:
                    return;

                case 7:
                    break;

                default:
                    return;
            }
        Label_0084:
            this.MenusFrame.Navigate(this._advancedView);
            goto Label_0055;
        }

        [EditorBrowsable(EditorBrowsableState.Never), GeneratedCode("PresentationBuildTasks", "4.0.0.0"), DebuggerNonUserCode]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    ((TaskWindow2) target).Loaded += new RoutedEventHandler(this.Window_Loaded);
                    ((TaskWindow2) target).Closing += new CancelEventHandler(this.Window_Closing);
                    return;

                case 2:
                    ((Grid) target).MouseLeftButtonDown += new MouseButtonEventHandler(this.Grid_MouseLeftButtonDown);
                    return;

                case 3:
                    this.lvMenu = (ListBox) target;
                    this.lvMenu.SelectionChanged += new SelectionChangedEventHandler(this.lvMenu_SelectionChanged);
                    return;

                case 4:
                    this.BtnInfo = (Button) target;
                    this.BtnInfo.Click += new RoutedEventHandler(this.BtnInfo_Click);
                    return;

                case 5:
                    this.BtnClose = (Button) target;
                    this.BtnClose.Click += new RoutedEventHandler(this.BtnClose_Click);
                    return;

                case 6:
                    this.txtTaskName = (TextBox) target;
                    return;

                case 7:
                    this.cmbPlatform = (ComboBox) target;
                    this.cmbPlatform.SelectionChanged += new SelectionChangedEventHandler(this.cmbPlatform_SelectionChanged);
                    return;

                case 8:
                    this.cmbCheckoutProfile = (ComboBox) target;
                    this.cmbCheckoutProfile.SelectionChanged += new SelectionChangedEventHandler(this.cmbCheckoutProfile_SelectionChanged);
                    return;

                case 9:
                    this.cmbProxyList = (ComboBox) target;
                    this.cmbProxyList.SelectionChanged += new SelectionChangedEventHandler(this.cmbProxyList_SelectionChanged);
                    return;

                case 10:
                    this.MenusFrame = (System.Windows.Controls.Frame) target;
                    return;

                case 11:
                    this.btnSaveAndRun = (Button) target;
                    this.btnSaveAndRun.Click += new RoutedEventHandler(this.btnSaveAndRun_Click);
                    return;

                case 12:
                    this.btnSave = (Button) target;
                    this.btnSave.Click += new RoutedEventHandler(this.btnSave_Click);
                    return;

                case 13:
                    this.btnCancel = (Button) target;
                    this.btnCancel.Click += new RoutedEventHandler(this.btnCancel_Click);
                    return;
            }
            this._contentLoaded = true;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ShopifyControl2 control3;
            IEnumerator enumerator;
            int num15;
            this._platformView = new PlatformView(this);
            this._generalView = new GeneralTaskSettingsView(this);
            this._advancedView = new AdvancedTaskSettingsView(this);
            this._advancedView.txtMassCreate.IsEnabled = false;
            this._advancedView.txtMassCreate.Opacity = 0.6;
            this._advancedView.lblMassCreate.Opacity = 0.6;
            this.LoadProfiles();
            this.LoadPlatforms();
            this.LoadProxies();
            if (this._operation != Global.FormOperation.update)
            {
                this._advancedView.txtMassCreate.IsEnabled = true;
                this._advancedView.txtMassCreate.Opacity = 1.0;
                this._advancedView.lblMassCreate.Opacity = 1.0;
                ShopifyControl2 control26 = (ShopifyControl2) this._platformView.content.Content;
                control26.radioButton_0.IsChecked = true;
                control26.rTypeDirect.IsChecked = true;
                this._generalView.chDelayRandom.IsChecked = true;
                this._generalView.txtDelayFrom.Text = "2000";
                this._generalView.txtDelayTo.Text = "3500";
                this._advancedView.chLocalIPCheckout.IsChecked = false;
                this._advancedView.chLocalIPCheckout.IsEnabled = false;
                this._generalView.chNotify.IsChecked = true;
                this._generalView.chSizeRandom.IsChecked = true;
                this._generalView.chSizePositive.IsChecked = true;
                goto Label_2480;
            }
            this.txtTaskName.Text = this._task.Name;
            this._generalView.chSizeRandom.IsChecked = new bool?(this._task.RandomSize);
            this._generalView.chPickRandomNotAvailable.IsChecked = new bool?(this._task.SizePickRandom);
            this._generalView.chRetryOnError.IsChecked = new bool?(this._task.RetryOnError);
            this._generalView.chNotify.IsChecked = new bool?(this._task.Notify);
            this._generalView.txtSize.Text = this._task.Size;
            if (!this._task.IsNegativeSizing)
            {
                this._generalView.chSizePositive.IsChecked = true;
            }
            else
            {
                this._generalView.chSizeNegative.IsChecked = true;
            }
            this._generalView.switchPriceCheck.IsChecked = new bool?(this._task.PriceCheck);
            this._generalView.txtPriceCheckMin.Text = this._task.MinimumPrice.ToString();
            this._generalView.txtPriceCheckMax.Text = this._task.MaximumPrice.ToString();
            this._generalView.txtCaptchaRequests.Text = this._task.CaptchaRequests.ToString();
            this._generalView.txtCheckoutDelay.Text = this._task.CheckoutDelay.ToString();
            this._advancedView.switchAdvancedCaptcha.IsChecked = new bool?(this._task.SpecificCaptcha);
            this._advancedView.textBox_0.Text = this._task.TwoCaptchaRequests.ToString();
            this._advancedView.txtAntiCaptcha.Text = this._task.AnticaptchaRequests.ToString();
            this._advancedView.txtImageTypers.Text = this._task.ImagetypersRequests.ToString();
            this._advancedView.txtDisolve.Text = this._task.DisolveRequests.ToString();
            this._advancedView.chPaypalProxyIgnore.IsChecked = new bool?(this._task.PaypalProxyIgnore);
            if (this._task.RetryDelay == TaskObject.RetryDelayEnum.exact)
            {
                this._generalView.chDelayExact.IsChecked = true;
            }
            else
            {
                this._generalView.chDelayRandom.IsChecked = true;
            }
            this._generalView.txtDelay.Text = this._task.Delay;
            this._generalView.txtDelayFrom.Text = this._task.DelayFrom;
            this._generalView.txtDelayTo.Text = this._task.DelayTo;
            this._advancedView.chLocalIPCheckout.IsChecked = new bool?(this._task.LocalIPCheckout);
            using (enumerator = ((IEnumerable) this.cmbPlatform.Items).GetEnumerator())
            {
                ComboBoxItem item5;
                goto Label_03ED;
            Label_03B2:
                item5 = (ComboBoxItem) enumerator.Current;
                if (this._task.Platform.ToString() == item5.Tag.ToString())
                {
                    goto Label_03F8;
                }
            Label_03ED:
                if (!enumerator.MoveNext())
                {
                    goto Label_041E;
                }
                goto Label_03B2;
            Label_03F8:
                this.cmbPlatform.SelectedItem = item5;
            }
        Label_041E:
            using (enumerator = ((IEnumerable) this.cmbCheckoutProfile.Items).GetEnumerator())
            {
                ComboBoxItem item2;
                goto Label_045F;
            Label_0433:
                item2 = (ComboBoxItem) enumerator.Current;
                if (this._task.CheckoutId == item2.Tag.ToString())
                {
                    goto Label_046A;
                }
            Label_045F:
                if (!enumerator.MoveNext())
                {
                    goto Label_0490;
                }
                goto Label_0433;
            Label_046A:
                this.cmbCheckoutProfile.SelectedItem = item2;
            }
        Label_0490:
            using (enumerator = ((IEnumerable) this.cmbProxyList.Items).GetEnumerator())
            {
                ComboBoxItem item6;
                goto Label_04D1;
            Label_04A5:
                item6 = (ComboBoxItem) enumerator.Current;
                if (this._task.ProxyListId == item6.Tag.ToString())
                {
                    goto Label_04DC;
                }
            Label_04D1:
                if (!enumerator.MoveNext())
                {
                    goto Label_0502;
                }
                goto Label_04A5;
            Label_04DC:
                this.cmbProxyList.SelectedItem = item6;
            }
        Label_0502:
            using (enumerator = ((IEnumerable) this._advancedView.cmbWatchTask.Items).GetEnumerator())
            {
                ComboBoxItem current;
                while (enumerator.MoveNext())
                {
                    current = (ComboBoxItem) enumerator.Current;
                    if (!string.IsNullOrEmpty(this._task.WatchTaskId) && (this._task.WatchTaskId == current.Tag.ToString()))
                    {
                        goto Label_0565;
                    }
                }
                goto Label_058E;
            Label_0565:
                this._advancedView.cmbWatchTask.SelectedItem = current;
            }
        Label_058E:
            switch (this._task.Platform)
            {
                case TaskObject.PlatformEnum.shopify:
                    base.Height = 600.0;
                    control3 = (ShopifyControl2) this._platformView.content.Content;
                    control3.switchLogin.IsChecked = new bool?(this._task.Login);
                    control3.txtUsername.Text = this._task.Username;
                    control3.txtPassword.Text = this._task.Password;
                    control3.switchLast25.IsChecked = new bool?(this._task.Last25Products);
                    control3.switchOldMode.IsChecked = new bool?(this._task.ShopifyIsOldMode);
                    if (this._task.Quantity < 1)
                    {
                        this._task.Quantity = 1;
                    }
                    control3.txtQuantity.Text = this._task.Quantity.ToString();
                    control3.txtLink.Text = this._task.Link;
                    control3.txtVariant.Text = this._task.Variant;
                    for (int j = 0; j < this._task.Keywords.Count; j++)
                    {
                        control3.txtPositiveKws.AppendText(this._task.Keywords[j]);
                    }
                    for (int k = 0; k < this._task.NegativeKeywords.Count; k++)
                    {
                        control3.txtNegativeKws.AppendText(this._task.NegativeKeywords[k]);
                    }
                    if (this._task.TaskType == TaskObject.TaskTypeEnum.directlink)
                    {
                        control3.rTypeDirect.IsChecked = true;
                    }
                    else if (this._task.TaskType == TaskObject.TaskTypeEnum.keywords)
                    {
                        control3.rTypeKeywords.IsChecked = true;
                    }
                    else
                    {
                        control3.rTypeVariant.IsChecked = true;
                    }
                    if (!string.IsNullOrEmpty(this._task.ShopifyWebsite))
                    {
                        using (enumerator = ((IEnumerable) control3.cmbWebsite.Items).GetEnumerator())
                        {
                            ComboBoxItem current;
                            while (enumerator.MoveNext())
                            {
                                current = (ComboBoxItem) enumerator.Current;
                                if (current.Tag.ToString() == this._task.ShopifyWebsite)
                                {
                                    goto Label_0868;
                                }
                            }
                            goto Label_08AC;
                        Label_0868:
                            control3.cmbWebsite.SelectedItem = current;
                            goto Label_08AC;
                        }
                    }
                    control3.cmbWebsite.SelectedIndex = control3.cmbWebsite.Items.Count - 1;
                    goto Label_08AC;

                case TaskObject.PlatformEnum.supreme:
                    goto Label_0A88;

                case TaskObject.PlatformEnum.sneakersnstuff:
                {
                    base.Height = 510.0;
                    SneakernstuffControl2 control4 = (SneakernstuffControl2) this._platformView.content.Content;
                    control4.switchLogin.IsChecked = new bool?(this._task.Login);
                    control4.txtUsername.Text = this._task.Username;
                    control4.txtPassword.Text = this._task.Password;
                    if (this._task.TaskType == TaskObject.TaskTypeEnum.directlink)
                    {
                        control4.rTypeDirect.IsChecked = true;
                    }
                    else
                    {
                        control4.rTypeKeywords.IsChecked = true;
                    }
                    control4.txtLink.Text = this._task.Link;
                    for (int j = 0; j < this._task.Keywords.Count; j++)
                    {
                        control4.txtPositiveKws.AppendText(this._task.Keywords[j]);
                    }
                    if (this._task.SupremeColorPick != TaskObject.SuprimeColorPickEnum.exact)
                    {
                        control4.rColorContains.IsChecked = true;
                    }
                    else
                    {
                        control4.rColorExact.IsChecked = true;
                    }
                    control4.txtColor.Text = this._task.Color;
                    break;
                }
                case TaskObject.PlatformEnum.footsites:
                {
                    base.Height = 500.0;
                    FootSitesControl2 control1 = (FootSitesControl2) this._platformView.content.Content;
                    control1.txtLink.Text = this._task.Link;
                    control1.txtCartSecurityCheck.Text = this._task.VariousStringData;
                    break;
                }
                case TaskObject.PlatformEnum.woodwood:
                {
                    base.Height = 500.0;
                    WoodwoodControl2 control7 = (WoodwoodControl2) this._platformView.content.Content;
                    if (this._task.TaskType == TaskObject.TaskTypeEnum.directlink)
                    {
                        control7.rTypeDirect.IsChecked = true;
                    }
                    else
                    {
                        control7.rTypeKeywords.IsChecked = true;
                    }
                    control7.txtLink.Text = this._task.Link;
                    for (int j = 0; j < this._task.Keywords.Count; j++)
                    {
                        control7.txtPositiveKws.AppendText(this._task.Keywords[j]);
                    }
                    control7.txtColor.Text = this._task.Color;
                    if (this._task.SupremeColorPick == TaskObject.SuprimeColorPickEnum.exact)
                    {
                        control7.rColorExact.IsChecked = true;
                    }
                    else
                    {
                        control7.rColorContains.IsChecked = true;
                    }
                    control7.chPickRandomColorNotAvailable.IsChecked = new bool?(this._task.ColorPickRandom);
                    break;
                }
                case TaskObject.PlatformEnum.hibbett:
                    base.Height = 500.0;
                    ((HibbettControl2) this._platformView.content.Content).txtLink.Text = this._task.Link;
                    break;

                case TaskObject.PlatformEnum.solebox:
                {
                    base.Height = 500.0;
                    SoleboxControl2 control14 = (SoleboxControl2) this._platformView.content.Content;
                    control14.txtUsername.Text = this._task.Username;
                    control14.txtPassword.Text = this._task.Password;
                    control14.txtLink.Text = this._task.Link;
                    if (this._task.Payment != TaskObject.PaymentEnum.creditcard)
                    {
                        if (this._task.Payment != TaskObject.PaymentEnum.paypal)
                        {
                            control14.rPaymentCash.IsChecked = true;
                        }
                        else
                        {
                            control14.radioButton_0.IsChecked = true;
                        }
                        break;
                    }
                    control14.radioButton_1.IsChecked = true;
                    break;
                }
                case TaskObject.PlatformEnum.nordstrom:
                {
                    base.Height = 500.0;
                    NordstromControl2 control13 = (NordstromControl2) this._platformView.content.Content;
                    control13.txtLink.Text = this._task.Link;
                    if (this._task.SupremeColorPick == TaskObject.SuprimeColorPickEnum.contains)
                    {
                        control13.rColorContains.IsChecked = true;
                    }
                    else
                    {
                        control13.rColorExact.IsChecked = true;
                    }
                    control13.txtColor.Text = this._task.Color;
                    control13.chPickRandomColorNotAvailable.IsChecked = new bool?(this._task.ColorPickRandom);
                    if (this._task.TaskType == TaskObject.TaskTypeEnum.directlink)
                    {
                        control13.rTypeDirect.IsChecked = true;
                    }
                    else
                    {
                        control13.rTypeVariant.IsChecked = true;
                    }
                    control13.txtSkuId.Text = this._task.SkuId;
                    control13.txtStyleId.Text = this._task.StyleId;
                    break;
                }
                case TaskObject.PlatformEnum.mesh:
                {
                    base.Height = 500.0;
                    MeshControl2 control16 = (MeshControl2) this._platformView.content.Content;
                    string variousStringData = this._task.VariousStringData;
                    if (variousStringData != "size")
                    {
                        switch (variousStringData)
                        {
                            case "hipstore":
                                control16.rMeshHipStore.IsChecked = true;
                                break;

                            case "jd":
                                control16.rMeshJd.IsChecked = true;
                                break;

                            case "footpatrol":
                                control16.rMeshFootpatrol.IsChecked = true;
                                break;
                        }
                    }
                    else
                    {
                        control16.rMeshSize.IsChecked = true;
                    }
                    if (this._task.AtcMethod != TaskObject.AtcMethodEnum.backend)
                    {
                        control16.rAtcFrontend.IsChecked = true;
                    }
                    else
                    {
                        control16.rAtcBackend.IsChecked = true;
                    }
                    if (this._task.Link.Contains("/") && (this._task.AtcMethod == TaskObject.AtcMethodEnum.backend))
                    {
                        control16.txtLink.Text = this._task.Link.Substring(this._task.Link.IndexOf("/") + 1).Trim();
                    }
                    else
                    {
                        control16.txtLink.Text = this._task.Link;
                    }
                    break;
                }
                case TaskObject.PlatformEnum.mrporter:
                {
                    base.Height = 500.0;
                    MrPorterControl2 control15 = (MrPorterControl2) this._platformView.content.Content;
                    control15.txtLink.Text = this._task.Link;
                    if (this._task.SupremeRegion == TaskObject.SupremeRegionEnum.USA)
                    {
                        control15.rRegionUsa.IsChecked = true;
                    }
                    else
                    {
                        control15.rRegionIntl.IsChecked = true;
                    }
                    control15.txtUsername.Text = this._task.Username;
                    control15.txtPassword.Text = this._task.Password;
                    control15.switchLogin.IsChecked = new bool?(this._task.Login);
                    break;
                }
                case TaskObject.PlatformEnum.holypopstore:
                {
                    base.Height = 500.0;
                    HolypopstoreControl control22 = (HolypopstoreControl) this._platformView.content.Content;
                    control22.txtUsername.Text = this._task.Username;
                    control22.txtPassword.Text = this._task.Password;
                    control22.txtLink.Text = this._task.Link;
                    break;
                }
                case TaskObject.PlatformEnum.backdoor:
                {
                    base.Height = 500.0;
                    BackdoorControl2 control19 = (BackdoorControl2) this._platformView.content.Content;
                    if (this._task.TaskType != TaskObject.TaskTypeEnum.directlink)
                    {
                        control19.rTypeKeywords.IsChecked = true;
                    }
                    else
                    {
                        control19.rTypeDirect.IsChecked = true;
                    }
                    control19.txtLink.Text = this._task.Link;
                    int num7 = 0;
                    while (true)
                    {
                        if (num7 >= this._task.Keywords.Count)
                        {
                            break;
                        }
                        control19.txtPositiveKws.AppendText(this._task.Keywords[num7]);
                        num7++;
                    }
                }
                case TaskObject.PlatformEnum.offwhite:
                {
                    base.Height = 500.0;
                    OffWhiteControl2 control21 = (OffWhiteControl2) this._platformView.content.Content;
                    if (this._task.TaskType != TaskObject.TaskTypeEnum.directlink)
                    {
                        control21.rTypeKeywords.IsChecked = true;
                    }
                    else
                    {
                        control21.rTypeDirect.IsChecked = true;
                    }
                    control21.txtLink.Text = this._task.Link;
                    for (int j = 0; j < this._task.Keywords.Count; j++)
                    {
                        control21.txtPositiveKws.AppendText(this._task.Keywords[j]);
                    }
                    if (this._task.Payment == TaskObject.PaymentEnum.creditcard)
                    {
                        control21.radioButton_0.IsChecked = true;
                    }
                    else
                    {
                        control21.rPaymentBankTransfer.IsChecked = true;
                    }
                    break;
                }
                case TaskObject.PlatformEnum.titolo:
                {
                    base.Height = 500.0;
                    TitoloControl2 control23 = (TitoloControl2) this._platformView.content.Content;
                    control23.txtLink.Text = this._task.Link;
                    control23.txtUsername.Text = this._task.Username;
                    control23.txtPassword.Text = this._task.Password;
                    break;
                }
                case TaskObject.PlatformEnum.funko:
                {
                    base.Height = 500.0;
                    FunkoControl2 control18 = (FunkoControl2) this._platformView.content.Content;
                    if (this._task.TaskType == TaskObject.TaskTypeEnum.directlink)
                    {
                        control18.rTypeDirect.IsChecked = true;
                    }
                    else
                    {
                        control18.rTypeKeywords.IsChecked = true;
                    }
                    control18.txtLink.Text = this._task.Link;
                    int num9 = 0;
                    while (true)
                    {
                        if (num9 >= this._task.Keywords.Count)
                        {
                            break;
                        }
                        control18.txtPositiveKws.AppendText(this._task.Keywords[num9]);
                        num9++;
                    }
                }
                case TaskObject.PlatformEnum.boxlunch:
                {
                    base.Height = 505.0;
                    BoxlunchControl2 control11 = (BoxlunchControl2) this._platformView.content.Content;
                    if (this._task.TaskType == TaskObject.TaskTypeEnum.directlink)
                    {
                        control11.rTypeDirect.IsChecked = true;
                    }
                    else
                    {
                        control11.rTypeKeywords.IsChecked = true;
                    }
                    control11.txtLink.Text = this._task.Link;
                    for (int j = 0; j < this._task.Keywords.Count; j++)
                    {
                        control11.txtPositiveKws.AppendText(this._task.Keywords[j]);
                    }
                    if (this._task.NegativeKeywords.Count > 0)
                    {
                        control11.txtNegativeKeywords.Text = this._task.NegativeKeywords[0];
                    }
                    if (string.IsNullOrEmpty(this._task.VariousStringData2))
                    {
                        control11.txtQuantity.Text = "1";
                    }
                    else
                    {
                        control11.txtQuantity.Text = this._task.VariousStringData2;
                    }
                    control11.txtCouponCode.Text = this._task.VariousStringData;
                    control11.switchLogin.IsChecked = new bool?(this._task.Login);
                    control11.txtUsername.Text = this._task.Username;
                    control11.txtPassword.Text = this._task.Password;
                    break;
                }
                case TaskObject.PlatformEnum.puma:
                {
                    base.Height = 500.0;
                    PumaControl2 control8 = (PumaControl2) this._platformView.content.Content;
                    control8.txtLink.Text = this._task.Link;
                    control8.txtColor.Text = this._task.Color;
                    control8.chPickRandomColorNotAvailable.IsChecked = new bool?(this._task.ColorPickRandom);
                    if (this._task.SupremeColorPick != TaskObject.SuprimeColorPickEnum.contains)
                    {
                        control8.rColorExact.IsChecked = true;
                        break;
                    }
                    control8.rColorContains.IsChecked = true;
                    break;
                }
                case TaskObject.PlatformEnum.converse:
                {
                    base.Height = 500.0;
                    ConverseControl2 control2 = (ConverseControl2) this._platformView.content.Content;
                    if (this._task.TaskType == TaskObject.TaskTypeEnum.directlink)
                    {
                        control2.rTypeDirect.IsChecked = true;
                    }
                    else
                    {
                        control2.rTypeKeywords.IsChecked = true;
                    }
                    control2.txtLink.Text = this._task.Link;
                    for (int j = 0; j < this._task.Keywords.Count; j++)
                    {
                        control2.txtPositiveKws.AppendText(this._task.Keywords[j]);
                    }
                    control2.txtColor.Text = this._task.Color;
                    control2.chPickRandomColorNotAvailable.IsChecked = new bool?(this._task.ColorPickRandom);
                    if (this._task.SupremeColorPick == TaskObject.SuprimeColorPickEnum.contains)
                    {
                        control2.rColorContains.IsChecked = true;
                    }
                    else
                    {
                        control2.rColorExact.IsChecked = true;
                    }
                    break;
                }
                case TaskObject.PlatformEnum.footlockerau:
                {
                    base.Height = 500.0;
                    FootlockerauControl2 control12 = (FootlockerauControl2) this._platformView.content.Content;
                    control12.txtLink.Text = this._task.Link;
                    if (this._task.Payment != TaskObject.PaymentEnum.creditcard)
                    {
                        control12.rPaymentCCMan.IsChecked = true;
                        break;
                    }
                    control12.radioButton_0.IsChecked = true;
                    break;
                }
                case TaskObject.PlatformEnum.footlockereu:
                {
                    base.Height = 500.0;
                    FootlockereuControl control17 = (FootlockereuControl) this._platformView.content.Content;
                    control17.txtLink.Text = this._task.Link;
                    if (this._task.Payment == TaskObject.PaymentEnum.creditcard)
                    {
                        control17.radioButton_0.IsChecked = true;
                    }
                    else
                    {
                        control17.rPaymentCCMan.IsChecked = true;
                    }
                    if (this._task.TaskType != TaskObject.TaskTypeEnum.directlink)
                    {
                        control17.rTypeKeywords.IsChecked = true;
                    }
                    else
                    {
                        control17.rTypeDirect.IsChecked = true;
                    }
                    control17.txtLink.Text = this._task.Link;
                    for (int j = 0; j < this._task.Keywords.Count; j++)
                    {
                        control17.txtPositiveKws.AppendText(this._task.Keywords[j]);
                    }
                    using (enumerator = ((IEnumerable) control17.cmbRegion.Items).GetEnumerator())
                    {
                        ComboBoxItem item8;
                        goto Label_1CFB;
                    Label_1CCF:
                        item8 = (ComboBoxItem) enumerator.Current;
                        if (item8.Tag.ToString() == this._task.Various)
                        {
                            goto Label_1D06;
                        }
                    Label_1CFB:
                        if (!enumerator.MoveNext())
                        {
                            break;
                        }
                        goto Label_1CCF;
                    Label_1D06:
                        control17.cmbRegion.SelectedItem = item8;
                        break;
                    }
                }
                case TaskObject.PlatformEnum.footaction:
                {
                    base.Height = 500.0;
                    FootactionControl control24 = (FootactionControl) this._platformView.content.Content;
                    control24.txtLink.Text = this._task.Link;
                    control24.chBypassCaptcha.IsChecked = new bool?(!this._task.SolveCaptcha);
                    break;
                }
                case TaskObject.PlatformEnum.footlocker:
                {
                    base.Height = 500.0;
                    FootlockerControl control25 = (FootlockerControl) this._platformView.content.Content;
                    control25.txtLink.Text = this._task.Link;
                    control25.chBypassCaptcha.IsChecked = new bool?(!this._task.SolveCaptcha);
                    break;
                }
                case TaskObject.PlatformEnum.supremeinstore:
                {
                    base.Height = 500.0;
                    SupremeInstoreControl2 control10 = (SupremeInstoreControl2) this._platformView.content.Content;
                    if (this._task.SupremeRegion != TaskObject.SupremeRegionEnum.EU)
                    {
                        control10.rRegionUsa.IsChecked = true;
                    }
                    else
                    {
                        control10.rRegionEu.IsChecked = true;
                    }
                    if (this._task.VariousStringData == "brooklyn")
                    {
                        control10.rLocBrooklyn.IsChecked = true;
                    }
                    else if (this._task.VariousStringData != "manhattan")
                    {
                        control10.rLocLA.IsChecked = true;
                    }
                    else
                    {
                        control10.rLocManhattan.IsChecked = true;
                    }
                    break;
                }
                case TaskObject.PlatformEnum.mcm:
                {
                    base.Height = 500.0;
                    McmControl2 control5 = (McmControl2) this._platformView.content.Content;
                    control5.txtLink.Text = this._task.Link;
                    control5.txtColor.Text = this._task.Color;
                    control5.chPickRandomColorNotAvailable.IsChecked = new bool?(this._task.ColorPickRandom);
                    if (this._task.SupremeColorPick == TaskObject.SuprimeColorPickEnum.contains)
                    {
                        control5.rColorContains.IsChecked = true;
                    }
                    else
                    {
                        control5.rColorExact.IsChecked = true;
                    }
                    if (this._task.TaskType == TaskObject.TaskTypeEnum.directlink)
                    {
                        control5.rTypeDirect.IsChecked = true;
                    }
                    else
                    {
                        control5.rTypeKeywords.IsChecked = true;
                    }
                    control5.txtLink.Text = this._task.Link;
                    for (int j = 0; j < this._task.Keywords.Count; j++)
                    {
                        control5.txtPositiveKws.AppendText(this._task.Keywords[j]);
                    }
                    break;
                }
                case TaskObject.PlatformEnum.sevres:
                {
                    base.Height = 505.0;
                    SevresControl control = (SevresControl) this._platformView.content.Content;
                    if (this._task.TaskType == TaskObject.TaskTypeEnum.directlink)
                    {
                        control.rTypeDirect.IsChecked = true;
                    }
                    else
                    {
                        control.rTypeKeywords.IsChecked = true;
                    }
                    control.txtLink.Text = this._task.Link;
                    for (int j = 0; j < this._task.Keywords.Count; j++)
                    {
                        control.txtPositiveKws.AppendText(this._task.Keywords[j]);
                    }
                    if (this._task.NegativeKeywords.Count > 0)
                    {
                        control.txtNegativeKeywords.Text = this._task.NegativeKeywords[0];
                    }
                    break;
                }
                case TaskObject.PlatformEnum.finishline:
                {
                    base.Height = 500.0;
                    FinishlineControl control9 = (FinishlineControl) this._platformView.content.Content;
                    if (this._task.TaskType == TaskObject.TaskTypeEnum.directlink)
                    {
                        control9.rTypeDirect.IsChecked = true;
                    }
                    else
                    {
                        control9.rTypeKeywords.IsChecked = true;
                    }
                    control9.txtLink.Text = this._task.Link;
                    for (int j = 0; j < this._task.Keywords.Count; j++)
                    {
                        control9.txtPositiveKws.AppendText(this._task.Keywords[j]);
                    }
                    control9.txtColor.Text = this._task.Color;
                    if (this._task.SupremeColorPick != TaskObject.SuprimeColorPickEnum.exact)
                    {
                        control9.rColorContains.IsChecked = true;
                    }
                    else
                    {
                        control9.rColorExact.IsChecked = true;
                    }
                    control9.chPickRandomColorNotAvailable.IsChecked = new bool?(this._task.ColorPickRandom);
                    break;
                }
                case TaskObject.PlatformEnum.hottopic:
                {
                    base.Height = 505.0;
                    HottopicControl control20 = (HottopicControl) this._platformView.content.Content;
                    if (this._task.TaskType == TaskObject.TaskTypeEnum.directlink)
                    {
                        control20.rTypeDirect.IsChecked = true;
                    }
                    else
                    {
                        control20.rTypeKeywords.IsChecked = true;
                    }
                    control20.txtLink.Text = this._task.Link;
                    for (int j = 0; j < this._task.Keywords.Count; j++)
                    {
                        control20.txtPositiveKws.AppendText(this._task.Keywords[j]);
                    }
                    if (this._task.NegativeKeywords.Count > 0)
                    {
                        control20.txtNegativeKeywords.Text = this._task.NegativeKeywords[0];
                    }
                    if (string.IsNullOrEmpty(this._task.VariousStringData2))
                    {
                        control20.txtQuantity.Text = "1";
                    }
                    else
                    {
                        control20.txtQuantity.Text = this._task.VariousStringData2;
                    }
                    control20.txtCouponCode.Text = this._task.VariousStringData;
                    control20.switchLogin.IsChecked = new bool?(this._task.Login);
                    control20.txtUsername.Text = this._task.Username;
                    control20.txtPassword.Text = this._task.Password;
                    break;
                }
            }
            goto Label_2480;
        Label_08AC:
            control3.switchDeepSearch.IsChecked = new bool?(this._task.DeepSearch);
            if ((this._task.DeepSearchLinks >= 1) && (this._task.DeepSearchLinks <= 100))
            {
                control3.txtDeepSearchLinks.Text = this._task.DeepSearchLinks.ToString();
            }
            else
            {
                control3.txtDeepSearchLinks.Text = "5";
            }
            if (this._task.IsShopifyCheckoutLink)
            {
                ComboBoxItem newItem = new ComboBoxItem {
                    Content = this._task.Link
                };
                List<string> list1 = new List<string> {
                    this._task.ShopifyCheckoutLink
                };
                newItem.Tag = list1;
                control3.cmbCheckoutlink.Items.Add(newItem);
                control3.cmbCheckoutlink.SelectedIndex = 0;
            }
            if (this._task.Payment == TaskObject.PaymentEnum.creditcard)
            {
                control3.radioButton_0.IsChecked = true;
            }
            else
            {
                control3.radioButton_1.IsChecked = true;
            }
            control3.chPickRandomColorNotAvailable.IsChecked = new bool?(this._task.ColorPickRandom);
            control3.txtColor.Text = this._task.Color;
            if (this._task.SupremeColorPick == TaskObject.SuprimeColorPickEnum.exact)
            {
                control3.rColorExact.IsChecked = true;
            }
            else
            {
                control3.rColorContains.IsChecked = true;
            }
            using (enumerator = ((IEnumerable) control3.cmbParentTask.Items).GetEnumerator())
            {
                ComboBoxItem current;
                while (enumerator.MoveNext())
                {
                    current = (ComboBoxItem) enumerator.Current;
                    if (this._task.ParentId == current.Tag.ToString())
                    {
                        goto Label_0A60;
                    }
                }
                goto Label_2480;
            Label_0A60:
                control3.cmbParentTask.SelectedItem = current;
                goto Label_2480;
            }
        Label_0A88:
            base.Height = 600.0;
            SupremeControl2 content = (SupremeControl2) this._platformView.content.Content;
            content.txtLink.Text = this._task.Link;
            content.txtColor.Text = this._task.Color;
            if (this._task.SupremeColorPick != TaskObject.SuprimeColorPickEnum.exact)
            {
                content.rSizeContains.IsChecked = true;
            }
            else
            {
                content.rSizeExact.IsChecked = true;
            }
            using (enumerator = ((IEnumerable) content.cmbGroup.Items).GetEnumerator())
            {
                ComboBoxItem current;
                while (enumerator.MoveNext())
                {
                    current = (ComboBoxItem) enumerator.Current;
                    if (current.Tag.ToString() == this._task.Group)
                    {
                        goto Label_0B5C;
                    }
                }
                goto Label_0B81;
            Label_0B5C:
                content.cmbGroup.SelectedItem = current;
            }
        Label_0B81:
            num15 = 0;
            while (num15 < this._task.Keywords.Count)
            {
                content.txtPositiveKws.AppendText(this._task.Keywords[num15]);
                num15++;
            }
            for (int i = 0; i < this._task.NegativeKeywords.Count; i++)
            {
                content.txtNegativeKws.AppendText(this._task.NegativeKeywords[i]);
            }
            if (this._task.TaskType != TaskObject.TaskTypeEnum.directlink)
            {
                if (this._task.TaskType != TaskObject.TaskTypeEnum.keywords)
                {
                    content.rTypeManualPicker.IsChecked = true;
                }
                else
                {
                    content.rTypeKeywords.IsChecked = true;
                }
            }
            else
            {
                content.rTypeDirect.IsChecked = true;
            }
            if (this._task.SupremeRegion != TaskObject.SupremeRegionEnum.USA)
            {
                if (this._task.SupremeRegion == TaskObject.SupremeRegionEnum.EU)
                {
                    content.rRegionEu.IsChecked = true;
                }
                else
                {
                    content.rRegionJapan.IsChecked = true;
                }
            }
            else
            {
                content.rRegionUsa.IsChecked = true;
            }
            using (enumerator = ((IEnumerable) content.cmbParentTask.Items).GetEnumerator())
            {
                ComboBoxItem current;
                while (enumerator.MoveNext())
                {
                    current = (ComboBoxItem) enumerator.Current;
                    if (this._task.ParentId == current.Tag.ToString())
                    {
                        goto Label_0CF2;
                    }
                }
                goto Label_0D17;
            Label_0CF2:
                content.cmbParentTask.SelectedItem = current;
            }
        Label_0D17:
            if (this._task.SupremeAutomation == TaskObject.SuprimeAutomationEnum.browserless)
            {
                content.rAutomationBrowserless.IsChecked = true;
            }
            else
            {
                content.rAutomationBrowser.IsChecked = true;
            }
            content.txtGmailUsername.Text = this._task.GmailEmail;
            content.txtGmailPassword.Text = this._task.GmailPassword;
            content.chPickRandomColorNotAvailable.IsChecked = new bool?(this._task.ColorPickRandom);
            content.chSolveCaptcha.IsChecked = new bool?(this._task.SolveCaptcha);
            if (this._task.Payment == TaskObject.PaymentEnum.creditcard)
            {
                content.radioButton_0.IsChecked = true;
            }
            else
            {
                content.rPaymentCash.IsChecked = true;
            }
        Label_2480:
            this.txtTaskName.Focus();
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly TaskWindow2.<>c <>9;
            public static Func<LoginPool, bool> <>9__21_3;
            public static Func<LoginPool, bool> <>9__21_4;
            public static Func<LoginPool, bool> <>9__21_5;
            public static Func<LoginPool, bool> <>9__21_6;
            public static Func<LoginPool, bool> <>9__21_7;
            public static Func<LoginPool, bool> <>9__21_8;
            public static Func<LoginPool, bool> <>9__21_9;
            public static Func<LoginPool, bool> <>9__21_10;

            static <>c()
            {
                Class7.RIuqtBYzWxthF();
                <>9 = new TaskWindow2.<>c();
            }

            public <>c()
            {
                Class7.RIuqtBYzWxthF();
            }

            internal bool <btnSave_Click>b__21_10(LoginPool x) => 
                (x.Website == LoginPoolEnum.Titolo);

            internal bool <btnSave_Click>b__21_3(LoginPool x) => 
                (x.Website == LoginPoolEnum.Undefeated);

            internal bool <btnSave_Click>b__21_4(LoginPool x) => 
                (x.Website == LoginPoolEnum.Undefeated);

            internal bool <btnSave_Click>b__21_5(LoginPool x) => 
                (x.Website == LoginPoolEnum.Solebox);

            internal bool <btnSave_Click>b__21_6(LoginPool x) => 
                (x.Website == LoginPoolEnum.Solebox);

            internal bool <btnSave_Click>b__21_7(LoginPool x) => 
                (x.Website == LoginPoolEnum.Holypopstore);

            internal bool <btnSave_Click>b__21_8(LoginPool x) => 
                (x.Website == LoginPoolEnum.Holypopstore);

            internal bool <btnSave_Click>b__21_9(LoginPool x) => 
                (x.Website == LoginPoolEnum.Titolo);
        }
    }
}

