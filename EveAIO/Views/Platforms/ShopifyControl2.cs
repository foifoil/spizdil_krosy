namespace EveAIO.Views.Platforms
{
    using EveAIO;
    using EveAIO.Pocos;
    using EveAIO.Tasks;
    using EveAIO.Views;
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
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Markup;
    using System.Windows.Media;

    public class ShopifyControl2 : UserControl, IComponentConnector
    {
        private TaskWindow2 _taskWindow;
        internal GroupBox gbWebsite;
        internal ComboBox cmbWebsite;
        internal Image imgInfo;
        internal TextBlock lblCheckoutLink;
        internal ComboBox cmbCheckoutlink;
        internal GroupBox gbLogin;
        public ToggleButton switchLogin;
        public TextBox txtUsername;
        public TextBox txtPassword;
        internal GroupBox gbTaskType;
        public RadioButton rTypeDirect;
        public RadioButton rTypeKeywords;
        public RadioButton rTypeVariant;
        public TextBox txtQuantity;
        internal StackPanel rowDirectlink;
        internal TextBlock lblLink;
        public TextBox txtLink;
        internal TextBlock lblLinkType;
        internal StackPanel rowVariant;
        internal TextBlock lblVariant;
        public TextBox txtVariant;
        internal StackPanel rowPositive;
        internal TextBlock lblPositiveKws;
        public TextBox txtPositiveKws;
        internal StackPanel rowNegative;
        internal TextBlock lblNegativeKws;
        public TextBox txtNegativeKws;
        internal StackPanel rowSearch;
        internal TextBlock textBlock_0;
        public ToggleButton switchLast25;
        internal StackPanel pDeepSearch;
        public ToggleButton switchDeepSearch;
        internal TextBlock lblDeepSearchLinks;
        internal TextBox txtDeepSearchLinks;
        internal GroupBox gbSize;
        public TextBox txtColor;
        public RadioButton rColorExact;
        public RadioButton rColorContains;
        public CheckBox chPickRandomColorNotAvailable;
        internal GroupBox gbPayment;
        public RadioButton radioButton_0;
        public RadioButton radioButton_1;
        public ComboBox cmbParentTask;
        internal TextBlock lblOldMode;
        public ToggleButton switchOldMode;
        private bool _contentLoaded;

        public ShopifyControl2(TaskWindow2 taskWindow)
        {
            Class7.RIuqtBYzWxthF();
            this.InitializeComponent();
            this._taskWindow = taskWindow;
            this.rTypeDirect.IsChecked = true;
            this.radioButton_0.IsChecked = true;
            this.rColorContains.IsChecked = true;
            foreach (KeyValuePair<string, string> pair in WebsitesInfo.PREDEFINED_SHOPIFY)
            {
                ComboBoxItem item1 = new ComboBoxItem {
                    Content = pair.Value,
                    Tag = pair.Key
                };
                this.cmbWebsite.Items.Add(item1);
            }
            ComboBoxItem newItem = new ComboBoxItem {
                Content = "",
                Tag = "-1"
            };
            this.cmbParentTask.Items.Add(newItem);
            foreach (TaskObject obj2 in from x in Global.SETTINGS.TASKS
                where (x.Platform == TaskObject.PlatformEnum.shopify) && string.IsNullOrEmpty(x.ParentId)
                select x)
            {
                if (obj2.Id != this._taskWindow._task.Id)
                {
                    ComboBoxItem item3 = new ComboBoxItem {
                        Content = obj2.Name,
                        Tag = obj2.Id
                    };
                    this.cmbParentTask.Items.Add(item3);
                }
            }
            this.cmbWebsite.SelectedIndex = this.cmbWebsite.Items.Count - 1;
            this.switchLast25.IsChecked = true;
        }

        public bool Check()
        {
            bool flag = true;
            if (this.switchLogin.IsChecked.HasValue && this.switchLogin.IsChecked.Value)
            {
                if (string.IsNullOrEmpty(this.txtUsername.Text))
                {
                    this.txtUsername.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                    flag = false;
                }
                else
                {
                    this.txtUsername.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                }
                if (!string.IsNullOrEmpty(this.txtPassword.Text))
                {
                    this.txtPassword.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                }
                else
                {
                    this.txtPassword.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                    flag = false;
                }
            }
            else
            {
                this.txtUsername.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                this.txtPassword.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
            }
            if (string.IsNullOrEmpty(this.txtLink.Text) && ((this.rTypeDirect.IsChecked.HasValue && this.rTypeDirect.IsChecked.Value) || (((ComboBoxItem) this.cmbWebsite.SelectedItem).Tag.ToString() == "other")))
            {
                this.txtLink.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                flag = false;
            }
            else
            {
                this.txtLink.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
            }
            if (this.rTypeKeywords.IsChecked.HasValue && this.rTypeKeywords.IsChecked.Value)
            {
                if (!string.IsNullOrEmpty(this.txtPositiveKws.Text))
                {
                    this.txtPositiveKws.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                }
                else
                {
                    this.txtPositiveKws.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                    flag = false;
                }
                this.txtVariant.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                return flag;
            }
            if (this.rTypeVariant.IsChecked.HasValue && this.rTypeVariant.IsChecked.Value)
            {
                if (!string.IsNullOrEmpty(this.txtVariant.Text))
                {
                    this.txtVariant.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                }
                else
                {
                    this.txtVariant.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                    flag = false;
                }
                this.txtPositiveKws.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                this.txtNegativeKws.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                return flag;
            }
            this.txtVariant.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
            this.txtPositiveKws.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
            this.txtNegativeKws.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
            return flag;
        }

        private void cmbCheckoutlink_DropDownOpened(object sender, EventArgs e)
        {
            this.cmbCheckoutlink.Items.Clear();
            ComboBoxItem newItem = new ComboBoxItem {
                Content = "-",
                Tag = "-"
            };
            this.cmbCheckoutlink.Items.Add(newItem);
            string item = ((ComboBoxItem) this.cmbWebsite.SelectedItem).Tag.ToString();
            List<CheckoutLink> checkoutLinks = Helpers.GetCheckoutLinks(WebsitesInfo.SHOPIFY_WEBS.First<ShopifyWebsiteInfo>(x => (x.Website == item)).HomeLink);
            if ((checkoutLinks != null) && (checkoutLinks.Count > 0))
            {
                foreach (CheckoutLink link in checkoutLinks)
                {
                    ComboBoxItem item2 = new ComboBoxItem {
                        Content = link.Name + " (" + link.Size + ")",
                        Tag = link.Links
                    };
                    this.cmbCheckoutlink.Items.Add(item2);
                }
            }
        }

        private void cmbCheckoutlink_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((this.cmbCheckoutlink.SelectedItem != null) && (((ComboBoxItem) this.cmbCheckoutlink.SelectedItem).Tag.ToString() != "-"))
            {
                ComboBoxItem selectedItem = (ComboBoxItem) this.cmbCheckoutlink.SelectedItem;
                string webItem = ((ComboBoxItem) this.cmbWebsite.SelectedItem).Tag.ToString();
                ShopifyWebsiteInfo info = WebsitesInfo.SHOPIFY_WEBS.First<ShopifyWebsiteInfo>(x => x.Website == webItem);
                this.gbTaskType.Opacity = 0.6;
                this.gbTaskType.IsEnabled = false;
                this.rTypeDirect.IsChecked = true;
                object[] objArray1 = new object[] { "(", info.HomeLink, ") ", selectedItem.Content };
                this.txtLink.Text = string.Concat(objArray1);
                this._taskWindow._generalView.gbSizing.Opacity = 0.6;
                this._taskWindow._generalView.gbSizing.IsEnabled = false;
                this._taskWindow._generalView.chSizeRandom.IsChecked = true;
            }
            else
            {
                this.gbTaskType.Opacity = 1.0;
                this.gbTaskType.IsEnabled = true;
                this.txtLink.Text = "";
                this._taskWindow._generalView.gbSizing.Opacity = 1.0;
                this._taskWindow._generalView.gbSizing.IsEnabled = true;
            }
        }

        private void cmbWebsite_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.cmbCheckoutlink.Items.Clear();
            if (this.cmbWebsite.SelectedItem != null)
            {
                string item = ((ComboBoxItem) this.cmbWebsite.SelectedItem).Tag.ToString();
                if (item != "other")
                {
                    this.lblCheckoutLink.Visibility = Visibility.Visible;
                    this.cmbCheckoutlink.Visibility = Visibility.Visible;
                    this.imgInfo.Visibility = Visibility.Visible;
                    ShopifyWebsiteInfo info = WebsitesInfo.SHOPIFY_WEBS.First<ShopifyWebsiteInfo>(x => x.Website == item);
                    string str = ((((("Website: " + info.HomeLink) + "\nLogin: " + (info.Login ? "✔" : "✘")) + "\nSmart checkout: " + (info.SmartCheckout ? "✔" : "✘")) + "\nCheckout links: " + (info.CheckoutLinks ? "✔" : "✘")) + "\nAtc links: " + (info.AtcLinks ? "✔" : "✘")) + "\nAdditional info: " + info.AdditionalInfo;
                    if (!info.OldMode)
                    {
                        this.lblOldMode.Visibility = Visibility.Hidden;
                        this.switchOldMode.Visibility = Visibility.Hidden;
                        this.switchOldMode.IsChecked = false;
                    }
                    else
                    {
                        this.lblOldMode.Visibility = Visibility.Visible;
                        this.switchOldMode.Visibility = Visibility.Visible;
                    }
                    if (!info.Login)
                    {
                        this.switchLogin.IsChecked = false;
                        this.txtUsername.Text = "";
                        this.txtPassword.Text = "";
                    }
                    this.gbLogin.Visibility = info.Login ? Visibility.Visible : Visibility.Collapsed;
                    this.gbSize.Visibility = info.Color ? Visibility.Visible : Visibility.Collapsed;
                    this.imgInfo.ToolTip = str;
                    if (this.rTypeKeywords.IsChecked.HasValue && this.rTypeKeywords.IsChecked.Value)
                    {
                        this.pDeepSearch.Visibility = Visibility.Visible;
                    }
                    if (item == "yeezy")
                    {
                        this.pDeepSearch.Visibility = Visibility.Collapsed;
                    }
                    if (this.rTypeDirect.IsChecked.HasValue && this.rTypeDirect.IsChecked.Value)
                    {
                        this.lblLink.Visibility = Visibility.Visible;
                        this.txtLink.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        this.lblLink.Visibility = Visibility.Collapsed;
                        this.txtLink.Visibility = Visibility.Collapsed;
                    }
                    if (this.rTypeKeywords.IsChecked.HasValue && this.rTypeKeywords.IsChecked.Value)
                    {
                        this.txtLink.Text = info.HomeLink;
                    }
                    if (!info.PaypalOnly)
                    {
                        this.gbPayment.IsEnabled = true;
                        this.gbPayment.Opacity = 1.0;
                        this.radioButton_0.IsChecked = true;
                    }
                    else
                    {
                        this.gbPayment.IsEnabled = false;
                        this.gbPayment.Opacity = 0.6;
                        this.radioButton_1.IsChecked = true;
                    }
                    this.rType_Checked(null, null);
                }
                else
                {
                    this.pDeepSearch.Visibility = Visibility.Collapsed;
                    this.lblLink.Visibility = Visibility.Visible;
                    this.txtLink.Visibility = Visibility.Visible;
                    this.imgInfo.Visibility = Visibility.Hidden;
                    this.gbLogin.Visibility = Visibility.Visible;
                    this.gbSize.Visibility = Visibility.Collapsed;
                    this.lblCheckoutLink.Visibility = Visibility.Hidden;
                    this.cmbCheckoutlink.Visibility = Visibility.Hidden;
                    this.gbTaskType.Opacity = 1.0;
                    this.gbTaskType.IsEnabled = true;
                    this.txtLink.Text = "";
                    this.gbPayment.IsEnabled = true;
                    this.gbPayment.Opacity = 1.0;
                    this.radioButton_0.IsChecked = true;
                    this.lblOldMode.Visibility = Visibility.Hidden;
                    this.switchOldMode.Visibility = Visibility.Hidden;
                    this.switchOldMode.IsChecked = false;
                }
                this.txtLink_TextChanged(null, null);
            }
        }

        [DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            // This item is obfuscated and can not be translated.
            Uri uri;
            if (!this._contentLoaded)
            {
                goto Label_002C;
            }
        Label_000D:
            switch (((-1951316554 ^ -525027359) % 4))
            {
                case 0:
                    goto Label_000D;

                case 2:
                    break;

                case 3:
                    return;

                default:
                    Application.LoadComponent(this, uri);
                    return;
            }
        Label_002C:
            this._contentLoaded = true;
            uri = new Uri("/EveAIO;component/views/platforms/shopifycontrol2.xaml", UriKind.Relative);
            goto Label_000D;
        }

        private void NumberCheck(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        private void Payment_checked(object sender, RoutedEventArgs e)
        {
            if (this.radioButton_0.IsChecked.HasValue && this.radioButton_0.IsChecked.Value)
            {
                this._taskWindow._advancedView.chPaypalProxyIgnore.Opacity = 0.6;
                this._taskWindow._advancedView.chPaypalProxyIgnore.IsEnabled = false;
            }
            else
            {
                this._taskWindow._advancedView.chPaypalProxyIgnore.Opacity = 1.0;
                this._taskWindow._advancedView.chPaypalProxyIgnore.IsEnabled = true;
            }
        }

        private void rType_Checked(object sender, RoutedEventArgs e)
        {
            if (this.rTypeDirect.IsChecked.HasValue && this.rTypeDirect.IsChecked.Value)
            {
                this.lblLink.Text = "DIRECT LINK:";
                this.txtVariant.IsEnabled = false;
                this.txtVariant.Opacity = 0.6;
                this.txtVariant.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                this.txtPositiveKws.IsEnabled = false;
                this.txtPositiveKws.Opacity = 0.6;
                this.txtPositiveKws.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                this.txtNegativeKws.IsEnabled = false;
                this.txtNegativeKws.Opacity = 0.6;
                this.txtNegativeKws.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                this.txtVariant.Clear();
                this.txtPositiveKws.Clear();
                this.txtNegativeKws.Clear();
                this.txtVariant.Visibility = Visibility.Collapsed;
                this.lblVariant.Visibility = Visibility.Collapsed;
                this.lblPositiveKws.Visibility = Visibility.Collapsed;
                this.txtPositiveKws.Visibility = Visibility.Collapsed;
                this.lblNegativeKws.Visibility = Visibility.Collapsed;
                this.txtNegativeKws.Visibility = Visibility.Collapsed;
                this.textBlock_0.Visibility = Visibility.Collapsed;
                this.switchLast25.Visibility = Visibility.Collapsed;
                this.lblLink.Visibility = Visibility.Visible;
                this.txtLink.Visibility = Visibility.Visible;
                this.pDeepSearch.Visibility = Visibility.Collapsed;
            }
            else if (this.rTypeKeywords.IsChecked.HasValue && this.rTypeKeywords.IsChecked.Value)
            {
                this.lblLink.Text = "HOME PAGE:";
                this.txtVariant.IsEnabled = false;
                this.txtVariant.Opacity = 0.6;
                this.txtVariant.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                this.txtPositiveKws.IsEnabled = true;
                this.txtPositiveKws.Opacity = 1.0;
                this.txtPositiveKws.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                this.txtNegativeKws.IsEnabled = true;
                this.txtNegativeKws.Opacity = 1.0;
                this.txtNegativeKws.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                this.txtVariant.Clear();
                this.txtVariant.Visibility = Visibility.Collapsed;
                this.lblVariant.Visibility = Visibility.Collapsed;
                this.lblPositiveKws.Visibility = Visibility.Visible;
                this.txtPositiveKws.Visibility = Visibility.Visible;
                this.lblNegativeKws.Visibility = Visibility.Visible;
                this.txtNegativeKws.Visibility = Visibility.Visible;
                this.textBlock_0.Visibility = Visibility.Visible;
                this.switchLast25.Visibility = Visibility.Visible;
                if (((ComboBoxItem) this.cmbWebsite.SelectedItem).Tag.ToString() == "other")
                {
                    this.lblLink.Visibility = Visibility.Visible;
                    this.txtLink.Visibility = Visibility.Visible;
                    this.pDeepSearch.Visibility = Visibility.Collapsed;
                }
                else
                {
                    this.txtLink.Text = WebsitesInfo.SHOPIFY_WEBS.First<ShopifyWebsiteInfo>(x => (x.Website == ((ComboBoxItem) this.cmbWebsite.SelectedItem).Tag.ToString())).HomeLink;
                    this.pDeepSearch.Visibility = Visibility.Visible;
                    this.lblLink.Visibility = Visibility.Collapsed;
                    this.txtLink.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                this.lblLink.Text = "HOME PAGE";
                this.txtVariant.IsEnabled = true;
                this.txtVariant.Opacity = 1.0;
                this.txtVariant.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                this.txtPositiveKws.IsEnabled = false;
                this.txtPositiveKws.Opacity = 0.6;
                this.txtPositiveKws.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                this.txtNegativeKws.IsEnabled = false;
                this.txtNegativeKws.Opacity = 0.6;
                this.txtNegativeKws.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                this.txtPositiveKws.Clear();
                this.txtNegativeKws.Clear();
                this.txtVariant.Visibility = Visibility.Visible;
                this.lblVariant.Visibility = Visibility.Visible;
                this.lblPositiveKws.Visibility = Visibility.Collapsed;
                this.txtPositiveKws.Visibility = Visibility.Collapsed;
                this.lblNegativeKws.Visibility = Visibility.Collapsed;
                this.txtNegativeKws.Visibility = Visibility.Collapsed;
                this.textBlock_0.Visibility = Visibility.Collapsed;
                this.switchLast25.Visibility = Visibility.Collapsed;
                if (((ComboBoxItem) this.cmbWebsite.SelectedItem).Tag.ToString() == "other")
                {
                    this.lblLink.Visibility = Visibility.Visible;
                    this.txtLink.Visibility = Visibility.Visible;
                }
                else
                {
                    this.txtLink.Text = WebsitesInfo.SHOPIFY_WEBS.First<ShopifyWebsiteInfo>(x => (x.Website == ((ComboBoxItem) this.cmbWebsite.SelectedItem).Tag.ToString())).HomeLink;
                    this.lblLink.Visibility = Visibility.Collapsed;
                    this.txtLink.Visibility = Visibility.Collapsed;
                }
                this.pDeepSearch.Visibility = Visibility.Collapsed;
            }
            this.txtLink_TextChanged(null, null);
        }

        private void switchDeepSearch_Checked(object sender, RoutedEventArgs e)
        {
            if (this.switchDeepSearch.IsChecked.HasValue && this.switchDeepSearch.IsChecked.Value)
            {
                this.lblDeepSearchLinks.IsEnabled = true;
                this.lblDeepSearchLinks.Opacity = 1.0;
                this.txtDeepSearchLinks.IsEnabled = true;
                this.txtDeepSearchLinks.Opacity = 1.0;
            }
            else
            {
                this.lblDeepSearchLinks.IsEnabled = false;
                this.lblDeepSearchLinks.Opacity = 0.6;
                this.txtDeepSearchLinks.IsEnabled = false;
                this.txtDeepSearchLinks.Opacity = 0.6;
            }
        }

        private void switchLogin_Checked(object sender, RoutedEventArgs e)
        {
            if (this.switchLogin.IsChecked.HasValue && this.switchLogin.IsChecked.Value)
            {
                this.txtUsername.Opacity = 1.0;
                this.txtUsername.IsEnabled = true;
                this.txtPassword.Opacity = 1.0;
                this.txtPassword.IsEnabled = true;
            }
            else
            {
                this.txtUsername.Opacity = 0.6;
                this.txtUsername.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                this.txtUsername.IsEnabled = false;
                this.txtPassword.Opacity = 0.6;
                this.txtPassword.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                this.txtPassword.IsEnabled = false;
            }
        }

        [DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0"), EditorBrowsable(EditorBrowsableState.Never)]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    this.gbWebsite = (GroupBox) target;
                    return;

                case 2:
                    this.cmbWebsite = (ComboBox) target;
                    this.cmbWebsite.SelectionChanged += new SelectionChangedEventHandler(this.cmbWebsite_SelectionChanged);
                    return;

                case 3:
                    this.imgInfo = (Image) target;
                    return;

                case 4:
                    this.lblCheckoutLink = (TextBlock) target;
                    return;

                case 5:
                    this.cmbCheckoutlink = (ComboBox) target;
                    this.cmbCheckoutlink.SelectionChanged += new SelectionChangedEventHandler(this.cmbCheckoutlink_SelectionChanged);
                    this.cmbCheckoutlink.DropDownOpened += new EventHandler(this.cmbCheckoutlink_DropDownOpened);
                    return;

                case 6:
                    this.gbLogin = (GroupBox) target;
                    return;

                case 7:
                    this.switchLogin = (ToggleButton) target;
                    this.switchLogin.Checked += new RoutedEventHandler(this.switchLogin_Checked);
                    this.switchLogin.Unchecked += new RoutedEventHandler(this.switchLogin_Checked);
                    return;

                case 8:
                    this.txtUsername = (TextBox) target;
                    return;

                case 9:
                    this.txtPassword = (TextBox) target;
                    return;

                case 10:
                    this.gbTaskType = (GroupBox) target;
                    return;

                case 11:
                    this.rTypeDirect = (RadioButton) target;
                    this.rTypeDirect.Checked += new RoutedEventHandler(this.rType_Checked);
                    this.rTypeDirect.Unchecked += new RoutedEventHandler(this.rType_Checked);
                    return;

                case 12:
                    this.rTypeKeywords = (RadioButton) target;
                    this.rTypeKeywords.Checked += new RoutedEventHandler(this.rType_Checked);
                    this.rTypeKeywords.Unchecked += new RoutedEventHandler(this.rType_Checked);
                    return;

                case 13:
                    this.rTypeVariant = (RadioButton) target;
                    this.rTypeVariant.Checked += new RoutedEventHandler(this.rType_Checked);
                    this.rTypeVariant.Unchecked += new RoutedEventHandler(this.rType_Checked);
                    return;

                case 14:
                    this.txtQuantity = (TextBox) target;
                    this.txtQuantity.PreviewTextInput += new TextCompositionEventHandler(this.NumberCheck);
                    return;

                case 15:
                    this.rowDirectlink = (StackPanel) target;
                    return;

                case 0x10:
                    this.lblLink = (TextBlock) target;
                    return;

                case 0x11:
                    this.txtLink = (TextBox) target;
                    this.txtLink.TextChanged += new TextChangedEventHandler(this.txtLink_TextChanged);
                    return;

                case 0x12:
                    this.lblLinkType = (TextBlock) target;
                    return;

                case 0x13:
                    this.rowVariant = (StackPanel) target;
                    return;

                case 20:
                    this.lblVariant = (TextBlock) target;
                    return;

                case 0x15:
                    this.txtVariant = (TextBox) target;
                    return;

                case 0x16:
                    this.rowPositive = (StackPanel) target;
                    return;

                case 0x17:
                    this.lblPositiveKws = (TextBlock) target;
                    return;

                case 0x18:
                    this.txtPositiveKws = (TextBox) target;
                    return;

                case 0x19:
                    this.rowNegative = (StackPanel) target;
                    return;

                case 0x1a:
                    this.lblNegativeKws = (TextBlock) target;
                    return;

                case 0x1b:
                    this.txtNegativeKws = (TextBox) target;
                    return;

                case 0x1c:
                    this.rowSearch = (StackPanel) target;
                    return;

                case 0x1d:
                    this.textBlock_0 = (TextBlock) target;
                    return;

                case 30:
                    this.switchLast25 = (ToggleButton) target;
                    return;

                case 0x1f:
                    this.pDeepSearch = (StackPanel) target;
                    return;

                case 0x20:
                    this.switchDeepSearch = (ToggleButton) target;
                    this.switchDeepSearch.Checked += new RoutedEventHandler(this.switchDeepSearch_Checked);
                    this.switchDeepSearch.Unchecked += new RoutedEventHandler(this.switchDeepSearch_Checked);
                    return;

                case 0x21:
                    this.lblDeepSearchLinks = (TextBlock) target;
                    return;

                case 0x22:
                    this.txtDeepSearchLinks = (TextBox) target;
                    this.txtDeepSearchLinks.PreviewTextInput += new TextCompositionEventHandler(this.NumberCheck);
                    return;

                case 0x23:
                    this.gbSize = (GroupBox) target;
                    return;

                case 0x24:
                    this.txtColor = (TextBox) target;
                    return;

                case 0x25:
                    this.rColorExact = (RadioButton) target;
                    return;

                case 0x26:
                    this.rColorContains = (RadioButton) target;
                    return;

                case 0x27:
                    this.chPickRandomColorNotAvailable = (CheckBox) target;
                    return;

                case 40:
                    this.gbPayment = (GroupBox) target;
                    return;

                case 0x29:
                    this.radioButton_0 = (RadioButton) target;
                    this.radioButton_0.Checked += new RoutedEventHandler(this.Payment_checked);
                    this.radioButton_0.Unchecked += new RoutedEventHandler(this.Payment_checked);
                    return;

                case 0x2a:
                    this.radioButton_1 = (RadioButton) target;
                    this.radioButton_1.Checked += new RoutedEventHandler(this.Payment_checked);
                    this.radioButton_1.Unchecked += new RoutedEventHandler(this.Payment_checked);
                    return;

                case 0x2b:
                    this.cmbParentTask = (ComboBox) target;
                    return;

                case 0x2c:
                    this.lblOldMode = (TextBlock) target;
                    return;

                case 0x2d:
                    this.switchOldMode = (ToggleButton) target;
                    return;
            }
            this._contentLoaded = true;
        }

        private void txtLink_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.txtLink.Text.ToLowerInvariant().Contains("/checkouts/"))
            {
                if (this.txtLink.Text.ToLowerInvariant().Contains("/stock_problems"))
                {
                    this.txtLink.Text = this.txtLink.Text.Substring(0, this.txtLink.Text.ToLowerInvariant().IndexOf("/stock_problems"));
                }
                if (this.txtLink.Text.ToLowerInvariant().Contains("?_ga"))
                {
                    this.txtLink.Text = this.txtLink.Text.Substring(0, this.txtLink.Text.ToLowerInvariant().IndexOf("?_ga"));
                }
                this._taskWindow._generalView.gbPriceCheck.IsEnabled = false;
                this._taskWindow._generalView.gbPriceCheck.Opacity = 0.6;
                this._taskWindow._generalView.gbSizing.IsEnabled = false;
                this._taskWindow._generalView.gbSizing.Opacity = 0.6;
                this._taskWindow._generalView.chSizeRandom.IsChecked = true;
                if (this.rTypeDirect.IsChecked.HasValue && this.rTypeDirect.IsChecked.Value)
                {
                    this.lblLinkType.Text = "checkout link";
                    this.lblLinkType.Visibility = Visibility.Visible;
                }
                else
                {
                    this.lblLinkType.Visibility = Visibility.Collapsed;
                }
            }
            else if (this.txtLink.Text.ToLowerInvariant().Contains("/cart/") && this.txtLink.Text.ToLowerInvariant().Contains(":1"))
            {
                this._taskWindow._generalView.gbPriceCheck.IsEnabled = false;
                this._taskWindow._generalView.gbPriceCheck.Opacity = 0.6;
                this._taskWindow._generalView.gbSizing.IsEnabled = false;
                this._taskWindow._generalView.gbSizing.Opacity = 0.6;
                this._taskWindow._generalView.chSizeRandom.IsChecked = true;
                if (this.rTypeDirect.IsChecked.HasValue && this.rTypeDirect.IsChecked.Value)
                {
                    this.lblLinkType.Text = "atc link";
                    this.lblLinkType.Visibility = Visibility.Visible;
                }
                else
                {
                    this.lblLinkType.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                this.lblLinkType.Visibility = Visibility.Collapsed;
                this._taskWindow._generalView.gbPriceCheck.IsEnabled = true;
                this._taskWindow._generalView.gbPriceCheck.Opacity = 1.0;
                this._taskWindow._generalView.gbSizing.IsEnabled = true;
                this._taskWindow._generalView.gbSizing.Opacity = 1.0;
            }
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly ShopifyControl2.<>c <>9;
            public static Func<TaskObject, bool> <>9__1_0;

            static <>c()
            {
                Class7.RIuqtBYzWxthF();
                <>9 = new ShopifyControl2.<>c();
            }

            public <>c()
            {
                Class7.RIuqtBYzWxthF();
            }

            internal bool <.ctor>b__1_0(TaskObject x) => 
                ((x.Platform == TaskObject.PlatformEnum.shopify) && string.IsNullOrEmpty(x.ParentId));
        }
    }
}

