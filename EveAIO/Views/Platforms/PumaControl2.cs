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

    public class PumaControl2 : UserControl, IComponentConnector
    {
        private TaskWindow2 _taskWindow;
        internal TextBlock lblLink;
        public TextBox txtLink;
        internal TextBlock lblColor;
        internal StackPanel pColorPick;
        public TextBox txtColor;
        public RadioButton rColorExact;
        public RadioButton rColorContains;
        public CheckBox chPickRandomColorNotAvailable;
        private bool _contentLoaded;

        public PumaControl2(TaskWindow2 taskWindow)
        {
            Class7.RIuqtBYzWxthF();
            this.InitializeComponent();
            this._taskWindow = taskWindow;
            this.rColorContains.IsChecked = true;
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
            switch (((-9964359 ^ -1687400225) % 4))
            {
                case 0:
                    return;

                case 1:
                    break;

                case 2:
                    return;

                case 3:
                    goto Label_000D;

                default:
                    return;
            }
        Label_002C:
            this._contentLoaded = true;
            Uri resourceLocator = new Uri("/EveAIO;component/views/platforms/pumacontrol2.xaml", UriKind.Relative);
            Application.LoadComponent(this, resourceLocator);
            goto Label_000D;
        }

        [DebuggerNonUserCode, EditorBrowsable(EditorBrowsableState.Never), GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    this.lblLink = (TextBlock) target;
                    return;

                case 2:
                    this.txtLink = (TextBox) target;
                    return;

                case 3:
                    this.lblColor = (TextBlock) target;
                    return;

                case 4:
                    this.pColorPick = (StackPanel) target;
                    return;

                case 5:
                    this.txtColor = (TextBox) target;
                    return;

                case 6:
                    this.rColorExact = (RadioButton) target;
                    return;

                case 7:
                    this.rColorContains = (RadioButton) target;
                    return;

                case 8:
                    this.chPickRandomColorNotAvailable = (CheckBox) target;
                    return;
            }
            this._contentLoaded = true;
        }
    }
}

