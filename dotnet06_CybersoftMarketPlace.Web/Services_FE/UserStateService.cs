using Infrastructure.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using static System.Net.WebRequestMethods;
public class UserStateService
{
    private readonly ILocalStorageService _localStorageService;
    public string accessToken = "";
    private readonly HttpClient _httpClient;

    public ProfileUserDTO? CurrentUser { get; private set; }
    public readonly NavigationManager _navigationManager;

    public UserStateService(ILocalStorageService localStorageService, IHttpClientFactory httpClientFactory,NavigationManager nav)
    {
        _localStorageService = localStorageService;
        _httpClient = httpClientFactory.CreateClient("CybersoftMarketplaceApi");
        _navigationManager = nav; //Service dùng để chuyển hướng trang
    }

    public Action OnChange { get; set; }
    public void StateHasChanged() => OnChange?.Invoke();

    public async Task LoginAsync(UserLoginDTO userLogin)
    {
        //Gọi api từ backend để đăng nhập
        var response = await _httpClient.PostAsJsonAsync("/api/User/Login", userLogin);
        if (response.IsSuccessStatusCode)
        {
            var responseData = await response.Content.ReadFromJsonAsync<HTTPResponseData<string>>();
            if (responseData != null && responseData.statusCode == 200)
            {
                accessToken = responseData.DataResponse;
                await _localStorageService.SetItemAsync("accessToken", accessToken);
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

                //Gọi hàm getprofile khi đăng nhập
                await GetProfileAsync();
                _navigationManager.NavigateTo("/profile"); //Chuyển hướng về trang chủ sau khi đăng nhập thành công

                StateHasChanged();
                
            }
        }
    }

    public async Task GetProfileAsync()
    {
        //Add header token
        string? token = await _localStorageService.GetItemAsync<string>("accessToken");
        Console.WriteLine($@"{token} - tokendemo");
        if (token != null)
        {

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            //Gọi api từ backend để lấy thông tin người dùng
            HttpResponseMessage? response = await _httpClient.PostAsync("/api/User/getProfile", null);

            if (response.IsSuccessStatusCode)
            {
                HTTPResponseData<ProfileUserDTO>? responseData = await response.Content.ReadFromJsonAsync<HTTPResponseData<ProfileUserDTO>>();
                if (responseData != null && responseData.statusCode == 200)
                {
                    CurrentUser = responseData.DataResponse;
                    accessToken = token;
                    StateHasChanged();
                }
            }
        }else
        {
            CurrentUser = null;
            accessToken = "";
            StateHasChanged();
        }
    }

    public async Task LogoutAsync()
    {
        accessToken = "";
        CurrentUser = null;
        await _localStorageService.RemoveItemAsync("accessToken");
        _httpClient.DefaultRequestHeaders.Authorization = null;
        StateHasChanged();
    }

}