namespace EveAIO.Views.Platforms
{
    using EveAIO;
    using EveAIO.Pocos;
    using EveAIO.Views;
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Markup;
    using System.Windows.Media;

    public class SupremeControl2 : UserControl, IComponentConnector
    {
        private bool _isCreated;
        private TaskWindow2 _taskWindow;
        public RadioButton rTypeDirect;
        public RadioButton rTypeKeywords;
        public RadioButton rTypeManualPicker;
        public CheckBox chSolveCaptcha;
        internal TextBlock lblLink;
        public TextBox txtLink;
        public ComboBox cmbGroup;
        internal TextBlock lblColor;
        public TextBox txtColor;
        public RadioButton rSizeExact;
        public RadioButton rSizeContains;
        public CheckBox chPickRandomColorNotAvailable;
        public TextBox txtPositiveKws;
        public TextBox txtNegativeKws;
        public ComboBox cmbParentTask;
        public RadioButton rAutomationBrowserless;
        public RadioButton rAutomationBrowser;
        public RadioButton rRegionUsa;
        public RadioButton rRegionEu;
        public RadioButton rRegionJapan;
        internal GroupBox gbGmail;
        public TextBox txtGmailUsername;
        public TextBox txtGmailPassword;
        public RadioButton radioButton_0;
        public RadioButton rPaymentCash;
        private bool _contentLoaded;

        public SupremeControl2(TaskWindow2 taskWindow)
        {
            Class7.RIuqtBYzWxthF();
            this._isCreated = true;
            this.InitializeComponent();
            this._taskWindow = taskWindow;
            this.LoadGroups();
            this.LoadTasks();
            this.rTypeDirect.IsChecked = true;
            this.rSizeExact.IsChecked = true;
            this.rRegionUsa.IsChecked = true;
            this.rAutomationBrowserless.IsChecked = true;
            this.radioButton_0.IsChecked = true;
            this._isCreated = false;
        }

        public bool Check()
        {
            bool flag = true;
            if (((this.cmbParentTask.SelectedItem != null) && (((ComboBoxItem) this.cmbParentTask.SelectedItem).Tag.ToString() != "-1")) && (this.rAutomationBrowser.IsChecked.HasValue && this.rAutomationBrowser.IsChecked.Value))
            {
                MessageBox.Show("Multicarting currently not support for browser mode", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                flag = false;
            }
            if ((this.rAutomationBrowser.IsChecked.HasValue && this.rAutomationBrowser.IsChecked.Value) && (this.rTypeKeywords.IsChecked.HasValue && this.rTypeKeywords.IsChecked.Value))
            {
                switch (((ComboBoxItem) this.cmbGroup.SelectedItem).Tag.ToString())
                {
                    case "all":
                    case "new":
                        MessageBox.Show("Selected searching group can't be used for browser mode", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                        flag = false;
                        break;
                }
            }
            if (!this.rTypeManualPicker.IsChecked.HasValue || !this.rTypeManualPicker.IsChecked.Value)
            {
                if (string.IsNullOrEmpty(this.txtLink.Text))
                {
                    this.txtLink.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                    flag = false;
                }
                else
                {
                    this.txtLink.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                }
                if (this.rTypeKeywords.IsChecked.HasValue && this.rTypeKeywords.IsChecked.Value)
                {
                    if (!string.IsNullOrEmpty(this.txtPositiveKws.Text))
                    {
                        this.txtPositiveKws.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                        return flag;
                    }
                    this.txtPositiveKws.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                    return false;
                }
                this.txtPositiveKws.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                this.txtNegativeKws.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
            }
            return flag;
        }

        private void cmbGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!this._isCreated)
            {
                string id = ((ComboBoxItem) this.cmbGroup.SelectedItem).Tag.ToString();
                List<KeyValuePair<string, string>> source = (List<KeyValuePair<string, string>>) Global.ASM.GetType("SvcHost.SvcHost").GetField("SUPREME_LINKS", BindingFlags.Public | BindingFlags.Static).GetValue(null);
                if (this.rTypeKeywords.IsChecked.HasValue && this.rTypeKeywords.IsChecked.Value)
                {
                    this.txtLink.Text = source.First<KeyValuePair<string, string>>(x => (x.Key == id)).Value;
                }
            }
        }

        private void cmbTask_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((this.cmbParentTask.SelectedItem != null) && (((ComboBoxItem) this.cmbParentTask.SelectedItem).Tag.ToString() != "-1"))
            {
                this._taskWindow.cmbCheckoutProfile.SelectedIndex = 0;
                this._taskWindow.cmbCheckoutProfile.IsEnabled = false;
                this._taskWindow.cmbCheckoutProfile.Opacity = 0.6;
            }
            else
            {
                this._taskWindow.cmbCheckoutProfile.IsEnabled = true;
                this._taskWindow.cmbCheckoutProfile.Opacity = 1.0;
            }
        }

