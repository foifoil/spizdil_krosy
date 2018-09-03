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
    using System.Windows.Media;

    public class HibbettControl2 : UserControl, IComponentConnector
    {
        private TaskWindow2 _taskWindow;
        internal TextBlock lblLink;
        public TextBox txtLink;
        private bool _contentLoaded;

        public HibbettControl2(TaskWindow2 taskWindow)
        {
            Class7.RIuqtBYzWxthF();
            this.InitializeComponent();
            this._taskWindow = taskWindow;
        }

        public bool Check()
        {
            if (string.IsNullOrEmpty(this.txtLink.Text))
            {
                this.txtLink.Background = (Brush) Application.Current.MainWindow.FindResource("MissingFieldBackground");
                return false;
            }
            this.txtLink.Background = (Brush) Application.Current.MainWindow.FindResource("FilledBackground");
            return true;
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
            switch (((0x2ac090f5 ^ 0x277a5098) % 4))
            {
                case 0:
                    goto Label_000D;

                case 1:
                    return;

                case 2:
                    break;

                case 3:
                    return;

                default:
                    return;
            }
        Label_002C:
            this._contentLoaded = true;
            Uri resourceLocator = new Uri("/EveAIO;component/views/platforms/hibbettcontrol2.xaml", UriKind.Relative);
            Application.LoadComponent(this, resourceLocator);
            goto Label_000D;
        }

        [DebuggerNonUserCode, EditorBrowsable(EditorBrowsableState.Never), GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            if (connectionId != 1)
            {
                if (connectionId != 2)
                {
                    this._contentLoaded = true;
                }
                else
                {
                    this.txtLink = (TextBox) target;
                }
            }
            else
            {
                this.lblLink = (TextBlock) target;
            }
        }
    }
}

