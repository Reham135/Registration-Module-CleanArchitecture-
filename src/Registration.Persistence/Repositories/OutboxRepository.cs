using Microsoft.EntityFrameworkCore;
using Registration.Application.Common.Interfaces;
using Registration.Application.Common.Models;

namespace Registration.Persistence.Repositories;

public class OutboxRepository : IOutboxRepository
{
    private readonly ApplicationDbContext _context;

    public OutboxRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<List<OutboxMessage>> GetUnprocessedMessagesAsync(int batchSize, CancellationToken cancellationToken)
    {
        return _context.OutboxMessages
            .Where(m => m.ProcessedOnUtc == null)
            .OrderBy(m => m.OccurredOnUtc)
            .Take(batchSize)
            .ToListAsync(cancellationToken);
    }

    public async Task MarkAsProcessedAsync(Guid id, DateTime processedOnUtc, CancellationToken cancellationToken)
    {
        var message = await _context.OutboxMessages.FirstAsync(m => m.Id == id, cancellationToken);
        message.MarkAsProcessed(processedOnUtc);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task MarkAsFailedAsync(Guid id, string error, CancellationToken cancellationToken)
    {
        var message = await _context.OutboxMessages.FirstAsync(m => m.Id == id, cancellationToken);
        message.MarkAsFailed(error);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
