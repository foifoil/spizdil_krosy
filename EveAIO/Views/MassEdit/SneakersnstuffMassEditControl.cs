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

    public class SneakersnstuffMassEditControl : UserControl, IComponentConnector
    {
        public ToggleButton switchTaskType;
        public RadioButton rTypeDirect;
        public RadioButton rTypeKeywords;
        internal TextBlock lblLink;
        public TextBox txtLink;
        internal TextBlock lblPositive;
        public TextBox txtPositiveKws;
        internal TextBlock lblColor;
        public TextBox txtColor;
        public RadioButton rColorExact;
        public RadioButton rColorContains;
        public ToggleButton switchLogin;
        internal TextBlock lblUsername;
        public TextBox txtUsername;
        internal TextBlock lblPassword;
        public TextBox txtPassword;
        private bool _contentLoaded;

        public SneakersnstuffMassEditControl()
        {
            Class7.RIuqtBYzWxthF();
            this.InitializeComponent();
        }

        public bool Check()
        {
            bool flag = true;
            if (this.switchLogin.IsChecked.HasValue && this.switchLogin.IsChecked.Value)
            {
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

        [DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocator = new Uri("/EveAIO;component/views/massedit/sneakersnstuffmasseditcontrol.xaml", UriKind.Relative);
                Application.LoadComponent(this, resourceLocator);
            }
        }

        private void rType_Checked(object sender, RoutedEventArgs e)
        {
            if (this.rTypeDirect.IsChecked.HasValue && this.rTypeDirect.IsChecked.Value)
            {
                this.lblLink.Text = "Direct link:";
                this.txtLink.Text = "";
                this.txtLink.IsEnabled = true;
                this.txtLink.Opacity = 1.0;
                this.txtPositiveKws.IsEnabled = false;
                this.txtPositiveKws.Opacity = 0.6;
                this.txtPositiveKws.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
                this.txtColor.IsEnabled = false;
                this.txtColor.Opacity = 0.6;
                this.rColorContains.IsEnabled = false;
                this.rColorContains.Opacity = 0.6;
                this.rColorExact.IsEnabled = false;
                this.rColorExact.Opacity = 0.6;
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
                this.txtColor.IsEnabled = true;
                this.txtColor.Opacity = 1.0;
                this.rColorContains.IsEnabled = true;
                this.rColorContains.Opacity = 1.0;
                this.rColorExact.IsEnabled = true;
                this.rColorExact.Opacity = 1.0;
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
                this.rTypeDirect.IsEnabled = true;
                this.rTypeDirect.Opacity = 1.0;
                this.rTypeKeywords.IsEnabled = true;
                this.rTypeKeywords.Opacity = 1.0;
                this.lblLink.IsEnabled = true;
                this.lblLink.Opacity = 1.0;
                this.txtLink.IsEnabled = true;
                this.txtLink.Opacity = 1.0;
                this.lblColor.IsEnabled = true;
                this.lblColor.Opacity = 1.0;
                this.txtColor.IsEnabled = true;
                this.txtColor.Opacity = 1.0;
                this.rColorExact.IsEnabled = true;
                this.rColorExact.Opacity = 1.0;
                this.rColorExact.IsChecked = true;
                this.rColorContains.IsEnabled = true;
                this.rColorContains.Opacity = 1.0;
                this.lblPositive.IsEnabled = true;
                this.lblPositive.Opacity = 1.0;
                this.txtPositiveKws.IsEnabled = true;
                this.txtPositiveKws.Opacity = 1.0;
                this.rTypeDirect.IsChecked = true;
            }
            else
            {
                this.rTypeDirect.IsChecked = false;
                this.rTypeKeywords.IsChecked = false;
                this.rTypeDirect.IsEnabled = false;
                this.rTypeDirect.Opacity = 0.6;
                this.rTypeKeywords.IsEnabled = false;
                this.rTypeKeywords.Opacity = 0.6;
                this.lblLink.IsEnabled = false;
                this.lblLink.Opacity = 0.6;
                this.txtLink.IsEnabled = false;
                this.txtLink.Opacity = 0.6;
                this.lblColor.IsEnabled = false;
                this.lblColor.Opacity = 0.6;
                this.txtColor.IsEnabled = false;
                this.txtColor.Opacity = 0.6;
                this.rColorExact.IsEnabled = false;
                this.rColorExact.Opacity = 0.6;
                this.rColorExact.IsChecked = false;
                this.rColorContains.IsEnabled = false;
                this.rColorContains.Opacity = 0.6;
                this.lblPositive.IsEnabled = false;
                this.lblPositive.Opacity = 0.6;
                this.txtPositiveKws.IsEnabled = false;
                this.txtPositiveKws.Opacity = 0.6;
            }
        }

        [DebuggerNonUserCode, EditorBrowsable(EditorBrowsableState.Never), GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    this.switchTaskType = (ToggleButton) target;
                    this.switchTaskType.Checked += new RoutedEventHandler(this.switchTaskType_Checked);
                    this.switchTaskType.Unchecked += new RoutedEventHandler(this.switchTaskType_Checked);
                    return;

                case 2:
                    this.rTypeDirect = (RadioButton) target;
                    this.rTypeDirect.Checked += new RoutedEventHandler(this.rType_Checked);
                    this.rTypeDirect.Unchecked += new RoutedEventHandler(this.rType_Checked);
                    return;

                case 3:
                    this.rTypeKeywords = (RadioButton) target;
                    this.rTypeKeywords.Checked += new RoutedEventHandler(this.rType_Checked);
                    this.rTypeKeywords.Unchecked += new RoutedEventHandler(this.rType_Checked);
                    return;

                case 4:
                    this.lblLink = (TextBlock) target;
                    return;

                case 5:
                    this.txtLink = (TextBox) target;
                    return;

                case 6:
                    this.lblPositive = (TextBlock) target;
                    return;

                case 7:
                    this.txtPositiveKws = (TextBox) target;
                    return;

                case 8:
                    this.lblColor = (TextBlock) target;
                    return;

                case 9:
                    this.txtColor = (TextBox) target;
                    return;

                case 10:
                    this.rColorExact = (RadioButton) target;
                    return;

                case 11:
                    this.rColorContains = (RadioButton) target;
                    return;

                case 12:
                    this.switchLogin = (ToggleButton) target;
                    this.switchLogin.Checked += new RoutedEventHandler(this.switchLogin_Checked);
                    this.switchLogin.Unchecked += new RoutedEventHandler(this.switchLogin_Checked);
                    return;

                case 13:
                    this.lblUsername = (TextBlock) target;
                    return;

                case 14:
                    this.txtUsername = (TextBox) target;
                    return;

                case 15:
                    this.lblPassword = (TextBlock) target;
                    return;

                case 0x10:
                    this.txtPassword = (TextBox) target;
                    return;
            }
            this._contentLoaded = true;
        }
    }
}

