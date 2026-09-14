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

        public async Task<PagedResult<AuditResponse>> HandleAsync(
            GetAuditsQuery query)
        {
            if (query.PageNumber <= 0)
                throw new ArgumentException(
                    "El número de página debe ser mayor a cero.");

            if (query.PageSize <= 0)
                throw new ArgumentException(
                    "El tamaño de página debe ser mayor a cero.");

            IEnumerable<AuditLog> audits;
            int totalCount;

            if (query.UserId.HasValue)
            {
                if (query.UserId.Value <= 0)
                    throw new ArgumentException(
                        "El Id de usuario no es válido.");

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