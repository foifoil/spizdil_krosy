namespace EveAIO.Views
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Markup;

    public class Browser3View : Page, IComponentConnector
    {
        private BrowserWindow _browser;
        internal TextBox txtBrowser;
        internal Button btnLoad;
        public Border border3;
        private bool _contentLoaded;

        public Browser3View(BrowserWindow browser)
        {
            Class7.RIuqtBYzWxthF();
            this.InitializeComponent();
            this._browser = browser;
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            this._browser._browser3.LoadUrl(this.txtBrowser.Text);
        }

        [DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            // This item is obfuscated and can not be translated.
            if (!this._contentLoaded)
            {
                goto Label_002C;
            }
        Label_000D:
            switch (((0x470aa826 ^ 0x283db239) % 4))
            {
                case 0:
                    goto Label_000D;

                case 1:
                    break;

                case 3:
                    return;

                default:
                {
                    Uri resourceLocator = new Uri("/EveAIO;component/views/browser3view.xaml", UriKind.Relative);
                    Application.LoadComponent(this, resourceLocator);
                    return;
                }
            }
        Label_002C:
            this._contentLoaded = true;
            goto Label_000D;
        }

        [GeneratedCode("PresentationBuildTasks", "4.0.0.0"), DebuggerNonUserCode, EditorBrowsable(EditorBrowsableState.Never)]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    this.txtBrowser = (TextBox) target;
                    return;

                case 2:
                    this.btnLoad = (Button) target;
                    this.btnLoad.Click += new RoutedEventHandler(this.btnLoad_Click);
                    return;

                case 3:
                    this.border3 = (Border) target;
                    return;
            }
            this._contentLoaded = true;
        }
    }
}

