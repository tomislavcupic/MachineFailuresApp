using System.Collections;
using MachineFailures.Application;
using MachineFailures.Domain;
using MachineFailures.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace MachineFailures.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MachinesController : ControllerBase
{
    private readonly MachineService _machineService;

    public MachinesController(MachineService machineService)
    {
        _machineService = machineService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Machine>>> GetMachines()
    {
        var machines = await _machineService.ListMachinesAsync();
        return Ok(machines);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Machine>> GetMachine(int id)
    {
        var machine = await _machineService.GetMachineAsync(id);
        if (machine == null)
            return NotFound();
        return Ok(machine);
    }

    [HttpPost]
    public async Task<ActionResult<Machine>> AddMachine(Machine machine)
    {
        var newMachine = await _machineService.AddMachineAsync(machine);
        return Ok(newMachine);
    }
}