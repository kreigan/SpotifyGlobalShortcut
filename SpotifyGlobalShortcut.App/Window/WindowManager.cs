namespace SpotifyGlobalShortcut.App.Window;

internal partial class WindowManager
{
    public static nint ForegroundWindow
    {
        get => GetForegroundWindow(); set => SetForegroundWindow(value);
    }

    public static bool IsMinimized(nint hwnd)
    {
        if (hwnd == nint.Zero)
            throw new ArgumentException("Window handle cannot be zero.", nameof(hwnd));
        return IsIconic(hwnd);
    }

    public static void HideWindow(nint hwnd)
    {
        if (hwnd == nint.Zero)
            throw new ArgumentException("Window handle cannot be zero.", nameof(hwnd));
        if (IsMinimized(hwnd))
            throw new InvalidOperationException("Window is already minimized.");
        ShowWindow(hwnd, SW_MINIMIZE);
    }
    public static void RestoreWindow(nint hwnd)
    {
        if (hwnd == nint.Zero)
            throw new ArgumentException("Window handle cannot be zero.", nameof(hwnd));
        if (!IsMinimized(hwnd))
            throw new InvalidOperationException("Window is not minimized.");

        ShowWindow(hwnd, SW_RESTORE);
    }
}