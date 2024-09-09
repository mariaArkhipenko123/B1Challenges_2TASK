using B1._Persistence;
using B1.Domain;
using Microsoft.AspNetCore.Http; // Для IFormFile
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Для DbContext и DbSet
using System.Collections.Generic; // Для ICollection
using System.Linq; // Для LINQ методов
using System.Text; // Для StringBuilder
using System.Threading.Tasks; // Для асинхронных методов

namespace B1.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ExcelWork _excelWork;

        public ReportsController(ApplicationDbContext context, ExcelWork excelWork)
        {
            _context = context;
            _excelWork = excelWork;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] IFormFile file)
        {
            await _excelWork.UploadExcel(file);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetReports()
        {
            var reports = await _context.Reports.Include(r => r.Bank).ToListAsync();
            return Ok(reports);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReportDetails(int id)
        {
            var balances = await _context.Balances.Where(b => b.ReportId == id).ToListAsync();
            if (balances == null || !balances.Any())
            {
                return NotFound(); // Возвращаем 404, если не найдены балансы
            }
            return Ok(balances);
        }

        [HttpGet("view")]
        public async Task<IActionResult> ViewReports()
        {
            var reports = await _context.Reports
                .Include(r => r.Balances) // Загружаем связанные балансы
                .Include(r => r.Bank) // Загружаем связанные банки
                .ToListAsync();

            var html = GenerateHtmlTable(reports);

            return Content(html, "text/html");
        }

        private string GenerateHtmlTable(IEnumerable<Report> reports)
        {
            var sb = new StringBuilder();
            sb.Append("<table border='1'>");
            sb.Append("<tr><th>Bank</th><th>Report Date</th><th>Balances</th></tr>");

            foreach (var report in reports)
            {
                sb.Append($"<tr><td>{report.Bank?.Name}</td><td>{report.ReportDate.ToShortDateString()}</td><td>");

                foreach (var balance in report.Balances)
                {
                    sb.Append($"<div>Account: {balance.AccountNumber}, Incoming: {balance.IncomingBalance}, Debit: {balance.DebitTurnover}, Credit: {balance.CreditTurnover}, Outgoing: {balance.OutgoingBalance}</div>");
                }

                sb.Append("</td></tr>");
            }

            sb.Append("</table>");
            return sb.ToString();
        }
    }
}
