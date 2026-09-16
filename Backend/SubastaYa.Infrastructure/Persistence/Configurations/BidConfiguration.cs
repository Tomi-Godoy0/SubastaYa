using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SubastaYa.Domain.Entities;

namespace SubastaYa.Infrastructure.Persistence.Configurations
{
    public class BidConfiguration : IEntityTypeConfiguration<Bid>
    {
        public void Configure(EntityTypeBuilder<Bid> entity)
        {
            //Primary key
            entity.HasKey(p => p.Id);

            //Propiedades
            entity.Property(p => p.Amount)
                  .HasPrecision(18, 2)
                  .IsRequired();

            entity.Property(p => p.CreatedAt)
                  .IsRequired();

            //----------- Relaciones ---------
            //Subasta
            entity.HasOne(p => p.Auction)
                  .WithMany(p => p.Bids) 
                  .HasForeignKey(p => p.AuctionId) 
                  .OnDelete(DeleteBehavior.Restrict);

            //Usuario
            entity.HasOne(p => p.Buyer)
                  .WithMany(p => p.Bids)
                  .HasForeignKey(p => p.BuyerId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
