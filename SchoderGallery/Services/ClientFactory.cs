using SchoderGallery.Helpers;

namespace SchoderGallery.Services;

public class ClientFactory
{
    public HttpClient Frontend { get; }
    public HttpClient Backend { get; }

    public ClientFactory(IHttpClientFactory httpClientFactory, HttpClient frontendClient)
    {
        Frontend = frontendClient;
        Backend = httpClientFactory.CreateClient(Constants.Backend);
    }
}