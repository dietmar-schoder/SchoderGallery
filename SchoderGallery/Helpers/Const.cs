namespace SchoderGallery.Helpers;

public static class Const
{
    //public const string SchoderGalleryServerUrl = "http://localhost:7045";
    public const string SchoderGalleryServerUrl = "https://schodergallery.azurewebsites.net/";
    public const string Frontend = nameof(Frontend);
    public const string Backend = nameof(Backend);
    public const string StoragePrefix = "SchoderGallery.";
    public const string VisitorStorageKey = "visitor.";
    public const int ARTWORKS_CACHE_TIMEOUT_MINUTES = 10;
}
