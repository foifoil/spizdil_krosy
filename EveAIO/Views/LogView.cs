namespace EveAIO.Views
{
    using EveAIO;
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Markup;

    public class LogView : Page, IComponentConnector
    {
        internal CheckBox chAutoScroll;
        internal Button btnPause;
        internal TextBlock txtPause;
        internal Button btnClear;
        public TextBox txtLog;
        private bool _contentLoaded;

        public LogView()
        {
            Class7.RIuqtBYzWxthF();
            this.InitializeComponent();
            this.chAutoScroll.IsChecked = new bool?(Global.SETTINGS.LogAutoScroll);
            this.txtPause.Text = "RESUME";
            this.btnPause.Style = (Style) base.FindResource("RedBtn");
            Global.LOG_PAUSED = true;
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            this.txtLog.Clear();
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            if (this.txtPause.Text == "PAUSE")
            {
                this.txtPause.Text = "RESUME";
                this.btnPause.Style = (Style) base.FindResource("RedBtn");
                Global.LOG_PAUSED = true;
            }
            else
            {
                this.txtPause.Text = "PAUSE";
                this.btnPause.Style = (Style) base.FindResource("WhiteBtn");
                Global.LOG_PAUSED = false;
            }
        }

        private void chAutoScroll_Checked(object sender, RoutedEventArgs e)
        {
            if (this.chAutoScroll.IsChecked.HasValue && this.chAutoScroll.IsChecked.Value)
            {
                Global.SETTINGS.LogAutoScroll = true;
            }
            else
            {
                Global.SETTINGS.LogAutoScroll = false;
            }
        }

        [DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocator = new Uri("/EveAIO;component/views/logview.xaml", UriKind.Relative);
                Application.LoadComponent(this, resourceLocator);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    this.chAutoScroll = (CheckBox) target;
                    this.chAutoScroll.Checked += new RoutedEventHandler(this.chAutoScroll_Checked);
                    this.chAutoScroll.Unchecked += new RoutedEventHandler(this.chAutoScroll_Checked);
                    return;

                case 2:
                    this.btnPause = (Button) target;
                    this.btnPause.Click += new RoutedEventHandler(this.btnPause_Click);
                    return;

                case 3:
                    this.txtPause = (TextBlock) target;
                    return;

                case 4:
                    this.btnClear = (Button) target;
                    this.btnClear.Click += new RoutedEventHandler(this.btnClear_Click);
                    return;

                case 5:
                    this.txtLog = (TextBox) target;
                    this.txtLog.TextChanged += new TextChangedEventHandler(this.txtLog_TextChanged);
                    return;
            }
            this._contentLoaded = true;
        }

        private void txtLog_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Global.SETTINGS.LogAutoScroll)
            {
                this.txtLog.ScrollToEnd();
            }
        }
    }
}

