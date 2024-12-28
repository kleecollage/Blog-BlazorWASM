using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Nodes;
using Blazored.LocalStorage;
using BlogAppWasm.Helpers;
using BlogAppWasm.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BlogAppWasm.Services;

public class AuthenticationService(
    HttpClient client, 
    ILocalStorageService localStorage, 
    AuthenticationStateProvider authenticationStateProvider) : IAuthenticationService
{
    public async Task<RegisterResponse> RegisterUser(UserRegister userToRegister)
    {
        var content = JsonConvert.SerializeObject(userToRegister);
        var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
        var response = await client.PostAsync($"{Initializer.UrlBase}/api/users/register", bodyContent);
        var contentTemp = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<RegisterResponse>(contentTemp);

        if (response.IsSuccessStatusCode) 
            return new RegisterResponse { Success = true };

        return result;
    }

    public async Task<AuthenticationResponse> Login(UserAuthentication userFromAuthentication)
    {
        var content = JsonConvert.SerializeObject(userFromAuthentication);
        var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
        var response = await client.PostAsync($"{Initializer.UrlBase}/api/users/login", bodyContent);
        var contentTemp = await response.Content.ReadAsStringAsync();
        var result = (JObject)JsonConvert.DeserializeObject(contentTemp);

        if (response.IsSuccessStatusCode)
        {
            var token = result["result"]["token"].Value<string>();
            var user = result["result"]["user"]["username"].Value<string>();
            await localStorage.SetItemAsync(Initializer.LocalToken, token);
            await localStorage.SetItemAsync(Initializer.DataLocalUser, user);
            ((AuthStateProvider)authenticationStateProvider).NotifyUserLogged(token);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            // OK :)
            return new AuthenticationResponse { Success = true };
        }
        
        return new AuthenticationResponse { Success = false };
    }

    public async Task Logout()
    {
        await localStorage.RemoveItemAsync(Initializer.LocalToken);
        await localStorage.RemoveItemAsync(Initializer.DataLocalUser);
        ((AuthStateProvider)authenticationStateProvider).NotifyUserLogOut();
        client.DefaultRequestHeaders.Authorization = null;
    }
}








