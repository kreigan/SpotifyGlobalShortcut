using System.Collections.Immutable;

namespace SpotifyGlobalShortcut.App.Input;

public class KeyboardShortcut
{
    private readonly List<VirtualKey> _inputs;

    public KeyboardShortcut(IEnumerable<VirtualKey> keys)
    {
        _inputs = [.. keys];
    }

    public KeyboardShortcut(params VirtualKey[] keys)
    {
        _inputs = [.. keys];
    }

    public ImmutableList<VirtualKey> Keys => [.. _inputs];

    public void AddKey(VirtualKey key) => _inputs.Add(key);
}
