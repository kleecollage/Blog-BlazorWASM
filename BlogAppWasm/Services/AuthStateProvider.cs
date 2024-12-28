using System.Net.Http.Headers;
using System.Security.Claims;
using Blazored.LocalStorage;
using BlogAppWasm.Helpers;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlogAppWasm.Services;

public class AuthStateProvider(HttpClient client, ILocalStorageService localStorageService): AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await localStorageService.GetItemAsync<string>(Initializer.LocalToken);
        if (token == null) 
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

        return new AuthenticationState(
            new ClaimsPrincipal(new ClaimsIdentity(
                JwtParser.ParseClaimsFromJwt(token), "jwtAuthType")));
    }
}