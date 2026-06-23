using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Library.Shaders;

public class SdfCircleEffect (GraphicsDevice graphicsDevice) : Effect (graphicsDevice, ShaderResource.SdfCircle.Bytecode)
{
}

public class SdfLineEffect (GraphicsDevice graphicsDevice) : Effect (graphicsDevice, ShaderResource.SdfLine.Bytecode)
{
}

public class SdfParabolaEffect (GraphicsDevice graphicsDevice) : Effect (graphicsDevice, ShaderResource.SdfParabola.Bytecode)
{
}