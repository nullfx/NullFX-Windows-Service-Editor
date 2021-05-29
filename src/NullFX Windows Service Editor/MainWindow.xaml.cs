using Microsoft.Win32;
using System;
using System.IO;
using System.ServiceProcess;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace NullFX.ServiceEditor {
    public partial class MainWindow : Window {
        ServiceController uninstallServiceController;
        Brush standardBrush = new SolidColorBrush ( SystemColors.ControlTextColor );
        Brush redBrush = new SolidColorBrush ( Colors.Red );

        public MainWindow ( ) { InitializeComponent ( ); }
        protected override void OnSourceInitialized ( EventArgs e ) {
            base.OnSourceInitialized ( e );
            this.HideMinimizeAndMaximizeButtons ( );
        }

        protected override void OnRenderSizeChanged ( SizeChangedInfo sizeInfo ) {
            base.OnRenderSizeChanged ( sizeInfo );
            warning.MinWidth = ( ( Width - 41 ) - 120 );
        }

        protected override void OnChildDesiredSizeChanged ( UIElement child ) {
            base.OnChildDesiredSizeChanged ( child );
            warning.MinWidth = ( ( Width - 41 ) - 120 );
        }

        private void HandleOnBrowseAppPath ( object sender, RoutedEventArgs e ) {
            var ofd = new OpenFileDialog { Title = "Locate Your Windows Service", Filter = "Windows Executable (*.exe)|*.exe" };
            if ( !string.IsNullOrEmpty ( applicationPath.Text ) ) {
                if ( File.Exists ( applicationPath.Text ) ) {
                    ofd.InitialDirectory = Path.GetDirectoryName ( applicationPath.Text );
                } else if ( Directory.Exists ( applicationPath.Text ) ) {
                    ofd.InitialDirectory = applicationPath.Text;
                }
            }
            if ( ofd.ShowDialog ( this ) ?? false ) {
                applicationPath.Text = ofd.FileName;
            }
        }

        private void HandleInstallService ( object sender, RoutedEventArgs e ) {
            if ( string.IsNullOrWhiteSpace ( applicationPath.Text ) || !File.Exists ( applicationPath.Text ) ) {
                successStatus.Content = "Application invalid or missing";
                successStatus.Foreground = redBrush;
                return;
            }
            try {
                if ( string.IsNullOrWhiteSpace ( logonName.Text ) ) {
                    NullFXServiceInstaller.Install ( serviceName.Text, displayName.Text, applicationPath.Text );
                } else {
                    NullFXServiceInstaller.Install ( serviceName.Text, displayName.Text, applicationPath.Text, applicationPath.Text, applicationPath.Text );
                }
                successStatus.Content = $"{displayName.Text} installed successfully";
                successStatus.Foreground = standardBrush;
            } catch ( Exception ex ) {
                successStatus.Content = ex.Message;
                successStatus.Foreground = redBrush;
            }
        }

        private void HandleUninstallService ( object sender, RoutedEventArgs e ) {
            if ( uninstallServiceController != null ) {
                try {
                    if ( MessageBox.Show ( this, $"Are you sure you wish to uninstall \"{uninstallServiceController.DisplayName}\"?\r\n\r\nTHIS ACTION CANNOT BE UNDONE!!!\r\n\r\nWE ARE NOT RESPONSIBLE IF YOU DESTROY YOUR OPERATING SYSTEM!", "Uninstall Service?", MessageBoxButton.YesNo, MessageBoxImage.Exclamation ) == MessageBoxResult.Yes ) {
                        if ( uninstallServiceController.CanStop ) {
                            uninstallServiceController.Stop ( );
                        }
                        NullFXServiceInstaller.Uninstall ( uninstallServiceController.ServiceName );
                        successStatus.Content = $"{uninstallServiceController.DisplayName} uninstalled successfully";
                        successStatus.Foreground = standardBrush;
                        uninstallServiceName.Items.Clear ( );
                        uninstallService.IsEnabled = false;
                        uninstallServiceController = null;
                    }
                } catch ( Exception ex ) {
                    successStatus.Content = ex.Message;
                    successStatus.Foreground = redBrush;
                }
            } else {
                successStatus.Content = "Failed to uninstall service";
                successStatus.Foreground = redBrush;
            }
        }

        private void HandleDropdownOpening ( object sender, EventArgs e ) {
            uninstallServiceName.Items.Clear ( );
            uninstallServiceName.Items.Add ( new ServiceControlContainer ( null ) );
            foreach ( ServiceController sc in ServiceController.GetServices ( ) ) {
                uninstallServiceName.Items.Add ( new ServiceControlContainer ( sc ) );
            }
        }

        private void HandleSelectionChanged ( object sender, SelectionChangedEventArgs e ) {
            if ( uninstallServiceName.SelectedItem == null ) return;
            uninstallServiceController = ( uninstallServiceName.SelectedItem as ServiceControlContainer ).Controller;
            uninstallService.IsEnabled = uninstallServiceController != null;
        }

        private void ValidateServiceName ( object sender, KeyEventArgs e ) {
            var keys = e.Key;
            e.Handled = !( keys == Key.Tab || keys >= Key.A && keys <= Key.Z || ( ( keys >= Key.D0 && keys <= Key.D9 ) || ( keys >= Key.NumPad0 && keys <= Key.NumPad9 ) ) || ( keys == Key.Back || keys == Key.Delete || keys == Key.Up || keys == Key.Down || keys == Key.Left || keys == Key.Right || keys == Key.Home || keys == Key.End ) );
        }

    }
}
