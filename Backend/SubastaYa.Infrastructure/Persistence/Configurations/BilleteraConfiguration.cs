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
    public class BilleteraConfiguration : IEntityTypeConfiguration<Billetera>
    {
        public void Configure(EntityTypeBuilder<Billetera> entity)
        {
            entity.ToTable("Billeteras");

            //Primary Key
            entity.HasKey(b => b.Id);

            //Propiedades
            entity.Property(b => b.SaldoTotal)
                  .IsRequired()
                  .HasPrecision(18, 2);

            entity.Property(b => b.SaldoRetenido)
                  .IsRequired()
                  .HasPrecision(18, 2);

            entity.Property(b => b.SaldoDisponible)
                  .IsRequired()
                  .HasPrecision(18, 2);

            entity.Property(b => b.Version)
                  .HasDefaultValue(1)
                  .IsConcurrencyToken();

            // ---- Relaciones ----
            entity.HasOne(b => b.Usuario)
                  .WithOne(u => u.Billetera)
                  .HasForeignKey<Billetera>(b => b.UsuarioId)
                  .OnDelete(DeleteBehavior.Cascade);




        }
    }
}
