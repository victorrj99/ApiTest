using System.Data.Common;
using ApiOwn.Data;
using ApiOwn.Extensions;
using ApiOwn.Models;
using ApiOwn.Services;
using ApiOwn.ViewsModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;

namespace ApiOwn.Controllers;


[ApiController]
public class AccountController : ControllerBase
{
    
    [HttpPost("v1/account/login")] // NEW REQUEST 
    public async Task<IActionResult> Login(
        [FromBody] LoginViewModel login,
        [FromServices] NewBlogDataContext context,
        [FromServices] TokenService tokenService)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
        var user = await context
            .Users
            .AsNoTracking()
            .Include(x => x.Roles)
            .FirstOrDefaultAsync(x => x.Email == login.Email);

        if (user == null)
            return StatusCode(401, new ResultViewModel<string>("Usuário ou senha inválida"));
        
        if (!PasswordHasher.Verify(login.Password, user.PasswordHash))
            return StatusCode(401, new ResultViewModel<string>("Usuário ou senha inválida"));

        try
        {
            var token = tokenService.GenerateToken(user);
            return Ok(new ResultViewModel<string>(token, null));
        }
        catch 
        {
            return StatusCode(500, new ResultViewModel<string>("Ocorreu um erro interno"));
        }
    }

    [HttpGet("v1/account/")]
    public async Task<IActionResult> GetAccounts([FromServices] NewBlogDataContext db)
    {
        return Ok(await db.Users.ToListAsync());
    }

    [HttpPost("v1/account")]
    public async Task<IActionResult> Post(
        [FromBody] RegisterViewModel model,
        [FromServices] NewBlogDataContext context)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
    
        var password = PasswordGenerator.Generate(25 );
        var user = new User()
        {
            Email = model.Email,
            Name = model.Name,
            Slug = model.Name.ToLower().Replace("@", "-").Replace(".", "-")
            
        };
        user.PasswordHash = PasswordHasher.Hash(password);
        try
        {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            return Ok(new ResultViewModel<dynamic>(new
            {
                user = user.Email, password
            }));
        }
        catch (DbException e)
        {
            return StatusCode(400, new ResultViewModel<string>("Este email já está cadastrado"));
        }
        catch 
        {
            return StatusCode(500, new ResultViewModel<string>("Ocorreu um erro interno"));
        }
    }
}