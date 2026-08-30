using System.Text.Json;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using static System.Net.WebRequestMethods;

public interface ICartService
{
    Task<HTTPResponseData<CartDTO>> GetCartByUserIdAsync(string userId);
    Task<HTTPResponseData<CartDTO>> AddItemToCartAsync(string userId, int productVariantId, int quantity=1);
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

    public async Task<HTTPResponseData<CartDTO>> AddItemToCartAsync(string userId, int productVariantId, int quantity=1)
    {
        //1. Kiểm tra product variant có tồn tại hay không
        var productVariant = await _unitOfWork.ProductVariantRepository.SingleOrDefault(p => p.Id == productVariantId);
        if (productVariant == null)
        {
            return new HTTPResponseData<CartDTO>
            {
                statusCode = 404,
                Message = "Product variant not found.",
                DataResponse = null
            };
        }
        //2. Thêm item vào giỏ hàng ở đây (chưa triển khai)

        //2.1 check sản phẩm còn đủ tồn hay không 
        if (productVariant.Stock <= quantity)
        {
            return new HTTPResponseData<CartDTO>
            {
                statusCode = 400,
                Message = "Not enough stock for the requested quantity.",
                DataResponse = null
            };
        }
        //2.2 check xem user đã có giỏ hàng chưa, nếu chưa thì tạo mới nếu có rồi thì check xem sản phẩm đã có trong giỏ hàng chưa, nếu có rồi thì update quantity, nếu chưa thì thêm mới
        Cart? checkCart = await _unitOfWork.CartRepository.SingleOrDefault(c => c.UserId == Guid.Parse(userId));
        Cart cart;
        if (checkCart == null)
        {
            cart = new Cart
            {
                UserId = Guid.Parse(userId),
                CartItems = new List<CartItem>
                {
                    new CartItem
                    {
                        VariantId = productVariantId,
                        Quantity = quantity, //nếu chưa có thì thêm mới, nếu có rồi thì update quantity
                        Price = productVariant.Price,
                        ImageUrl = productVariant.Image
                    }
                }
            };
            await _unitOfWork.CartRepository.AddAsync(cart);
        }
        else
        {
            cart = checkCart;
            //Kiểm tra xem sản phẩm đã có trong giỏ hàng chưa
            var existingCartItem = await _unitOfWork.CartItemRepository.SingleOrDefault(item => item.VariantId == productVariantId && item.CartId == cart.Id);
            if (existingCartItem != null)
            {
                //Nếu có rồi thì update quantity
                existingCartItem.Quantity += quantity; //nếu có rồi thay đổi số lướng
            }
            else
            {
                //Thêm vào database 
                //Nếu chưa có thì thêm mới
                await _unitOfWork.CartItemRepository.AddAsync(new CartItem
                {
                    VariantId = productVariantId,
                    Quantity = quantity,
                    Price = productVariant.Price,
                    ImageUrl = productVariant.Image,
                    CartId = cart.Id
                });

            }
        }

        await _unitOfWork.SaveChangesAsync();

        return new HTTPResponseData<CartDTO>
        {
            statusCode = 200,
            Message = "Item added to cart successfully.",
            DataResponse = new CartDTO
            {
                UserId = userId,
                CreatedDate = DateTime.Now,
                CartItems = _unitOfWork.CartItemRepository.WhereSql(item => item.CartId == cart.Id).Select(item => new CartItemDTO
                {
                    ProductVariantId = item.VariantId,
                    Quantity = item.Quantity,
                    Price = item.Price ?? 0,
                    Image = item.Variant.Image,
                    Name = item.Variant.VariantName
                }).ToList()
            }
        };
    }

    public async Task<HTTPResponseData<CartDTO>> GetCartByUserIdAsync(string userId)
    {
        //Xây dựng tính năng load giỏ hàng từ database theo userId
        CartDTO? cartDTO = _unitOfWork.CartRepository.WhereSql(p => p.UserId == Guid.Parse(userId)).Select(cart => new CartDTO
        {
            UserId = cart.UserId.ToString(),
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