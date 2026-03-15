using System.Collections;
using MachineFailures.Application;
using MachineFailures.Domain;
using MachineFailures.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace MachineFailures.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FailuresController : ControllerBase
{
    private readonly FailureService _failureService;

    public FailuresController(FailureService failureService)
    {
        _failureService = failureService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Failure>>> GetFailures()
    {
        var failures = await _failureService.ListFailuresAsync();
        return Ok(failures);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Failure>> GetFailure(int id)
    {
        var failure = await _failureService.GetFailureAsync(id);
        if (failure == null)
            return NotFound();
        return Ok(failure);
    }

    [HttpPost]
    public async Task<ActionResult<Failure>> AddFailure(Failure failure)
    {
        var newFailure = await _failureService.AddFailureAsync(failure);
        return Ok(newFailure);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Failure>> UpdateFailure(int id, Failure failure)
    {
        if (id != failure.Id)
            return BadRequest("Route id does not match failure id.");

        var updated = await _failureService.UpdateFailureAsync(failure);
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteFailure(int id)
    {
        await _failureService.DeleteFailureAsync(id);
        return Ok();
    }
}