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

    public class BackdoorMassEditControl : UserControl, IComponentConnector
    {
        public ToggleButton switchTaskType;
        public RadioButton rTypeDirect;
        public RadioButton rTypeKeywords;
        internal TextBlock lblLink;
        public TextBox txtLink;
        internal TextBlock lblKeywords;
        public TextBox txtPositiveKws;
        private bool _contentLoaded;

        public BackdoorMassEditControl()
        {
            Class7.RIuqtBYzWxthF();
            this.InitializeComponent();
        }

        public bool Check()
        {
            bool flag = true;
            if (this.switchTaskType.IsChecked.HasValue && this.switchTaskType.IsChecked.Value)
            {
                if (this.rTypeDirect.IsChecked.HasValue && this.rTypeDirect.IsChecked.Value)
                {
                    if (string.IsNullOrEmpty(this.txtLink.Text))
                    {
                        this.txtLink.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                        flag = false;
                    }
                    else
                    {
                        this.txtLink.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
                    }
                    this.txtPositiveKws.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
                    return flag;
                }
                if (string.IsNullOrEmpty(this.txtPositiveKws.Text))
                {
                    this.txtPositiveKws.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                    flag = false;
                }
                else
                {
                    this.txtPositiveKws.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
                }
                this.txtLink.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
                return flag;
            }
            this.txtLink.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
            this.txtPositiveKws.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
            return flag;
        }

        [DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocator = new Uri("/EveAIO;component/views/massedit/backdoormasseditcontrol.xaml", UriKind.Relative);
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
                this.lblKeywords.IsEnabled = true;
                this.lblKeywords.Opacity = 1.0;
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
                this.lblKeywords.IsEnabled = false;
                this.lblKeywords.Opacity = 0.6;
                this.txtPositiveKws.IsEnabled = false;
                this.txtPositiveKws.Opacity = 0.6;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), GeneratedCode("PresentationBuildTasks", "4.0.0.0"), DebuggerNonUserCode]
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
                    this.lblKeywords = (TextBlock) target;
                    return;

                case 7:
                    this.txtPositiveKws = (TextBox) target;
                    return;
            }
            this._contentLoaded = true;
        }
    }
}

