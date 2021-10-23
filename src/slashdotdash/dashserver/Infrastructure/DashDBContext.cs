using dashserver.Models.DB;
using Microsoft.EntityFrameworkCore;

namespace dashserver.Infrastructure
{
    /// <summary>
    /// Контекст БД
    /// </summary>
    public class DashDBContext : DbContext
    {
        public DbSet<KPI> KPIs { get; set; }
        public DbSet<KPIRecord> KPIRecords { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<ResourceGroup> ResourceGroups { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<PlanDay> PlanDays { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<StockBalance> StockBalances { get; set; }
        public DbSet<StockLink> StockLinks { get; set; }

        public DashDBContext(DbContextOptions<DashDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
