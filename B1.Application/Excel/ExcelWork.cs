using B1._Persistence;
using B1.Domain;
using Microsoft.AspNetCore.Http; // Для IFormFile
using OfficeOpenXml; // Для ExcelPackage
using System.Threading.Tasks;

public class ExcelWork
{
    private readonly ApplicationDbContext _context;

    public ExcelWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task UploadExcel(IFormFile file)
    {
        using (var package = new ExcelPackage(file.OpenReadStream()))
        {
            var worksheet = package.Workbook.Worksheets[0];
            int rowCount = worksheet.Dimension.Rows;

            for (int row = 2; row <= rowCount; row++)
            {
                var balance = new Balance
                {
                    AccountNumber = worksheet.Cells[row, 1].Text,
                    IncomingBalance = decimal.Parse(worksheet.Cells[row, 2].Text),
                    DebitTurnover = decimal.Parse(worksheet.Cells[row, 3].Text),
                    CreditTurnover = decimal.Parse(worksheet.Cells[row, 4].Text),
                    OutgoingBalance = decimal.Parse(worksheet.Cells[row, 5].Text)
                };

                _context.Balances.Add(balance);
            }

            await _context.SaveChangesAsync();
        }
    }
}
