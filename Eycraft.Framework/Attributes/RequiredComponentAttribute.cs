using System;

namespace Eycraft.Framework.Attributes;

[AttributeUsage (AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public sealed class RequiredComponentAttribute : Attribute
{
}
