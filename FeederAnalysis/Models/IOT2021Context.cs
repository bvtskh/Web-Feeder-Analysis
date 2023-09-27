namespace FeederAnalysis.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class IOT2021Context : DbContext
    {
        public IOT2021Context()
            : base("name=IOT2021Connection")
        {
        }

        public virtual DbSet<PDA_ErrorHistory> PDA_ErrorHistory { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PDA_ErrorHistory>()
                .Property(e => e.Model)
                .IsUnicode(false);
        }
    }
}
