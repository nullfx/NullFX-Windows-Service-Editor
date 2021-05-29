using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace NullFX.ServiceEditor {
    internal class NullFXServiceInstaller {
        internal static void Install ( string serviceName, string displayName, string applicationPath ) {
            Install ( serviceName, displayName, applicationPath, null, null );
        }

        internal static void Install ( string serviceName, string displayName, string applicationPath, string userName, string password ) {
            var appPath = applicationPath;
            if ( applicationPath.Length > 256 ) {
                var appName = Path.GetFileName ( applicationPath );
                if ( appName.Length > 256 ) {
                    throw new ArgumentException ( "the application name is too long" );
                }
                var dir = Path.GetDirectoryName ( applicationPath );
                if ( dir.EndsWith ( '\\' ) ) { dir = dir.Trim ( '\\' ); }
                StringBuilder shortPath = new StringBuilder ( 256 - appName.Length );
                NativeMethods.GetShortPathName ( dir, shortPath, ( uint )shortPath.Capacity );
                appPath = shortPath.ToString ( );
                appPath = Path.Combine ( appPath, Path.GetFileName ( applicationPath ) );
            }
            if ( File.Exists ( appPath ) ) {
                var sch = NativeMethods.OpenSCManager ( Environment.MachineName, null, NativeMethods.SC_MANAGER_ALL );
                var result = NativeMethods.CreateService ( sch, serviceName, displayName, NativeMethods.ACCESS_TYPE_ALL, NativeMethods.SERVICE_TYPE_WIN32_OWN_PROCESS, NativeMethods.START_TYPE_AUTO, NativeMethods.SERVICE_ERROR_NORMAL, appPath, null, IntPtr.Zero, null, userName, password );
                if ( result == IntPtr.Zero ) {
                    NativeMethods.CloseServiceHandle ( result );
                    NativeMethods.CloseServiceHandle ( sch );
                    try { Uninstall ( serviceName ); } catch { }
                    throw new Win32Exception ( Marshal.GetLastWin32Error ( ), $"An error occured when attempting to install service {serviceName}" );
                }
                NativeMethods.CloseServiceHandle ( result );
                NativeMethods.CloseServiceHandle ( sch );
            } else {
                throw new FileNotFoundException ( $"{appPath} not found" );
            }
        }

        internal static void Uninstall ( string serviceName ) {
            var sch = NativeMethods.OpenSCManager ( Environment.MachineName, null, NativeMethods.SC_MANAGER_ALL );
            var svc = NativeMethods.OpenService ( sch, serviceName, NativeMethods.SERVICE_ALL_ACCESS );
            var success = NativeMethods.DeleteService ( svc );
            NativeMethods.CloseServiceHandle ( svc );
            NativeMethods.CloseServiceHandle ( sch );
            if ( !success ) { throw new Win32Exception ( Marshal.GetLastWin32Error ( ), $"An error occured when attempting to delete service {serviceName}" ); }
        }
    }
}
