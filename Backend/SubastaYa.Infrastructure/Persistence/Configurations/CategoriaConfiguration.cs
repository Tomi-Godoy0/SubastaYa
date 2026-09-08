using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SubastaYa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaYa.Infrastructure.Persistence.Configurations
{
    public class CategoriaConfiguration : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> entity)
        {
            entity.ToTable("Categorias");

            // Primary Key
            entity.HasKey(c => c.Id);

            // Configuración de propiedades
            entity.Property(c => c.Nombre)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(c => c.UrlIcono)
                  .HasMaxLength(500);
        }
    }
}
