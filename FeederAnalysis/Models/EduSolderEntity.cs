using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeederAnalysis.Models
{
    public class ReportsSolder
    {
        public string StaffCode { get; set; }
        public string FullName { get; set; }
        public string PosName { get; set; }
        public string DeptCode { get; set; }
        public string CUS_Name { get; set; }
        public string CapDoHan { get; set; }
        public string NangCapDo { get; set; }
        public DateTime? NgayThiXacNhan { get; set; }
        public string DateExp
        {
            get
            {
                if (NgayThiXacNhan == null)
                {
                    return string.Empty;
                }
                else
                {
                    var date = Convert.ToDateTime(NgayThiXacNhan);
                    return date.ToString("yyyy-MM-dd");
                }
            }
        }
    }
}
