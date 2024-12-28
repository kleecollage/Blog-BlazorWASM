using System.Web;
using BlogAppWasm.Models;
using BlogAppWasm.Services;
using Microsoft.AspNetCore.Components;

namespace BlogAppWasm.Pages.Authentication;

public partial class Login : ComponentBase
{
    [Inject] public IAuthenticationService AuthenticationService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }
    
    private UserAuthentication UserAuthentication = new();
    public bool OnProcess { get; set; } = false;
    public bool AuthenticationErrors { get; set; }
    public string Errors { get; set; }
    
    public string UrlReturn { get; set; }

    private async Task UserAccess()
    {
        AuthenticationErrors = true;
        OnProcess = true;
        var result = await AuthenticationService.Login(UserAuthentication);
        if (result.Success)
        {
            OnProcess = false;
            var urlAbsolute = new Uri(NavigationManager.Uri);
            var queryParameters = HttpUtility.ParseQueryString(urlAbsolute.Query);
            UrlReturn = queryParameters["returnUrl"];
            if (string.IsNullOrEmpty(UrlReturn))
            {
                NavigationManager.NavigateTo("/");
            }
            else
            {
                NavigationManager.NavigateTo("/" + UrlReturn);
            }
        }
        else
        {
            OnProcess = false;
            AuthenticationErrors = true;
            Errors = "Invalid username or password.";
            NavigationManager.NavigateTo("/login");
        }
    }
}