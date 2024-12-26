using ApiBlogApp.Data;
using ApiBlogApp.Models;
using ApiBlogApp.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace ApiBlogApp.Repository;

public class PostRepository(ApplicationDbContext context): IPostRepository
{
    public ICollection<Post> GetAllPosts()
    {
        return context.Posts.OrderBy(p => p.Id).ToList();
    }

    public Post GetPostById(int id)
    {
        return context.Posts.FirstOrDefault(p => p.Id == id);
    }

    public bool PostExists(string title)
    {
        bool value = context.Posts.Any(p => p.Title.ToLower().Trim() == title.ToLower().Trim());
        return value;
    }

    public bool PostExists(int id)
    {
        return context.Posts.Any(p => p.Id == id);
    }

    public bool CreatePost(Post post)
    {
        post.CreatedAt = DateTime.Now;
        context.Posts.Add(post);
        return Save();
    }

    public bool UpdatePost(Post post)
    {
        post.UpdatedAt = DateTime.Now;
        var imageFromDb = context.Posts.AsNoTracking().FirstOrDefault(p => p.Id == post.Id);

        if (post.PathImage == null)
            post.PathImage = imageFromDb.PathImage;
        
        context.Posts.Update(post);
        return Save();
    }

    public bool DeletePost(Post post)
    {
        context.Posts.Remove(post);
        return Save();
    }
    public bool Save()
    {
        return context.SaveChanges() >= 0;
    }
}