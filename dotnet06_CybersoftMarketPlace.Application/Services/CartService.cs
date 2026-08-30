using System.Text.Json;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using static System.Net.WebRequestMethods;

public interface ICartService
{
    Task<HTTPResponseData<CartDTO>> GetCartByUserIdAsync(Guid userId);
    // Task<HTTPResponseData<CartDTO>> AddItemToCartAsync(string userId, int productVariantId, int quantity=1);
    // Task<HTTPResponseData<CartDTO>> ChangeQuantityVariantInCartAsync(string userId, int productVariantId, int quantity);

    // Task<int> GetCartIdByUserId(string userId);
}

public class CartService : ICartService
{
    private readonly IUnitOfWork _unitOfWork;

    public CartService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<HTTPResponseData<CartDTO>> GetCartByUserIdAsync(Guid userId)
    {
        CartDTO? cartDTO = _unitOfWork.CartRepository.WhereSql(p => p.UserId == userId).Select(cart => new CartDTO
        {
            UserId = userId,
            CreatedDate = DateTime.Now,
            CartItems = cart.CartItems.Select(item => new CartItemDTO
            {
                ProductVariantId = item.VariantId,
                Quantity = item.Quantity,
                Price = item.Price ?? 0,
                Image = item.Variant.Image,
                Name = item.Variant.VariantName
            }).ToList()
        }).FirstOrDefault();

        if (cartDTO == null)
        {
            return new HTTPResponseData<CartDTO>
            {
                statusCode = 200,
                Message = "Cart not found for the specified user.",
                DataResponse = new CartDTO
                {
                    UserId = userId,
                    CreatedDate = DateTime.Now,
                    CartItems = new List<CartItemDTO>()
                }
            };
        }
        return new HTTPResponseData<CartDTO>
        {
            statusCode = 200,
            Message = "Cart retrieved successfully.",
            DataResponse = cartDTO
        };

    }
}