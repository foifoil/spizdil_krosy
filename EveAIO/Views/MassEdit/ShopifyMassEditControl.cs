namespace EveAIO.Views.MassEdit
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Markup;
    using System.Windows.Media;

    public class ShopifyMassEditControl : UserControl, IComponentConnector
    {
        public ToggleButton switchCheckout;
        public RadioButton radioButton_0;
        public RadioButton radioButton_1;
        public ToggleButton switchAtc;
        public RadioButton rAtcFrontend;
        public RadioButton rAtcBackend;
        public ToggleButton switchLogin;
        internal TextBlock lblUsername;
        public TextBox txtUsername;
        internal TextBlock lblPassword;
        public TextBox txtPassword;
        public ToggleButton switchTaskType;
        public RadioButton rTypeDirect;
        public RadioButton rTypeKeywords;
        public RadioButton rTypeVariant;
        internal TextBlock lblLink;
        public TextBox txtLink;
        internal TextBlock lblVariant;
        public TextBox txtVariant;
        internal TextBlock lblPositive;
        public TextBox txtPositiveKws;
        internal TextBlock lblNegative;
        public TextBox txtNegativeKws;
        private bool _contentLoaded;

        public ShopifyMassEditControl()
        {
            Class7.RIuqtBYzWxthF();
            this.InitializeComponent();
        }

        public bool Check()
        {
            bool flag = true;
            if (this.switchLogin.IsChecked.HasValue && this.switchLogin.IsChecked.Value)
            {
                if (!string.IsNullOrEmpty(this.txtUsername.Text))
                {
                    this.txtUsername.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
                }
                else
                {
                    this.txtUsername.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                    flag = false;
                }
                if (string.IsNullOrEmpty(this.txtPassword.Text))
                {
                    this.txtPassword.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                    return false;
                }
                this.txtPassword.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
                return flag;
            }
            this.txtUsername.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
            this.txtPassword.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
            return flag;
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
            switch (((-835856426 ^ -488539636) % 4))
            {
                case 1:
                    break;

                case 2:
                    return;

                case 3:
                    goto Label_000D;

                default:
                {
                    Uri resourceLocator = new Uri("/EveAIO;component/views/massedit/shopifymasseditcontrol.xaml", UriKind.Relative);
                    Application.LoadComponent(this, resourceLocator);
                    return;
                }
            }
        Label_002C:
            this._contentLoaded = true;
            goto Label_000D;
        }

        private void rType_Checked(object sender, RoutedEventArgs e)
        {
            if (this.rTypeDirect.IsChecked.HasValue && this.rTypeDirect.IsChecked.Value)
            {
                this.lblLink.Text = "Direct link:";
                this.txtVariant.IsEnabled = false;
                this.txtVariant.Opacity = 0.6;
                this.txtVariant.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
                this.txtPositiveKws.IsEnabled = false;
                this.txtPositiveKws.Opacity = 0.6;
                this.txtPositiveKws.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
                this.txtNegativeKws.IsEnabled = false;
                this.txtNegativeKws.Opacity = 0.6;
                this.txtNegativeKws.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
                this.txtVariant.Clear();
                this.txtPositiveKws.Clear();
                this.txtNegativeKws.Clear();
            }
            else if (this.rTypeKeywords.IsChecked.HasValue && this.rTypeKeywords.IsChecked.Value)
            {
                this.lblLink.Text = "Home page:";
                this.txtVariant.IsEnabled = false;
                this.txtVariant.Opacity = 0.6;
                this.txtVariant.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
                this.txtPositiveKws.IsEnabled = true;
                this.txtPositiveKws.Opacity = 1.0;
                this.txtPositiveKws.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
                this.txtNegativeKws.IsEnabled = true;
                this.txtNegativeKws.Opacity = 1.0;
                this.txtNegativeKws.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
                this.txtVariant.Clear();
            }
            else
            {
                this.lblLink.Text = "Home page:";
                this.txtVariant.IsEnabled = true;
                this.txtVariant.Opacity = 1.0;
                this.txtVariant.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
                this.txtPositiveKws.IsEnabled = false;
                this.txtPositiveKws.Opacity = 0.6;
                this.txtPositiveKws.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
                this.txtNegativeKws.IsEnabled = false;
                this.txtNegativeKws.Opacity = 0.6;
                this.txtNegativeKws.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
                this.txtPositiveKws.Clear();
                this.txtNegativeKws.Clear();
            }
        }

        private void switchAtc_Checked(object sender, RoutedEventArgs e)
        {
            if (this.switchAtc.IsChecked.HasValue && this.switchAtc.IsChecked.Value)
            {
                this.rAtcFrontend.IsEnabled = true;
                this.rAtcFrontend.Opacity = 1.0;
                this.rAtcFrontend.IsChecked = true;
                this.rAtcBackend.IsEnabled = true;
                this.rAtcBackend.Opacity = 1.0;
            }
            else
            {
                this.rAtcFrontend.IsEnabled = false;
                this.rAtcFrontend.Opacity = 0.6;
                this.rAtcBackend.IsEnabled = false;
                this.rAtcBackend.Opacity = 0.6;
            }
        }

        private void switchCheckout_Checked(object sender, RoutedEventArgs e)
        {
            if (this.switchCheckout.IsChecked.HasValue && this.switchCheckout.IsChecked.Value)
            {
                this.radioButton_0.IsEnabled = true;
                this.radioButton_0.Opacity = 1.0;
                this.radioButton_0.IsChecked = true;
                this.radioButton_1.IsEnabled = true;
                this.radioButton_1.Opacity = 1.0;
            }
            else
            {
                this.radioButton_0.IsEnabled = false;
                this.radioButton_0.Opacity = 0.6;
                this.radioButton_1.IsEnabled = false;
                this.radioButton_1.Opacity = 0.6;
            }
        }

        private void switchLogin_Checked(object sender, RoutedEventArgs e)
        {
            if (this.switchLogin.IsChecked.HasValue && this.switchLogin.IsChecked.Value)
            {
                this.lblUsername.IsEnabled = true;
                this.lblUsername.Opacity = 1.0;
                this.lblPassword.IsEnabled = true;
                this.lblPassword.Opacity = 1.0;
                this.txtUsername.IsEnabled = true;
                this.txtUsername.Opacity = 1.0;
                this.txtPassword.IsEnabled = true;
                this.txtPassword.Opacity = 1.0;
            }
            else
            {
                this.lblUsername.IsEnabled = false;
                this.lblUsername.Opacity = 0.6;
                this.lblPassword.IsEnabled = false;
                this.lblPassword.Opacity = 0.6;
                this.txtUsername.IsEnabled = false;
                this.txtUsername.Opacity = 0.6;
                this.txtPassword.IsEnabled = false;
                this.txtPassword.Opacity = 0.6;
            }
        }

        private void switchTaskType_Checked(object sender, RoutedEventArgs e)
        {
            if (this.switchTaskType.IsChecked.HasValue && this.switchTaskType.IsChecked.Value)
            {
                this.lblLink.IsEnabled = true;
                this.lblLink.Opacity = 1.0;
                this.lblVariant.IsEnabled = true;
                this.lblVariant.Opacity = 1.0;
                this.lblPositive.IsEnabled = true;
                this.lblPositive.Opacity = 1.0;
                this.lblNegative.IsEnabled = true;
                this.lblNegative.Opacity = 1.0;
                this.txtLink.IsEnabled = true;
                this.txtLink.Opacity = 1.0;
                this.txtVariant.IsEnabled = true;
                this.txtVariant.Opacity = 1.0;
                this.txtPositiveKws.IsEnabled = true;
                this.txtPositiveKws.Opacity = 1.0;
                this.txtNegativeKws.IsEnabled = true;
                this.txtNegativeKws.Opacity = 1.0;
                this.rTypeDirect.IsEnabled = true;
                this.rTypeDirect.Opacity = 1.0;
                this.rTypeKeywords.IsEnabled = true;
                this.rTypeKeywords.Opacity = 1.0;
                this.rTypeVariant.IsEnabled = true;
                this.rTypeVariant.Opacity = 1.0;
                this.rTypeDirect.IsChecked = true;
            }
            else
            {
                this.rTypeDirect.IsChecked = false;
                this.rTypeKeywords.IsChecked = false;
                this.rTypeVariant.IsChecked = false;
                this.lblLink.IsEnabled = false;
                this.lblLink.Opacity = 0.6;
                this.lblVariant.IsEnabled = false;
                this.lblVariant.Opacity = 0.6;
                this.lblPositive.IsEnabled = false;
                this.lblPositive.Opacity = 0.6;
                this.lblNegative.IsEnabled = false;
                this.lblNegative.Opacity = 0.6;
                this.txtLink.IsEnabled = false;
                this.txtLink.Opacity = 0.6;
                this.txtVariant.IsEnabled = false;
                this.txtVariant.Opacity = 0.6;
                this.txtPositiveKws.IsEnabled = false;
                this.txtPositiveKws.Opacity = 0.6;
                this.txtNegativeKws.IsEnabled = false;
                this.txtNegativeKws.Opacity = 0.6;
                this.rTypeDirect.IsEnabled = false;
                this.rTypeDirect.Opacity = 0.6;
                this.rTypeKeywords.IsEnabled = false;
                this.rTypeKeywords.Opacity = 0.6;
                this.rTypeVariant.IsEnabled = false;
                this.rTypeVariant.Opacity = 0.6;
            }
        }

        [GeneratedCode("PresentationBuildTasks", "4.0.0.0"), DebuggerNonUserCode, EditorBrowsable(EditorBrowsableState.Never)]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    this.switchCheckout = (ToggleButton) target;
                    this.switchCheckout.Checked += new RoutedEventHandler(this.switchCheckout_Checked);
                    this.switchCheckout.Unchecked += new RoutedEventHandler(this.switchCheckout_Checked);
                    return;

                case 2:
                    this.radioButton_0 = (RadioButton) target;
                    return;

                case 3:
                    this.radioButton_1 = (RadioButton) target;
                    return;

                case 4:
                    this.switchAtc = (ToggleButton) target;
                    this.switchAtc.Checked += new RoutedEventHandler(this.switchAtc_Checked);
                    this.switchAtc.Unchecked += new RoutedEventHandler(this.switchAtc_Checked);
                    return;

                case 5:
                    this.rAtcFrontend = (RadioButton) target;
                    return;

                case 6:
                    this.rAtcBackend = (RadioButton) target;
                    return;

                case 7:
                    this.switchLogin = (ToggleButton) target;
                    this.switchLogin.Checked += new RoutedEventHandler(this.switchLogin_Checked);
                    this.switchLogin.Unchecked += new RoutedEventHandler(this.switchLogin_Checked);
                    return;

                case 8:
                    this.lblUsername = (TextBlock) target;
                    return;

                case 9:
                    this.txtUsername = (TextBox) target;
                    return;

                case 10:
                    this.lblPassword = (TextBlock) target;
                    return;

                case 11:
                    this.txtPassword = (TextBox) target;
                    return;

                case 12:
                    this.switchTaskType = (ToggleButton) target;
                    this.switchTaskType.Checked += new RoutedEventHandler(this.switchTaskType_Checked);
                    this.switchTaskType.Unchecked += new RoutedEventHandler(this.switchTaskType_Checked);
                    return;

                case 13:
                    this.rTypeDirect = (RadioButton) target;
                    this.rTypeDirect.Checked += new RoutedEventHandler(this.rType_Checked);
                    this.rTypeDirect.Unchecked += new RoutedEventHandler(this.rType_Checked);
                    return;

                case 14:
                    this.rTypeKeywords = (RadioButton) target;
                    this.rTypeKeywords.Checked += new RoutedEventHandler(this.rType_Checked);
                    this.rTypeKeywords.Unchecked += new RoutedEventHandler(this.rType_Checked);
                    return;

                case 15:
                    this.rTypeVariant = (RadioButton) target;
                    this.rTypeVariant.Checked += new RoutedEventHandler(this.rType_Checked);
                    this.rTypeVariant.Unchecked += new RoutedEventHandler(this.rType_Checked);
                    return;

                case 0x10:
                    this.lblLink = (TextBlock) target;
                    return;

                case 0x11:
                    this.txtLink = (TextBox) target;
                    return;

                case 0x12:
                    this.lblVariant = (TextBlock) target;
                    return;

                case 0x13:
                    this.txtVariant = (TextBox) target;
                    return;

                case 20:
                    this.lblPositive = (TextBlock) target;
                    return;

                case 0x15:
                    this.txtPositiveKws = (TextBox) target;
                    return;

                case 0x16:
                    this.lblNegative = (TextBlock) target;
                    return;

                case 0x17:
                    this.txtNegativeKws = (TextBox) target;
                    return;
            }
            this._contentLoaded = true;
        }
    }
}

