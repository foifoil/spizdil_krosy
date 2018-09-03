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

    public class FootactionControl : UserControl, IComponentConnector
    {
        private TaskWindow2 _taskWindow;
        internal TextBlock lblLink;
        public TextBox txtLink;
        public CheckBox chBypassCaptcha;
        private bool _contentLoaded;

        public FootactionControl(TaskWindow2 taskWindow)
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

        [DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocator = new Uri("/EveAIO;component/views/platforms/footactioncontrol.xaml", UriKind.Relative);
                Application.LoadComponent(this, resourceLocator);
            }
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

