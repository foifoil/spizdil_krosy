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

    public class SupremeInstoreControl2 : UserControl, IComponentConnector
    {
        private TaskWindow2 _taskWindow;
        public RadioButton rRegionUsa;
        public RadioButton rRegionEu;
        public RadioButton rRegionJapan;
        public RadioButton rLocBrooklyn;
        public RadioButton rLocManhattan;
        public RadioButton rLocLA;
        private bool _contentLoaded;

        public SupremeInstoreControl2(TaskWindow2 taskWindow)
        {
            Class7.RIuqtBYzWxthF();
            this.InitializeComponent();
            this._taskWindow = taskWindow;
            this.rRegionUsa.IsChecked = true;
            this.rLocBrooklyn.IsChecked = true;
            this._taskWindow._generalView.chSizeRandom.IsChecked = true;
        }

        public bool Check() => 
            true;

        [DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocator = new Uri("/EveAIO;component/views/platforms/supremeinstorecontrol2.xaml", UriKind.Relative);
                Application.LoadComponent(this, resourceLocator);
            }
        }

        private void rRegion_Checked(object sender, RoutedEventArgs e)
        {
            if (this.rRegionEu.IsChecked.HasValue && this.rRegionEu.IsChecked.Value)
            {
                this.rLocBrooklyn.IsEnabled = false;
                this.rLocBrooklyn.Opacity = 0.6;
                this.rLocLA.IsEnabled = false;
                this.rLocLA.Opacity = 0.6;
                this.rLocManhattan.IsEnabled = false;
                this.rLocManhattan.Opacity = 0.6;
            }
            else
            {
                this.rLocBrooklyn.IsEnabled = true;
                this.rLocBrooklyn.Opacity = 1.0;
                this.rLocLA.IsEnabled = true;
                this.rLocLA.Opacity = 1.0;
                this.rLocManhattan.IsEnabled = true;
                this.rLocManhattan.Opacity = 1.0;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), GeneratedCode("PresentationBuildTasks", "4.0.0.0"), DebuggerNonUserCode]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    this.rRegionUsa = (RadioButton) target;
                    this.rRegionUsa.Checked += new RoutedEventHandler(this.rRegion_Checked);
                    return;

                case 2:
                    this.rRegionEu = (RadioButton) target;
                    this.rRegionEu.Checked += new RoutedEventHandler(this.rRegion_Checked);
                    return;

                case 3:
                    this.rRegionJapan = (RadioButton) target;
                    this.rRegionJapan.Checked += new RoutedEventHandler(this.rRegion_Checked);
                    return;

                case 4:
                    this.rLocBrooklyn = (RadioButton) target;
                    return;

                case 5:
                    this.rLocManhattan = (RadioButton) target;
                    return;

                case 6:
                    this.rLocLA = (RadioButton) target;
                    return;
            }
            this._contentLoaded = true;
        }
    }
}

