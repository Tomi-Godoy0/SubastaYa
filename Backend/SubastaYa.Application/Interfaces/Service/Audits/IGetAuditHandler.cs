using SubastaYa.Application.UseCases.Audits.GetAudit;
using SubastaYa.Domain.Entities;

namespace SubastaYa.Application.Interfaces.Service.Audits
{
    public interface IGetAuditHandler
    {
        Task<AuditLog?> HandleAsync(
            GetAuditQuery query);
    }
}