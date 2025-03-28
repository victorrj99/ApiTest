﻿using ApiOwn.Data;
using ApiOwn.Extensions;
using ApiOwn.Models;
using ApiOwn.ViewsModels;
using ApiOwn.ViewsModels.Categories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace ApiOwn.Controllers;
[ApiController]

public class CategoryController : ControllerBase
{
    [HttpGet("v1/categories")]
    public IActionResult GetAsync(
        [FromServices] IMemoryCache cache,
        [FromServices] NewBlogDataContext db)
    {
        try
        {
            var categories =  cache.GetOrCreate("CacheCategories", entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
                return GetCategories(db);
            });
            
            return Ok( new ResultViewModel<List<Category>>(categories));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<List<Category>>("Falha interna no servidor"));
        }
    }

    private List<Category> GetCategories(NewBlogDataContext context)
    {
        return context.Categories.ToList();
    }

    [HttpGet("v1/categories/{id:int}")]
    public async Task<IActionResult> GetCategoryIdAsync([FromServices] NewBlogDataContext db, [FromRoute] int id)
    {
        User.IsInRole("Admin");
        try
        {
            var category = await db.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
                return NotFound(new ResultViewModel<Category>("Conteúdo não encontrado"));
        
            return Ok(new ResultViewModel<Category>(category));
        }
        catch
        {
            return StatusCode(500, @$"0X584D56 - 
                {new ResultViewModel<List<Category>>("Falha interna no servidor")}");
        }
    }
    
    [HttpPost("v1/categories")]
    public async Task<IActionResult> PostAsync([FromBody] EditorCategoryViewModel model, [FromServices] NewBlogDataContext db)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));
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