        [DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocator = new Uri("/EveAIO;component/views/platforms/supremecontrol2.xaml", UriKind.Relative);
                Application.LoadComponent(this, resourceLocator);
            }
        }

        private void LoadGroups()
        {
            Type type = Global.ASM.GetType("SvcHost.SvcHost");
            type.GetMembers();
            foreach (KeyValuePair<string, string> pair in (List<KeyValuePair<string, string>>) type.GetField("SUPREME_GROUPS", BindingFlags.Public | BindingFlags.Static).GetValue(null))
            {
                ComboBoxItem newItem = new ComboBoxItem {
                    Content = pair.Value,
                    Tag = pair.Key
                };
                this.cmbGroup.Items.Add(newItem);
            }
            this.cmbGroup.SelectedIndex = 0;
        }

        private void LoadTasks()
        {
            ComboBoxItem newItem = new ComboBoxItem {
                Content = "",
                Tag = "-1"
            };
            this.cmbParentTask.Items.Add(newItem);
            foreach (TaskObject obj2 in from x in Global.SETTINGS.TASKS
                where (x.Platform == TaskObject.PlatformEnum.supreme) && string.IsNullOrEmpty(x.ParentId)
                select x)
            {
                if (obj2.Id != this._taskWindow._task.Id)
                {
                    ComboBoxItem item2 = new ComboBoxItem {
                        Content = obj2.Name,
                        Tag = obj2.Id
                    };
                    this.cmbParentTask.Items.Add(item2);
                }
            }
            this.cmbGroup.SelectedIndex = 0;
        }

        private void rAutomation_Checked(object sender, RoutedEventArgs e)
        {
            if (this.rAutomationBrowser.IsChecked.HasValue && this.rAutomationBrowser.IsChecked.Value)
            {
                this.gbGmail.IsEnabled = true;
                this.gbGmail.Opacity = 1.0;
            }
            else
            {
                this.gbGmail.IsEnabled = false;
                this.gbGmail.Opacity = 0.6;
            }
        }

        private void rRegion_Checked(object sender, RoutedEventArgs e)
        {
            if (this.rRegionJapan.IsChecked.HasValue && this.rRegionJapan.IsChecked.Value)
            {
                this.rPaymentCash.IsEnabled = true;
                this.rPaymentCash.Opacity = 1.0;
            }
            else
            {
                this.radioButton_0.IsChecked = true;
                this.rPaymentCash.IsEnabled = false;
                this.rPaymentCash.Opacity = 0.6;
            }
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
                this.txtNegativeKws.IsEnabled = false;
                this.txtNegativeKws.Opacity = 0.6;
                this.txtNegativeKws.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                this.txtColor.IsEnabled = false;
                this.txtColor.Opacity = 0.6;
                this.txtColor.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                this.cmbGroup.IsEnabled = false;
                this.cmbGroup.Opacity = 0.6;
                this.cmbGroup.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                this.txtPositiveKws.Clear();
                this.txtNegativeKws.Clear();
                this.rSizeContains.IsEnabled = false;
                this.rSizeContains.Opacity = 0.6;
                this.rSizeExact.IsEnabled = false;
                this.rSizeExact.Opacity = 0.6;
                this.chPickRandomColorNotAvailable.IsEnabled = false;
                this.chPickRandomColorNotAvailable.Opacity = 0.6;
            }
            else if (this.rTypeKeywords.IsChecked.HasValue && this.rTypeKeywords.IsChecked.Value)
            {
                this.cmbGroup_SelectionChanged(null, null);
                this.lblLink.Text = "SEARCH PAGE:";
                this.txtLink.IsEnabled = false;
                this.txtLink.Opacity = 0.6;
                this.txtPositiveKws.IsEnabled = true;
                this.txtPositiveKws.Opacity = 1.0;
                this.txtPositiveKws.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                this.txtNegativeKws.IsEnabled = true;
                this.txtNegativeKws.Opacity = 1.0;
                this.txtNegativeKws.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                this.txtColor.IsEnabled = true;
                this.txtColor.Opacity = 1.0;
                this.txtColor.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                this.cmbGroup.IsEnabled = true;
                this.cmbGroup.Opacity = 1.0;
                this.cmbGroup.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                this.rSizeContains.IsEnabled = true;
                this.rSizeContains.Opacity = 1.0;
                this.rSizeExact.IsEnabled = true;
                this.rSizeExact.Opacity = 1.0;
                this.chPickRandomColorNotAvailable.IsEnabled = true;
                this.chPickRandomColorNotAvailable.Opacity = 1.0;
            }
            else
            {
                this.lblLink.Text = "DIRECT LINK:";
                this.txtLink.Text = "";
                this.txtLink.IsEnabled = false;
                this.txtLink.Opacity = 0.6;
                this.txtPositiveKws.IsEnabled = false;
                this.txtPositiveKws.Opacity = 0.6;
                this.txtPositiveKws.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                this.txtNegativeKws.IsEnabled = false;
                this.txtNegativeKws.Opacity = 0.6;
                this.txtNegativeKws.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                this.txtColor.IsEnabled = false;
                this.txtColor.Opacity = 0.6;
                this.txtColor.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                this.cmbGroup.IsEnabled = false;
                this.cmbGroup.Opacity = 0.6;
                this.cmbGroup.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                this.txtPositiveKws.Clear();
                this.txtNegativeKws.Clear();
                this.rSizeContains.IsEnabled = false;
                this.rSizeContains.Opacity = 0.6;
                this.rSizeExact.IsEnabled = false;
                this.rSizeExact.Opacity = 0.6;
                this.chPickRandomColorNotAvailable.IsEnabled = false;
                this.chPickRandomColorNotAvailable.Opacity = 0.6;
            }
        }

        [GeneratedCode("PresentationBuildTasks", "4.0.0.0"), EditorBrowsable(EditorBrowsableState.Never), DebuggerNonUserCode]
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
                    this.rTypeManualPicker = (RadioButton) target;
                    this.rTypeManualPicker.Checked += new RoutedEventHandler(this.rType_Checked);
                    this.rTypeManualPicker.Unchecked += new RoutedEventHandler(this.rType_Checked);
                    return;

                case 4:
                    this.chSolveCaptcha = (CheckBox) target;
                    return;

                case 5:
                    this.lblLink = (TextBlock) target;
                    return;

                case 6:
                    this.txtLink = (TextBox) target;
                    return;

                case 7:
                    this.cmbGroup = (ComboBox) target;
                    this.cmbGroup.SelectionChanged += new SelectionChangedEventHandler(this.cmbGroup_SelectionChanged);
                    return;

                case 8:
                    this.lblColor = (TextBlock) target;
                    return;

                case 9:
                    this.txtColor = (TextBox) target;
                    return;

                case 10:
                    this.rSizeExact = (RadioButton) target;
                    return;

                case 11:
                    this.rSizeContains = (RadioButton) target;
                    return;

                case 12:
                    this.chPickRandomColorNotAvailable = (CheckBox) target;
                    return;

                case 13:
                    this.txtPositiveKws = (TextBox) target;
                    return;

                case 14:
                    this.txtNegativeKws = (TextBox) target;
                    return;

                case 15:
                    this.cmbParentTask = (ComboBox) target;
                    this.cmbParentTask.SelectionChanged += new SelectionChangedEventHandler(this.cmbTask_SelectionChanged);
                    return;

                case 0x10:
                    this.rAutomationBrowserless = (RadioButton) target;
                    this.rAutomationBrowserless.Checked += new RoutedEventHandler(this.rAutomation_Checked);
                    this.rAutomationBrowserless.Unchecked += new RoutedEventHandler(this.rAutomation_Checked);
                    return;

                case 0x11:
                    this.rAutomationBrowser = (RadioButton) target;
                    this.rAutomationBrowser.Checked += new RoutedEventHandler(this.rAutomation_Checked);
                    this.rAutomationBrowser.Unchecked += new RoutedEventHandler(this.rAutomation_Checked);
                    return;

                case 0x12:
                    this.rRegionUsa = (RadioButton) target;
                    this.rRegionUsa.Checked += new RoutedEventHandler(this.rRegion_Checked);
                    return;

                case 0x13:
                    this.rRegionEu = (RadioButton) target;
                    this.rRegionEu.Checked += new RoutedEventHandler(this.rRegion_Checked);
                    return;

                case 20:
                    this.rRegionJapan = (RadioButton) target;
                    this.rRegionJapan.Checked += new RoutedEventHandler(this.rRegion_Checked);
                    return;

                case 0x15:
                    this.gbGmail = (GroupBox) target;
                    return;

                case 0x16:
                    this.txtGmailUsername = (TextBox) target;
                    return;

                case 0x17:
                    this.txtGmailPassword = (TextBox) target;
                    return;

                case 0x18:
                    this.radioButton_0 = (RadioButton) target;
                    return;

                case 0x19:
                    this.rPaymentCash = (RadioButton) target;
                    return;
            }
            this._contentLoaded = true;
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly SupremeControl2.<>c <>9;
            public static Func<TaskObject, bool> <>9__5_0;

            static <>c()
            {
                Class7.RIuqtBYzWxthF();
                <>9 = new SupremeControl2.<>c();
            }

            public <>c()
            {
                Class7.RIuqtBYzWxthF();
            }

            internal bool <LoadTasks>b__5_0(TaskObject x) => 
                ((x.Platform == TaskObject.PlatformEnum.supreme) && string.IsNullOrEmpty(x.ParentId));
        }
    }
}

