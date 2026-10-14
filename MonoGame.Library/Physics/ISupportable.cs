using Microsoft.Xna.Framework;

namespace MonoGame.Library.Physics;

public interface ISupportable
{
    public Vector2 Position { get; }

    public Vector2 Support (Vector2 direction);

    public void GetContacts (in ISupportable other, Vector2 normal, float penetration, out Contact selfContact, out Contact otherContact);

    public void GetContacts (in Circle other, Vector2 normal, float penetration, out Contact selfContact, out Contact otherContact);

    public void GetContacts (in Polygon other, Vector2 normal, float penetration, out Contact selfContact, out Contact otherContact);
}
