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

    public class SoleboxMassEditControl : UserControl, IComponentConnector
    {
        public ToggleButton switchDirectLink;
        public TextBox txtLink;
        public ToggleButton switchLogin;
        internal TextBlock lblUsername;
        public TextBox txtUsername;
        internal TextBlock lblPassword;
        public TextBox txtPassword;
        public ToggleButton switchPayment;
        public RadioButton radioButton_0;
        public RadioButton radioButton_1;
        private bool _contentLoaded;

        public SoleboxMassEditControl()
        {
            Class7.RIuqtBYzWxthF();
            this.InitializeComponent();
            this.radioButton_0.IsChecked = true;
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
                    flag = false;
                }
                else
                {
                    this.txtPassword.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
                }
            }
            else
            {
                this.txtUsername.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
                this.txtPassword.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
            }
            if (!this.switchDirectLink.IsChecked.HasValue || !this.switchDirectLink.IsChecked.Value)
            {
                return flag;
            }
            if (!string.IsNullOrEmpty(this.txtLink.Text))
            {
                this.txtLink.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
                return flag;
            }
            this.txtLink.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
            return false;
        }

        [DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocator = new Uri("/EveAIO;component/views/massedit/soleboxmasseditcontrol.xaml", UriKind.Relative);
                Application.LoadComponent(this, resourceLocator);
            }
        }

        private void switchDirectLink_Checked(object sender, RoutedEventArgs e)
        {
            if (this.switchDirectLink.IsChecked.HasValue && this.switchDirectLink.IsChecked.Value)
            {
                this.txtLink.IsEnabled = true;
                this.txtLink.Opacity = 1.0;
            }
            else
            {
                this.txtLink.IsEnabled = false;
                this.txtLink.Opacity = 0.6;
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

        private void switchPayment_Checked(object sender, RoutedEventArgs e)
        {
            if (this.switchPayment.IsChecked.HasValue && this.switchPayment.IsChecked.Value)
            {
                this.radioButton_0.IsEnabled = true;
                this.radioButton_0.Opacity = 1.0;
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

        [DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0"), EditorBrowsable(EditorBrowsableState.Never)]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    this.switchDirectLink = (ToggleButton) target;
                    this.switchDirectLink.Checked += new RoutedEventHandler(this.switchDirectLink_Checked);
                    this.switchDirectLink.Unchecked += new RoutedEventHandler(this.switchDirectLink_Checked);
                    return;

                case 2:
                    this.txtLink = (TextBox) target;
                    return;

                case 3:
                    this.switchLogin = (ToggleButton) target;
                    this.switchLogin.Checked += new RoutedEventHandler(this.switchLogin_Checked);
                    this.switchLogin.Unchecked += new RoutedEventHandler(this.switchLogin_Checked);
                    return;

                case 4:
                    this.lblUsername = (TextBlock) target;
                    return;

                case 5:
                    this.txtUsername = (TextBox) target;
                    return;

                case 6:
                    this.lblPassword = (TextBlock) target;
                    return;

                case 7:
                    this.txtPassword = (TextBox) target;
                    return;

                case 8:
                    this.switchPayment = (ToggleButton) target;
                    this.switchPayment.Checked += new RoutedEventHandler(this.switchPayment_Checked);
                    this.switchPayment.Unchecked += new RoutedEventHandler(this.switchPayment_Checked);
                    return;

                case 9:
                    this.radioButton_0 = (RadioButton) target;
                    return;

                case 10:
                    this.radioButton_1 = (RadioButton) target;
                    return;
            }
            this._contentLoaded = true;
        }
    }
}

