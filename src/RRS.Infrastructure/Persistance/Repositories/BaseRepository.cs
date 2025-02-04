using Microsoft.EntityFrameworkCore;
using RSS.Domain.Entities;

namespace RRS.Infrastructure.Persistance.Repositories;
public abstract class BaseRepository<TEntity>
    where TEntity : Entity
{
    protected readonly ReadOnlyApplicationDbContext _applicationDbContext;

    protected BaseRepository(ReadOnlyApplicationDbContext readOnlyApplicationDbContext)
    {
        _applicationDbContext = readOnlyApplicationDbContext;
    }

    protected DbSet<TEntity> Entity()
    {
        var entity = _applicationDbContext.Set<TEntity>();

        return entity;
    }

    protected IQueryable<TEntity> EntityNotDeleted()
    {
        return Entity().Where(entity => entity.DeletedAt == null);
    }
}
