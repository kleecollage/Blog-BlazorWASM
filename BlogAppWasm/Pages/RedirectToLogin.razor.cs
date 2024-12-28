using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlogAppWasm.Pages;

public partial class RedirectToLogin : ComponentBase
{
    [Inject] private NavigationManager NavigationManager { get; set; }
    [CascadingParameter] private Task<AuthenticationState> StateAuthenticationProvider { get; set; }
    private bool NoAuthorized { get; set; } = false;
      
    protected override async Task OnInitializedAsync()
    {
        var stateAuthorization = await StateAuthenticationProvider;
        if (stateAuthorization.User == null)
        {
            var returnUrl = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
            if (string.IsNullOrEmpty(returnUrl))
            {
                NavigationManager.NavigateTo("/login", true);
            }
            else
            {
                NavigationManager.NavigateTo($"Access?returnUrl={returnUrl}");
            }
        }
        else
        {
            NoAuthorized = true;
        }
    }
}