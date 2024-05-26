using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace Svk.Client.Shared.Token;

public class TokenService
{
    private readonly IAccessTokenProvider _accessTokenProvider;
    private string _accessToken;

    public TokenService(IAccessTokenProvider accessTokenProvider)
    {
        _accessTokenProvider = accessTokenProvider;
    }

    public async Task<string> GetAccessTokenAsync()
    {
        var tokenResult = await _accessTokenProvider.RequestAccessToken();

        if (tokenResult.TryGetToken(out var token))
        {
            _accessToken = token.Value;
        }

        return _accessToken;
    }

    public async Task AddAuthorizationHeaderAsync(HttpClient client)
    {
        var accessToken = await GetAccessTokenAsync();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
    }
}