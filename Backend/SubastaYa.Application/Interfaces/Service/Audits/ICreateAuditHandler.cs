using SubastaYa.Application.UseCases.Audits.CreateAudit;
using SubastaYa.Domain.Entities;

namespace SubastaYa.Application.Interfaces.Service.Audits
{
    public interface ICreateAuditHandler
    {
        public Task<AuditLog> HandleAsync(CreateAuditCommand command);
    }
}
