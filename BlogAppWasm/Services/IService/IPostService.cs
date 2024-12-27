using BlogAppWasm.Models;

namespace BlogAppWasm.Services.IService;

public interface IPostService
{
    public Task<IEnumerable<Post>> GetAllPosts();
    public Task<Post> GetPostById(int postId);
    public Task<Post> CreatePost(Post post);
}