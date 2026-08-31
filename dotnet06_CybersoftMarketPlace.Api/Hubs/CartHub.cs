//signalr hub
using System.Text.Json;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

public class CartHub : Hub
{
    private readonly ICartService _cartService;

    public CartHub(ICartService cartService)
    {
        _cartService = cartService;
    }

    //connect to hub
    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
        Console.WriteLine($@"Đã kết nối hub thành công ws server!");

    }

    //Lấy userId từ claim "name" của token (xem NameClaimType trong Program.cs)
    private string GetUserId()
    {
        string? userId = Context.User?.Identity?.Name;

        if (string.IsNullOrEmpty(userId))
        {
            throw new HubException("Không đọc được userId từ token.");
        }

        return userId;
    }

    //api hub get all gio hang
    [Authorize]
    public async Task HandleGetCartByUserId()
    {
        Console.WriteLine($@"Gọi được hàm Get cart by userid!");

        //Lấy userId từ claim của token
        string userId = GetUserId();

        Console.WriteLine($@"Đã kết nối hub thành công ws server! - userId: {userId}");

        //Gọi service trả về giỏ hàng tương ứng của userid đó
        HTTPResponseData<CartDTO>? response = await _cartService.GetCartByUserIdAsync(userId);

        //Gửi dữ liệu giỏ hàng về cho client
        await Clients.User(userId).SendAsync("GetCartByUserId", response.DataResponse);
    }


    // [Authorize]
    // public async Task ChangeQuantityVariantInCart(int productVariantId, int newQuantity)
    // {
    //     //Lấy userId từ claim của token
    //     string userId = GetUserId();

    //     //Gọi service để thay đổi số lượng sản phẩm trong giỏ hàng
    //     HTTPResponseData<CartDTO>? response = await _cartService.ChangeQuantityVariantInCartAsync(userId, productVariantId, newQuantity);

    //     Console.WriteLine($@"{JsonSerializer.Serialize(response.Message)} - ketqua");
    //     //Gửi dữ liệu giỏ hàng về cho client
    //     await Clients.User(userId).SendAsync("GetCartByUserId", response.DataResponse);
    // }
    
    [Authorize]
    public async Task HandleAddItemToCart(int productVariantId, int quantity)
    {
        Console.WriteLine($@"hub server add to cart");
        //Lấy userId từ claim của token
        string userId = GetUserId();

        //Gọi service để thêm item vào giỏ hàng của userid đó 
        HTTPResponseData<CartDTO>? response = await _cartService.AddItemToCartAsync(userId, productVariantId, quantity);

        Console.WriteLine($@"{JsonSerializer.Serialize(response.Message)} - ketqua");
        //Gửi dữ liệu giỏ hàng về cho client
        await Clients.User(userId).SendAsync("GetCartByUserId", response.DataResponse);
    }





    //disconnect from hub
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);

    }
}