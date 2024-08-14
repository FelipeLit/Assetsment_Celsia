using assetsment_Celsia.Models;
using Microsoft.EntityFrameworkCore;

namespace assetsment_Celsia.Data
{
    public class CelsiaContext : DbContext
    {
        public CelsiaContext(DbContextOptions<CelsiaContext> options) : base(options)
        {

        }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Platform> Platforms { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Administrator> Administrators { get; set; }

    }
}