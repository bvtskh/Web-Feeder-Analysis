using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace FeederAnalysis.DAL.DX
{
    public partial class ECOContext : DbContext
    {
        public ECOContext()
            : base("name=ECOContext")
        {
        }

        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Area> Areas { get; set; }
        public virtual DbSet<ECO_ControlSheet> ECO_ControlSheet { get; set; }
        public virtual DbSet<HistoryUpdateControlSheet> HistoryUpdateControlSheets { get; set; }
        public virtual DbSet<Model_Family> Model_Family { get; set; }
        public virtual DbSet<Purchase_Action> Purchase_Action { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<Version> Versions { get; set; }
        public virtual DbSet<WO_Relationship> WO_Relationship { get; set; }
        public virtual DbSet<WoChanging> WoChangings { get; set; }
        public virtual DbSet<WochangingInfoAdvanced> WochangingInfoAdvanceds { get; set; }
        public virtual DbSet<WoModified> WoModifieds { get; set; }
        public virtual DbSet<ECOSchedule> ECOSchedules { get; set; }
        public virtual DbSet<WoChangingHistory> WoChangingHistories { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ECO_ControlSheet>()
                .Property(e => e.History)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ECO_ControlSheet>()
                .Property(e => e.Ver_EE)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ECO_ControlSheet>()
                .Property(e => e.Ver_EA)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Model_Family>()
                .Property(e => e.Origin_Model)
                .IsUnicode(false);

            modelBuilder.Entity<WoChanging>()
                .Property(e => e.ORDER_NO)
                .IsUnicode(false);

            modelBuilder.Entity<WoChanging>()
                .Property(e => e.ECO_NO)
                .IsUnicode(false);

            modelBuilder.Entity<WochangingInfoAdvanced>()
                .Property(e => e.ECO_NO)
                .IsUnicode(false);

            modelBuilder.Entity<WoModified>()
                .Property(e => e.OldWO)
                .IsUnicode(false);

            modelBuilder.Entity<WoModified>()
                .Property(e => e.NewWO)
                .IsUnicode(false);

            modelBuilder.Entity<WoModified>()
                .Property(e => e.ECO)
                .IsUnicode(false);

            modelBuilder.Entity<WoChangingHistory>()
                .Property(e => e.ORDER_NO)
                .IsUnicode(false);

            modelBuilder.Entity<WoChangingHistory>()
                .Property(e => e.ECO_NO)
                .IsUnicode(false);
        }
    }
}
