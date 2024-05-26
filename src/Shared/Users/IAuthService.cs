namespace Svk.Shared.Users;

public interface IAuthService
{
    Task<string> CreateLoaderAsync(LoaderDto.Mutate model);

    Task<string> CreateManagerAsync(ManagerDto.Mutate model);

    Task DeleteUserAsync(string auth0Id);
}