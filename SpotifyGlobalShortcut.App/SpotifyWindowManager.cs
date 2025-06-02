using System.Diagnostics;

using Windows.Win32;

namespace SpotifyGlobalShortcut.App;

public class SpotifyWindowManager(string processName)
{
    private readonly string _processName = processName;

    public AppWindow? GetSpotifyWindow()
    {
        Process[] processes = Process.GetProcessesByName(_processName);
        nint hwnd = (from proc in processes
                     where proc.MainWindowHandle != nint.Zero
                     select proc.MainWindowHandle).FirstOrDefault();
        
        if (hwnd == nint.Zero)
        {
            return null;
        }
        
        AppWindow spotifyWindow = new(hwnd);
        return spotifyWindow.TopMost ? GetMainSpotifyWindow(spotifyWindow) : spotifyWindow;
    }

    private static AppWindow GetMainSpotifyWindow(AppWindow miniPlayerWindow)
    {
        AppWindow? window = null;
        bool result = PInvoke.EnumThreadWindows(miniPlayerWindow.ThreadId, (hwnd, _) =>
        {
            if (hwnd == nint.Zero)
                return true;

            window = new(hwnd);
            if (window.HWND != miniPlayerWindow.HWND
                && window.ClassName.Equals(miniPlayerWindow.ClassName)
                && !window.TopMost)
            {
                return false;
            }
            return true;
        }, 0);

        if (result || window == null)
        {
            throw new InvalidOperationException("Failed to find the main Spotify window.");
        }

        return window;
    }
}
