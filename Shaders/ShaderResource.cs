using System;
using System.IO;
using System.Reflection;

namespace MonoGame.Library.Shaders;

internal class ShaderResource (string name)
{
    public const string SdfCircleName = "MonoGame.Samples.Library.Shaders.Sdf.SdfCircle.mgfxo";

    public const string SdfLineName = "MonoGame.Samples.Library.Shaders.Sdf.SdfLine.mgfxo";

    public const string SdfParabolaName = "MonoGame.Samples.Library.Shaders.Sdf.SdfParabola.mgfxo";

    public static readonly ShaderResource SdfCircle = new (SdfCircleName);

    public static readonly ShaderResource SdfLine = new (SdfLineName);

    public static readonly ShaderResource SdfParabola = new (SdfParabolaName);

    private readonly object _locker = new ();

    public byte[] Bytecode
    {
        get
        {
            if (_bytecode is null)
            {
                lock (_locker)
                {
                    if (_bytecode is not null)
                    {
                        return _bytecode;
                    }

                    _bytecode = PlatformGetBytecode (name);
                }
            }

            return _bytecode;
        }
    }

    private byte[]? _bytecode;

    private static byte[] PlatformGetBytecode (string name)
    {
        Assembly? assembly = Assembly.GetAssembly (typeof (ShaderResource));
        ArgumentNullException.ThrowIfNull (assembly);

        Stream? manifestResourceStream = assembly.GetManifestResourceStream (name);
        ArgumentNullException.ThrowIfNull (manifestResourceStream);

        using MemoryStream memoryStream = new ();
        manifestResourceStream.CopyTo (memoryStream);
        return memoryStream.ToArray ();
    }
}
