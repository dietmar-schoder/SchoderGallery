namespace SchoderGallery.Builders;

public class BuilderFactory(IEnumerable<IBuilder> builders)
{
    private readonly Dictionary<BuilderType, IBuilder> _builders = builders.ToDictionary(b => b.Type, b => b);

    public IBuilder GetBuilder(BuilderType type) => _builders[type];
}