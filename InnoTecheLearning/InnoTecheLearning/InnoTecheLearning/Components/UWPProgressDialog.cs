#if WINDOWS_PHONE_APP || WINDOWS_UWP
using global::System;
using global::System.ComponentModel;
using global::System.Runtime.CompilerServices;
using global::System.Windows.Input;
using global::Windows.ApplicationModel.Core;
using global::Windows.UI.Core;
using global::Windows.UI.Xaml;
using global::Windows.UI.Xaml.Controls;
using global::Windows.UI.Xaml.Data;
using global::Windows.UI.Xaml.Markup;

namespace InnoTecheLearning
{

    public class Command : ICommand
    {
        public event EventHandler CanExecuteChanged;
        readonly Action action;

        public Command(Action action)
        {
            this.action = action;
        }


        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this.action();
        }
    }
    public interface IProgressDialog : IDisposable
    {
        string Title { get; set; }
        int PercentComplete { get; set; }
        bool IsShowing { get; }

        void Show();
        void Hide();
    }
    public enum MaskType
    {
        Black,
        Gradient,
        Clear,
        None
    }
    public class ProgressDialogConfig
    {
        public static string DefaultCancelText { get; set; } = "Cancel";
        public static string DefaultTitle { get; set; } = "Loading";
        public static MaskType DefaultMaskType { get; set; } = MaskType.Black;


        public string CancelText { get; set; }
        public string Title { get; set; }
        public bool AutoShow { get; set; }
        public bool IsDeterministic { get; set; }
        public MaskType MaskType { get; set; }
        public Action OnCancel { get; set; }


        public ProgressDialogConfig()
        {
            this.Title = DefaultTitle;
            this.CancelText = DefaultCancelText;
            this.MaskType = DefaultMaskType;
            this.AutoShow = true;
        }


        public ProgressDialogConfig SetCancel(string cancelText = null, Action onCancel = null)
        {
            if (cancelText != null)
                this.CancelText = cancelText;

            this.OnCancel = onCancel;
            return this;
        }


        public ProgressDialogConfig SetTitle(string title)
        {
            this.Title = title;
            return this;
        }


        public ProgressDialogConfig SetMaskType(MaskType maskType)
        {
            this.MaskType = maskType;
            return this;
        }


        public ProgressDialogConfig SetAutoShow(bool autoShow)
        {
            this.AutoShow = autoShow;
            return this;
        }


        public ProgressDialogConfig SetIsDeterministic(bool isDeterministic)
        {
            this.IsDeterministic = isDeterministic;
            return this;
        }
    }
    public class ProgressContentDialog : ContentDialog
    {
        public ProgressContentDialog()
        {
            this.InitializeComponent();
        }
        private void InitializeComponent()
        {
            this.TitleTemplate =
                (DataTemplate)XamlReader.Load(
@"  &lt;ContentDialog
        x:Class=""Acr.UserDialogs.ProgressContentDialog""
        xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
        xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
        xmlns:local=""using:Acr.UserDialogs""
        xmlns:d=""http://schemas.microsoft.com/expression/blend/2008""
        xmlns:mc=""http://schemas.openxmlformats.org/markup-compatibility/2006""
        mc:Ignorable=""d""
        MinHeight=""0""&gt;

        &lt;ContentDialog.Content&gt;
            &lt;StackPanel Margin=""0""&gt;
                &lt;TextBlock Text=""{Binding Title}"" FontSize=""17"" HorizontalAlignment=""Center"" VerticalAlignment=""Center"" Margin=""0,0,0,5"" /&gt;
                &lt;ProgressBar x:Name=""ProgressBar"" Value=""{Binding PercentComplete}"" HorizontalAlignment=""Stretch"" IsIndeterminate=""{Binding IsIndeterministic}"" Minimum=""0"" Maximum=""100"" /&gt;
                &lt;TextBlock Text=""{Binding PercentComplete}"" HorizontalAlignment=""Center"" FontSize=""17"" Visibility=""{Binding TextPercentVisibility}"" Margin=""0,0,0,5"" /&gt;
                &lt;Button Visibility=""{Binding CancelVisibility}"" HorizontalAlignment=""Center"" Content=""{Binding CancelText}"" Command=""{Binding Cancel}"" /&gt;
            &lt;/StackPanel&gt;
        &lt;/ContentDialog.Content&gt;
    &lt;/ContentDialog&gt;");
        }
        /**/
    }
    public class ProgressDialog : IProgressDialog, INotifyPropertyChanged
        {
            readonly ProgressContentDialog dialog;
            readonly ProgressDialogConfig config;


            public ProgressDialog(ProgressDialogConfig config)
            {
                this.config = config;
            this.dialog = new ProgressContentDialog();
                this.Cancel = new Command(() => config.OnCancel?.Invoke());

            }


            public bool IsShowing { get; private set; }


            int percent;
            public int PercentComplete
            {
                get { return this.percent; }
                set
                {
                    if (value > 100)
                        this.percent = 100;
                    else if (value < 0)
                        this.percent = 0;
                    else
                        this.percent = value;
                    this.Change();
                }
            }


            public string CancelText => this.config.CancelText;
            public bool IsIndeterministic => !this.config.IsDeterministic;
            public Visibility TextPercentVisibility => this.config.IsDeterministic ? Visibility.Visible : Visibility.Collapsed;


            string title;
            public string Title
            {
                get { return this.title; }
                set
                {
                    this.title = value;
                    this.Change();
                }
            }


            public void Dispose()
            {
                this.Hide();
            }


            public void Hide()
            {
                if (!this.IsShowing)
                    return;

                this.IsShowing = false;
                this.Dispatch(() => this.dialog.Hide());
            }


            public void Show()
            {
                if (this.IsShowing)
                    return;

                this.IsShowing = true;

                this.Dispatch(() =>
#pragma warning disable 4014
                this.dialog.ShowAsync()
#pragma warning restore 4014
                );
            }


            void Change([CallerMemberName] string property = null)
            {
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
            }


            public ICommand Cancel { get; }
            public Visibility CancelVisibility => this.config.OnCancel == null
                ? Visibility.Collapsed
                : Visibility.Visible;


            public event PropertyChangedEventHandler PropertyChanged;


            protected virtual void Dispatch(Action action)
            {
#pragma warning disable 1024, 1633, 1634, 4014
#pragma arning idsable 4014
            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action());
#pragma warning restore 1024, 1633, 1634, 4014
        }
    }
    }
#endif