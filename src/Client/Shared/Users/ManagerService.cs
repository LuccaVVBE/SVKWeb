using System.Net.Http.Json;
using Svk.Client.Extensions;
using Svk.Client.Shared.Token;
using Svk.Shared.Misc;
using Svk.Shared.Users;

namespace Svk.Client.Shared.Users;

public class ManagerService : IManagerService
{
    private readonly HttpClient client;
    private readonly TokenService tokenService;
    private const string endpoint = "api/Manager";
    private readonly int MaxRetries = 3;

    public ManagerService(HttpClient client, TokenService tokenService)
    {
        this.client = client;
        this.tokenService = tokenService;
    }

    private async Task AddAuthorizationHeaderAsync()
    {
        await tokenService.AddAuthorizationHeaderAsync(client);
    }

    private async Task<T> RetryOnExceptionAsync<T>(Func<Task<T>> action)
    {
        for (int retryCount = 0; retryCount < MaxRetries; retryCount++)
        {
            try
            {
                await AddAuthorizationHeaderAsync();
                return await action();
            }
            catch (HttpRequestException)
            {
                Thread.Sleep(1500);
            }
            catch (Exception)
            {

                throw; // Rethrow unhandled exceptions
            }
        }

        throw new InvalidOperationException($"Failed after {MaxRetries} retries.");
    }

    public async Task<Result.GetItemsPaginated<ManagerDto.Index>> GetIndexAsync(UserRequest.Index request)
    {
        return await RetryOnExceptionAsync(async () =>
        {
            var response =
            await client.GetFromJsonAsync<Result.GetItemsPaginated<ManagerDto.Index>>(
                $"{endpoint}?{request.AsQueryString()}");

            return response!;
        });
    }

    public async Task<ManagerDto.Detail> GetDetailAsync(int managerId)
    {
        return await RetryOnExceptionAsync(async () =>
        {
            var response = await client.GetFromJsonAsync<ManagerDto.Detail>($"{endpoint}/{managerId}");

            return response!;
        });
    }

    public async Task<int> CreateAsync(ManagerDto.Mutate model)
    {
        await AddAuthorizationHeaderAsync();
        var response = await client.PostAsJsonAsync(endpoint, model);
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Failed to create manager: {response.StatusCode}");
        }

        var managerId = await response.Content.ReadFromJsonAsync<int>();
        return managerId;
    }

    public async Task EditAsync(int managerId, ManagerDto.Mutate model)
    {
        await AddAuthorizationHeaderAsync();
        var response = await client.PutAsJsonAsync($"{endpoint}/{managerId}", model);
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Failed to edit manager: {response.StatusCode}");
        }
    }

    public async Task DeleteAsync(int managerId)
    {
        await AddAuthorizationHeaderAsync();
        var response = await client.DeleteAsync($"{endpoint}/{managerId}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Failed to delete manager: {response.StatusCode}");
        }
    }
}