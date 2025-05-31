using SpotifyGlobalShortcut.App;
using SpotifyGlobalShortcut.App.Input;

namespace SpotifyGlobalShortcut.Tests;

public class KeyboardShortcutTests
{
    [Fact]
    public void InputSingleKeyGeneratesCorrectInputs()
    {
        var shortcut = new KeyboardShortcut(VirtualKey._A);
        var inputs = shortcut.Input;

        Assert.Equal(2, inputs.Length);

        // Key down
        Assert.Equal(1, inputs[0].type);
        Assert.Equal((ushort)VirtualKey._A, inputs[0].u.ki.wVk);
        Assert.Equal((uint)0, inputs[0].u.ki.dwFlags);

        // Key up
        Assert.Equal(1, inputs[1].type);
        Assert.Equal((ushort)VirtualKey._A, inputs[1].u.ki.wVk);
        Assert.Equal((uint)0x0002, inputs[1].u.ki.dwFlags);
    }

    [Fact]
    public void InputMultipleKeysGeneratesCorrectInputs()
    {
        var shortcut = new KeyboardShortcut(VirtualKey.MENU, VirtualKey.SHIFT, VirtualKey._B);
        var inputs = shortcut.Input;

        Assert.Equal(6, inputs.Length);

        // Down events
        Assert.Equal((ushort)VirtualKey.MENU, inputs[0].u.ki.wVk);
        Assert.Equal((uint)0, inputs[0].u.ki.dwFlags);

        Assert.Equal((ushort)VirtualKey.SHIFT, inputs[1].u.ki.wVk);
        Assert.Equal((uint)0, inputs[1].u.ki.dwFlags);

        Assert.Equal((ushort)VirtualKey._B, inputs[2].u.ki.wVk);
        Assert.Equal((uint)0, inputs[2].u.ki.dwFlags);

        // Up events (reverse order)
        Assert.Equal((ushort)VirtualKey._B, inputs[3].u.ki.wVk);
        Assert.Equal((uint)0x0002, inputs[3].u.ki.dwFlags);

        Assert.Equal((ushort)VirtualKey.SHIFT, inputs[4].u.ki.wVk);
        Assert.Equal((uint)0x0002, inputs[4].u.ki.dwFlags);

        Assert.Equal((ushort)VirtualKey.MENU, inputs[5].u.ki.wVk);
        Assert.Equal((uint)0x0002, inputs[5].u.ki.dwFlags);
    }

    [Fact]
    public void InputNoDuplicateKeys()
    {
        var shortcut = new KeyboardShortcut(VirtualKey._C, VirtualKey._C);
        var inputs = shortcut.Input;

        // Should still generate 4 inputs (2 for each key, even if duplicate)
        Assert.Equal(4, inputs.Length);
        Assert.All(inputs, input => Assert.Equal((ushort)VirtualKey._C, input.u.ki.wVk));
    }

    [Fact]
    public void InputOrderIsCorrect()
    {
        var shortcut = new KeyboardShortcut(VirtualKey.CONTROL, VirtualKey.MENU, VirtualKey._D);
        var inputs = shortcut.Input;

        Assert.Equal(6, inputs.Length);

        // Down: CONTROL, ALT, D
        Assert.Equal((ushort)VirtualKey.CONTROL, inputs[0].u.ki.wVk);
        Assert.Equal((ushort)VirtualKey.MENU, inputs[1].u.ki.wVk);
        Assert.Equal((ushort)VirtualKey._D, inputs[2].u.ki.wVk);

        // Up: D, ALT, CONTROL
        Assert.Equal((ushort)VirtualKey._D, inputs[3].u.ki.wVk);
        Assert.Equal((ushort)VirtualKey.MENU, inputs[4].u.ki.wVk);
        Assert.Equal((ushort)VirtualKey.CONTROL, inputs[5].u.ki.wVk);
    }
}