namespace EveAIO.Views
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Markup;

    public class Browser2View : Page, IComponentConnector
    {
        public Border border2;
        private bool _contentLoaded;

        public Browser2View()
        {
            Class7.RIuqtBYzWxthF();
            this.InitializeComponent();
        }

        [DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocator = new Uri("/EveAIO;component/views/browser2view.xaml", UriKind.Relative);
                Application.LoadComponent(this, resourceLocator);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            // This item is obfuscated and can not be translated.
            if (connectionId != 1)
            {
                goto Label_0028;
            }
        Label_0009:
            switch (((0xf59ac73 ^ 0x51279466) % 4))
            {
                case 0:
                    goto Label_0009;

                case 1:
                    this.border2 = (Border) target;
                    return;

                case 2:
                    break;

                case 3:
                    return;

                default:
                    return;
            }
        Label_0028:
            this._contentLoaded = true;
            goto Label_0009;
        }
    }
}

