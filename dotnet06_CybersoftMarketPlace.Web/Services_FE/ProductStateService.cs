
using System.Text.Json;
using dotnet06_CybersoftMarketPlace.Web.Pages;

public class ProductStateService
{
    private readonly HttpClient _httpClient;
    public List<ProductIndexPageDTO> Products { get; private set; } = new List<ProductIndexPageDTO>();

    public ProductDetailDTO productDetailDTO { get; private set; } = new ProductDetailDTO();

    public ProductVariantDetailDTO productVarSelected { get; private set; } = new ProductVariantDetailDTO();


    public ProductStateService(HttpClient httpClient, IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("CybersoftMarketplaceApi");
    }


    public void SetProductVarSelected(ProductVariantDetailDTO productVar)
    {
        productVarSelected = productVar;
        StateHasChanged();
    }


    public async Task LoadProductsAsync(string keyword = "", int pageIndex = 1, int pageSize = 10)
    {
        //Gọi api từ backend để lấy danh sách sản phẩm
        var response = await _httpClient.GetAsync($"/api/Product/GetAll?keyword={keyword}&pageIndex={pageIndex}&pageSize={pageSize}");
        if (response.IsSuccessStatusCode)
        {
            var responseData = await response.Content.ReadFromJsonAsync<HTTPResponseData<List<ProductIndexPageDTO>>>();
            if (responseData != null && responseData.statusCode == 200)
            {
                // Console.WriteLine($@"{JsonSerializer.Serialize(responseData.DataResponse)}");
                //Cập nhật api response data vào state management
                Products = responseData.DataResponse;
                StateHasChanged();
            }
        }
    }


    public async Task LoadProductDetailAsync(int productId)
    {
        //Gọi api từ backend để lấy chi tiết sản phẩm
        var response = await _httpClient.GetAsync($"/api/Product/GetProductDetail/{productId}");
        if (response.IsSuccessStatusCode)
        {
            var responseData = await response.Content.ReadFromJsonAsync<HTTPResponseData<ProductDetailDTO>>();
            if (responseData != null && responseData.statusCode == 200)
            {
                // Console.WriteLine($@"{JsonSerializer.Serialize(responseData.DataResponse)}");
                //Cập nhật api response data vào state management
                productDetailDTO = responseData.DataResponse;
                productVarSelected = productDetailDTO.ListProductVariant.FirstOrDefault() ?? new ProductVariantDetailDTO();
                StateHasChanged();
            }
        }
    }


    public Action OnChange { get; set; }

    public void StateHasChanged() => OnChange?.Invoke();

  
}