using System.Net.Http.Headers;
using System.Net.Http.Json;
using Svk.Client.Shared.Token;
using Svk.Shared.Users;

namespace Svk.Client.Shared.Users
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient client;
        private readonly TokenService tokenService;
        private readonly string endpoint = "https://devops8.eu.auth0.com/";

        public AuthService(HttpClient client, TokenService tokenService)
        {
            this.client = client;
            this.tokenService = tokenService;
        }

        private Task AddAuthorizationHeaderAsync()
        {
            return tokenService.AddAuthorizationHeaderAsync(client);
        }

        private async Task<string?> GetBearerTokenAsync()
        {
            var response = await client.PostAsJsonAsync(endpoint + "oauth/token", new
            {
                client_id = "4FNUPF3eid3ct8cPKYd2BDD0CbY3sYgk",
                client_secret = "VSGxV5yWcTOlfB0O_BVljMRE-Nxt1e1SB_yP_tOQu5tbjx8JIeA1j48hB6ZRGrCE",
                audience = "https://devops8.eu.auth0.com/api/v2/",
                grant_type = "client_credentials"
            });

            response.EnsureSuccessStatusCode();

            var auth0Response = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>();
            return auth0Response!["access_token"].ToString();
        }

        public async Task<string> CreateLoaderAsync(LoaderDto.Mutate model)
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", await GetBearerTokenAsync());
            var password = Guid.NewGuid().ToString().Substring(0, 9);


            var response = await client.PostAsJsonAsync(endpoint + "api/v2/users", new
            {
                name = model.Name, 
                model.Email,
                password,
                connection = "Username-Password-Authentication"
            });

            response.EnsureSuccessStatusCode();

            var auth0Response = await response.Content.ReadFromJsonAsync<Auth0Response>();
            model.Auth0Id = auth0Response!.user_id.Split('|')[1];

            await AddAuthorizationHeaderAsync();
            await client.PostAsJsonAsync("api/loader/", model);

            return password;
        }

        public async Task<string> CreateManagerAsync(ManagerDto.Mutate model)
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", await GetBearerTokenAsync());
            var password = Guid.NewGuid().ToString().Substring(0, 9);


            var response = await client.PostAsJsonAsync(endpoint + "api/v2/users", new
            {
				name = model.Name,
				model.Email,
                password,
                connection = "Username-Password-Authentication"
            });

            response.EnsureSuccessStatusCode();



            var auth0Response = await response.Content.ReadFromJsonAsync<Auth0Response>();
            model.Auth0Id = auth0Response!.user_id.Split('|')[1];


            //Add Manager role to auth0 user
            response = await client.PostAsJsonAsync(endpoint + "api/v2/roles/rol_d7rWahYsIuSyl98n/users", new
            {
                users = new string[]{ auth0Response.user_id}
            });

            response.EnsureSuccessStatusCode();

            await AddAuthorizationHeaderAsync();
            await client.PostAsJsonAsync("api/manager/", model);

            return password;
        }

        public async Task DeleteUserAsync(string auth0Id)
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", await GetBearerTokenAsync());
            var response = await client.DeleteAsync(endpoint + $"api/v2/users/{auth0Id}");
            response.EnsureSuccessStatusCode();
        }


        private class Auth0Response
        {
            public string user_id { get; set; }
            public bool email_verified { get; set; }
            public string email { get; set; }
        }
    }
}