namespace EveAIO.Views
{
    using EveAIO;
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Markup;

    public class Splash : Window, IComponentConnector
    {
        private SplashMode _mode;
        private bool _readyToGo;
        private bool _canActivate;
        private CallResult _callResult;
        internal Grid activationGrid;
        internal TextBlock lblActivation;
        internal TextBox txtActivationKey;
        internal Button btnActivate;
        internal Grid checkGrid;
        internal ProgressBar progBar;
        internal TextBlock txtProgBar;
        private bool _contentLoaded;

        public Splash()
        {
            Class7.RIuqtBYzWxthF();
            this._canActivate = true;
            this._callResult = CallResult.e;
            this.InitializeComponent();
            Global.Init();
        }

        private void btnActivate_Click(object sender, RoutedEventArgs e)
        {
            if (!this._canActivate)
            {
                MessageBox.Show("You have to wait 20 seconds before trying again ...", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                this.DoWork();
            }
        }

        private void DoWork()
        {
            bool flag = true;
            if (true | flag)
            {
                this._readyToGo = true;
                Global.IP = Helpers.GetIP();
                try
                {
                    Global.ASM = Assembly.Load(EveAIO.License.Down("XakFor.Net"));
                    this._callResult = CallResult.ok;
                    this.Finish();
                }
                catch (Exception exception)
                {
                    Global.Logger.Error("Error starting application", exception);
                    this._callResult = CallResult.errorDown;
                }
            }
        }

        private void Finish()
        {
            base.Visibility = Visibility.Collapsed;
            new MainWindow().Show();
            base.Close();
        }

        [GeneratedCode("PresentationBuildTasks", "4.0.0.0"), DebuggerNonUserCode]
        public void InitializeComponent()
        {
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocator = new Uri("/EveAIO;component/views/splash.xaml", UriKind.Relative);
                Application.LoadComponent(this, resourceLocator);
            }
        }

        [DebuggerNonUserCode, EditorBrowsable(EditorBrowsableState.Never), GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    ((Splash) target).Loaded += new RoutedEventHandler(this.Window_Loaded);
                    return;

                case 2:
                    this.activationGrid = (Grid) target;
                    return;

                case 3:
                    this.lblActivation = (TextBlock) target;
                    return;

                case 4:
                    this.txtActivationKey = (TextBox) target;
                    this.txtActivationKey.TextChanged += new TextChangedEventHandler(this.txtActivationKey_TextChanged);
                    return;

                case 5:
                    this.btnActivate = (Button) target;
                    this.btnActivate.Click += new RoutedEventHandler(this.btnActivate_Click);
                    return;

                case 6:
                    this.checkGrid = (Grid) target;
                    return;

                case 7:
                    this.progBar = (ProgressBar) target;
                    return;

                case 8:
                    this.txtProgBar = (TextBlock) target;
                    return;
            }
            this._contentLoaded = true;
        }

        private void txtActivationKey_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.btnActivate.IsEnabled = !string.IsNullOrEmpty(this.txtActivationKey.Text);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.checkGrid.Visibility = Visibility.Visible;
            this._mode = SplashMode.check;
            this.txtProgBar.Text = "LOADING ...";
            this.DoWork();
        }

        public enum CallResult
        {
            ok,
            errorActivation,
            errorCheck,
            errorDown,
            alreadyActivated,
            notActive,
            invalidKey,
            e
        }

        private enum SplashMode
        {
            activation,
            check
        }
    }
}

