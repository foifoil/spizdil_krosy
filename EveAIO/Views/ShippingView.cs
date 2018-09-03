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

    public class ShippingView : Page, IComponentConnector
    {
        private ProfileWindow _parent;
        public TextBox txtFirstNameShipping;
        public TextBox txtLastNameShipping;
        public TextBox txtAddress1Shipping;
        public TextBox txtAddress2Shipping;
        public TextBox txtCityShipping;
        public TextBox txtStateShipping;
        public ComboBox cmbStateShipping;
        public TextBox txtZipShipping;
        public ComboBox cmbCountryShipping;
        public TextBox txtEmailShipping;
        public TextBox txtPhoneShipping;
        internal TextBlock lblVariousShipping;
        public TextBox txtVariousShipping;
        private bool _contentLoaded;

        public ShippingView(ProfileWindow parent)
        {
            Class7.RIuqtBYzWxthF();
            this.InitializeComponent();
            this._parent = parent;
        }

        private void cmbCountryShipping_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.cmbCountryShipping.SelectedItem != null)
            {
                this.cmbStateShipping.Items.Clear();
                this.lblVariousShipping.Visibility = Visibility.Collapsed;
                this.txtVariousShipping.Visibility = Visibility.Collapsed;
                ComboBoxItem selectedItem = (ComboBoxItem) this.cmbCountryShipping.SelectedItem;
                if (selectedItem.Tag.ToString() == "US")
                {
                    this.cmbStateShipping.Visibility = Visibility.Visible;
                    this.txtStateShipping.Visibility = Visibility.Collapsed;
                    Type type = Global.ASM.GetType("SvcHost.SvcHost");
                    MethodInfo method = type.GetMethod("GetUsaStates");
                    object obj4 = Activator.CreateInstance(type);
                    foreach (KeyValuePair<string, string> pair7 in (List<KeyValuePair<string, string>>) method.Invoke(obj4, null))
                    {
                        ComboBoxItem newItem = new ComboBoxItem {
                            Content = pair7.Value,
                            Tag = pair7.Key
                        };
                        this.cmbStateShipping.Items.Add(newItem);
                    }
                    this.cmbStateShipping.SelectedIndex = 0;
                }
                else if (selectedItem.Tag.ToString() == "CA")
                {
                    this.cmbStateShipping.Visibility = Visibility.Visible;
                    this.txtStateShipping.Visibility = Visibility.Collapsed;
                    Type type = Global.ASM.GetType("SvcHost.SvcHost");
                    MethodInfo method = type.GetMethod("GetCanadaStates");
                    object obj7 = Activator.CreateInstance(type);
                    foreach (KeyValuePair<string, string> pair9 in (List<KeyValuePair<string, string>>) method.Invoke(obj7, null))
                    {
                        ComboBoxItem newItem = new ComboBoxItem {
                            Content = pair9.Value,
                            Tag = pair9.Key
                        };
                        this.cmbStateShipping.Items.Add(newItem);
                    }
                    this.cmbStateShipping.SelectedIndex = 0;
                }
                else if (selectedItem.Tag.ToString() == "IT")
                {
                    this.cmbStateShipping.Visibility = Visibility.Visible;
                    this.txtStateShipping.Visibility = Visibility.Collapsed;
                    Type type = Global.ASM.GetType("SvcHost.SvcHost");
                    MethodInfo method = type.GetMethod("GetItalyStates");
                    object obj8 = Activator.CreateInstance(type);
                    foreach (KeyValuePair<string, string> pair5 in (List<KeyValuePair<string, string>>) method.Invoke(obj8, null))
                    {
                        ComboBoxItem newItem = new ComboBoxItem {
                            Content = pair5.Value,
                            Tag = pair5.Key
                        };
                        this.cmbStateShipping.Items.Add(newItem);
                    }
                    this.lblVariousShipping.Text = "Fiscal code:";
                    this.lblVariousShipping.Visibility = Visibility.Visible;
                    this.txtVariousShipping.Visibility = Visibility.Visible;
                    this.cmbStateShipping.SelectedIndex = 0;
                }
                else if (selectedItem.Tag.ToString() == "RU")
                {
                    this.cmbStateShipping.Visibility = Visibility.Visible;
                    this.txtStateShipping.Visibility = Visibility.Collapsed;
                    Type type = Global.ASM.GetType("SvcHost.SvcHost");
                    MethodInfo method = type.GetMethod("GetRussiaStates");
                    object obj6 = Activator.CreateInstance(type);
                    foreach (KeyValuePair<string, string> pair4 in (List<KeyValuePair<string, string>>) method.Invoke(obj6, null))
                    {
                        ComboBoxItem newItem = new ComboBoxItem {
                            Content = pair4.Value,
                            Tag = pair4.Key
                        };
                        this.cmbStateShipping.Items.Add(newItem);
                    }
                    this.cmbStateShipping.SelectedIndex = 0;
                }
                else if (selectedItem.Tag.ToString() == "JP")
                {
                    this.cmbStateShipping.Visibility = Visibility.Visible;
                    this.txtStateShipping.Visibility = Visibility.Collapsed;
                    Type type = Global.ASM.GetType("SvcHost.SvcHost");
                    MethodInfo method = type.GetMethod("GetJapanStates");
                    object obj9 = Activator.CreateInstance(type);
                    foreach (KeyValuePair<string, string> pair in (List<KeyValuePair<string, string>>) method.Invoke(obj9, null))
                    {
                        ComboBoxItem newItem = new ComboBoxItem {
                            Content = pair.Value,
                            Tag = pair.Key
                        };
                        this.cmbStateShipping.Items.Add(newItem);
                    }
                    this.cmbStateShipping.SelectedIndex = 0;
                }
                else if (selectedItem.Tag.ToString() == "AU")
                {
                    this.cmbStateShipping.Visibility = Visibility.Visible;
                    this.txtStateShipping.Visibility = Visibility.Collapsed;
                    Type type = Global.ASM.GetType("SvcHost.SvcHost");
                    MethodInfo method = type.GetMethod("GetAustraliaStates");
                    object obj3 = Activator.CreateInstance(type);
                    foreach (KeyValuePair<string, string> pair8 in (List<KeyValuePair<string, string>>) method.Invoke(obj3, null))
                    {
                        ComboBoxItem newItem = new ComboBoxItem {
                            Content = pair8.Value,
                            Tag = pair8.Key
                        };
                        this.cmbStateShipping.Items.Add(newItem);
                    }
                    this.cmbStateShipping.SelectedIndex = 0;
                }
                else if (selectedItem.Tag.ToString() == "PT")
                {
                    this.cmbStateShipping.Visibility = Visibility.Visible;
                    this.txtStateShipping.Visibility = Visibility.Collapsed;
                    Type type = Global.ASM.GetType("SvcHost.SvcHost");
                    MethodInfo method = type.GetMethod("GetPortugalStates");
                    object obj2 = Activator.CreateInstance(type);
                    foreach (KeyValuePair<string, string> pair6 in (List<KeyValuePair<string, string>>) method.Invoke(obj2, null))
                    {
                        ComboBoxItem newItem = new ComboBoxItem {
                            Content = pair6.Value,
                            Tag = pair6.Key
                        };
                        this.cmbStateShipping.Items.Add(newItem);
                    }
                    this.cmbStateShipping.SelectedIndex = 0;
                }
                else if (selectedItem.Tag.ToString() == "TH")
                {
                    this.cmbStateShipping.Visibility = Visibility.Visible;
                    this.txtStateShipping.Visibility = Visibility.Collapsed;
                    Type type = Global.ASM.GetType("SvcHost.SvcHost");
                    MethodInfo method = type.GetMethod("GetThailandStates");
                    object obj5 = Activator.CreateInstance(type);
                    foreach (KeyValuePair<string, string> pair2 in (List<KeyValuePair<string, string>>) method.Invoke(obj5, null))
                    {
                        ComboBoxItem newItem = new ComboBoxItem {
                            Content = pair2.Value,
                            Tag = pair2.Key
                        };
                        this.cmbStateShipping.Items.Add(newItem);
                    }
                    this.cmbStateShipping.SelectedIndex = 0;
                }
                else if (selectedItem.Tag.ToString() == "MY")
                {
                    this.cmbStateShipping.Visibility = Visibility.Visible;
                    this.txtStateShipping.Visibility = Visibility.Collapsed;
                    Type type = Global.ASM.GetType("SvcHost.SvcHost");
                    MethodInfo method = type.GetMethod("GetMalaysiaStates");
                    object obj10 = Activator.CreateInstance(type);
                    foreach (KeyValuePair<string, string> pair3 in (List<KeyValuePair<string, string>>) method.Invoke(obj10, null))
                    {
                        ComboBoxItem newItem = new ComboBoxItem {
                            Content = pair3.Value,
                            Tag = pair3.Key
                        };
                        this.cmbStateShipping.Items.Add(newItem);
                    }
                    this.cmbStateShipping.SelectedIndex = 0;
                }
                else
                {
                    this.cmbStateShipping.Visibility = Visibility.Collapsed;
                    this.txtStateShipping.Visibility = Visibility.Visible;
                }
            }
        }

        private void cmbStateShipping_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.cmbStateShipping.SelectedItem != null)
            {
                this.txtStateShipping.Text = ((ComboBoxItem) this.cmbStateShipping.SelectedItem).Content.ToString();
            }
        }

        [GeneratedCode("PresentationBuildTasks", "4.0.0.0"), DebuggerNonUserCode]
        public void InitializeComponent()
        {
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocator = new Uri("/EveAIO;component/views/shippingview.xaml", UriKind.Relative);
                Application.LoadComponent(this, resourceLocator);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), GeneratedCode("PresentationBuildTasks", "4.0.0.0"), DebuggerNonUserCode]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    this.txtFirstNameShipping = (TextBox) target;
                    return;

                case 2:
                    this.txtLastNameShipping = (TextBox) target;
                    return;

                case 3:
                    this.txtAddress1Shipping = (TextBox) target;
                    return;

                case 4:
                    this.txtAddress2Shipping = (TextBox) target;
                    return;

                case 5:
                    this.txtCityShipping = (TextBox) target;
                    return;

                case 6:
                    this.txtStateShipping = (TextBox) target;
                    return;

                case 7:
                    this.cmbStateShipping = (ComboBox) target;
                    this.cmbStateShipping.SelectionChanged += new SelectionChangedEventHandler(this.cmbStateShipping_SelectionChanged);
                    return;

                case 8:
                    this.txtZipShipping = (TextBox) target;
                    return;

                case 9:
                    this.cmbCountryShipping = (ComboBox) target;
                    this.cmbCountryShipping.SelectionChanged += new SelectionChangedEventHandler(this.cmbCountryShipping_SelectionChanged);
                    return;

                case 10:
                    this.txtEmailShipping = (TextBox) target;
                    return;

                case 11:
                    this.txtPhoneShipping = (TextBox) target;
                    this.txtPhoneShipping.PreviewTextInput += new TextCompositionEventHandler(this.txtCvv_PreviewTextInput);
                    return;

                case 12:
                    this.lblVariousShipping = (TextBlock) target;
                    return;

                case 13:
                    this.txtVariousShipping = (TextBox) target;
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

