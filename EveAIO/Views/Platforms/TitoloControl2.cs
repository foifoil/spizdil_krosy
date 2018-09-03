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

    public class TitoloControl2 : UserControl, IComponentConnector
    {
        private TaskWindow2 _taskWindow;
        public TextBox txtLink;
        public TextBox txtUsername;
        public TextBox txtPassword;
        private bool _contentLoaded;

        public TitoloControl2(TaskWindow2 taskWindow)
        {
            Class7.RIuqtBYzWxthF();
            this.InitializeComponent();
            this._taskWindow = taskWindow;
        }

        public bool Check()
        {
            bool flag = true;
            if (!string.IsNullOrEmpty(this.txtUsername.Text))
            {
                this.txtUsername.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
            }
            else
            {
                this.txtUsername.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                flag = false;
            }
            if (!string.IsNullOrEmpty(this.txtPassword.Text))
            {
                this.txtPassword.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
            }
            else
            {
                this.txtPassword.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                flag = false;
            }
            if (!string.IsNullOrEmpty(this.txtLink.Text))
            {
                this.txtLink.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                return flag;
            }
            this.txtLink.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
            return false;
        }

        [DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocator = new Uri("/EveAIO;component/views/platforms/titolocontrol2.xaml", UriKind.Relative);
                Application.LoadComponent(this, resourceLocator);
            }
        }

        [DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0"), EditorBrowsable(EditorBrowsableState.Never)]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    this.txtLink = (TextBox) target;
                    return;

                case 2:
                    this.txtUsername = (TextBox) target;
                    return;

                case 3:
                    this.txtPassword = (TextBox) target;
                    return;
            }
            this._contentLoaded = true;
        }
    }
}

