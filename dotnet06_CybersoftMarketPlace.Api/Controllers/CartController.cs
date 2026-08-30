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

        [Authorize]
        [HttpPost("AddItemToCart")]
        public async Task<IActionResult> AddItemToCart([FromBody] AddItemToCartDTO addItemToCartDTO)
        {
            int productVariantId = addItemToCartDTO.ProductVariantId;
            int quantity = addItemToCartDTO.Quantity;

            //Validate input
            if (productVariantId <= 0 || quantity <= 0)
            {
                return BadRequest(new HTTPResponseData<string>
                {
                    statusCode = 400,
                    Message = "Invalid product variant ID or quantity.",
                    DataResponse = null
                });
            }
            {

                //Gọi service để thêm item vào giỏ hàng của userid đó 
                HTTPResponseData<CartDTO>? response = await _cartService.AddItemToCartAsync(User.Identity.Name, productVariantId, quantity);
                return StatusCode(response.statusCode, response);
            }
        }
    }
}