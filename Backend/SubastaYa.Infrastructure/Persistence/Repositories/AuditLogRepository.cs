using Microsoft.EntityFrameworkCore;
using SubastaYa.Application.Interfaces.Repositories;
using SubastaYa.Domain.Entities;

namespace SubastaYa.Infrastructure.Persistence.Repositories
{
    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly AppDbContext _context;

        public AuditLogRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AuditLog> AddAsync(AuditLog auditLog)
        {
            await _context.AuditLogs.AddAsync(auditLog);

            return auditLog;
        }

        public async Task<(IEnumerable<AuditLog> Audits, int TotalCount)> GetAllAsync(
            int pageNumber,
            int pageSize)
        {
            var query = _context.AuditLogs
                .AsNoTracking()
                .OrderByDescending(a => a.CreatedAt);

            var totalCount = await query.CountAsync();

            var audits = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (audits, totalCount);
        }

        public async Task<(IEnumerable<AuditLog> Audits, int TotalCount)> GetByUserIdAsync(
            int userId,
            int pageNumber,
            int pageSize)
        {
            var query = _context.AuditLogs
                .AsNoTracking()
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.CreatedAt);

            var totalCount = await query.CountAsync();

            var audits = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (audits, totalCount);
        }

        public async Task<AuditLog?> GetByIdAsync(int id)
        {
            return await _context.AuditLogs
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}

