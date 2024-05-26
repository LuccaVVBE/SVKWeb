using Svk.Shared.Misc;

namespace Svk.Shared.Users;

public interface ILoaderService
{
    Task<Result.GetItemsPaginated<LoaderDto.Index>> GetIndexAsync(UserRequest.Index request);

    Task<LoaderDto.Detail> GetDetailAsync(int loaderId);

    Task<LoaderDto.Detail> GetDetailAsync(string auth0Id);

    //subject = auth0id
    Task<int> CreateAsync(LoaderDto.Mutate model);
    Task EditAsync(int loaderId, LoaderDto.Mutate model);
    Task DeleteAsync(int loaderId);
}