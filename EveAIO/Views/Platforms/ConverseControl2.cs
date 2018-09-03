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

    public class ConverseControl2 : UserControl, IComponentConnector
    {
        private TaskWindow2 _taskWindow;
        public RadioButton rTypeDirect;
        public RadioButton rTypeKeywords;
        internal TextBlock lblLink;
        public TextBox txtLink;
        public TextBox txtPositiveKws;
        internal TextBlock lblColor;
        internal StackPanel pColorPick;
        public TextBox txtColor;
        public RadioButton rColorExact;
        public RadioButton rColorContains;
        public CheckBox chPickRandomColorNotAvailable;
        private bool _contentLoaded;

        public ConverseControl2(TaskWindow2 taskWindow)
        {
            Class7.RIuqtBYzWxthF();
            this.InitializeComponent();
            this._taskWindow = taskWindow;
            this.rColorContains.IsChecked = true;
            this.rTypeDirect.IsChecked = true;
        }

        public bool Check()
        {
            bool flag = true;
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
                Uri resourceLocator = new Uri("/EveAIO;component/views/platforms/conversecontrol2.xaml", UriKind.Relative);
                Application.LoadComponent(this, resourceLocator);
            }
        }

        private void rType_Checked(object sender, RoutedEventArgs e)
        {
            if (this.rTypeDirect.IsChecked.HasValue && this.rTypeDirect.IsChecked.Value)
            {
                this.lblLink.Text = "DIRECT LINK:";
                this.txtLink.Opacity = 1.0;
                this.txtPositiveKws.IsEnabled = false;
                this.txtPositiveKws.Opacity = 0.6;
                this.txtPositiveKws.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                this.txtPositiveKws.Clear();
            }
            else
            {
                this.lblLink.Text = "SEARCH PAGE:";
                this.txtPositiveKws.IsEnabled = true;
                this.txtPositiveKws.Opacity = 1.0;
                this.txtPositiveKws.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
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
                    this.lblColor = (TextBlock) target;
                    return;

                case 7:
                    this.pColorPick = (StackPanel) target;
                    return;

                case 8:
                    this.txtColor = (TextBox) target;
                    return;

                case 9:
                    this.rColorExact = (RadioButton) target;
                    return;

                case 10:
                    this.rColorContains = (RadioButton) target;
                    return;

                case 11:
                    this.chPickRandomColorNotAvailable = (CheckBox) target;
                    return;
            }
            this._contentLoaded = true;
        }
    }
}

