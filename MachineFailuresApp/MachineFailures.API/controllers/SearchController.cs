using MachineFailures.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/search")]
public class SearchController : ControllerBase
{
    private readonly MachineFailureDbContext _context;

    public SearchController(MachineFailureDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Search(
        string? name,
        DateTime? from,
        DateTime? to,
        int categoryId)
    {
        to = to?.ToUniversalTime();
        from = from?.ToUniversalTime();
        var query =
            from f in _context.Failures
            join m in _context.Machines on f.MachineId equals m.Id
            join c in _context.Categories on m.CategoryId equals c.Id
            select new
            {
                MachineName = m.Name,
                CategoryName = c.Name,
                FailureName = f.Name,
                StartOfFailure = f.StartOfFailure,
                EndOfFailure = f.EndOfFailure,
                CategoryId = c.Id
            };

        if (!string.IsNullOrWhiteSpace(name))
        {
            query = query.Where(x => x.MachineName.Contains(name));
        }

        if (categoryId > 0)
        {
            query = query.Where(x => x.CategoryId == categoryId);
        }

        if (from.HasValue)
        {
            query = query.Where(x => x.StartOfFailure >= from.Value);
        }

        if (to.HasValue)
        {
            query = query.Where(x => x.StartOfFailure <= to.Value);
        }

        var results = await query.ToListAsync();

        return Ok(results);
    }
}