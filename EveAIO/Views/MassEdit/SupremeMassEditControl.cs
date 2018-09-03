namespace EveAIO.Views.MassEdit
{
    using EveAIO;
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Markup;
    using System.Windows.Media;

    public class SupremeMassEditControl : UserControl, IComponentConnector
    {
        public ToggleButton switchTaskStype;
        public RadioButton rTypeDirect;
        public RadioButton rTypeKeywords;
        internal TextBlock lblLink;
        public TextBox txtLink;
        internal TextBlock lblGroup;
        public ComboBox cmbGroup;
        internal TextBlock lblColor;
        public TextBox txtColor;
        public RadioButton rSizeExact;
        public RadioButton rSizeContains;
        internal TextBlock lblPositive;
        public TextBox txtPositiveKws;
        internal TextBlock lblNegative;
        public TextBox txtNegativeKws;
        private bool _contentLoaded;

        public SupremeMassEditControl()
        {
            Class7.RIuqtBYzWxthF();
            this.InitializeComponent();
            this.LoadGroups();
        }

        public bool Check() => 
            true;

        private void cmbGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string id = ((ComboBoxItem) this.cmbGroup.SelectedItem).Tag.ToString();
            List<KeyValuePair<string, string>> source = (List<KeyValuePair<string, string>>) Global.ASM.GetType("SvcHost.SvcHost").GetField("SUPREME_LINKS", BindingFlags.Public | BindingFlags.Static).GetValue(null);
            if (this.rTypeKeywords.IsChecked.HasValue && this.rTypeKeywords.IsChecked.Value)
            {
                this.txtLink.Text = source.First<KeyValuePair<string, string>>(x => (x.Key == id)).Value;
            }
        }

        [GeneratedCode("PresentationBuildTasks", "4.0.0.0"), DebuggerNonUserCode]
        public void InitializeComponent()
        {
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocator = new Uri("/EveAIO;component/views/massedit/suprememasseditcontrol.xaml", UriKind.Relative);
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

        private void rType_Checked(object sender, RoutedEventArgs e)
        {
            if (this.rTypeDirect.IsChecked.HasValue && this.rTypeDirect.IsChecked.Value)
            {
                this.lblLink.Text = "Direct link:";
                this.txtLink.Text = "";
                this.txtLink.IsEnabled = true;
                this.txtLink.Opacity = 1.0;
                this.txtPositiveKws.IsEnabled = false;
                this.txtPositiveKws.Opacity = 0.6;
                this.txtPositiveKws.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
                this.txtNegativeKws.IsEnabled = false;
                this.txtNegativeKws.Opacity = 0.6;
                this.txtNegativeKws.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
                this.txtColor.IsEnabled = false;
                this.txtColor.Opacity = 0.6;
                this.txtColor.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
                this.cmbGroup.IsEnabled = false;
                this.cmbGroup.Opacity = 0.6;
                this.cmbGroup.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
                this.txtPositiveKws.Clear();
                this.txtNegativeKws.Clear();
            }
            else if (this.rTypeKeywords.IsChecked.HasValue && this.rTypeKeywords.IsChecked.Value)
            {
                this.cmbGroup_SelectionChanged(null, null);
                this.lblLink.Text = "Search page:";
                this.txtLink.IsEnabled = false;
                this.txtLink.Opacity = 0.6;
                this.txtPositiveKws.IsEnabled = true;
                this.txtPositiveKws.Opacity = 1.0;
                this.txtPositiveKws.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
                this.txtNegativeKws.IsEnabled = true;
                this.txtNegativeKws.Opacity = 1.0;
                this.txtNegativeKws.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
                this.txtColor.IsEnabled = true;
                this.txtColor.Opacity = 1.0;
                this.txtColor.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
                this.cmbGroup.IsEnabled = true;
                this.cmbGroup.Opacity = 1.0;
                this.cmbGroup.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
            }
        }

        private void switchCheckout_Checked(object sender, RoutedEventArgs e)
        {
        }

        private void switchTaskStype_Checked(object sender, RoutedEventArgs e)
        {
            if (this.switchTaskStype.IsChecked.HasValue && this.switchTaskStype.IsChecked.Value)
            {
                this.rTypeDirect.IsEnabled = true;
                this.rTypeDirect.Opacity = 1.0;
                this.rTypeKeywords.IsEnabled = true;
                this.rTypeKeywords.Opacity = 1.0;
                this.lblLink.IsEnabled = true;
                this.lblLink.Opacity = 1.0;
                this.txtLink.IsEnabled = true;
                this.txtLink.Opacity = 1.0;
                this.lblGroup.IsEnabled = true;
                this.lblGroup.Opacity = 1.0;
                this.cmbGroup.IsEnabled = true;
                this.cmbGroup.Opacity = 1.0;
                this.lblColor.IsEnabled = true;
                this.lblColor.Opacity = 1.0;
                this.txtColor.IsEnabled = true;
                this.txtColor.Opacity = 1.0;
                this.rSizeExact.IsEnabled = true;
                this.rSizeExact.Opacity = 1.0;
                this.rSizeExact.IsChecked = true;
                this.rSizeContains.IsEnabled = true;
                this.rSizeContains.Opacity = 1.0;
                this.lblPositive.IsEnabled = true;
                this.lblPositive.Opacity = 1.0;
                this.txtPositiveKws.IsEnabled = true;
                this.txtPositiveKws.Opacity = 1.0;
                this.lblNegative.IsEnabled = true;
                this.lblNegative.Opacity = 1.0;
                this.txtNegativeKws.IsEnabled = true;
                this.txtNegativeKws.Opacity = 1.0;
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
                this.lblGroup.IsEnabled = false;
                this.lblGroup.Opacity = 0.6;
                this.cmbGroup.IsEnabled = false;
                this.cmbGroup.Opacity = 0.6;
                this.lblColor.IsEnabled = false;
                this.lblColor.Opacity = 0.6;
                this.txtColor.IsEnabled = false;
                this.txtColor.Opacity = 0.6;
                this.rSizeExact.IsEnabled = false;
                this.rSizeExact.Opacity = 0.6;
                this.rSizeExact.IsChecked = false;
                this.rSizeContains.IsEnabled = false;
                this.rSizeContains.Opacity = 0.6;
                this.lblPositive.IsEnabled = false;
                this.lblPositive.Opacity = 0.6;
                this.txtPositiveKws.IsEnabled = false;
                this.txtPositiveKws.Opacity = 0.6;
                this.lblNegative.IsEnabled = false;
                this.lblNegative.Opacity = 0.6;
                this.txtNegativeKws.IsEnabled = false;
                this.txtNegativeKws.Opacity = 0.6;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    this.switchTaskStype = (ToggleButton) target;
                    this.switchTaskStype.Checked += new RoutedEventHandler(this.switchTaskStype_Checked);
                    this.switchTaskStype.Unchecked += new RoutedEventHandler(this.switchTaskStype_Checked);
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
                    this.lblGroup = (TextBlock) target;
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
                    this.lblPositive = (TextBlock) target;
                    return;

                case 13:
                    this.txtPositiveKws = (TextBox) target;
                    return;

                case 14:
                    this.lblNegative = (TextBlock) target;
                    return;

                case 15:
                    this.txtNegativeKws = (TextBox) target;
                    return;
            }
            this._contentLoaded = true;
        }
    }
}

