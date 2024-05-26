using System.Net.Http.Json;
using Svk.Client.Shared.Token;
using Svk.Shared.Files;

namespace Svk.Client.Shared.Fotos;

public class PictureService : IFileService
{
    private const string endpoint = "api/picture";
    private readonly HttpClient client;
    private readonly TokenService tokenService;

    public PictureService(HttpClient client, TokenService tokenService)
    {
        this.client = client;
        this.tokenService = tokenService;
    }

    private async Task AddAuthorizationHeaderAsync()
    {
        await tokenService.AddAuthorizationHeaderAsync(client);
    }

    public async Task<string> GetSignedUploadUrlAsync(string bucket, string fileName, int expiry = 604800)
    {
        await AddAuthorizationHeaderAsync();
        var response = await client.PostAsJsonAsync($"{endpoint}?fileName={fileName}&bucket={bucket}", new { });
        return await response.Content.ReadFromJsonAsync<string>() ?? throw new InvalidOperationException();
    }

    public async Task<string> GetSignedDownloadUrlAsync(string bucket, string fileName, int expiry = 86400)
    {
        await AddAuthorizationHeaderAsync();
        var response = await client.GetFromJsonAsync<string>($"{endpoint}?fileName={fileName}&bucket={bucket}");
        return response!;
    }

    public Task<bool> MakeBucketAsync(string bucket)
    {
        throw new NotImplementedException();
    }

    public Task<bool> RemoveBucketAsync(string bucket)
    {
        throw new NotImplementedException();
    }

    public Task<bool> BucketExistsAsync(string bucket)
    {
        throw new NotImplementedException();
    }
}