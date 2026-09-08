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
    public class PujaConfiguration : IEntityTypeConfiguration<Puja>
    {
        public void Configure(EntityTypeBuilder<Puja> entity)
        {
            entity.ToTable("Pujas");

            //Primary key
            entity.HasKey(p => p.Id);

            //Propiedades
            entity.Property(p => p.Monto)
                  .HasPrecision(18, 2) //Monto con precision de 18 digitos y 2 decimales
                  .IsRequired();

            entity.Property(p => p.FechaPuja)
                  .IsRequired();

            //----------- Relaciones ---------
            //Subasta
            entity.HasOne(p => p.Subasta) //Puja tiene una subasta
                  .WithMany(p => p.Pujas) //Subasta tiene muchas pujas
                  .HasForeignKey(p => p.SubastaId) //La clave foranea es SubastaId
                  .OnDelete(DeleteBehavior.Cascade); //Si se elimina la subasta, se eliminan las pujas

            //Usuario
            entity.HasOne(p => p.Comprador)
                  .WithMany(p => p.Pujas)
                  .HasForeignKey(p => p.CompradorId)
                  .OnDelete(DeleteBehavior.Restrict); // Definir luego si eliminamos al usuario, que pasa con las pujas. Por ahora no se puede eliminar el usuario si tiene pujas.

            //Una idea es que si se elimina un usuario, hacer que pase a Usuario Eliminado y que no pueda iniciar sesión, pero que sus pujas queden registradas. Esto es para mantener la integridad de los datos y el historial de las pujas.
        }
    }
}
