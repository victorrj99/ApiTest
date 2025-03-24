using ApiOwn.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiOwn.Data.Mappings;

public class CategoryMap : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        //Tabela
        builder.ToTable("Category");
        //chave primaria
        builder.HasKey(x => x.Id);
        
        // identity
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn(); 
        
        //propriedades
        builder.Property(x => x.Name)
            .IsRequired() // torna o campo obrigatório
            .HasColumnName("Name") // mome da coluna
            .HasColumnType("nvarchar") // tipo de dados
            .HasMaxLength(80); // máximo de caracteres
        
        builder.Property(x => x.Slug)
            .IsRequired() // torna o campo obrigatório
            .HasColumnName("Slug") // mome da coluna
            .HasColumnType("varchar") // tipo de dados
            .HasMaxLength(80); // máximo de caracteres
        
        // indices 
        builder.HasIndex(x => x.Slug, "IX_Category_Slug")
            .IsUnique(); // Torna o indice em único, não poderá ter duas com o mesmo indice do slug

    }
}