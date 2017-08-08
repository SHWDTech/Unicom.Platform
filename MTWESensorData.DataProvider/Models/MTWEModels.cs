namespace MTWESensorData.DataProvider.Models
{
    using System.Data.Entity;

    public partial class MTWEModels : DbContext
    {
        public MTWEModels()
            : base("name=MTWEModels")
        {
        }

        public virtual DbSet<dev_info> DevInfo { get; set; }

        public virtual DbSet<sensor_data_hour> SensorDataHour { get; set; }

        public virtual DbSet<sensor_data_min> SensorDataMin { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<dev_info>()
                .Property(e => e.DEV_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<dev_info>()
                .Property(e => e.VER)
                .IsUnicode(false);

            modelBuilder.Entity<dev_info>()
                .Property(e => e.IP)
                .IsUnicode(false);

            modelBuilder.Entity<dev_info>()
                .Property(e => e.MASK)
                .IsUnicode(false);

            modelBuilder.Entity<dev_info>()
                .Property(e => e.GATEWAY)
                .IsUnicode(false);

            modelBuilder.Entity<dev_info>()
                .Property(e => e.STATE)
                .IsUnicode(false);

            modelBuilder.Entity<dev_info>()
                .Property(e => e.LAST)
                .IsUnicode(false);

            modelBuilder.Entity<sensor_data_hour>()
                .Property(e => e.StatCode)
                .IsUnicode(false);

            modelBuilder.Entity<sensor_data_hour>()
                .Property(e => e.DataStatus)
                .IsUnicode(false);

            modelBuilder.Entity<sensor_data_min>()
                .Property(e => e.StatCode)
                .IsUnicode(false);

            modelBuilder.Entity<sensor_data_min>()
                .Property(e => e.DataStatus)
                .IsUnicode(false);
        }
    }
}
