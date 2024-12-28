using BlogAppWasm.Models;
using BlogAppWasm.Services;
using Microsoft.AspNetCore.Components;

namespace BlogAppWasm.Pages.Authentication;

public partial class Register : ComponentBase
{
    [Inject] public IAuthenticationService AuthenticationService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }
    
    private UserRegister UserToRegister = new();
    public bool OnProcess { get; set; } = false;
    public bool RegisterErrors { get; set; }
    public IEnumerable<string> Errors { get; set; } = new List<string>();

    private async Task RegisterUser()
    {
        RegisterErrors = true;
        OnProcess = true;
        var result = await AuthenticationService.RegisterUser(UserToRegister);
        if (result.Success)
        {
            OnProcess = false;
            NavigationManager.NavigateTo("/access");
        }
        else
        {
            OnProcess = false;
            Errors = result.Errors;
            RegisterErrors = true;
        }
    }
}














