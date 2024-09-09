using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B1.Domain
{
    public class Bank
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Report> Reports { get; set; }
    }

    public class Report
    {
        public int Id { get; set; }
        public int BankId { get; set; }
        public DateTime ReportDate { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public Bank Bank { get; set; }
        public ICollection<Balance> Balances { get; set; }
    }

    public class Balance
    {
        public int Id { get; set; }
        public int ReportId { get; set; }
        public string AccountNumber { get; set; }
        public decimal IncomingBalance { get; set; }
        public decimal DebitTurnover { get; set; }
        public decimal CreditTurnover { get; set; }
        public decimal OutgoingBalance { get; set; }
    }

}
