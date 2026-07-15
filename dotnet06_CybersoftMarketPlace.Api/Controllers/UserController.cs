using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace dotnet06_CybersoftMarketPlace.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        [HttpGet("getAllUser")]
        public async Task<IActionResult> GetAllUser()
        {
            await Task.Delay(100);

            return Ok(new
            {
                message = "Get users"
            });
        }
    }
}