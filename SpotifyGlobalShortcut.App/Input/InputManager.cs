using System.Runtime.InteropServices;

using Windows.Win32;
using Windows.Win32.UI.Input.KeyboardAndMouse;

namespace SpotifyGlobalShortcut.App.Input;

public partial class InputManager
{
    public static bool SendShortcut(KeyboardShortcut shortcut, out string errorMessage)
    {
        ReadOnlySpan<INPUT> input = ShortcutToInput(shortcut);
        uint result = PInvoke.SendInput(input, Marshal.SizeOf<INPUT>());
        if (result != (uint)input.Length)
        {
            errorMessage = $"Error sending input: {Marshal.GetLastWin32Error()}";
            return false;
        }
        else
        {
            errorMessage = string.Empty;
            return true;
        }
    }

    private static ReadOnlySpan<INPUT> ShortcutToInput(KeyboardShortcut shortcut)
    {
        List<VirtualKey> keys = [.. shortcut.Keys];
        INPUT[] inputs = new INPUT[keys.Count * 2];
        for (int i = 0, j = inputs.Length - 1; i < keys.Count; i++)
        {
            VirtualKey key = keys[i];
            inputs[i] = MakeInputFromKey(key);
            inputs[j - i] = MakeInputFromKey(key, true);
        }

        return new ReadOnlySpan<INPUT>(inputs);
    }

    private static INPUT MakeInputFromKey(VirtualKey key, bool keyUp = false)
    {
        INPUT input = new()
        {
            type = INPUT_TYPE.INPUT_KEYBOARD,
            Anonymous =
            {
                ki = new KEYBDINPUT
                {
                    wVk = (VIRTUAL_KEY)key,
                    wScan = 0,
                    dwFlags = keyUp ? KEYBD_EVENT_FLAGS.KEYEVENTF_KEYUP : 0,
                    dwExtraInfo = (nuint)PInvoke.GetMessageExtraInfo().Value,
                }
            }
        };

        return input;
    }
}
