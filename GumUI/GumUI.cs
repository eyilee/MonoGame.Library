using Gum.Wireframe;
using MonoGameGum;
using MonoGameGum.GueDeriving;

namespace MonoGame.Library.GumUI;

public abstract class GumUI
{
    public GraphicalUiElement RootObject => _rootObject;

    protected readonly GraphicalUiElement _rootObject;

    private bool _instantiated;

    public GumUI ()
    {
        _rootObject = new ContainerRuntime ();
        _rootObject.Dock (Dock.SizeToChildren);
    }

    public GumUI (GraphicalUiElement rootObject)
    {
        _rootObject = rootObject;
    }

    protected virtual void OnInstantiate () { }

    private GumUI Instantiate (GraphicalUiElement? parent = null)
    {
        if (_instantiated)
        {
            return this;
        }

        OnInstantiate ();

        if (parent == null)
        {
            _rootObject.AddToRoot ();
        }
        else
        {
            parent.AddChild (_rootObject);
        }

        _instantiated = true;

        return this;
    }

    protected virtual void OnDetach () { }

    public void Detach ()
    {
        if (!_instantiated)
        {
            return;
        }

        OnDetach ();

        _rootObject.RemoveFromRoot ();
    }

    public static T Instantiate<T> (T gumUI, GraphicalUiElement? parent = null) where T : GumUI
    {
        return (T)gumUI.Instantiate (parent);
    }
}
