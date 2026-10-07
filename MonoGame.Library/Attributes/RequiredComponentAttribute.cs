using System;

namespace MonoGame.Library.Attributes;

[AttributeUsage (AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public sealed class RequiredComponentAttribute : Attribute
{
}
