namespace FeederAnalysis.DAL.UMES
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class UmesContext : DbContext
    {
        public UmesContext()
            : base("name=UmesConnection")
        {
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

        }
    }
}
