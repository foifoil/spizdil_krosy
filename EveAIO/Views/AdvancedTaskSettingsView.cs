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

    public class AdvancedTaskSettingsView : Page, IComponentConnector
    {
        private TaskWindow2 _parent;
        internal ToggleButton switchAdvancedCaptcha;
        internal TextBox textBox_0;
        internal TextBox txtAntiCaptcha;
        internal TextBox txtImageTypers;
        internal TextBox txtDisolve;
        public ComboBox cmbWatchTask;
        internal CheckBox chLocalIPCheckout;
        internal TextBlock lblMassCreate;
        internal TextBox txtMassCreate;
        internal CheckBox chPaypalProxyIgnore;
        private bool _contentLoaded;

        public AdvancedTaskSettingsView(TaskWindow2 parent)
        {
            Class7.RIuqtBYzWxthF();
            this.InitializeComponent();
            this._parent = parent;
        }

        [DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            // This item is obfuscated and can not be translated.
            Uri uri;
            if (!this._contentLoaded)
            {
                goto Label_002C;
            }
        Label_000D:
            switch (((0x5437f5dc ^ 0x28ab1475) % 4))
            {
                case 0:
                    break;

                case 1:
                    return;

                case 3:
                    goto Label_000D;

                default:
                    Application.LoadComponent(this, uri);
                    return;
            }
        Label_002C:
            this._contentLoaded = true;
            uri = new Uri("/EveAIO;component/views/advancedtasksettingsview.xaml", UriKind.Relative);
            goto Label_000D;
        }

        private void NumberCheck(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        private void switchAdvancedCaptcha_Checked(object sender, RoutedEventArgs e)
        {
            if (this.switchAdvancedCaptcha.IsChecked.HasValue && this.switchAdvancedCaptcha.IsChecked.Value)
            {
                this.textBox_0.Opacity = 1.0;
                this.textBox_0.IsEnabled = true;
                this.txtAntiCaptcha.Opacity = 1.0;
                this.txtAntiCaptcha.IsEnabled = true;
                this.txtImageTypers.Opacity = 1.0;
                this.txtImageTypers.IsEnabled = true;
                this.txtDisolve.Opacity = 1.0;
                this.txtDisolve.IsEnabled = true;
                this._parent._generalView.txtCaptchaRequests.IsEnabled = false;
                this._parent._generalView.txtCaptchaRequests.Opacity = 0.6;
            }
            else
            {
                this.textBox_0.Opacity = 0.6;
                this.textBox_0.IsEnabled = false;
                this.txtDisolve.Opacity = 0.6;
                this.txtDisolve.IsEnabled = false;
                this.txtAntiCaptcha.Opacity = 0.6;
                this.txtAntiCaptcha.IsEnabled = false;
                this.txtImageTypers.Opacity = 0.6;
                this.txtImageTypers.IsEnabled = false;
                this._parent._generalView.txtCaptchaRequests.IsEnabled = true;
                this._parent._generalView.txtCaptchaRequests.Opacity = 1.0;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), GeneratedCode("PresentationBuildTasks", "4.0.0.0"), DebuggerNonUserCode]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    this.switchAdvancedCaptcha = (ToggleButton) target;
                    this.switchAdvancedCaptcha.Checked += new RoutedEventHandler(this.switchAdvancedCaptcha_Checked);
                    this.switchAdvancedCaptcha.Unchecked += new RoutedEventHandler(this.switchAdvancedCaptcha_Checked);
                    return;

                case 2:
                    this.textBox_0 = (TextBox) target;
                    this.textBox_0.PreviewTextInput += new TextCompositionEventHandler(this.NumberCheck);
                    return;

                case 3:
                    this.txtAntiCaptcha = (TextBox) target;
                    this.txtAntiCaptcha.PreviewTextInput += new TextCompositionEventHandler(this.NumberCheck);
                    return;

                case 4:
                    this.txtImageTypers = (TextBox) target;
                    this.txtImageTypers.PreviewTextInput += new TextCompositionEventHandler(this.NumberCheck);
                    return;

                case 5:
                    this.txtDisolve = (TextBox) target;
                    this.txtDisolve.PreviewTextInput += new TextCompositionEventHandler(this.NumberCheck);
                    return;

                case 6:
                    this.cmbWatchTask = (ComboBox) target;
                    return;

                case 7:
                    this.chLocalIPCheckout = (CheckBox) target;
                    return;

                case 8:
                    this.lblMassCreate = (TextBlock) target;
                    return;

                case 9:
                    this.txtMassCreate = (TextBox) target;
                    this.txtMassCreate.PreviewTextInput += new TextCompositionEventHandler(this.NumberCheck);
                    return;

                case 10:
                    this.chPaypalProxyIgnore = (CheckBox) target;
                    return;
            }
            this._contentLoaded = true;
        }
    }
}

