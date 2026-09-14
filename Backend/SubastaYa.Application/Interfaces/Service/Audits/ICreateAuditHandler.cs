using SubastaYa.Application.UseCases.Audits.CreateAudit;
using SubastaYa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaYa.Application.Interfaces.Service.Audits
{
    public interface ICreateAuditHandler
    {
        public Task<AuditLog> HandleAsync(CreateAuditCommand command);
    }
}
