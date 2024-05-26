using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;
using Svk.Client.Extensions;
using Svk.Client.Shared.Token;
using Svk.Shared.Controles;
using Svk.Shared.Misc;

namespace Svk.Client.Shared.Controles;

public class ControleService : IControleService
{
    private const string endpoint = "api/Controle";
    private readonly HttpClient client;
    private readonly TokenService tokenService;
    private readonly int MaxRetries = 3;

    public ControleService(HttpClient client, TokenService tokenService)
    {
        this.client = client;
        this.tokenService = tokenService;
    }


    private async Task AddAuthorizationHeaderAsync()
    {
        await tokenService.AddAuthorizationHeaderAsync(client);
    }

    //SHOULD ONLY BE USED ON GET CALLS!
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

    public async Task<Result.GetItemsPaginated<ControleDto.Index>> GetIndexAsync(ControleRequest.Index req)
    {
        return await RetryOnExceptionAsync(async () =>
        {
            var response =
            await client.GetFromJsonAsync<Result.GetItemsPaginated<ControleDto.Index>>(
                $"{endpoint}?{req.AsQueryString()}");
            return response!;
        });
    }

    public async Task<Result.GetItemsPaginated<ControleDto.Index>> GetByLaderAsync(int laderId,
        ControleRequest.Index req)
    {
        await AddAuthorizationHeaderAsync();
        throw new NotImplementedException();
    }

    public async Task<ControleDto.Detail> GetDetailAsync(int id)
    {
        return await RetryOnExceptionAsync(async () =>
        {
            var response = await client.GetFromJsonAsync<ControleDto.Detail>($"{endpoint}/{id}");
            return response!;
        });
    }

    public async Task<ControleResult.Create> CreateAsync(ControleDto.Create model)
    {
        await AddAuthorizationHeaderAsync();
        throw new NotImplementedException();
    }

    public async Task<ControleResult.Edit> EditAsync(int controleId, ControleDto.Edit model)
    {
        await AddAuthorizationHeaderAsync();
        var result = await client.PutAsJsonAsync($"{endpoint}/{controleId}", model);
        return await result.Content.ReadFromJsonAsync<ControleResult.Edit>() ?? throw new InvalidOperationException();
    }

}