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

    public class NordstromControl2 : UserControl, IComponentConnector
    {
        private TaskWindow2 _taskWindow;
        public RadioButton rTypeDirect;
        public RadioButton rTypeVariant;
        internal TextBlock lblLink;
        public TextBox txtLink;
        internal TextBlock lblSkuId;
        public TextBox txtSkuId;
        internal TextBlock lblColor;
        internal TextBlock lblStyleId;
        public TextBox txtColor;
        public TextBox txtStyleId;
        internal StackPanel pColorPick;
        public RadioButton rColorExact;
        public RadioButton rColorContains;
        public CheckBox chPickRandomColorNotAvailable;
        private bool _contentLoaded;

        public NordstromControl2(TaskWindow2 taskWindow)
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
            if (this.rTypeDirect.IsChecked.HasValue && this.rTypeDirect.IsChecked.Value)
            {
                if (string.IsNullOrEmpty(this.txtLink.Text))
                {
                    this.txtLink.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                    return false;
                }
                this.txtLink.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                return flag;
            }
            if (string.IsNullOrEmpty(this.txtSkuId.Text))
            {
                this.txtSkuId.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                flag = false;
            }
            else
            {
                this.txtSkuId.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
            }
            if (string.IsNullOrEmpty(this.txtStyleId.Text))
            {
                this.txtStyleId.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                return false;
            }
            this.txtStyleId.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
            return flag;
        }

        [DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocator = new Uri("/EveAIO;component/views/platforms/nordstromcontrol2.xaml", UriKind.Relative);
                Application.LoadComponent(this, resourceLocator);
            }
        }

        private void rType_Checked(object sender, RoutedEventArgs e)
        {
            if (this.rTypeDirect.IsChecked.Value)
            {
                this._taskWindow._generalView.gbSizing.IsEnabled = true;
                this._taskWindow._generalView.gbSizing.Opacity = 1.0;
            }
            else
            {
                this._taskWindow._generalView.gbSizing.IsEnabled = false;
                this._taskWindow._generalView.gbSizing.Opacity = 0.6;
                this._taskWindow._generalView.chSizeRandom.IsChecked = true;
            }
            this.lblLink.Visibility = this.rTypeDirect.IsChecked.Value ? Visibility.Visible : Visibility.Collapsed;
            this.txtLink.Visibility = this.rTypeDirect.IsChecked.Value ? Visibility.Visible : Visibility.Collapsed;
            this.lblColor.Visibility = this.rTypeDirect.IsChecked.Value ? Visibility.Visible : Visibility.Collapsed;
            this.txtColor.Visibility = this.rTypeDirect.IsChecked.Value ? Visibility.Visible : Visibility.Collapsed;
            this.pColorPick.Visibility = this.rTypeDirect.IsChecked.Value ? Visibility.Visible : Visibility.Collapsed;
            this.lblSkuId.Visibility = !this.rTypeDirect.IsChecked.Value ? Visibility.Visible : Visibility.Collapsed;
            this.txtSkuId.Visibility = !this.rTypeDirect.IsChecked.Value ? Visibility.Visible : Visibility.Collapsed;
            this.lblStyleId.Visibility = !this.rTypeDirect.IsChecked.Value ? Visibility.Visible : Visibility.Collapsed;
            this.txtStyleId.Visibility = !this.rTypeDirect.IsChecked.Value ? Visibility.Visible : Visibility.Collapsed;
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
                    this.rTypeVariant = (RadioButton) target;
                    this.rTypeVariant.Checked += new RoutedEventHandler(this.rType_Checked);
                    this.rTypeVariant.Unchecked += new RoutedEventHandler(this.rType_Checked);
                    return;

                case 3:
                    this.lblLink = (TextBlock) target;
                    return;

                case 4:
                    this.txtLink = (TextBox) target;
                    return;

                case 5:
                    this.lblSkuId = (TextBlock) target;
                    return;

                case 6:
                    this.txtSkuId = (TextBox) target;
                    return;

                case 7:
                    this.lblColor = (TextBlock) target;
                    return;

                case 8:
                    this.lblStyleId = (TextBlock) target;
                    return;

                case 9:
                    this.txtColor = (TextBox) target;
                    return;

                case 10:
                    this.txtStyleId = (TextBox) target;
                    return;

                case 11:
                    this.pColorPick = (StackPanel) target;
                    return;

                case 12:
                    this.rColorExact = (RadioButton) target;
                    return;

                case 13:
                    this.rColorContains = (RadioButton) target;
                    return;

                case 14:
                    this.chPickRandomColorNotAvailable = (CheckBox) target;
                    return;
            }
            this._contentLoaded = true;
        }
    }
}

