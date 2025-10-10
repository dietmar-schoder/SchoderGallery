using SchoderGallery.DTOs;

namespace SchoderGallery.Helpers;

public class SizeHelperFactory(IEnumerable<ISizeHelper> helpers)
{
    private readonly Dictionary<SizeType, ISizeHelper> _helpers = helpers.ToDictionary(h => h.SizeType, h => h);

    public ISizeHelper GetHelper(SizeType sizeType) =>
        _helpers[sizeType];
}