using Microsoft.AspNetCore.Mvc;
using static System.Net.WebRequestMethods;
using System.Threading.Tasks;
using Infrastructure.Repositories;
using Infrastructure.Models;
using backend_netcore_dotnet06.Helper;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
namespace dotnet06_CybersoftMarketPlace.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly JwtAuthService _jwtService;
        //Chỉ làm việc với service, không làm việc trực tiếp với repository
        public CartController(ICartService cartService, JwtAuthService jwtService)
        {
            _cartService = cartService;
            _jwtService = jwtService;
        }

        [Authorize]
        [HttpGet("GetCartByUserId")]
        public async Task<IActionResult> GetCartByUserId()
        {

            //Gọi service trả về giỏ hàng tương ứng của userid đó 
            HTTPResponseData<CartDTO>? response = await _cartService.GetCartByUserIdAsync(HttpContext.User.Identity.Name);
            return StatusCode(response.statusCode, response);
        }


    }
}