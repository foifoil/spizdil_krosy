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

    public class FootlockerControl : UserControl, IComponentConnector
    {
        private TaskWindow2 _taskWindow;
        internal TextBlock lblLink;
        public TextBox txtLink;
        public CheckBox chBypassCaptcha;
        private bool _contentLoaded;

        public FootlockerControl(TaskWindow2 taskWindow)
        {
            Class7.RIuqtBYzWxthF();
            this.InitializeComponent();
            this._taskWindow = taskWindow;
            this.chBypassCaptcha.IsChecked = true;
        }

        public bool Check()
        {
            if (string.IsNullOrEmpty(this.txtLink.Text))
            {
                this.txtLink.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                return false;
            }
            this.txtLink.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
            return true;
        }

        [GeneratedCode("PresentationBuildTasks", "4.0.0.0"), DebuggerNonUserCode]
        public void InitializeComponent()
        {
            // This item is obfuscated and can not be translated.
            if (!this._contentLoaded)
            {
                goto Label_002C;
            }
        Label_000D:
            switch (((-428785030 ^ -509704637) % 4))
            {
                case 0:
                    goto Label_000D;

                case 1:
                    return;

                case 3:
                    break;

                default:
                {
                    Uri resourceLocator = new Uri("/EveAIO;component/views/platforms/footlockercontrol.xaml", UriKind.Relative);
                    Application.LoadComponent(this, resourceLocator);
                    return;
                }
            }
        Label_002C:
            this._contentLoaded = true;
            goto Label_000D;
        }

        [DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0"), EditorBrowsable(EditorBrowsableState.Never)]
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
                    this.chBypassCaptcha = (CheckBox) target;
                    return;
            }
            this._contentLoaded = true;
        }
    }
}

