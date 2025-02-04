using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using RRS.Infrastructure.Extensions;
using RRS.Infrastructure.Persistance.Repositories.MeetingRoom;
using RRS.Infrastructure.Persistance.Repositories.Reservation;
using RSS.Domain.Entities;
using System.Data.Common;

namespace RRS.Infrastructure.Persistance.Transaction;
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly ILogger<UnitOfWork> _logger;
    private readonly IMediator _mediator;

    public UnitOfWork(ApplicationDbContext applicationDbContext, ILogger<UnitOfWork> logger, IMediator mediator)
    {
        _applicationDbContext = applicationDbContext;
        _logger = logger;
        _mediator = mediator;
        MeetingRoomRepository = new MeetingRoomRepository(applicationDbContext);
        ReservationRepository = new ReservationRepository(applicationDbContext);
    }

    public IMeetingRoomRepository MeetingRoomRepository { get; init; }
    public IReservationRepository ReservationRepository { get; init; }

    public void Dispose()
    {
        _applicationDbContext.Dispose();
    }

    public async Task SubmitChangesAsync(CancellationToken cancellationToken = default)
    {
        var transaction = await _applicationDbContext.Database.BeginTransactionAsync(cancellationToken);

        var entitiesWithDomainEvents = _applicationDbContext.ChangeTracker
            .Entries<Entity>()
            .Where(entry => entry.Entity.DomainEvents.Any())
            .Select(entry => entry.Entity)
            .ToList();

        try
        {
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            await _mediator.DispatchDomainEventsAsync(entitiesWithDomainEvents, cancellationToken);

            await transaction.CommitAsync();

            foreach (var entity in entitiesWithDomainEvents)
            {
                entity.ClearDomainEvents();
            }
        }
        catch (DbException ex)
        {
            _logger.LogError(ex, "An error occured while tried to save changes.");

            await HandleRollbackAsync(transaction, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Unexpected error occured while tried to save changes.");

            await HandleRollbackAsync(transaction, cancellationToken);
        }
        finally
        {
        }
    }

    private async Task HandleRollbackAsync(IDbContextTransaction transaction, CancellationToken token = default)
    {
        await transaction.RollbackAsync(token);
    }
}
