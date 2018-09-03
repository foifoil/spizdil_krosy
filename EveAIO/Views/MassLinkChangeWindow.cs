namespace EveAIO.Views
{
    using EveAIO;
    using EveAIO.Pocos;
    using EveAIO.Tasks;
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Markup;

    public class MassLinkChangeWindow : Window, IComponentConnector
    {
        private bool _dontStart;
        private MassLinkChangeModeEnum _mode;
        internal TextBlock txtTitle;
        internal Button BtnClose;
        public TextBox txtLink;
        internal Button btnSave;
        internal Button btnStart;
        private bool _contentLoaded;

        public MassLinkChangeWindow(Window owner, MassLinkChangeModeEnum mode)
        {
            Class7.RIuqtBYzWxthF();
            this._mode = mode;
            base.Owner = owner;
            this.InitializeComponent();
            if (this._mode == MassLinkChangeModeEnum.ALL)
            {
                this.txtTitle.Text = this.txtTitle.Text + " - ALL MODE";
            }
            try
            {
                this.txtLink.Text = Clipboard.GetText();
            }
            catch
            {
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            base.Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            this._dontStart = true;
            this.btnStart_Click(null, null);
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            List<string> ids = new List<string>();
            string text = this.txtLink.Text;
            int startIndex = text.IndexOf("//") + 2;
            int index = text.Substring(startIndex).IndexOf("/");
            if (index != -1)
            {
                text = text.Substring(0, startIndex + index);
            }
            foreach (TaskObject obj2 in Global.SETTINGS.TASKS)
            {
                string link = obj2.Link;
                if ((obj2.TaskType == TaskObject.TaskTypeEnum.directlink) && (obj2.Platform != TaskObject.PlatformEnum.supremeinstore))
                {
                    if (obj2.IsShopifyCheckoutLink)
                    {
                        link = obj2.ShopifyCheckoutLink;
                    }
                    startIndex = link.IndexOf("//") + 2;
                    index = link.Substring(startIndex).IndexOf("/");
                    if (index != -1)
                    {
                        link = link.Substring(0, startIndex + index);
                    }
                }
                if (text == link)
                {
                    obj2.Link = this.txtLink.Text.Trim();
                    obj2.TaskType = TaskObject.TaskTypeEnum.directlink;
                    obj2.Keywords.Clear();
                    obj2.NegativeKeywords.Clear();
                    ids.Add(obj2.Id);
                }
            }
            Helpers.SaveSettings();
            if (!this._dontStart)
            {
                TaskManager.StartTasks(ids);
            }
            base.Close();
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
                Uri resourceLocator = new Uri("/EveAIO;component/views/masslinkchangewindow.xaml", UriKind.Relative);
                Application.LoadComponent(this, resourceLocator);
            }
        }

        private void QuickTaskCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (this.btnStart.IsEnabled)
            {
                this.btnStart_Click(null, null);
            }
        }

        [GeneratedCode("PresentationBuildTasks", "4.0.0.0"), EditorBrowsable(EditorBrowsableState.Never), DebuggerNonUserCode]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.QuickTaskCommand_Executed);
                    return;

                case 2:
                    ((Grid) target).MouseLeftButtonDown += new MouseButtonEventHandler(this.Grid_MouseLeftButtonDown);
                    return;

                case 3:
                    this.txtTitle = (TextBlock) target;
                    return;

                case 4:
                    this.BtnClose = (Button) target;
                    this.BtnClose.Click += new RoutedEventHandler(this.BtnClose_Click);
                    return;

                case 5:
                    this.txtLink = (TextBox) target;
                    this.txtLink.TextChanged += new TextChangedEventHandler(this.txtLink_TextChanged);
                    return;

                case 6:
                    this.btnSave = (Button) target;
                    this.btnSave.Click += new RoutedEventHandler(this.btnSave_Click);
                    return;

                case 7:
                    this.btnStart = (Button) target;
                    this.btnStart.Click += new RoutedEventHandler(this.btnStart_Click);
                    return;
            }
            this._contentLoaded = true;
        }

        private void txtLink_TextChanged(object sender, TextChangedEventArgs e)
        {
            if ((!string.IsNullOrEmpty(this.txtLink.Text.Trim()) && this.txtLink.Text.ToLowerInvariant().Contains("http")) && (this.txtLink.Text.Length > 8))
            {
                this.btnStart.IsEnabled = true;
                this.btnStart.Opacity = 1.0;
                this.btnSave.IsEnabled = true;
                this.btnSave.Opacity = 1.0;
            }
            else
            {
                this.btnStart.IsEnabled = false;
                this.btnStart.Opacity = 0.6;
                this.btnSave.IsEnabled = false;
                this.btnSave.Opacity = 0.6;
            }
        }

        public enum MassLinkChangeModeEnum
        {
            ALL,
            SPECIFIC
        }
    }
}

