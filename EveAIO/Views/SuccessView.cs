namespace EveAIO.Views
{
    using EveAIO;
    using EveAIO.Pocos;
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Markup;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    public class SuccessView : Page, IComponentConnector, IStyleConnector
    {
        internal Button btnScreenshot;
        internal TextBlock txtPause;
        internal Button btnClear;
        internal Button btnPostSuccess;
        public ListBox listSuccess;
        private bool _contentLoaded;

        public SuccessView()
        {
            Class7.RIuqtBYzWxthF();
            this.InitializeComponent();
            this.listSuccess.ItemsSource = Global.SUCCESS;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            Global.SUCCESS.Clear();
        }

        private void btnPostSuccess_Click(object sender, RoutedEventArgs e)
        {
            if (!Global.SUCCESS.Any<SuccessObject>(x => (x.CheckoutHidden.HasValue && (((SuccessObject.ActionEnum) x.CheckoutHidden.Value) == SuccessObject.ActionEnum.yes))))
            {
                MessageBox.Show("You have no successful checkouts to share", "Sorry", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            else
            {
                try
                {
                    Helpers.PostDiscordSuccess();
                    MessageBox.Show("Sucess posted", "Thank you", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
                catch
                {
                }
            }
        }

        private void btnScreenshot_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RenderTargetBitmap image = new RenderTargetBitmap((int) base.ActualWidth, (int) base.ActualHeight, 96.0, 96.0, PixelFormats.Pbgra32);
                image.Render(this);
                DrawingVisual visual = new DrawingVisual();
                using (DrawingContext context = visual.RenderOpen())
                {
                    VisualBrush brush = new VisualBrush(this);
                    context.DrawRectangle(brush, null, new Rect(new Point(), new Size(base.Width, base.Height)));
                }
                image.Render(visual);
                Clipboard.SetImage(image);
                MessageBox.Show("Image successfuly copied to the clipboard", "Screenshot taken", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            catch
            {
            }
        }

        [GeneratedCode("PresentationBuildTasks", "4.0.0.0"), DebuggerNonUserCode]
        public void InitializeComponent()
        {
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocator = new Uri("/EveAIO;component/views/successview.xaml", UriKind.Relative);
                Application.LoadComponent(this, resourceLocator);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
        }

        [DebuggerNonUserCode, EditorBrowsable(EditorBrowsableState.Never), GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    ((SuccessView) target).Loaded += new RoutedEventHandler(this.Page_Loaded);
                    return;

                case 2:
                    this.btnScreenshot = (Button) target;
                    this.btnScreenshot.Click += new RoutedEventHandler(this.btnScreenshot_Click);
                    return;

                case 3:
                    this.txtPause = (TextBlock) target;
                    return;

                case 4:
                    this.btnClear = (Button) target;
                    this.btnClear.Click += new RoutedEventHandler(this.btnClear_Click);
                    return;

                case 5:
                    this.btnPostSuccess = (Button) target;
                    this.btnPostSuccess.Click += new RoutedEventHandler(this.btnPostSuccess_Click);
                    return;

                case 6:
                    this.listSuccess = (ListBox) target;
                    return;
            }
            this._contentLoaded = true;
        }

        [EditorBrowsable(EditorBrowsableState.Never), DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        void IStyleConnector.Connect(int connectionId, object target)
        {
            if (connectionId == 7)
            {
                ((Border) target).MouseDown += new MouseButtonEventHandler(this.Border_MouseDown);
            }
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly SuccessView.<>c <>9;
            public static Func<SuccessObject, bool> <>9__5_0;

            static <>c()
            {
                Class7.RIuqtBYzWxthF();
                <>9 = new SuccessView.<>c();
            }

            public <>c()
            {
                Class7.RIuqtBYzWxthF();
            }

            internal bool <btnPostSuccess_Click>b__5_0(SuccessObject x) => 
                (x.CheckoutHidden.HasValue && (((SuccessObject.ActionEnum) x.CheckoutHidden.Value) == SuccessObject.ActionEnum.yes));
        }
    }
}

