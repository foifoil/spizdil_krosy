namespace EveAIO.Views
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Markup;

    public class SoleboxInfo : Window, IComponentConnector
    {
        public bool Understood;
        internal Button BtnClose;
        internal ToggleButton switchUnderstood;
        internal Button btnStart;
        private bool _contentLoaded;

        public SoleboxInfo(TaskWindow2 owner)
        {
            Class7.RIuqtBYzWxthF();
            base.Owner = owner;
            this.InitializeComponent();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            base.Close();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            this.Understood = true;
            base.Close();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.DragMove();
        }

        [GeneratedCode("PresentationBuildTasks", "4.0.0.0"), DebuggerNonUserCode]
        public void InitializeComponent()
        {
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocator = new Uri("/EveAIO;component/views/soleboxinfo.xaml", UriKind.Relative);
                Application.LoadComponent(this, resourceLocator);
            }
        }

        private void switchUnderstood_Checked(object sender, RoutedEventArgs e)
        {
            if (this.switchUnderstood.IsChecked.HasValue && this.switchUnderstood.IsChecked.Value)
            {
                this.btnStart.IsEnabled = true;
                this.btnStart.Opacity = 1.0;
            }
            else
            {
                this.btnStart.IsEnabled = false;
                this.btnStart.Opacity = 0.6;
            }
        }

        [DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0"), EditorBrowsable(EditorBrowsableState.Never)]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    ((Grid) target).MouseLeftButtonDown += new MouseButtonEventHandler(this.Grid_MouseLeftButtonDown);
                    return;

                case 2:
                    this.BtnClose = (Button) target;
                    this.BtnClose.Click += new RoutedEventHandler(this.BtnClose_Click);
                    return;

                case 3:
                    this.switchUnderstood = (ToggleButton) target;
                    this.switchUnderstood.Checked += new RoutedEventHandler(this.switchUnderstood_Checked);
                    this.switchUnderstood.Unchecked += new RoutedEventHandler(this.switchUnderstood_Checked);
                    return;

                case 4:
                    this.btnStart = (Button) target;
                    this.btnStart.Click += new RoutedEventHandler(this.btnStart_Click);
                    return;
            }
            this._contentLoaded = true;
        }
    }
}

