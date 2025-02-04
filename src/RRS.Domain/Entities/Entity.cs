using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RSS.Domain.Entities;

[PrimaryKey(nameof(Id))]
public abstract class Entity
{
    protected readonly List<INotification> _domainEvents = new List<INotification>();

    protected Entity()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;

        EntityChanged(CreatedAt);
    }

    [NotMapped]
    public IReadOnlyList<INotification> DomainEvents => _domainEvents;

    public Guid Id { get; set; }

    [Required]
    public DateTime CreatedAt { get; protected set; }
    public DateTime UpdatedAt { get; protected set; }
    public DateTime? DeletedAt { get; protected set; }

    public virtual void Delete()
    {
        DeletedAt = DateTime.UtcNow;

        EntityChanged(DeletedAt);
    }

    protected void EntityChanged(DateTime? time = null)
    {
        time ??= DateTime.UtcNow;

        UpdatedAt = time.Value;
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
