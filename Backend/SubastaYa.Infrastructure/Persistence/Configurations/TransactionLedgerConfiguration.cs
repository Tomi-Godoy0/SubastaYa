using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SubastaYa.Domain.Entities;

namespace SubastaYa.Infrastructure.Persistence.Configurations
{
    public class TransactionLedgerConfiguration : IEntityTypeConfiguration<TransactionLedger>
    {
        public void Configure(EntityTypeBuilder<TransactionLedger> entity)
        {
            // Primary Key
            entity.HasKey(t => t.Id);

            // Propiedades
            entity.Property(t => t.Type)
                  .IsRequired()
                  .HasMaxLength(20);

            entity.Property(t => t.Amount)
                  .IsRequired()
                  .HasPrecision(18, 2);

            entity.Property(t => t.CreatedAt)
                  .IsRequired();

            // ---- Relaciones ----
            entity.HasOne(t => t.Wallet)
                  .WithMany(t => t.TransactionLedgers)
                  .HasForeignKey(t => t.WalletId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(t => t.Auction)
                  .WithMany(t => t.TransactionLedgers)
                  .HasForeignKey(t => t.AuctionId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
