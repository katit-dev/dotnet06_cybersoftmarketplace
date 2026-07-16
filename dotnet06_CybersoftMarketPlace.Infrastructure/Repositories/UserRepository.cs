// Nơi định nghĩa các phương thức truy xuất dữ liệu liên quan đến User

using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public interface IUserRepository
{
    Task<List<User>> GetAllUsersAsync();
}

public class UserRepository : IUserRepository
{
    private readonly CybersoftMarketPlaceContext _context;

    public UserRepository(CybersoftMarketPlaceContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }
}