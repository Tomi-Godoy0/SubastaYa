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
    public class BidConfiguration : IEntityTypeConfiguration<Bid>
    {
        public void Configure(EntityTypeBuilder<Bid> entity)
        {
            //Primary key
            entity.HasKey(p => p.Id);

            //Propiedades
            entity.Property(p => p.Amount)
                  .HasPrecision(18, 2) //Monto con precision de 18 digitos y 2 decimales
                  .IsRequired();

            entity.Property(p => p.CreatedAt)
                  .IsRequired();

            //----------- Relaciones ---------
            //Subasta
            entity.HasOne(p => p.Auction) //Puja tiene una subasta
                  .WithMany(p => p.Bids) //Subasta tiene muchas pujas
                  .HasForeignKey(p => p.AuctionId) //La clave foranea es SubastaId
                  .OnDelete(DeleteBehavior.Restrict);

            //Usuario
            entity.HasOne(p => p.Buyer)
                  .WithMany(p => p.Bids)
                  .HasForeignKey(p => p.BuyerId)
                  .OnDelete(DeleteBehavior.Restrict); // Definir luego si eliminamos al usuario, que pasa con las pujas. Por ahora no se puede eliminar el usuario si tiene pujas.

            //Una idea es que si se elimina un usuario, hacer que pase a Usuario Eliminado y que no pueda iniciar sesión, pero que sus pujas queden registradas. Esto es para mantener la integridad de los datos y el historial de las pujas.
        }
    }
}
