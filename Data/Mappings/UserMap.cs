using ApiOwn.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiOwn.Data.Mappings;

public class UserMap : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Tabela
        builder.ToTable("User");
        
        // Chave primária
        builder.HasKey(x => x.Id);
        
        // Identity
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(80)
            .HasColumnName("Name")
            .HasColumnType("nvarchar");

        builder.Property(x => x.Slug)
            .IsRequired()
            .HasMaxLength(80)
            .HasColumnName("Slug")
            .HasColumnType("varchar");
        
        builder.Property(x => x.Email);
        builder.Property(x => x.PasswordHash);
        builder.Property(x => x.Instagram);
        builder.Property(x => x.Bio);
        builder.Property(x => x.Image);
        
        // indices

        builder.HasIndex(x => x.Slug, "IX_User_Slug")
            .IsUnique();

        // Relacionamento
        
        builder.HasMany(x => x.Roles)
            .WithMany(x => x.Users)
            .UsingEntity<Dictionary<string, object>>(
                "UserRole", 
                role => role
                    .HasOne<Role>()
                    .WithMany()
                    .HasForeignKey("RoleId")
                    .HasConstraintName("FK_UserRole_RoleId")
                    .OnDelete(DeleteBehavior.Cascade),
                user => user
                    .HasOne<User>()
                    .WithMany()
                    .HasForeignKey("UserId")
                    .HasConstraintName("FK_UserRole_UserId")
                    .OnDelete(DeleteBehavior.Cascade));
    }
}