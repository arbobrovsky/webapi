using Microsoft.EntityFrameworkCore;
using webapi.Data.Models;

namespace webapi.Data
{
    public class EFDBContext : DbContext
    {
        public EFDBContext(DbContextOptions<EFDBContext> options) : base(options) {
            Database.EnsureCreated();
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Currency> Currencies {get;set;}
        public DbSet<CurrencyChange> CurrencyChanges {get;set;}

    }
}
