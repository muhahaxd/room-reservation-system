using Microsoft.EntityFrameworkCore;
using RSS.Domain.Entities;
using System.Linq.Expressions;

namespace RRS.Infrastructure.Persistance.Repositories;
public class ReadOnlyRepository<TEntity> : BaseRepository<TEntity>, IReadOnlyRepository<TEntity>
    where TEntity : Entity
{
    public ReadOnlyRepository(ReadOnlyApplicationDbContext readOnlyApplicationDbContext) : base(readOnlyApplicationDbContext)
    {
    }

    public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await EntityNotDeleted().CountAsync(predicate, cancellationToken);
    }

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await EntityNotDeleted().AnyAsync(predicate, cancellationToken);
    }

    public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await EntityNotDeleted().FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<TEntity?> GetByIdOrDefultAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await EntityNotDeleted().FirstOrDefaultAsync(entity => entity.Id.Equals(id), cancellationToken);
    }

    public async Task<IReadOnlyList<TEntity>> ListAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await EntityNotDeleted().Where(predicate).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<TEntity>> ListAsync<TKey>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> orderBy, CancellationToken cancellationToken = default)
    {
        return await EntityNotDeleted().Where(predicate).OrderBy(orderBy).ToListAsync(cancellationToken);
    }
}
