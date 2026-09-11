using Microsoft.EntityFrameworkCore.Storage;
using SubastaYa.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaYa.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            if (_transaction == null) throw new InvalidOperationException("No hay ninguna transacción activa");

            await _transaction.CommitAsync();

            _transaction = null;
        }

        public async Task RollbackAsync()
        {
            if (_transaction == null) throw new InvalidOperationException("No hay ninguna transacción activa");

            await _transaction.RollbackAsync();

            _transaction = null;
        }

        public Task SaveChangesAsync(CancellationToken ct = default) => _context.SaveChangesAsync(ct);
    }
}
