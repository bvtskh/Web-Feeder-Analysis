using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FeederAnalysis.Models;

namespace FeederAnalysis.Business
{
    public class FeederHelper
    {
        public static List<FeederEntity> GetAllFeeder()
        {
            var sql = @"SELECT [FeederNo],[DatePlan] FROM [Feeder Maintenace].[dbo].[tb_Feeder]";
            using (var _feederContext = new FeederMaintenaceContext())
            {
                return _feederContext.Database.SqlQuery<FeederEntity>(sql, "").ToList();
            }
        }
    }
}
