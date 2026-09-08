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
    public class SubastaConfiguration : IEntityTypeConfiguration<Subasta>
    {
        public void Configure(EntityTypeBuilder<Subasta> entity)
        {
            entity.ToTable("Subastas");

            //Primary Key
            entity.HasKey(s => s.Id);

            //Propiedades
            entity.Property(s => s.Titulo)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(s => s.Descripcion)
                  .IsRequired()
                  .HasMaxLength(500);

            entity.Property(s => s.UrlImagen)
                  .IsRequired()
                  .HasMaxLength(200);

            entity.Property(s => s.PrecioBase)
                  .IsRequired()
                  .HasPrecision(18, 2);

            entity.Property(s => s.IncrementoMinimo)
                  .IsRequired()
                  .HasPrecision(18, 2);

            entity.Property(s => s.FechaInicio)
                  .IsRequired();

            entity.Property(s => s.FechaFin)
                  .IsRequired();

            entity.Property(s => s.Estado)
                   .IsRequired()
                   .HasMaxLength(20);

            entity.Property(s => s.Version)
                  .HasDefaultValue(1)
                  .IsConcurrencyToken();

            // ---- Relaciones ----
            //Vendedor
            entity.HasOne(s => s.Vendedor)
                  .WithMany(s => s.Subastas)
                  .HasForeignKey(s => s.VendedorId)
                  .OnDelete(DeleteBehavior.Restrict); //Si se elimina el vendedor, no se eliminan las subastas

            //Categoria
            entity.HasOne(s => s.Categoria)
                  .WithMany(s => s.Subastas)
                  .HasForeignKey(s => s.CategoriaId)
                  .OnDelete(DeleteBehavior.Restrict); //Si se elimina la categoria, no se eliminan las subastas
        }
    }
}
