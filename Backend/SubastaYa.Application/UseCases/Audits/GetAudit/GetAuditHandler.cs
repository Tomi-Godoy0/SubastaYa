using SubastaYa.Application.Interfaces.DTOs;
using SubastaYa.Application.Interfaces.Repositories;
using SubastaYa.Application.Interfaces.Service.Audits;
using SubastaYa.Domain.Entities;
using SubastaYa.Domain.Exceptions;

namespace SubastaYa.Application.UseCases.Audits.GetAudit
{
    public class GetAuditHandler : IGetAuditHandler
    {
        private readonly IAuditLogRepository _auditRepository;

        public GetAuditHandler(
            IAuditLogRepository auditRepository)
        {
            _auditRepository = auditRepository;
        }

        public async Task<AuditResponse> HandleAsync(GetAuditQuery query)
        {
            var audit = await _auditRepository.GetByIdAsync(query.Id)
                ?? throw new NotFoundException($"Auditoría {query.Id} no encontrada.");

            return new AuditResponse
            {
                Id = audit.Id,
                Entity = audit.Entity,
                EntityId = audit.EntityId,
                Action = audit.Action,
                UserId = audit.UserId,
                DetailJson = audit.DetailJson,
                CreatedAt = audit.CreatedAt
            };
        }
    }
}