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
            // Configuramos las propiedades de la base de datos de la entidad TransaccionLedger

        }
    }
}
