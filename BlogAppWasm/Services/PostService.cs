using System.Text;
using BlogAppWasm.Helpers;
using BlogAppWasm.Models;
using BlogAppWasm.Services.IService;
using Newtonsoft.Json;

namespace BlogAppWasm.Services;

public class PostService(HttpClient client) : IPostService
{
    public async Task<IEnumerable<Post>> GetAllPosts()
    {
        var response = await client.GetAsync($"{Initializer.UrlBase}/api/posts");
        var content = await response.Content.ReadAsStringAsync();
        var posts = JsonConvert.DeserializeObject<IEnumerable<Post>>(content);

        return posts;
    }

    public async Task<Post> GetPostById(int postId)
    {
        var response = await client.GetAsync($"{Initializer.UrlBase}/api/posts/{postId}");
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var post = JsonConvert.DeserializeObject<Post>(content);
            
            return post;
        }
        else
        {
            var content = await response.Content.ReadAsStringAsync();
            var errorModel = JsonConvert.DeserializeObject<ErrorModel>(content);
            throw new Exception(errorModel.ErrorMessage);
        }
    }

    public async Task<Post> CreatePost(Post post)
    {
        var content = JsonConvert.SerializeObject(post);
        var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
        var response = await client.PostAsync($"{Initializer.UrlBase}/api/posts", bodyContent);
        
        if (response.IsSuccessStatusCode)
        {
            var contentTemp = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Post>(contentTemp);
            
            return result;
        }
        else
        {
            var contentTemp = await response.Content.ReadAsStringAsync();
            var errorModel = JsonConvert.DeserializeObject<ErrorModel>(contentTemp);
            throw new Exception(errorModel.ErrorMessage);
        }
    }

    public async Task<Post> UpdatePost(int postId, Post post)
    {
        var content = JsonConvert.SerializeObject(post);
        var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
        var response = await client.PostAsync($"{Initializer.UrlBase}/api/posts/{postId}", bodyContent);
        
        if (response.IsSuccessStatusCode)
        {
            var contentTemp = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Post>(contentTemp);
            
            return result;
        }
        else
        {
            var contentTemp = await response.Content.ReadAsStringAsync();
            var errorModel = JsonConvert.DeserializeObject<ErrorModel>(contentTemp);
            throw new Exception(errorModel.ErrorMessage);
        }
    }

    public async Task<bool> DeletePost(int postId)
    {
        var response = await client.GetAsync($"{Initializer.UrlBase}/api/posts/{postId}");
        if (response.IsSuccessStatusCode) return true;
        
        var content = await response.Content.ReadAsStringAsync();
        var errorModel = JsonConvert.DeserializeObject<ErrorModel>(content);
        throw new Exception(errorModel.ErrorMessage);
        }

    public async Task<string> UploadImage(MultipartFormDataContent content)
    {
        var postResult = await client.PostAsync($"{Initializer.UrlBase}/api/upload", content);
        var postContent = await postResult.Content.ReadAsStringAsync();
        if (!postResult.IsSuccessStatusCode)
            throw new ApplicationException(postContent);

        var postImage = Path.Combine($"{Initializer.UrlBase}", postContent);
        return postImage;
    }
}