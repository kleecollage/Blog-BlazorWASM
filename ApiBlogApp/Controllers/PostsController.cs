using ApiBlogApp.Models;
using ApiBlogApp.Models.Dtos;
using ApiBlogApp.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ApiBlogApp.Controllers;

[Route("api/posts")]
[ApiController]
public class PostsController(IPostRepository postRepo, IMapper mapper) : ControllerBase
{
    // GET: Retrieve all posts [host/api/posts]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetPosts()
    {
        var listPosts = postRepo.GetAllPosts();
        var listPostsDto = new List<PostDto>();

        foreach (var post in listPosts)
        {
            listPostsDto.Add(mapper.Map<PostDto>(post));
        }
        
        return Ok(listPostsDto);
    }
    
    // GET: Retrieve a single post by its ID [host/api/posts/{postId}]
    [HttpGet("{postId:int}", Name = "GetPost")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetPost(int postId)
    {
        var itemPost = postRepo.GetPostById(postId);

        if (itemPost == null) return NotFound();
        
        var itemPostDto = mapper.Map<PostDto>(itemPost);
        
        return Ok(itemPostDto);
    }    
    
    // POST: Create a new post [host/api/posts]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(201, Type = typeof(PostCreateDto))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult CreatePost([FromBody] PostCreateDto postCreateDto)
    {
        if (!ModelState.IsValid) return BadRequest();
        if (postCreateDto == null) return BadRequest(ModelState);
        
        if (postRepo.PostExists(postCreateDto.Title))
        {
            ModelState.AddModelError(
                "Title",
                "Post with this title already exists"
            );
            return StatusCode(404, ModelState);  
        }
        
        var post = mapper.Map<Post>(postCreateDto);
        if (!postRepo.CreatePost(post))
        {
            ModelState.AddModelError("", $"Something went wrong saving post{post.Title}");
            return StatusCode(500, ModelState);
        }
        return CreatedAtRoute("GetPost", new { postId = post.Id }, post);
    }
    
    // PATCH: Partially update an existing post [host/api/posts/{postId}]
    [HttpPatch("{postId:int}", Name = "UpdatePatchPost")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(201, Type = typeof(PostUpdateDto))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult UpdatePatchPost(int postId, [FromBody] PostUpdateDto postUpdateDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        if (postUpdateDto == null || postId != postUpdateDto.Id) return BadRequest(ModelState);

        if (postRepo.PostExists(postUpdateDto.Title))
        {
            ModelState.AddModelError("Title", "Post with this title already exists");
            return StatusCode(404, ModelState);
        }
        
        var post = mapper.Map<Post>(postUpdateDto);
        if (!postRepo.UpdatePost(post))
        {
            ModelState.AddModelError("", $"Something went wrong updating post{post.Title}");
            return StatusCode(500, ModelState);
        }
        
        return NoContent();
    }
    
    // DELETE: Remove a post by its ID [host/api/posts/{postId}]
    [HttpDelete("{postId:int}", Name = "DeletePost")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult DeletePost(int postId)
    {
        if (!postRepo.PostExists(postId)) return NotFound();
        
        var post = postRepo.GetPostById(postId);
        if (!postRepo.DeletePost(post))
        {
            ModelState.AddModelError("", $"Something went wrong deleting post{post.Title}");
            return StatusCode(500, ModelState);
        }
        
        return NoContent();
    }
}











