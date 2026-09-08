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
    public class TransaccionLedgerConfiguration : IEntityTypeConfiguration<TransaccionLedger>
    {
        public void Configure(EntityTypeBuilder<TransaccionLedger> entity)
        {
            entity.ToTable("TransaccionLedgers");

            // Primary Key
            entity.HasKey(t => t.Id);

            // Propiedades
            entity.Property(t => t.Tipo)
                  .IsRequired()
                  .HasMaxLength(20);

            entity.Property(t => t.Monto)
                  .IsRequired()
                  .HasPrecision(18, 2);

            entity.Property(t => t.Fecha)
                  .IsRequired();

            // ---- Relaciones ----
            entity.HasOne(t => t.Billetera)
                  .WithMany(t => t.TransaccionesLedgers)
                  .HasForeignKey(t => t.BilleteraId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(t => t.Subasta)
                  .WithMany(t => t.TransaccionesLedgers)
                  .HasForeignKey(t => t.SubastaId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
