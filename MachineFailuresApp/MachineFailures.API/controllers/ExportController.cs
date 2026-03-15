using System.Collections;
using MachineFailures.Application;
using MachineFailures.Domain;
using MachineFailures.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace MachineFailures.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExportController : ControllerBase
{
    private readonly ExportService _exportService;

    public ExportController(ExportService exportService)
    {
        _exportService = exportService;
    }

    [HttpGet]
    public async Task<IActionResult> ExportFailures(DateTime from, DateTime to)
    {
        var fileBytes = await _exportService.ExportFailuresAsync(from, to);
        return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "failures.xlsx");
    }
}