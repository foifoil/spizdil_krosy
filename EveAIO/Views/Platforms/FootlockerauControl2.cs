namespace EveAIO.Views.Platforms
{
    using EveAIO.Views;
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Markup;
    using System.Windows.Media;

    public class FootlockerauControl2 : UserControl, IComponentConnector
    {
        private TaskWindow2 _taskWindow;
        internal TextBlock lblLink;
        public TextBox txtLink;
        internal GroupBox gbPayment;
        public RadioButton radioButton_0;
        public RadioButton rPaymentCCMan;
        private bool _contentLoaded;

        public FootlockerauControl2(TaskWindow2 taskWindow)
        {
            Class7.RIuqtBYzWxthF();
            this.InitializeComponent();
            this._taskWindow = taskWindow;
            this.radioButton_0.IsChecked = true;
        }

        public bool Check()
        {
            if (string.IsNullOrEmpty(this.txtLink.Text))
            {
                this.txtLink.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                return false;
            }
            this.txtLink.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
            return true;
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
            switch (((-565035564 ^ -372243131) % 4))
            {
                case 1:
                    return;

                case 2:
                    break;

                case 3:
                    goto Label_000D;

                default:
                    Application.LoadComponent(this, uri);
                    return;
            }
        Label_002C:
            this._contentLoaded = true;
            uri = new Uri("/EveAIO;component/views/platforms/footlockeraucontrol2.xaml", UriKind.Relative);
            goto Label_000D;
        }

        [DebuggerNonUserCode, EditorBrowsable(EditorBrowsableState.Never), GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    this.lblLink = (TextBlock) target;
                    return;

                case 2:
                    this.txtLink = (TextBox) target;
                    return;

                case 3:
                    this.gbPayment = (GroupBox) target;
                    return;

                case 4:
                    this.radioButton_0 = (RadioButton) target;
                    return;

                case 5:
                    this.rPaymentCCMan = (RadioButton) target;
                    return;
            }
            this._contentLoaded = true;
        }
    }
}

