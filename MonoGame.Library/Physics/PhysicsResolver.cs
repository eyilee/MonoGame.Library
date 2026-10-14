using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MonoGame.Library.Physics;

public class PhysicsResolver
{
    public int Iterations { get; set; } = 4;

    public void Resolve (HashSet<ColliderCollision> collisions)
    {
        for (int i = 0; i < Iterations; i++)
        {
            foreach (ColliderCollision collision in collisions)
            {
                Resolve (collision);
            }
        }
    }

    private static void Resolve (ColliderCollision collision)
    {
        Collider self = collision.Self;
        Collider other = collision.Other;

        if (!self.PhysicsBody.IsDynamic && !other.PhysicsBody.IsDynamic)
        {
            return;
        }

        if (!collision.TryGetContacts (out Contact selfContact, out Contact otherContact))
        {
            return;
        }

        Vector2 selfCorrection = selfContact.Normal * selfContact.Penetration * 0.5f;
        Vector2 otherCorrection = otherContact.Normal * otherContact.Penetration * 0.5f;

        if (!self.PhysicsBody.IsDynamic)
        {
            other.Transform.Position -= otherCorrection;
        }
        else if (!other.PhysicsBody.IsDynamic)
        {
            self.Transform.Position -= selfCorrection;
        }
        else
        {
            self.Transform.Position -= selfCorrection / 2f;
            other.Transform.Position -= otherCorrection / 2f;
        }
    }
}
