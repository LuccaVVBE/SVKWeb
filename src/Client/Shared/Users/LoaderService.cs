using System.Net.Http.Json;
using Svk.Client.Extensions;
using Svk.Client.Shared.Token;
using Svk.Shared.Misc;
using Svk.Shared.Users;

namespace Svk.Client.Shared.Users;

public class LoaderService : ILoaderService
{
    private readonly HttpClient client;
    private readonly TokenService tokenService;
    private const string endpoint = "api/Loader";
    private readonly int MaxRetries = 3;

    public LoaderService(HttpClient client, TokenService tokenService)
    {
        this.client = client;
        this.tokenService = tokenService;
    }

    //SHOULD ONLY BE USED ON GET CALLS!
    private async Task<T> RetryOnExceptionAsync<T>(Func<Task<T>> action)
    {
        for (int retryCount = 0; retryCount < MaxRetries; retryCount++)
        {
            try
            {
                await tokenService.AddAuthorizationHeaderAsync(client);
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


    public async Task<Result.GetItemsPaginated<LoaderDto.Index>> GetIndexAsync(UserRequest.Index request)
    {
        return await RetryOnExceptionAsync(async () =>
        {
            var response =
            await client.GetFromJsonAsync<Result.GetItemsPaginated<LoaderDto.Index>>(
                $"{endpoint}?{request.AsQueryString()}");

            return response!;
        });
    }

    public async Task<LoaderDto.Detail> GetDetailAsync(int loaderId)
    {
        return await RetryOnExceptionAsync(async () =>
        {
            var response = await client.GetFromJsonAsync<LoaderDto.Detail>($"{endpoint}/{loaderId}");

            return response!;
        });
    }

    public Task<LoaderDto.Detail> GetDetailAsync(string auth0Id)
    {
        throw new NotImplementedException();
    }

    public async Task EditAsync(int loaderId, LoaderDto.Mutate model)
    {
        
            var response = await client.PutAsJsonAsync($"{endpoint}/{loaderId}", model);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to edit loader: {response.StatusCode}");
            }
    }

    public async Task DeleteAsync(int loaderId)
    {
        await tokenService.AddAuthorizationHeaderAsync(client);
        var response = await client.DeleteAsync($"{endpoint}/{loaderId}");

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Failed to delete loader: {response.StatusCode}");
        }
    }

    public async Task<int> CreateAsync(LoaderDto.Mutate model)
    {
        await tokenService.AddAuthorizationHeaderAsync(client);
        var response = await client.PostAsJsonAsync($"{endpoint}", model);
        if (response.IsSuccessStatusCode)
        {
            var createdLoaderId = await response.Content.ReadFromJsonAsync<int>();
            return createdLoaderId;
        }
        else
        {
            throw new HttpRequestException($"Failed to create loader: {response.StatusCode}");
        }
    }
}