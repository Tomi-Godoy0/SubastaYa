using SubastaYa.Application.Interfaces;
using SubastaYa.Application.Interfaces.Repositories;
using SubastaYa.Application.Interfaces.Service.Audits;
using SubastaYa.Domain.Entities;

namespace SubastaYa.Application.UseCases.Audits.CreateAudit
{
    public class CreateAuditHandler : ICreateAuditHandler
    {
        private readonly IAuditLogRepository _auditRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateAuditHandler(IAuditLogRepository auditRepository, IUnitOfWork unitOfWork)
        {
            _auditRepository = auditRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<AuditLog> HandleAsync(CreateAuditCommand command)
        {
            var audit = new AuditLog
            {
                Entity = command.Entity.Trim(),
                EntityId = command.EntityId,
                Action = command.Action.Trim(),
                UserId = command.UserId,
                DetailJson = command.DetailJson,
                CreatedAt = DateTime.UtcNow
            };

            await _auditRepository.AddAsync(audit);
            await _unitOfWork.SaveChangesAsync();

            return audit;
        }
    }
}