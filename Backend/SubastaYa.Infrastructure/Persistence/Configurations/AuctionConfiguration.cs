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
    public class AuctionConfiguration : IEntityTypeConfiguration<Auction>
    {
        public void Configure(EntityTypeBuilder<Auction> entity)
        {
            //Primary Key
            entity.HasKey(s => s.Id);

            //Propiedades
            entity.Property(s => s.Title)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(s => s.Description)
                  .IsRequired()
                  .HasMaxLength(500);

            entity.Property(s => s.ImageUrl)
                  .IsRequired()
                  .HasMaxLength(200);

            entity.Property(s => s.BasePrice)
                  .IsRequired()
                  .HasPrecision(18, 2);

            entity.Property(s => s.CurrentBidAmount)
                  .IsRequired()
                  .HasPrecision(18, 2);

            entity.Property(s => s.MinimumIncrement)
                  .IsRequired()
                  .HasPrecision(18, 2);

            entity.Property(s => s.StartDate)
                  .IsRequired();

            entity.Property(s => s.EndDate)
                  .IsRequired();

            entity.Property(s => s.Status)
                   .IsRequired()
                   .HasMaxLength(20);

            entity.Property(s => s.Version)
                  .IsRowVersion();

            // ---- Relaciones ----
            //Vendedor
            entity.HasOne(s => s.Seller)
                  .WithMany(s => s.Auctions)
                  .HasForeignKey(s => s.SellerId)
                  .OnDelete(DeleteBehavior.Restrict); //Si se elimina el vendedor, no se eliminan las subastas

            //Categoria
            entity.HasOne(s => s.Category)
                  .WithMany(s => s.Auctions)
                  .HasForeignKey(s => s.CategoryId)
                  .OnDelete(DeleteBehavior.Restrict); //Si se elimina la categoria, no se eliminan las subastas
        }
    }
}
