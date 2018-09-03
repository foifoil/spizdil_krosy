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

    public class FootSitesControl2 : UserControl, IComponentConnector
    {
        private TaskWindow2 _taskWindow;
        internal TextBlock lblLink;
        public TextBox txtLink;
        public TextBox txtCartSecurityCheck;
        internal Image imgInfo;
        private bool _contentLoaded;

        public FootSitesControl2(TaskWindow2 taskWindow)
        {
            Class7.RIuqtBYzWxthF();
            this.InitializeComponent();
            this._taskWindow = taskWindow;
            this.imgInfo.ToolTip = "(Mandatory) Security check to ensure the correct product is in cart. \nProvide SKU of the product";
        }

        public bool Check()
        {
            bool flag = true;
            if (string.IsNullOrEmpty(this.txtLink.Text))
            {
                this.txtLink.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                flag = false;
            }
            else
            {
                this.txtLink.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
            }
            if (!string.IsNullOrEmpty(this.txtCartSecurityCheck.Text))
            {
                this.txtCartSecurityCheck.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
                return flag;
            }
            this.txtCartSecurityCheck.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
            return false;
        }

        [GeneratedCode("PresentationBuildTasks", "4.0.0.0"), DebuggerNonUserCode]
        public void InitializeComponent()
        {
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocator = new Uri("/EveAIO;component/views/platforms/footsitescontrol2.xaml", UriKind.Relative);
                Application.LoadComponent(this, resourceLocator);
            }
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
                    this.txtCartSecurityCheck = (TextBox) target;
                    return;

                case 4:
                    this.imgInfo = (Image) target;
                    return;
            }
            this._contentLoaded = true;
        }
    }
}

