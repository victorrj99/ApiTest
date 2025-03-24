using ApiOwn.Data.Mappings;
using ApiOwn.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiOwn.Data;

public class NewBlogDataContext : DbContext
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<User> Users { get; set; }
    


    protected override void OnConfiguring(DbContextOptionsBuilder option)
    {
        option.UseSqlServer(
            "Server=localhost,1433;Database=AppBlog;User ID=sa;Password=26971463Rj@;TrustServerCertificate=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CategoryMap());
        modelBuilder.ApplyConfiguration(new UserMap());
        modelBuilder.ApplyConfiguration(new PostMap());
    }
}