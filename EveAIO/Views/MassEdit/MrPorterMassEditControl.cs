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

    public class MrPorterMassEditControl : UserControl, IComponentConnector
    {
        public ToggleButton switchDirectLink;
        public TextBox txtLink;
        public RadioButton rRegionUsa;
        public RadioButton rRegionIntl;
        private bool _contentLoaded;

        public MrPorterMassEditControl()
        {
            Class7.RIuqtBYzWxthF();
            this.InitializeComponent();
            this.rRegionUsa.IsChecked = true;
        }

        public bool Check()
        {
            bool flag = true;
            if (!this.switchDirectLink.IsChecked.HasValue || !this.switchDirectLink.IsChecked.Value)
            {
                return flag;
            }
            if (!string.IsNullOrEmpty(this.txtLink.Text))
            {
                this.txtLink.Background = (Brush) Application.Current.MainWindow.FindResource("ButtonBackground");
                return flag;
            }
            this.txtLink.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
            return false;
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
            switch (((-726943025 ^ -1214245334) % 4))
            {
                case 0:
                    goto Label_000D;

                case 1:
                    return;

                case 2:
                    return;

                case 3:
                    break;

                default:
                    return;
            }
        Label_002C:
            this._contentLoaded = true;
            Uri resourceLocator = new Uri("/EveAIO;component/views/massedit/mrportermasseditcontrol.xaml", UriKind.Relative);
            Application.LoadComponent(this, resourceLocator);
            goto Label_000D;
        }

        private void switchDirectLink_Checked(object sender, RoutedEventArgs e)
        {
            if (this.switchDirectLink.IsChecked.HasValue && this.switchDirectLink.IsChecked.Value)
            {
                this.txtLink.IsEnabled = true;
                this.txtLink.Opacity = 1.0;
                this.rRegionUsa.IsEnabled = true;
                this.rRegionUsa.Opacity = 1.0;
                this.rRegionIntl.IsEnabled = true;
                this.rRegionIntl.Opacity = 1.0;
            }
            else
            {
                this.txtLink.IsEnabled = false;
                this.txtLink.Opacity = 0.6;
                this.rRegionUsa.IsEnabled = false;
                this.rRegionUsa.Opacity = 0.6;
                this.rRegionIntl.IsEnabled = false;
                this.rRegionIntl.Opacity = 0.6;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), GeneratedCode("PresentationBuildTasks", "4.0.0.0"), DebuggerNonUserCode]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    this.switchDirectLink = (ToggleButton) target;
                    this.switchDirectLink.Checked += new RoutedEventHandler(this.switchDirectLink_Checked);
                    this.switchDirectLink.Unchecked += new RoutedEventHandler(this.switchDirectLink_Checked);
                    return;

                case 2:
                    this.txtLink = (TextBox) target;
                    return;

                case 3:
                    this.rRegionUsa = (RadioButton) target;
                    return;

                case 4:
                    this.rRegionIntl = (RadioButton) target;
                    return;
            }
            this._contentLoaded = true;
        }
    }
}

