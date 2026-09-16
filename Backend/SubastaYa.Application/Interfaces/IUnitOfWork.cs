namespace SubastaYa.Application.Interfaces
{
    public interface IUnitOfWork
    {
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
        Task SaveChangesAsync(CancellationToken ct = default);
        void ClearTracking();
    }
}
