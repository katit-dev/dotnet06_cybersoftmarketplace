
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
    public Task RegisterUserAsync(UserRegisterDTO model)
    {
        throw new NotImplementedException();
    }
}