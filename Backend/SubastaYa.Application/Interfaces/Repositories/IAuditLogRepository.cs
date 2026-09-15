using SubastaYa.Domain.Entities;

namespace SubastaYa.Application.Interfaces.Repositories
{
    public interface IAuditLogRepository
    {
        Task<AuditLog> AddAsync(AuditLog auditLog);

        Task<(List<AuditLog> Audits, int TotalCount)> GetAllAsync(
            int pageNumber,
            int pageSize);

        Task<(List<AuditLog> Audits, int TotalCount)> GetByUserIdAsync(
            int userId,
            int pageNumber,
            int pageSize);

        Task<AuditLog?> GetByIdAsync(int id);
    }
}
