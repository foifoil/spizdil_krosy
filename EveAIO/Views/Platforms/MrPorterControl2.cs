namespace EveAIO.Views.Platforms
{
    using EveAIO.Views;
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Markup;
    using System.Windows.Media;

    public class MrPorterControl2 : UserControl, IComponentConnector
    {
        private TaskWindow2 _taskWindow;
        internal TextBlock lblLink;
        public TextBox txtLink;
        public RadioButton rRegionUsa;
        public RadioButton rRegionIntl;
        public ToggleButton switchLogin;
        internal TextBlock lblUsername;
        public TextBox txtUsername;
        internal TextBlock lblPassword;
        public TextBox txtPassword;
        private bool _contentLoaded;

        public MrPorterControl2(TaskWindow2 taskWindow)
        {
            Class7.RIuqtBYzWxthF();
            this.InitializeComponent();
            this._taskWindow = taskWindow;
            this.rRegionUsa.IsChecked = true;
        }

        public bool Check()
        {
            bool flag = true;
            if (!string.IsNullOrEmpty(this.txtLink.Text))
            {
                this.txtLink.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                return flag;
            }
            this.txtLink.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
            return false;
        }

        [GeneratedCode("PresentationBuildTasks", "4.0.0.0"), DebuggerNonUserCode]
        public void InitializeComponent()
        {
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocator = new Uri("/EveAIO;component/views/platforms/mrportercontrol2.xaml", UriKind.Relative);
                Application.LoadComponent(this, resourceLocator);
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
                this.lblUsername.Opacity = 1.0;
                this.lblPassword.Opacity = 1.0;
            }
            else
            {
                this.txtUsername.Opacity = 0.6;
                this.txtUsername.IsEnabled = false;
                this.txtUsername.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                this.txtPassword.Opacity = 0.6;
                this.txtPassword.IsEnabled = false;
                this.txtPassword.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                this.lblUsername.Opacity = 0.6;
                this.lblPassword.Opacity = 0.6;
            }
        }

        [GeneratedCode("PresentationBuildTasks", "4.0.0.0"), DebuggerNonUserCode, EditorBrowsable(EditorBrowsableState.Never)]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    this.lblLink = (TextBlock) target;
                    return;

                case 2:
                    this.txtLink = (TextBox) target;
                    return;

                case 3:
                    this.rRegionUsa = (RadioButton) target;
                    return;

                case 4:
                    this.rRegionIntl = (RadioButton) target;
                    return;

                case 5:
                    this.switchLogin = (ToggleButton) target;
                    this.switchLogin.Checked += new RoutedEventHandler(this.switchLogin_Checked);
                    this.switchLogin.Unchecked += new RoutedEventHandler(this.switchLogin_Checked);
                    return;

                case 6:
                    this.lblUsername = (TextBlock) target;
                    return;

                case 7:
                    this.txtUsername = (TextBox) target;
                    return;

                case 8:
                    this.lblPassword = (TextBlock) target;
                    return;

                case 9:
                    this.txtPassword = (TextBox) target;
                    return;
            }
            this._contentLoaded = true;
        }
    }
}

