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

    public class BackdoorControl2 : UserControl, IComponentConnector
    {
        private TaskWindow2 _taskWindow;
        public RadioButton rTypeDirect;
        public RadioButton rTypeKeywords;
        internal TextBlock lblLink;
        public TextBox txtLink;
        internal TextBlock lblKeywords;
        public TextBox txtPositiveKws;
        private bool _contentLoaded;

        public BackdoorControl2(TaskWindow2 taskWindow)
        {
            Class7.RIuqtBYzWxthF();
            this.InitializeComponent();
            this._taskWindow = taskWindow;
            this.rTypeDirect.IsChecked = true;
        }

        public bool Check()
        {
            bool flag = true;
            if (this.rTypeDirect.IsChecked.HasValue && this.rTypeDirect.IsChecked.Value)
            {
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

        [DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocator = new Uri("/EveAIO;component/views/platforms/backdoorcontrol2.xaml", UriKind.Relative);
                Application.LoadComponent(this, resourceLocator);
            }
        }

        private void rType_Checked(object sender, RoutedEventArgs e)
        {
            if (this.rTypeDirect.IsChecked.HasValue && this.rTypeDirect.IsChecked.Value)
            {
                this.lblLink.Opacity = 1.0;
                this.lblLink.IsEnabled = true;
                this.txtLink.Opacity = 1.0;
                this.txtLink.IsEnabled = true;
                this.lblKeywords.Opacity = 0.6;
                this.lblKeywords.IsEnabled = false;
                this.txtPositiveKws.Opacity = 0.6;
                this.txtPositiveKws.IsEnabled = false;
                this.txtPositiveKws.Clear();
            }
            else
            {
                this.txtLink.Text = "default search";
                this.lblLink.Opacity = 0.6;
                this.lblLink.IsEnabled = false;
                this.txtLink.Opacity = 0.6;
                this.txtLink.IsEnabled = false;
                this.lblKeywords.Opacity = 1.0;
                this.lblKeywords.IsEnabled = true;
                this.txtPositiveKws.Opacity = 1.0;
                this.txtPositiveKws.IsEnabled = true;
                this.txtLink.Clear();
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
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
                    this.lblKeywords = (TextBlock) target;
                    return;

                case 6:
                    this.txtPositiveKws = (TextBox) target;
                    return;
            }
            this._contentLoaded = true;
        }
    }
}

