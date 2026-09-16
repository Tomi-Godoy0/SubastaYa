using SubastaYa.Application.Common;
using SubastaYa.Application.Interfaces.DTOs;
using SubastaYa.Application.UseCases.Audits.GetAudits;

namespace SubastaYa.Application.Interfaces.Service.Audits
{
    public interface IGetAuditsHandler
    {
        public Task<PagedResult<AuditResponse>> HandleAsync(
            GetAuditsQuery query);
    }
}