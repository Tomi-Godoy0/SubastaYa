using SubastaYa.Application.Interfaces.Repositories;
using SubastaYa.Application.Interfaces.Service.Audits;
using SubastaYa.Domain.Entities;

namespace SubastaYa.Application.UseCases.Audits.CreateAudit
{
    public class CreateAuditHandler : ICreateAuditHandler
    {
        private readonly IAuditLogRepository _auditRepository;

        public CreateAuditHandler(
            IAuditLogRepository auditRepository)
        {
            _auditRepository = auditRepository;
        }

        public async Task<AuditLog> HandleAsync(
            CreateAuditCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.Entity))
                throw new ArgumentException(
                    "La entidad es obligatoria.");

            if (command.EntityId <= 0)
                throw new ArgumentException(
                    "El EntityId no es válido.");

            if (string.IsNullOrWhiteSpace(command.Action))
                throw new ArgumentException(
                    "La acción es obligatoria.");

            if (command.CreatedAt == default)
                command.CreatedAt = DateTime.UtcNow;

            var audit = new AuditLog
            {
                Entity = command.Entity.Trim(),
                EntityId = command.EntityId,
                Action = command.Action.Trim(),
                UserId = command.UserId,
                DetailJson = command.DetailJson,
                CreatedAt = command.CreatedAt
            };

            return await _auditRepository.AddAsync(audit);
        }
    }
}