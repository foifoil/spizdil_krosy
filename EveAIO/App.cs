namespace EveAIO
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Threading;
    using System.Windows;
    using System.Windows.Markup;
    using System.Windows.Threading;

    public class App : Application, IComponentConnector
    {
        private static Mutex mutex = new Mutex(true, "{F7863DBE-A2B8-440A-9A21-A652AB7C0DC9}");
        private bool _contentLoaded;

        private static void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            if (e.Exception.Message.Contains("System.OutOfMemoryException"))
            {
                MessageBox.Show("System ran out of memory", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                Global.Logger.Error("System ran out of memory", e.Exception);
            }
            else
            {
                MessageBox.Show("Unexpected error occured", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                Global.Logger.Error("Unexpected error occured", e.Exception);
            }
            e.Handled = true;
        }

        [GeneratedCode("PresentationBuildTasks", "4.0.0.0"), DebuggerNonUserCode]
        public void InitializeComponent()
        {
            base.StartupUri = new Uri("Views/Splash.xaml", UriKind.Relative);
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocator = new Uri("/EveAIO;component/app.xaml", UriKind.Relative);
                Application.LoadComponent(this, resourceLocator);
            }
        }

        [STAThread]
        private static void Main()
        {
            Helpers.ProxyCheck();
            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                App app1 = new App();
                app1.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(App.App_DispatcherUnhandledException);
                app1.InitializeComponent();
                app1.Run();
                mutex.ReleaseMutex();
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            this._contentLoaded = true;
        }
    }
}

