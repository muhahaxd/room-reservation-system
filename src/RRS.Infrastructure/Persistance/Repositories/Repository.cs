using RSS.Domain.Entities;

namespace RRS.Infrastructure.Persistance.Repositories;
public class Repository<TEntity> : ReadOnlyRepository<TEntity>, IRepository<TEntity>
    where TEntity : Entity
{
    public Repository(ApplicationDbContext readOnlyApplicationDbContext) : base(readOnlyApplicationDbContext)
    {
    }

    public async Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var createdEntity = await Entity().AddAsync(entity, cancellationToken);

        return createdEntity.Entity;
    }

    public void UpdateOne(TEntity entity)
    {
        Entity().Update(entity);
    }
}
