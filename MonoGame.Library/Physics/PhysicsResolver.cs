using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MonoGame.Library.Physics;

public class PhysicsResolver
{
    public int Iterations { get; set; } = 4;

    public void Resolve (HashSet<Collision> collisions)
    {
        for (int i = 0; i < Iterations; i++)
        {
            foreach (Collision collision in collisions)
            {
                Resolve (collision);
            }
        }
    }

    private static void Resolve (Collision collision)
    {
        PhysicsBody bodyA = collision.Self;
        PhysicsBody bodyB = collision.Other;

        if (bodyA.IsStatic && bodyB.IsStatic)
        {
            return;
        }

        if (!collision.TryGetContact (out Contact contact))
        {
            return;
        }

        Vector2 correction = contact.Normal * contact.Penetration;

        if (bodyA.IsStatic)
        {
            bodyB.Owner.Position -= correction;
        }
        else if (bodyB.IsStatic)
        {
            bodyA.Owner.Position += correction;
        }
        else
        {
            Vector2 halfCorrection = correction / 2f;

            bodyA.Owner.Position += halfCorrection;
            bodyB.Owner.Position -= halfCorrection;
        }
    }
}
