using Microsoft.EntityFrameworkCore;

namespace US_Txes_WebAPI_Core.DbModels
{
    public class CustomDbContext : DbContext
    {
        public DbSet<StateDb> States { get; set; }
        public DbSet<ZipCodeDb> ZipCodes { get; set; }
        public DbSet<FeeDb> Fees { get; set; }
        public DbSet<VehicleFeeDb> VehicleFees { get; set; }

        public CustomDbContext()
        {
        }

        public CustomDbContext(DbContextOptions<CustomDbContext> options): base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
    }
}
