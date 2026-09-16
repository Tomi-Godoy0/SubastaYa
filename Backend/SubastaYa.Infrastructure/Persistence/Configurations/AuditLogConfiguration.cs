using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SubastaYa.Domain.Entities;

namespace SubastaYa.Infrastructure.Persistence.Configurations
{
    public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> entity)
        {   
            //Primary Key
            entity.HasKey(a => a.Id);

            //Propiedades
            entity.Property(a => a.Entity)
                  .IsRequired()
                  .HasMaxLength(50);

            entity.Property(a => a.EntityId)
                  .IsRequired();

            entity.Property(a => a.Action)
                  .IsRequired()
                  .HasMaxLength(20);

            entity.Property(a => a.CreatedAt)
                  .IsRequired();

            // ---- Relaciones ----
            entity.HasOne(a => a.User)
                  .WithMany(u => u.AuditLogs)
                  .HasForeignKey(a => a.UserId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
