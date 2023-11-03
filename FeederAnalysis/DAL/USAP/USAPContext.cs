namespace FeederAnalysis.DAL.USAP
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class USAPContext : DbContext
    {
        public USAPContext()
            : base("name=USAPContext")
        {
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
