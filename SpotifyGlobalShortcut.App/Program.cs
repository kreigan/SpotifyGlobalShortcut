using System.Runtime.Versioning;

using SpotifyGlobalShortcut.App.Input;

namespace SpotifyGlobalShortcut.App;

[SupportedOSPlatform("windows5.0")]
internal partial class Program
{
    private const string SpotifyProcessName = "Spotify";
    
    public static void Main(string[] args)
    {
        string processName = args.Length > 0 ? args[0] : SpotifyProcessName;
        SpotifyWindowManager manager = new(processName);
        AppWindow? spotifyWindow = manager.GetSpotifyWindow()
            ?? throw new InvalidOperationException($"Spotify window not found. Is {processName} running?");

        bool wasMinimized = spotifyWindow.Minimized;
        if (wasMinimized)
        {
            spotifyWindow.Restore();
        }

        AppWindow? currentWindow = AppWindow.GetForeground();
        spotifyWindow.ToForeground();
        Thread.Sleep(100);

        LikeDislike();

        Thread.Sleep(100);
        if (wasMinimized)
        {
            spotifyWindow.Hide();
        }

        currentWindow?.ToForeground();
    }

    private static void LikeDislike()
    {
        KeyboardShortcut likeDislike = new(
            VirtualKey.MENU,
            VirtualKey.SHIFT,
            VirtualKey._B
        );

        bool success = InputManager.SendShortcut(likeDislike, out string errorMessage);
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
