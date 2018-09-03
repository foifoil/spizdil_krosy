namespace EveAIO.Views
{
    using EveAIO;
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Markup;

    public class LoginPoolsWindow : Window, IComponentConnector
    {
        private List<KeyValuePair<LoginPoolEnum, List<string>>> _workingList;
        private bool _isLoaded;
        private bool _isReloading;
        internal Button BtnClose;
        internal TextBlock lblWebsite;
        public ComboBox cmbWebsite;
        public TextBox txtLogins;
        internal Button btnSave;
        internal Button btnCancel;
        private bool _contentLoaded;

        public LoginPoolsWindow(MainWindow owner)
        {
            Class7.RIuqtBYzWxthF();
            this._workingList = new List<KeyValuePair<LoginPoolEnum, List<string>>>();
            base.Owner = owner;
            this.InitializeComponent();
            foreach (LoginPool login in Global.SETTINGS.LOGIN_POOL)
            {
                if (!this._workingList.Any<KeyValuePair<LoginPoolEnum, List<string>>>(x => (((LoginPoolEnum) x.Key) == login.Website)))
                {
                    this._workingList.Add(new KeyValuePair<LoginPoolEnum, List<string>>(login.Website, new List<string>()));
                }
                if (!string.IsNullOrEmpty(login.Link))
                {
                    this._workingList.First<KeyValuePair<LoginPoolEnum, List<string>>>(x => (((LoginPoolEnum) x.Key) == login.Website)).Value.Add(login.Link);
                }
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            base.Close();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            base.Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Global.SETTINGS.LOGIN_POOL.Clear();
            foreach (KeyValuePair<LoginPoolEnum, List<string>> pair in this._workingList)
            {
                using (List<string>.Enumerator enumerator2 = pair.Value.GetEnumerator())
                {
                    while (enumerator2.MoveNext())
                    {
                        string str;
                    Label_0043:
                        if (0x2f4622f5 || true)
                        {
                            goto Label_0082;
                        }
                    Label_0046:
                        str = enumerator2.Current;
                        LoginPool item = new LoginPool {
                            Website = pair.Key,
                            Link = str
                        };
                        Global.SETTINGS.LOGIN_POOL.Add(item);
                    Label_0082:
                        switch (((0x2f4622f5 ^ 0x47d9ed6f) % 4))
                        {
                            case 0:
                                goto Label_0082;

                            case 1:
                            {
                                continue;
                            }
                            case 2:
                                goto Label_0046;
                        }
                    }
                    goto Label_0043;
                }
            }
            Helpers.SaveSettings();
            base.Close();
        }

        private void cmbWebsite_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this._isReloading = true;
            this.txtLogins.Clear();
            string selected = ((ComboBoxItem) this.cmbWebsite.SelectedItem).Tag.ToString();
            if (this._workingList.Any<KeyValuePair<LoginPoolEnum, List<string>>>(x => x.Key.ToString() == selected))
            {
                foreach (string str in this._workingList.First<KeyValuePair<LoginPoolEnum, List<string>>>(x => (x.Key.ToString() == selected)).Value)
                {
                    this.txtLogins.AppendText(str);
                }
                this._isReloading = false;
            }
            else
            {
                this._isReloading = false;
            }
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.DragMove();
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
            switch (((-785317252 ^ -822707001) % 4))
            {
                case 0:
                    goto Label_000D;

                case 2:
                    break;

                case 3:
                    return;

                default:
                {
                    Uri resourceLocator = new Uri("/EveAIO;component/views/loginpoolswindow.xaml", UriKind.Relative);
                    Application.LoadComponent(this, resourceLocator);
                    return;
                }
            }
        Label_002C:
            this._contentLoaded = true;
            goto Label_000D;
        }

        [EditorBrowsable(EditorBrowsableState.Never), DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    ((LoginPoolsWindow) target).Loaded += new RoutedEventHandler(this.Window_Loaded);
                    return;

                case 2:
                    ((Grid) target).MouseLeftButtonDown += new MouseButtonEventHandler(this.Grid_MouseLeftButtonDown);
                    return;

                case 3:
                    this.BtnClose = (Button) target;
                    this.BtnClose.Click += new RoutedEventHandler(this.BtnClose_Click);
                    return;

                case 4:
                    this.lblWebsite = (TextBlock) target;
                    return;

                case 5:
                    this.cmbWebsite = (ComboBox) target;
                    this.cmbWebsite.SelectionChanged += new SelectionChangedEventHandler(this.cmbWebsite_SelectionChanged);
                    return;

                case 6:
                    this.txtLogins = (TextBox) target;
                    this.txtLogins.TextChanged += new TextChangedEventHandler(this.txtLogins_TextChanged);
                    return;

                case 7:
                    this.btnSave = (Button) target;
                    this.btnSave.Click += new RoutedEventHandler(this.btnSave_Click);
                    return;

                case 8:
                    this.btnCancel = (Button) target;
                    this.btnCancel.Click += new RoutedEventHandler(this.btnCancel_Click);
                    return;
            }
            this._contentLoaded = true;
        }

        private void txtLogins_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this._isLoaded && !this._isReloading)
            {
                object selected = ((ComboBoxItem) this.cmbWebsite.SelectedItem).Tag;
                if (!this._workingList.Any<KeyValuePair<LoginPoolEnum, List<string>>>(x => (x.Key.ToString() == selected.ToString())))
                {
                    this._workingList.Add(new KeyValuePair<LoginPoolEnum, List<string>>((LoginPoolEnum) selected, new List<string>()));
                }
                List<string> list = this._workingList.First<KeyValuePair<LoginPoolEnum, List<string>>>(x => (x.Key.ToString() == selected.ToString())).Value;
                list.Clear();
                for (int i = 0; i < this.txtLogins.LineCount; i++)
                {
                    string lineText = this.txtLogins.GetLineText(i);
                    if (!string.IsNullOrEmpty(lineText) && (lineText != "\n"))
                    {
                        list.Add(lineText);
                    }
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (object obj2 in Enum.GetValues(typeof(LoginPoolEnum)))
            {
                ComboBoxItem newItem = new ComboBoxItem {
                    Content = obj2,
                    Tag = obj2
                };
                this.cmbWebsite.Items.Add(newItem);
            }
            this.cmbWebsite.SelectedIndex = 0;
            this._isLoaded = true;
        }
    }
}

