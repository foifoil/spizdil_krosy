namespace EveAIO.Views
{
    using EveAIO;
    using EveAIO.Captcha;
    using EveAIO.Pocos;
    using System;
    using System.CodeDom.Compiler;
    using System.Collections;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Markup;
    using System.Windows.Media;

    public class CaptchaView : Page, IComponentConnector
    {
        private Random _rnd;
        private bool _isInit;
        internal Button btnManualSolver1;
        internal ComboBox cmbProxyList1;
        internal Button btnManualSolver1Clear;
        internal ToggleButton switchSolver1;
        internal Button btnManualSolver2;
        internal ComboBox cmbProxyList2;
        internal Button btnManualSolver2Clear;
        internal ToggleButton switchSolver2;
        internal Button btnManualSolver3;
        internal ComboBox cmbProxyList3;
        internal Button btnManualSolver3Clear;
        internal ToggleButton switchSolver3;
        internal Button btnManualSolver4;
        internal ComboBox cmbProxyList4;
        internal Button btnManualSolver4Clear;
        internal ToggleButton switchSolver4;
        internal Button btnManualSolver5;
        internal ComboBox cmbProxyList5;
        internal Button btnManualSolver5Clear;
        internal ToggleButton switchSolver5;
        public TextBox txtTwoCaptchaApiKey;
        internal TextBlock lblTwoCaptchaBalance;
        internal ProgressBar progBarTwoCaptcha;
        internal Button btnTwoChapchaBalance;
        public TextBox txtAntiCaptchaApiKey;
        internal TextBlock lblAntiCaptchaBalance;
        internal ProgressBar progBarAntiCaptcha;
        internal Button btnAntiChapchaBalance;
        public TextBox txtImageTypersUsername;
        public TextBox txtImageTypersPassword;
        internal TextBlock lblImageTypersBalance;
        internal ProgressBar progBarImageTypers;
        internal Button btnImageTypersBalance;
        public TextBox txtDisolveApiKey;
        public TextBox txtDisolveIp;
        private bool _contentLoaded;

        public CaptchaView()
        {
            Class7.RIuqtBYzWxthF();
            this._rnd = new Random(DateTime.Now.Millisecond);
            this._rnd.Next(0, 500);
            this.InitializeComponent();
            this.txtTwoCaptchaApiKey.Text = Global.SETTINGS.TwoCaptchaApiKey;
            this.txtAntiCaptchaApiKey.Text = Global.SETTINGS.AntiCaptchaApiKey;
            this.txtImageTypersUsername.Text = Global.SETTINGS.ImageTypersUsername;
            this.txtImageTypersPassword.Text = Global.SETTINGS.ImageTypersPassword;
            this.txtDisolveApiKey.Text = Global.SETTINGS.DisolveApiKey;
            this.txtDisolveIp.Text = Global.SETTINGS.DisolveIp;
            this.switchSolver1.IsChecked = new bool?(Global.SETTINGS.Solver1Enabled);
            this.switchSolver2.IsChecked = new bool?(Global.SETTINGS.Solver2Enabled);
            this.switchSolver3.IsChecked = new bool?(Global.SETTINGS.Solver3Enabled);
            this.switchSolver4.IsChecked = new bool?(Global.SETTINGS.Solver4Enabled);
            this.switchSolver5.IsChecked = new bool?(Global.SETTINGS.Solver5Enabled);
            if (((!Global.SETTINGS.Solver1Enabled && !Global.SETTINGS.Solver2Enabled) && (!Global.SETTINGS.Solver3Enabled && !Global.SETTINGS.Solver4Enabled)) && !Global.SETTINGS.Solver5Enabled)
            {
                this.switchSolver1.IsChecked = true;
            }
        }

        private void btnAntiChapchaBalance_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtAntiCaptchaApiKey.Text))
            {
                this.txtAntiCaptchaApiKey.Background = (Brush) Application.Current.FindResource("MissingFieldBackground");
            }
            else
            {
                TaskScheduler current;
                this.txtAntiCaptchaApiKey.Background = (Brush) Application.Current.FindResource("ButtonBackground");
                if (SynchronizationContext.Current != null)
                {
                    current = TaskScheduler.FromCurrentSynchronizationContext();
                }
                else
                {
                    current = TaskScheduler.Current;
                }
                this.btnAntiChapchaBalance.IsEnabled = false;
                this.btnAntiChapchaBalance.Opacity = 0.6;
                this.progBarAntiCaptcha.Visibility = Visibility.Visible;
                this.progBarAntiCaptcha.IsEnabled = true;
                double balance = -1.0;
                Task.Factory.StartNew(() => balance = CaptchaSolver.AntiCaptchaGetBalance()).ContinueWith(delegate (Task t) {
                    if (balance == -1.0)
                    {
                        this.lblAntiCaptchaBalance.Text = "error";
                    }
                    else
                    {
                        this.lblAntiCaptchaBalance.Text = "$" + balance.ToString();
                    }
                    this.btnAntiChapchaBalance.IsEnabled = true;
                    this.btnAntiChapchaBalance.Opacity = 1.0;
                    this.progBarAntiCaptcha.Visibility = Visibility.Hidden;
                    this.progBarAntiCaptcha.IsEnabled = false;
                }, current);
            }
        }

        private void btnImageTypersBalance_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtImageTypersUsername.Text) && !string.IsNullOrEmpty(this.txtImageTypersPassword.Text))
            {
                TaskScheduler current;
                this.txtImageTypersUsername.Background = (Brush) Application.Current.FindResource("ButtonBackground");
                this.txtImageTypersPassword.Background = (Brush) Application.Current.FindResource("ButtonBackground");
                if (SynchronizationContext.Current != null)
                {
                    current = TaskScheduler.FromCurrentSynchronizationContext();
                }
                else
                {
                    current = TaskScheduler.Current;
                }
                this.btnImageTypersBalance.IsEnabled = false;
                this.btnImageTypersBalance.Opacity = 0.6;
                this.progBarImageTypers.Visibility = Visibility.Visible;
                this.progBarImageTypers.IsEnabled = true;
                double balance = -1.0;
                Task.Factory.StartNew(() => balance = CaptchaSolver.ImageTypersGetBalance()).ContinueWith(delegate (Task t) {
                    if (balance == -1.0)
                    {
                        this.lblImageTypersBalance.Text = "error";
                    }
                    else
                    {
                        this.lblImageTypersBalance.Text = "$" + balance.ToString();
                    }
                    this.btnImageTypersBalance.IsEnabled = true;
                    this.btnImageTypersBalance.Opacity = 1.0;
                    this.progBarImageTypers.Visibility = Visibility.Hidden;
                    this.progBarImageTypers.IsEnabled = false;
                }, current);
            }
            else
            {
                if (string.IsNullOrEmpty(this.txtImageTypersUsername.Text))
                {
                    this.txtImageTypersUsername.Background = (Brush) Application.Current.FindResource("MissingFieldBackground");
                }
                else
                {
                    this.txtImageTypersUsername.Background = (Brush) Application.Current.FindResource("ButtonBackground");
                }
                if (string.IsNullOrEmpty(this.txtImageTypersPassword.Text))
                {
                    this.txtImageTypersPassword.Background = (Brush) Application.Current.FindResource("MissingFieldBackground");
                }
                else
                {
                    this.txtImageTypersPassword.Background = (Brush) Application.Current.FindResource("ButtonBackground");
                }
            }
        }

        private void btnManualSolver1_Click(object sender, RoutedEventArgs e)
        {
            if (Global.CAPTCHA_SOLVER1 == null)
            {
                Global.CAPTCHA_SOLVER1 = new CaptchaSolverWindow("window1");
                Global.CAPTCHA_SOLVER1.Show();
            }
            else
            {
                if (!Global.CaptchaSolver1Opened)
                {
                    Global.CAPTCHA_SOLVER1.Show();
                    Global.CaptchaSolver1Opened = true;
                }
                Global.CAPTCHA_SOLVER1.Activate();
            }
        }

        private void btnManualSolver1Clear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Global.CAPTCHA_SOLVER1.Stop(true);
                try
                {
                    try
                    {
                        Global.CAPTCHA_SOLVER1.BROWSER.Close();
                    }
                    catch
                    {
                    }
                    Global.CAPTCHA_SOLVER1.IsHardClose = true;
                    Global.CAPTCHA_SOLVER1.Close();
                }
                catch
                {
                }
                Global.CaptchaSolver1Opened = false;
                Global.CAPTCHA_SOLVER1 = new CaptchaSolverWindow("window1");
                Global.CAPTCHA_SOLVER1.Visibility = Visibility.Hidden;
                Global.CAPTCHA_SOLVER1.Show();
                Global.CAPTCHA_SOLVER1.Hide();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error clearing solver cache", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                Global.Logger.Error("Error clearing solver cache", exception);
            }
        }

        private void btnManualSolver2_Click(object sender, RoutedEventArgs e)
        {
            if (Global.CAPTCHA_SOLVER2 == null)
            {
                Global.CAPTCHA_SOLVER2 = new CaptchaSolverWindow("window2");
                Global.CAPTCHA_SOLVER2.Show();
            }
            else
            {
                if (!Global.CaptchaSolver2Opened)
                {
                    Global.CAPTCHA_SOLVER2.Show();
                    Global.CaptchaSolver2Opened = true;
                }
                Global.CAPTCHA_SOLVER2.Activate();
            }
        }

        private void btnManualSolver2Clear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Global.CAPTCHA_SOLVER2.Stop(true);
                try
                {
                    try
                    {
                        Global.CAPTCHA_SOLVER2.BROWSER.Close();
                    }
                    catch
                    {
                    }
                    Global.CAPTCHA_SOLVER2.IsHardClose = true;
                    Global.CAPTCHA_SOLVER2.Close();
                }
                catch
                {
                }
                Global.CaptchaSolver2Opened = false;
                Global.CAPTCHA_SOLVER2 = new CaptchaSolverWindow("window2");
                Global.CAPTCHA_SOLVER2.Visibility = Visibility.Hidden;
                Global.CAPTCHA_SOLVER2.Show();
                Global.CAPTCHA_SOLVER2.Hide();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error clearing solver cache", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                Global.Logger.Error("Error clearing solver cache", exception);
            }
        }

        private void btnManualSolver3_Click(object sender, RoutedEventArgs e)
        {
            if (Global.CAPTCHA_SOLVER3 == null)
            {
                Global.CAPTCHA_SOLVER3 = new CaptchaSolverWindow("window3");
                Global.CAPTCHA_SOLVER3.Show();
            }
            else
            {
                if (!Global.CaptchaSolver3Opened)
                {
                    Global.CAPTCHA_SOLVER3.Show();
                    Global.CaptchaSolver3Opened = true;
                }
                Global.CAPTCHA_SOLVER3.Activate();
            }
        }

        private void btnManualSolver3Clear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Global.CAPTCHA_SOLVER3.Stop(true);
                try
                {
                    try
                    {
                        Global.CAPTCHA_SOLVER3.BROWSER.Close();
                    }
                    catch
                    {
                    }
                    Global.CAPTCHA_SOLVER3.IsHardClose = true;
                    Global.CAPTCHA_SOLVER3.Close();
                }
                catch
                {
                }
                Global.CaptchaSolver3Opened = false;
                Global.CAPTCHA_SOLVER3 = new CaptchaSolverWindow("window3");
                Global.CAPTCHA_SOLVER3.Visibility = Visibility.Hidden;
                Global.CAPTCHA_SOLVER3.Show();
                Global.CAPTCHA_SOLVER3.Hide();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error clearing solver cache", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                Global.Logger.Error("Error clearing solver cache", exception);
            }
        }

        private void btnManualSolver4_Click(object sender, RoutedEventArgs e)
        {
            if (Global.CAPTCHA_SOLVER4 == null)
            {
                Global.CAPTCHA_SOLVER4 = new CaptchaSolverWindow("window4");
                Global.CAPTCHA_SOLVER4.Show();
            }
            else
            {
                if (!Global.CaptchaSolver4Opened)
                {
                    Global.CAPTCHA_SOLVER4.Show();
                    Global.CaptchaSolver4Opened = true;
                }
                Global.CAPTCHA_SOLVER4.Activate();
            }
        }

        private void btnManualSolver4Clear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Global.CAPTCHA_SOLVER4.Stop(true);
                try
                {
                    try
                    {
                        Global.CAPTCHA_SOLVER1.BROWSER.Close();
                    }
                    catch
                    {
                    }
                    Global.CAPTCHA_SOLVER4.IsHardClose = true;
                    Global.CAPTCHA_SOLVER4.Close();
                }
                catch
                {
                }
                Global.CaptchaSolver4Opened = false;
                Global.CAPTCHA_SOLVER4 = new CaptchaSolverWindow("window4");
                Global.CAPTCHA_SOLVER4.Visibility = Visibility.Hidden;
                Global.CAPTCHA_SOLVER4.Show();
                Global.CAPTCHA_SOLVER4.Hide();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error clearing solver cache", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                Global.Logger.Error("Error clearing solver cache", exception);
            }
        }

        private void btnManualSolver5_Click(object sender, RoutedEventArgs e)
        {
            if (Global.CAPTCHA_SOLVER5 == null)
            {
                Global.CAPTCHA_SOLVER5 = new CaptchaSolverWindow("window5");
                Global.CAPTCHA_SOLVER5.Show();
            }
            else
            {
                if (!Global.CaptchaSolver5Opened)
                {
                    Global.CAPTCHA_SOLVER5.Show();
                    Global.CaptchaSolver5Opened = true;
                }
                Global.CAPTCHA_SOLVER5.Activate();
            }
        }

        private void btnManualSolver5Clear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Global.CAPTCHA_SOLVER5.Stop(true);
                try
                {
                    try
                    {
                        Global.CAPTCHA_SOLVER5.BROWSER.Close();
                    }
                    catch
                    {
                    }
                    Global.CAPTCHA_SOLVER5.IsHardClose = true;
                    Global.CAPTCHA_SOLVER5.Close();
                }
                catch
                {
                }
                Global.CaptchaSolver5Opened = false;
                Global.CAPTCHA_SOLVER5 = new CaptchaSolverWindow("window5");
                Global.CAPTCHA_SOLVER5.Visibility = Visibility.Hidden;
                Global.CAPTCHA_SOLVER5.Show();
                Global.CAPTCHA_SOLVER5.Hide();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error clearing solver cache", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                Global.Logger.Error("Error clearing solver cache", exception);
            }
        }

        private void btnTwoChapchaBalance_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtTwoCaptchaApiKey.Text))
            {
                this.txtTwoCaptchaApiKey.Background = (Brush) Application.Current.FindResource("MissingFieldBackground");
            }
            else
            {
                TaskScheduler current;
                this.txtTwoCaptchaApiKey.Background = (Brush) Application.Current.FindResource("ButtonBackground");
                if (SynchronizationContext.Current == null)
                {
                    current = TaskScheduler.Current;
                }
                else
                {
                    current = TaskScheduler.FromCurrentSynchronizationContext();
                }
                this.btnTwoChapchaBalance.IsEnabled = false;
                this.btnTwoChapchaBalance.Opacity = 0.6;
                this.progBarTwoCaptcha.Visibility = Visibility.Visible;
                this.progBarTwoCaptcha.IsEnabled = true;
                double balance = -1.0;
                Task.Factory.StartNew(() => balance = CaptchaSolver.TwoCaptchaGetBalance()).ContinueWith(delegate (Task t) {
                    if (balance == -1.0)
                    {
                        this.lblTwoCaptchaBalance.Text = "error";
                    }
                    else
                    {
                        this.lblTwoCaptchaBalance.Text = "$" + balance.ToString();
                    }
                    this.btnTwoChapchaBalance.IsEnabled = true;
                    this.btnTwoChapchaBalance.Opacity = 1.0;
                    this.progBarTwoCaptcha.Visibility = Visibility.Hidden;
                    this.progBarTwoCaptcha.IsEnabled = false;
                }, current);
            }
        }

        private void cmbProxyList1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!this._isInit)
            {
                Global.SETTINGS.Solver1ProxyList = ((this.cmbProxyList1.SelectedItem == null) || (((ComboBoxItem) this.cmbProxyList1.SelectedItem).Tag.ToString() == "-1")) ? "" : ((ComboBoxItem) this.cmbProxyList1.SelectedItem).Tag.ToString();
                try
                {
                    Global.CAPTCHA_SOLVER1.Stop(false);
                    try
                    {
                        try
                        {
                            Global.CAPTCHA_SOLVER1.BROWSER.Close();
                        }
                        catch
                        {
                        }
                        Global.CAPTCHA_SOLVER1.Close();
                    }
                    catch
                    {
                    }
                    Global.CAPTCHA_SOLVER1 = new CaptchaSolverWindow("window1");
                    Global.CAPTCHA_SOLVER1.Visibility = Visibility.Hidden;
                    Global.CAPTCHA_SOLVER1.Show();
                    Global.CAPTCHA_SOLVER1.Hide();
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Error clearing solver cache", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                    Global.Logger.Error("Error clearing solver cache", exception);
                }
                Helpers.SaveSettings();
            }
        }

        private void cmbProxyList2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!this._isInit)
            {
                Global.SETTINGS.Solver2ProxyList = ((this.cmbProxyList2.SelectedItem == null) || (((ComboBoxItem) this.cmbProxyList2.SelectedItem).Tag.ToString() == "-1")) ? "" : ((ComboBoxItem) this.cmbProxyList2.SelectedItem).Tag.ToString();
                try
                {
                    Global.CAPTCHA_SOLVER2.Stop(false);
                    try
                    {
                        try
                        {
                            Global.CAPTCHA_SOLVER2.BROWSER.Close();
                        }
                        catch
                        {
                        }
                        Global.CAPTCHA_SOLVER2.Close();
                    }
                    catch
                    {
                    }
                    Global.CAPTCHA_SOLVER2 = new CaptchaSolverWindow("window2");
                    Global.CAPTCHA_SOLVER2.Visibility = Visibility.Hidden;
                    Global.CAPTCHA_SOLVER2.Show();
                    Global.CAPTCHA_SOLVER2.Hide();
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Error clearing solver cache", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                    Global.Logger.Error("Error clearing solver cache", exception);
                }
                Helpers.SaveSettings();
            }
        }

        private void cmbProxyList3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!this._isInit)
            {
                Global.SETTINGS.Solver3ProxyList = ((this.cmbProxyList3.SelectedItem == null) || (((ComboBoxItem) this.cmbProxyList3.SelectedItem).Tag.ToString() == "-1")) ? "" : ((ComboBoxItem) this.cmbProxyList3.SelectedItem).Tag.ToString();
                try
                {
                    Global.CAPTCHA_SOLVER3.Stop(false);
                    try
                    {
                        try
                        {
                            Global.CAPTCHA_SOLVER3.BROWSER.Close();
                        }
                        catch
                        {
                        }
                        Global.CAPTCHA_SOLVER3.Close();
                    }
                    catch
                    {
                    }
                    Global.CAPTCHA_SOLVER3 = new CaptchaSolverWindow("window3");
                    Global.CAPTCHA_SOLVER3.Visibility = Visibility.Hidden;
                    Global.CAPTCHA_SOLVER3.Show();
                    Global.CAPTCHA_SOLVER3.Hide();
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Error clearing solver cache", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                    Global.Logger.Error("Error clearing solver cache", exception);
                }
                Helpers.SaveSettings();
            }
        }

        private void cmbProxyList4_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!this._isInit)
            {
                Global.SETTINGS.Solver4ProxyList = ((this.cmbProxyList4.SelectedItem == null) || (((ComboBoxItem) this.cmbProxyList4.SelectedItem).Tag.ToString() == "-1")) ? "" : ((ComboBoxItem) this.cmbProxyList4.SelectedItem).Tag.ToString();
                try
                {
                    Global.CAPTCHA_SOLVER4.Stop(true);
                    try
                    {
                        try
                        {
                            Global.CAPTCHA_SOLVER4.BROWSER.Close();
                        }
                        catch
                        {
                        }
                        Global.CAPTCHA_SOLVER4.Close();
                    }
                    catch
                    {
                    }
                    Global.CAPTCHA_SOLVER4 = new CaptchaSolverWindow("window4");
                    Global.CAPTCHA_SOLVER4.Visibility = Visibility.Hidden;
                    Global.CAPTCHA_SOLVER4.Show();
                    Global.CAPTCHA_SOLVER4.Hide();
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Error clearing solver cache", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                    Global.Logger.Error("Error clearing solver cache", exception);
                }
                Helpers.SaveSettings();
            }
        }

        private void cmbProxyList5_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!this._isInit)
            {
                Global.SETTINGS.Solver5ProxyList = ((this.cmbProxyList5.SelectedItem == null) || (((ComboBoxItem) this.cmbProxyList5.SelectedItem).Tag.ToString() == "-1")) ? "" : ((ComboBoxItem) this.cmbProxyList5.SelectedItem).Tag.ToString();
                try
                {
                    Global.CAPTCHA_SOLVER5.Stop(false);
                    try
                    {
                        try
                        {
                            Global.CAPTCHA_SOLVER5.BROWSER.Close();
                        }
                        catch
                        {
                        }
                        Global.CAPTCHA_SOLVER5.Close();
                    }
                    catch
                    {
                    }
                    Global.CAPTCHA_SOLVER5 = new CaptchaSolverWindow("window5");
                    Global.CAPTCHA_SOLVER5.Visibility = Visibility.Hidden;
                    Global.CAPTCHA_SOLVER5.Show();
                    Global.CAPTCHA_SOLVER5.Hide();
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Error clearing solver cache", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                    Global.Logger.Error("Error clearing solver cache", exception);
                }
                Helpers.SaveSettings();
            }
        }

        public void Init()
        {
            ComboBoxItem item;
            IEnumerator enumerator2;
            this._isInit = true;
            this.cmbProxyList1.Items.Clear();
            ComboBoxItem newItem = new ComboBoxItem {
                Content = "",
                Tag = "-1"
            };
            this.cmbProxyList1.Items.Add(newItem);
            foreach (ProxyListObject obj5 in Global.SETTINGS.PROXIES)
            {
                item = new ComboBoxItem();
                object[] objArray1 = new object[] { obj5.Name, " (", obj5.ProxiesCount, ")" };
                item.Content = string.Concat(objArray1);
                item.Tag = obj5.Id;
                this.cmbProxyList1.Items.Add(item);
            }
            this.cmbProxyList2.Items.Clear();
            ComboBoxItem item7 = new ComboBoxItem {
                Content = "",
                Tag = "-1"
            };
            this.cmbProxyList2.Items.Add(item7);
            foreach (ProxyListObject obj4 in Global.SETTINGS.PROXIES)
            {
                item = new ComboBoxItem();
                object[] objArray2 = new object[] { obj4.Name, " (", obj4.ProxiesCount, ")" };
                item.Content = string.Concat(objArray2);
                item.Tag = obj4.Id;
                this.cmbProxyList2.Items.Add(item);
            }
            this.cmbProxyList3.Items.Clear();
            ComboBoxItem item8 = new ComboBoxItem {
                Content = "",
                Tag = "-1"
            };
            this.cmbProxyList3.Items.Add(item8);
            foreach (ProxyListObject obj2 in Global.SETTINGS.PROXIES)
            {
                item = new ComboBoxItem();
                object[] objArray3 = new object[] { obj2.Name, " (", obj2.ProxiesCount, ")" };
                item.Content = string.Concat(objArray3);
                item.Tag = obj2.Id;
                this.cmbProxyList3.Items.Add(item);
            }
            this.cmbProxyList4.Items.Clear();
            ComboBoxItem item9 = new ComboBoxItem {
                Content = "",
                Tag = "-1"
            };
            this.cmbProxyList4.Items.Add(item9);
            foreach (ProxyListObject obj3 in Global.SETTINGS.PROXIES)
            {
                item = new ComboBoxItem();
                object[] objArray4 = new object[] { obj3.Name, " (", obj3.ProxiesCount, ")" };
                item.Content = string.Concat(objArray4);
                item.Tag = obj3.Id;
                this.cmbProxyList4.Items.Add(item);
            }
            this.cmbProxyList5.Items.Clear();
            ComboBoxItem item10 = new ComboBoxItem {
                Content = "",
                Tag = "-1"
            };
            this.cmbProxyList5.Items.Add(item10);
            foreach (ProxyListObject obj6 in Global.SETTINGS.PROXIES)
            {
                item = new ComboBoxItem();
                object[] objArray5 = new object[] { obj6.Name, " (", obj6.ProxiesCount, ")" };
                item.Content = string.Concat(objArray5);
                item.Tag = obj6.Id;
                this.cmbProxyList5.Items.Add(item);
            }
            using (enumerator2 = ((IEnumerable) this.cmbProxyList1.Items).GetEnumerator())
            {
                ComboBoxItem current;
                while (enumerator2.MoveNext())
                {
                    current = (ComboBoxItem) enumerator2.Current;
                    if (Global.SETTINGS.Solver1ProxyList == current.Tag.ToString())
                    {
                        goto Label_044F;
                    }
                }
                goto Label_0473;
            Label_044F:
                this.cmbProxyList1.SelectedItem = current;
            }
        Label_0473:
            using (enumerator2 = ((IEnumerable) this.cmbProxyList2.Items).GetEnumerator())
            {
                ComboBoxItem current;
                while (enumerator2.MoveNext())
                {
                    current = (ComboBoxItem) enumerator2.Current;
                    if (Global.SETTINGS.Solver2ProxyList == current.Tag.ToString())
                    {
                        goto Label_04BE;
                    }
                }
                goto Label_04E2;
            Label_04BE:
                this.cmbProxyList2.SelectedItem = current;
            }
        Label_04E2:
            using (enumerator2 = ((IEnumerable) this.cmbProxyList3.Items).GetEnumerator())
            {
                ComboBoxItem current;
                while (enumerator2.MoveNext())
                {
                    current = (ComboBoxItem) enumerator2.Current;
                    if (Global.SETTINGS.Solver3ProxyList == current.Tag.ToString())
                    {
                        goto Label_052D;
                    }
                }
                goto Label_0551;
            Label_052D:
                this.cmbProxyList3.SelectedItem = current;
            }
        Label_0551:
            using (enumerator2 = ((IEnumerable) this.cmbProxyList4.Items).GetEnumerator())
            {
                ComboBoxItem current;
                while (enumerator2.MoveNext())
                {
                    current = (ComboBoxItem) enumerator2.Current;
                    if (Global.SETTINGS.Solver4ProxyList == current.Tag.ToString())
                    {
                        goto Label_059C;
                    }
                }
                goto Label_05C0;
            Label_059C:
                this.cmbProxyList4.SelectedItem = current;
            }
        Label_05C0:
            using (enumerator2 = ((IEnumerable) this.cmbProxyList5.Items).GetEnumerator())
            {
                ComboBoxItem item2;
                goto Label_0600;
            Label_05D5:
                item2 = (ComboBoxItem) enumerator2.Current;
                if (Global.SETTINGS.Solver5ProxyList == item2.Tag.ToString())
                {
                    goto Label_060B;
                }
            Label_0600:
                if (!enumerator2.MoveNext())
                {
                    goto Label_0631;
                }
                goto Label_05D5;
            Label_060B:
                this.cmbProxyList5.SelectedItem = item2;
            }
        Label_0631:
            this._isInit = false;
        }

        [GeneratedCode("PresentationBuildTasks", "4.0.0.0"), DebuggerNonUserCode]
        public void InitializeComponent()
        {
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocator = new Uri("/EveAIO;component/views/captchaview.xaml", UriKind.Relative);
                Application.LoadComponent(this, resourceLocator);
            }
        }

        private void switchSolver1_Checked(object sender, RoutedEventArgs e)
        {
            if (this.switchSolver1.IsChecked.HasValue && this.switchSolver1.IsChecked.Value)
            {
                this.btnManualSolver1.IsEnabled = true;
                this.btnManualSolver1.Opacity = 1.0;
                this.btnManualSolver1Clear.IsEnabled = true;
                this.btnManualSolver1Clear.Opacity = 1.0;
                this.cmbProxyList1.IsEnabled = true;
                this.cmbProxyList1.Opacity = 1.0;
                Global.SETTINGS.Solver1Enabled = true;
            }
            else
            {
                this.btnManualSolver1.IsEnabled = false;
                this.btnManualSolver1.Opacity = 0.6;
                this.btnManualSolver1Clear.IsEnabled = false;
                this.btnManualSolver1Clear.Opacity = 0.6;
                this.cmbProxyList1.IsEnabled = false;
                this.cmbProxyList1.Opacity = 0.6;
                Global.SETTINGS.Solver1Enabled = false;
            }
            if (Global.CAPTCHA_SOLVER1 != null)
            {
                Global.CAPTCHA_SOLVER1.IsUserEnabled = Global.SETTINGS.Solver1Enabled;
            }
            Helpers.SaveSettings();
        }

        private void switchSolver2_Checked(object sender, RoutedEventArgs e)
        {
            if (this.switchSolver2.IsChecked.HasValue && this.switchSolver2.IsChecked.Value)
            {
                this.btnManualSolver2.IsEnabled = true;
                this.btnManualSolver2.Opacity = 1.0;
                this.btnManualSolver2Clear.IsEnabled = true;
                this.btnManualSolver2Clear.Opacity = 1.0;
                this.cmbProxyList2.IsEnabled = true;
                this.cmbProxyList2.Opacity = 1.0;
                Global.SETTINGS.Solver2Enabled = true;
            }
            else
            {
                this.btnManualSolver2.IsEnabled = false;
                this.btnManualSolver2.Opacity = 0.6;
                this.btnManualSolver2Clear.IsEnabled = false;
                this.btnManualSolver2Clear.Opacity = 0.6;
                this.cmbProxyList2.IsEnabled = false;
                this.cmbProxyList2.Opacity = 0.6;
                Global.SETTINGS.Solver2Enabled = false;
            }
            if (Global.CAPTCHA_SOLVER2 != null)
            {
                Global.CAPTCHA_SOLVER2.IsUserEnabled = Global.SETTINGS.Solver2Enabled;
            }
            Helpers.SaveSettings();
        }

        private void switchSolver3_Checked(object sender, RoutedEventArgs e)
        {
            if (this.switchSolver3.IsChecked.HasValue && this.switchSolver3.IsChecked.Value)
            {
                this.btnManualSolver3.IsEnabled = true;
                this.btnManualSolver3.Opacity = 1.0;
                this.btnManualSolver3Clear.IsEnabled = true;
                this.btnManualSolver3Clear.Opacity = 1.0;
                this.cmbProxyList3.IsEnabled = true;
                this.cmbProxyList3.Opacity = 1.0;
                Global.SETTINGS.Solver3Enabled = true;
            }
            else
            {
                this.btnManualSolver3.IsEnabled = false;
                this.btnManualSolver3.Opacity = 0.6;
                this.btnManualSolver3Clear.IsEnabled = false;
                this.btnManualSolver3Clear.Opacity = 0.6;
                this.cmbProxyList3.IsEnabled = false;
                this.cmbProxyList3.Opacity = 0.6;
                Global.SETTINGS.Solver3Enabled = false;
            }
            if (Global.CAPTCHA_SOLVER3 != null)
            {
                Global.CAPTCHA_SOLVER3.IsUserEnabled = Global.SETTINGS.Solver3Enabled;
            }
            Helpers.SaveSettings();
        }

        private void switchSolver4_Checked(object sender, RoutedEventArgs e)
        {
            if (this.switchSolver4.IsChecked.HasValue && this.switchSolver4.IsChecked.Value)
            {
                this.btnManualSolver4.IsEnabled = true;
                this.btnManualSolver4.Opacity = 1.0;
                this.btnManualSolver4Clear.IsEnabled = true;
                this.btnManualSolver4Clear.Opacity = 1.0;
                this.cmbProxyList4.IsEnabled = true;
                this.cmbProxyList4.Opacity = 1.0;
                Global.SETTINGS.Solver4Enabled = true;
            }
            else
            {
                this.btnManualSolver4.IsEnabled = false;
                this.btnManualSolver4.Opacity = 0.6;
                this.btnManualSolver4Clear.IsEnabled = false;
                this.btnManualSolver4Clear.Opacity = 0.6;
                this.cmbProxyList4.IsEnabled = false;
                this.cmbProxyList4.Opacity = 0.6;
                Global.SETTINGS.Solver4Enabled = false;
            }
            if (Global.CAPTCHA_SOLVER4 != null)
            {
                Global.CAPTCHA_SOLVER4.IsUserEnabled = Global.SETTINGS.Solver4Enabled;
            }
            Helpers.SaveSettings();
        }

        private void switchSolver5_Checked(object sender, RoutedEventArgs e)
        {
            if (this.switchSolver5.IsChecked.HasValue && this.switchSolver5.IsChecked.Value)
            {
                this.btnManualSolver5.IsEnabled = true;
                this.btnManualSolver5.Opacity = 1.0;
                this.btnManualSolver5Clear.IsEnabled = true;
                this.btnManualSolver5Clear.Opacity = 1.0;
                this.cmbProxyList5.IsEnabled = true;
                this.cmbProxyList5.Opacity = 1.0;
                Global.SETTINGS.Solver5Enabled = true;
            }
            else
            {
                this.btnManualSolver5.IsEnabled = false;
                this.btnManualSolver5.Opacity = 0.6;
                this.btnManualSolver5Clear.IsEnabled = false;
                this.btnManualSolver5Clear.Opacity = 0.6;
                this.cmbProxyList5.IsEnabled = false;
                this.cmbProxyList5.Opacity = 0.6;
                Global.SETTINGS.Solver5Enabled = false;
            }
            if (Global.CAPTCHA_SOLVER5 != null)
            {
                Global.CAPTCHA_SOLVER5.IsUserEnabled = Global.SETTINGS.Solver5Enabled;
            }
            Helpers.SaveSettings();
        }

        [DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0"), EditorBrowsable(EditorBrowsableState.Never)]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    this.btnManualSolver1 = (Button) target;
                    this.btnManualSolver1.Click += new RoutedEventHandler(this.btnManualSolver1_Click);
                    return;

                case 2:
                    this.cmbProxyList1 = (ComboBox) target;
                    this.cmbProxyList1.SelectionChanged += new SelectionChangedEventHandler(this.cmbProxyList1_SelectionChanged);
                    return;

                case 3:
                    this.btnManualSolver1Clear = (Button) target;
                    this.btnManualSolver1Clear.Click += new RoutedEventHandler(this.btnManualSolver1Clear_Click);
                    return;

                case 4:
                    this.switchSolver1 = (ToggleButton) target;
                    this.switchSolver1.Checked += new RoutedEventHandler(this.switchSolver1_Checked);
                    this.switchSolver1.Unchecked += new RoutedEventHandler(this.switchSolver1_Checked);
                    return;

                case 5:
                    this.btnManualSolver2 = (Button) target;
                    this.btnManualSolver2.Click += new RoutedEventHandler(this.btnManualSolver2_Click);
                    return;

                case 6:
                    this.cmbProxyList2 = (ComboBox) target;
                    this.cmbProxyList2.SelectionChanged += new SelectionChangedEventHandler(this.cmbProxyList2_SelectionChanged);
                    return;

                case 7:
                    this.btnManualSolver2Clear = (Button) target;
                    this.btnManualSolver2Clear.Click += new RoutedEventHandler(this.btnManualSolver2Clear_Click);
                    return;

                case 8:
                    this.switchSolver2 = (ToggleButton) target;
                    this.switchSolver2.Checked += new RoutedEventHandler(this.switchSolver2_Checked);
                    this.switchSolver2.Unchecked += new RoutedEventHandler(this.switchSolver2_Checked);
                    return;

                case 9:
                    this.btnManualSolver3 = (Button) target;
                    this.btnManualSolver3.Click += new RoutedEventHandler(this.btnManualSolver3_Click);
                    return;

                case 10:
                    this.cmbProxyList3 = (ComboBox) target;
                    this.cmbProxyList3.SelectionChanged += new SelectionChangedEventHandler(this.cmbProxyList3_SelectionChanged);
                    return;

                case 11:
                    this.btnManualSolver3Clear = (Button) target;
                    this.btnManualSolver3Clear.Click += new RoutedEventHandler(this.btnManualSolver3Clear_Click);
                    return;

                case 12:
                    this.switchSolver3 = (ToggleButton) target;
                    this.switchSolver3.Checked += new RoutedEventHandler(this.switchSolver3_Checked);
                    this.switchSolver3.Unchecked += new RoutedEventHandler(this.switchSolver3_Checked);
                    return;

                case 13:
                    this.btnManualSolver4 = (Button) target;
                    this.btnManualSolver4.Click += new RoutedEventHandler(this.btnManualSolver4_Click);
                    return;

                case 14:
                    this.cmbProxyList4 = (ComboBox) target;
                    this.cmbProxyList4.SelectionChanged += new SelectionChangedEventHandler(this.cmbProxyList4_SelectionChanged);
                    return;

                case 15:
                    this.btnManualSolver4Clear = (Button) target;
                    this.btnManualSolver4Clear.Click += new RoutedEventHandler(this.btnManualSolver4Clear_Click);
                    return;

                case 0x10:
                    this.switchSolver4 = (ToggleButton) target;
                    this.switchSolver4.Checked += new RoutedEventHandler(this.switchSolver4_Checked);
                    this.switchSolver4.Unchecked += new RoutedEventHandler(this.switchSolver4_Checked);
                    return;

                case 0x11:
                    this.btnManualSolver5 = (Button) target;
                    this.btnManualSolver5.Click += new RoutedEventHandler(this.btnManualSolver5_Click);
                    return;

                case 0x12:
                    this.cmbProxyList5 = (ComboBox) target;
                    this.cmbProxyList5.SelectionChanged += new SelectionChangedEventHandler(this.cmbProxyList5_SelectionChanged);
                    return;

                case 0x13:
                    this.btnManualSolver5Clear = (Button) target;
                    this.btnManualSolver5Clear.Click += new RoutedEventHandler(this.btnManualSolver5Clear_Click);
                    return;

                case 20:
                    this.switchSolver5 = (ToggleButton) target;
                    this.switchSolver5.Checked += new RoutedEventHandler(this.switchSolver5_Checked);
                    this.switchSolver5.Unchecked += new RoutedEventHandler(this.switchSolver5_Checked);
                    return;

                case 0x15:
                    this.txtTwoCaptchaApiKey = (TextBox) target;
                    this.txtTwoCaptchaApiKey.TextChanged += new TextChangedEventHandler(this.txtTwoCaptchaApiKey_TextChanged);
                    return;

                case 0x16:
                    this.lblTwoCaptchaBalance = (TextBlock) target;
                    return;

                case 0x17:
                    this.progBarTwoCaptcha = (ProgressBar) target;
                    return;

                case 0x18:
                    this.btnTwoChapchaBalance = (Button) target;
                    this.btnTwoChapchaBalance.Click += new RoutedEventHandler(this.btnTwoChapchaBalance_Click);
                    return;

                case 0x19:
                    this.txtAntiCaptchaApiKey = (TextBox) target;
                    this.txtAntiCaptchaApiKey.TextChanged += new TextChangedEventHandler(this.txtAntiCaptchaApiKey_TextChanged);
                    return;

                case 0x1a:
                    this.lblAntiCaptchaBalance = (TextBlock) target;
                    return;

                case 0x1b:
                    this.progBarAntiCaptcha = (ProgressBar) target;
                    return;

                case 0x1c:
                    this.btnAntiChapchaBalance = (Button) target;
                    this.btnAntiChapchaBalance.Click += new RoutedEventHandler(this.btnAntiChapchaBalance_Click);
                    return;

                case 0x1d:
                    this.txtImageTypersUsername = (TextBox) target;
                    this.txtImageTypersUsername.TextChanged += new TextChangedEventHandler(this.txtImageTypersUsername_TextChanged);
                    return;

                case 30:
                    this.txtImageTypersPassword = (TextBox) target;
                    this.txtImageTypersPassword.TextChanged += new TextChangedEventHandler(this.txtImageTypersPassword_TextChanged);
                    return;

                case 0x1f:
                    this.lblImageTypersBalance = (TextBlock) target;
                    return;

                case 0x20:
                    this.progBarImageTypers = (ProgressBar) target;
                    return;

                case 0x21:
                    this.btnImageTypersBalance = (Button) target;
                    this.btnImageTypersBalance.Click += new RoutedEventHandler(this.btnImageTypersBalance_Click);
                    return;

                case 0x22:
                    this.txtDisolveApiKey = (TextBox) target;
                    this.txtDisolveApiKey.TextChanged += new TextChangedEventHandler(this.txtDisolveApiKey_TextChanged);
                    return;

                case 0x23:
                    this.txtDisolveIp = (TextBox) target;
                    this.txtDisolveIp.TextChanged += new TextChangedEventHandler(this.txtDisolveIP_TextChanged);
                    return;
            }
            this._contentLoaded = true;
        }

        private void txtAntiCaptchaApiKey_TextChanged(object sender, TextChangedEventArgs e)
        {
            Global.SETTINGS.AntiCaptchaApiKey = this.txtAntiCaptchaApiKey.Text.Trim();
        }

        private void txtCvv_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        private void txtDisolveApiKey_TextChanged(object sender, TextChangedEventArgs e)
        {
            Global.SETTINGS.DisolveApiKey = this.txtDisolveApiKey.Text.Trim();
        }

        private void txtDisolveIP_TextChanged(object sender, TextChangedEventArgs e)
        {
            Global.SETTINGS.DisolveIp = this.txtDisolveIp.Text.Trim();
        }

        private void txtImageTypersPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            Global.SETTINGS.ImageTypersPassword = this.txtImageTypersPassword.Text.Trim();
        }

        private void txtImageTypersUsername_TextChanged(object sender, TextChangedEventArgs e)
        {
            Global.SETTINGS.ImageTypersUsername = this.txtImageTypersUsername.Text.Trim();
        }

        private void txtTwoCaptchaApiKey_TextChanged(object sender, TextChangedEventArgs e)
        {
            Global.SETTINGS.TwoCaptchaApiKey = this.txtTwoCaptchaApiKey.Text.Trim();
        }
    }
}

