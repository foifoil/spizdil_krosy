namespace EveAIO.Views
{
    using EveAIO;
    using EveAIO.Privacy;
    using System;
    using System.CodeDom.Compiler;
    using System.Collections;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Markup;
    using System.Windows.Media;

    public class PrivacyView : Page, IComponentConnector
    {
        private ProfileWindow _parent;
        public ToggleButton switchPrivacy;
        public ProgressBar progBarPrivacy;
        public TextBlock lblLoadingText;
        internal TextBlock lblPrivacyCards;
        internal ComboBox cmbPrivacyCards;
        internal Button btnPrivacyReload;
        public GroupBox brNewPrivacyCard;
        public TextBox txtPrivacyNickName;
        public TextBox txtPrivacyLimit;
        public RadioButton rPrivacyMerchant;
        public RadioButton rPrivacyBurner;
        public Button btnPrivacyAddCard;
        private bool _contentLoaded;

        public PrivacyView(ProfileWindow parent)
        {
            Class7.RIuqtBYzWxthF();
            this.InitializeComponent();
            this._parent = parent;
        }

        private void btnPrivacyAddCard_Click(object sender, RoutedEventArgs e)
        {
            if (!this.PrivacyCheck())
            {
                return;
            }
            string str = Global.PRIVACY_MANAGER.CreateCard(this.txtPrivacyNickName.Text.Trim(), (!this.rPrivacyBurner.IsChecked.HasValue || !this.rPrivacyBurner.IsChecked.Value) ? PrivacyManager.PrivacyCardType.merchant : PrivacyManager.PrivacyCardType.burner, string.IsNullOrEmpty(this.txtPrivacyLimit.Text.Trim()) ? null : new int?(int.Parse(this.txtPrivacyLimit.Text.Trim())));
            if (string.IsNullOrEmpty(str))
            {
                return;
            }
            this.btnPrivacyReload_Click(null, null);
            using (IEnumerator enumerator = ((IEnumerable) this.cmbPrivacyCards.Items).GetEnumerator())
            {
                ComboBoxItem current;
                while (enumerator.MoveNext())
                {
                    current = (ComboBoxItem) enumerator.Current;
                    if (current.Tag.ToString() == str)
                    {
                        goto Label_00E3;
                    }
                }
                goto Label_0105;
            Label_00E3:
                this.cmbPrivacyCards.SelectedItem = current;
            }
        Label_0105:
            this.txtPrivacyLimit.Text = "";
            this.txtPrivacyNickName.Text = "";
        }

        private void btnPrivacyReload_Click(object sender, RoutedEventArgs e)
        {
            this.cmbPrivacyCards.Items.Clear();
            ComboBoxItem newItem = new ComboBoxItem {
                Content = "",
                Tag = "-1"
            };
            this.cmbPrivacyCards.Items.Add(newItem);
            foreach (PrivacyCard card in Global.PRIVACY_MANAGER.Cards)
            {
                ComboBoxItem item2 = new ComboBoxItem {
                    Content = card.Name,
                    Tag = card.CardId
                };
                this.cmbPrivacyCards.Items.Add(item2);
            }
        }

        private void cmbPrivacyCards_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IEnumerator enumerator;
            if ((this.cmbPrivacyCards.SelectedItem == null) || (((ComboBoxItem) this.cmbPrivacyCards.SelectedItem).Tag.ToString() == "-1"))
            {
                this._parent._paymentView.lblPrivacyName.Text = "";
                this._parent._paymentView.lblPrivacyName.Visibility = Visibility.Hidden;
                return;
            }
            PrivacyCard card = Global.PRIVACY_MANAGER.Cards.First<PrivacyCard>(x => x.CardId == ((ComboBoxItem) this.cmbPrivacyCards.SelectedItem).Tag.ToString());
            this._parent._paymentView.txtCvv.Text = card.Cvv;
            this._parent._paymentView.textBox_0.Text = card.CardNumber;
            using (enumerator = ((IEnumerable) this._parent._paymentView.cmbExpirationMM.Items).GetEnumerator())
            {
                ComboBoxItem item;
                goto Label_00CE;
            Label_00AA:
                item = (ComboBoxItem) enumerator.Current;
                if (item.Tag.ToString() == card.ExpMonth)
                {
                    goto Label_00D8;
                }
            Label_00CE:
                if (!enumerator.MoveNext())
                {
                    goto Label_0106;
                }
                goto Label_00AA;
            Label_00D8:
                this._parent._paymentView.cmbExpirationMM.SelectedItem = item;
            }
        Label_0106:
            using (enumerator = ((IEnumerable) this._parent._paymentView.cmbExpirationYYYY.Items).GetEnumerator())
            {
                ComboBoxItem item3;
                goto Label_014A;
            Label_0124:
                item3 = (ComboBoxItem) enumerator.Current;
                if (item3.Tag.ToString() == card.ExpYear)
                {
                    goto Label_0154;
                }
            Label_014A:
                if (!enumerator.MoveNext())
                {
                    goto Label_0183;
                }
                goto Label_0124;
            Label_0154:
                this._parent._paymentView.cmbExpirationYYYY.SelectedItem = item3;
            }
        Label_0183:
            using (enumerator = ((IEnumerable) this._parent._paymentView.cmbCardType.Items).GetEnumerator())
            {
                ComboBoxItem item2;
                goto Label_01C7;
            Label_01A1:
                item2 = (ComboBoxItem) enumerator.Current;
                if (item2.Tag.ToString() == "1")
                {
                    goto Label_01D1;
                }
            Label_01C7:
                if (!enumerator.MoveNext())
                {
                    goto Label_0200;
                }
                goto Label_01A1;
            Label_01D1:
                this._parent._paymentView.cmbCardType.SelectedItem = item2;
            }
        Label_0200:
            this._parent._paymentView.lblPrivacyName.Text = "Card name: " + card.Name;
            this._parent._paymentView.lblPrivacyName.Visibility = Visibility.Visible;
        }

        [DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocator = new Uri("/EveAIO;component/views/privacyview.xaml", UriKind.Relative);
                Application.LoadComponent(this, resourceLocator);
            }
        }

        private bool PrivacyCheck()
        {
            bool flag = true;
            if (string.IsNullOrEmpty(this.txtPrivacyNickName.Text))
            {
                this.txtPrivacyNickName.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                flag = false;
            }
            else
            {
                this.txtPrivacyNickName.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
            }
            if (this.rPrivacyBurner.IsChecked.HasValue && this.rPrivacyBurner.IsChecked.Value)
            {
                if (!string.IsNullOrEmpty(this.txtPrivacyLimit.Text))
                {
                    this.txtPrivacyLimit.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
                    return flag;
                }
                this.txtPrivacyLimit.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                return false;
            }
            this.txtPrivacyLimit.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
            return flag;
        }

        private void switchPrivacy_Checked(object sender, RoutedEventArgs e)
        {
            if (this.switchPrivacy.IsChecked.HasValue && this.switchPrivacy.IsChecked.Value)
            {
                bool cont = false;
                if ((Global.PRIVACY_MANAGER == null) || !Global.PRIVACY_MANAGER.IsLoggedIn)
                {
                    TaskScheduler current;
                    if (SynchronizationContext.Current != null)
                    {
                        current = TaskScheduler.FromCurrentSynchronizationContext();
                    }
                    else
                    {
                        current = TaskScheduler.Current;
                    }
                    this.switchPrivacy.IsEnabled = false;
                    this.lblLoadingText.Visibility = Visibility.Visible;
                    this.progBarPrivacy.Visibility = Visibility.Visible;
                    this.progBarPrivacy.IsEnabled = true;
                    Task.Factory.StartNew(delegate {
                        Global.PRIVACY_MANAGER = new PrivacyManager();
                        Global.PRIVACY_MANAGER.Login();
                        if (Global.PRIVACY_MANAGER.IsLoggedIn)
                        {
                            cont = true;
                            Global.PRIVACY_MANAGER.LoadCards();
                        }
                    }).ContinueWith(delegate (Task t) {
                        this.switchPrivacy.IsEnabled = true;
                        this.lblLoadingText.Visibility = Visibility.Hidden;
                        this.progBarPrivacy.Visibility = Visibility.Hidden;
                        this.progBarPrivacy.IsEnabled = false;
                        if (cont)
                        {
                            this.cmbPrivacyCards.IsEnabled = true;
                            this.cmbPrivacyCards.Opacity = 1.0;
                            this.lblPrivacyCards.IsEnabled = true;
                            this.lblPrivacyCards.Opacity = 1.0;
                            this.btnPrivacyReload.IsEnabled = true;
                            this.btnPrivacyReload.Opacity = 1.0;
                            this.brNewPrivacyCard.IsEnabled = true;
                            this.brNewPrivacyCard.Opacity = 1.0;
                            this._parent._paymentView.textBox_0.IsEnabled = false;
                            this._parent._paymentView.textBox_0.Opacity = 0.6;
                            this._parent._paymentView.cmbCardType.IsEnabled = false;
                            this._parent._paymentView.cmbCardType.Opacity = 0.6;
                            this._parent._paymentView.cmbExpirationMM.IsEnabled = false;
                            this._parent._paymentView.cmbExpirationMM.Opacity = 0.6;
                            this._parent._paymentView.cmbExpirationYYYY.IsEnabled = false;
                            this._parent._paymentView.cmbExpirationYYYY.Opacity = 0.6;
                            this._parent._paymentView.txtCvv.IsEnabled = false;
                            this._parent._paymentView.txtCvv.Opacity = 0.6;
                            this.cmbPrivacyCards.Items.Clear();
                            ComboBoxItem newItem = new ComboBoxItem {
                                Content = "",
                                Tag = "-1"
                            };
                            this.cmbPrivacyCards.Items.Add(newItem);
                            foreach (PrivacyCard card in Global.PRIVACY_MANAGER.Cards)
                            {
                                ComboBoxItem item2 = new ComboBoxItem {
                                    Content = card.Name,
                                    Tag = card.CardId
                                };
                                this.cmbPrivacyCards.Items.Add(item2);
                            }
                        }
                        else
                        {
                            this.switchPrivacy.IsChecked = false;
                        }
                    }, current);
                }
                else
                {
                    this.cmbPrivacyCards.IsEnabled = true;
                    this.cmbPrivacyCards.Opacity = 1.0;
                    this.lblPrivacyCards.IsEnabled = true;
                    this.lblPrivacyCards.Opacity = 1.0;
                    this.btnPrivacyReload.IsEnabled = true;
                    this.btnPrivacyReload.Opacity = 1.0;
                    this.brNewPrivacyCard.IsEnabled = true;
                    this.brNewPrivacyCard.Opacity = 1.0;
                    this._parent._paymentView.textBox_0.IsEnabled = false;
                    this._parent._paymentView.textBox_0.Opacity = 0.6;
                    this._parent._paymentView.cmbCardType.IsEnabled = false;
                    this._parent._paymentView.cmbCardType.Opacity = 0.6;
                    this._parent._paymentView.cmbExpirationMM.IsEnabled = false;
                    this._parent._paymentView.cmbExpirationMM.Opacity = 0.6;
                    this._parent._paymentView.cmbExpirationYYYY.IsEnabled = false;
                    this._parent._paymentView.cmbExpirationYYYY.Opacity = 0.6;
                    this._parent._paymentView.txtCvv.IsEnabled = false;
                    this._parent._paymentView.txtCvv.Opacity = 0.6;
                    this.cmbPrivacyCards.Items.Clear();
                    ComboBoxItem newItem = new ComboBoxItem {
                        Content = "",
                        Tag = "-1"
                    };
                    this.cmbPrivacyCards.Items.Add(newItem);
                    foreach (PrivacyCard card in Global.PRIVACY_MANAGER.Cards)
                    {
                        ComboBoxItem item2 = new ComboBoxItem {
                            Content = card.Name,
                            Tag = card.CardId
                        };
                        this.cmbPrivacyCards.Items.Add(item2);
                    }
                }
            }
            else
            {
                this.cmbPrivacyCards.IsEnabled = false;
                this.cmbPrivacyCards.Opacity = 0.6;
                this.lblPrivacyCards.IsEnabled = false;
                this.lblPrivacyCards.Opacity = 0.6;
                this.btnPrivacyReload.IsEnabled = false;
                this.btnPrivacyReload.Opacity = 0.6;
                this.brNewPrivacyCard.IsEnabled = false;
                this.brNewPrivacyCard.Opacity = 0.6;
                this._parent._paymentView.textBox_0.IsEnabled = true;
                this._parent._paymentView.textBox_0.Opacity = 1.0;
                this._parent._paymentView.cmbCardType.IsEnabled = true;
                this._parent._paymentView.cmbCardType.Opacity = 1.0;
                this._parent._paymentView.cmbExpirationMM.IsEnabled = true;
                this._parent._paymentView.cmbExpirationMM.Opacity = 1.0;
                this._parent._paymentView.cmbExpirationYYYY.IsEnabled = true;
                this._parent._paymentView.cmbExpirationYYYY.Opacity = 1.0;
                this._parent._paymentView.txtCvv.IsEnabled = true;
                this._parent._paymentView.txtCvv.Opacity = 1.0;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), GeneratedCode("PresentationBuildTasks", "4.0.0.0"), DebuggerNonUserCode]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    this.switchPrivacy = (ToggleButton) target;
                    this.switchPrivacy.Checked += new RoutedEventHandler(this.switchPrivacy_Checked);
                    this.switchPrivacy.Unchecked += new RoutedEventHandler(this.switchPrivacy_Checked);
                    return;

                case 2:
                    this.progBarPrivacy = (ProgressBar) target;
                    return;

                case 3:
                    this.lblLoadingText = (TextBlock) target;
                    return;

                case 4:
                    this.lblPrivacyCards = (TextBlock) target;
                    return;

                case 5:
                    this.cmbPrivacyCards = (ComboBox) target;
                    this.cmbPrivacyCards.SelectionChanged += new SelectionChangedEventHandler(this.cmbPrivacyCards_SelectionChanged);
                    return;

                case 6:
                    this.btnPrivacyReload = (Button) target;
                    this.btnPrivacyReload.Click += new RoutedEventHandler(this.btnPrivacyReload_Click);
                    return;

                case 7:
                    this.brNewPrivacyCard = (GroupBox) target;
                    return;

                case 8:
                    this.txtPrivacyNickName = (TextBox) target;
                    return;

                case 9:
                    this.txtPrivacyLimit = (TextBox) target;
                    this.txtPrivacyLimit.PreviewTextInput += new TextCompositionEventHandler(this.txtCvv_PreviewTextInput);
                    return;

                case 10:
                    this.rPrivacyMerchant = (RadioButton) target;
                    return;

                case 11:
                    this.rPrivacyBurner = (RadioButton) target;
                    return;

                case 12:
                    this.btnPrivacyAddCard = (Button) target;
                    this.btnPrivacyAddCard.Click += new RoutedEventHandler(this.btnPrivacyAddCard_Click);
                    return;
            }
            this._contentLoaded = true;
        }

        private void txtCvv_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        private void txtCvv_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!this._parent._privacyView.switchPrivacy.IsChecked.HasValue || !this._parent._privacyView.switchPrivacy.IsChecked.Value)
            {
                this._parent._paymentView.lblPrivacyName.Text = "";
                this._parent._paymentView.Visibility = Visibility.Hidden;
            }
        }
    }
}

