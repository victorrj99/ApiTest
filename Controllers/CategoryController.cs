using ApiOwn.Data;
using ApiOwn.Models;
using ApiOwn.ViewsModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiOwn.Controllers;
[ApiController]

public class CategoryController : ControllerBase
{
    [HttpGet("v1/categories")]
    public async Task<IActionResult> GetAsync([FromServices] NewBlogDataContext db)
        => Ok(await db.Categories.ToListAsync());

    [HttpGet("v1/categories/{id:int}")]
    public async Task<IActionResult> GetCategoryIdAsync([FromServices] NewBlogDataContext db, [FromRoute] int id)
    {
        try
        {
            var category = await db.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
                return NotFound(category);
        
            return Ok(category);
        }
        catch (Exception e)
        {
            return StatusCode(500, $"0X584D56 - {e.Message}");
        }
    }
    
    [HttpPost("v1/categories")]
    public async Task<IActionResult> PostAsync([FromBody] EditorCategoryViewModel model, [FromServices] NewBlogDataContext db)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            var category = new Category()
            {
                Id = 0,
                Name = model.Name,
                Slug = model.Slug.ToLower(),
            };
            await db.Categories.AddAsync(category);
            await db.SaveChangesAsync();
            return Created($"v1/categories/{category.Id}", category);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut("v1/categories/{id:int}")]
    public async Task<IActionResult> PutAsync(
        [FromServices] NewBlogDataContext db, 
        [FromRoute] int id, 
        [FromBody] EditorCategoryViewModel model)
    {
        try
        {
            var category = await db.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
                return NotFound(category);
            category.Name = model.Name;
            category.Slug = model.Slug.ToLower();
            db.Categories.Update(category);
            await db.SaveChangesAsync();
            return Ok(category);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("v1/categories/{id:int}")]
    public async Task<IActionResult> DeleteAsync([FromServices] NewBlogDataContext db, [FromRoute] int id)
    {
        try
        {
            var category = await db.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
                return NotFound(category);
            db.Categories.Remove(category);
            await db.SaveChangesAsync();
            return Ok(category);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}