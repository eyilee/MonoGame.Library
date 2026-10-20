using Microsoft.Xna.Framework.Content.Pipeline;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace MonoGame.Library.Content.Pipeline.Importers;

[ContentImporter (".spritesheet.json", DisplayName = "SpriteSheet JSON Importer", DefaultProcessor = "SpriteSheetProcessor")]
public sealed class SpriteSheetImporter : ContentImporter<SpriteSheetDocument>
{
    public override string Version => "1";

    public override SpriteSheetDocument Import (string filename, ContentImporterContext context)
    {
        Stream stream = File.OpenRead (filename);

        JsonDocumentOptions options = new ()
        {
            CommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = false
        };

        JsonDocument jsonDocument = JsonDocument.Parse (stream, options);

        return new SpriteSheetDocument
        {
            Sprite = jsonDocument.RootElement.GetProperty ("sprite").GetString () ?? string.Empty,
            Regions = GetRegions (jsonDocument),
            Animations = GetAnimations (jsonDocument)
        };
    }

    private static Dictionary<string, SpriteSheetRegionDocument> GetRegions (JsonDocument jsonDocument)
    {
        Dictionary<string, SpriteSheetRegionDocument> regions = [];

        foreach (JsonElement spriteElement in jsonDocument.RootElement.GetProperty ("sprites").EnumerateArray ())
        {
            regions.Add (spriteElement.GetProperty ("name").GetString () ?? string.Empty, new SpriteSheetRegionDocument
            {
                X = spriteElement.GetProperty ("x").GetInt32 (),
                Y = spriteElement.GetProperty ("y").GetInt32 (),
                Width = spriteElement.GetProperty ("width").GetInt32 (),
                Height = spriteElement.GetProperty ("height").GetInt32 ()
            });
        }

        return regions;
    }

    private static Dictionary<string, SpriteSheetAnimationDocument> GetAnimations (JsonDocument jsonDocument)
    {
        Dictionary<string, SpriteSheetAnimationDocument> animations = [];

        foreach (JsonElement animationElement in jsonDocument.RootElement.GetProperty ("animations").EnumerateArray ())
        {
            List<SpriteSheetAnimationFrameDocument> frames = [];

            foreach (JsonElement frameElement in animationElement.GetProperty ("frames").EnumerateArray ())
            {
                frames.Add (new SpriteSheetAnimationFrameDocument
                {
                    Region = frameElement.GetProperty ("region").GetString () ?? string.Empty,
                    Duration = frameElement.GetProperty ("duration").GetInt32 ()
                });
            }

            animations.Add (animationElement.GetProperty ("name").GetString () ?? string.Empty, new SpriteSheetAnimationDocument
            {
                Frames = frames
            });
        }

        return animations;
    }
}
