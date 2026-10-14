using System;

namespace MonoGame.Library.Input;

internal class KeyboardObserver
{
    public event EventHandler<KeyboardEventArgs>? Observers;

    public bool HasObservers => Observers != null && Observers.GetInvocationList ().Length > 0;

    public void Notify (object? sender, KeyboardEventArgs eventArgs) => Observers?.Invoke (sender, eventArgs);
}
