using System.Text;
using ApiOwn;
using ApiOwn.Data;
using ApiOwn.Models;
using ApiOwn.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<NewBlogDataContext>();
builder.Services.AddTransient<TokenService>(); // cria um novo ( CRIA UMA NOVA INSTANCIA ) 
//builder.Services.AddScoped(); // Funciona por requesição
//builder.Services.AddSingleton(); // Singleton -> 1 por app
var key = Encoding.ASCII.GetBytes(Configuration.JwtKey);
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});




var app = builder.Build();
app.MapControllers();
app.UseAuthentication(); // Sempre vem primeiro
app.UseAuthorization(); // Sempre vem depois

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// app.MapGet("/", (string nome) =>
// {
//     return Results.Ok(nome);
// });
//
// app.MapPost("/", (User user) =>
// {
//     return Results.Ok(user);
// });

app.Run();



