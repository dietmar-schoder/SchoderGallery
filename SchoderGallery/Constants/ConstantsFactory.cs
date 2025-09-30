namespace SchoderGallery.Constants;

public interface IConstantsFactory
{
    IConstants GetConstants(ScreenMode mode);
}

public class ConstantsFactory(IEnumerable<IConstants> constantsList) : IConstantsFactory
{
    public IConstants GetConstants(ScreenMode mode) =>
        constantsList.FirstOrDefault(c => c.ScreenMode == mode);
}