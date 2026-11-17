namespace Eycraft.Framework.Physics;

public class Collision (Collider self, Collider other, Contact selfContact, Contact otherContact)
{
    public Collider Self { get; } = self;

    public Collider Other { get; } = other;

    public Contact SelfContact { get; } = selfContact;

    public Contact OtherContact { get; } = otherContact;
}
