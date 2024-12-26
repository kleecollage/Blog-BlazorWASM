using ApiBlogApp.Models;

namespace ApiBlogApp.Repository.IRepository;

public interface IPostRepository
{
    ICollection<Post> GetAllPosts();
    Post GetPostById(int id);
    bool PostExists(string title);
    bool PostExists(int id);
    bool CreatePost(Post post);
    bool UpdatePost(Post post);
    bool DeletePost(Post post);
    bool Save();
}