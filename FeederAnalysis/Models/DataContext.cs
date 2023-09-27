namespace FeederAnalysis.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DataContext : DbContext
    {
        public DataContext()
            : base("name=FeederConn")
        {
        }

        public virtual DbSet<FeederAlarm> FeederAlarms { get; set; }
        public virtual DbSet<LineSetting> LineSettings { get; set; }
        public virtual DbSet<LocationEntity> Locations { get; set; }
        public virtual DbSet<Tokusai_Item> TokusaiItems { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
