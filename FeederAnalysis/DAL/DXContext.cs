namespace FeederAnalysis.DAL
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using FeederAnalysis.Models;

    public partial class DXContext : DbContext
    {
        public DXContext()
            : base("name=DXConnection")
        {
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
