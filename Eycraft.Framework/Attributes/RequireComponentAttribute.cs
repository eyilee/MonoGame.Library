using System;

namespace Eycraft.Framework.Attributes;

[AttributeUsage (AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public sealed class RequireComponentAttribute (Type componentType) : Attribute
{
    public Type ComponentType { get; } = componentType;
}
