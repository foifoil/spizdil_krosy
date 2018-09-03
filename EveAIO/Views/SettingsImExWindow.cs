namespace EveAIO.Views
{
    using EveAIO;
    using Microsoft.Win32;
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Markup;
    using System.Xml.Serialization;

    public class SettingsImExWindow : Window, IComponentConnector
    {
        private WindowStyleEnum _style;
        private static XmlSerializer _serSettings;
        internal TextBlock txtTitle;
        internal Button BtnClose;
        public TextBox txtFile;
        internal Button btnFile;
        internal TextBlock lblInfo;
        internal Button btnSave;
        internal TextBlock txtButton;
        internal Button btnCancel;
        private bool _contentLoaded;

        static SettingsImExWindow()
        {
            Class7.RIuqtBYzWxthF();
            _serSettings = new XmlSerializer(typeof(Settings));
        }

        public SettingsImExWindow(MainWindow owner, WindowStyleEnum style)
        {
            Class7.RIuqtBYzWxthF();
            base.Owner = owner;
            this._style = style;
            this.InitializeComponent();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            base.Close();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            base.Close();
        }

        private void btnFile_Click(object sender, RoutedEventArgs e)
        {
            bool? nullable;
            if (this._style == WindowStyleEnum.EXPORT)
            {
                SaveFileDialog dialog2 = new SaveFileDialog {
                    Filter = "*.settings|*.settings"
                };
                nullable = dialog2.ShowDialog();
                if (nullable.GetValueOrDefault() ? nullable.HasValue : false)
                {
                    this.txtFile.Text = dialog2.FileName;
                }
            }
            else
            {
                OpenFileDialog dialog = new OpenFileDialog {
                    Filter = "*.settings|*.settings"
                };
                nullable = dialog.ShowDialog();
                if (nullable.GetValueOrDefault() ? nullable.HasValue : false)
                {
                    this.txtFile.Text = dialog.FileName;
                }
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (this._style == WindowStyleEnum.EXPORT)
            {
                try
                {
                    string plainText = "";
                    using (MemoryStream stream = new MemoryStream())
                    {
                        _serSettings.Serialize((Stream) stream, Global.SETTINGS);
                        plainText = Encoding.UTF8.GetString(stream.ToArray());
                    }
                    string contents = EncryptorAes.Encrypt(plainText);
                    File.WriteAllText(this.txtFile.Text, contents);
                    MessageBox.Show("Settings exported successfuly", "Export", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    base.Close();
                }
                catch (Exception exception)
                {
                    Global.Logger.Error("Error occured while exporting the settings", exception);
                    MessageBox.Show("Error occured while exporting the settings", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                }
            }
            else
            {
                try
                {
                    using (StringReader reader = new StringReader(EncryptorAes.Decrypt(File.ReadAllText(this.txtFile.Text.Trim()))))
                    {
                        Global.SETTINGS = (Settings) _serSettings.Deserialize(reader);
                    }
                    Helpers.SaveSettings();
                    MessageBox.Show("Settings import successfuly, EveAIO will now close", "Export", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    Environment.Exit(0);
                }
                catch (Exception exception2)
                {
                    Global.Logger.Error("Error occured while importing the settings", exception2);
                    MessageBox.Show("Error occured while importing the settings", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                }
            }
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
                Uri resourceLocator = new Uri("/EveAIO;component/views/settingsimexwindow.xaml", UriKind.Relative);
                Application.LoadComponent(this, resourceLocator);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    ((SettingsImExWindow) target).Loaded += new RoutedEventHandler(this.Window_Loaded);
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
                    this.txtFile = (TextBox) target;
                    this.txtFile.TextChanged += new TextChangedEventHandler(this.txtFile_TextChanged);
                    return;

                case 6:
                    this.btnFile = (Button) target;
                    this.btnFile.Click += new RoutedEventHandler(this.btnFile_Click);
                    return;

                case 7:
                    this.lblInfo = (TextBlock) target;
                    return;

                case 8:
                    this.btnSave = (Button) target;
                    this.btnSave.Click += new RoutedEventHandler(this.btnSave_Click);
                    return;

                case 9:
                    this.txtButton = (TextBlock) target;
                    return;

                case 10:
                    this.btnCancel = (Button) target;
                    this.btnCancel.Click += new RoutedEventHandler(this.btnCancel_Click);
                    return;
            }
            this._contentLoaded = true;
        }

        private void txtFile_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtFile.Text.Trim()))
            {
                this.btnSave.IsEnabled = true;
                this.btnSave.Opacity = 1.0;
            }
            else
            {
                this.btnSave.IsEnabled = false;
                this.btnSave.Opacity = 0.6;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (this._style == WindowStyleEnum.EXPORT)
            {
                this.txtTitle.Text = "EXPORT SETTINGS";
                this.lblInfo.Visibility = Visibility.Collapsed;
                base.Height = 110.0;
                this.txtButton.Text = "EXPORT";
            }
            else
            {
                this.txtTitle.Text = "IMPORT SETTINGS";
                this.txtButton.Text = "IMPORT";
            }
        }

        public enum WindowStyleEnum
        {
            IMPORT,
            EXPORT
        }
    }
}

