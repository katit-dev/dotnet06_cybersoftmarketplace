using System.Text.Json;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using static System.Net.WebRequestMethods;

public interface ICartService
{
    Task<HTTPResponseData<CartDTO>> GetCartByUserIdAsync(string userId);
    // Task<HTTPResponseData<CartDTO>> AddItemToCartAsync(string userId, int productVariantId, int quantity=1);
    // Task<HTTPResponseData<CartDTO>> ChangeQuantityVariantInCartAsync(string userId, int productVariantId, int quantity);

    // Task<int> GetCartIdByUserId(string userId);
}