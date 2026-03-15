using ClosedXML.Excel;
using MachineFailures.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace MachineFailures.Application;

public class ExportService
{
    private readonly MachineFailureDbContext _context;

    public ExportService(MachineFailureDbContext context)
    {
        _context = context;
    }

    public async Task<byte[]> ExportFailuresAsync(DateTime fromDate, DateTime toDate)
    {
        fromDate = fromDate.ToUniversalTime();
        toDate = toDate.ToUniversalTime();
        
        var failures =
            from f in _context.Failures
            join m in _context.Machines on f.MachineId equals m.Id
            where f.StartOfFailure >= fromDate && f.StartOfFailure <= toDate
            select new
            {
                MachineName = m.Name,
                FailureName = f.Name,
                StartOfFailure = f.StartOfFailure,
                EndOfFailure = f.EndOfFailure
            };

        var data = await failures.ToListAsync();

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Failures");

        worksheet.Cell(1, 1).Value = "Machine";
        worksheet.Cell(1, 2).Value = "Failure";
        worksheet.Cell(1, 3).Value = "Start";
        worksheet.Cell(1, 4).Value = "End";

        int row = 2;

        foreach (var f in data)
        {
            worksheet.Cell(row, 1).Value = f.MachineName;
            worksheet.Cell(row, 2).Value = f.FailureName;
            worksheet.Cell(row, 3).Value = f.StartOfFailure;
            worksheet.Cell(row, 4).Value = f.EndOfFailure;
            row++;
        }

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);

        return stream.ToArray();
    }
}