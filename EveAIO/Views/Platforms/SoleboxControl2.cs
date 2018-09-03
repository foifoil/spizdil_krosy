namespace EveAIO.Views.Platforms
{
    using EveAIO.Views;
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Markup;
    using System.Windows.Media;

    public class SoleboxControl2 : UserControl, IComponentConnector
    {
        private TaskWindow2 _taskWindow;
        public TextBox txtLink;
        public TextBox txtUsername;
        public TextBox txtPassword;
        public RadioButton rPaymentCash;
        public RadioButton radioButton_0;
        public RadioButton radioButton_1;
        private bool _contentLoaded;

        public SoleboxControl2(TaskWindow2 taskWindow)
        {
            Class7.RIuqtBYzWxthF();
            this.InitializeComponent();
            this._taskWindow = taskWindow;
            this.radioButton_1.IsChecked = true;
        }

        public bool Check()
        {
            bool flag = true;
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
            // This item is obfuscated and can not be translated.
            if (!this._contentLoaded)
            {
                goto Label_002C;
            }
        Label_000D:
            switch (((-1138027754 ^ -1232820385) % 4))
            {
                case 0:
                    goto Label_000D;

                case 1:
                    return;

                case 3:
                    break;

                default:
                {
                    Uri resourceLocator = new Uri("/EveAIO;component/views/platforms/soleboxcontrol2.xaml", UriKind.Relative);
                    Application.LoadComponent(this, resourceLocator);
                    return;
                }
            }
        Label_002C:
            this._contentLoaded = true;
            goto Label_000D;
        }

        private void Payment_checked(object sender, RoutedEventArgs e)
        {
            if (this.radioButton_0.IsChecked.HasValue && this.radioButton_0.IsChecked.Value)
            {
                this._taskWindow._advancedView.chPaypalProxyIgnore.Opacity = 1.0;
                this._taskWindow._advancedView.chPaypalProxyIgnore.IsEnabled = true;
            }
            else
            {
                this._taskWindow._advancedView.chPaypalProxyIgnore.Opacity = 0.6;
                this._taskWindow._advancedView.chPaypalProxyIgnore.IsEnabled = false;
            }
        }

        [DebuggerNonUserCode, EditorBrowsable(EditorBrowsableState.Never), GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    this.txtLink = (TextBox) target;
                    this.txtLink.TextChanged += new TextChangedEventHandler(this.txtLink_TextChanged);
                    return;

                case 2:
                    this.txtUsername = (TextBox) target;
                    return;

                case 3:
                    this.txtPassword = (TextBox) target;
                    return;

                case 4:
                    this.rPaymentCash = (RadioButton) target;
                    this.rPaymentCash.Checked += new RoutedEventHandler(this.Payment_checked);
                    this.rPaymentCash.Unchecked += new RoutedEventHandler(this.Payment_checked);
                    return;

                case 5:
                    this.radioButton_0 = (RadioButton) target;
                    this.radioButton_0.Checked += new RoutedEventHandler(this.Payment_checked);
                    this.radioButton_0.Unchecked += new RoutedEventHandler(this.Payment_checked);
                    return;

                case 6:
                    this.radioButton_1 = (RadioButton) target;
                    this.radioButton_1.Checked += new RoutedEventHandler(this.Payment_checked);
                    this.radioButton_1.Unchecked += new RoutedEventHandler(this.Payment_checked);
                    return;
            }
            this._contentLoaded = true;
        }

        private void txtLink_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.txtLink.Text.Contains("aid="))
            {
                this._taskWindow._generalView.gbSizing.IsEnabled = false;
                this._taskWindow._generalView.gbSizing.Opacity = 0.6;
                this._taskWindow._generalView.chSizeRandom.IsChecked = true;
            }
            else
            {
                this._taskWindow._generalView.gbSizing.IsEnabled = true;
                this._taskWindow._generalView.gbSizing.Opacity = 1.0;
            }
        }
    }
}

