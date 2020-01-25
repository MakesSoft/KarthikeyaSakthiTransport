using Microsoft.EntityFrameworkCore;

namespace KarthikeyasakthiTransport.Model
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public DbSet<Registration> Registration { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Unit> Unit { get; set; }
        public DbSet<Group> Group { get; set; }
        public DbSet<SubGroup> SubGroup { get; set; }
        public DbSet<ItemMaster> ItemMaster { get; set; }
        public DbSet<MenuMaster> MenuMaster { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<LedgerMaster> LedgerMaster { get; set; }
        public DbSet<SalesBill> SalesBill { get; set; }
        public DbSet<SalesBillItems> SalesBillItems { get; set; }
    }
}