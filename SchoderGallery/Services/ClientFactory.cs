using SchoderGallery.Helpers;

namespace SchoderGallery.Services;

public class ClientFactory
{
    public HttpClient Frontend { get; }
    public HttpClient Backend { get; }

    // I think it needs that...
    public ClientFactory(HttpClient frontendClient, IHttpClientFactory httpClientFactory)
    {
        Frontend = frontendClient;
        Backend = httpClientFactory.CreateClient(Const.Backend);
    }
}
