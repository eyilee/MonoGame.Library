using Microsoft.Xna.Framework.Graphics;

namespace Eycraft.Framework.Graphics;

public interface IBatchEncoder<T> where T : struct, IVertexType
{
    public virtual void Encode (T[] batchVertices, int index, Mesh mesh) { }
}
