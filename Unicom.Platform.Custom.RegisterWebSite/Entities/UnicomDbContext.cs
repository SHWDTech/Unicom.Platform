using System.Data.Entity;

namespace Unicom.Platform.Custom.RegisterWebSite.Entities
{
    public class UnicomDbContext : DbContext
    {
        public UnicomDbContext() : base("Unicom_Register")
        {
            
        }

        public UnicomDbContext(string connStr) : base(connStr)
        {

        }

        public virtual DbSet<EmsDevice> EmsDevices { get; set; }

        public virtual DbSet<EmsProject> EmsProjects { get; set; }

        public virtual DbSet<EmsProjectCategory> EmsProjectCategories { get; set; }

        public virtual DbSet<EmsDistrict> EmsDistricts { get; set; }

        public virtual DbSet<EmsProjectType> EmsProjectTypes { get; set; }

        public virtual DbSet<EmsProjectPeriod> EmsProjectPeriods { get; set; }

        public virtual DbSet<EmsRegion> EmsRegions { get; set; }
    }
}