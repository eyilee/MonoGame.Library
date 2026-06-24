using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace MonoGame.Library.Graphics;

public class TextureAtlas (Texture2DResource texture)
{
    public Texture2DResource Texture { get; set; } = texture;

    private readonly Dictionary<string, TextureRegion> _regions = [];

    public void AddRegion (string name, int x, int y, int width, int height)
    {
        _regions.Add (name, new TextureRegion (Texture, x, y, width, height));
    }

    public bool TryGetRegion (string name, out TextureRegion? region) => _regions.TryGetValue (name, out region);

    public bool RemoveRegion (string name) => _regions.Remove (name);

    public static TextureAtlas FromFile (ContentManager content, string fileName)
    {
        string filePath = Path.Combine (content.RootDirectory, fileName);
        using Stream stream = TitleContainer.OpenStream (filePath);
        using XmlReader reader = XmlReader.Create (stream);
        XElement root = XDocument.Load (reader).Root ?? throw new NullReferenceException ();

        XElement texture = root.Element ("Texture") ?? throw new NullReferenceException ();
        Texture2DResource textureResource = new (fileName, content.Load<Texture2D> (texture.Value));
        TextureAtlas atlas = new (textureResource);

        XElement regions = root.Element ("Regions") ?? throw new NullReferenceException ();
        foreach (var region in regions.Elements ("Region"))
        {
            string name = region.Attribute ("name")?.Value ?? string.Empty;
            int x = int.Parse (region.Attribute ("x")?.Value ?? "0");
            int y = int.Parse (region.Attribute ("y")?.Value ?? "0");
            int width = int.Parse (region.Attribute ("width")?.Value ?? "0");
            int height = int.Parse (region.Attribute ("height")?.Value ?? "0");

            if (!string.IsNullOrEmpty (name))
            {
                atlas.AddRegion (name, x, y, width, height);
            }
        }

        return atlas;
    }
}
