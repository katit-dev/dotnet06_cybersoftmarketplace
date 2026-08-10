
using System.Text.Json;
using dotnet06_CybersoftMarketPlace.Web.Pages;

public class ProductStateService
{
    private readonly HttpClient _httpClient;

    public ProductStateService(HttpClient httpClient, IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("CybersoftMarketplaceApi");
    }





    public Action OnChange { get; set; }

    public void StateHasChanged() => OnChange?.Invoke();

  
}