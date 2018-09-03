namespace EveAIO.Views
{
    using EveAIO;
    using EveAIO.Pocos;
    using Microsoft.Win32;
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Markup;
    using System.Xml.Serialization;

    public class ProfilesView : Page, IComponentConnector, IStyleConnector
    {
        internal Button BtnNewProfile;
        internal Button BtnEditProfile;
        internal Button BtnDeleteProfile;
        internal Button BtnDuplicateProfile;
        internal Button BtnImportProfile;
        internal Button BtnExportProfile;
        internal Button BtnPrivacyManager;
        internal Button BtnProfileGroups;
        internal Button BtnLogins;
        internal ListBox listProfiles;
        internal MenuItem contextBtnAdd;
        internal MenuItem contextBtnEdit;
        internal MenuItem contextBtnDelete;
        internal MenuItem contextBtnDuplicate;
        internal MenuItem contextBtnSelectAll;
        private bool _contentLoaded;

        public ProfilesView()
        {
            Class7.RIuqtBYzWxthF();
            this.InitializeComponent();
            this.listProfiles.ItemsSource = Global.SETTINGS.PROFILES;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                this.BtnEditProfile_Click(null, null);
            }
        }

        private void BtnDeleteProfile_Click(object sender, RoutedEventArgs e)
        {
            if (this.listProfiles.SelectedItems.Count > 0)
            {
                if (MessageBox.Show("Really delete the selected profiles?", "Are you sure?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    List<string> list = new List<string>();
                    foreach (ProfileObject obj2 in this.listProfiles.SelectedItems)
                    {
                        list.Add(obj2.Id);
                    }
                    foreach (string id in list)
                    {
                        ProfileObject item = Global.SETTINGS.PROFILES.First<ProfileObject>(x => x.Id == id);
                        Global.SETTINGS.PROFILES.Remove(item);
                    }
                }
                Helpers.SaveSettings();
            }
        }

        private void BtnDuplicateProfile_Click(object sender, RoutedEventArgs e)
        {
            if (this.listProfiles.SelectedItems.Count > 0)
            {
                foreach (ProfileObject obj2 in this.listProfiles.SelectedItems)
                {
                    ProfileObject item = new ProfileObject {
                        Id = Guid.NewGuid().ToString(),
                        No = Global.SETTINGS.PROFILES.Count + 1,
                        Name = obj2.Name,
                        SameBillingShipping = obj2.SameBillingShipping,
                        OnePerWebsite = obj2.OnePerWebsite,
                        FirstName = obj2.FirstName,
                        LastName = obj2.LastName,
                        Address1 = obj2.Address1,
                        Address2 = obj2.Address2,
                        City = obj2.City,
                        Zip = obj2.Zip,
                        State = obj2.State,
                        StateId = obj2.StateId,
                        CountryId = obj2.CountryId,
                        Phone = obj2.Phone,
                        Email = obj2.Email,
                        FirstNameShipping = obj2.FirstNameShipping,
                        LastNameShipping = obj2.LastNameShipping,
                        Address1Shipping = obj2.Address1Shipping,
                        Address2Shipping = obj2.Address2Shipping,
                        CityShipping = obj2.CityShipping,
                        ZipShipping = obj2.ZipShipping,
                        EmailShipping = obj2.EmailShipping,
                        PhoneShipping = obj2.PhoneShipping,
                        StateIdShipping = obj2.StateIdShipping,
                        StateShipping = obj2.StateShipping,
                        CountryIdShipping = obj2.CountryIdShipping,
                        NameOnCard = obj2.NameOnCard,
                        CardTypeId = obj2.CardTypeId,
                        CCNumber = obj2.CCNumber,
                        Cvv = obj2.Cvv,
                        ExpiryMonth = obj2.ExpiryMonth,
                        ExpiryYear = obj2.ExpiryYear,
                        Privacy = obj2.Privacy,
                        PrivacyCardName = obj2.PrivacyCardName,
                        BirthdayDay = obj2.BirthdayDay,
                        BirthdayMonth = obj2.BirthdayMonth,
                        BirthdayYear = obj2.BirthdayYear,
                        IdGroup = obj2.IdGroup,
                        VariousBilling = obj2.VariousBilling,
                        VariousShipping = obj2.VariousShipping
                    };
                    Global.SETTINGS.PROFILES.Add(item);
                }
                Helpers.SaveSettings();
            }
        }

        private void BtnEditProfile_Click(object sender, RoutedEventArgs e)
        {
            if (this.listProfiles.SelectedItems.Count == 1)
            {
                ProfileObject profile = (ProfileObject) this.listProfiles.SelectedItems[0];
                new ProfileWindow((Window) Global.MAIN_WINDOW, profile).ShowDialog();
            }
        }

        private void BtnExportProfile_Click(object sender, RoutedEventArgs e)
        {
            if (Global.SETTINGS.PROFILES.Count > 0)
            {
                try
                {
                    SaveFileDialog dialog = new SaveFileDialog {
                        Filter = "*.xml|*.xml"
                    };
                    bool? nullable = dialog.ShowDialog();
                    if (nullable.GetValueOrDefault() ? nullable.HasValue : false)
                    {
                        List<Profile> o = new List<Profile>();
                        foreach (ProfileObject obj2 in Global.SETTINGS.PROFILES)
                        {
                            Profile item = new Profile {
                                BillingAddressLine1 = obj2.Address1,
                                BillingAddressLine2 = obj2.Address2,
                                BillingCity = obj2.City,
                                BillingCountryCode = obj2.CountryId,
                                BillingEmail = obj2.Email,
                                BillingFirstName = obj2.FirstName,
                                BillingLastName = obj2.LastName,
                                BillingPhone = obj2.Phone,
                                BillingState = obj2.StateId,
                                BillingZip = obj2.Zip,
                                CreditCardNumber = obj2.CCNumber,
                                CardType = obj2.CardType,
                                Cvv = obj2.Cvv,
                                ExpirationMonth = obj2.ExpiryMonth,
                                ExpirationYear = obj2.ExpiryYear,
                                NameOnCard = obj2.NameOnCard,
                                OneCheckoutPerWebsite = obj2.OnePerWebsite,
                                ProfileName = obj2.Name,
                                SameBillingShipping = obj2.SameBillingShipping,
                                ShippingAddressLine1 = obj2.Address1Shipping,
                                ShippingAddressLine2 = obj2.Address2Shipping,
                                ShippingCity = obj2.CityShipping,
                                ShippingCountryCode = obj2.CountryIdShipping,
                                ShippingEmail = obj2.EmailShipping,
                                ShippingFirstName = obj2.FirstNameShipping,
                                ShippingLastName = obj2.LastName,
                                ShippingPhone = obj2.PhoneShipping,
                                ShippingState = obj2.StateIdShipping,
                                ShippingZip = obj2.Zip,
                                BirthDay = string.IsNullOrEmpty(obj2.BirthdayDay) ? "1" : obj2.BirthdayDay,
                                BirthMonth = string.IsNullOrEmpty(obj2.BirthdayMonth) ? "1" : obj2.BirthdayMonth,
                                BirthYear = string.IsNullOrEmpty(obj2.BirthdayYear) ? "1980" : obj2.BirthdayYear
                            };
                            o.Add(item);
                        }
                        XmlSerializer serializer = new XmlSerializer(typeof(List<Profile>));
                        using (StreamWriter writer = new StreamWriter(dialog.FileName))
                        {
                            serializer.Serialize((TextWriter) writer, o);
                        }
                        MessageBox.Show("Profiles exported ...", "Information", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    }
                }
                catch (Exception exception)
                {
                    Global.Logger.Error("Error while exporting profiles", exception);
                    MessageBox.Show("Error occured while exporting profiles", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                }
            }
        }

        private void BtnImportProfile_Click(object sender, RoutedEventArgs e)
        {
            new ProfileImportWindow((MainWindow) Global.MAIN_WINDOW).ShowDialog();
        }

        private void BtnLogins_Click(object sender, RoutedEventArgs e)
        {
            new LoginPoolsWindow((MainWindow) Global.MAIN_WINDOW).ShowDialog();
        }

        private void BtnNewProfile_Click(object sender, RoutedEventArgs e)
        {
            new ProfileWindow((Window) Global.MAIN_WINDOW).ShowDialog();
        }

        private void BtnPrivacyManager_Click(object sender, RoutedEventArgs e)
        {
            new PrivacyWindow((Window) Global.MAIN_WINDOW).ShowDialog();
        }

        private void BtnProfileGroups_Click(object sender, RoutedEventArgs e)
        {
            new ProfileGroupWindow((Window) Global.MAIN_WINDOW).ShowDialog();
        }

        private void contextBtnAdd_Click(object sender, RoutedEventArgs e)
        {
            this.BtnNewProfile_Click(null, null);
        }

        private void contextBtnDelete_Click(object sender, RoutedEventArgs e)
        {
            this.BtnDeleteProfile_Click(null, null);
        }

        private void contextBtnDuplicate_Click(object sender, RoutedEventArgs e)
        {
            this.BtnDuplicateProfile_Click(null, null);
        }

        private void contextBtnEdit_Click(object sender, RoutedEventArgs e)
        {
            this.BtnEditProfile_Click(null, null);
        }

        private void contextBtnSelectAll_Click(object sender, RoutedEventArgs e)
        {
            this.listProfiles.SelectAll();
        }

        private void ContextMenu_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            this.contextBtnEdit.IsEnabled = this.listProfiles.SelectedItems.Count == 1;
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
            switch (((-715643311 ^ -1026233925) % 4))
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
            this._contentLoaded = true;
            Uri resourceLocator = new Uri("/EveAIO;component/views/profilesview.xaml", UriKind.Relative);
            Application.LoadComponent(this, resourceLocator);
            goto Label_000D;
        }

        [GeneratedCode("PresentationBuildTasks", "4.0.0.0"), DebuggerNonUserCode, EditorBrowsable(EditorBrowsableState.Never)]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    this.BtnNewProfile = (Button) target;
                    this.BtnNewProfile.Click += new RoutedEventHandler(this.BtnNewProfile_Click);
                    return;

                case 2:
                    this.BtnEditProfile = (Button) target;
                    this.BtnEditProfile.Click += new RoutedEventHandler(this.BtnEditProfile_Click);
                    return;

                case 3:
                    this.BtnDeleteProfile = (Button) target;
                    this.BtnDeleteProfile.Click += new RoutedEventHandler(this.BtnDeleteProfile_Click);
                    return;

                case 4:
                    this.BtnDuplicateProfile = (Button) target;
                    this.BtnDuplicateProfile.Click += new RoutedEventHandler(this.BtnDuplicateProfile_Click);
                    return;

                case 5:
                    this.BtnImportProfile = (Button) target;
                    this.BtnImportProfile.Click += new RoutedEventHandler(this.BtnImportProfile_Click);
                    return;

                case 6:
                    this.BtnExportProfile = (Button) target;
                    this.BtnExportProfile.Click += new RoutedEventHandler(this.BtnExportProfile_Click);
                    return;

                case 7:
                    this.BtnPrivacyManager = (Button) target;
                    this.BtnPrivacyManager.Click += new RoutedEventHandler(this.BtnPrivacyManager_Click);
                    return;

                case 8:
                    this.BtnProfileGroups = (Button) target;
                    this.BtnProfileGroups.Click += new RoutedEventHandler(this.BtnProfileGroups_Click);
                    return;

                case 9:
                    this.BtnLogins = (Button) target;
                    this.BtnLogins.Click += new RoutedEventHandler(this.BtnLogins_Click);
                    return;

                case 10:
                    this.listProfiles = (ListBox) target;
                    this.listProfiles.ContextMenuOpening += new ContextMenuEventHandler(this.ContextMenu_ContextMenuOpening);
                    return;

                case 11:
                    this.contextBtnAdd = (MenuItem) target;
                    this.contextBtnAdd.Click += new RoutedEventHandler(this.contextBtnAdd_Click);
                    return;

                case 12:
                    this.contextBtnEdit = (MenuItem) target;
                    this.contextBtnEdit.Click += new RoutedEventHandler(this.contextBtnEdit_Click);
                    return;

                case 13:
                    this.contextBtnDelete = (MenuItem) target;
                    this.contextBtnDelete.Click += new RoutedEventHandler(this.contextBtnDelete_Click);
                    return;

                case 14:
                    this.contextBtnDuplicate = (MenuItem) target;
                    this.contextBtnDuplicate.Click += new RoutedEventHandler(this.contextBtnDuplicate_Click);
                    return;

                case 15:
                    this.contextBtnSelectAll = (MenuItem) target;
                    this.contextBtnSelectAll.Click += new RoutedEventHandler(this.contextBtnSelectAll_Click);
                    return;
            }
            this._contentLoaded = true;
        }

        [EditorBrowsable(EditorBrowsableState.Never), DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        void IStyleConnector.Connect(int connectionId, object target)
        {
            if (connectionId == 0x10)
            {
                ((Border) target).MouseDown += new MouseButtonEventHandler(this.Border_MouseDown);
            }
        }
    }
}

