using Microsoft.Xna.Framework;
using System;
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
        PhysicsBody bodySelf = collision.Self;
        PhysicsBody bodyOther = collision.Other;

        if (bodySelf.IsStatic && bodyOther.IsStatic)
        {
            return;
        }

        if (!collision.TryGetContact (out Contact contact))
        {
            return;
        }

        Vector2 correction = contact.Normal * contact.Penetration * 0.5f;

        if (bodySelf.IsStatic)
        {
            bodyOther.Owner.Position += correction;
        }
        else if (bodyOther.IsStatic)
        {
            bodySelf.Owner.Position -= correction;
        }
        else
        {
            Vector2 halfCorrection = correction / 2f;

            bodySelf.Owner.Position -= halfCorrection;
            bodyOther.Owner.Position += halfCorrection;
        }
    }
}
