using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace dotnet06_CybersoftMarketPlace.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("getAllUser")]
        public async Task<IActionResult> GetAllUser()
        {
            var users = await _userRepository.GetAllUsersAsync();

            return Ok(users);
        }
    }
}