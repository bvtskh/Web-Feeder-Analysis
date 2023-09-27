using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FeederAnalysis.DAL.LCA
{
    public class PIContext : DbContext
    {
        static string piConnection = @"data source=172.28.10.22;initial catalog=PI_BASE;user id=sa;password=$umcevn123;MultipleActiveResultSets=True;App=EntityFramework";
        public PIContext() : base(piConnection)
        {

        }
    }
}
