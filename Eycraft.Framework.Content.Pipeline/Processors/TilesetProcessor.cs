using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using System.IO;

namespace Eycraft.Framework.Content.Pipeline.Processors;

[ContentProcessor (DisplayName = "Tileset Processor")]
public sealed class TilesetProcessor : ContentProcessor<TilesetDocument, TilesetDocument>
{
    public override string Version => "1";

    public override TilesetDocument Process (TilesetDocument input, ContentProcessorContext context)
    {
        string jsonPath = context.SourceIdentity.SourceFilename;
        string directory = Path.Combine (context.ProjectDirectory, Path.GetDirectoryName (jsonPath) ?? string.Empty);

        string[] candidates = Directory.GetFiles (directory, input.Name, SearchOption.TopDirectoryOnly);

        if (candidates.Length == 0)
        {
            throw new FileNotFoundException ($"Sprite sheet texture '{input.Name}' not found in '{directory}'.");
        }

        string texturePath = candidates[0];

        input.Texture = context.BuildAndLoadAsset<TextureContent, TextureContent> (new ExternalReference<TextureContent> (texturePath), new TextureImporter (), new TextureProcessor ());

        return input;
    }
}
