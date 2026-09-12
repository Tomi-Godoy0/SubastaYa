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

        public async Task<IEnumerable<AuditLog>> GetAllAsync()
        {
            return await _context.AuditLogs
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetByUserIdAsync(int userId)
        {
            return await _context.AuditLogs
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task<AuditLog?> GetByIdAsync(int id)
        {
            return await _context.AuditLogs
                .FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}