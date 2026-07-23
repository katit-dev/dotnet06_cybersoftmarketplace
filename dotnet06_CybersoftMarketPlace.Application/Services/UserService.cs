
using backend_netcore_dotnet06.Helper;
using Infrastructure.Models;
using Infrastructure.Repositories;

public interface IUserService
{
    public Task RegisterUserAsync(UserRegisterDTO model);
}

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IUserRoleRepository _userRoleRepository;

    // Trong tầng service sẽ gọi các repository để xử lý
    public UserService(
        IUnitOfWork unitOfWork,
        IUserRepository userRepository,
        IUserRoleRepository userRoleRepository)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _userRoleRepository = userRoleRepository;
    }
    public async Task RegisterUserAsync(UserRegisterDTO model)
    {
        try
        {
            // 1/ insert vào bảng user
            User userModel = new User
            {
                Id = Guid.NewGuid(),
                Username = model.Username,
                FullName = model.FullName,
                Alias = HelperFunction.StringToSlug(model.FullName),
                Email = model.Email,
                Phone = model.Phone,
                Avatar = @$"https://ui-avatars.com/api/?name={model.FullName}&background=random&size=128",
                PasswordHash = HelperFunction.HashPassword(model.Password),
                Address = model.Address,
                CreatedAt = DateTime.Now,
                Deleted = false,
            };

            await _userRepository.AddAsync(userModel);

            // 2/ insert vào bảng user_role

            // Add liên bảng userrole
            UserRole userRoleModel = new UserRole
            {
                UserId = userModel.Id,
                RoleId = UserRoleConst.User, // Mặc định khi đăng ký sẽ là role user
            };

            userModel.UserRoles.Add(userRoleModel);

            // Sau khi thêm userrole từ usermodel thì SaveChangesAsync
            // sẽ tự động thêm vào bảng UserRole
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
        }


    }

}
