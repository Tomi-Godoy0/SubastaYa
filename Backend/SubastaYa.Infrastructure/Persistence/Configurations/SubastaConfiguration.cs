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
    public class SubastaConfiguration : IEntityTypeConfiguration<Subasta>
    {
        public void Configure(EntityTypeBuilder<Subasta> entity)
        {

        }
    }
}
