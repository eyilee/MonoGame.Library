using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using System.IO;

namespace Eycraft.Framework.Content.Pipeline.Processors;

[ContentProcessor (DisplayName = "SpriteSheet Processor")]
public sealed class SpriteSheetProcessor : ContentProcessor<SpriteSheetDocument, SpriteSheetDocument>
{
    public override string Version => "1";

    public override SpriteSheetDocument Process (SpriteSheetDocument input, ContentProcessorContext context)
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
