using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: InternalsVisibleTo("SpotifyGlobalShortcut.Tests")]
namespace SpotifyGlobalShortcut.App.Input;

internal partial class KeyboardShortcut
{
    private readonly List<VirtualKey> _inputs;

    public KeyboardShortcut(IEnumerable<VirtualKey> combination)
    {
        _inputs = [.. combination];
    }

    public KeyboardShortcut(params VirtualKey[] combination)
    {
        _inputs = [.. combination];
    }

    public void AddKey(VirtualKey key)
    {
        if (!_inputs.Contains(key))
            _inputs.Add(key);
    }

    public INPUT[] Input
    {
        get
        {
            static INPUT GenerateInput(VirtualKey key, bool keyUp = false)
            {
                return new()
                {
                    type = INPUT_KEYBOARD,
                    u = new InputUnion
                    {
                        ki = new KEYBDINPUT
                        {
                            wVk = (ushort)key,
                            wScan = 0,
                            dwFlags = keyUp ? KEYEVENTF_KEYUP : 0,
                            dwExtraInfo = GetMessageExtraInfo(),
                        }
                    }
                };
            }

            INPUT[] inputs = new INPUT[_inputs.Count * 2];
            for (int i = 0, j = inputs.Length - 1; i < _inputs.Count; i++)
            {
                VirtualKey key = _inputs[i];
                inputs[i] = GenerateInput(key);
                inputs[j - i] = GenerateInput(key, true);
            }

            return inputs;
        }
    }

    [LibraryImport("user32.dll", SetLastError = true)]
    private static partial nint GetMessageExtraInfo();

    public const int INPUT_KEYBOARD = 1;
    public const uint KEYEVENTF_KEYUP = 0x0002;

    public struct INPUT
    {
        public int type;
        public InputUnion u;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct InputUnion
    {
        [FieldOffset(0)] private MOUSEINPUT mi;
        [FieldOffset(0)] public KEYBDINPUT ki;
        [FieldOffset(0)] private HARDWAREINPUT hi;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct MOUSEINPUT
    {
        public int dx;
        public int dy;
        public uint mouseData;
        public uint dwFlags;
        public uint time;
        public IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct KEYBDINPUT
    {
        public ushort wVk;
        public ushort wScan;
        public uint dwFlags;
        public uint time;
        public nint dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct HARDWAREINPUT
    {
        public uint uMsg;
        public ushort wParamL;
        public ushort wParamH;
    }

    [LibraryImport("user32.dll", SetLastError = true)]
    public static partial uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

    public static bool SendShortcut(KeyboardShortcut shortcut, out string errorMessage)
    {
        INPUT[] input = shortcut.Input;
        uint result = SendInput((uint)input.Length, input, Marshal.SizeOf<INPUT>());
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
}

public enum VirtualKey : ushort
{
    LWIN = 0x5B,
    RWIN = 0x5C,
    SHIFT = 0x10,
    CONTROL = 0x11,
    MENU = 0x12,
    LSHIFT = 0xA0,
    RSHIFT = 0xA1,
    LCONTROL = 0xA2,
    RCONTROL = 0xA3,
    LMENU = 0xA4,
    RMENU = 0xA5,
    ESCAPE = 0x1B,
    PGUP = 0x21,
    PGDOWN = 0x22,
    END = 0x23,
    HOME = 0x24,
    ARRLEFT = 0x25,
    ARRUP = 0x26,
    ARRRIGHT = 0x27,
    ARRDOWN = 0x28,
    INSERT = 0x2D,
    DELETE = 0x2E,
    _0 = 0x30,
    _1 = 0x31,
    _2 = 0x32,
    _3 = 0x33,
    _4 = 0x34,
    _5 = 0x35,
    _6 = 0x36,
    _7 = 0x37,
    _8 = 0x38,
    _9 = 0x39,
    _A = 0x41,
    _B = 0x42,
    _C = 0x43,
    _D = 0x44,
    _E = 0x45,
    _F = 0x46,
    _G = 0x47,
    _H = 0x48,
    _I = 0x49,
    _J = 0x4A,
    _K = 0x4B,
    _L = 0x4C,
    _M = 0x4D,
    _N = 0x4E,
    _O = 0x4F,
    _P = 0x50,
    _Q = 0x51,
    _R = 0x52,
    _S = 0x53,
    _T = 0x54,
    _U = 0x55,
    _V = 0x56,
    _W = 0x57,
    _X = 0x58,
    _Y = 0x59,
    _Z = 0x5A,
    F1 = 0x70,
    F2 = 0x71,
    F3 = 0x72,
    F4 = 0x73,
    F5 = 0x74,
    F6 = 0x75,
    F7 = 0x76,
    F8 = 0x77,
    F9 = 0x78,
    F10 = 0x79,
    F11 = 0x7A,
    F12 = 0x7B
}

public enum ScanCode : short
{
    LWIN = 91,
    RWIN = 92,
    SHIFT = 42,
    CONTROL = 29,
    ALT = 56,
    LSHIFT = 42,
    RSHIFT = 54,
    LCONTROL = 29,
    RCONTROL = 29,
    LALT = 56,
    RALT = 56,
    ESCAPE = 1,
    PGUP = 73,
    PGDOWN = 81,
    END = 79,
    HOME = 71,
    ARRLEFT = 75,
    ARRUP = 72,
    ARRRIGHT = 77,
    ARRDOWN = 80,
    INSERT = 82,
    DELETE = 83,
    _0 = 11,
    _1 = 2,
    _2 = 3,
    _3 = 4,
    _4 = 5,
    _5 = 6,
    _6 = 7,
    _7 = 8,
    _8 = 9,
    _9 = 10,
    _A = 30,
    _B = 48,
    _C = 46,
    _D = 32,
    _E = 18,
    _F = 33,
    _G = 34,
    _H = 35,
    _I = 23,
    _J = 36,
    _K = 37,
    _L = 38,
    _M = 50,
    _N = 49,
    _O = 24,
    _P = 25,
    _Q = 16,
    _R = 19,
    _S = 31,
    _T = 20,
    _U = 22,
    _V = 47,
    _W = 17,
    _X = 45,
    _Y = 21,
    _Z = 44,
    F1 = 59,
    F2 = 60,
    F3 = 61,
    F4 = 62,
    F5 = 63,
    F6 = 64,
    F7 = 65,
    F8 = 66,
    F9 = 67,
    F10 = 68,
    F11 = 87,
    F12 = 88
}