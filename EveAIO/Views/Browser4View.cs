namespace EveAIO.Views
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Markup;

    public class Browser4View : Page, IComponentConnector
    {
        public Border border4;
        private bool _contentLoaded;

        public Browser4View()
        {
            Class7.RIuqtBYzWxthF();
            this.InitializeComponent();
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
            switch (((-1850080197 ^ -1337136094) % 4))
            {
                case 0:
                    break;

                case 1:
                    return;

                case 2:
                    goto Label_000D;

                case 3:
                    return;

                default:
                    return;
            }
        Label_002C:
            this._contentLoaded = true;
            Uri resourceLocator = new Uri("/EveAIO;component/views/browser4view.xaml", UriKind.Relative);
            Application.LoadComponent(this, resourceLocator);
            goto Label_000D;
        }

        [GeneratedCode("PresentationBuildTasks", "4.0.0.0"), DebuggerNonUserCode, EditorBrowsable(EditorBrowsableState.Never)]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            // This item is obfuscated and can not be translated.
            if (connectionId != 1)
            {
                goto Label_0028;
            }
        Label_0009:
            switch (((0x1f16dc2d ^ 0x48475dcf) % 4))
            {
                case 0:
                    return;

                case 1:
                    break;

                case 2:
                    this.border4 = (Border) target;
                    return;

                case 3:
                    goto Label_0009;

                default:
                    return;
            }
        Label_0028:
            this._contentLoaded = true;
            goto Label_0009;
        }
    }
}

