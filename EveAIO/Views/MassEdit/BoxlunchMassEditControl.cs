namespace EveAIO.Views.MassEdit
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Markup;
    using System.Windows.Media;

    public class BoxlunchMassEditControl : UserControl, IComponentConnector
    {
        public ToggleButton switchQuantity;
        internal TextBlock lblQuantity;
        public TextBox txtQuantity;
        public ToggleButton switchCoupon;
        internal TextBlock lblCouponCode;
        public TextBox txtCouponCode;
        public ToggleButton switchLogin;
        internal TextBlock lblUsername;
        public TextBox txtUsername;
        internal TextBlock lblPassword;
        public TextBox txtPassword;
        public ToggleButton switchTaskType;
        public RadioButton rTypeDirect;
        public RadioButton rTypeKeywords;
        internal TextBlock lblLink;
        public TextBox txtLink;
        internal TextBlock lblPositive;
        public TextBox txtPositiveKws;
        internal TextBlock lblNegative;
        public TextBox txtNegativeKws;
        private bool _contentLoaded;

        public BoxlunchMassEditControl()
        {
            Class7.RIuqtBYzWxthF();
            this.InitializeComponent();
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
                    this.txtUsername.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
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
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocator = new Uri("/EveAIO;component/views/massedit/boxlunchmasseditcontrol.xaml", UriKind.Relative);
                Application.LoadComponent(this, resourceLocator);
            }
        }

        private void NumberCheck(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        private void rType_Checked(object sender, RoutedEventArgs e)
        {
            if (this.rTypeDirect.IsChecked.HasValue && this.rTypeDirect.IsChecked.Value)
            {
                this.lblLink.Text = "Direct link:";
                this.txtLink.Clear();
                this.txtLink.IsEnabled = true;
                this.txtLink.Opacity = 1.0;
                this.txtPositiveKws.IsEnabled = false;
                this.txtPositiveKws.Opacity = 0.6;
                this.txtPositiveKws.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
                this.txtNegativeKws.IsEnabled = false;
                this.txtNegativeKws.Opacity = 0.6;
                this.txtNegativeKws.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
                this.txtPositiveKws.Clear();
                this.txtNegativeKws.Clear();
            }
            else
            {
                this.lblLink.Text = "Search page:";
                this.txtLink.Text = "default search";
                this.txtLink.IsEnabled = false;
                this.txtLink.Opacity = 0.6;
                this.txtPositiveKws.IsEnabled = true;
                this.txtPositiveKws.Opacity = 1.0;
                this.txtPositiveKws.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
                this.txtNegativeKws.IsEnabled = true;
                this.txtNegativeKws.Opacity = 1.0;
                this.txtNegativeKws.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
            }
        }

        private void switchCoupon_Checked(object sender, RoutedEventArgs e)
        {
            if (this.switchCoupon.IsChecked.HasValue && this.switchCoupon.IsChecked.Value)
            {
                this.lblCouponCode.IsEnabled = true;
                this.lblCouponCode.Opacity = 1.0;
                this.txtCouponCode.IsEnabled = true;
                this.txtCouponCode.Opacity = 1.0;
            }
            else
            {
                this.lblCouponCode.IsEnabled = false;
                this.lblCouponCode.Opacity = 0.6;
                this.txtCouponCode.IsEnabled = false;
                this.txtCouponCode.Opacity = 0.6;
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

        private void switchQuantity_Checked(object sender, RoutedEventArgs e)
        {
            if (this.switchQuantity.IsChecked.HasValue && this.switchQuantity.IsChecked.Value)
            {
                this.lblQuantity.IsEnabled = true;
                this.lblQuantity.Opacity = 1.0;
                this.txtQuantity.IsEnabled = true;
                this.txtQuantity.Opacity = 1.0;
            }
            else
            {
                this.lblQuantity.IsEnabled = false;
                this.lblQuantity.Opacity = 0.6;
                this.txtQuantity.IsEnabled = false;
                this.txtQuantity.Opacity = 0.6;
            }
        }

        private void switchTaskType_Checked(object sender, RoutedEventArgs e)
        {
            if (this.switchTaskType.IsChecked.HasValue && this.switchTaskType.IsChecked.Value)
            {
                this.lblLink.IsEnabled = true;
                this.lblLink.Opacity = 1.0;
                this.lblPositive.IsEnabled = true;
                this.lblPositive.Opacity = 1.0;
                this.lblNegative.IsEnabled = true;
                this.lblNegative.Opacity = 1.0;
                this.txtLink.IsEnabled = true;
                this.txtLink.Opacity = 1.0;
                this.txtPositiveKws.IsEnabled = true;
                this.txtPositiveKws.Opacity = 1.0;
                this.txtNegativeKws.IsEnabled = true;
                this.txtNegativeKws.Opacity = 1.0;
                this.rTypeDirect.IsEnabled = true;
                this.rTypeDirect.Opacity = 1.0;
                this.rTypeKeywords.IsEnabled = true;
                this.rTypeKeywords.Opacity = 1.0;
                this.rTypeDirect.IsChecked = true;
            }
            else
            {
                this.rTypeDirect.IsChecked = false;
                this.rTypeKeywords.IsChecked = false;
                this.lblLink.IsEnabled = false;
                this.lblLink.Opacity = 0.6;
                this.lblPositive.IsEnabled = false;
                this.lblPositive.Opacity = 0.6;
                this.lblNegative.IsEnabled = false;
                this.lblNegative.Opacity = 0.6;
                this.txtLink.IsEnabled = false;
                this.txtLink.Opacity = 0.6;
                this.txtPositiveKws.IsEnabled = false;
                this.txtPositiveKws.Opacity = 0.6;
                this.txtNegativeKws.IsEnabled = false;
                this.txtNegativeKws.Opacity = 0.6;
                this.rTypeDirect.IsEnabled = false;
                this.rTypeDirect.Opacity = 0.6;
                this.rTypeKeywords.IsEnabled = false;
                this.rTypeKeywords.Opacity = 0.6;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), GeneratedCode("PresentationBuildTasks", "4.0.0.0"), DebuggerNonUserCode]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    this.switchQuantity = (ToggleButton) target;
                    this.switchQuantity.Checked += new RoutedEventHandler(this.switchQuantity_Checked);
                    this.switchQuantity.Unchecked += new RoutedEventHandler(this.switchQuantity_Checked);
                    return;

                case 2:
                    this.lblQuantity = (TextBlock) target;
                    return;

                case 3:
                    this.txtQuantity = (TextBox) target;
                    this.txtQuantity.PreviewTextInput += new TextCompositionEventHandler(this.NumberCheck);
                    return;

                case 4:
                    this.switchCoupon = (ToggleButton) target;
                    this.switchCoupon.Checked += new RoutedEventHandler(this.switchCoupon_Checked);
                    this.switchCoupon.Unchecked += new RoutedEventHandler(this.switchCoupon_Checked);
                    return;

                case 5:
                    this.lblCouponCode = (TextBlock) target;
                    return;

                case 6:
                    this.txtCouponCode = (TextBox) target;
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
                    this.lblLink = (TextBlock) target;
                    return;

                case 0x10:
                    this.txtLink = (TextBox) target;
                    return;

                case 0x11:
                    this.lblPositive = (TextBlock) target;
                    return;

                case 0x12:
                    this.txtPositiveKws = (TextBox) target;
                    return;

                case 0x13:
                    this.lblNegative = (TextBlock) target;
                    return;

                case 20:
                    this.txtNegativeKws = (TextBox) target;
                    return;
            }
            this._contentLoaded = true;
        }
    }
}

