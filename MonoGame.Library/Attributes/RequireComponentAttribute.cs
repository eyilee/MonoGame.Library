using System;

namespace MonoGame.Library.Attributes;

[AttributeUsage (AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public sealed class RequireComponentAttribute (Type componentType) : Attribute
{
    public Type ComponentType { get; } = componentType;
}
