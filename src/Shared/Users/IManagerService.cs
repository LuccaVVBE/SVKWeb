using Svk.Shared.Misc;

namespace Svk.Shared.Users;

public interface IManagerService
{
    Task<Result.GetItemsPaginated<ManagerDto.Index>> GetIndexAsync(UserRequest.Index request);
    Task<ManagerDto.Detail> GetDetailAsync(int managerId);
    Task<int> CreateAsync(ManagerDto.Mutate model);
    Task EditAsync(int managerId, ManagerDto.Mutate model);
    Task DeleteAsync(int managerId);
}