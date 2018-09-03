namespace EveAIO.Views.Platforms
{
    using EveAIO.Views;
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

    public class BoxlunchControl2 : UserControl, IComponentConnector
    {
        private TaskWindow2 _taskWindow;
        public RadioButton rTypeDirect;
        public RadioButton rTypeKeywords;
        public TextBox txtQuantity;
        public TextBox txtCouponCode;
        internal TextBlock lblLink;
        public TextBox txtLink;
        public TextBox txtPositiveKws;
        internal TextBlock lblNegativeKeywords;
        public TextBox txtNegativeKeywords;
        internal GroupBox gbLogin;
        public ToggleButton switchLogin;
        public TextBox txtUsername;
        public TextBox txtPassword;
        private bool _contentLoaded;

        public BoxlunchControl2(TaskWindow2 taskWindow)
        {
            Class7.RIuqtBYzWxthF();
            this.InitializeComponent();
            this._taskWindow = taskWindow;
            this.rTypeDirect.IsChecked = true;
        }

        public bool Check()
        {
            bool flag = true;
            if (this.switchLogin.IsChecked.HasValue && this.switchLogin.IsChecked.Value)
            {
                if (!string.IsNullOrEmpty(this.txtUsername.Text))
                {
                    this.txtUsername.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                }
                else
                {
                    this.txtUsername.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                    flag = false;
                }
                if (string.IsNullOrEmpty(this.txtPassword.Text))
                {
                    this.txtPassword.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                    flag = false;
                }
                else
                {
                    this.txtPassword.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                }
            }
            else
            {
                this.txtUsername.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                this.txtPassword.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
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
                this.txtLink.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                return flag;
            }
            if (!string.IsNullOrEmpty(this.txtLink.Text))
            {
                this.txtLink.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
            }
            else
            {
                this.txtLink.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                flag = false;
            }
            this.txtPositiveKws.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
            return flag;
        }

        [DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocator = new Uri("/EveAIO;component/views/platforms/boxlunchcontrol2.xaml", UriKind.Relative);
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
                this.lblLink.Text = "DIRECT LINK:";
                this.txtLink.Text = "";
                this.txtLink.IsEnabled = true;
                this.txtLink.Opacity = 1.0;
                this.txtPositiveKws.IsEnabled = false;
                this.txtPositiveKws.Opacity = 0.6;
                this.txtPositiveKws.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                this.txtNegativeKeywords.IsEnabled = false;
                this.txtNegativeKeywords.Opacity = 0.6;
                this.txtPositiveKws.Clear();
                this.txtNegativeKeywords.Clear();
            }
            else
            {
                this.lblLink.Text = "SEARCH PAGE:";
                this.txtLink.Text = "default search";
                this.txtLink.IsEnabled = false;
                this.txtLink.Opacity = 0.6;
                this.txtPositiveKws.IsEnabled = true;
                this.txtPositiveKws.Opacity = 1.0;
                this.txtPositiveKws.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                this.txtNegativeKeywords.IsEnabled = true;
                this.txtNegativeKeywords.Opacity = 1.0;
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
                this.txtUsername.IsEnabled = false;
                this.txtPassword.Opacity = 0.6;
                this.txtPassword.IsEnabled = false;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), GeneratedCode("PresentationBuildTasks", "4.0.0.0"), DebuggerNonUserCode]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    this.rTypeDirect = (RadioButton) target;
                    this.rTypeDirect.Checked += new RoutedEventHandler(this.rType_Checked);
                    this.rTypeDirect.Unchecked += new RoutedEventHandler(this.rType_Checked);
                    return;

                case 2:
                    this.rTypeKeywords = (RadioButton) target;
                    this.rTypeKeywords.Checked += new RoutedEventHandler(this.rType_Checked);
                    this.rTypeKeywords.Unchecked += new RoutedEventHandler(this.rType_Checked);
                    return;

                case 3:
                    this.txtQuantity = (TextBox) target;
                    this.txtQuantity.PreviewTextInput += new TextCompositionEventHandler(this.NumberCheck);
                    return;

                case 4:
                    this.txtCouponCode = (TextBox) target;
                    return;

                case 5:
                    this.lblLink = (TextBlock) target;
                    return;

                case 6:
                    this.txtLink = (TextBox) target;
                    return;

                case 7:
                    this.txtPositiveKws = (TextBox) target;
                    return;

                case 8:
                    this.lblNegativeKeywords = (TextBlock) target;
                    return;

                case 9:
                    this.txtNegativeKeywords = (TextBox) target;
                    return;

                case 10:
                    this.gbLogin = (GroupBox) target;
                    return;

                case 11:
                    this.switchLogin = (ToggleButton) target;
                    this.switchLogin.Checked += new RoutedEventHandler(this.switchLogin_Checked);
                    this.switchLogin.Unchecked += new RoutedEventHandler(this.switchLogin_Checked);
                    return;

                case 12:
                    this.txtUsername = (TextBox) target;
                    return;

                case 13:
                    this.txtPassword = (TextBox) target;
                    return;
            }
            this._contentLoaded = true;
        }
    }
}

