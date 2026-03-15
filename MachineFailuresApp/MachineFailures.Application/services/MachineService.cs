using MachineFailures.Domain;
using MachineFailures.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace MachineFailures.Application;

public class MachineService
{
    private readonly MachineFailureDbContext _context;

    public MachineService(MachineFailureDbContext context)
    {
        _context = context;
    }

    public async Task<List<Machine>> ListMachinesAsync()
    {
        return await _context.Machines.ToListAsync();
    }

    public async Task<Machine?> GetMachineAsync(int id)
    {
        return await _context.Machines.FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<Machine> AddMachineAsync(Machine machine)
    {
        if (string.IsNullOrWhiteSpace(machine.Name))
            throw new ArgumentException("Machine name is required.", nameof(machine.Name));

        var exists = await _context.Machines.AnyAsync(m => m.Name == machine.Name);
        if (exists)
        {
            throw new InvalidOperationException("There is already a macjine wihh that name");
        }

        _context.Machines.Add(machine);
        await _context.SaveChangesAsync();

        return machine;
    }
}