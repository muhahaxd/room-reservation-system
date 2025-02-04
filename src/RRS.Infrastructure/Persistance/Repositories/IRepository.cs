using RSS.Domain.Entities;

namespace RRS.Infrastructure.Persistance.Repositories;
public interface IRepository<TEntity> : IReadOnlyRepository<TEntity>
    where TEntity : Entity
{
    Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default);
    void UpdateOne(TEntity entity);
}
