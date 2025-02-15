using Avalonia.Controls;
using System.Runtime.InteropServices;
using System;

namespace HistoriansGuild.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        ExtendClientAreaToDecorationsHint = true;
        ExtendClientAreaChromeHints = Avalonia.Platform.ExtendClientAreaChromeHints.NoChrome;
        ExtendClientAreaTitleBarHeightHint = -1;

        //Opened += (_, _) => RemoveRoundedCorners();
        //Resized += (_, _) => RemoveRoundedCorners();
    }

    private void RemoveRoundedCorners()
    {
        int DWMWA_WINDOW_CORNER_PREFERENCE = 33;
        int DWMWCP_DONOTROUND = 1;

        if (OperatingSystem.IsWindows())
        {
            var handle = TryGetPlatformHandle();
            if (handle == null) return;

            var hwnd = handle.Handle;
            if (hwnd != IntPtr.Zero)
            {
                DwmSetWindowAttribute(hwnd, DWMWA_WINDOW_CORNER_PREFERENCE, ref DWMWCP_DONOTROUND, sizeof(int));
            }
        }
    }

    [DllImport("dwmapi.dll")]
    private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);
}