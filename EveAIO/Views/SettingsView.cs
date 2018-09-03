namespace EveAIO.Views
{
    using EveAIO;
    using EveAIO.Pocos;
    using System;
    using System.CodeDom.Compiler;
    using System.Collections;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Markup;
    using System.Windows.Media;
    using System.Windows.Threading;

    public class SettingsView : UserControl, IComponentConnector
    {
        private DispatcherTimer _logCleaner;
        private bool _isInit;
        internal Button btnDeactivate;
        internal Button btnUpdateCheck;
        internal ToggleButton switchCollapseTasks;
        internal TextBox txtShopifySmartSchedule;
        internal ComboBox cmbQuickTaskProfile;
        internal ComboBox cmbQuickTaskProxyList;
        internal RadioButton chDelayExact;
        internal RadioButton chDelayRandom;
        internal TextBox txtDelay;
        internal TextBox txtDelayFrom;
        internal TextBox txtDelayTo;
        internal GroupBox gbSizing;
        internal TextBox txtSize;
        internal CheckBox chSizeRandom;
        internal CheckBox chPickRandomNotAvailable;
        internal ToggleButton switchQuicktaskAutopaste;
        internal Button btnReset;
        internal ToggleButton switchLogCleaner;
        internal TextBlock lblLogCleanerText;
        internal TextBox txtLogReset;
        internal TextBlock lblLogCleanerMinutes;
        internal ToggleButton switchSolverBeep;
        internal ToggleButton switchAtcBeep;
        internal ToggleButton switchCheckoutBeep;
        internal ToggleButton switchPaypalBeep;
        public RadioButton rEnvLight;
        public RadioButton rEnvDark;
        internal Button btnExportSettings;
        internal Button btnImportSettings;
        private bool _contentLoaded;

        public SettingsView()
        {
            Class7.RIuqtBYzWxthF();
            this.InitializeComponent();
            this._logCleaner = new DispatcherTimer();
            this._logCleaner.Tick += new EventHandler(this._logCleaner_Tick);
            this.txtLogReset.Text = Global.SETTINGS.LogCleanerMinutes.ToString();
            this.switchLogCleaner.IsChecked = new bool?(Global.SETTINGS.LogCleaner);
            this.switchSolverBeep.IsChecked = new bool?(Global.SETTINGS.SolverBeep);
            this.switchAtcBeep.IsChecked = new bool?(Global.SETTINGS.AtcBeep);
            this.switchCheckoutBeep.IsChecked = new bool?(Global.SETTINGS.CheckoutBeep);
            this.switchPaypalBeep.IsChecked = new bool?(Global.SETTINGS.PayPalBeep);
            this.switchCollapseTasks.IsChecked = new bool?(Global.SETTINGS.CollapseTasks);
            if (Global.SETTINGS.EnvLight)
            {
                this.rEnvLight.IsChecked = true;
            }
            else
            {
                this.rEnvDark.IsChecked = true;
            }
        }

        private void _logCleaner_Tick(object sender, EventArgs e)
        {
            this._logCleaner.Stop();
            Global.ViewLog.txtLog.Clear();
            this._logCleaner.Interval = TimeSpan.FromMinutes((double) Global.SETTINGS.LogCleanerMinutes);
            this._logCleaner.Start();
        }

        private void btnDeactivate_Click(object sender, RoutedEventArgs e)
        {
            Helpers.Deactivate();
        }

        private void btnExportSettings_Click(object sender, RoutedEventArgs e)
        {
            new SettingsImExWindow((MainWindow) Global.MAIN_WINDOW, SettingsImExWindow.WindowStyleEnum.EXPORT).ShowDialog();
        }

        private void btnImportSettings_Click(object sender, RoutedEventArgs e)
        {
            new SettingsImExWindow((MainWindow) Global.MAIN_WINDOW, SettingsImExWindow.WindowStyleEnum.IMPORT).ShowDialog();
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            Global.SUCCESS_PROFILES.Clear();
        }

        private void btnUpdateCheck_Click(object sender, RoutedEventArgs e)
        {
            Global.MAIN_WINDOW.updater.Visibility = Visibility.Visible;
            Global.MAIN_WINDOW.updater.ForceCheckForUpdate();
        }

        private void chDelay_Checked(object sender, RoutedEventArgs e)
        {
            if (this.chDelayExact.IsChecked.HasValue && this.chDelayExact.IsChecked.Value)
            {
                this.txtDelay.IsEnabled = true;
                this.txtDelay.Opacity = 1.0;
                this.txtDelayFrom.IsEnabled = false;
                this.txtDelayFrom.Opacity = 0.6;
                this.txtDelayTo.IsEnabled = false;
                this.txtDelayTo.Opacity = 0.6;
            }
            else
            {
                this.txtDelay.IsEnabled = false;
                this.txtDelay.Opacity = 0.6;
                this.txtDelayFrom.IsEnabled = true;
                this.txtDelayFrom.Opacity = 1.0;
                this.txtDelayTo.IsEnabled = true;
                this.txtDelayTo.Opacity = 1.0;
            }
            if (!this._isInit)
            {
                if (this.chDelayRandom.IsChecked.HasValue && this.chDelayRandom.IsChecked.Value)
                {
                    Global.SETTINGS.QUICK_TASK.RetryDelay = TaskObject.RetryDelayEnum.random;
                }
                else
                {
                    Global.SETTINGS.QUICK_TASK.RetryDelay = TaskObject.RetryDelayEnum.exact;
                }
                Helpers.SaveSettings();
            }
        }

        private void chPickRandomNotAvailable_Checked(object sender, RoutedEventArgs e)
        {
            if (this.chPickRandomNotAvailable.IsChecked.HasValue && this.chPickRandomNotAvailable.IsChecked.Value)
            {
                Global.SETTINGS.QUICK_TASK.SizePickRandom = true;
            }
            else
            {
                Global.SETTINGS.QUICK_TASK.SizePickRandom = false;
            }
            if (!this._isInit)
            {
                Helpers.SaveSettings();
            }
        }

        private void chSizeRandom_Checked(object sender, RoutedEventArgs e)
        {
            if (this.chSizeRandom.IsChecked.HasValue && this.chSizeRandom.IsChecked.Value)
            {
                this.txtSize.Text = "Random";
                this.txtSize.IsEnabled = false;
                this.txtSize.Opacity = 0.6;
                this.chPickRandomNotAvailable.IsEnabled = false;
                this.chPickRandomNotAvailable.Opacity = 0.6;
                Global.SETTINGS.QUICK_TASK.RandomSize = true;
            }
            else
            {
                this.txtSize.Text = "";
                this.txtSize.IsEnabled = true;
                this.txtSize.Opacity = 1.0;
                this.chPickRandomNotAvailable.IsEnabled = true;
                this.chPickRandomNotAvailable.Opacity = 1.0;
                Global.SETTINGS.QUICK_TASK.RandomSize = false;
            }
            if (!this._isInit)
            {
                Helpers.SaveSettings();
            }
        }

        private void cmbQuickTaskProfile_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // This item is obfuscated and can not be translated.
            if (this._isInit)
            {
                goto Label_004F;
            }
        Label_0008:
            Global.SETTINGS.QUICK_TASK.IdProfile = (this.cmbQuickTaskProfile.SelectedItem == null) ? "" : ((ComboBoxItem) this.cmbQuickTaskProfile.SelectedItem).Tag.ToString();
            Helpers.SaveSettings();
        Label_004F:
            switch (((0x2675905a ^ 0x506cc660) % 4))
            {
                case 0:
                    goto Label_0008;

                case 1:
                    return;

                case 3:
                    goto Label_004F;
            }
        }

        private void cmbQuickTaskProxyList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // This item is obfuscated and can not be translated.
            if (this._isInit)
            {
                goto Label_004A;
            }
        Label_0008:
            Global.SETTINGS.QUICK_TASK.IdProxyList = (this.cmbQuickTaskProxyList.SelectedItem == null) ? "" : ((ComboBoxItem) this.cmbQuickTaskProxyList.SelectedItem).Tag.ToString();
        Label_004A:
            switch (((0x30ba6fe0 ^ 0x7e4af46a) % 4))
            {
                case 0:
                    goto Label_0008;

                case 1:
                    return;

                case 3:
                    goto Label_004A;
            }
            Helpers.SaveSettings();
        }

        public void Init()
        {
            IEnumerator enumerator3;
            this._isInit = true;
            this.cmbQuickTaskProfile.Items.Clear();
            ComboBoxItem newItem = new ComboBoxItem {
                Content = "",
                Tag = "-1"
            };
            this.cmbQuickTaskProfile.Items.Add(newItem);
            bool flag = false;
            foreach (ProfileGroupObject obj4 in Global.SETTINGS.PROFILES_GROUPS)
            {
                flag = true;
                ComboBoxItem item2 = new ComboBoxItem();
                string[] textArray1 = new string[] { "*", obj4.Name, " (", obj4.Profiles, ")" };
                item2.Content = string.Concat(textArray1);
                item2.Tag = obj4.Id;
                this.cmbQuickTaskProfile.Items.Add(item2);
            }
            if (flag)
            {
                ComboBoxItem item4 = new ComboBoxItem {
                    Content = "---------",
                    Tag = "-1"
                };
                this.cmbQuickTaskProfile.Items.Add(item4);
            }
            foreach (ProfileObject obj3 in Global.SETTINGS.PROFILES)
            {
                ComboBoxItem item5 = new ComboBoxItem {
                    Content = obj3.Name,
                    Tag = obj3.Id
                };
                this.cmbQuickTaskProfile.Items.Add(item5);
            }
            if (!string.IsNullOrEmpty(Global.SETTINGS.QUICK_TASK.IdProfile))
            {
                using (enumerator3 = ((IEnumerable) this.cmbQuickTaskProfile.Items).GetEnumerator())
                {
                    ComboBoxItem current;
                    while (enumerator3.MoveNext())
                    {
                        current = (ComboBoxItem) enumerator3.Current;
                        if (Global.SETTINGS.QUICK_TASK.IdProfile == current.Tag.ToString())
                        {
                            goto Label_01E4;
                        }
                    }
                    goto Label_0208;
                Label_01E4:
                    this.cmbQuickTaskProfile.SelectedItem = current;
                }
            }
        Label_0208:
            this.cmbQuickTaskProxyList.Items.Clear();
            ComboBoxItem item6 = new ComboBoxItem {
                Content = "",
                Tag = "-1"
            };
            this.cmbQuickTaskProxyList.Items.Add(item6);
            foreach (ProxyListObject obj2 in Global.SETTINGS.PROXIES)
            {
                ComboBoxItem item7 = new ComboBoxItem {
                    Content = obj2.Name,
                    Tag = obj2.Id
                };
                this.cmbQuickTaskProxyList.Items.Add(item7);
            }
            if (!string.IsNullOrEmpty(Global.SETTINGS.QUICK_TASK.IdProxyList))
            {
                using (enumerator3 = ((IEnumerable) this.cmbQuickTaskProxyList.Items).GetEnumerator())
                {
                    ComboBoxItem item3;
                    goto Label_02FC;
                Label_02CC:
                    item3 = (ComboBoxItem) enumerator3.Current;
                    if (Global.SETTINGS.QUICK_TASK.IdProxyList == item3.Tag.ToString())
                    {
                        goto Label_0307;
                    }
                Label_02FC:
                    if (!enumerator3.MoveNext())
                    {
                        goto Label_032D;
                    }
                    goto Label_02CC;
                Label_0307:
                    this.cmbQuickTaskProxyList.SelectedItem = item3;
                }
            }
        Label_032D:
            if (Global.SETTINGS.QUICK_TASK.RetryDelay == TaskObject.RetryDelayEnum.exact)
            {
                this.chDelayExact.IsChecked = true;
            }
            else
            {
                this.chDelayRandom.IsChecked = true;
            }
            this.txtDelay.Text = Global.SETTINGS.QUICK_TASK.Delay;
            this.txtDelayFrom.Text = Global.SETTINGS.QUICK_TASK.DelayFrom;
            this.txtDelayTo.Text = Global.SETTINGS.QUICK_TASK.DelayTo;
            this.switchQuicktaskAutopaste.IsChecked = new bool?(Global.SETTINGS.QUICK_TASK.Autopaste);
            this.txtSize.Text = Global.SETTINGS.QUICK_TASK.Size;
            this.chSizeRandom.IsChecked = new bool?(Global.SETTINGS.QUICK_TASK.RandomSize);
            this.chPickRandomNotAvailable.IsChecked = new bool?(Global.SETTINGS.QUICK_TASK.SizePickRandom);
            this.txtShopifySmartSchedule.Text = Global.SETTINGS.ShopifySmartDelay.ToString();
            this._isInit = false;
        }

        [GeneratedCode("PresentationBuildTasks", "4.0.0.0"), DebuggerNonUserCode]
        public void InitializeComponent()
        {
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocator = new Uri("/EveAIO;component/views/settingsview.xaml", UriKind.Relative);
                Application.LoadComponent(this, resourceLocator);
            }
        }

        private void NumberCheck(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        private void rEnv_Checked(object sender, RoutedEventArgs e)
        {
            if (this.rEnvLight.IsChecked.HasValue && this.rEnvLight.IsChecked.Value)
            {
                Application.Current.Resources["TextColor2"] = (SolidColorBrush) Application.Current.Resources["TextColorLight"];
                Application.Current.Resources["GradientStop1"] = (Color) Application.Current.Resources["GradientStop1Light"];
                Application.Current.Resources["GradientStop2"] = (Color) Application.Current.Resources["GradientStop2Light"];
                Application.Current.Resources["GradientStop3"] = (Color) Application.Current.Resources["GradientStop3Light"];
                Application.Current.Resources["MainBackground"] = (SolidColorBrush) Application.Current.Resources["MainBackgroundLight"];
                Application.Current.Resources["WindowBorder"] = (SolidColorBrush) Application.Current.Resources["WindowBorderLight"];
                Application.Current.Resources["ContentBackground"] = (SolidColorBrush) Application.Current.Resources["ContentBackgroundLight"];
                Application.Current.Resources["Blue"] = (SolidColorBrush) Application.Current.Resources["BlueLight"];
                Application.Current.Resources["ButtonBackground"] = (SolidColorBrush) Application.Current.Resources["ButtonBackgroundLight"];
                Application.Current.Resources["ButtonForeground"] = (SolidColorBrush) Application.Current.Resources["ButtonForegroundLight"];
                Application.Current.Resources["ButtonBorder"] = (SolidColorBrush) Application.Current.Resources["ButtonBorderLight"];
                Application.Current.Resources["GridSelected"] = (SolidColorBrush) Application.Current.Resources["GridSelectedLight"];
                Application.Current.Resources["FilledBackground"] = (SolidColorBrush) Application.Current.Resources["ContentBackgroundLight"];
                Application.Current.Resources["ColorBorder"] = (Color) Application.Current.Resources["ColorBorderLight"];
                Application.Current.Resources["AnimatedSwitchWhite"] = (Style) Application.Current.Resources["AnimatedSwitchLight"];
                Application.Current.Resources["Tab"] = (SolidColorBrush) Application.Current.Resources["TabLight"];
                Application.Current.Resources["TabSelected"] = (SolidColorBrush) Application.Current.Resources["TabSelectedLight"];
                Global.SETTINGS.EnvLight = true;
            }
            else
            {
                Application.Current.Resources["TextColor2"] = (SolidColorBrush) Application.Current.Resources["TextColorDark"];
                Application.Current.Resources["GradientStop1"] = (Color) Application.Current.Resources["GradientStop1Dark"];
                Application.Current.Resources["GradientStop2"] = (Color) Application.Current.Resources["GradientStop2Dark"];
                Application.Current.Resources["GradientStop3"] = (Color) Application.Current.Resources["GradientStop3Dark"];
                Application.Current.Resources["MainBackground"] = (SolidColorBrush) Application.Current.Resources["MainBackgroundDark"];
                Application.Current.Resources["WindowBorder"] = (SolidColorBrush) Application.Current.Resources["WindowBorderDark"];
                Application.Current.Resources["ContentBackground"] = (SolidColorBrush) Application.Current.Resources["ContentBackgroundDark"];
                Application.Current.Resources["Blue"] = (SolidColorBrush) Application.Current.Resources["BlueDark"];
                Application.Current.Resources["ButtonBackground"] = (SolidColorBrush) Application.Current.Resources["ButtonBackgroundDark"];
                Application.Current.Resources["ButtonForeground"] = (SolidColorBrush) Application.Current.Resources["ButtonForegroundDark"];
                Application.Current.Resources["ButtonBorder"] = (SolidColorBrush) Application.Current.Resources["ButtonBorderDark"];
                Application.Current.Resources["GridSelected"] = (SolidColorBrush) Application.Current.Resources["GridSelectedDark"];
                Application.Current.Resources["FilledBackground"] = (SolidColorBrush) Application.Current.Resources["ContentBackgroundDark"];
                Application.Current.Resources["ColorBorder"] = (Color) Application.Current.Resources["ColorBorderDark"];
                Application.Current.Resources["AnimatedSwitchWhite"] = (Style) Application.Current.Resources["AnimatedSwitchDark"];
                Application.Current.Resources["Tab"] = (SolidColorBrush) Application.Current.Resources["TabDark"];
                Application.Current.Resources["TabSelected"] = (SolidColorBrush) Application.Current.Resources["TabSelectedDark"];
                Global.SETTINGS.EnvLight = false;
            }
            if (!this._isInit)
            {
                Helpers.SaveSettings();
            }
        }

        private void switchAtcBeep_Checked(object sender, RoutedEventArgs e)
        {
            Global.SETTINGS.AtcBeep = !this.switchAtcBeep.IsChecked.HasValue ? false : this.switchAtcBeep.IsChecked.Value;
            Helpers.SaveSettings();
        }

        private void switchCheckoutBeep_Checked(object sender, RoutedEventArgs e)
        {
            Global.SETTINGS.CheckoutBeep = !this.switchCheckoutBeep.IsChecked.HasValue ? false : this.switchCheckoutBeep.IsChecked.Value;
            Helpers.SaveSettings();
        }

        private void switchCollapseTasks_Checked(object sender, RoutedEventArgs e)
        {
            Global.SETTINGS.CollapseTasks = !this.switchCollapseTasks.IsChecked.HasValue ? false : this.switchCollapseTasks.IsChecked.Value;
            Helpers.SaveSettings();
        }

        private void switchLogCleaner_Checked(object sender, RoutedEventArgs e)
        {
            Global.SETTINGS.LogCleaner = !this.switchLogCleaner.IsChecked.HasValue ? false : this.switchLogCleaner.IsChecked.Value;
            if (Global.SETTINGS.LogCleaner)
            {
                if (Global.SETTINGS.LogCleanerMinutes < 1)
                {
                    Global.SETTINGS.LogCleanerMinutes = 3;
                }
                this._logCleaner.Interval = TimeSpan.FromMinutes((double) Global.SETTINGS.LogCleanerMinutes);
                this._logCleaner.Start();
                this.txtLogReset.IsEnabled = true;
                this.txtLogReset.Opacity = 1.0;
                this.lblLogCleanerMinutes.IsEnabled = true;
                this.lblLogCleanerMinutes.Opacity = 1.0;
                this.lblLogCleanerText.IsEnabled = true;
                this.lblLogCleanerText.Opacity = 1.0;
            }
            else
            {
                this._logCleaner.Stop();
                this.txtLogReset.IsEnabled = false;
                this.txtLogReset.Opacity = 0.6;
                this.lblLogCleanerMinutes.IsEnabled = false;
                this.lblLogCleanerMinutes.Opacity = 0.6;
                this.lblLogCleanerText.IsEnabled = false;
                this.lblLogCleanerText.Opacity = 0.6;
            }
            Helpers.SaveSettings();
        }

        private void switchPaypalBeep_Checked(object sender, RoutedEventArgs e)
        {
            Global.SETTINGS.PayPalBeep = !this.switchPaypalBeep.IsChecked.HasValue ? false : this.switchPaypalBeep.IsChecked.Value;
            Helpers.SaveSettings();
        }

        private void switchQuicktaskAutopaste_Checked(object sender, RoutedEventArgs e)
        {
            if (!this._isInit)
            {
                if (this.switchQuicktaskAutopaste.IsChecked.HasValue && this.switchQuicktaskAutopaste.IsChecked.Value)
                {
                    Global.SETTINGS.QUICK_TASK.Autopaste = true;
                }
                else
                {
                    Global.SETTINGS.QUICK_TASK.Autopaste = false;
                }
                Helpers.SaveSettings();
            }
        }

        private void switchSolverBeep_Checked(object sender, RoutedEventArgs e)
        {
            Global.SETTINGS.SolverBeep = !this.switchSolverBeep.IsChecked.HasValue ? false : this.switchSolverBeep.IsChecked.Value;
            Helpers.SaveSettings();
        }

        [DebuggerNonUserCode, EditorBrowsable(EditorBrowsableState.Never), GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    this.btnDeactivate = (Button) target;
                    this.btnDeactivate.Click += new RoutedEventHandler(this.btnDeactivate_Click);
                    return;

                case 2:
                    this.btnUpdateCheck = (Button) target;
                    this.btnUpdateCheck.Click += new RoutedEventHandler(this.btnUpdateCheck_Click);
                    return;

                case 3:
                    this.switchCollapseTasks = (ToggleButton) target;
                    this.switchCollapseTasks.Checked += new RoutedEventHandler(this.switchCollapseTasks_Checked);
                    this.switchCollapseTasks.Unchecked += new RoutedEventHandler(this.switchCollapseTasks_Checked);
                    return;

                case 4:
                    this.txtShopifySmartSchedule = (TextBox) target;
                    this.txtShopifySmartSchedule.TextChanged += new TextChangedEventHandler(this.txtShopifySmartSchedule_TextChanged);
                    this.txtShopifySmartSchedule.PreviewTextInput += new TextCompositionEventHandler(this.NumberCheck);
                    return;

                case 5:
                    this.cmbQuickTaskProfile = (ComboBox) target;
                    this.cmbQuickTaskProfile.SelectionChanged += new SelectionChangedEventHandler(this.cmbQuickTaskProfile_SelectionChanged);
                    return;

                case 6:
                    this.cmbQuickTaskProxyList = (ComboBox) target;
                    this.cmbQuickTaskProxyList.SelectionChanged += new SelectionChangedEventHandler(this.cmbQuickTaskProxyList_SelectionChanged);
                    return;

                case 7:
                    this.chDelayExact = (RadioButton) target;
                    this.chDelayExact.Checked += new RoutedEventHandler(this.chDelay_Checked);
                    this.chDelayExact.Unchecked += new RoutedEventHandler(this.chDelay_Checked);
                    return;

                case 8:
                    this.chDelayRandom = (RadioButton) target;
                    this.chDelayRandom.Checked += new RoutedEventHandler(this.chDelay_Checked);
                    this.chDelayRandom.Unchecked += new RoutedEventHandler(this.chDelay_Checked);
                    return;

                case 9:
                    this.txtDelay = (TextBox) target;
                    this.txtDelay.PreviewTextInput += new TextCompositionEventHandler(this.NumberCheck);
                    this.txtDelay.TextChanged += new TextChangedEventHandler(this.txtDelay_TextChanged);
                    return;

                case 10:
                    this.txtDelayFrom = (TextBox) target;
                    this.txtDelayFrom.PreviewTextInput += new TextCompositionEventHandler(this.NumberCheck);
                    this.txtDelayFrom.TextChanged += new TextChangedEventHandler(this.txtDelayFrom_TextChanged);
                    return;

                case 11:
                    this.txtDelayTo = (TextBox) target;
                    this.txtDelayTo.PreviewTextInput += new TextCompositionEventHandler(this.NumberCheck);
                    this.txtDelayTo.TextChanged += new TextChangedEventHandler(this.txtDelayTo_TextChanged);
                    return;

                case 12:
                    this.gbSizing = (GroupBox) target;
                    return;

                case 13:
                    this.txtSize = (TextBox) target;
                    this.txtSize.TextChanged += new TextChangedEventHandler(this.txtSize_TextChanged);
                    return;

                case 14:
                    this.chSizeRandom = (CheckBox) target;
                    this.chSizeRandom.Checked += new RoutedEventHandler(this.chSizeRandom_Checked);
                    this.chSizeRandom.Unchecked += new RoutedEventHandler(this.chSizeRandom_Checked);
                    return;

                case 15:
                    this.chPickRandomNotAvailable = (CheckBox) target;
                    this.chPickRandomNotAvailable.Checked += new RoutedEventHandler(this.chPickRandomNotAvailable_Checked);
                    this.chPickRandomNotAvailable.Unchecked += new RoutedEventHandler(this.chPickRandomNotAvailable_Checked);
                    return;

                case 0x10:
                    this.switchQuicktaskAutopaste = (ToggleButton) target;
                    this.switchQuicktaskAutopaste.Checked += new RoutedEventHandler(this.switchQuicktaskAutopaste_Checked);
                    this.switchQuicktaskAutopaste.Unchecked += new RoutedEventHandler(this.switchQuicktaskAutopaste_Checked);
                    return;

                case 0x11:
                    this.btnReset = (Button) target;
                    this.btnReset.Click += new RoutedEventHandler(this.btnReset_Click);
                    return;

                case 0x12:
                    this.switchLogCleaner = (ToggleButton) target;
                    this.switchLogCleaner.Checked += new RoutedEventHandler(this.switchLogCleaner_Checked);
                    this.switchLogCleaner.Unchecked += new RoutedEventHandler(this.switchLogCleaner_Checked);
                    return;

                case 0x13:
                    this.lblLogCleanerText = (TextBlock) target;
                    return;

                case 20:
                    this.txtLogReset = (TextBox) target;
                    this.txtLogReset.PreviewTextInput += new TextCompositionEventHandler(this.txtLogReset_PreviewTextInput);
                    this.txtLogReset.TextChanged += new TextChangedEventHandler(this.txtLogReset_TextChanged);
                    return;

                case 0x15:
                    this.lblLogCleanerMinutes = (TextBlock) target;
                    return;

                case 0x16:
                    this.switchSolverBeep = (ToggleButton) target;
                    this.switchSolverBeep.Checked += new RoutedEventHandler(this.switchSolverBeep_Checked);
                    this.switchSolverBeep.Unchecked += new RoutedEventHandler(this.switchSolverBeep_Checked);
                    return;

                case 0x17:
                    this.switchAtcBeep = (ToggleButton) target;
                    this.switchAtcBeep.Checked += new RoutedEventHandler(this.switchAtcBeep_Checked);
                    this.switchAtcBeep.Unchecked += new RoutedEventHandler(this.switchAtcBeep_Checked);
                    return;

                case 0x18:
                    this.switchCheckoutBeep = (ToggleButton) target;
                    this.switchCheckoutBeep.Checked += new RoutedEventHandler(this.switchCheckoutBeep_Checked);
                    this.switchCheckoutBeep.Unchecked += new RoutedEventHandler(this.switchCheckoutBeep_Checked);
                    return;

                case 0x19:
                    this.switchPaypalBeep = (ToggleButton) target;
                    this.switchPaypalBeep.Checked += new RoutedEventHandler(this.switchPaypalBeep_Checked);
                    this.switchPaypalBeep.Unchecked += new RoutedEventHandler(this.switchPaypalBeep_Checked);
                    return;

                case 0x1a:
                    this.rEnvLight = (RadioButton) target;
                    this.rEnvLight.Checked += new RoutedEventHandler(this.rEnv_Checked);
                    this.rEnvLight.Unchecked += new RoutedEventHandler(this.rEnv_Checked);
                    return;

                case 0x1b:
                    this.rEnvDark = (RadioButton) target;
                    this.rEnvDark.Checked += new RoutedEventHandler(this.rEnv_Checked);
                    this.rEnvDark.Unchecked += new RoutedEventHandler(this.rEnv_Checked);
                    return;

                case 0x1c:
                    this.btnExportSettings = (Button) target;
                    this.btnExportSettings.Click += new RoutedEventHandler(this.btnExportSettings_Click);
                    return;

                case 0x1d:
                    this.btnImportSettings = (Button) target;
                    this.btnImportSettings.Click += new RoutedEventHandler(this.btnImportSettings_Click);
                    return;
            }
            this._contentLoaded = true;
        }

        private void txtDelay_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!this._isInit)
            {
                Global.SETTINGS.QUICK_TASK.Delay = this.txtDelay.Text.Trim();
                Helpers.SaveSettings();
            }
        }

        private void txtDelayFrom_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!this._isInit)
            {
                Global.SETTINGS.QUICK_TASK.DelayFrom = this.txtDelayFrom.Text.Trim();
                Helpers.SaveSettings();
            }
        }

        private void txtDelayTo_TextChanged(object sender, TextChangedEventArgs e)
        {
            // This item is obfuscated and can not be translated.
            if (!this._isInit)
            {
                goto Label_002C;
            }
        Label_000D:
            switch (((0xf3aeaed ^ 0x404199db) % 4))
            {
                case 1:
                    break;

                case 2:
                    return;

                case 3:
                    goto Label_000D;

                default:
                    Helpers.SaveSettings();
                    return;
            }
        Label_002C:
            Global.SETTINGS.QUICK_TASK.DelayTo = this.txtDelayTo.Text.Trim();
            goto Label_000D;
        }

        private void txtLogReset_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        private void txtLogReset_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtLogReset.Text))
            {
                Global.SETTINGS.LogCleanerMinutes = int.Parse(this.txtLogReset.Text.Trim());
                Helpers.SaveSettings();
            }
        }

        private void txtShopifySmartSchedule_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!this._isInit)
            {
                try
                {
                    Global.SETTINGS.ShopifySmartDelay = int.Parse(this.txtShopifySmartSchedule.Text.Trim());
                }
                catch
                {
                    Global.SETTINGS.ShopifySmartDelay = 10;
                }
                Helpers.SaveSettings();
            }
        }

        private void txtSize_TextChanged(object sender, TextChangedEventArgs e)
        {
            // This item is obfuscated and can not be translated.
            if (!this._isInit)
            {
                goto Label_002C;
            }
        Label_000D:
            switch (((0x41b5dac9 ^ 0x15f0d480) % 4))
            {
                case 0:
                    goto Label_000D;

                case 1:
                    return;

                case 2:
                    return;

                case 3:
                    break;

                default:
                    return;
            }
        Label_002C:
            Global.SETTINGS.QUICK_TASK.Size = this.txtSize.Text.Trim();
            Helpers.SaveSettings();
            goto Label_000D;
        }
    }
}

