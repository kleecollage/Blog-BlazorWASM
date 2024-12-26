using ApiBlogApp.Models;

namespace ApiBlogApp.Repository.IRepository;

public interface IPostRepository
{
    ICollection<Post> GetAllPosts();
    Post GetPostById(int id);
}