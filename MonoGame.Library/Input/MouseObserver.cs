using System;

namespace MonoGame.Library.Input;

internal class MouseObserver
{
    public event EventHandler<MouseEventArgs>? Observers;

    public bool HasObservers => Observers != null && Observers.GetInvocationList ().Length > 0;

    public void Notify (object? sender, MouseEventArgs eventArgs) => Observers?.Invoke (sender, eventArgs);
}
