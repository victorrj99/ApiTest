using System.Data.Common;
using System.Text.RegularExpressions;
using ApiOwn.Data;
using ApiOwn.Extensions;
using ApiOwn.Models;
using ApiOwn.Services;
using ApiOwn.ViewsModels;
using ApiOwn.ViewsModels.Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
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
        [FromServices] EmailService emailService,
        [FromServices] NewBlogDataContext context)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
    
        var password = PasswordGenerator.Generate(25 );
        var user = new User()
        {
            Email = model.Email,
            Name = model.Name,
            Slug = model.Email.ToLower().Replace("@", "-").Replace(".", "-").Replace(" ", "-")
            
        };
        user.PasswordHash = PasswordHasher.Hash(password);
        
        try
        {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            emailService.Send(user.Name, user.Email, "Bem vindo ao BM",
                $"Olá seu usuário é {user.Name}, e sua senha é {password}" );
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
    
    [HttpDelete("v1/account/{id}")]
    public async Task<IActionResult> Delete(
        [FromRoute] int id,
        [FromServices] NewBlogDataContext context)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
        
        var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (user == null)
            return StatusCode(404, new ResultViewModel<string>("Usuário não encontrado"));
        
        try
        {
            
            context.Users.Remove(user);
            await context.SaveChangesAsync();
            return Ok(new ResultViewModel<string>("Usuário removido com sucesso"));
        }
        catch (DbException e)
        {
            return StatusCode(400, new ResultViewModel<string>("usuário não cadastrado"));
        }
        catch 
        {
            return StatusCode(500, new ResultViewModel<string>("Ocorreu um erro interno"));
        }
    }

    [Authorize]
    [HttpPost("v1/account/upload-image")]
    public async Task<IActionResult> UploadImage(
        [FromBody] UploadImageViewModel model,
        [FromServices] NewBlogDataContext context)
    {
        var fileName = $"{Guid.NewGuid().ToString()}.jpg";
        var data = new Regex(@"^data:image/(.*);base64,")
            .Replace(model.Base64Image, "");
        var bytes = Convert.FromBase64String(data);

        try
        {
            await System.IO.File.WriteAllBytesAsync($"wwwroot/images/{fileName}", bytes);
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("0x864Y8 - Ocorreu um erro interno"));
        }

        var user = await context
            .Users
            .FirstOrDefaultAsync(x => x.Email == User.Identity.Name);
        if (user == null)
            return NotFound( new ResultViewModel<User>("Usuário não encontrado"));
        
        user.Image = $"https://localhost:0000/images/{fileName}";

        try
        {
            context.Users.Update(user);
            await context.SaveChangesAsync();
            return Ok(new ResultViewModel<string>("Imagem atualizada com sucesso", null));
        }
        catch (Exception e)
        {
            return StatusCode(500, new ResultViewModel<string>("5050XY4A - Ocorreu um erro interno"));
        }
    }
}