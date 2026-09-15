using SubastaYa.Application.Interfaces.DTOs;
using SubastaYa.Application.Interfaces.Repositories;
using SubastaYa.Application.Interfaces.Service.Audits;
using SubastaYa.Domain.Entities;

namespace SubastaYa.Application.UseCases.Audits.GetAudits
{
    public class GetAuditsHandler : IGetAuditsHandler
    {
        private readonly IAuditLogRepository _auditRepository;

        public GetAuditsHandler(
            IAuditLogRepository auditRepository)
        {
            _auditRepository = auditRepository;
        }

        public async Task<PagedResult<AuditResponse>> HandleAsync(GetAuditsQuery query)
        {
            List<AuditLog> audits;
            int totalCount;

            if (query.UserId.HasValue)
            {
                (audits, totalCount) =
                    await _auditRepository.GetByUserIdAsync(
                        query.UserId.Value,
                        query.PageNumber,
                        query.PageSize);
            }
            else
            {
                (audits, totalCount) =
                    await _auditRepository.GetAllAsync(
                        query.PageNumber,
                        query.PageSize);
            }

            var items = audits.Select(a => new AuditResponse
            {
                Id = a.Id,
                Entity = a.Entity,
                EntityId = a.EntityId,
                Action = a.Action,
                UserId = a.UserId,
                DetailJson = a.DetailJson,
                CreatedAt = a.CreatedAt
            }).ToList();

            return new PagedResult<AuditResponse>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize
            };
        }
    }
}