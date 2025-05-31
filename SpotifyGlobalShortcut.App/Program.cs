using System.Diagnostics;

using SpotifyGlobalShortcut.App.Input;
using SpotifyGlobalShortcut.App.Window;

namespace SpotifyGlobalShortcut.App;

internal partial class Program
{
    public static void Main(string[] args)
    {
        nint originalWindow = WindowManager.ForegroundWindow;

        string processName = args.Length > 0 ? args[0] : "Spotify";
        Process[] processes = Process.GetProcessesByName(processName);
        nint hwnd = (from proc in processes
                     where proc.MainWindowHandle != nint.Zero
                     select proc.MainWindowHandle).FirstOrDefault();

        if (WindowManager.IsMinimized(hwnd))
            WindowManager.RestoreWindow(hwnd);

        WindowManager.ForegroundWindow = hwnd;
        Thread.Sleep(100);

        LikeDislike();

        Thread.Sleep(100);
        WindowManager.ForegroundWindow = originalWindow;
    }

    private static void LikeDislike()
    {
        KeyboardShortcut likeDislike = new(
            VirtualKey.MENU,
            VirtualKey.SHIFT,
            VirtualKey._B
        );
        
        bool success = SendShortcut(likeDislike, out string errorMessage);
        if (!success)
        {
            Console.WriteLine($"Failed to send shortcut: {errorMessage}");
        }
        else
        {
            Console.WriteLine("Shortcut sent successfully.");
        }
    }
}
