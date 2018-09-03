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

    public class AdidasControl : UserControl, IComponentConnector
    {
        private TaskWindow2 _taskWindow;
        public RadioButton rTypeDirect;
        public RadioButton rTypeKeywords;
        internal TextBlock lblLink;
        public TextBox txtLink;
        public TextBox txtPositiveKws;
        public ToggleButton switchLogin;
        public TextBox txtUsername;
        public TextBox txtPassword;
        private bool _contentLoaded;

        public AdidasControl(TaskWindow2 taskWindow)
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
                    this.txtUsername.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
                }
                else
                {
                    this.txtUsername.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                    flag = false;
                }
                if (!string.IsNullOrEmpty(this.txtPassword.Text))
                {
                    this.txtPassword.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
                }
                else
                {
                    this.txtPassword.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                    flag = false;
                }
            }
            else
            {
                this.txtUsername.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
                this.txtPassword.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
            }
            if (this.rTypeKeywords.IsChecked.HasValue && this.rTypeKeywords.IsChecked.Value)
            {
                if (!string.IsNullOrEmpty(this.txtPositiveKws.Text))
                {
                    this.txtPositiveKws.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
                }
                else
                {
                    this.txtPositiveKws.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                    flag = false;
                }
                this.txtLink.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
                return flag;
            }
            if (!string.IsNullOrEmpty(this.txtLink.Text))
            {
                this.txtLink.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
            }
            else
            {
                this.txtLink.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                flag = false;
            }
            this.txtPositiveKws.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
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
            switch (((-535385582 ^ -75133635) % 4))
            {
                case 0:
                    goto Label_000D;

                case 1:
                    break;

                case 2:
                    return;

                case 3:
                    return;

                default:
                    return;
            }
        Label_002C:
            this._contentLoaded = true;
            Uri resourceLocator = new Uri("/EveAIO;component/views/platforms/adidascontrol.xaml", UriKind.Relative);
            Application.LoadComponent(this, resourceLocator);
            goto Label_000D;
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
                this.txtUsername.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
                this.txtPassword.Opacity = 0.6;
                this.txtPassword.IsEnabled = false;
                this.txtPassword.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
            }
        }

        [DebuggerNonUserCode, EditorBrowsable(EditorBrowsableState.Never), GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
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
                    this.lblLink = (TextBlock) target;
                    return;

                case 4:
                    this.txtLink = (TextBox) target;
                    return;

                case 5:
                    this.txtPositiveKws = (TextBox) target;
                    return;

                case 6:
                    this.switchLogin = (ToggleButton) target;
                    this.switchLogin.Checked += new RoutedEventHandler(this.switchLogin_Checked);
                    this.switchLogin.Unchecked += new RoutedEventHandler(this.switchLogin_Checked);
                    return;

                case 7:
                    this.txtUsername = (TextBox) target;
                    return;

                case 8:
                    this.txtPassword = (TextBox) target;
                    return;
            }
            this._contentLoaded = true;
        }
    }
}

