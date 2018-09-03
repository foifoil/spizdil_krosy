namespace EveAIO.Views
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Markup;

    public class PaymentView : Page, IComponentConnector
    {
        private ProfileWindow _parent;
        public TextBox txtNameOnCard;
        public ComboBox cmbCardType;
        public TextBox textBox_0;
        public ComboBox cmbExpirationMM;
        public ComboBox cmbExpirationYYYY;
        public TextBox txtCvv;
        public TextBlock lblPrivacyName;
        public TextBox txtBirthdayMonth;
        public TextBox txtBirthdayDay;
        public TextBox txtBirthdayYear;
        private bool _contentLoaded;

        public PaymentView(ProfileWindow parent)
        {
            Class7.RIuqtBYzWxthF();
            this.InitializeComponent();
            this._parent = parent;
        }

        private void cmbCardType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!this._parent._privacyView.switchPrivacy.IsChecked.HasValue || !this._parent._privacyView.switchPrivacy.IsChecked.Value)
            {
                this.lblPrivacyName.Text = "";
                this.lblPrivacyName.Visibility = Visibility.Hidden;
            }
        }

        private void cmbExpirationMM_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!this._parent._privacyView.switchPrivacy.IsChecked.HasValue || !this._parent._privacyView.switchPrivacy.IsChecked.Value)
            {
                this.lblPrivacyName.Text = "";
                this.lblPrivacyName.Visibility = Visibility.Hidden;
            }
        }

        private void cmbExpirationYYYY_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!this._parent._privacyView.switchPrivacy.IsChecked.HasValue || !this._parent._privacyView.switchPrivacy.IsChecked.Value)
            {
                this.lblPrivacyName.Text = "";
                this.lblPrivacyName.Visibility = Visibility.Hidden;
            }
        }

        [DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocator = new Uri("/EveAIO;component/views/paymentview.xaml", UriKind.Relative);
                Application.LoadComponent(this, resourceLocator);
            }
        }

        private void NumberCheck(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        [GeneratedCode("PresentationBuildTasks", "4.0.0.0"), DebuggerNonUserCode, EditorBrowsable(EditorBrowsableState.Never)]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    this.txtNameOnCard = (TextBox) target;
                    return;

                case 2:
                    this.cmbCardType = (ComboBox) target;
                    this.cmbCardType.SelectionChanged += new SelectionChangedEventHandler(this.cmbCardType_SelectionChanged);
                    return;

                case 3:
                    this.textBox_0 = (TextBox) target;
                    this.textBox_0.PreviewTextInput += new TextCompositionEventHandler(this.txtCvv_PreviewTextInput);
                    this.textBox_0.TextChanged += new TextChangedEventHandler(this.txtCCNumber_TextChanged);
                    return;

                case 4:
                    this.cmbExpirationMM = (ComboBox) target;
                    this.cmbExpirationMM.SelectionChanged += new SelectionChangedEventHandler(this.cmbExpirationMM_SelectionChanged);
                    return;

                case 5:
                    this.cmbExpirationYYYY = (ComboBox) target;
                    this.cmbExpirationYYYY.SelectionChanged += new SelectionChangedEventHandler(this.cmbExpirationYYYY_SelectionChanged);
                    return;

                case 6:
                    this.txtCvv = (TextBox) target;
                    this.txtCvv.PreviewTextInput += new TextCompositionEventHandler(this.txtCvv_PreviewTextInput);
                    this.txtCvv.TextChanged += new TextChangedEventHandler(this.txtCvv_TextChanged);
                    return;

                case 7:
                    this.lblPrivacyName = (TextBlock) target;
                    return;

                case 8:
                    this.txtBirthdayMonth = (TextBox) target;
                    this.txtBirthdayMonth.PreviewTextInput += new TextCompositionEventHandler(this.NumberCheck);
                    return;

                case 9:
                    this.txtBirthdayDay = (TextBox) target;
                    this.txtBirthdayDay.PreviewTextInput += new TextCompositionEventHandler(this.NumberCheck);
                    return;

                case 10:
                    this.txtBirthdayYear = (TextBox) target;
                    this.txtBirthdayYear.PreviewTextInput += new TextCompositionEventHandler(this.NumberCheck);
                    return;
            }
            this._contentLoaded = true;
        }

        private void txtCCNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!this._parent._privacyView.switchPrivacy.IsChecked.HasValue || !this._parent._privacyView.switchPrivacy.IsChecked.Value)
            {
                this.lblPrivacyName.Text = "";
                this.lblPrivacyName.Visibility = Visibility.Hidden;
            }
        }

        private void txtCvv_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        private void txtCvv_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!this._parent._privacyView.switchPrivacy.IsChecked.HasValue || !this._parent._privacyView.switchPrivacy.IsChecked.Value)
            {
                this.lblPrivacyName.Text = "";
                this.lblPrivacyName.Visibility = Visibility.Hidden;
            }
        }
    }
}

