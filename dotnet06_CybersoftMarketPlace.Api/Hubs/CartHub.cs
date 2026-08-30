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



    //disconnect from hub
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);

    }



}
