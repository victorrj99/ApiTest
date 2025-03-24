using ApiOwn.Data;
using ApiOwn.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<NewBlogDataContext>();


var app = builder.Build();
app.MapControllers();

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



