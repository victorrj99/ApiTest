using ApiOwn.Data;
using ApiOwn.Extensions;
using ApiOwn.Models;
using ApiOwn.Services;
using ApiOwn.ViewsModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiOwn.Controllers;


[ApiController]
public class AccountController : ControllerBase
{
    
    [HttpPost("v1/account/login")] // NEW REQUEST 
    public IActionResult Login([FromServices] TokenService tokenService)
    {
        var token = tokenService.GenerateToken(null);
        
        return Ok(token);
    }

    [HttpPost("v1/account/register")]
    public async Task<IActionResult> Post(
        [FromBody] RegisterViewModel model,
        [FromServices] NewBlogDataContext context)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
    
        var user = new User()
        {
            Email = model.Email,
            Name = model.Name,
            Slug = model.Name.ToLower().Replace("@", "-").Replace(".", "-")
        };
        
        
        return Ok(await context.Users.AddAsync(user));
    }
}