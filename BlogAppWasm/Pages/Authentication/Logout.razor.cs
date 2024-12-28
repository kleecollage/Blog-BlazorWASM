using BlogAppWasm.Services;
using Microsoft.AspNetCore.Components;

namespace BlogAppWasm.Pages.Authentication;

public partial class Logout : ComponentBase
{
    [Inject] public IAuthenticationService AuthenticationService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await AuthenticationService.Logout();
        NavigationManager.NavigateTo("/");
    }
}