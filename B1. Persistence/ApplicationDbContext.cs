using B1.Domain;
using Microsoft.EntityFrameworkCore; // Для DbContext и DbSet
using System.Collections.Generic; // Для ICollection
using System.Linq; // Для LINQ методов
using System.Threading.Tasks; // Для асинхронных методов
using System;

namespace B1._Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Bank> Banks { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Balance> Balances { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }
    }

}
