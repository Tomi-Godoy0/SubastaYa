using SubastaYa.Application.Interfaces.Repositories;
using SubastaYa.Application.Interfaces.Service.Audits;
using SubastaYa.Domain.Entities;

namespace SubastaYa.Application.UseCases.Audits.GetAudits
{
    public class GetAuditHandler : IAuditLogHandler
    {
        private readonly IAuditLogRepository _auditRepository;

        public GetAuditHandler(
            IAuditLogRepository auditRepository)
        {
            _auditRepository = auditRepository;
        }

        public async Task<IEnumerable<AuditLog>> HandleAsync(
            GetAuditQuery query)
        {
            if (query.UserId.HasValue)
            {
                if (query.UserId.Value <= 0)
                    throw new ArgumentException(
                        "El usuario no es válido.");

                return await _auditRepository
                    .GetByUserIdAsync(query.UserId.Value);
            }

            return await _auditRepository.GetAllAsync();
        }
    }
}