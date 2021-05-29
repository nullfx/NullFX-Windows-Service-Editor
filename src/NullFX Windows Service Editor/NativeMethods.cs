using System;
using System.Runtime.InteropServices;
using System.Text;

namespace NullFX.ServiceEditor {
    internal static class NativeMethods {
        internal const int ACCESS_TYPE_ALL = 0xf01ff;
        internal const int SC_MANAGER_ALL = 0xf003f;
        internal const int SERVICE_ALL_ACCESS = 0xf01ff;
        internal const int SERVICE_ERROR_NORMAL = 0x00000001;
        internal const int SERVICE_TYPE_WIN32_OWN_PROCESS = 0x10;
        internal const int START_TYPE_AUTO = 2;
        internal const int GWL_STYLE = -16;
        internal const int WS_MAXIMIZEBOX = 0x10000;
        internal const int WS_MINIMIZEBOX = 0x20000;

        [DllImport ( "user32.dll" )]
        internal extern static int GetWindowLong ( IntPtr hwnd, int index );

        [DllImport ( "user32.dll" )]
        internal extern static int SetWindowLong ( IntPtr hwnd, int index, int value );

        [DllImport ( "advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true )]
        internal static extern IntPtr OpenSCManager ( string machineName, string databaseName, int access );

        [DllImport ( "advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true )]
        internal static extern IntPtr CreateService ( IntPtr databaseHandle, string serviceName, string displayName, int access, int serviceType, int startType, int errorControl, string binaryPath, string loadOrderGroup, IntPtr pTagId, string dependencies, string servicesStartName, string password );
        
        [DllImport ( "advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true )]
        internal static extern bool CloseServiceHandle ( IntPtr handle );
        
        [DllImport ( "advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true )]
        internal static extern bool DeleteService ( IntPtr serviceHandle );
        
        [DllImport ( "advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true )]
        internal static extern IntPtr OpenService ( IntPtr databaseHandle, string serviceName, int access );
        
        [DllImport ( "kernel32.dll" )]
        internal static extern uint GetShortPathName ( string lpszLongPath, [Out] StringBuilder lpszShortPath, uint cchBuffer );
    }
}
