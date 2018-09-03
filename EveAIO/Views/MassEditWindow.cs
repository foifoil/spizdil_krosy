namespace EveAIO.Views
{
    using EveAIO;
    using EveAIO.Pocos;
    using EveAIO.Views.MassEdit;
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Markup;
    using System.Windows.Media;

    public class MassEditWindow : Window, IComponentConnector
    {
        private List<string> _ids;
        private TaskObject.PlatformEnum _platform;
        internal Button BtnClose;
        public ToggleButton switchProfile;
        internal TextBlock lblProfile;
        internal ComboBox cmbCheckoutProfile;
        public ToggleButton switchProxyList;
        internal TextBlock lblProxyList;
        internal ComboBox cmbProxyList;
        internal ToggleButton switchSize;
        internal TextBlock lblSize;
        internal TextBox txtSize;
        internal CheckBox chSizeRandom;
        internal CheckBox chPickRandomNotAvailable;
        internal ToggleButton switchPriceCheck;
        internal TextBlock lblMinimum;
        internal TextBox txtPriceCheckMin;
        internal TextBlock lblMaximum;
        internal TextBox txtPriceCheckMax;
        internal ToggleButton switchDelay;
        internal RadioButton chDelayExact;
        internal RadioButton chDelayRandom;
        internal TextBlock lblDelay;
        internal TextBox txtDelay;
        internal TextBlock lblFrom;
        internal TextBox txtDelayFrom;
        internal TextBlock lblTo;
        internal TextBox txtDelayTo;
        public ToggleButton switchCaptcha;
        internal TextBlock lblCaptcha;
        internal TextBox txtCaptchaRequests;
        public ToggleButton switchNotifications;
        internal CheckBox chNotify;
        internal ContentPresenter content;
        internal Button btnSave;
        internal Button btnCancel;
        private bool _contentLoaded;

        public MassEditWindow(Window owner, List<string> ids, TaskObject.PlatformEnum platform)
        {
            Class7.RIuqtBYzWxthF();
            this.InitializeComponent();
            base.Owner = owner;
            this._ids = ids;
            this._platform = platform;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            base.Close();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            base.Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!this.Check())
            {
                return;
            }
            using (List<string>.Enumerator enumerator = this._ids.GetEnumerator())
            {
                BoxlunchMassEditControl content;
            Label_0017:
                if (!enumerator.MoveNext())
                {
                    goto Label_1425;
                }
                string id = enumerator.Current;
                TaskObject obj2 = Global.SETTINGS.TASKS.First<TaskObject>(x => x.Id == id);
                if (this.switchProfile.IsChecked.HasValue && this.switchProfile.IsChecked.Value)
                {
                    obj2.CheckoutId = ((ComboBoxItem) this.cmbCheckoutProfile.SelectedItem).Tag.ToString();
                }
                if (this.switchProxyList.IsChecked.HasValue && this.switchProxyList.IsChecked.Value)
                {
                    obj2.ProxyListId = ((this.cmbProxyList.SelectedItem == null) || (((ComboBoxItem) this.cmbProxyList.SelectedItem).Tag.ToString() == "-1")) ? "" : ((ComboBoxItem) this.cmbProxyList.SelectedItem).Tag.ToString();
                }
                if (this.switchSize.IsChecked.HasValue && this.switchSize.IsChecked.Value)
                {
                    obj2.Size = this.txtSize.Text.Trim();
                    obj2.RandomSize = !this.chSizeRandom.IsChecked.HasValue ? false : this.chSizeRandom.IsChecked.Value;
                    obj2.SizePickRandom = !this.chPickRandomNotAvailable.IsChecked.HasValue ? false : this.chPickRandomNotAvailable.IsChecked.Value;
                }
                if (this.switchPriceCheck.IsChecked.HasValue && this.switchPriceCheck.IsChecked.Value)
                {
                    obj2.PriceCheck = !this.switchPriceCheck.IsChecked.HasValue ? false : this.switchPriceCheck.IsChecked.Value;
                    obj2.MinimumPrice = string.IsNullOrEmpty(this.txtPriceCheckMin.Text.Trim()) ? 0 : int.Parse(this.txtPriceCheckMin.Text.Trim());
                    obj2.MaximumPrice = string.IsNullOrEmpty(this.txtPriceCheckMax.Text.Trim()) ? 0 : int.Parse(this.txtPriceCheckMax.Text.Trim());
                }
                if (this.switchDelay.IsChecked.HasValue && this.switchDelay.IsChecked.Value)
                {
                    obj2.RetryDelay = (!this.chDelayExact.IsChecked.HasValue || !this.chDelayExact.IsChecked.Value) ? TaskObject.RetryDelayEnum.random : TaskObject.RetryDelayEnum.exact;
                    obj2.Delay = this.txtDelay.Text.Trim();
                    obj2.DelayFrom = this.txtDelayFrom.Text.Trim();
                    obj2.DelayTo = this.txtDelayTo.Text.Trim();
                    obj2.DelayChanged = true;
                }
                if (this.switchCaptcha.IsChecked.HasValue && this.switchCaptcha.IsChecked.Value)
                {
                    obj2.CaptchaRequests = string.IsNullOrEmpty(this.txtCaptchaRequests.Text.Trim()) ? 0 : int.Parse(this.txtCaptchaRequests.Text.Trim());
                }
                if (this.switchNotifications.IsChecked.HasValue && this.switchNotifications.IsChecked.Value)
                {
                    obj2.Notify = !this.chNotify.IsChecked.HasValue ? false : this.chNotify.IsChecked.Value;
                }
                TaskObject.PlatformEnum enum2 = this._platform;
                if (enum2 <= TaskObject.PlatformEnum.supreme)
                {
                    if (enum2 == TaskObject.PlatformEnum.shopify)
                    {
                        ShopifyMassEditControl content = (ShopifyMassEditControl) this.content.Content;
                        if (content.switchCheckout.IsChecked.HasValue && content.switchCheckout.IsChecked.Value)
                        {
                            if (content.radioButton_0.IsChecked.HasValue && content.radioButton_0.IsChecked.Value)
                            {
                                obj2.Payment = TaskObject.PaymentEnum.creditcard;
                            }
                            else
                            {
                                obj2.Payment = TaskObject.PaymentEnum.paypal;
                            }
                        }
                        if (content.switchAtc.IsChecked.HasValue && content.switchAtc.IsChecked.Value)
                        {
                            if (content.rAtcBackend.IsChecked.HasValue && content.rAtcBackend.IsChecked.Value)
                            {
                                obj2.AtcMethod = TaskObject.AtcMethodEnum.backend;
                            }
                            else
                            {
                                obj2.AtcMethod = TaskObject.AtcMethodEnum.frontend;
                            }
                        }
                        if (content.switchLogin.IsChecked.HasValue && content.switchLogin.IsChecked.Value)
                        {
                            obj2.Login = !content.switchLogin.IsChecked.HasValue ? false : content.switchLogin.IsChecked.Value;
                            obj2.Username = content.txtUsername.Text.Trim();
                            obj2.Password = content.txtPassword.Text.Trim();
                        }
                        if (content.switchTaskType.IsChecked.HasValue && content.switchTaskType.IsChecked.Value)
                        {
                            if (!string.IsNullOrEmpty(content.txtLink.Text.Trim()))
                            {
                                obj2.Link = content.txtLink.Text.Trim();
                            }
                            if (!string.IsNullOrEmpty(content.txtVariant.Text.Trim()))
                            {
                                obj2.Variant = content.txtVariant.Text.Trim();
                            }
                            obj2.Keywords.Clear();
                            obj2.PositiveKeywordsChange();
                            if (!string.IsNullOrEmpty(content.txtPositiveKws.Text.Trim()))
                            {
                                for (int i = 0; i < content.txtPositiveKws.LineCount; i++)
                                {
                                    string lineText = content.txtPositiveKws.GetLineText(i);
                                    if (!string.IsNullOrEmpty(lineText) && (lineText != "\n"))
                                    {
                                        obj2.Keywords.Add(lineText);
                                    }
                                }
                            }
                            obj2.NegativeKeywords.Clear();
                            obj2.NegativeKeywordsChange();
                            if (!string.IsNullOrEmpty(content.txtNegativeKws.Text.Trim()))
                            {
                                for (int i = 0; i < content.txtNegativeKws.LineCount; i++)
                                {
                                    string lineText = content.txtNegativeKws.GetLineText(i);
                                    if (!string.IsNullOrEmpty(lineText) && (lineText != "\n"))
                                    {
                                        obj2.NegativeKeywords.Add(lineText);
                                    }
                                }
                            }
                            if (content.rTypeDirect.IsChecked.HasValue && content.rTypeDirect.IsChecked.Value)
                            {
                                obj2.TaskType = TaskObject.TaskTypeEnum.directlink;
                            }
                            else if (content.rTypeKeywords.IsChecked.HasValue && content.rTypeKeywords.IsChecked.Value)
                            {
                                obj2.TaskType = TaskObject.TaskTypeEnum.keywords;
                            }
                            else
                            {
                                obj2.TaskType = TaskObject.TaskTypeEnum.variant;
                            }
                        }
                    }
                    else if (enum2 == TaskObject.PlatformEnum.supreme)
                    {
                        SupremeMassEditControl content = (SupremeMassEditControl) this.content.Content;
                        if (content.switchTaskStype.IsChecked.HasValue && content.switchTaskStype.IsChecked.Value)
                        {
                            if (!string.IsNullOrEmpty(content.txtLink.Text.Trim()))
                            {
                                obj2.Link = content.txtLink.Text.Trim();
                            }
                            obj2.Color = content.txtColor.Text.Trim();
                            obj2.Group = ((ComboBoxItem) content.cmbGroup.SelectedItem).Tag.ToString();
                            if (content.rSizeExact.IsChecked.HasValue && content.rSizeExact.IsChecked.Value)
                            {
                                obj2.SupremeColorPick = TaskObject.SuprimeColorPickEnum.exact;
                            }
                            else
                            {
                                obj2.SupremeColorPick = TaskObject.SuprimeColorPickEnum.contains;
                            }
                            obj2.Keywords.Clear();
                            obj2.PositiveKeywordsChange();
                            if (!string.IsNullOrEmpty(content.txtPositiveKws.Text.Trim()))
                            {
                                for (int i = 0; i < content.txtPositiveKws.LineCount; i++)
                                {
                                    string lineText = content.txtPositiveKws.GetLineText(i);
                                    if (!string.IsNullOrEmpty(lineText) && (lineText != "\n"))
                                    {
                                        obj2.Keywords.Add(lineText);
                                    }
                                }
                            }
                            obj2.NegativeKeywords.Clear();
                            obj2.NegativeKeywordsChange();
                            if (!string.IsNullOrEmpty(content.txtNegativeKws.Text.Trim()))
                            {
                                for (int i = 0; i < content.txtNegativeKws.LineCount; i++)
                                {
                                    string lineText = content.txtNegativeKws.GetLineText(i);
                                    if (!string.IsNullOrEmpty(lineText) && (lineText != "\n"))
                                    {
                                        obj2.NegativeKeywords.Add(lineText);
                                    }
                                }
                            }
                            if (content.rTypeDirect.IsChecked.HasValue && content.rTypeDirect.IsChecked.Value)
                            {
                                obj2.TaskType = TaskObject.TaskTypeEnum.directlink;
                            }
                            else
                            {
                                obj2.TaskType = TaskObject.TaskTypeEnum.keywords;
                            }
                        }
                    }
                    goto Label_0017;
                }
                switch (enum2)
                {
                    case TaskObject.PlatformEnum.hibbett:
                    {
                        HibbettMassEditControl content = (HibbettMassEditControl) this.content.Content;
                        if (content.switchDirectLink.IsChecked.HasValue && content.switchDirectLink.IsChecked.Value)
                        {
                            obj2.Link = content.txtLink.Text.Trim();
                        }
                        goto Label_0017;
                    }
                    case TaskObject.PlatformEnum.solebox:
                    {
                        SoleboxMassEditControl content = (SoleboxMassEditControl) this.content.Content;
                        if (content.switchLogin.IsChecked.HasValue && content.switchLogin.IsChecked.Value)
                        {
                            obj2.Login = true;
                            obj2.Username = content.txtUsername.Text.Trim();
                            obj2.Password = content.txtPassword.Text.Trim();
                        }
                        if (content.switchDirectLink.IsChecked.HasValue && content.switchDirectLink.IsChecked.Value)
                        {
                            obj2.Link = content.txtLink.Text.Trim();
                        }
                        if (content.switchPayment.IsChecked.HasValue && content.switchPayment.IsChecked.Value)
                        {
                            if (content.radioButton_0.IsChecked.HasValue && content.radioButton_0.IsChecked.Value)
                            {
                                obj2.Payment = TaskObject.PaymentEnum.creditcard;
                            }
                            else
                            {
                                obj2.Payment = TaskObject.PaymentEnum.paypal;
                            }
                        }
                        goto Label_0017;
                    }
                    case TaskObject.PlatformEnum.nordstrom:
                    case TaskObject.PlatformEnum.mesh:
                    case TaskObject.PlatformEnum.holypopstore:
                    case TaskObject.PlatformEnum.barneys:
                        goto Label_0017;

                    case TaskObject.PlatformEnum.mrporter:
                    {
                        MrPorterMassEditControl content = (MrPorterMassEditControl) this.content.Content;
                        if (content.switchDirectLink.IsChecked.HasValue && content.switchDirectLink.IsChecked.Value)
                        {
                            obj2.Link = content.txtLink.Text.Trim();
                            if (!content.rRegionUsa.IsChecked.HasValue || !content.rRegionUsa.IsChecked.Value)
                            {
                                goto Label_10F6;
                            }
                            obj2.SupremeRegion = TaskObject.SupremeRegionEnum.USA;
                        }
                        goto Label_0017;
                    }
                    case TaskObject.PlatformEnum.backdoor:
                    {
                        BackdoorMassEditControl content = (BackdoorMassEditControl) this.content.Content;
                        if (content.switchTaskType.IsChecked.HasValue && content.switchTaskType.IsChecked.Value)
                        {
                            obj2.Link = "";
                            obj2.Keywords.Clear();
                            obj2.PositiveKeywordsChange();
                            if (!string.IsNullOrEmpty(content.txtLink.Text.Trim()))
                            {
                                obj2.Link = content.txtLink.Text.Trim();
                            }
                            if (!string.IsNullOrEmpty(content.txtPositiveKws.Text.Trim()))
                            {
                                for (int i = 0; i < content.txtPositiveKws.LineCount; i++)
                                {
                                    string lineText = content.txtPositiveKws.GetLineText(i);
                                    if (!string.IsNullOrEmpty(lineText) && (lineText != "\n"))
                                    {
                                        obj2.Keywords.Add(lineText);
                                    }
                                }
                            }
                            if (content.rTypeDirect.IsChecked.HasValue && content.rTypeDirect.IsChecked.Value)
                            {
                                obj2.TaskType = TaskObject.TaskTypeEnum.directlink;
                            }
                            else
                            {
                                obj2.TaskType = TaskObject.TaskTypeEnum.keywords;
                            }
                        }
                        goto Label_0017;
                    }
                    case TaskObject.PlatformEnum.offwhite:
                    {
                        OffWhiteMassEditControl content = (OffWhiteMassEditControl) this.content.Content;
                        if (content.switchTaskType.IsChecked.HasValue && content.switchTaskType.IsChecked.Value)
                        {
                            obj2.Link = "";
                            obj2.PositiveKeywordsChange();
                            obj2.Keywords.Clear();
                            if (!string.IsNullOrEmpty(content.txtLink.Text.Trim()))
                            {
                                obj2.Link = content.txtLink.Text.Trim();
                            }
                            if (!string.IsNullOrEmpty(content.txtPositiveKws.Text.Trim()))
                            {
                                for (int i = 0; i < content.txtPositiveKws.LineCount; i++)
                                {
                                    string lineText = content.txtPositiveKws.GetLineText(i);
                                    if (!string.IsNullOrEmpty(lineText) && (lineText != "\n"))
                                    {
                                        obj2.Keywords.Add(lineText);
                                    }
                                }
                            }
                            if (content.rTypeDirect.IsChecked.HasValue && content.rTypeDirect.IsChecked.Value)
                            {
                                obj2.TaskType = TaskObject.TaskTypeEnum.directlink;
                            }
                            else
                            {
                                obj2.TaskType = TaskObject.TaskTypeEnum.keywords;
                            }
                        }
                        if (content.switchPayment.IsChecked.HasValue && content.switchPayment.IsChecked.Value)
                        {
                            if (content.radioButton_0.IsChecked.HasValue && content.radioButton_0.IsChecked.Value)
                            {
                                obj2.Payment = TaskObject.PaymentEnum.creditcard;
                            }
                            else
                            {
                                obj2.Payment = TaskObject.PaymentEnum.bankTransfer;
                            }
                        }
                        goto Label_0017;
                    }
                    case TaskObject.PlatformEnum.sneakersnstuff:
                    {
                        SneakersnstuffMassEditControl content = (SneakersnstuffMassEditControl) this.content.Content;
                        if (content.switchLogin.IsChecked.HasValue && content.switchLogin.IsChecked.Value)
                        {
                            obj2.Login = !content.switchLogin.IsChecked.HasValue ? false : content.switchLogin.IsChecked.Value;
                            obj2.Username = content.txtUsername.Text.Trim();
                            obj2.Password = content.txtPassword.Text.Trim();
                        }
                        if (content.switchTaskType.IsChecked.HasValue && content.switchTaskType.IsChecked.Value)
                        {
                            if (!string.IsNullOrEmpty(content.txtLink.Text.Trim()))
                            {
                                obj2.Link = content.txtLink.Text.Trim();
                            }
                            obj2.Color = content.txtColor.Text.Trim();
                            if (content.rColorExact.IsChecked.HasValue && content.rColorExact.IsChecked.Value)
                            {
                                obj2.SupremeColorPick = TaskObject.SuprimeColorPickEnum.exact;
                            }
                            else
                            {
                                obj2.SupremeColorPick = TaskObject.SuprimeColorPickEnum.contains;
                            }
                            obj2.Keywords.Clear();
                            obj2.PositiveKeywordsChange();
                            if (!string.IsNullOrEmpty(content.txtPositiveKws.Text.Trim()))
                            {
                                for (int i = 0; i < content.txtPositiveKws.LineCount; i++)
                                {
                                    string lineText = content.txtPositiveKws.GetLineText(i);
                                    if (!string.IsNullOrEmpty(lineText) && (lineText != "\n"))
                                    {
                                        obj2.Keywords.Add(lineText);
                                    }
                                }
                            }
                            if (content.rTypeDirect.IsChecked.HasValue && content.rTypeDirect.IsChecked.Value)
                            {
                                obj2.TaskType = TaskObject.TaskTypeEnum.directlink;
                            }
                            else
                            {
                                obj2.TaskType = TaskObject.TaskTypeEnum.keywords;
                            }
                        }
                        goto Label_0017;
                    }
                    default:
                        if (enum2 != TaskObject.PlatformEnum.boxlunch)
                        {
                            goto Label_0017;
                        }
                        content = (BoxlunchMassEditControl) this.content.Content;
                        if (content.switchCoupon.IsChecked.HasValue && content.switchCoupon.IsChecked.Value)
                        {
                            obj2.VariousStringData = content.txtCouponCode.Text.Trim();
                        }
                        if (content.switchQuantity.IsChecked.HasValue && content.switchQuantity.IsChecked.Value)
                        {
                            try
                            {
                                int num9 = int.Parse(content.txtQuantity.Text);
                                if ((num9 > 0) && (num9 <= 10))
                                {
                                    obj2.VariousStringData2 = num9.ToString();
                                }
                                else
                                {
                                    obj2.VariousStringData2 = "1";
                                }
                            }
                            catch
                            {
                                obj2.VariousStringData2 = "1";
                            }
                        }
                        goto Label_13F7;
                }
            Label_0CE8:
                if (content.switchLogin.IsChecked.Value)
                {
                    obj2.Login = !content.switchLogin.IsChecked.HasValue ? false : content.switchLogin.IsChecked.Value;
                    obj2.Username = content.txtUsername.Text.Trim();
                    obj2.Password = content.txtPassword.Text.Trim();
                }
            Label_0D62:
                if (content.switchTaskType.IsChecked.HasValue && content.switchTaskType.IsChecked.Value)
                {
                    if (!string.IsNullOrEmpty(content.txtLink.Text.Trim()))
                    {
                        obj2.Link = content.txtLink.Text.Trim();
                    }
                    obj2.Keywords.Clear();
                    obj2.PositiveKeywordsChange();
                    if (!string.IsNullOrEmpty(content.txtPositiveKws.Text.Trim()))
                    {
                        for (int i = 0; i < content.txtPositiveKws.LineCount; i++)
                        {
                            string lineText = content.txtPositiveKws.GetLineText(i);
                            if (!string.IsNullOrEmpty(lineText) && (lineText != "\n"))
                            {
                                obj2.Keywords.Add(lineText);
                            }
                        }
                    }
                    obj2.NegativeKeywords.Clear();
                    obj2.NegativeKeywordsChange();
                    if (!string.IsNullOrEmpty(content.txtNegativeKws.Text.Trim()))
                    {
                        obj2.NegativeKeywords.Add(content.txtNegativeKws.Text.Trim());
                    }
                    if (content.rTypeDirect.IsChecked.HasValue && content.rTypeDirect.IsChecked.Value)
                    {
                        obj2.TaskType = TaskObject.TaskTypeEnum.directlink;
                    }
                    else
                    {
                        obj2.TaskType = TaskObject.TaskTypeEnum.keywords;
                    }
                }
                goto Label_0017;
            Label_10F6:
                obj2.SupremeRegion = TaskObject.SupremeRegionEnum.EU;
                goto Label_0017;
            Label_13F7:
                if (!content.switchLogin.IsChecked.HasValue)
                {
                    goto Label_0D62;
                }
                goto Label_0CE8;
            }
        Label_1425:
            Helpers.SaveSettings();
            base.Close();
        }

        private void chDelay_Checked(object sender, RoutedEventArgs e)
        {
            if (this.chDelayExact.IsChecked.HasValue && this.chDelayExact.IsChecked.Value)
            {
                this.txtDelay.IsEnabled = true;
                this.txtDelay.Opacity = 1.0;
                this.lblDelay.IsEnabled = true;
                this.lblDelay.Opacity = 1.0;
                this.txtDelayFrom.IsEnabled = false;
                this.txtDelayFrom.Opacity = 0.6;
                this.txtDelayTo.IsEnabled = false;
                this.txtDelayTo.Opacity = 0.6;
                this.lblFrom.IsEnabled = false;
                this.lblFrom.Opacity = 0.6;
                this.lblTo.IsEnabled = false;
                this.lblTo.Opacity = 0.6;
                this.txtDelayFrom.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
                this.txtDelayTo.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
            }
            else
            {
                this.txtDelay.IsEnabled = false;
                this.txtDelay.Opacity = 0.6;
                this.lblDelay.IsEnabled = false;
                this.lblDelay.Opacity = 0.6;
                this.txtDelayFrom.IsEnabled = true;
                this.txtDelayFrom.Opacity = 1.0;
                this.txtDelayTo.IsEnabled = true;
                this.txtDelayTo.Opacity = 1.0;
                this.lblFrom.IsEnabled = true;
                this.lblFrom.Opacity = 1.0;
                this.lblTo.IsEnabled = true;
                this.lblTo.Opacity = 1.0;
                this.txtDelay.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
            }
        }

        private bool Check()
        {
            bool flag = true;
            if (this.switchProfile.IsChecked.HasValue && this.switchProfile.IsChecked.Value)
            {
                if ((this.cmbCheckoutProfile.SelectedItem != null) && (((ComboBoxItem) this.cmbCheckoutProfile.SelectedItem).Tag.ToString() != "-1"))
                {
                    ToggleButton templatedParent = this.cmbCheckoutProfile.Template.FindName("abc", this.cmbCheckoutProfile) as ToggleButton;
                    (templatedParent.Template.FindName("InnerBorder", templatedParent) as Border).Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
                }
                else
                {
                    ToggleButton templatedParent = this.cmbCheckoutProfile.Template.FindName("abc", this.cmbCheckoutProfile) as ToggleButton;
                    (templatedParent.Template.FindName("InnerBorder", templatedParent) as Border).Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                    flag = false;
                }
            }
            else
            {
                ToggleButton templatedParent = this.cmbCheckoutProfile.Template.FindName("abc", this.cmbCheckoutProfile) as ToggleButton;
                (templatedParent.Template.FindName("InnerBorder", templatedParent) as Border).Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
            }
            if (this.switchSize.IsChecked.HasValue && this.switchSize.IsChecked.Value)
            {
                if (this.chSizeRandom.IsChecked.HasValue && this.chSizeRandom.IsChecked.Value)
                {
                    this.txtSize.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
                }
                else if (!string.IsNullOrEmpty(this.txtSize.Text))
                {
                    this.txtSize.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
                }
                else
                {
                    this.txtSize.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                    flag = false;
                }
            }
            else
            {
                this.txtSize.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
            }
            if (this.switchDelay.IsChecked.HasValue && this.switchDelay.IsChecked.Value)
            {
                if (this.chDelayExact.IsChecked.HasValue && this.chDelayExact.IsChecked.Value)
                {
                    if (string.IsNullOrEmpty(this.txtDelay.Text))
                    {
                        this.txtDelay.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                        flag = false;
                    }
                    else
                    {
                        this.txtDelay.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
                    }
                    this.txtDelayFrom.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
                    this.txtDelayTo.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
                }
                if (this.chDelayRandom.IsChecked.HasValue && this.chDelayRandom.IsChecked.Value)
                {
                    if (!string.IsNullOrEmpty(this.txtDelayFrom.Text))
                    {
                        this.txtDelayFrom.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
                    }
                    else
                    {
                        this.txtDelayFrom.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                        flag = false;
                    }
                    if (!string.IsNullOrEmpty(this.txtDelayTo.Text))
                    {
                        this.txtDelayTo.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
                    }
                    else
                    {
                        this.txtDelayTo.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                        flag = false;
                    }
                    this.txtDelay.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
                }
            }
            bool flag2 = false;
            TaskObject.PlatformEnum enum2 = this._platform;
            if (enum2 <= TaskObject.PlatformEnum.supreme)
            {
                switch (enum2)
                {
                    case TaskObject.PlatformEnum.shopify:
                        flag2 = ((ShopifyMassEditControl) this.content.Content).Check();
                        break;

                    case TaskObject.PlatformEnum.supreme:
                        flag2 = ((SupremeMassEditControl) this.content.Content).Check();
                        break;
                }
            }
            else
            {
                switch (enum2)
                {
                    case TaskObject.PlatformEnum.hibbett:
                        flag2 = ((HibbettMassEditControl) this.content.Content).Check();
                        goto Label_05E9;

                    case TaskObject.PlatformEnum.solebox:
                        flag2 = ((SoleboxMassEditControl) this.content.Content).Check();
                        goto Label_05E9;

                    case TaskObject.PlatformEnum.nordstrom:
                    case TaskObject.PlatformEnum.mesh:
                    case TaskObject.PlatformEnum.holypopstore:
                    case TaskObject.PlatformEnum.barneys:
                        goto Label_05E9;

                    case TaskObject.PlatformEnum.mrporter:
                        flag2 = ((MrPorterMassEditControl) this.content.Content).Check();
                        goto Label_05E9;

                    case TaskObject.PlatformEnum.backdoor:
                        flag2 = ((BackdoorMassEditControl) this.content.Content).Check();
                        goto Label_05E9;

                    case TaskObject.PlatformEnum.offwhite:
                        flag2 = ((OffWhiteMassEditControl) this.content.Content).Check();
                        goto Label_05E9;

                    case TaskObject.PlatformEnum.sneakersnstuff:
                        flag2 = ((SneakersnstuffMassEditControl) this.content.Content).Check();
                        goto Label_05E9;
                }
                if (enum2 == TaskObject.PlatformEnum.boxlunch)
                {
                    flag2 = ((BoxlunchMassEditControl) this.content.Content).Check();
                }
            }
        Label_05E9:
            return (flag & flag2);
        }

        private void chSizeRandom_Checked(object sender, RoutedEventArgs e)
        {
            if (this.chSizeRandom.IsChecked.HasValue && this.chSizeRandom.IsChecked.Value)
            {
                this.txtSize.Text = "Random";
                this.txtSize.IsEnabled = false;
                this.txtSize.Opacity = 0.6;
                this.txtSize.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
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

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.DragMove();
        }

        [DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            // This item is obfuscated and can not be translated.
            if (!this._contentLoaded)
            {
                goto Label_002C;
            }
        Label_000D:
            switch (((-1408744471 ^ -1142387804) % 4))
            {
                case 0:
                    break;

                case 1:
                    return;

                case 2:
                    return;

                case 3:
                    goto Label_000D;

                default:
                    return;
            }
        Label_002C:
            this._contentLoaded = true;
            Uri resourceLocator = new Uri("/EveAIO;component/views/masseditwindow.xaml", UriKind.Relative);
            Application.LoadComponent(this, resourceLocator);
            goto Label_000D;
        }

        private void LoadProfiles()
        {
            ComboBoxItem newItem = new ComboBoxItem {
                Content = "",
                Tag = "-1"
            };
            this.cmbCheckoutProfile.Items.Add(newItem);
            foreach (ProfileObject obj2 in Global.SETTINGS.PROFILES)
            {
                ComboBoxItem item2 = new ComboBoxItem {
                    Content = obj2.Name,
                    Tag = obj2.Id
                };
                this.cmbCheckoutProfile.Items.Add(item2);
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

        private void NumberCheck(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        private void switchCaptcha_Checked(object sender, RoutedEventArgs e)
        {
            if (this.switchCaptcha.IsChecked.HasValue && this.switchCaptcha.IsChecked.Value)
            {
                this.lblCaptcha.IsEnabled = true;
                this.lblCaptcha.Opacity = 1.0;
                this.txtCaptchaRequests.IsEnabled = true;
                this.txtCaptchaRequests.Opacity = 1.0;
            }
            else
            {
                this.lblCaptcha.IsEnabled = false;
                this.lblCaptcha.Opacity = 0.6;
                this.txtCaptchaRequests.IsEnabled = false;
                this.txtCaptchaRequests.Opacity = 0.6;
            }
        }

        private void switchDelay_Checked(object sender, RoutedEventArgs e)
        {
            if (this.switchDelay.IsChecked.HasValue && this.switchDelay.IsChecked.Value)
            {
                this.chDelayExact.IsChecked = true;
                this.chDelayExact.IsEnabled = true;
                this.chDelayExact.Opacity = 1.0;
                this.chDelayRandom.IsEnabled = true;
                this.chDelayRandom.Opacity = 1.0;
                this.lblDelay.IsEnabled = true;
                this.lblDelay.Opacity = 1.0;
            }
            else
            {
                this.chDelayExact.IsEnabled = false;
                this.chDelayExact.Opacity = 0.6;
                this.chDelayRandom.IsEnabled = false;
                this.chDelayRandom.Opacity = 0.6;
                this.lblDelay.IsEnabled = false;
                this.lblDelay.Opacity = 0.6;
                this.lblFrom.IsEnabled = false;
                this.lblFrom.Opacity = 0.6;
                this.lblTo.IsEnabled = false;
                this.lblTo.Opacity = 0.6;
                this.txtDelayFrom.IsEnabled = false;
                this.txtDelayFrom.Opacity = 0.6;
                this.txtDelayTo.IsEnabled = false;
                this.txtDelayTo.Opacity = 0.6;
            }
        }

        private void switchNotifications_Checked(object sender, RoutedEventArgs e)
        {
            if (this.switchNotifications.IsChecked.HasValue && this.switchNotifications.IsChecked.Value)
            {
                this.chNotify.IsEnabled = true;
                this.chNotify.Opacity = 1.0;
            }
            else
            {
                this.chNotify.IsEnabled = false;
                this.chNotify.Opacity = 0.6;
            }
        }

        private void switchPriceCheck_Checked(object sender, RoutedEventArgs e)
        {
            if (this.switchPriceCheck.IsChecked.HasValue && this.switchPriceCheck.IsChecked.Value)
            {
                this.lblMinimum.IsEnabled = true;
                this.lblMinimum.Opacity = 1.0;
                this.lblMaximum.IsEnabled = true;
                this.lblMaximum.Opacity = 1.0;
                this.txtPriceCheckMin.IsEnabled = true;
                this.txtPriceCheckMin.Opacity = 1.0;
                this.txtPriceCheckMax.IsEnabled = true;
                this.txtPriceCheckMax.Opacity = 1.0;
            }
            else
            {
                this.lblMinimum.IsEnabled = false;
                this.lblMinimum.Opacity = 0.6;
                this.lblMaximum.IsEnabled = false;
                this.lblMaximum.Opacity = 0.6;
                this.txtPriceCheckMin.IsEnabled = false;
                this.txtPriceCheckMin.Opacity = 0.6;
                this.txtPriceCheckMax.IsEnabled = false;
                this.txtPriceCheckMax.Opacity = 0.6;
            }
        }

        private void switchProfile_Checked(object sender, RoutedEventArgs e)
        {
            if (this.switchProfile.IsChecked.HasValue && this.switchProfile.IsChecked.Value)
            {
                this.lblProfile.IsEnabled = true;
                this.lblProfile.Opacity = 1.0;
                this.cmbCheckoutProfile.IsEnabled = true;
                this.cmbCheckoutProfile.Opacity = 1.0;
            }
            else
            {
                this.lblProfile.IsEnabled = false;
                this.lblProfile.Opacity = 0.6;
                this.cmbCheckoutProfile.IsEnabled = false;
                this.cmbCheckoutProfile.Opacity = 0.6;
            }
        }

        private void switchProxyList_Checked(object sender, RoutedEventArgs e)
        {
            if (this.switchProxyList.IsChecked.HasValue && this.switchProxyList.IsChecked.Value)
            {
                this.lblProxyList.IsEnabled = true;
                this.lblProxyList.Opacity = 1.0;
                this.cmbProxyList.IsEnabled = true;
                this.cmbProxyList.Opacity = 1.0;
            }
            else
            {
                this.lblProxyList.IsEnabled = false;
                this.lblProxyList.Opacity = 0.6;
                this.cmbProxyList.IsEnabled = false;
                this.cmbProxyList.Opacity = 0.6;
            }
        }

        private void switchSize_Checked(object sender, RoutedEventArgs e)
        {
            if (this.switchSize.IsChecked.HasValue && this.switchSize.IsChecked.Value)
            {
                this.lblSize.IsEnabled = true;
                this.lblSize.Opacity = 1.0;
                this.txtSize.IsEnabled = true;
                this.txtSize.Opacity = 1.0;
                this.chSizeRandom.IsEnabled = true;
                this.chSizeRandom.Opacity = 1.0;
                this.chPickRandomNotAvailable.IsEnabled = true;
                this.chPickRandomNotAvailable.Opacity = 1.0;
            }
            else
            {
                this.lblSize.IsEnabled = false;
                this.lblSize.Opacity = 0.6;
                this.txtSize.IsEnabled = false;
                this.txtSize.Opacity = 0.6;
                this.chSizeRandom.IsEnabled = false;
                this.chSizeRandom.Opacity = 0.6;
                this.chPickRandomNotAvailable.IsEnabled = false;
                this.chPickRandomNotAvailable.Opacity = 0.6;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), GeneratedCode("PresentationBuildTasks", "4.0.0.0"), DebuggerNonUserCode]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    ((MassEditWindow) target).Loaded += new RoutedEventHandler(this.Window_Loaded);
                    return;

                case 2:
                    ((Grid) target).MouseLeftButtonDown += new MouseButtonEventHandler(this.Grid_MouseLeftButtonDown);
                    return;

                case 3:
                    this.BtnClose = (Button) target;
                    this.BtnClose.Click += new RoutedEventHandler(this.BtnClose_Click);
                    return;

                case 4:
                    this.switchProfile = (ToggleButton) target;
                    this.switchProfile.Checked += new RoutedEventHandler(this.switchProfile_Checked);
                    this.switchProfile.Unchecked += new RoutedEventHandler(this.switchProfile_Checked);
                    return;

                case 5:
                    this.lblProfile = (TextBlock) target;
                    return;

                case 6:
                    this.cmbCheckoutProfile = (ComboBox) target;
                    return;

                case 7:
                    this.switchProxyList = (ToggleButton) target;
                    this.switchProxyList.Checked += new RoutedEventHandler(this.switchProxyList_Checked);
                    this.switchProxyList.Unchecked += new RoutedEventHandler(this.switchProxyList_Checked);
                    return;

                case 8:
                    this.lblProxyList = (TextBlock) target;
                    return;

                case 9:
                    this.cmbProxyList = (ComboBox) target;
                    return;

                case 10:
                    this.switchSize = (ToggleButton) target;
                    this.switchSize.Checked += new RoutedEventHandler(this.switchSize_Checked);
                    this.switchSize.Unchecked += new RoutedEventHandler(this.switchSize_Checked);
                    return;

                case 11:
                    this.lblSize = (TextBlock) target;
                    return;

                case 12:
                    this.txtSize = (TextBox) target;
                    return;

                case 13:
                    this.chSizeRandom = (CheckBox) target;
                    this.chSizeRandom.Checked += new RoutedEventHandler(this.chSizeRandom_Checked);
                    this.chSizeRandom.Unchecked += new RoutedEventHandler(this.chSizeRandom_Checked);
                    return;

                case 14:
                    this.chPickRandomNotAvailable = (CheckBox) target;
                    return;

                case 15:
                    this.switchPriceCheck = (ToggleButton) target;
                    this.switchPriceCheck.Checked += new RoutedEventHandler(this.switchPriceCheck_Checked);
                    this.switchPriceCheck.Unchecked += new RoutedEventHandler(this.switchPriceCheck_Checked);
                    return;

                case 0x10:
                    this.lblMinimum = (TextBlock) target;
                    return;

                case 0x11:
                    this.txtPriceCheckMin = (TextBox) target;
                    this.txtPriceCheckMin.PreviewTextInput += new TextCompositionEventHandler(this.NumberCheck);
                    return;

                case 0x12:
                    this.lblMaximum = (TextBlock) target;
                    return;

                case 0x13:
                    this.txtPriceCheckMax = (TextBox) target;
                    this.txtPriceCheckMax.PreviewTextInput += new TextCompositionEventHandler(this.NumberCheck);
                    return;

                case 20:
                    this.switchDelay = (ToggleButton) target;
                    this.switchDelay.Checked += new RoutedEventHandler(this.switchDelay_Checked);
                    this.switchDelay.Unchecked += new RoutedEventHandler(this.switchDelay_Checked);
                    return;

                case 0x15:
                    this.chDelayExact = (RadioButton) target;
                    this.chDelayExact.Checked += new RoutedEventHandler(this.chDelay_Checked);
                    this.chDelayExact.Unchecked += new RoutedEventHandler(this.chDelay_Checked);
                    return;

                case 0x16:
                    this.chDelayRandom = (RadioButton) target;
                    this.chDelayRandom.Checked += new RoutedEventHandler(this.chDelay_Checked);
                    this.chDelayRandom.Unchecked += new RoutedEventHandler(this.chDelay_Checked);
                    return;

                case 0x17:
                    this.lblDelay = (TextBlock) target;
                    return;

                case 0x18:
                    this.txtDelay = (TextBox) target;
                    this.txtDelay.PreviewTextInput += new TextCompositionEventHandler(this.NumberCheck);
                    return;

                case 0x19:
                    this.lblFrom = (TextBlock) target;
                    return;

                case 0x1a:
                    this.txtDelayFrom = (TextBox) target;
                    this.txtDelayFrom.PreviewTextInput += new TextCompositionEventHandler(this.NumberCheck);
                    return;

                case 0x1b:
                    this.lblTo = (TextBlock) target;
                    return;

                case 0x1c:
                    this.txtDelayTo = (TextBox) target;
                    this.txtDelayTo.PreviewTextInput += new TextCompositionEventHandler(this.NumberCheck);
                    return;

                case 0x1d:
                    this.switchCaptcha = (ToggleButton) target;
                    this.switchCaptcha.Checked += new RoutedEventHandler(this.switchCaptcha_Checked);
                    this.switchCaptcha.Unchecked += new RoutedEventHandler(this.switchCaptcha_Checked);
                    return;

                case 30:
                    this.lblCaptcha = (TextBlock) target;
                    return;

                case 0x1f:
                    this.txtCaptchaRequests = (TextBox) target;
                    this.txtCaptchaRequests.PreviewTextInput += new TextCompositionEventHandler(this.NumberCheck);
                    return;

                case 0x20:
                    this.switchNotifications = (ToggleButton) target;
                    this.switchNotifications.Checked += new RoutedEventHandler(this.switchNotifications_Checked);
                    this.switchNotifications.Unchecked += new RoutedEventHandler(this.switchNotifications_Checked);
                    return;

                case 0x21:
                    this.chNotify = (CheckBox) target;
                    return;

                case 0x22:
                    this.content = (ContentPresenter) target;
                    return;

                case 0x23:
                    this.btnSave = (Button) target;
                    this.btnSave.Click += new RoutedEventHandler(this.btnSave_Click);
                    return;

                case 0x24:
                    this.btnCancel = (Button) target;
                    this.btnCancel.Click += new RoutedEventHandler(this.btnCancel_Click);
                    return;
            }
            this._contentLoaded = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.LoadProfiles();
            this.LoadProxies();
            TaskObject.PlatformEnum enum2 = this._platform;
            if (enum2 <= TaskObject.PlatformEnum.supreme)
            {
                switch (enum2)
                {
                    case TaskObject.PlatformEnum.shopify:
                        base.Height = 520.0;
                        this.content.Content = new ShopifyMassEditControl();
                        return;

                    case TaskObject.PlatformEnum.supreme:
                        base.Height = 465.0;
                        this.content.Content = new SupremeMassEditControl();
                        return;
                }
            }
            else
            {
                switch (enum2)
                {
                    case TaskObject.PlatformEnum.hibbett:
                        base.Height = 340.0;
                        this.content.Content = new HibbettMassEditControl();
                        return;

                    case TaskObject.PlatformEnum.solebox:
                        base.Height = 470.0;
                        this.content.Content = new SoleboxMassEditControl();
                        return;

                    case TaskObject.PlatformEnum.nordstrom:
                    case TaskObject.PlatformEnum.mesh:
                    case TaskObject.PlatformEnum.holypopstore:
                    case TaskObject.PlatformEnum.barneys:
                        break;

                    case TaskObject.PlatformEnum.mrporter:
                        base.Height = 340.0;
                        this.content.Content = new MrPorterMassEditControl();
                        return;

                    case TaskObject.PlatformEnum.backdoor:
                        base.Height = 435.0;
                        this.content.Content = new BackdoorMassEditControl();
                        return;

                    case TaskObject.PlatformEnum.offwhite:
                        base.Height = 500.0;
                        this.content.Content = new OffWhiteMassEditControl();
                        return;

                    case TaskObject.PlatformEnum.sneakersnstuff:
                        base.Height = 475.0;
                        this.content.Content = new SneakersnstuffMassEditControl();
                        return;

                    case TaskObject.PlatformEnum.boxlunch:
                        base.Height = 510.0;
                        this.content.Content = new BoxlunchMassEditControl();
                        break;

                    default:
                        return;
                }
            }
        }
    }
}

