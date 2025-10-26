using SchoderGallery.Navigation;

namespace SchoderGallery.Builders;

public class BuilderFactory(IEnumerable<IBuilder> builders)
{
    private readonly Dictionary<FloorType, IBuilder> _builders = builders.ToDictionary(b => b.FloorType, b => b);

    public IBuilder GetBuilder(FloorType type) => _builders[type];
}