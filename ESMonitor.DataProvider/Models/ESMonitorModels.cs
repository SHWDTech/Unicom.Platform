namespace ESMonitor.DataProvider.Models
{
    using System.Data.Entity;

    public class EsMonitorModels : DbContext
    {
        public EsMonitorModels()
            : base("name=ESMonitorModels")
        {
        }

        public virtual DbSet<T_Country> Country { get; set; }
        public virtual DbSet<T_Devs> Devs { get; set; }
        public virtual DbSet<T_ESDay> EsDay { get; set; }
        public virtual DbSet<T_ESHour> EsHour { get; set; }
        public virtual DbSet<T_ESMin> EsMin { get; set; }
        public virtual DbSet<T_Stats> Stats { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<T_Country>()
                .Property(e => e.Country)
                .IsFixedLength();

            modelBuilder.Entity<T_Country>()
                .HasMany(e => e.T_Stats)
                .WithRequired(e => e.T_Country)
                .HasForeignKey(e => e.Country)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<T_Devs>()
                .Property(e => e.VideoURL)
                .IsUnicode(false);

            modelBuilder.Entity<T_ESDay>()
                .Property(e => e.DataStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<T_ESDay>()
                .Property(e => e.Country)
                .IsUnicode(false);

            modelBuilder.Entity<T_ESHour>()
                .Property(e => e.DataStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<T_ESHour>()
                .Property(e => e.Country)
                .IsUnicode(false);

            modelBuilder.Entity<T_ESMin>()
                .Property(e => e.DataStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<T_ESMin>()
                .Property(e => e.Country)
                .IsUnicode(false);

            modelBuilder.Entity<T_Stats>()
                .Property(e => e.StatCode)
                .IsUnicode(false);

            modelBuilder.Entity<T_Stats>()
                .Property(e => e.StatName)
                .IsUnicode(false);

            modelBuilder.Entity<T_Stats>()
                .Property(e => e.ChargeMan)
                .IsUnicode(false);

            modelBuilder.Entity<T_Stats>()
                .Property(e => e.Telepone)
                .IsUnicode(false);

            modelBuilder.Entity<T_Stats>()
                .Property(e => e.Longitude)
                .HasPrecision(9, 6);

            modelBuilder.Entity<T_Stats>()
                .Property(e => e.Latitude)
                .HasPrecision(9, 6);

            modelBuilder.Entity<T_Stats>()
                .Property(e => e.Department)
                .IsUnicode(false);

            modelBuilder.Entity<T_Stats>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<T_Stats>()
                .Property(e => e.Street)
                .IsUnicode(false);

            modelBuilder.Entity<T_Stats>()
                .Property(e => e.ProType)
                .IsUnicode(false);
        }
    }
}
