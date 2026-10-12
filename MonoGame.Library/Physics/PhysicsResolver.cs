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

        if (!collision.TryGetContact (out Contact contact))
        {
            return;
        }

        Vector2 correction = contact.Normal * contact.Penetration * 0.5f;

        if (!self.PhysicsBody.IsDynamic)
        {
            other.Transform.Position += correction;
        }
        else if (!other.PhysicsBody.IsDynamic)
        {
            self.Transform.Position -= correction;
        }
        else
        {
            Vector2 halfCorrection = correction / 2f;

            self.Transform.Position -= halfCorrection;
            other.Transform.Position += halfCorrection;
        }
    }
}
