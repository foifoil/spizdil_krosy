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

    public class HolypopstoreControl : UserControl, IComponentConnector
    {
        private TaskWindow2 _taskWindow;
        internal TextBlock lblLink;
        public TextBox txtLink;
        public TextBox txtUsername;
        public TextBox txtPassword;
        private bool _contentLoaded;

        public HolypopstoreControl(TaskWindow2 taskWindow)
        {
            Class7.RIuqtBYzWxthF();
            this.InitializeComponent();
            this._taskWindow = taskWindow;
        }

        public bool Check()
        {
            bool flag = true;
            if (string.IsNullOrEmpty(this.txtUsername.Text))
            {
                this.txtUsername.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                flag = false;
            }
            else
            {
                this.txtUsername.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
            }
            if (string.IsNullOrEmpty(this.txtPassword.Text))
            {
                this.txtPassword.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                flag = false;
            }
            else
            {
                this.txtPassword.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
            }
            if (!string.IsNullOrEmpty(this.txtLink.Text))
            {
                this.txtLink.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
                return flag;
            }
            this.txtLink.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
            return false;
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
            switch (((-1457155457 ^ -1838076022) % 4))
            {
                case 0:
                    break;

                case 1:
                    return;

                case 2:
                    goto Label_000D;

                default:
                    Application.LoadComponent(this, uri);
                    return;
            }
        Label_002C:
            this._contentLoaded = true;
            uri = new Uri("/EveAIO;component/views/platforms/holypopstorecontrol.xaml", UriKind.Relative);
            goto Label_000D;
        }

        [EditorBrowsable(EditorBrowsableState.Never), DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
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
                    this.txtUsername = (TextBox) target;
                    return;

                case 4:
                    this.txtPassword = (TextBox) target;
                    return;
            }
            this._contentLoaded = true;
        }
    }
}

