namespace EveAIO.Views
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Markup;

    public class GeneralTaskSettingsView : Page, IComponentConnector
    {
        private TaskWindow2 _parent;
        internal GroupBox gbSizing;
        public RadioButton chSizePositive;
        public RadioButton chSizeNegative;
        public TextBox txtSize;
        public CheckBox chSizeRandom;
        public CheckBox chPickRandomNotAvailable;
        internal GroupBox gbPriceCheck;
        public ToggleButton switchPriceCheck;
        public TextBox txtPriceCheckMin;
        public TextBox txtPriceCheckMax;
        public RadioButton chDelayExact;
        public RadioButton chDelayRandom;
        public TextBox txtDelay;
        public TextBox txtDelayFrom;
        public TextBox txtDelayTo;
        public CheckBox chRetryOnError;
        public TextBox txtCaptchaRequests;
        public CheckBox chNotify;
        public TextBox txtCheckoutDelay;
        private bool _contentLoaded;

        public GeneralTaskSettingsView(TaskWindow2 parent)
        {
            Class7.RIuqtBYzWxthF();
            this.InitializeComponent();
            this._parent = parent;
        }

        private void chDelay_Checked(object sender, RoutedEventArgs e)
        {
            if (this.chDelayExact.IsChecked.HasValue && this.chDelayExact.IsChecked.Value)
            {
                this.txtDelay.IsEnabled = true;
                this.txtDelay.Opacity = 1.0;
                this.txtDelayFrom.IsEnabled = false;
                this.txtDelayFrom.Opacity = 0.6;
                this.txtDelayTo.IsEnabled = false;
                this.txtDelayTo.Opacity = 0.6;
            }
            else
            {
                this.txtDelay.IsEnabled = false;
                this.txtDelay.Opacity = 0.6;
                this.txtDelayFrom.IsEnabled = true;
                this.txtDelayFrom.Opacity = 1.0;
                this.txtDelayTo.IsEnabled = true;
                this.txtDelayTo.Opacity = 1.0;
            }
        }

        private void chSizePositiveNegative_Checked(object sender, RoutedEventArgs e)
        {
            if (this.chSizeNegative.IsChecked.HasValue && this.chSizeNegative.IsChecked.Value)
            {
                this.chPickRandomNotAvailable.IsChecked = false;
                this.chPickRandomNotAvailable.IsEnabled = false;
                this.chPickRandomNotAvailable.Opacity = 0.6;
                this.chSizeRandom.IsChecked = true;
                this.chSizeRandom.IsEnabled = false;
                this.chSizeRandom.Opacity = 0.6;
                this.txtSize.IsEnabled = true;
                this.txtSize.Opacity = 1.0;
            }
            else
            {
                this.chPickRandomNotAvailable.IsEnabled = true;
                this.chPickRandomNotAvailable.Opacity = 1.0;
                this.chSizeRandom.IsEnabled = true;
                this.chSizeRandom.Opacity = 1.0;
            }
        }

        private void chSizeRandom_Checked(object sender, RoutedEventArgs e)
        {
            if (this.chSizeRandom.IsChecked.HasValue && this.chSizeRandom.IsChecked.Value)
            {
                if (!this.chSizeNegative.IsChecked.HasValue || !this.chSizeNegative.IsChecked.Value)
                {
                    this.txtSize.Text = "Random";
                    this.txtSize.IsEnabled = false;
                    this.txtSize.Opacity = 0.6;
                }
                this.chPickRandomNotAvailable.IsEnabled = false;
                this.chPickRandomNotAvailable.Opacity = 0.6;
            }
            else
            {
                this.txtSize.Text = "";
                this.txtSize.IsEnabled = true;
                this.txtSize.Opacity = 1.0;
                this.chPickRandomNotAvailable.IsEnabled = true;
                this.chPickRandomNotAvailable.Opacity = 1.0;
            }
        }

        [GeneratedCode("PresentationBuildTasks", "4.0.0.0"), DebuggerNonUserCode]
        public void InitializeComponent()
        {
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocator = new Uri("/EveAIO;component/views/generaltasksettingsview.xaml", UriKind.Relative);
                Application.LoadComponent(this, resourceLocator);
            }
        }

        private void NumberCheck(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        private void switchPriceCheck_Checked(object sender, RoutedEventArgs e)
        {
            if (this.switchPriceCheck.IsChecked.HasValue && this.switchPriceCheck.IsChecked.Value)
            {
                this.txtPriceCheckMin.Opacity = 1.0;
                this.txtPriceCheckMax.Opacity = 1.0;
                this.txtPriceCheckMin.IsEnabled = true;
                this.txtPriceCheckMax.IsEnabled = true;
            }
            else
            {
                this.txtPriceCheckMin.Opacity = 0.6;
                this.txtPriceCheckMax.Opacity = 0.6;
                this.txtPriceCheckMin.IsEnabled = false;
                this.txtPriceCheckMax.IsEnabled = false;
            }
        }

        [GeneratedCode("PresentationBuildTasks", "4.0.0.0"), DebuggerNonUserCode, EditorBrowsable(EditorBrowsableState.Never)]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    this.gbSizing = (GroupBox) target;
                    return;

                case 2:
                    this.chSizePositive = (RadioButton) target;
                    this.chSizePositive.Checked += new RoutedEventHandler(this.chSizePositiveNegative_Checked);
                    this.chSizePositive.Unchecked += new RoutedEventHandler(this.chSizePositiveNegative_Checked);
                    return;

                case 3:
                    this.chSizeNegative = (RadioButton) target;
                    this.chSizeNegative.Checked += new RoutedEventHandler(this.chSizePositiveNegative_Checked);
                    this.chSizeNegative.Unchecked += new RoutedEventHandler(this.chSizePositiveNegative_Checked);
                    return;

                case 4:
                    this.txtSize = (TextBox) target;
                    return;

                case 5:
                    this.chSizeRandom = (CheckBox) target;
                    this.chSizeRandom.Checked += new RoutedEventHandler(this.chSizeRandom_Checked);
                    this.chSizeRandom.Unchecked += new RoutedEventHandler(this.chSizeRandom_Checked);
                    return;

                case 6:
                    this.chPickRandomNotAvailable = (CheckBox) target;
                    return;

                case 7:
                    this.gbPriceCheck = (GroupBox) target;
                    return;

                case 8:
                    this.switchPriceCheck = (ToggleButton) target;
                    this.switchPriceCheck.Checked += new RoutedEventHandler(this.switchPriceCheck_Checked);
                    this.switchPriceCheck.Unchecked += new RoutedEventHandler(this.switchPriceCheck_Checked);
                    return;

                case 9:
                    this.txtPriceCheckMin = (TextBox) target;
                    this.txtPriceCheckMin.PreviewTextInput += new TextCompositionEventHandler(this.NumberCheck);
                    return;

                case 10:
                    this.txtPriceCheckMax = (TextBox) target;
                    this.txtPriceCheckMax.PreviewTextInput += new TextCompositionEventHandler(this.NumberCheck);
                    return;

                case 11:
                    this.chDelayExact = (RadioButton) target;
                    this.chDelayExact.Checked += new RoutedEventHandler(this.chDelay_Checked);
                    this.chDelayExact.Unchecked += new RoutedEventHandler(this.chDelay_Checked);
                    return;

                case 12:
                    this.chDelayRandom = (RadioButton) target;
                    this.chDelayRandom.Checked += new RoutedEventHandler(this.chDelay_Checked);
                    this.chDelayRandom.Unchecked += new RoutedEventHandler(this.chDelay_Checked);
                    return;

                case 13:
                    this.txtDelay = (TextBox) target;
                    this.txtDelay.PreviewTextInput += new TextCompositionEventHandler(this.NumberCheck);
                    return;

                case 14:
                    this.txtDelayFrom = (TextBox) target;
                    this.txtDelayFrom.PreviewTextInput += new TextCompositionEventHandler(this.NumberCheck);
                    return;

                case 15:
                    this.txtDelayTo = (TextBox) target;
                    this.txtDelayTo.PreviewTextInput += new TextCompositionEventHandler(this.NumberCheck);
                    return;

                case 0x10:
                    this.chRetryOnError = (CheckBox) target;
                    return;

                case 0x11:
                    this.txtCaptchaRequests = (TextBox) target;
                    this.txtCaptchaRequests.PreviewTextInput += new TextCompositionEventHandler(this.NumberCheck);
                    return;

                case 0x12:
                    this.chNotify = (CheckBox) target;
                    return;

                case 0x13:
                    this.txtCheckoutDelay = (TextBox) target;
                    this.txtCheckoutDelay.PreviewTextInput += new TextCompositionEventHandler(this.NumberCheck);
                    return;
            }
            this._contentLoaded = true;
        }
    }
}

