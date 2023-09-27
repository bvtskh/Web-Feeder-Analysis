namespace FeederAnalysis.DAL.LCA
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class LCAHumanContext : DbContext
    {
        public LCAHumanContext()
            : base("name=HumanConnection")
        {
        }

        public virtual DbSet<Tbl_Mankichi> Tbl_Mankichi { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Tbl_Mankichi>()
                .Property(e => e.Dept)
                .IsUnicode(false);

            modelBuilder.Entity<Tbl_Mankichi>()
                .Property(e => e.Position)
                .IsUnicode(false);

            modelBuilder.Entity<Tbl_Mankichi>()
                .Property(e => e.TinhCode)
                .IsUnicode(false);

            modelBuilder.Entity<Tbl_Mankichi>()
                .Property(e => e.HuyenCode)
                .IsUnicode(false);

            modelBuilder.Entity<Tbl_Mankichi>()
                .Property(e => e.XaCode)
                .IsUnicode(false);
        }
    }
}
