namespace EveAIO.Views.MassEdit
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Markup;
    using System.Windows.Media;

    public class HibbettMassEditControl : UserControl, IComponentConnector
    {
        public ToggleButton switchDirectLink;
        public TextBox txtLink;
        private bool _contentLoaded;

        public HibbettMassEditControl()
        {
            Class7.RIuqtBYzWxthF();
            this.InitializeComponent();
        }

        public bool Check()
        {
            if (this.switchDirectLink.IsChecked.HasValue && this.switchDirectLink.IsChecked.Value)
            {
                if (string.IsNullOrEmpty(this.txtLink.Text))
                {
                    this.txtLink.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                    return false;
                }
                this.txtLink.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
            }
            return true;
        }

        [GeneratedCode("PresentationBuildTasks", "4.0.0.0"), DebuggerNonUserCode]
        public void InitializeComponent()
        {
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocator = new Uri("/EveAIO;component/views/massedit/hibbettmasseditcontrol.xaml", UriKind.Relative);
                Application.LoadComponent(this, resourceLocator);
            }
        }

        private void switchDirectLink_Checked(object sender, RoutedEventArgs e)
        {
            if (this.switchDirectLink.IsChecked.HasValue && this.switchDirectLink.IsChecked.Value)
            {
                this.txtLink.IsEnabled = true;
                this.txtLink.Opacity = 1.0;
            }
            else
            {
                this.txtLink.IsEnabled = false;
                this.txtLink.Opacity = 0.6;
            }
        }

        [GeneratedCode("PresentationBuildTasks", "4.0.0.0"), EditorBrowsable(EditorBrowsableState.Never), DebuggerNonUserCode]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            if (connectionId != 1)
            {
                if (connectionId != 2)
                {
                    this._contentLoaded = true;
                }
                else
                {
                    this.txtLink = (TextBox) target;
                }
            }
            else
            {
                this.switchDirectLink = (ToggleButton) target;
                this.switchDirectLink.Checked += new RoutedEventHandler(this.switchDirectLink_Checked);
                this.switchDirectLink.Unchecked += new RoutedEventHandler(this.switchDirectLink_Checked);
            }
        }
    }
}

