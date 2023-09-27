namespace FeederAnalysis.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class FeederMaintenaceContext : DbContext
    {
        public FeederMaintenaceContext()
            : base("name=FeederMaintenaceConn")
        {
        }

        public virtual DbSet<tb_Feeder> tb_Feeder { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tb_Feeder>()
                .Property(e => e.FeederNo)
                .IsUnicode(false);

            modelBuilder.Entity<tb_Feeder>()
                .Property(e => e.Vendor)
                .IsUnicode(false);

            modelBuilder.Entity<tb_Feeder>()
                .Property(e => e.Size)
                .IsUnicode(false);
        }
    }
}
