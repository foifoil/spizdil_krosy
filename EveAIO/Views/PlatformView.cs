namespace EveAIO.Views
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Markup;

    public class PlatformView : Page, IComponentConnector
    {
        private TaskWindow2 _parent;
        public ContentPresenter content;
        private bool _contentLoaded;

        public PlatformView(TaskWindow2 parent)
        {
            Class7.RIuqtBYzWxthF();
            this.InitializeComponent();
            this._parent = parent;
        }

        [GeneratedCode("PresentationBuildTasks", "4.0.0.0"), DebuggerNonUserCode]
        public void InitializeComponent()
        {
            // This item is obfuscated and can not be translated.
            Uri uri;
            if (!this._contentLoaded)
            {
                goto Label_002C;
            }
        Label_000D:
            switch (((0x358628d5 ^ 0x7785d8b3) % 4))
            {
                case 1:
                    break;

                case 2:
                    return;

                case 3:
                    goto Label_000D;

                default:
                    Application.LoadComponent(this, uri);
                    return;
            }
        Label_002C:
            this._contentLoaded = true;
            uri = new Uri("/EveAIO;component/views/platformview.xaml", UriKind.Relative);
            goto Label_000D;
        }

        [DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0"), EditorBrowsable(EditorBrowsableState.Never)]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            if (connectionId == 1)
            {
                this.content = (ContentPresenter) target;
            }
            else
            {
                this._contentLoaded = true;
            }
        }
    }
}

