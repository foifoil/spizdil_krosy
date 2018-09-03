namespace EveAIO.Views
{
    using EveAIO;
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Markup;

    public class BillingView : Page, IComponentConnector
    {
        private ProfileWindow _parent;
        public TextBox txtFirstNameBilling;
        public TextBox txtLastNameBilling;
        public TextBox txtAddress1Billing;
        public TextBox txtAddress2Billing;
        public TextBox txtCityBilling;
        public TextBox txtStateBilling;
        public ComboBox cmbStateBilling;
        public TextBox txtZipBilling;
        public ComboBox cmbCountryBilling;
        public TextBox txtEmailBilling;
        public TextBox txtPhoneBilling;
        internal TextBlock lblVariousBilling;
        public TextBox txtVariousBilling;
        private bool _contentLoaded;

        public BillingView(ProfileWindow parent)
        {
            Class7.RIuqtBYzWxthF();
            this.InitializeComponent();
            this._parent = parent;
        }

        private void cmbCountryBilling_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.cmbCountryBilling.SelectedItem != null)
            {
                this.cmbStateBilling.Items.Clear();
                this.lblVariousBilling.Visibility = Visibility.Collapsed;
                this.txtVariousBilling.Visibility = Visibility.Collapsed;
                ComboBoxItem selectedItem = (ComboBoxItem) this.cmbCountryBilling.SelectedItem;
                if (selectedItem.Tag.ToString() == "US")
                {
                    this.cmbStateBilling.Visibility = Visibility.Visible;
                    this.txtStateBilling.Visibility = Visibility.Collapsed;
                    Type type = Global.ASM.GetType("SvcHost.SvcHost");
                    MethodInfo method = type.GetMethod("GetUsaStates");
                    object obj6 = Activator.CreateInstance(type);
                    foreach (KeyValuePair<string, string> pair8 in (List<KeyValuePair<string, string>>) method.Invoke(obj6, null))
                    {
                        ComboBoxItem newItem = new ComboBoxItem {
                            Content = pair8.Value,
                            Tag = pair8.Key
                        };
                        this.cmbStateBilling.Items.Add(newItem);
                    }
                    this.cmbStateBilling.SelectedIndex = 0;
                }
                else if (selectedItem.Tag.ToString() == "CA")
                {
                    this.cmbStateBilling.Visibility = Visibility.Visible;
                    this.txtStateBilling.Visibility = Visibility.Collapsed;
                    Type type = Global.ASM.GetType("SvcHost.SvcHost");
                    MethodInfo method = type.GetMethod("GetCanadaStates");
                    object obj7 = Activator.CreateInstance(type);
                    foreach (KeyValuePair<string, string> pair3 in (List<KeyValuePair<string, string>>) method.Invoke(obj7, null))
                    {
                        ComboBoxItem newItem = new ComboBoxItem {
                            Content = pair3.Value,
                            Tag = pair3.Key
                        };
                        this.cmbStateBilling.Items.Add(newItem);
                    }
                    this.cmbStateBilling.SelectedIndex = 0;
                }
                else if (selectedItem.Tag.ToString() == "IT")
                {
                    this.cmbStateBilling.Visibility = Visibility.Visible;
                    this.txtStateBilling.Visibility = Visibility.Collapsed;
                    Type type = Global.ASM.GetType("SvcHost.SvcHost");
                    MethodInfo method = type.GetMethod("GetItalyStates");
                    object obj2 = Activator.CreateInstance(type);
                    foreach (KeyValuePair<string, string> pair2 in (List<KeyValuePair<string, string>>) method.Invoke(obj2, null))
                    {
                        ComboBoxItem newItem = new ComboBoxItem {
                            Content = pair2.Value,
                            Tag = pair2.Key
                        };
                        this.cmbStateBilling.Items.Add(newItem);
                    }
                    this.cmbStateBilling.SelectedIndex = 0;
                    this.lblVariousBilling.Text = "Fiscal code:";
                    this.lblVariousBilling.Visibility = Visibility.Visible;
                    this.txtVariousBilling.Visibility = Visibility.Visible;
                }
                else if (selectedItem.Tag.ToString() == "RU")
                {
                    this.cmbStateBilling.Visibility = Visibility.Visible;
                    this.txtStateBilling.Visibility = Visibility.Collapsed;
                    Type type = Global.ASM.GetType("SvcHost.SvcHost");
                    MethodInfo method = type.GetMethod("GetRussiaStates");
                    object obj10 = Activator.CreateInstance(type);
                    foreach (KeyValuePair<string, string> pair7 in (List<KeyValuePair<string, string>>) method.Invoke(obj10, null))
                    {
                        ComboBoxItem newItem = new ComboBoxItem {
                            Content = pair7.Value,
                            Tag = pair7.Key
                        };
                        this.cmbStateBilling.Items.Add(newItem);
                    }
                    this.cmbStateBilling.SelectedIndex = 0;
                }
                else if (selectedItem.Tag.ToString() == "JP")
                {
                    this.cmbStateBilling.Visibility = Visibility.Visible;
                    this.txtStateBilling.Visibility = Visibility.Collapsed;
                    Type type = Global.ASM.GetType("SvcHost.SvcHost");
                    MethodInfo method = type.GetMethod("GetJapanStates");
                    object obj4 = Activator.CreateInstance(type);
                    foreach (KeyValuePair<string, string> pair6 in (List<KeyValuePair<string, string>>) method.Invoke(obj4, null))
                    {
                        ComboBoxItem newItem = new ComboBoxItem {
                            Content = pair6.Value,
                            Tag = pair6.Key
                        };
                        this.cmbStateBilling.Items.Add(newItem);
                    }
                    this.cmbStateBilling.SelectedIndex = 0;
                }
                else if (selectedItem.Tag.ToString() == "AU")
                {
                    this.cmbStateBilling.Visibility = Visibility.Visible;
                    this.txtStateBilling.Visibility = Visibility.Collapsed;
                    Type type = Global.ASM.GetType("SvcHost.SvcHost");
                    MethodInfo method = type.GetMethod("GetAustraliaStates");
                    object obj5 = Activator.CreateInstance(type);
                    foreach (KeyValuePair<string, string> pair4 in (List<KeyValuePair<string, string>>) method.Invoke(obj5, null))
                    {
                        ComboBoxItem newItem = new ComboBoxItem {
                            Content = pair4.Value,
                            Tag = pair4.Key
                        };
                        this.cmbStateBilling.Items.Add(newItem);
                    }
                    this.cmbStateBilling.SelectedIndex = 0;
                }
                else if (selectedItem.Tag.ToString() == "PT")
                {
                    this.cmbStateBilling.Visibility = Visibility.Visible;
                    this.txtStateBilling.Visibility = Visibility.Collapsed;
                    Type type = Global.ASM.GetType("SvcHost.SvcHost");
                    MethodInfo method = type.GetMethod("GetPortugalStates");
                    object obj8 = Activator.CreateInstance(type);
                    foreach (KeyValuePair<string, string> pair9 in (List<KeyValuePair<string, string>>) method.Invoke(obj8, null))
                    {
                        ComboBoxItem newItem = new ComboBoxItem {
                            Content = pair9.Value,
                            Tag = pair9.Key
                        };
                        this.cmbStateBilling.Items.Add(newItem);
                    }
                    this.cmbStateBilling.SelectedIndex = 0;
                }
                else if (selectedItem.Tag.ToString() == "TH")
                {
                    this.cmbStateBilling.Visibility = Visibility.Visible;
                    this.txtStateBilling.Visibility = Visibility.Collapsed;
                    Type type = Global.ASM.GetType("SvcHost.SvcHost");
                    MethodInfo method = type.GetMethod("GetThailandStates");
                    object obj9 = Activator.CreateInstance(type);
                    foreach (KeyValuePair<string, string> pair5 in (List<KeyValuePair<string, string>>) method.Invoke(obj9, null))
                    {
                        ComboBoxItem newItem = new ComboBoxItem {
                            Content = pair5.Value,
                            Tag = pair5.Key
                        };
                        this.cmbStateBilling.Items.Add(newItem);
                    }
                    this.cmbStateBilling.SelectedIndex = 0;
                }
                else if (selectedItem.Tag.ToString() == "MY")
                {
                    this.cmbStateBilling.Visibility = Visibility.Visible;
                    this.txtStateBilling.Visibility = Visibility.Collapsed;
                    Type type = Global.ASM.GetType("SvcHost.SvcHost");
                    MethodInfo method = type.GetMethod("GetMalaysiaStates");
                    object obj3 = Activator.CreateInstance(type);
                    foreach (KeyValuePair<string, string> pair in (List<KeyValuePair<string, string>>) method.Invoke(obj3, null))
                    {
                        ComboBoxItem newItem = new ComboBoxItem {
                            Content = pair.Value,
                            Tag = pair.Key
                        };
                        this.cmbStateBilling.Items.Add(newItem);
                    }
                    this.cmbStateBilling.SelectedIndex = 0;
                }
                else
                {
                    this.cmbStateBilling.Visibility = Visibility.Collapsed;
                    this.txtStateBilling.Visibility = Visibility.Visible;
                }
            }
        }

        private void cmbStateBilling_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.cmbStateBilling.SelectedItem != null)
            {
                this.txtStateBilling.Text = ((ComboBoxItem) this.cmbStateBilling.SelectedItem).Content.ToString();
            }
        }

        [DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocator = new Uri("/EveAIO;component/views/billingview.xaml", UriKind.Relative);
                Application.LoadComponent(this, resourceLocator);
            }
        }

        [GeneratedCode("PresentationBuildTasks", "4.0.0.0"), DebuggerNonUserCode, EditorBrowsable(EditorBrowsableState.Never)]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    this.txtFirstNameBilling = (TextBox) target;
                    return;

                case 2:
                    this.txtLastNameBilling = (TextBox) target;
                    return;

                case 3:
                    this.txtAddress1Billing = (TextBox) target;
                    return;

                case 4:
                    this.txtAddress2Billing = (TextBox) target;
                    return;

                case 5:
                    this.txtCityBilling = (TextBox) target;
                    return;

                case 6:
                    this.txtStateBilling = (TextBox) target;
                    return;

                case 7:
                    this.cmbStateBilling = (ComboBox) target;
                    this.cmbStateBilling.SelectionChanged += new SelectionChangedEventHandler(this.cmbStateBilling_SelectionChanged);
                    return;

                case 8:
                    this.txtZipBilling = (TextBox) target;
                    return;

                case 9:
                    this.cmbCountryBilling = (ComboBox) target;
                    this.cmbCountryBilling.SelectionChanged += new SelectionChangedEventHandler(this.cmbCountryBilling_SelectionChanged);
                    return;

                case 10:
                    this.txtEmailBilling = (TextBox) target;
                    return;

                case 11:
                    this.txtPhoneBilling = (TextBox) target;
                    this.txtPhoneBilling.PreviewTextInput += new TextCompositionEventHandler(this.txtCvv_PreviewTextInput);
                    return;

                case 12:
                    this.lblVariousBilling = (TextBlock) target;
                    return;

                case 13:
                    this.txtVariousBilling = (TextBox) target;
                    return;
            }
            this._contentLoaded = true;
        }

        private void txtCvv_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }
    }
}

