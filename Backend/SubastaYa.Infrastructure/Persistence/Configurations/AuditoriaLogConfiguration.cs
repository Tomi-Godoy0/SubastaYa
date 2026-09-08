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
    public class AuditoriaLogConfiguration : IEntityTypeConfiguration<AuditoriaLog>
    {
        public void Configure(EntityTypeBuilder<AuditoriaLog> entity)
        {
            entity.ToTable("AuditoriaLogs");
            
            //Primary Key
            entity.HasKey(a => a.Id);

            //Propiedades
            entity.Property(a => a.Entidad)
                  .IsRequired()
                  .HasMaxLength(50);

            entity.Property(a => a.EntidadId)
                  .IsRequired();

            entity.Property(a => a.Accion)
                  .IsRequired()
                  .HasMaxLength(20);

            entity.Property(a => a.DetallesJson)
                  .IsRequired();

            entity.Property(a => a.Fecha)
                  .IsRequired();

            // ---- Relaciones ----
            entity.HasOne(a => a.Usuario)
                  .WithMany(u => u.Auditorias)
                  .HasForeignKey(a => a.UsuarioId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
