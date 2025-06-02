using System.Runtime.InteropServices;

using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;

namespace SpotifyGlobalShortcut.App;

public partial class AppWindow
{
    private readonly HWND _hwnd;
    private readonly nint _extendedStyle;

    public AppWindow(nint hwnd)
    {
        if (hwnd == nint.Zero)
            throw new ArgumentException("AppWindow handle cannot be zero.", nameof(hwnd));

        _hwnd = new(hwnd);

        ClassName = GetWindowClass();

        uint processId = 0;
        unsafe
        {
            uint* processIdPtr = &processId;
            ThreadId = PInvoke.GetWindowThreadProcessId(_hwnd, processIdPtr);
        }
        ProcessId = processId;

        _extendedStyle = PInvoke.GetWindowLongPtr(_hwnd, WINDOW_LONG_PTR_INDEX.GWL_EXSTYLE);
    }

    public static AppWindow? GetForeground()
    {
        nint hwnd = PInvoke.GetForegroundWindow();
        return hwnd == nint.Zero ? null : new AppWindow(hwnd);
    }

    public nint HWND => _hwnd;

    public string ClassName { get; }

    public uint ProcessId { get; }

    public uint ThreadId { get; }

    public bool Foreground => PInvoke.GetForegroundWindow() == HWND;

    public bool Minimized => PInvoke.IsIconic(_hwnd);

    public bool TopMost => (_extendedStyle & (nint)WINDOW_EX_STYLE.WS_EX_TOPMOST) != 0;

    public bool ToForeground()
    {
        bool result = PInvoke.SetForegroundWindow(_hwnd);
        return !result ? throw new InvalidOperationException($"Failed to set window to foreground: {Marshal.GetLastWin32Error()}") : result;
    }

    public void Hide() => PInvoke.ShowWindow(_hwnd, SHOW_WINDOW_CMD.SW_MINIMIZE);

    public void Restore() => PInvoke.ShowWindow(_hwnd, SHOW_WINDOW_CMD.SW_RESTORE);

    private string GetWindowClass()
    {
        Span<char> buffer = new char[256];
        int length = PInvoke.GetClassName(_hwnd, buffer);
        if (length == 0)
        {
            throw new InvalidOperationException($"Failed to get window class name: {Marshal.GetLastWin32Error()}");
        }
        return buffer[..length].ToString();
    }
}