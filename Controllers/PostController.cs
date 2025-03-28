using ApiOwn.Data;
using ApiOwn.ViewsModels.Posts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiOwn.Controllers;

[ApiController]
public class PostController : ControllerBase
{
    [HttpGet("v1/posts")]
    public async Task<IActionResult> GetPostsAsync(
        [FromServices] NewBlogDataContext db)
    {
        var posts = await db
            .Posts
            .Include(x => x.Category)
            .Include(x => x.Author)
            .Select(x => new ListPostViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Slug = x.Slug,
                LastUpdateDate = x.LastUpdateDate,
                Category = x.Category.Name,
                Author = x.Author.Name
            })
            .Skip(0 * 25)
            .Take(25)
            .OrderByDescending(x => x.LastUpdateDate)
            .ToListAsync();
        
        return Ok(posts);
    }
    
    
    
}