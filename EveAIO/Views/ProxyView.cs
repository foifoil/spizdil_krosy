namespace EveAIO.Views
{
    using EveAIO;
    using EveAIO.Pocos;
    using Microsoft.Win32;
    using Newtonsoft.Json;
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Markup;

    public class ProxyView : UserControl, IComponentConnector, IStyleConnector
    {
        internal Button BtnNewList;
        internal Button BtnEditList;
        internal Button BtnDeleteList;
        internal Button BtnDuplicateList;
        internal Button BtnExportList;
        internal Button BtnImportList;
        internal ListBox gvProxyLists;
        internal MenuItem contextBtnAdd;
        internal MenuItem contextBtnEdit;
        internal MenuItem contextBtnDelete;
        internal MenuItem contextBtnDuplicate;
        internal MenuItem contextBtnSelectAll;
        private bool _contentLoaded;

        public ProxyView()
        {
            Class7.RIuqtBYzWxthF();
            this.InitializeComponent();
            this.gvProxyLists.ItemsSource = Global.SETTINGS.PROXIES;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                this.BtnEditList_Click(null, null);
            }
        }

        private void BtnDeleteList_Click(object sender, RoutedEventArgs e)
        {
            if (this.gvProxyLists.SelectedItems.Count > 0)
            {
                if (MessageBox.Show("Really delete the selected proxy lists?", "Are you sure?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    List<string> list = new List<string>();
                    foreach (ProxyListObject obj3 in this.gvProxyLists.SelectedItems)
                    {
                        list.Add(obj3.Id);
                    }
                    foreach (string id in list)
                    {
                        ProxyListObject item = Global.SETTINGS.PROXIES.First<ProxyListObject>(x => x.Id == id);
                        Global.SETTINGS.PROXIES.Remove(item);
                    }
                }
                Helpers.SaveSettings();
            }
        }

        private void BtnDuplicateList_Click(object sender, RoutedEventArgs e)
        {
            if (this.gvProxyLists.SelectedItems.Count > 0)
            {
                foreach (ProxyListObject obj2 in this.gvProxyLists.SelectedItems)
                {
                    ProxyListObject item = new ProxyListObject {
                        Id = Guid.NewGuid().ToString(),
                        Name = obj2.Name,
                        ProxyTestUrl = obj2.ProxyTestUrl,
                        ProxyTimeout = obj2.ProxyTimeout,
                        Rotation = obj2.Rotation
                    };
                    item.Proxies = new ObservableCollection<ProxyObject>();
                    foreach (ProxyObject obj4 in obj2.Proxies)
                    {
                        ProxyObject obj5 = new ProxyObject {
                            Id = Guid.NewGuid().ToString(),
                            IP = obj4.IP,
                            Port = obj4.Port,
                            Username = obj4.Username,
                            Password = obj4.Password,
                            Enabled = obj4.Enabled
                        };
                        item.Proxies.Add(obj5);
                    }
                    Global.SETTINGS.PROXIES.Add(item);
                }
                Helpers.SaveSettings();
            }
        }

        private void BtnEditList_Click(object sender, RoutedEventArgs e)
        {
            if (this.gvProxyLists.SelectedItems.Count == 1)
            {
                ProxyListObject proxyList = (ProxyListObject) this.gvProxyLists.SelectedItems[0];
                new ProxyListWindow((Window) Global.MAIN_WINDOW, proxyList).ShowDialog();
            }
        }

        private void BtnExportList_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog dialog = new SaveFileDialog {
                    Filter = "*.json|*.json"
                };
                bool? nullable = dialog.ShowDialog();
                if (nullable.GetValueOrDefault() ? nullable.HasValue : false)
                {
                    File.WriteAllText(dialog.FileName, Newtonsoft.Json.JsonConvert.SerializeObject(Global.SETTINGS.PROXIES));
                    MessageBox.Show("Exported ...", "Export", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
            }
            catch (Exception exception)
            {
                Global.Logger.Error("Error occured while exporting the proxy lists", exception);
                MessageBox.Show("Error occured while exporting the proxy lists", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }

        private void BtnImportList_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog {
                    Filter = "*.json|*.json"
                };
                bool? nullable = dialog.ShowDialog();
                if (nullable.GetValueOrDefault() ? nullable.HasValue : false)
                {
                    foreach (ProxyListObject obj2 in Newtonsoft.Json.JsonConvert.DeserializeObject<List<ProxyListObject>>(File.ReadAllText(dialog.FileName)))
                    {
                        obj2.Id = Guid.NewGuid().ToString();
                        using (IEnumerator<ProxyObject> enumerator2 = obj2.Proxies.GetEnumerator())
                        {
                            while (enumerator2.MoveNext())
                            {
                                enumerator2.Current.Id = Guid.NewGuid().ToString();
                            }
                        }
                        Global.SETTINGS.PROXIES.Add(obj2);
                    }
                    Helpers.SaveSettings();
                    MessageBox.Show("Imported ...", "Import", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
            }
            catch (Exception exception)
            {
                Global.Logger.Error("Error occured while importing the proxy lists", exception);
                MessageBox.Show("Error occured while importing the proxy lists", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }

        private void BtnNewList_Click(object sender, RoutedEventArgs e)
        {
            new ProxyListWindow((Window) Global.MAIN_WINDOW).ShowDialog();
        }

        private void contextBtnAdd_Click(object sender, RoutedEventArgs e)
        {
            this.BtnNewList_Click(null, null);
        }

        private void contextBtnDelete_Click(object sender, RoutedEventArgs e)
        {
            this.BtnDeleteList_Click(null, null);
        }

        private void contextBtnDuplicate_Click(object sender, RoutedEventArgs e)
        {
            this.BtnDuplicateList_Click(null, null);
        }

        private void contextBtnEdit_Click(object sender, RoutedEventArgs e)
        {
            this.BtnEditList_Click(null, null);
        }

        private void contextBtnSelectAll_Click(object sender, RoutedEventArgs e)
        {
            this.gvProxyLists.SelectAll();
        }

        private void ContextMenu_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            this.contextBtnEdit.IsEnabled = this.gvProxyLists.SelectedItems.Count == 1;
        }

        [DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocator = new Uri("/EveAIO;component/views/proxyview.xaml", UriKind.Relative);
                Application.LoadComponent(this, resourceLocator);
            }
        }

        [DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0"), EditorBrowsable(EditorBrowsableState.Never)]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    this.BtnNewList = (Button) target;
                    this.BtnNewList.Click += new RoutedEventHandler(this.BtnNewList_Click);
                    return;

                case 2:
                    this.BtnEditList = (Button) target;
                    this.BtnEditList.Click += new RoutedEventHandler(this.BtnEditList_Click);
                    return;

                case 3:
                    this.BtnDeleteList = (Button) target;
                    this.BtnDeleteList.Click += new RoutedEventHandler(this.BtnDeleteList_Click);
                    return;

                case 4:
                    this.BtnDuplicateList = (Button) target;
                    this.BtnDuplicateList.Click += new RoutedEventHandler(this.BtnDuplicateList_Click);
                    return;

                case 5:
                    this.BtnExportList = (Button) target;
                    this.BtnExportList.Click += new RoutedEventHandler(this.BtnExportList_Click);
                    return;

                case 6:
                    this.BtnImportList = (Button) target;
                    this.BtnImportList.Click += new RoutedEventHandler(this.BtnImportList_Click);
                    return;

                case 7:
                    this.gvProxyLists = (ListBox) target;
                    this.gvProxyLists.ContextMenuOpening += new ContextMenuEventHandler(this.ContextMenu_ContextMenuOpening);
                    return;

                case 8:
                    this.contextBtnAdd = (MenuItem) target;
                    this.contextBtnAdd.Click += new RoutedEventHandler(this.contextBtnAdd_Click);
                    return;

                case 9:
                    this.contextBtnEdit = (MenuItem) target;
                    this.contextBtnEdit.Click += new RoutedEventHandler(this.contextBtnEdit_Click);
                    return;

                case 10:
                    this.contextBtnDelete = (MenuItem) target;
                    this.contextBtnDelete.Click += new RoutedEventHandler(this.contextBtnDelete_Click);
                    return;

                case 11:
                    this.contextBtnDuplicate = (MenuItem) target;
                    this.contextBtnDuplicate.Click += new RoutedEventHandler(this.contextBtnDuplicate_Click);
                    return;

                case 12:
                    this.contextBtnSelectAll = (MenuItem) target;
                    this.contextBtnSelectAll.Click += new RoutedEventHandler(this.contextBtnSelectAll_Click);
                    return;
            }
            this._contentLoaded = true;
        }

        [EditorBrowsable(EditorBrowsableState.Never), DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        void IStyleConnector.Connect(int connectionId, object target)
        {
            if (connectionId == 13)
            {
                ((Border) target).MouseDown += new MouseButtonEventHandler(this.Border_MouseDown);
            }
        }
    }
}

