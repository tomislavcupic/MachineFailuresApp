using MachineFailures.Domain;
using MachineFailures.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace MachineFailures.Application;

public class FailureService
{
    private readonly MachineFailureDbContext _context;

    public FailureService(MachineFailureDbContext context)
    {
        _context = context;
    }

    public async Task<List<Failure>> ListFailuresAsync()
    {
        return await _context.Failures.ToListAsync();
    }

    public async Task<Failure?> GetFailureAsync(int id)
    {
        return await _context.Failures.FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task<Failure> AddFailureAsync(Failure failure)
    {
        failure.StartOfFailure = failure.StartOfFailure.ToUniversalTime();
        failure.EndOfFailure = failure.EndOfFailure?.ToUniversalTime();

        if (string.IsNullOrWhiteSpace(failure.Name))
            throw new ArgumentException("Failure name is required.", nameof(failure.Name));
        if (string.IsNullOrWhiteSpace(failure.Description))
            throw new ArgumentException("Description is required.", nameof(failure.Description));
        if (failure.EndOfFailure.HasValue && failure.EndOfFailure < failure.StartOfFailure)
            throw new ArgumentException("End date cannot be before start date.", nameof(failure.EndOfFailure));

        var activeFailure = await _context.Failures
            .Where(f => f.MachineId == failure.MachineId && f.EndOfFailure == null)
            .FirstOrDefaultAsync();

        if (activeFailure != null)
            throw new InvalidOperationException("Machine already has an active failure.");

        var overlaps = await _context.Failures
            .Where(f => f.MachineId == failure.MachineId)
            .AnyAsync(f => f.StartOfFailure < (failure.EndOfFailure ?? DateTime.MaxValue) && (f.EndOfFailure ?? DateTime.MaxValue) > failure.StartOfFailure);

        if (overlaps)
            throw new InvalidOperationException("This machine already has a failure in the given time window.");

        _context.Failures.Add(failure);
        await _context.SaveChangesAsync();
        return failure;
    }

    public async Task<Failure> UpdateFailureAsync(Failure failure)
    {
        var existing = await _context.Failures.FindAsync(failure.Id);
        if (existing == null)
            throw new InvalidOperationException("Failure not found.");

        existing.Name = failure.Name;
        existing.Description = failure.Description;
        existing.StartOfFailure = failure.StartOfFailure;
        existing.EndOfFailure = failure.EndOfFailure;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task DeleteFailureAsync(int id)
    {
        var existing = await _context.Failures.FindAsync(id);
        if (existing == null)
            throw new InvalidOperationException("Failure not found.");

        _context.Failures.Remove(existing);
        await _context.SaveChangesAsync();
    }
}