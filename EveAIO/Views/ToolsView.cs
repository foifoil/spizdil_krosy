namespace EveAIO.Views
{
    using EveAIO;
    using EveAIO.Pocos;
    using System;
    using System.CodeDom.Compiler;
    using System.Collections;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Markup;

    public class ToolsView : Page, IComponentConnector
    {
        internal ComboBox cmbProxyList;
        internal Button btnSupremePicker;
        internal ListBox lvKnowledge;
        internal Button btnKnowledge;
        private bool _contentLoaded;

        public ToolsView()
        {
            Class7.RIuqtBYzWxthF();
            this.InitializeComponent();
            base.Loaded += new RoutedEventHandler(this.ToolsView_Loaded);
        }

        private void btnKnowledge_Click(object sender, RoutedEventArgs e)
        {
            if (this.lvKnowledge.SelectedItems.Count == 1)
            {
                Process.Start(((ListBoxItem) this.lvKnowledge.SelectedItem).Tag.ToString());
            }
        }

        private void btnSupremePicker_Click(object sender, RoutedEventArgs e)
        {
            Global.SUPREME_PICKER = new SupremePickerWindow();
            Global.SUPREME_PICKER.Show();
        }

        private void cmbProxyList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Global.SETTINGS.ImagePickerProxyId = ((this.cmbProxyList.SelectedItem == null) || (((ComboBoxItem) this.cmbProxyList.SelectedItem).Tag.ToString() == "-1")) ? "" : ((ComboBoxItem) this.cmbProxyList.SelectedItem).Tag.ToString();
        }

        [GeneratedCode("PresentationBuildTasks", "4.0.0.0"), DebuggerNonUserCode]
        public void InitializeComponent()
        {
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocator = new Uri("/EveAIO;component/views/toolsview.xaml", UriKind.Relative);
                Application.LoadComponent(this, resourceLocator);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), GeneratedCode("PresentationBuildTasks", "4.0.0.0"), DebuggerNonUserCode]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    this.cmbProxyList = (ComboBox) target;
                    this.cmbProxyList.SelectionChanged += new SelectionChangedEventHandler(this.cmbProxyList_SelectionChanged);
                    return;

                case 2:
                    this.btnSupremePicker = (Button) target;
                    this.btnSupremePicker.Click += new RoutedEventHandler(this.btnSupremePicker_Click);
                    return;

                case 3:
                    this.lvKnowledge = (ListBox) target;
                    return;

                case 4:
                    this.btnKnowledge = (Button) target;
                    this.btnKnowledge.Click += new RoutedEventHandler(this.btnKnowledge_Click);
                    return;
            }
            this._contentLoaded = true;
        }

        private void ToolsView_Loaded(object sender, RoutedEventArgs e)
        {
            this.cmbProxyList.Items.Clear();
            ComboBoxItem newItem = new ComboBoxItem {
                Content = "",
                Tag = "-1"
            };
            this.cmbProxyList.Items.Add(newItem);
            foreach (ProxyListObject obj2 in Global.SETTINGS.PROXIES)
            {
                ComboBoxItem item = new ComboBoxItem();
                object[] objArray1 = new object[] { obj2.Name, " (", obj2.ProxiesCount, ")" };
                item.Content = string.Concat(objArray1);
                item.Tag = obj2.Id;
                this.cmbProxyList.Items.Add(item);
            }
            if (!string.IsNullOrEmpty(Global.SETTINGS.ImagePickerProxyId))
            {
                using (IEnumerator enumerator2 = ((IEnumerable) this.cmbProxyList.Items).GetEnumerator())
                {
                    ComboBoxItem item2;
                    goto Label_011B;
                Label_00F0:
                    item2 = (ComboBoxItem) enumerator2.Current;
                    if (Global.SETTINGS.ImagePickerProxyId == item2.Tag.ToString())
                    {
                        goto Label_0126;
                    }
                Label_011B:
                    if (!enumerator2.MoveNext())
                    {
                        return;
                    }
                    goto Label_00F0;
                Label_0126:
                    this.cmbProxyList.SelectedItem = item2;
                }
            }
        }
    }
}

