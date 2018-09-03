namespace EveAIO.Views
{
    using EveAIO;
    using EveAIO.Pocos;
    using EveAIO.Privacy;
    using System;
    using System.CodeDom.Compiler;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Markup;
    using System.Windows.Media;

    public class ProfileWindow : Window, IComponentConnector
    {
        internal BillingView _billingView;
        internal ShippingView _shippingView;
        internal PaymentView _paymentView;
        internal PrivacyView _privacyView;
        private Global.FormOperation _operation;
        private ProfileObject _profile;
        internal ListBox lvMenu;
        internal ListBoxItem shippingMenuItem;
        internal ListBoxItem privacyMenuItem;
        internal Button BtnClose;
        internal TextBox txtProfileName;
        internal CheckBox chSameBilling;
        internal CheckBox chOneCheckoutPerSite;
        internal ComboBox cmbGroup;
        public System.Windows.Controls.Frame MenusFrame;
        internal Button btnGenerateDummy;
        internal Button btnSave;
        internal Button btnCancel;
        private bool _contentLoaded;

        public ProfileWindow(Window owner)
        {
            Class7.RIuqtBYzWxthF();
            this.InitializeComponent();
            base.Owner = owner;
            this._operation = Global.FormOperation.insert;
            ProfileObject obj1 = new ProfileObject {
                Id = Guid.NewGuid().ToString(),
                No = Global.SETTINGS.PROFILES.Count + 1
            };
            this._profile = obj1;
        }

        public ProfileWindow(Window owner, ProfileObject profile)
        {
            Class7.RIuqtBYzWxthF();
            this.InitializeComponent();
            base.Owner = owner;
            this._operation = Global.FormOperation.update;
            this._profile = profile;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            base.Close();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            base.Close();
        }

        private void btnGenerateDummy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Global.DI_DATA.Clear();
                int num = Global.RANDOM.Next(1, 3);
                Global.DI_DATA.Add("card", num.ToString());
                string str = Global.CLIENT.Post("http://card-generator.com/gencc.htm", Global.DI_DATA).Text();
                str = str.Substring(str.IndexOf("number"));
                str = str.Substring(str.IndexOf(">") + 1);
                string str3 = str.Substring(0, str.IndexOf("</"));
                str = str.Substring(str.IndexOf("cvv"));
                str = str.Substring(str.IndexOf(">") + 1);
                string str4 = str.Substring(0, str.IndexOf("</"));
                str = str.Substring(str.IndexOf("exp"));
                str = str.Substring(str.IndexOf(">") + 1);
                str.Substring(0, str.IndexOf("</"));
                str = str.Substring(str.IndexOf("name"));
                str = str.Substring(str.IndexOf(">") + 1);
                string str2 = str.Substring(0, str.IndexOf("</"));
                this._paymentView.textBox_0.Text = str3.Replace(" ", "");
                this._paymentView.txtNameOnCard.Text = str2;
                this._paymentView.txtCvv.Text = str4;
                if (num != 1)
                {
                    this._paymentView.cmbCardType.SelectedIndex = 2;
                }
                else
                {
                    this._paymentView.cmbCardType.SelectedIndex = 0;
                }
                this._paymentView.cmbExpirationMM.SelectedIndex = 5;
                this._paymentView.cmbExpirationYYYY.SelectedIndex = 3;
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error generating credit card", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                Global.Logger.Error("Error generating dummy credit card", exception);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (this.Check())
            {
                this._profile.Name = this.txtProfileName.Text.Trim();
                this._profile.FirstName = this._billingView.txtFirstNameBilling.Text.Trim();
                this._profile.LastName = this._billingView.txtLastNameBilling.Text.Trim();
                this._profile.Address1 = this._billingView.txtAddress1Billing.Text.Trim();
                this._profile.Address2 = this._billingView.txtAddress2Billing.Text.Trim();
                this._profile.City = this._billingView.txtCityBilling.Text.Trim();
                this._profile.Zip = this._billingView.txtZipBilling.Text.Trim();
                this._profile.Email = this._billingView.txtEmailBilling.Text.Trim();
                this._profile.Phone = Helpers.ExtractNumbers(this._billingView.txtPhoneBilling.Text.Trim());
                this._profile.CountryId = ((ComboBoxItem) this._billingView.cmbCountryBilling.SelectedItem).Tag.ToString();
                this._profile.StateId = (this._billingView.cmbStateBilling.SelectedItem != null) ? ((ComboBoxItem) this._billingView.cmbStateBilling.SelectedItem).Tag.ToString() : "";
                this._profile.State = this._billingView.txtStateBilling.Text.Trim();
                this._profile.VariousBilling = this._billingView.txtVariousBilling.Text.Trim();
                if (this._privacyView.switchPrivacy.IsChecked.HasValue && this._privacyView.switchPrivacy.IsChecked.Value)
                {
                    this._profile.Privacy = true;
                    PrivacyCard card = Global.PRIVACY_MANAGER.Cards.First<PrivacyCard>(x => x.CardId == ((ComboBoxItem) this._privacyView.cmbPrivacyCards.SelectedItem).Tag.ToString());
                    this._profile.PrivacyCardName = card.Name;
                }
                else if (string.IsNullOrEmpty(this._paymentView.lblPrivacyName.Text.Trim()))
                {
                    this._profile.Privacy = false;
                    this._profile.PrivacyCardName = "";
                }
                else
                {
                    this._profile.Privacy = true;
                }
                if (!this.chSameBilling.IsChecked.Value)
                {
                    this._profile.FirstNameShipping = this._shippingView.txtFirstNameShipping.Text.Trim();
                    this._profile.LastNameShipping = this._shippingView.txtLastNameShipping.Text.Trim();
                    this._profile.Address1Shipping = this._shippingView.txtAddress1Shipping.Text.Trim();
                    this._profile.Address2Shipping = this._shippingView.txtAddress2Shipping.Text.Trim();
                    this._profile.CityShipping = this._shippingView.txtCityShipping.Text.Trim();
                    this._profile.ZipShipping = this._shippingView.txtZipShipping.Text.Trim();
                    this._profile.EmailShipping = this._shippingView.txtEmailShipping.Text.Trim();
                    this._profile.PhoneShipping = Helpers.ExtractNumbers(this._shippingView.txtPhoneShipping.Text.Trim());
                    this._profile.CountryIdShipping = ((ComboBoxItem) this._shippingView.cmbCountryShipping.SelectedItem).Tag.ToString();
                    this._profile.StateIdShipping = (this._shippingView.cmbStateShipping.SelectedItem != null) ? ((ComboBoxItem) this._shippingView.cmbStateShipping.SelectedItem).Tag.ToString() : "";
                    this._profile.StateShipping = this._shippingView.txtStateShipping.Text.Trim();
                    this._profile.VariousShipping = this._shippingView.txtVariousShipping.Text.Trim();
                }
                else
                {
                    this._profile.FirstNameShipping = this._billingView.txtFirstNameBilling.Text.Trim();
                    this._profile.LastNameShipping = this._billingView.txtLastNameBilling.Text.Trim();
                    this._profile.Address1Shipping = this._billingView.txtAddress1Billing.Text.Trim();
                    this._profile.Address2Shipping = this._billingView.txtAddress2Billing.Text.Trim();
                    this._profile.CityShipping = this._billingView.txtCityBilling.Text.Trim();
                    this._profile.ZipShipping = this._billingView.txtZipBilling.Text.Trim();
                    this._profile.EmailShipping = this._billingView.txtEmailBilling.Text.Trim();
                    this._profile.PhoneShipping = Helpers.ExtractNumbers(this._billingView.txtPhoneBilling.Text.Trim());
                    this._profile.CountryIdShipping = ((ComboBoxItem) this._billingView.cmbCountryBilling.SelectedItem).Tag.ToString();
                    this._profile.StateIdShipping = (this._billingView.cmbStateBilling.SelectedItem != null) ? ((ComboBoxItem) this._billingView.cmbStateBilling.SelectedItem).Tag.ToString() : "";
                    this._profile.StateShipping = this._billingView.txtStateBilling.Text.Trim();
                    this._profile.VariousShipping = this._billingView.txtVariousBilling.Text.Trim();
                }
                this._profile.IdGroup = (this.cmbGroup.SelectedItem != null) ? ((ComboBoxItem) this.cmbGroup.SelectedItem).Tag.ToString() : "";
                this._profile.NameOnCard = this._paymentView.txtNameOnCard.Text.Trim();
                this._profile.CCNumber = this._paymentView.textBox_0.Text.Replace(" ", "").Trim();
                this._profile.Cvv = this._paymentView.txtCvv.Text.Trim();
                this._profile.CardTypeId = ((ComboBoxItem) this._paymentView.cmbCardType.SelectedItem).Tag.ToString();
                this._profile.ExpiryMonth = ((ComboBoxItem) this._paymentView.cmbExpirationMM.SelectedItem).Tag.ToString();
                this._profile.ExpiryYear = ((ComboBoxItem) this._paymentView.cmbExpirationYYYY.SelectedItem).Tag.ToString();
                this._profile.SameBillingShipping = this.chSameBilling.IsChecked.Value;
                this._profile.OnePerWebsite = this.chOneCheckoutPerSite.IsChecked.Value;
                try
                {
                    int num2 = int.Parse(this._paymentView.txtBirthdayDay.Text);
                    if ((num2 >= 1) && (num2 <= 0x1f))
                    {
                        this._profile.BirthdayDay = num2.ToString();
                    }
                    else
                    {
                        this._profile.BirthdayDay = "1";
                    }
                }
                catch
                {
                    this._profile.BirthdayDay = "1";
                }
                try
                {
                    int num3 = int.Parse(this._paymentView.txtBirthdayMonth.Text);
                    if ((num3 >= 1) && (num3 <= 12))
                    {
                        this._profile.BirthdayMonth = num3.ToString();
                    }
                    else
                    {
                        this._profile.BirthdayMonth = "1";
                    }
                }
                catch
                {
                    this._profile.BirthdayMonth = "1";
                }
                try
                {
                    int num4 = int.Parse(this._paymentView.txtBirthdayYear.Text);
                    if ((num4 >= 0x76c) && (num4 <= 0x7da))
                    {
                        this._profile.BirthdayYear = num4.ToString();
                    }
                    else
                    {
                        this._profile.BirthdayYear = "1";
                    }
                }
                catch
                {
                    this._profile.BirthdayYear = "1980";
                }
                if (this._operation == Global.FormOperation.insert)
                {
                    Global.SETTINGS.PROFILES.Add(this._profile);
                }
                Helpers.SaveSettings();
                base.Close();
            }
        }

        private bool Check()
        {
            bool flag = true;
            if (!string.IsNullOrEmpty(this.txtProfileName.Text))
            {
                this.txtProfileName.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
            }
            else
            {
                this.txtProfileName.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                flag = false;
            }
            if (string.IsNullOrEmpty(this._billingView.txtFirstNameBilling.Text))
            {
                this._billingView.txtFirstNameBilling.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                flag = false;
            }
            else
            {
                this._billingView.txtFirstNameBilling.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
            }
            if (!string.IsNullOrEmpty(this._billingView.txtLastNameBilling.Text))
            {
                this._billingView.txtLastNameBilling.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
            }
            else
            {
                this._billingView.txtLastNameBilling.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                flag = false;
            }
            if (string.IsNullOrEmpty(this._billingView.txtAddress1Billing.Text))
            {
                this._billingView.txtAddress1Billing.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                flag = false;
            }
            else
            {
                this._billingView.txtAddress1Billing.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
            }
            if (!string.IsNullOrEmpty(this._billingView.txtCityBilling.Text))
            {
                this._billingView.txtCityBilling.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
            }
            else
            {
                this._billingView.txtCityBilling.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                flag = false;
            }
            if (!string.IsNullOrEmpty(this._billingView.txtZipBilling.Text))
            {
                this._billingView.txtZipBilling.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
            }
            else
            {
                this._billingView.txtZipBilling.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                flag = false;
            }
            if (string.IsNullOrEmpty(this._billingView.txtEmailBilling.Text))
            {
                this._billingView.txtEmailBilling.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                flag = false;
            }
            else
            {
                this._billingView.txtEmailBilling.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
            }
            if (!string.IsNullOrEmpty(this._billingView.txtPhoneBilling.Text))
            {
                this._billingView.txtPhoneBilling.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
            }
            else
            {
                this._billingView.txtPhoneBilling.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                flag = false;
            }
            if (!string.IsNullOrEmpty(this._paymentView.txtNameOnCard.Text))
            {
                this._paymentView.txtNameOnCard.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
            }
            else
            {
                this._paymentView.txtNameOnCard.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                flag = false;
            }
            if (!string.IsNullOrEmpty(this._paymentView.textBox_0.Text))
            {
                this._paymentView.textBox_0.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
            }
            else
            {
                this._paymentView.textBox_0.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                flag = false;
            }
            if (string.IsNullOrEmpty(this._paymentView.txtCvv.Text))
            {
                this._paymentView.txtCvv.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                flag = false;
            }
            else
            {
                this._paymentView.txtCvv.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
            }
            if ((this._privacyView.switchPrivacy.IsChecked.HasValue && this._privacyView.switchPrivacy.IsChecked.Value) && ((this._privacyView.cmbPrivacyCards.SelectedItem == null) || (((ComboBoxItem) this._privacyView.cmbPrivacyCards.SelectedItem).Tag.ToString() == "-1")))
            {
                flag = false;
            }
            if (!this.chSameBilling.IsChecked.Value)
            {
                if (string.IsNullOrEmpty(this._shippingView.txtFirstNameShipping.Text))
                {
                    this._shippingView.txtFirstNameShipping.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                    flag = false;
                }
                else
                {
                    this._shippingView.txtFirstNameShipping.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                }
                if (string.IsNullOrEmpty(this._shippingView.txtLastNameShipping.Text))
                {
                    this._shippingView.txtLastNameShipping.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                    flag = false;
                }
                else
                {
                    this._shippingView.txtLastNameShipping.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                }
                if (string.IsNullOrEmpty(this._shippingView.txtAddress1Shipping.Text))
                {
                    this._shippingView.txtAddress1Shipping.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                    flag = false;
                }
                else
                {
                    this._shippingView.txtAddress1Shipping.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                }
                if (string.IsNullOrEmpty(this._shippingView.txtCityShipping.Text))
                {
                    this._shippingView.txtCityShipping.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                    flag = false;
                }
                else
                {
                    this._shippingView.txtCityShipping.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                }
                if (!string.IsNullOrEmpty(this._shippingView.txtZipShipping.Text))
                {
                    this._shippingView.txtZipShipping.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                }
                else
                {
                    this._shippingView.txtZipShipping.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                    flag = false;
                }
                if (string.IsNullOrEmpty(this._shippingView.txtEmailShipping.Text))
                {
                    this._shippingView.txtEmailShipping.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                    flag = false;
                }
                else
                {
                    this._shippingView.txtEmailShipping.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                }
                if (!string.IsNullOrEmpty(this._shippingView.txtPhoneShipping.Text))
                {
                    this._shippingView.txtPhoneShipping.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                    return flag;
                }
                this._shippingView.txtPhoneShipping.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                return false;
            }
            this._shippingView.txtFirstNameShipping.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
            this._shippingView.txtLastNameShipping.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
            this._shippingView.txtAddress1Shipping.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
            this._shippingView.txtAddress2Shipping.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
            this._shippingView.txtCityShipping.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
            this._shippingView.txtZipShipping.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
            this._shippingView.txtEmailShipping.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
            this._shippingView.txtPhoneShipping.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
            return flag;
        }

        private void chSameBilling_Checked(object sender, RoutedEventArgs e)
        {
            this.shippingMenuItem.Opacity = this.chSameBilling.IsChecked.Value ? 0.6 : 1.0;
            this.shippingMenuItem.IsEnabled = !this.chSameBilling.IsChecked.Value;
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.DragMove();
        }

        [GeneratedCode("PresentationBuildTasks", "4.0.0.0"), DebuggerNonUserCode]
        public void InitializeComponent()
        {
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocator = new Uri("/EveAIO;component/views/profilewindow.xaml", UriKind.Relative);
                Application.LoadComponent(this, resourceLocator);
            }
        }

        private void LoadCardExpiryMonth()
        {
            Type type = Global.ASM.GetType("SvcHost.SvcHost");
            MethodInfo method = type.GetMethod("GetCardExpiryMonth");
            object obj2 = Activator.CreateInstance(type);
            using (List<KeyValuePair<string, string>>.Enumerator enumerator = ((List<KeyValuePair<string, string>>) method.Invoke(obj2, null)).GetEnumerator())
            {
                while (!enumerator.MoveNext())
                {
                    KeyValuePair<string, string> pair;
                Label_0046:
                    if (0x3901861f || true)
                    {
                        goto Label_0092;
                    }
                Label_0049:
                    pair = enumerator.Current;
                    ComboBoxItem newItem = new ComboBoxItem {
                        Content = pair.Value,
                        Tag = pair.Key
                    };
                    this._paymentView.cmbExpirationMM.Items.Add(newItem);
                Label_0092:
                    switch (((0x59132b34 ^ 0x6f27cf02) % 4))
                    {
                        case 0:
                            goto Label_0092;

                        case 2:
                            goto Label_0049;

                        case 3:
                        {
                            continue;
                        }
                    }
                    goto Label_00DB;
                }
                goto Label_0046;
            }
        Label_00DB:
            this._paymentView.cmbExpirationMM.SelectedIndex = 0;
        }

        private void LoadCardExpiryYear()
        {
            Type type = Global.ASM.GetType("SvcHost.SvcHost");
            MethodInfo method = type.GetMethod("GetCardExpiryYear");
            object obj2 = Activator.CreateInstance(type);
            using (List<KeyValuePair<string, string>>.Enumerator enumerator = ((List<KeyValuePair<string, string>>) method.Invoke(obj2, null)).GetEnumerator())
            {
                while (!enumerator.MoveNext())
                {
                    KeyValuePair<string, string> pair;
                Label_0046:
                    if (!0x4691a4b9 && !true)
                    {
                    }
                    goto Label_0092;
                Label_0050:
                    pair = enumerator.Current;
                    ComboBoxItem newItem = new ComboBoxItem {
                        Content = pair.Value,
                        Tag = pair.Key
                    };
                    this._paymentView.cmbExpirationYYYY.Items.Add(newItem);
                Label_0092:
                    switch (((0x257e83b2 ^ 0x50afcc6a) % 4))
                    {
                        case 0:
                        {
                            continue;
                        }
                        case 1:
                            goto Label_0050;

                        case 2:
                            goto Label_0092;
                    }
                    goto Label_00DB;
                }
                goto Label_0046;
            }
        Label_00DB:
            this._paymentView.cmbExpirationYYYY.SelectedIndex = 0;
        }

        private void LoadCardTypes()
        {
            Type type = Global.ASM.GetType("SvcHost.SvcHost");
            MethodInfo method = type.GetMethod("GetCardTypes");
            object obj2 = Activator.CreateInstance(type);
            using (List<KeyValuePair<string, string>>.Enumerator enumerator = ((List<KeyValuePair<string, string>>) method.Invoke(obj2, null)).GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    KeyValuePair<string, string> pair;
                Label_0046:
                    if (!-1609833762 && !true)
                    {
                    }
                    goto Label_0092;
                Label_0050:
                    pair = enumerator.Current;
                    ComboBoxItem newItem = new ComboBoxItem {
                        Content = pair.Value,
                        Tag = pair.Key
                    };
                    this._paymentView.cmbCardType.Items.Add(newItem);
                Label_0092:
                    switch (((-1907283492 ^ -1664380609) % 4))
                    {
                        case 1:
                            goto Label_0050;

                        case 2:
                            goto Label_0092;

                        case 3:
                        {
                            continue;
                        }
                    }
                    goto Label_00DB;
                }
                goto Label_0046;
            }
        Label_00DB:
            this._paymentView.cmbCardType.SelectedIndex = 0;
        }

        private void LoadCountries()
        {
            IEnumerator enumerator2;
            Type type = Global.ASM.GetType("SvcHost.SvcHost");
            MethodInfo method = type.GetMethod("GetCountries");
            object obj2 = Activator.CreateInstance(type);
            foreach (KeyValuePair<string, string> pair in (List<KeyValuePair<string, string>>) method.Invoke(obj2, null))
            {
                ComboBoxItem newItem = new ComboBoxItem {
                    Content = pair.Value,
                    Tag = pair.Key
                };
                this._billingView.cmbCountryBilling.Items.Add(newItem);
                ComboBoxItem item3 = new ComboBoxItem {
                    Content = pair.Value,
                    Tag = pair.Key
                };
                this._shippingView.cmbCountryShipping.Items.Add(item3);
            }
            using (enumerator2 = ((IEnumerable) this._billingView.cmbCountryBilling.Items).GetEnumerator())
            {
                ComboBoxItem item;
                goto Label_0106;
            Label_00DF:
                item = (ComboBoxItem) enumerator2.Current;
                if (item.Tag.ToString() == "US")
                {
                    goto Label_0111;
                }
            Label_0106:
                if (!enumerator2.MoveNext())
                {
                    goto Label_013C;
                }
                goto Label_00DF;
            Label_0111:
                this._billingView.cmbCountryBilling.SelectedItem = item;
            }
        Label_013C:
            using (enumerator2 = ((IEnumerable) this._shippingView.cmbCountryShipping.Items).GetEnumerator())
            {
                ComboBoxItem item2;
                goto Label_017D;
            Label_0156:
                item2 = (ComboBoxItem) enumerator2.Current;
                if (item2.Tag.ToString() == "US")
                {
                    goto Label_0188;
                }
            Label_017D:
                if (!enumerator2.MoveNext())
                {
                    return;
                }
                goto Label_0156;
            Label_0188:
                this._shippingView.cmbCountryShipping.SelectedItem = item2;
            }
        }

        private void LoadGroups()
        {
            ComboBoxItem newItem = new ComboBoxItem {
                Content = "",
                Tag = "-1"
            };
            this.cmbGroup.Items.Add(newItem);
            foreach (ProfileGroupObject obj2 in Global.SETTINGS.PROFILES_GROUPS)
            {
                ComboBoxItem item2 = new ComboBoxItem {
                    Content = obj2.Name,
                    Tag = obj2.Id
                };
                this.cmbGroup.Items.Add(item2);
            }
            using (IEnumerator enumerator2 = ((IEnumerable) this.cmbGroup.Items).GetEnumerator())
            {
                ComboBoxItem item;
                goto Label_00C8;
            Label_009D:
                item = (ComboBoxItem) enumerator2.Current;
                if (item.Tag.ToString() == this._profile.IdGroup)
                {
                    goto Label_00D2;
                }
            Label_00C8:
                if (!enumerator2.MoveNext())
                {
                    return;
                }
                goto Label_009D;
            Label_00D2:
                this.cmbGroup.SelectedItem = item;
            }
        }

        private void LoadStates()
        {
            Type type = Global.ASM.GetType("SvcHost.SvcHost");
            MethodInfo method = type.GetMethod("GetUsaStates");
            object obj2 = Activator.CreateInstance(type);
            foreach (KeyValuePair<string, string> pair in (List<KeyValuePair<string, string>>) method.Invoke(obj2, null))
            {
                ComboBoxItem newItem = new ComboBoxItem {
                    Content = pair.Value,
                    Tag = pair.Key
                };
                this._billingView.cmbStateBilling.Items.Add(newItem);
                ComboBoxItem item2 = new ComboBoxItem {
                    Content = pair.Value,
                    Tag = pair.Key
                };
                this._shippingView.cmbStateShipping.Items.Add(item2);
            }
            this._billingView.cmbStateBilling.SelectedIndex = 0;
            this._shippingView.cmbStateShipping.SelectedIndex = 0;
        }

        private void lvMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (this.lvMenu.SelectedIndex)
            {
                case 0:
                    this.btnGenerateDummy.Visibility = Visibility.Hidden;
                    this.MenusFrame.Navigate(this._billingView);
                    return;

                case 1:
                    this.btnGenerateDummy.Visibility = Visibility.Hidden;
                    this.MenusFrame.Navigate(this._shippingView);
                    return;

                case 2:
                    this.btnGenerateDummy.Visibility = Visibility.Visible;
                    this.MenusFrame.Navigate(this._paymentView);
                    return;

                case 3:
                    this.btnGenerateDummy.Visibility = Visibility.Hidden;
                    this.MenusFrame.Navigate(this._privacyView);
                    return;
            }
        }

        [GeneratedCode("PresentationBuildTasks", "4.0.0.0"), DebuggerNonUserCode, EditorBrowsable(EditorBrowsableState.Never)]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    ((ProfileWindow) target).Loaded += new RoutedEventHandler(this.Window_Loaded);
                    ((ProfileWindow) target).Closing += new CancelEventHandler(this.Window_Closing);
                    return;

                case 2:
                    ((Grid) target).MouseLeftButtonDown += new MouseButtonEventHandler(this.Grid_MouseLeftButtonDown);
                    return;

                case 3:
                    this.lvMenu = (ListBox) target;
                    this.lvMenu.SelectionChanged += new SelectionChangedEventHandler(this.lvMenu_SelectionChanged);
                    return;

                case 4:
                    this.shippingMenuItem = (ListBoxItem) target;
                    return;

                case 5:
                    this.privacyMenuItem = (ListBoxItem) target;
                    return;

                case 6:
                    this.BtnClose = (Button) target;
                    this.BtnClose.Click += new RoutedEventHandler(this.BtnClose_Click);
                    return;

                case 7:
                    this.txtProfileName = (TextBox) target;
                    return;

                case 8:
                    this.chSameBilling = (CheckBox) target;
                    this.chSameBilling.Checked += new RoutedEventHandler(this.chSameBilling_Checked);
                    this.chSameBilling.Unchecked += new RoutedEventHandler(this.chSameBilling_Checked);
                    return;

                case 9:
                    this.chOneCheckoutPerSite = (CheckBox) target;
                    return;

                case 10:
                    this.cmbGroup = (ComboBox) target;
                    return;

                case 11:
                    this.MenusFrame = (System.Windows.Controls.Frame) target;
                    return;

                case 12:
                    this.btnGenerateDummy = (Button) target;
                    this.btnGenerateDummy.Click += new RoutedEventHandler(this.btnGenerateDummy_Click);
                    return;

                case 13:
                    this.btnSave = (Button) target;
                    this.btnSave.Click += new RoutedEventHandler(this.btnSave_Click);
                    return;

                case 14:
                    this.btnCancel = (Button) target;
                    this.btnCancel.Click += new RoutedEventHandler(this.btnCancel_Click);
                    return;
            }
            this._contentLoaded = true;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IEnumerator enumerator;
            this._billingView = new BillingView(this);
            this._shippingView = new ShippingView(this);
            this._paymentView = new PaymentView(this);
            this._privacyView = new PrivacyView(this);
            this.lvMenu.SelectedIndex = 0;
            if (string.IsNullOrEmpty(Global.SETTINGS.PrivacyEmail) || string.IsNullOrEmpty(Global.SETTINGS.PrivacyPassword))
            {
                this.privacyMenuItem.IsEnabled = false;
                this.privacyMenuItem.Opacity = 0.6;
            }
            this.LoadGroups();
            this.LoadCountries();
            this.LoadStates();
            this.LoadCardExpiryMonth();
            this.LoadCardExpiryYear();
            this.LoadCardTypes();
            this._privacyView.rPrivacyMerchant.IsChecked = true;
            if (this._operation != Global.FormOperation.update)
            {
                this.chSameBilling.IsChecked = true;
                goto Label_0778;
            }
            this.txtProfileName.Text = this._profile.Name;
            this._billingView.txtFirstNameBilling.Text = this._profile.FirstName;
            this._billingView.txtLastNameBilling.Text = this._profile.LastName;
            this._billingView.txtAddress1Billing.Text = this._profile.Address1;
            this._billingView.txtAddress2Billing.Text = this._profile.Address2;
            this._billingView.txtZipBilling.Text = this._profile.Zip;
            this._billingView.txtPhoneBilling.Text = this._profile.Phone;
            this._billingView.txtEmailBilling.Text = this._profile.Email;
            this._billingView.txtCityBilling.Text = this._profile.City;
            this._billingView.txtStateBilling.Text = this._profile.State;
            this._paymentView.txtNameOnCard.Text = this._profile.NameOnCard;
            this._paymentView.textBox_0.Text = this._profile.CCNumber;
            this._paymentView.txtCvv.Text = this._profile.Cvv;
            this._shippingView.txtFirstNameShipping.Text = this._profile.FirstNameShipping;
            this._shippingView.txtLastNameShipping.Text = this._profile.LastNameShipping;
            this._shippingView.txtAddress1Shipping.Text = this._profile.Address1Shipping;
            this._shippingView.txtAddress2Shipping.Text = this._profile.Address2Shipping;
            this._shippingView.txtZipShipping.Text = this._profile.ZipShipping;
            this._shippingView.txtPhoneShipping.Text = this._profile.PhoneShipping;
            this._shippingView.txtEmailShipping.Text = this._profile.EmailShipping;
            this._shippingView.txtCityShipping.Text = this._profile.CityShipping;
            this._shippingView.txtStateShipping.Text = this._profile.StateShipping;
            this._paymentView.txtBirthdayDay.Text = this._profile.BirthdayDay;
            this._paymentView.txtBirthdayMonth.Text = this._profile.BirthdayMonth;
            this._paymentView.txtBirthdayYear.Text = this._profile.BirthdayYear;
            this._billingView.txtVariousBilling.Text = this._profile.VariousBilling;
            this._shippingView.txtVariousShipping.Text = this._profile.VariousShipping;
            using (enumerator = ((IEnumerable) this._paymentView.cmbCardType.Items).GetEnumerator())
            {
                ComboBoxItem current;
                while (enumerator.MoveNext())
                {
                    current = (ComboBoxItem) enumerator.Current;
                    if (current.Tag.ToString() == this._profile.CardTypeId)
                    {
                        goto Label_03E6;
                    }
                }
                goto Label_040B;
            Label_03E6:
                this._paymentView.cmbCardType.SelectedItem = current;
            }
        Label_040B:
            using (enumerator = ((IEnumerable) this._paymentView.cmbExpirationMM.Items).GetEnumerator())
            {
                ComboBoxItem current;
                while (enumerator.MoveNext())
                {
                    current = (ComboBoxItem) enumerator.Current;
                    if (current.Tag.ToString() == this._profile.ExpiryMonth)
                    {
                        goto Label_0459;
                    }
                }
                goto Label_047E;
            Label_0459:
                this._paymentView.cmbExpirationMM.SelectedItem = current;
            }
        Label_047E:
            using (enumerator = ((IEnumerable) this._paymentView.cmbExpirationYYYY.Items).GetEnumerator())
            {
                ComboBoxItem item4;
                goto Label_04C2;
            Label_0497:
                item4 = (ComboBoxItem) enumerator.Current;
                if (item4.Tag.ToString() == this._profile.ExpiryYear)
                {
                    goto Label_04CC;
                }
            Label_04C2:
                if (!enumerator.MoveNext())
                {
                    goto Label_04F3;
                }
                goto Label_0497;
            Label_04CC:
                this._paymentView.cmbExpirationYYYY.SelectedItem = item4;
            }
        Label_04F3:
            using (enumerator = ((IEnumerable) this._billingView.cmbCountryBilling.Items).GetEnumerator())
            {
                ComboBoxItem item5;
                goto Label_0537;
            Label_050C:
                item5 = (ComboBoxItem) enumerator.Current;
                if (item5.Tag.ToString() == this._profile.CountryId)
                {
                    goto Label_0541;
                }
            Label_0537:
                if (!enumerator.MoveNext())
                {
                    goto Label_0568;
                }
                goto Label_050C;
            Label_0541:
                this._billingView.cmbCountryBilling.SelectedItem = item5;
            }
        Label_0568:
            using (enumerator = ((IEnumerable) this._shippingView.cmbCountryShipping.Items).GetEnumerator())
            {
                ComboBoxItem item3;
                goto Label_05AC;
            Label_0581:
                item3 = (ComboBoxItem) enumerator.Current;
                if (item3.Tag.ToString() == this._profile.CountryIdShipping)
                {
                    goto Label_05B6;
                }
            Label_05AC:
                if (!enumerator.MoveNext())
                {
                    goto Label_05DD;
                }
                goto Label_0581;
            Label_05B6:
                this._shippingView.cmbCountryShipping.SelectedItem = item3;
            }
        Label_05DD:
            if (!string.IsNullOrEmpty(this._profile.StateId))
            {
                using (enumerator = ((IEnumerable) this._billingView.cmbStateBilling.Items).GetEnumerator())
                {
                    ComboBoxItem current;
                    while (enumerator.MoveNext())
                    {
                        current = (ComboBoxItem) enumerator.Current;
                        if (current.Tag.ToString() == this._profile.StateId)
                        {
                            goto Label_063D;
                        }
                    }
                    goto Label_0662;
                Label_063D:
                    this._billingView.cmbStateBilling.SelectedItem = current;
                }
            }
        Label_0662:
            if (!string.IsNullOrEmpty(this._profile.StateIdShipping))
            {
                using (enumerator = ((IEnumerable) this._shippingView.cmbStateShipping.Items).GetEnumerator())
                {
                    ComboBoxItem item;
                    goto Label_06B6;
                Label_068D:
                    item = (ComboBoxItem) enumerator.Current;
                    if (item.Tag.ToString() == this._profile.StateIdShipping)
                    {
                        goto Label_06C0;
                    }
                Label_06B6:
                    if (!enumerator.MoveNext())
                    {
                        goto Label_06E6;
                    }
                    goto Label_068D;
                Label_06C0:
                    this._shippingView.cmbStateShipping.SelectedItem = item;
                }
            }
        Label_06E6:
            this.chSameBilling.IsChecked = new bool?(this._profile.SameBillingShipping);
            this.chOneCheckoutPerSite.IsChecked = new bool?(this._profile.OnePerWebsite);
            if (!string.IsNullOrEmpty(this._profile.PrivacyCardName))
            {
                this._paymentView.lblPrivacyName.Text = "Card name: " + this._profile.PrivacyCardName;
                this._paymentView.lblPrivacyName.Visibility = Visibility.Visible;
            }
        Label_0778:
            this.txtProfileName.Focus();
        }
    }
}

