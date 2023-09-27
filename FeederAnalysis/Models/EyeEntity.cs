using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeederAnalysis.Models
{
    public class EyeEntity
    {
        public string StaffCode { get; set; }
        public string FullName { get; set; }
        public string PosName { get; set; }
        public string DeptCode { get; set; }
        public string CUS_Name { get; set; }
        public string CapDo { get; set; }
        public string NangCap { get; set; }
        public DateTime? NgayThi { get; set; }
        public string DateExp
        {
            get
            {
                if (NgayThi == null)
                {
                    return string.Empty;
                }
                else
                {
                    var date = Convert.ToDateTime(NgayThi);
                    return date.ToString("yyyy-MM-dd");
                }
            }
        }
    }
}
