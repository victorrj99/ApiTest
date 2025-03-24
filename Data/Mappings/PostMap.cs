using ApiOwn.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiOwn.Data.Mappings;

public class PostMap : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        // Tabela
        builder.ToTable("Post");
        
        // Chave Primária
        builder.HasKey(x => x.Id);
        
        // Identity

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();
        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(160)
            .HasColumnName("Title")
            .HasColumnType("varchar");
        builder.Property(x => x.Slug)
            .IsRequired()
            .HasMaxLength(80)
            .HasColumnName("Slug")
            .HasColumnType("nvarchar");
        builder.Property(x => x.LastUpdateDate)
            .IsRequired()
            .HasColumnName("LastUpdateDate")
            .HasColumnType("smalldatetime")
            .HasDefaultValueSql("getdate()")
            //.HasDefaultValue(DateTime.Now.ToUniversalTime());
            .HasMaxLength(60);

        builder.Property(x => x.Body);
        builder.Property(x => x.Summary);
        
        
        // Indices 
        builder.HasIndex(x => x.Slug, "IX_Post_Slug")
            .IsUnique();
        
        // Relacionamentos
        builder.HasOne(x => x.Author)
            .WithMany(x => x.Posts)
            .HasConstraintName("FK_Post_Author")
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(x => x.Category)
            .WithMany(x => x.Posts)
            .HasConstraintName("FK_Post_Category")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Tags)
            .WithMany(x => x.Posts)
            .UsingEntity<Dictionary<string, object>>(
                    "PostTag", 
                    post => post
                        .HasOne<Tag>()
                        .WithMany()
                        .HasForeignKey("PostId")
                        .HasConstraintName("FK_PostTag_PostId")
                        .OnDelete(DeleteBehavior.Cascade),
                    tag => tag
                        .HasOne<Post>()
                        .WithMany()
                        .HasForeignKey("TagId")
                        .HasConstraintName("FK_PostTag_TagId")
                        .OnDelete(DeleteBehavior.Cascade));


    }
}