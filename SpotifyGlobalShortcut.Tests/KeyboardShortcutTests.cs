using SpotifyGlobalShortcut.App.Input;

namespace SpotifyGlobalShortcut.Tests;

public class KeyboardShortcutTests
{
    [Fact]
    public void InputSingleKeyGeneratesCorrectInputs()
    {
        var shortcut = new KeyboardShortcut(VirtualKey._A);
        var inputs = InputManager.ShortcutToInput(shortcut);

        Assert.Equal(2, inputs.Length);

        // Key down
        Assert.Equal((uint)1, (uint)inputs[0].type);
        Assert.Equal((ushort)VirtualKey._A, (ushort)inputs[0].Anonymous.ki.wVk);
        Assert.Equal((uint)0, (uint)inputs[0].Anonymous.ki.dwFlags);

        // Key up
        Assert.Equal((uint)1, (uint)inputs[1].type);
        Assert.Equal((ushort)VirtualKey._A, (ushort)inputs[0].Anonymous.ki.wVk);
        Assert.Equal((uint)0x0002, (uint)inputs[1].Anonymous.ki.dwFlags);
    }

    [Fact]
    public void InputMultipleKeysGeneratesCorrectInputs()
    {
        var shortcut = new KeyboardShortcut(VirtualKey.MENU, VirtualKey.SHIFT, VirtualKey._B);
        var inputs = InputManager.ShortcutToInput(shortcut);

        Assert.Equal(6, inputs.Length);

        // Down events
        Assert.Equal((ushort)VirtualKey.MENU, (uint)inputs[0].Anonymous.ki.wVk);
        Assert.Equal((uint)0, (uint)inputs[0].Anonymous.ki.dwFlags);

        Assert.Equal((ushort)VirtualKey.SHIFT, (uint)inputs[1].Anonymous.ki.wVk);
        Assert.Equal((uint)0, (uint)inputs[1].Anonymous.ki.dwFlags);

        Assert.Equal((ushort)VirtualKey._B, (uint)inputs[2].Anonymous.ki.wVk);
        Assert.Equal((uint)0, (uint)inputs[2].Anonymous.ki.dwFlags);

        // Up events (reverse order)
        Assert.Equal((ushort)VirtualKey._B, (uint)inputs[3].Anonymous.ki.wVk);
        Assert.Equal((uint)0x0002, (uint)inputs[3].Anonymous.ki.dwFlags);

        Assert.Equal((ushort)VirtualKey.SHIFT, (uint)inputs[4].Anonymous.ki.wVk);
        Assert.Equal((uint)0x0002, (uint)inputs[4].Anonymous.ki.dwFlags);

        Assert.Equal((ushort)VirtualKey.MENU, (uint)inputs[5].Anonymous.ki.wVk);
        Assert.Equal((uint)0x0002, (uint)inputs[5].Anonymous.ki.dwFlags);
    }

    [Fact]
    public void InputNoDuplicateKeys()
    {
        var shortcut = new KeyboardShortcut(VirtualKey._C, VirtualKey._C);
        var inputs = InputManager.ShortcutToInput(shortcut).ToArray();

        // Should still generate 4 inputs (2 for each key, even if duplicate)
        Assert.Equal(4, inputs.Length);
        Assert.All(inputs, input => Assert.Equal((ushort)VirtualKey._C, (ushort)input.Anonymous.ki.wVk));
    }

    [Fact]
    public void InputOrderIsCorrect()
    {
        var shortcut = new KeyboardShortcut(VirtualKey.CONTROL, VirtualKey.MENU, VirtualKey._D);
        var inputs = InputManager.ShortcutToInput(shortcut);

        Assert.Equal(6, inputs.Length);

        // Down: CONTROL, ALT, D
        Assert.Equal((ushort)VirtualKey.CONTROL, (uint)inputs[0].Anonymous.ki.wVk);
        Assert.Equal((ushort)VirtualKey.MENU, (uint)inputs[1].Anonymous.ki.wVk);
        Assert.Equal((ushort)VirtualKey._D, (uint)inputs[2].Anonymous.ki.wVk);

        // Up: D, ALT, CONTROL
        Assert.Equal((ushort)VirtualKey._D, (uint)inputs[3].Anonymous.ki.wVk);
        Assert.Equal((ushort)VirtualKey.MENU, (uint)inputs[4].Anonymous.ki.wVk);
        Assert.Equal((ushort)VirtualKey.CONTROL, (uint)inputs[5].Anonymous.ki.wVk);
    }
}