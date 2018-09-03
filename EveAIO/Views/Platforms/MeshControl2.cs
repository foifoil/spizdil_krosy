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

    public class MeshControl2 : UserControl, IComponentConnector
    {
        private TaskWindow2 _taskWindow;
        public RadioButton rAtcFrontend;
        public RadioButton rAtcBackend;
        public RadioButton rMeshSize;
        public RadioButton rMeshHipStore;
        public RadioButton rMeshJd;
        public RadioButton rMeshFootpatrol;
        internal TextBlock lblSku;
        public TextBox txtLink;
        private bool _contentLoaded;

        public MeshControl2(TaskWindow2 taskWindow)
        {
            Class7.RIuqtBYzWxthF();
            this.InitializeComponent();
            this._taskWindow = taskWindow;
            this.rMeshSize.IsChecked = true;
            this.rAtcBackend.IsChecked = true;
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
            if ((this.rAtcBackend.IsChecked.HasValue && this.rAtcBackend.IsChecked.Value) && (this.rMeshHipStore.IsChecked.HasValue && this.rMeshHipStore.IsChecked.Value))
            {
                MessageBox.Show("The Hipstore now usable only via Frondend method", "Info", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                flag = false;
            }
            return flag;
        }

        [DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocator = new Uri("/EveAIO;component/views/platforms/meshcontrol2.xaml", UriKind.Relative);
                Application.LoadComponent(this, resourceLocator);
            }
        }

        private void rAtc_Checked(object sender, RoutedEventArgs e)
        {
            if (this.rAtcBackend.IsChecked.HasValue && this.rAtcBackend.IsChecked.Value)
            {
                this.txtLink.Width = 150.0;
                this.lblSku.Text = "SKU:";
                this.txtLink.Text = "";
                this.txtLink.MaxLength = 12;
            }
            else
            {
                this.txtLink.Width = 565.0;
                this.lblSku.Text = "LINK:";
                this.txtLink.Text = "";
                this.txtLink.MaxLength = 250;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    this.rAtcFrontend = (RadioButton) target;
                    this.rAtcFrontend.Checked += new RoutedEventHandler(this.rAtc_Checked);
                    this.rAtcFrontend.Unchecked += new RoutedEventHandler(this.rAtc_Checked);
                    return;

                case 2:
                    this.rAtcBackend = (RadioButton) target;
                    this.rAtcBackend.Checked += new RoutedEventHandler(this.rAtc_Checked);
                    this.rAtcBackend.Unchecked += new RoutedEventHandler(this.rAtc_Checked);
                    return;

                case 3:
                    this.rMeshSize = (RadioButton) target;
                    return;

                case 4:
                    this.rMeshHipStore = (RadioButton) target;
                    return;

                case 5:
                    this.rMeshJd = (RadioButton) target;
                    return;

                case 6:
                    this.rMeshFootpatrol = (RadioButton) target;
                    return;

                case 7:
                    this.lblSku = (TextBlock) target;
                    return;

                case 8:
                    this.txtLink = (TextBox) target;
                    return;
            }
            this._contentLoaded = true;
        }
    }
}

