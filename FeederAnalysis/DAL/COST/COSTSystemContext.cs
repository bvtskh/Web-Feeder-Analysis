namespace FeederAnalysis.DAL.COST
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class COSTSystemContext : DbContext
    {
        public COSTSystemContext()
            : base("name=COSTSystemContext")
        {
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
