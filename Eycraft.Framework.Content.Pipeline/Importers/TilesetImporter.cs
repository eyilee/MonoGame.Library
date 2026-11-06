using Microsoft.Xna.Framework.Content.Pipeline;
using System.IO;
using System.Text.Json;

namespace Eycraft.Framework.Content.Pipeline.Importers;

[ContentImporter (".tileset", DisplayName = "Tileset JSON Importer", DefaultProcessor = "TilesetProcessor")]
public sealed class TilesetImporter : ContentImporter<TilesetDocument>
{
    public override string Version => "1";

    public override TilesetDocument Import (string filename, ContentImporterContext context)
    {
        Stream stream = File.OpenRead (filename);

        JsonDocumentOptions options = new ()
        {
            CommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = false
        };

        JsonDocument jsonDocument = JsonDocument.Parse (stream, options);

        return new TilesetDocument
        {
            Name = jsonDocument.RootElement.GetProperty ("name").GetString () ?? string.Empty,
            TileWidth = jsonDocument.RootElement.GetProperty ("tile_width").GetInt32 (),
            TileHeight = jsonDocument.RootElement.GetProperty ("tile_height").GetInt32 ()
        };
    }
}
