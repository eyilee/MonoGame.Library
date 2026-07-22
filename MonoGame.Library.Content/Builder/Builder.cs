using MonoGame.Framework.Content.Pipeline.Builder;

namespace MonoGame.Library.Content.Builder;

public class Builder : ContentBuilder
{
    public override IContentCollection GetContentCollection ()
    {
        var contentCollection = new ContentCollection ();

        contentCollection.Include<WildcardRule> ("**.fx");

        // By default, no content will be imported from the Assets folder using the default importer for their file type.
        // Please define your content collection rules here.

        /* Examples

        // Import all content in the Assets folder using the default importer for their file type.
        contentCollection.Include<WildcardRule>("*");

        // Only copy content from the assets folder rather than build it with the pipeline.
        contentCollection.IncludeCopy<WildcardRule>("*.json");

        // Exclude assets that match the pattern., only required overriding a default import behaviour.
        contentCollection.Exclude<WildcardRule>("Font/*.txt");

        // Include a specific asset with processor parameters.
        contentCollection.Include("Models/character.glb", new FbxImporter(),
            new MeshAnimatedModelProcessor()
            {
                Scale = 100.0f
            }
        );
        */

        return contentCollection;
    }
}
