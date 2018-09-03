namespace EveAIO.Views.Platforms
{
    using EveAIO.Views;
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Markup;
    using System.Windows.Media;

    public class FootlockereuControl : UserControl, IComponentConnector
    {
        private TaskWindow2 _taskWindow;
        public RadioButton rTypeDirect;
        public RadioButton rTypeKeywords;
        internal TextBlock lblRegion;
        public ComboBox cmbRegion;
        internal TextBlock lblLink;
        public TextBox txtLink;
        public TextBox txtPositiveKws;
        internal GroupBox gbPayment;
        public RadioButton radioButton_0;
        public RadioButton rPaymentCCMan;
        private bool _contentLoaded;

        public FootlockereuControl(TaskWindow2 taskWindow)
        {
            Class7.RIuqtBYzWxthF();
            this.InitializeComponent();
            this._taskWindow = taskWindow;
            this.rTypeDirect.IsChecked = true;
            this.radioButton_0.IsChecked = true;
            ComboBoxItem newItem = new ComboBoxItem {
                Content = "DE",
                Tag = "DE"
            };
            this.cmbRegion.Items.Add(newItem);
            ComboBoxItem item2 = new ComboBoxItem {
                Content = "GB",
                Tag = "GB"
            };
            this.cmbRegion.Items.Add(item2);
            ComboBoxItem item3 = new ComboBoxItem {
                Content = "NL",
                Tag = "NL"
            };
            this.cmbRegion.Items.Add(item3);
            ComboBoxItem item4 = new ComboBoxItem {
                Content = "LU",
                Tag = "LU"
            };
            this.cmbRegion.Items.Add(item4);
            ComboBoxItem item5 = new ComboBoxItem {
                Content = "BE",
                Tag = "BE"
            };
            this.cmbRegion.Items.Add(item5);
            ComboBoxItem item6 = new ComboBoxItem {
                Content = "IT",
                Tag = "IT"
            };
            this.cmbRegion.Items.Add(item6);
            ComboBoxItem item7 = new ComboBoxItem {
                Content = "ES",
                Tag = "ES"
            };
            this.cmbRegion.Items.Add(item7);
            ComboBoxItem item8 = new ComboBoxItem {
                Content = "FR",
                Tag = "FR"
            };
            this.cmbRegion.Items.Add(item8);
            ComboBoxItem item9 = new ComboBoxItem {
                Content = "DK",
                Tag = "DK"
            };
            this.cmbRegion.Items.Add(item9);
            ComboBoxItem item10 = new ComboBoxItem {
                Content = "SE",
                Tag = "SE"
            };
            this.cmbRegion.Items.Add(item10);
            ComboBoxItem item11 = new ComboBoxItem {
                Content = "NO",
                Tag = "NO"
            };
            this.cmbRegion.Items.Add(item11);
            this.cmbRegion.SelectedIndex = 1;
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
                return flag;
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

        private void imgInfo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start("https://eve-robotics.com/knowledge-base/footlockereu/");
        }

        [DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            // This item is obfuscated and can not be translated.
            if (!this._contentLoaded)
            {
                goto Label_002C;
            }
        Label_000D:
            switch (((0x1ace5ba8 ^ 0x5728b1f3) % 4))
            {
                case 0:
                    goto Label_000D;

                case 2:
                    break;

                case 3:
                    return;

                default:
                {
                    Uri resourceLocator = new Uri("/EveAIO;component/views/platforms/footlockereucontrol.xaml", UriKind.Relative);
                    Application.LoadComponent(this, resourceLocator);
                    return;
                }
            }
        Label_002C:
            this._contentLoaded = true;
            goto Label_000D;
        }

        private void rType_Checked(object sender, RoutedEventArgs e)
        {
            if (this.rTypeDirect.IsChecked.HasValue && this.rTypeDirect.IsChecked.Value)
            {
                this.lblLink.Text = "DIRECT LINK:";
                this.txtLink.Text = "";
                this.txtLink.IsEnabled = true;
                this.txtLink.Opacity = 1.0;
                this.txtPositiveKws.IsEnabled = false;
                this.txtPositiveKws.Opacity = 0.6;
                this.txtPositiveKws.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                this.lblRegion.Opacity = 0.6;
                this.cmbRegion.Opacity = 0.6;
                this.cmbRegion.IsEnabled = false;
                this.txtPositiveKws.Clear();
            }
            else
            {
                this.lblLink.Text = "SEARCH PAGE:";
                this.txtLink.Text = "default search";
                this.txtLink.IsEnabled = false;
                this.txtLink.Opacity = 0.6;
                this.txtPositiveKws.IsEnabled = true;
                this.txtPositiveKws.Opacity = 1.0;
                this.txtPositiveKws.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                this.lblRegion.Opacity = 1.0;
                this.cmbRegion.Opacity = 1.0;
                this.cmbRegion.IsEnabled = true;
            }
        }

        [GeneratedCode("PresentationBuildTasks", "4.0.0.0"), DebuggerNonUserCode, EditorBrowsable(EditorBrowsableState.Never)]
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
                    this.lblRegion = (TextBlock) target;
                    return;

                case 4:
                    this.cmbRegion = (ComboBox) target;
                    return;

                case 5:
                    this.lblLink = (TextBlock) target;
                    return;

                case 6:
                    this.txtLink = (TextBox) target;
                    return;

                case 7:
                    this.txtPositiveKws = (TextBox) target;
                    return;

                case 8:
                    this.gbPayment = (GroupBox) target;
                    return;

                case 9:
                    this.radioButton_0 = (RadioButton) target;
                    return;

                case 10:
                    this.rPaymentCCMan = (RadioButton) target;
                    return;
            }
            this._contentLoaded = true;
        }
    }
}

