namespace ESMonitor.DataProvider.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class EsMonitorModels : DbContext
    {
        public EsMonitorModels()
            : base("name=ESMonitorModels")
        {
        }

        public virtual DbSet<T_Country> T_Country { get; set; }
        public virtual DbSet<T_Devs> T_Devs { get; set; }
        public virtual DbSet<EsDay> EsDay { get; set; }
        public virtual DbSet<EsHour> EsHour { get; set; }
        public virtual DbSet<EsMin> EsMin { get; set; }
        public virtual DbSet<T_Stats> T_Stats { get; set; }

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

            modelBuilder.Entity<EsDay>()
                .Property(e => e.DataStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<EsDay>()
                .Property(e => e.Country)
                .IsUnicode(false);

            modelBuilder.Entity<EsHour>()
                .Property(e => e.DataStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<EsHour>()
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
