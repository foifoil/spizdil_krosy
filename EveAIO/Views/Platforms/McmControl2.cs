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

    public class McmControl2 : UserControl, IComponentConnector
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

        public McmControl2(TaskWindow2 taskWindow)
        {
            Class7.RIuqtBYzWxthF();
            this.InitializeComponent();
            this._taskWindow = taskWindow;
            this.rColorContains.IsChecked = true;
            this.rTypeDirect.IsChecked = true;
        }

        public bool Check()
        {
            if (string.IsNullOrEmpty(this.txtLink.Text))
            {
                this.txtLink.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                return false;
            }
            this.txtLink.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
            return true;
        }

        [GeneratedCode("PresentationBuildTasks", "4.0.0.0"), DebuggerNonUserCode]
        public void InitializeComponent()
        {
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocator = new Uri("/EveAIO;component/views/platforms/mcmcontrol2.xaml", UriKind.Relative);
                Application.LoadComponent(this, resourceLocator);
            }
        }

        private void rType_Checked(object sender, RoutedEventArgs e)
        {
            if (this.rTypeDirect.IsChecked.HasValue && this.rTypeDirect.IsChecked.Value)
            {
                this.lblLink.Text = "DIRECT LINK:";
                this.txtLink.Visibility = Visibility.Visible;
                this.txtPositiveKws.Visibility = Visibility.Collapsed;
                this.txtPositiveKws.Clear();
            }
            else
            {
                this.lblLink.Text = "KEYWORDS:";
                this.txtLink.Visibility = Visibility.Collapsed;
                this.txtPositiveKws.Visibility = Visibility.Visible;
                this.txtLink.Text = "Default search";
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

