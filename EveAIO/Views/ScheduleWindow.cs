namespace EveAIO.Views
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Markup;

    public class ScheduleWindow : Window, IComponentConnector
    {
        public DateTime PickedTime;
        internal Button BtnClose;
        internal TextBox txtSchedule;
        internal Button btnSchedule;
        private bool _contentLoaded;

        public ScheduleWindow(Window owner)
        {
            Class7.RIuqtBYzWxthF();
            this.InitializeComponent();
            base.Owner = owner;
            this.txtSchedule.Text = DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss");
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            base.Close();
        }

        private void btnSchedule_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.PickedTime = DateTime.ParseExact(this.txtSchedule.Text.Trim(), "MM-dd-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            }
            catch
            {
                MessageBox.Show("Incorrect date time format", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                return;
            }
            base.DialogResult = true;
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.DragMove();
        }

        [DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocator = new Uri("/EveAIO;component/views/schedulewindow.xaml", UriKind.Relative);
                Application.LoadComponent(this, resourceLocator);
            }
        }

        [DebuggerNonUserCode, EditorBrowsable(EditorBrowsableState.Never), GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
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
                    this.txtSchedule = (TextBox) target;
                    return;

                case 4:
                    this.btnSchedule = (Button) target;
                    this.btnSchedule.Click += new RoutedEventHandler(this.btnSchedule_Click);
                    return;
            }
            this._contentLoaded = true;
        }
    }
}

