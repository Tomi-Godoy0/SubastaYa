using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SubastaYa.Domain.Entities;

namespace SubastaYa.Infrastructure.Persistence.Configurations
{
    public class WalletConfiguration : IEntityTypeConfiguration<Wallet>
    {
        public void Configure(EntityTypeBuilder<Wallet> entity)
        {
            //Primary Key
            entity.HasKey(b => b.Id);

            //Propiedades
            entity.Property(b => b.TotalBalance)
                  .IsRequired()
                  .HasPrecision(18, 2);

            entity.Property(b => b.HeldBalance)
                  .IsRequired()
                  .HasPrecision(18, 2);

            entity.Property(b => b.Version)
                  .IsRowVersion();

            // ---- Relaciones ----
            entity.HasOne(b => b.User)
                  .WithOne(u => u.Wallet)
                  .HasForeignKey<Wallet>(b => b.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
