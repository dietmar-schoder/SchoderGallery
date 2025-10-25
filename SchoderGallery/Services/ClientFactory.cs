using SchoderGallery.Helpers;

namespace SchoderGallery.Services;

public class ClientFactory
{
    public HttpClient Frontend { get; }
    public HttpClient Backend { get; }

    public ClientFactory(HttpClient frontendClient, IHttpClientFactory httpClientFactory)
    {
        Frontend = frontendClient;
        Backend = httpClientFactory.CreateClient(Constants.Backend);
    }
}
