using System;
using System.Windows;

namespace NullFX.ServiceEditor {
    internal static class WindowExtensions {

        internal static void HideMinimizeAndMaximizeButtons ( this Window window ) {
            IntPtr hwnd = new System.Windows.Interop.WindowInteropHelper ( window ).Handle;
            var currentStyle = NativeMethods.GetWindowLong ( hwnd, NativeMethods.GWL_STYLE );

            NativeMethods.SetWindowLong ( hwnd, NativeMethods.GWL_STYLE, ( currentStyle & ~NativeMethods.WS_MAXIMIZEBOX & ~NativeMethods.WS_MINIMIZEBOX ) );
        }
    }
}
