
public interface IUserService
{
    public Task RegisterUserAsync(UserRegisterDTO model);
}

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Task RegisterUserAsync(UserRegisterDTO model)
    {
        throw new NotImplementedException();
    }
}