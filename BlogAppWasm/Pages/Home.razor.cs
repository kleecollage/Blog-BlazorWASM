using BlogAppWasm.Models;
using BlogAppWasm.Services.IService;
using Microsoft.AspNetCore.Components;

namespace BlogAppWasm.Pages;

public partial class Home
{
    [Inject] private IPostService PostService { get; set; }
    private IEnumerable<Post> Posts { get; set; } = new List<Post>();

    protected override async Task OnInitializedAsync()
    {
        Posts = await PostService.GetAllPosts();
    }
}