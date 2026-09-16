using SubastaYa.Application.Interfaces.DTOs;
using SubastaYa.Application.UseCases.Audits.GetAudit;

namespace SubastaYa.Application.Interfaces.Service.Audits
{
    public interface IGetAuditHandler
    {
        public Task<AuditResponse> HandleAsync(GetAuditQuery query);
    }
}